using UnityEngine;
using IboshEngine.Runtime.Systems.UISystem.Panels;
using UnityEngine.UI;
using KardeslerGrabShowroom.Utilities;
using IboshEngine.Runtime.Core.EventManagement;

namespace KardeslerGrabShowroom.Systems.UISystem.Panels
{
    public class MenuPanel : IboshPanel
    {
        [SerializeField] private Button showroomButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button aboutButton;
        [SerializeField] private Button contactButton;
        [SerializeField] private Button requestFormButton;

        #region Event Subscriptions

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            showroomButton.onClick.AddListener(OnShowroomButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            aboutButton.onClick.AddListener(OnAboutButtonClicked);
            contactButton.onClick.AddListener(OnContactButtonClicked);
            requestFormButton.onClick.AddListener(OnRequestFormButtonClicked);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            showroomButton.onClick.RemoveListener(OnShowroomButtonClicked);
            settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            aboutButton.onClick.RemoveListener(OnAboutButtonClicked);
            contactButton.onClick.RemoveListener(OnContactButtonClicked);
            requestFormButton.onClick.RemoveListener(OnRequestFormButtonClicked);
        }

        #endregion

        #region Button Actions

        private void OnShowroomButtonClicked()
        {
            EventManagerProvider.UI.Broadcast(UIEvent.OnShowroomButtonClicked);
            Hide();
        }

        private void OnSettingsButtonClicked()
        {
            EventManagerProvider.UI.Broadcast(UIEvent.OnSettingsButtonClicked);
        }

        private void OnAboutButtonClicked()
        {
            EventManagerProvider.UI.Broadcast(UIEvent.OnAboutButtonClicked);
        }

        private void OnContactButtonClicked()
        {
            EventManagerProvider.UI.Broadcast(UIEvent.OnContactButtonClicked);
        }

        private void OnRequestFormButtonClicked()
        {
            EventManagerProvider.UI.Broadcast(UIEvent.OnRequestFormButtonClicked);
        }

        #endregion
    }
}

