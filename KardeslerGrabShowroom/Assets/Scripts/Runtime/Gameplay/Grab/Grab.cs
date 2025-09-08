using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using KardeslerGrab.Showroom.Utilities;
using Cysharp.Threading.Tasks;
using IboshEngine.Runtime.Core.EventManagement;

namespace KardeslerGrabShowroom.Gameplay.Grab
{
    public class Grab : MonoBehaviour
    {
        private List<Renderer> _mainRenderers = new();
        private List<Renderer> _subRenderers = new();
        private Tween _rotationTween;
        private bool _isRotating = false;
        private Color _mainColor;
        private Color _subColor;

        public Color MainColor => _mainColor;
        public Color SubColor => _subColor;

        #region Built-In

        private void Awake()
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
                    else if (material != null && material.name.Contains("M_Other"))
                    {
                        _subRenderers.Add(renderer);
                    }
                }
            }
            if (_mainRenderers.Count > 0) _mainColor = _mainRenderers[0].material.color;
            if (_subRenderers.Count > 0) _subColor = _subRenderers[0].material.color;
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
            transform.position = from.position;
            await MoveToAsync(to);
            StartRotation();
            EventManagerProvider.Showroom.Broadcast(ShowroomEvent.OnGrabMovementCompleted);
        }

        public async void Dispose(Transform from, Transform to)
        {
            StopRotation();
            transform.position = from.position;
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
                Debug.LogWarning("Rotation is already active!");
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
                Debug.Log("Rotation paused");
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
                Debug.Log("Rotation resumed");
            }
        }

        #endregion
 
        #region Movement

        public async UniTask MoveToAsync(Transform target)
        {
            await transform.DOMove(target.position, Settings.GrabMovementDuration).SetEase(Ease.InOutSine).ToUniTask();
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
    }
}
