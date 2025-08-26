using UnityEngine;
using IboshEngine.Runtime.Systems.UISystem.Panels;
using UnityEngine.UI;
using KardeslerGrabShowroom.Utilities;

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
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            previousGrabButton.onClick.RemoveListener(OnPreviousGrabButtonClicked);
            nextGrabButton.onClick.RemoveListener(OnNextGrabButtonClicked);
        }

        #endregion
    
        #region Event Handling

        private void OnPreviousGrabButtonClicked()
        {
            GameResources.Instance.Showroom.GetPreviousGrab();
        }

        private void OnNextGrabButtonClicked()
        {
            GameResources.Instance.Showroom.GetNextGrab();
        }

        #endregion
    }
}
