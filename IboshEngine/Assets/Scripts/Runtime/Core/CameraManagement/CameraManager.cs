using UnityEngine;
using IboshEngine.Runtime.Utilities.Singleton;
using Unity.Cinemachine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using IboshEngine.Runtime.Core.EventManagement;
using DG.Tweening;
using IboshEngine.Runtime.Utilities.Debugger;

namespace IboshEngine.Runtime.Core.CameraManagement
{
    /// <summary>
    /// Manages the camera system in the game.
    /// </summary>
    public class CameraManager : IboshSingleton<CameraManager>
    {
        [BoxGroup("Virtual Cameras")]
        [SerializeField]
        private CinemachineCamera sampleCamera;

        private CinemachineCamera _currentCamera;
        private CinemachineBrain _cinemachineBrain;
        private float _movementDelay;

        #region Built-In

        protected override void Awake()
        {
            base.Awake();
            _cinemachineBrain = GetComponent<CinemachineBrain>();
            _movementDelay = _cinemachineBrain.DefaultBlend.BlendTime;
        }

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        #endregion

        #region Event Subscription

        private void SubscribeToEvents()
        {

        }

        private void UnsubscribeFromEvents()
        {

        }

        #endregion

        #region Movement

        public async void MoveToNone()
        {
            EventManagerProvider.Camera.Broadcast(CameraEvent.OnNoneCameraStarted);
            await SwitchToCamAsync(sampleCamera);
            EventManagerProvider.Camera.Broadcast(CameraEvent.OnNoneCameraCompleted);
        }

        #endregion

        #region Virtual Camera Management

        private async UniTask SwitchToCamAsync(CinemachineCamera targetCam)
        {
            SetPriority(targetCam);
            await UniTask.Delay((int)(_movementDelay * 1000));
        }

        private void SetPriority(CinemachineCamera targetCam)
        {
            ResetPriorities();
            targetCam.Priority = 10;
            _currentCamera = targetCam;
            IboshDebugger.LogMessage($"Switched to {targetCam.name}", "Camera", IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Magenta);
        }

        private void ResetPriorities()
        {
            sampleCamera.Priority = 0;
        }

        #endregion

        #region Inspector Buttons

        [Button(ButtonSizes.Medium)]
        public void ToSample()
        {
            SetPriority(sampleCamera);
        }

        #endregion

        #region Camera Shake

        public void ShakeCamera(float duration, float strength, CameraShakeDirection direction)
        {
            if (_currentCamera == null) return;

            Vector3 shakeStrength = Vector3.zero;

            switch (direction)
            {
                case CameraShakeDirection.Horizontal:
                    shakeStrength = new Vector3(strength, 0, 0);
                    break;
                case CameraShakeDirection.Vertical:
                    shakeStrength = new Vector3(0, 0, strength);
                    break;
                case CameraShakeDirection.Both:
                    shakeStrength = new Vector3(strength, 0, strength);
                    break;
                default:
                    break;
            }

            _currentCamera.transform.DOShakePosition(duration, shakeStrength);
        }

        #endregion
    }

    public enum CameraShakeDirection
    {
        Horizontal,
        Vertical,
        Both
    }
}