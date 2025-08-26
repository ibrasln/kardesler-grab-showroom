using IboshEngine.Runtime.Systems.UISystem.Panels;
using IboshEngine.Runtime.Core.EventManagement;
using IboshEngine.Runtime.Utilities;

namespace KardeslerGrabShowroom.Systems.UISystem.Panels
{
    public class RequestFormPanel : PopupPanel
    {
        #region Event Subscription

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            EventManagerProvider.UI.AddListener(UIEvent.OnRequestFormButtonClicked, Show);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            EventManagerProvider.UI.RemoveListener(UIEvent.OnRequestFormButtonClicked, Show);
        }

        #endregion
    }
}   