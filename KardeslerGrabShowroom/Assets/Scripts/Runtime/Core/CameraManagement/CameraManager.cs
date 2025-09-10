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
        [BoxGroup("Virtual Cameras")][SerializeField] private CinemachineCamera noneCamera;
        [BoxGroup("Virtual Cameras")][SerializeField] private CinemachineCamera menuCamera;
        [BoxGroup("Virtual Cameras")][SerializeField] private CinemachineCamera intermediateCamera;
        [BoxGroup("Virtual Cameras")][SerializeField] private CinemachineCamera showroomCamera;
        [BoxGroup("Virtual Cameras")][SerializeField] private CinemachineCamera colorPickerCamera;

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
            EventManagerProvider.UI.AddListener(UIEvent.OnColorPickerButtonClicked, HandleOnColorPickerButtonClicked);
            EventManagerProvider.UI.AddListener(UIEvent.OnColorPickerPanelClosed, HandleOnColorPickerPanelClosed);
        }

        private void UnsubscribeFromEvents()
        {
            EventManagerProvider.UI.RemoveListener(UIEvent.OnShowroomButtonClicked, HandleOnShowroomButtonClicked); 
            EventManagerProvider.UI.RemoveListener(UIEvent.OnColorPickerButtonClicked, HandleOnColorPickerButtonClicked);
            EventManagerProvider.UI.RemoveListener(UIEvent.OnColorPickerPanelClosed, HandleOnColorPickerPanelClosed);
        }

        #endregion

        #region Event Handling

        private async void HandleOnShowroomButtonClicked()
        {
            await UniTask.Delay(250);
            MoveToShowroom(true);
        }

        private async void HandleOnColorPickerButtonClicked()
        {
            await UniTask.Delay(250);
            MoveToColorPicker();
        }

        private void HandleOnColorPickerPanelClosed()
        {
            MoveToShowroom(false);
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

        public async void MoveToShowroom(bool hasIntermediate)
        {
            EventManagerProvider.Camera.Broadcast(CameraEvent.OnShowroomCameraStarted);
            if (hasIntermediate)
            {
                await SwitchToCamAsync(intermediateCamera);
            }
            await SwitchToCamAsync(showroomCamera);
            EventManagerProvider.Camera.Broadcast(CameraEvent.OnShowroomCameraCompleted);
        }

        public async void MoveToColorPicker()
        {
            EventManagerProvider.Camera.Broadcast(CameraEvent.OnColorPickerCameraStarted);
            await SwitchToCamAsync(colorPickerCamera);
            EventManagerProvider.Camera.Broadcast(CameraEvent.OnColorPickerCameraCompleted);
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
        }

        private void ResetPriorities()
        {
            noneCamera.Priority = 0;
            menuCamera.Priority = 0;
            showroomCamera.Priority = 0;
            intermediateCamera.Priority = 0;
            colorPickerCamera.Priority = 0;
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
        public void ToIntermediate()
        {
            SetPriority(intermediateCamera);
        }

        [Button(ButtonSizes.Medium)]
        public void ToShowroom()
        {
            SetPriority(showroomCamera);
        }

        [Button(ButtonSizes.Medium)]
        public void ToColorSwitcher()
        {
            SetPriority(colorPickerCamera);
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