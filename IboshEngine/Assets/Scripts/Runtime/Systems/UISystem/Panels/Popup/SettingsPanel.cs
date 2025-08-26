using Sirenix.OdinInspector;
using IboshEngine.Runtime.Core.EventManagement;
using UnityEngine;
using UnityEngine.UI;

namespace IboshEngine.Runtime.Systems.UISystem.Panels.Popup
{
    public class SettingsPanel : PopupPanel
    {
        [BoxGroup("Buttons")][SerializeField] private Button sfxButton;
        [BoxGroup("Buttons")][SerializeField] private Button musicButton;
        [BoxGroup("Buttons")][SerializeField] private Button hapticButton;

        [BoxGroup("Sprites")][SerializeField] private Sprite musicOnSprite;
        [BoxGroup("Sprites")][SerializeField] private Sprite musicOffSprite;
        [BoxGroup("Sprites")][SerializeField] private Sprite sfxOnSprite;
        [BoxGroup("Sprites")][SerializeField] private Sprite sfxOffSprite;
        [BoxGroup("Sprites")][SerializeField] private Sprite hapticOnSprite;
        [BoxGroup("Sprites")][SerializeField] private Sprite hapticOffSprite;

        private Image _musicImage, _sfxImage, _hapticImage;

        #region Built-In

        protected override void Awake()
        {
            base.Awake();

            _musicImage = musicButton.GetComponent<Image>();
            _sfxImage = sfxButton.GetComponent<Image>();
            _hapticImage = hapticButton.GetComponent<Image>();

            // Load saved preferences
            UpdateButtonSprite("Music", _musicImage, musicOnSprite, musicOffSprite);
            UpdateButtonSprite("SFX", _sfxImage, sfxOnSprite, sfxOffSprite);
            UpdateButtonSprite("Haptic", _hapticImage, hapticOnSprite, hapticOffSprite);

            // Assign click listeners
            musicButton.onClick.AddListener(() => ToggleSetting("Music", _musicImage, musicOnSprite, musicOffSprite));
            sfxButton.onClick.AddListener(() => ToggleSetting("SFX", _sfxImage, sfxOnSprite, sfxOffSprite));
            hapticButton.onClick.AddListener(() => ToggleSetting("Haptic", _hapticImage, hapticOnSprite, hapticOffSprite));
        }

        #endregion

        #region UI Management

        public override void Show()
        {
            base.Show();
        }

        void ToggleSetting(string key, Image buttonImage, Sprite onSprite, Sprite offSprite)
        {
            bool current = PlayerPrefs.GetInt(key, 1) == 1;
            bool next = !current;
            PlayerPrefs.SetInt(key, next ? 1 : 0);
            buttonImage.sprite = next ? onSprite : offSprite;

            Debug.Log($"{key}: {(next ? "On" : "Off")}");
        }

        void UpdateButtonSprite(string key, Image buttonImage, Sprite onSprite, Sprite offSprite)
        {
            bool isOn = PlayerPrefs.GetInt(key, 1) == 1;
            buttonImage.sprite = isOn ? onSprite : offSprite;
        }

        #endregion

        #region Event Subscription

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            EventManagerProvider.UI.AddListener(UIEvent.OnSettingsButtonClicked, Show);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            EventManagerProvider.UI.RemoveListener(UIEvent.OnSettingsButtonClicked, Show);
        }

        #endregion
    }
}