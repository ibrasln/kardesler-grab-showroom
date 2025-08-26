using IboshEngine.Runtime.Systems.UISystem.Panels;
using IboshEngine.Runtime.Core.EventManagement;
using IboshEngine.Runtime.Utilities;

namespace KardeslerGrabShowroom.Systems.UISystem.Panels
{
    public class ContactPanel : PopupPanel
    {
        #region Event Subscription

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            EventManagerProvider.UI.AddListener(UIEvent.OnContactButtonClicked, Show);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            EventManagerProvider.UI.RemoveListener(UIEvent.OnContactButtonClicked, Show);
        }

        #endregion
    }
}