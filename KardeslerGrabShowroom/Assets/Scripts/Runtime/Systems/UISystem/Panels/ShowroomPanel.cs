using UnityEngine;
using IboshEngine.Runtime.Systems.UISystem.Panels;
using UnityEngine.UI;
using KardeslerGrabShowroom.Utilities;
using IboshEngine.Runtime.Core.EventManagement;
using IboshEngine.Runtime.Utilities;
using Cysharp.Threading.Tasks;

namespace KardeslerGrabShowroom.Systems.UISystem.Panels
{
    public class ShowroomPanel : IboshPanel
    {
        [SerializeField] private Button previousGrabButton;
        [SerializeField] private Button nextGrabButton;

        #region Built-In

        #endregion

        #region Event Subscriptions

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            previousGrabButton.onClick.AddListener(OnPreviousGrabButtonClicked);
            nextGrabButton.onClick.AddListener(OnNextGrabButtonClicked);

            EventManagerProvider.Camera.AddListener(CameraEvent.OnShowroomCameraCompleted, HandleOnShowroomCameraCompleted);
            EventManagerProvider.Showroom.AddListener(ShowroomEvent.OnGrabMovementStarted, DisableButtons);
            EventManagerProvider.Showroom.AddListener(ShowroomEvent.OnGrabMovementCompleted, EnableButtons);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            previousGrabButton.onClick.RemoveListener(OnPreviousGrabButtonClicked);
            nextGrabButton.onClick.RemoveListener(OnNextGrabButtonClicked);

            EventManagerProvider.Camera.RemoveListener(CameraEvent.OnShowroomCameraCompleted, HandleOnShowroomCameraCompleted);
            EventManagerProvider.Showroom.RemoveListener(ShowroomEvent.OnGrabMovementStarted, DisableButtons);
            EventManagerProvider.Showroom.RemoveListener(ShowroomEvent.OnGrabMovementCompleted, EnableButtons);
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

        #endregion

        #region UI Management

        private void DisableButtons()
        {
            previousGrabButton.interactable = false;
            nextGrabButton.interactable = false;
        }
        
        private void EnableButtons()
        {
            previousGrabButton.interactable = true;
            nextGrabButton.interactable = true;
        }

        #endregion
    }
}
