using UnityEngine;
using IboshEngine.Runtime.Systems.UISystem.Panels;
using UnityEngine.UI;
using KardeslerGrabShowroom.Utilities;
using IboshEngine.Runtime.Core.EventManagement;
using IboshEngine.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using KardeslerGrabShowroom.Gameplay.Grab;

namespace KardeslerGrabShowroom.Systems.UISystem.Panels
{
    public class ShowroomPanel : IboshPanel
    {
        [SerializeField] private Button previousGrabButton;
        [SerializeField] private Button nextGrabButton;
        [SerializeField] private Button colorPickerButton;
        [SerializeField] private Button grabDetailsButton;


        #region Event Subscriptions

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            previousGrabButton.onClick.AddListener(OnPreviousGrabButtonClicked);
            nextGrabButton.onClick.AddListener(OnNextGrabButtonClicked);
            colorPickerButton.onClick.AddListener(OnColorPickerButtonClicked);
            grabDetailsButton.onClick.AddListener(OnGrabDetailsButtonClicked);

            EventManagerProvider.Camera.AddListener(CameraEvent.OnShowroomCameraCompleted, HandleOnShowroomCameraCompleted);
            EventManagerProvider.Showroom.AddListener(ShowroomEvent.OnGrabMovementStarted, DisableButtons);
            EventManagerProvider.Showroom.AddListener(ShowroomEvent.OnGrabMovementCompleted, EnableButtons);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            previousGrabButton.onClick.RemoveListener(OnPreviousGrabButtonClicked);
            nextGrabButton.onClick.RemoveListener(OnNextGrabButtonClicked);
            colorPickerButton.onClick.RemoveListener(OnColorPickerButtonClicked);
            grabDetailsButton.onClick.RemoveListener(OnGrabDetailsButtonClicked);

            EventManagerProvider.Camera.RemoveListener(CameraEvent.OnShowroomCameraCompleted, HandleOnShowroomCameraCompleted);
            EventManagerProvider.Showroom.RemoveListener(ShowroomEvent.OnGrabMovementStarted, ShowBlockingOverlay);
            EventManagerProvider.Showroom.RemoveListener(ShowroomEvent.OnGrabMovementCompleted, HideBlockingOverlay);
        }

        #endregion
    
        #region Event Handling

        private async void HandleOnShowroomCameraCompleted()
        {
            await UniTask.Delay(250);
            Show();
        }

        #endregion

        #region Button Actions

        private void OnPreviousGrabButtonClicked()
        {
            GameResources.Instance.Showroom.GetPreviousGrab();
        }

        private void OnNextGrabButtonClicked()
        {
            GameResources.Instance.Showroom.GetNextGrab();
        }

        private void OnColorPickerButtonClicked()
        {
            EventManagerProvider.UI.Broadcast(UIEvent.OnColorPickerButtonClicked);
        }

        private void OnGrabDetailsButtonClicked()
        {
            EventManagerProvider.UI.Broadcast(UIEvent.OnGrabDetailsButtonClicked);
        }

        #endregion

        #region UI Management

        private void DisableButtons()
        {
            previousGrabButton.interactable = false;
            nextGrabButton.interactable = false;
            colorPickerButton.interactable = false;
            grabDetailsButton.interactable = false;
        }
        
        private void EnableButtons()
        {
            previousGrabButton.interactable = true;
            nextGrabButton.interactable = true;
            colorPickerButton.interactable = true;
            grabDetailsButton.interactable = true;
        }

        #endregion
    }
}
