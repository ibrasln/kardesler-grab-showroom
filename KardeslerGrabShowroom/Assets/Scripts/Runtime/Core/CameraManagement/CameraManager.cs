using UnityEngine;
using IboshEngine.Runtime.Utilities.Singleton;
using Unity.Cinemachine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using IboshEngine.Runtime.Core.EventManagement;
using DG.Tweening;
using IboshEngine.Runtime.Utilities.Debugger;
using Cysharp.Threading.Tasks;

namespace IboshEngine.Runtime.Core.CameraManagement
{
    /// <summary>
    /// Manages the camera system in the game.
    /// </summary>
    public class CameraManager : IboshSingleton<CameraManager>
    {
        [BoxGroup("Virtual Cameras")][SerializeField] private CinemachineCamera noneCamera;
        [BoxGroup("Virtual Cameras")][SerializeField] private CinemachineCamera menuCamera;
        [BoxGroup("Virtual Cameras")][SerializeField] private CinemachineCamera showroomCamera;

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
            EventManagerProvider.UI.AddListener(UIEvent.OnShowroomButtonClicked, HandleOnShowroomButtonClicked);
        }

        private void UnsubscribeFromEvents()
        {
            EventManagerProvider.UI.RemoveListener(UIEvent.OnShowroomButtonClicked, HandleOnShowroomButtonClicked); 
        }

        #endregion

        #region Event Handling

        private async void HandleOnShowroomButtonClicked()
        {
            await UniTask.Delay(250);
            MoveToShowroom();
        }

        #endregion

        #region Movement

        public async void MoveToNone()
        {
            EventManagerProvider.Camera.Broadcast(CameraEvent.OnNoneCameraStarted);
            await SwitchToCamAsync(noneCamera);
            EventManagerProvider.Camera.Broadcast(CameraEvent.OnNoneCameraCompleted);
        }

        public async void MoveToMenu()
        {
            EventManagerProvider.Camera.Broadcast(CameraEvent.OnMenuCameraStarted);
            await SwitchToCamAsync(menuCamera);
            EventManagerProvider.Camera.Broadcast(CameraEvent.OnMenuCameraCompleted);
        }

        public async void MoveToShowroom()
        {
            EventManagerProvider.Camera.Broadcast(CameraEvent.OnShowroomCameraStarted);
            await SwitchToCamAsync(showroomCamera);
            EventManagerProvider.Camera.Broadcast(CameraEvent.OnShowroomCameraCompleted);
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
            noneCamera.Priority = 0;
            menuCamera.Priority = 0;
            showroomCamera.Priority = 0;
        }

        #endregion

        #region Inspector Buttons

        [Button(ButtonSizes.Medium)]
        public void ToNone()
        {
            SetPriority(noneCamera);
        }

        [Button(ButtonSizes.Medium)]
        public void ToMenu()
        {
            SetPriority(menuCamera);
        }

        [Button(ButtonSizes.Medium)]
        public void ToShowroom()
        {
            SetPriority(showroomCamera);
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