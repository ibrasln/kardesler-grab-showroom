using IboshEngine.Runtime.Core.AudioManagement;
using UnityEngine;
using UnityEngine.UI;

namespace IboshEngine.Runtime.Systems.UISystem
{
	public class SoundGroup : MonoBehaviour
	{
		[SerializeField] private SoundGroupType soundGroupType;
		[SerializeField] private Slider slider;
		[SerializeField] private Image handle;
		[SerializeField] private Button toggleOffButton;
		[SerializeField] private Button toggleOnButton;
		[SerializeField] private Color handleOffColor;
		[SerializeField] private Color handleOnColor;

		private float _previousVolume = 1;

		#region Built-In
		private void Start()
		{
			float savedVolume = PlayerPrefs.GetFloat($"Volume_{soundGroupType}", 1f);
			SetSlider(savedVolume);
			OnSliderValueChanged(savedVolume);
		}

		private void OnEnable()
		{
			slider.onValueChanged.AddListener(OnSliderValueChanged);
			toggleOffButton.onClick.AddListener(OnToggleOffButtonClicked);
			toggleOnButton.onClick.AddListener(OnToggleOnButtonClicked);
		}

		private void OnDisable()
		{
			slider.onValueChanged.RemoveListener(OnSliderValueChanged);
			toggleOffButton.onClick.RemoveListener(OnToggleOffButtonClicked);
			toggleOnButton.onClick.RemoveListener(OnToggleOnButtonClicked);
		}
		#endregion

		private void OnSliderValueChanged(float value)
		{
			if (soundGroupType == SoundGroupType.SFX)
			{
				AudioManager.Instance.SetSFXVolume(value);
			}
			else
			{
				AudioManager.Instance.SetMusicVolume(value);
			}

			if (value > 0)
			{
				handle.color = handleOnColor;
				EnableToggleOffButton();
			}
			else
			{
				handle.color = handleOffColor;
				EnableToggleOnButton();
			}

			PlayerPrefs.SetFloat($"Volume_{soundGroupType}", value);
			PlayerPrefs.Save();
		}

		private void OnToggleOnButtonClicked()
		{
			EnableToggleOffButton();
			SetSlider(_previousVolume);
		}

		private void OnToggleOffButtonClicked()
		{
			EnableToggleOnButton();
			_previousVolume = slider.value;
			SetSlider(0);
		}

		private void EnableToggleOnButton()
		{
			toggleOnButton.gameObject.SetActive(true);
			toggleOffButton.gameObject.SetActive(false);
		}

		private void EnableToggleOffButton()
		{
			toggleOnButton.gameObject.SetActive(false);
			toggleOffButton.gameObject.SetActive(true);
		}

		private void SetSlider(float value)
		{
			slider.value = value;
		}
	}

	public enum SoundGroupType
	{
		Music,
		SFX
	}
}