#region Using Statements
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using IboshEngine.Runtime.Utilities.Singleton;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
#endregion

namespace IboshEngine.Runtime.Core.AudioManagement
{
    /// <summary>
    /// Manages audio playback and settings for both music and sound effects in the game.
    /// Implements the Singleton pattern to ensure only one instance exists.
    /// </summary>
    public class AudioManager : IboshSingleton<AudioManager>
    {
        private const float mixBufferClearDelay = 0.05f;
        private static readonly List<string> mixBuffer = new();

        [FoldoutGroup("Music Properties")]
        [ReadOnly]
        [SerializeField]
        private float musicVolume;

        [FoldoutGroup("Music Properties")]
        [SerializeField]
        private AudioSource musicAudioSource;

        [FoldoutGroup("Music Properties")]
        [SerializeField]
        private float defaultFadeDuration = 1f;

        [FoldoutGroup("SFX Properties")]
        [ReadOnly]
        [SerializeField]
        private float sfxVolume;

        [FoldoutGroup("SFX Properties")]
        [SerializeField]
        private AudioSource sfxAudioSource;

        private AudioSO _currentMusic;
        private List<AudioSO> _audios = new();
        private Sequence _currentFadeSequence;

        #region Built-In

        protected async override void Awake()
        {
            base.Awake();

            _audios = Resources.LoadAll<AudioSO>("Audios").ToList();

            await UniTask.Delay(100);

            sfxVolume = PlayerPrefs.GetInt("SFX", 1);
            sfxAudioSource.volume = sfxVolume;

            musicVolume = PlayerPrefs.GetInt("Music", 1);
            musicAudioSource.volume = musicVolume;

            StartCoroutine(MixBufferRoutine());
        }

        #endregion

        #region Audio Management

        public async UniTask PlayMusicWithFade(string clip, float fadeDuration = .5f)
        {
            if (fadeDuration < 0)
                fadeDuration = defaultFadeDuration;

            AudioSO audioSO = GetSoundByName(clip);

            // Kill any existing fade sequence
            _currentFadeSequence?.Kill();

            // Create new fade sequence
            _currentFadeSequence = DOTween.Sequence();

            // Fade out current music if playing
            if (musicAudioSource.isPlaying)
            {
                await musicAudioSource.DOFade(0f, fadeDuration).ToUniTask();
                musicAudioSource.Stop();
            }

            // Set and play new music
            _currentMusic = audioSO;
            musicAudioSource.clip = audioSO.Clip;
            musicAudioSource.volume = 0f;
            musicAudioSource.Play();

            // Fade in new music
            await musicAudioSource.DOFade(musicVolume, fadeDuration).ToUniTask();
        }

        public async UniTask StopMusicWithFade(float fadeDuration = .5f)
        {
            if (fadeDuration < 0)
                fadeDuration = defaultFadeDuration;

            if (!musicAudioSource.isPlaying) return;

            // Kill any existing fade sequence
            _currentFadeSequence?.Kill();

            // Create new fade sequence
            _currentFadeSequence = DOTween.Sequence();

            // Fade out current music
            await musicAudioSource.DOFade(0f, fadeDuration).ToUniTask();
            musicAudioSource.Stop();
            _currentMusic = null;
        }

        public async void PlayMusic(string clip, float fadeDuration = .5f)
        {
            if (_currentMusic != null && _currentMusic.Name == clip) return;
            await StopMusicWithFade(fadeDuration);
            await PlayMusicWithFade(clip, fadeDuration);
        }

        public void PlayMusicInstant(string clip)
        {
            AudioSO audioSO = GetSoundByName(clip);
            if (_currentMusic == audioSO) return;

            if (musicAudioSource.isPlaying) musicAudioSource.Stop();
            _currentMusic = audioSO;

            musicAudioSource.clip = audioSO.Clip;
            musicAudioSource.volume = musicVolume;
            musicAudioSource.Play();
        }

        /// <summary>
        /// Retrieves an AudioSO asset by its name from the loaded audio collection.
        /// </summary>
        /// <param name="audioName">The name of the audio to find</param>
        /// <returns>The matching AudioSO or null if not found</returns>
        private AudioSO GetSoundByName(string audioName)
        {
            return _audios.Find(x => x.Name == audioName);
        }

        /// <summary>
        /// Coroutine that periodically clears the mix buffer to prevent audio overlap.
        /// </summary>
        private IEnumerator MixBufferRoutine()
        {
            float time = 0;

            while (true)
            {
                time += Time.unscaledDeltaTime;
                yield return 0;
                if (time >= mixBufferClearDelay)
                {
                    mixBuffer.Clear();
                    time = 0;
                }
            }
        }

        /// <summary>
        /// Stops the currently playing sound effect.
        /// </summary>
        public void StopSound()
        {
            sfxAudioSource.Stop();
        }

        /// <summary>
        /// Plays a sound effect by its clip name.
        /// </summary>
        /// <param name="clip">The name of the audio clip to play</param>
        public void PlaySound(string clip)
        {
            AudioSO audioSO = GetSoundByName(clip);

            if (audioSO == null || mixBuffer.Contains(clip)) return;

            if (audioSO.Clip == null) return;

            sfxAudioSource.pitch = audioSO.HasRandomPitch ? Random.Range(.8f, 1.2f) : 1f;
            sfxAudioSource.PlayOneShot(audioSO.Clip);
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = volume;
            sfxAudioSource.volume = sfxVolume;
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = volume;
            musicAudioSource.volume = musicVolume;
        }

        #endregion
    }
}