using IboshEngine.Runtime.Systems.UISystem.Panels;
using IboshEngine.Runtime.Core.EventManagement;
using IboshEngine.Runtime.Utilities;

namespace KardeslerGrabShowroom.Systems.UISystem.Panels
{
    public class AboutPanel : PopupPanel
    {
        #region Event Subscription

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            EventManagerProvider.UI.AddListener(UIEvent.OnAboutButtonClicked, Show);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            EventManagerProvider.UI.RemoveListener(UIEvent.OnAboutButtonClicked, Show);
        }

        #endregion
    }
}