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
using DG.Tweening;
using TMPro;

namespace KardeslerGrabShowroom.Systems.UISystem.Panels
{
	public class WarningPanel : PopupPanel 
	{
		[SerializeField] private Button buttonCancel;
		[SerializeField] private Button buttonOK;
		[SerializeField] private TextMeshProUGUI textWarning;
		[SerializeField] private ColorPickerPanel colorPickerPanel;

		#region Event Subscription

		protected override void SubscribeToEvents()
		{
			base.SubscribeToEvents();
			buttonCancel.onClick.AddListener(Hide);
			buttonOK.onClick.AddListener(Hide);

			EventManagerProvider.UI.AddListener(UIEvent.OnColorPickerCannotHide, HandleOnColorPickerCannotHide);
		}
		
		protected override void UnsubscribeFromEvents()
		{
			base.UnsubscribeFromEvents();
			buttonCancel.onClick.RemoveListener(Hide);
			buttonOK.onClick.RemoveListener(Hide);

			EventManagerProvider.UI.RemoveListener(UIEvent.OnColorPickerCannotHide, HandleOnColorPickerCannotHide);
		}
		
		#endregion

		#region Event Handling

		private void HandleOnColorPickerCannotHide()
		{
			Show();
			textWarning.text = "You have not saved your changes. Do you want to apply your changes?";
			buttonCancel.onClick.AddListener(HandleOnCancelButtonClicked);
			buttonOK.onClick.AddListener(HandleOnOKButtonClicked);
		}

		private void HandleOnCancelButtonClicked()
		{
			colorPickerPanel.CurrentColorPicker.HandleOnColorPickerCancelled();
			buttonCancel.onClick.RemoveListener(HandleOnCancelButtonClicked);
		}

		private void HandleOnOKButtonClicked()
		{
			colorPickerPanel.OnApplyButtonClicked();
			buttonOK.onClick.RemoveListener(HandleOnOKButtonClicked);
		}


		#endregion

	}
}