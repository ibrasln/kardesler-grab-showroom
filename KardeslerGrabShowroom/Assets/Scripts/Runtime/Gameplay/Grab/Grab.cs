using UnityEngine;
using DG.Tweening;
using KardeslerGrab.Showroom.Utilities;
using Cysharp.Threading.Tasks;
using IboshEngine.Runtime.Core.EventManagement;

namespace KardeslerGrabShowroom.Gameplay.Grab
{
    public class Grab : MonoBehaviour
    {
        private Tween _rotationTween;
        private bool _isRotating = false;

        #region Built-In

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

        #region Reset

        public void ResetRotation()
        {
            transform.rotation = Quaternion.Euler(0, Settings.GrabDefaultAngle, 0);
        }

        #endregion

        #region Rotation

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

            _rotationTween = transform.DORotate(Settings.RotationAxis * 360f, 360f / Settings.RotationSpeed, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
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
    }
}
