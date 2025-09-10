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

namespace KardeslerGrabShowroom.Systems.UISystem.Panels
{
	public class ColorPickerPanel : PopupPanel
	{
		[SerializeField] private ColorPicker mainColorPicker;
		[SerializeField] private ColorPicker subColorPicker;
		[SerializeField] private Button buttonMainColor;
		[SerializeField] private Button buttonSubColor;
		[SerializeField] private Button buttonCancel;
		[SerializeField] private Button buttonApply;

		private ColorPicker _currentColorPicker;

		public ColorPicker CurrentColorPicker => _currentColorPicker;

		#region Event Subscription

		protected override void SubscribeToEvents()
		{
			base.SubscribeToEvents();
			buttonMainColor.onClick.AddListener(OnMainColorButtonClicked);
			buttonSubColor.onClick.AddListener(OnSubColorButtonClicked);
			buttonApply.onClick.AddListener(OnApplyButtonClicked);
			buttonCancel.onClick.AddListener(OnCancelButtonClicked);

			EventManagerProvider.UI.AddListener(UIEvent.OnColorPickerButtonClicked, Show);
			EventManagerProvider.UI.AddListener(UIEvent.OnColorPickerApplied, Hide);
			EventManagerProvider.UI.AddListener(UIEvent.OnColorPickerCancelled, Hide);
		}

		protected override void UnsubscribeFromEvents()
		{
			base.UnsubscribeFromEvents();
			buttonMainColor.onClick.RemoveListener(OnMainColorButtonClicked);
			buttonSubColor.onClick.RemoveListener(OnSubColorButtonClicked);
			buttonApply.onClick.RemoveListener(OnApplyButtonClicked);
			buttonCancel.onClick.RemoveListener(OnCancelButtonClicked);

			EventManagerProvider.UI.RemoveListener(UIEvent.OnColorPickerButtonClicked, Show);
			EventManagerProvider.UI.RemoveListener(UIEvent.OnColorPickerApplied, Hide);
			EventManagerProvider.UI.RemoveListener(UIEvent.OnColorPickerCancelled, Hide);
		}

		#endregion

		#region Button Actions

		private void OnMainColorButtonClicked()
		{
			if (_currentColorPicker == mainColorPicker) return;
			if (!subColorPicker.CheckCanHide()) return;
			
			buttonMainColor.transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack);
			buttonSubColor.transform.DOScale(1f, 0.1f).SetEase(Ease.OutBack);
			
			subColorPicker.Hide();

			_currentColorPicker = mainColorPicker;
			mainColorPicker.gameObject.SetActive(true);
		}

		private void OnSubColorButtonClicked()
		{
			if (_currentColorPicker == subColorPicker) return;
			if (!mainColorPicker.CheckCanHide()) return;
			
			buttonMainColor.transform.DOScale(1f, 0.1f).SetEase(Ease.OutBack);
			buttonSubColor.transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack);
			
			mainColorPicker.Hide();

			_currentColorPicker = subColorPicker;
			subColorPicker.gameObject.SetActive(true);
		}

		public void OnApplyButtonClicked()
		{
			EventManagerProvider.UI.Broadcast(UIEvent.OnColorPickerApplied);
		}

		public void OnCancelButtonClicked()
		{
			if (!_currentColorPicker.CheckCanHide()) return;
			EventManagerProvider.UI.Broadcast(UIEvent.OnColorPickerCancelled);
			Hide();
		}

		protected override void OnCloseButtonClicked()
		{
			if (!_currentColorPicker.CheckCanHide()) return;
			base.OnCloseButtonClicked();
			EventManagerProvider.UI.Broadcast(UIEvent.OnColorPickerCancelled);
		}

		#endregion
		
		#region UI Management

		public override void Show()
		{
			base.Show();
			mainColorPicker?.Initialize(GameResources.Instance.Showroom.CurrentGrab.MainColor, ColorPickerType.Main);
			subColorPicker?.Initialize(GameResources.Instance.Showroom.CurrentGrab.SubColor, ColorPickerType.Sub);

			OnMainColorButtonClicked();
		}

		public override void Hide()
		{
			base.Hide();
			EventManagerProvider.UI.Broadcast(UIEvent.OnColorPickerPanelClosed);
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