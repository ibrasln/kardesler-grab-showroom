using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using IboshEngine.Runtime.Systems.UISystem.Panels;
using IboshEngine.Runtime.Core.EventManagement;
using IboshEngine.Runtime.Utilities;
using UnityEngine.UI;
using KardeslerGrabShowroom.Gameplay.Grab;
using KardeslerGrabShowroom.Utilities;

namespace KardeslerGrabShowroom.Systems.UISystem.Panels
{
	public class ColorSwitcherPanel : PopupPanel 
	{
		[SerializeField] private ColorPicker mainColorPicker;
		[SerializeField] private ColorPicker subColorPicker;

		#region Event Subscription

		protected override void SubscribeToEvents()
		{
			base.SubscribeToEvents();
			EventManagerProvider.UI.AddListener(UIEvent.OnColorSwitcherButtonClicked, Show);
		}

		protected override void UnsubscribeFromEvents()
		{
			base.UnsubscribeFromEvents();
			EventManagerProvider.UI.RemoveListener(UIEvent.OnColorSwitcherButtonClicked, Show);
		}

		#endregion
		
		#region Button Actions

		#endregion

		#region UI Management

		public override void Show()
		{
			base.Show();
			mainColorPicker?.Initialize(GameResources.Instance.Showroom.CurrentGrab.MainColor);
			subColorPicker?.Initialize(GameResources.Instance.Showroom.CurrentGrab.SubColor);
		}

		public override void Hide()
		{
			base.Hide();
			EventManagerProvider.UI.Broadcast(UIEvent.OnColorSwitcherPanelClosed);
		}
		
		#endregion

		#region Color Panel Management

		private void SetColorPanelButtons(Transform colorButtonsParent)
		{
			foreach (Transform colorButton in colorButtonsParent)
            {
                Button button = colorButton.GetComponent<Button>();
                Image image = colorButton.GetComponent<Image>();

                if (button != null && image != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        Grab currentGrab = GameResources.Instance.Showroom.CurrentGrab;
                        if (currentGrab != null)
                        {
                            currentGrab.SetMainColor(image.color);
                        }
                    });
                }
            }
		}

		#endregion
	}
}