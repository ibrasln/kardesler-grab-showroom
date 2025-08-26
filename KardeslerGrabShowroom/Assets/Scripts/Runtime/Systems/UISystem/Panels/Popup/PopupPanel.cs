using Sirenix.OdinInspector;
using IboshEngine.Runtime.Utilities.Debugger;
using UnityEngine;
using UnityEngine.UI;

namespace IboshEngine.Runtime.Systems.UISystem.Panels
{
    /// <summary>
    /// Abstract base class for popup panels in the UI system.
    /// Provides common functionality for popup panels including background close button, close button, and notification display.
    /// </summary>
    public abstract class PopupPanel : IboshPanel
    {
        [BoxGroup("Popup Panel Properties")]
        [SerializeField]
        private Button backgroundCloseButton;

        [BoxGroup("Popup Panel Properties")]
        [SerializeField]
        private Button closeButton;

        [BoxGroup("Popup Panel Properties")]
        [SerializeField]
        private NotifyController notify;

        #region Built-In

        private void OnValidate()
        {
            if (closeButton == null)
            {
                IboshDebugger.LogWarning($"Close button is missing!", gameObject.name, IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Red);
            }

            if (backgroundCloseButton == null)
            {
                IboshDebugger.LogWarning($"Background close button is missing!", gameObject.name, IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Red);
            }
        }

        #endregion

        #region Event Subscription

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            closeButton?.onClick.AddListener(Hide);
            backgroundCloseButton?.onClick.AddListener(Hide);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            closeButton?.onClick.RemoveListener(Hide);
            backgroundCloseButton?.onClick.RemoveListener(Hide);
        }

        #endregion

        public override void Show()
        {
            base.Show();
            //InputManager.Instance.DisableInput();
        }

        protected void EnableCloseButtons()
        {
            closeButton.interactable = true;
            backgroundCloseButton.interactable = true;
        }

        protected void DisableCloseButtons()
        {
            closeButton.interactable = false;
            backgroundCloseButton.interactable = false;
        }

        protected void ShowNotify()
        {
            notify.gameObject.SetActive(true);
        }

        protected void HideNotify()
        {
            notify.gameObject.SetActive(false);
        }
    }
}