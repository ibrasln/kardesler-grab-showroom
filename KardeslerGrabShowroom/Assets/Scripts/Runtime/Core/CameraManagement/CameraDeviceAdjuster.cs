using UnityEngine;
using Unity.Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;

namespace KardeslerGrabShowroom.Core.CameraManagement
{
	public class CameraDeviceAdjuster : MonoBehaviour
	{
		[ReadOnly] public DeviceType CurrentDeviceType;

		[SerializeField] private SerializedDictionary<CinemachineCamera, CameraSettings> cameraSettingsByCamera;

		void Start()
		{
			if (IsTablet()) CurrentDeviceType = DeviceType.Tablet;
			else CurrentDeviceType = DeviceType.Phone;

			ApplySettings();
		}

		private void ApplySettings()
		{
			foreach (var cameraSetting in cameraSettingsByCamera)
			{
				if (CurrentDeviceType == DeviceType.Tablet)
				{
					cameraSetting.Key.transform.position = cameraSetting.Value.TabletPosition;
					cameraSetting.Key.transform.rotation = Quaternion.Euler(cameraSetting.Value.TabletRotation);
					cameraSetting.Key.Lens.FieldOfView = cameraSetting.Value.TabletFOV;
				}
				else
				{
					cameraSetting.Key.transform.position = cameraSetting.Value.PhonePosition;
					cameraSetting.Key.transform.rotation = Quaternion.Euler(cameraSetting.Value.PhoneRotation);
					cameraSetting.Key.Lens.FieldOfView = cameraSetting.Value.PhoneFOV;
				}
			}
		}

		private bool IsTablet()
		{
			float aspect = (float)Screen.width / Screen.height;

			// Simple heuristic:
			// Tablets usually have wider aspect ratios (> 1.6f is common for phones).
			// Also check physical size if DPI is available.
			float dpi = Screen.dpi;
			float screenSizeInInches = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / dpi;

			return screenSizeInInches > 6.5f; // Treat >6.5 inches as "tablet"
		}

		#region Inspector Buttons

		[Button(ButtonSizes.Medium)]
		public void Phone()
		{
			CurrentDeviceType = DeviceType.Phone;
			ApplySettings();
		}

		[Button(ButtonSizes.Medium)]
		public void Tablet()
		{
			CurrentDeviceType = DeviceType.Tablet;
			ApplySettings();
		}

		#endregion
	}
}

public enum DeviceType
{
	Phone,
	Tablet,
}