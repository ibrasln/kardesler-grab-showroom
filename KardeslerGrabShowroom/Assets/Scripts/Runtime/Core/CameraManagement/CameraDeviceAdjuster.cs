using UnityEngine;
using Unity.Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;

namespace KardeslerGrabShowroom.Core.CameraManagement
{
    [ExecuteAlways] // run in editor and in play mode
    public class CameraDeviceAdjuster : MonoBehaviour
    {
        [ReadOnly] public DeviceType CurrentDeviceType;

        [SerializeField] private SerializedDictionary<CinemachineCamera, CameraSettings> cameraSettingsByCamera;

        private void OnEnable()
        {
            ApplyForCurrentDevice();
        }

        private void Start()
        {
            ApplyForCurrentDevice();
        }

        private void ApplyForCurrentDevice()
        {
            CurrentDeviceType = IsTablet() ? DeviceType.Tablet : DeviceType.Phone;
            ApplySettings();
        }

        private void ApplySettings()
        {
            if (cameraSettingsByCamera == null) return;

            foreach (var cameraSetting in cameraSettingsByCamera)
            {
                if (cameraSetting.Key == null) continue;

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
			float w = Screen.width;
			float h = Screen.height;

			// Always use the larger side as width
			float aspect = Mathf.Max(w, h) / Mathf.Min(w, h);
			Debug.Log($"Aspect: {aspect}");

			// Tablets ~4:3 → 1.33
			// Phones ~16:9 or taller → 1.6+
			return aspect < 1.5f;
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
    Tablet
}
