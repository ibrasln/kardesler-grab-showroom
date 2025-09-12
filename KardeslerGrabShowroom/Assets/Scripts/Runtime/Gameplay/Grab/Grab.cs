using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using KardeslerGrab.Showroom.Utilities;
using Cysharp.Threading.Tasks;
using IboshEngine.Runtime.Core.EventManagement;
using Sirenix.OdinInspector;

namespace KardeslerGrabShowroom.Gameplay.Grab
{
    public class Grab : MonoBehaviour
    {
        [InlineEditor] public GrabData Data;
        private List<Renderer> _mainRenderers = new();
        private List<Renderer> _subRenderers = new();
        private Tween _rotationTween;
        private bool _isRotating = false;
        private Color _mainColor;
        private Color _subColor;

        public Color MainColor => _mainColor;
        public Color SubColor => _subColor;

        #region Built-In

        private async void Awake()
        {
            List<Renderer> allRenderers = GetComponentsInChildren<Renderer>(true).ToList();
            _mainRenderers = new List<Renderer>();
            _subRenderers = new List<Renderer>();

            foreach (Renderer renderer in allRenderers)
            {
                foreach (Material material in renderer.sharedMaterials)
                {
                    if (material != null && material.name.Contains("M_Grab"))
                    {
                        _mainRenderers.Add(renderer);
                    }
                    else if (material != null && material.name.Contains("M_Sub"))
                    {
                        _subRenderers.Add(renderer);
                    }
                }
            }

            await UniTask.Delay(1000);
            
            if (_mainRenderers.Count > 0) 
            {
                _mainColor = Data.MainColor;
                SetMainColor(_mainColor);
            }
            if (_subRenderers.Count > 0) 
            {
                _subColor = Data.SubColor;
                SetSubColor(_subColor);
            }
        }

        private void Start()
        {
            ResetRotation();
        }

        private void OnDestroy()
        {
            StopRotation();
        }

        private void OnDisable()
        {
            if (_isRotating)
            {
                PauseRotation();
            }
        }

        private void OnEnable()
        {
            if (_rotationTween != null && _rotationTween.IsActive() && !_isRotating)
            {
                ResumeRotation();
            }
        }

        #endregion

        #region Initialization & Disposal

        public async void Initialize(Transform from, Transform to)
        {
            EventManagerProvider.Showroom.Broadcast(ShowroomEvent.OnGrabMovementStarted);
            transform.position = new Vector3(from.position.x, from.position.y + Data.YOffset, from.position.z);
            await MoveToAsync(to);
            StartRotation();
            EventManagerProvider.Showroom.Broadcast(ShowroomEvent.OnGrabMovementCompleted);
        }

        public async void Dispose(Transform from, Transform to)
        {
            StopRotation();
            transform.position = new Vector3(from.position.x, from.position.y + Data.YOffset, from.position.z);
            await MoveToAsync(to);
            ResetRotation();
            gameObject.SetActive(false);
        }

        #endregion

        #region Rotation

         public void ResetRotation()
        {
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        /// <summary>
        /// Starts continuous rotation of the object
        /// </summary>
        public void StartRotation()
        {
            if (_isRotating)
            {
                return;
            }

            StopRotation();

            _rotationTween = transform.DORotate(
                    new Vector3(0, 360, 0),
                    360f / Settings.RotationSpeed,
                    RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental)
                .OnStart(() => _isRotating = true)
                .OnKill(() => _isRotating = false);
        }

        /// <summary>
        /// Stops the continuous rotation
        /// </summary>
        public void StopRotation()
        {
            if (_rotationTween != null && _rotationTween.IsActive())
            {
                _rotationTween.Kill();
                _rotationTween = null;
                _isRotating = false;
            }
        }

        /// <summary>
        /// Pauses the rotation (can be resumed)
        /// </summary>
        public void PauseRotation()
        {
            if (_rotationTween != null && _rotationTween.IsActive() && _isRotating)
            {
                _rotationTween.Pause();
            }
        }

        /// <summary>
        /// Resumes the rotation from where it was paused
        /// </summary>
        public void ResumeRotation()
        {
            if (_rotationTween != null && _rotationTween.IsActive() && !_isRotating)
            {
                _rotationTween.Play();
                _isRotating = true;
            }
        }

        #endregion
 
        #region Movement

        public async UniTask MoveToAsync(Transform target)
        {
            await transform.DOMove(new Vector3(target.position.x, target.position.y + Data.YOffset, target.position.z), Settings.GrabMovementDuration).SetEase(Ease.InOutSine).ToUniTask();
        }

        #endregion

        #region Color

        public void SetMainColor(Color color)
        {
            foreach (var renderer in _mainRenderers)
            {
                renderer.material.color = color;
            }
            _mainColor = color;
        }

        public void SetSubColor(Color color)
        {
            foreach (var renderer in _subRenderers)
            {
                renderer.material.color = color;
            }
            _subColor = color;
        }

        #endregion
    
        #region Inspector Buttons

        [Button(ButtonSizes.Medium)]
        public void SetMaterials()
        {
            List<Renderer> allRenderers = GetComponentsInChildren<Renderer>(true).ToList();

            Material mainMaterial = Resources.Load<Material>("Materials/M_Grab");
            Material subMaterial = Resources.Load<Material>("Materials/M_Sub");
            Material chassisMaterial = Resources.Load<Material>("Materials/M_Chassis");

            foreach (Renderer renderer in allRenderers)
            {
                if ((renderer.name.Contains("MİLİ") && !renderer.name.Contains("SACI")) || (renderer.name.Contains("PERNO") && !renderer.name.Contains("SACI")) || renderer.name.Contains("SOMUN") || renderer.name.Contains("ŞAPKA"))
                {
                    renderer.material = subMaterial;
                }
                else if (renderer.name.Contains("PİSTON") && !renderer.name.Contains("SACI"))
                {
                    renderer.material = chassisMaterial;
                }
                else
                {
                    renderer.material = mainMaterial;
                }
            }
        }

        #endregion
    }
}
