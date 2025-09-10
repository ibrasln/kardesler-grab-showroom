using UnityEngine;
using Sirenix.OdinInspector;

namespace KardeslerGrabShowroom.Core.CameraManagement
{
	[CreateAssetMenu(fileName = "CameraSettings", menuName = "Camera/CameraSettings")]
	public class CameraSettings : ScriptableObject 
	{
		[FoldoutGroup("Phone Settings")] public Vector3 PhonePosition;
		[FoldoutGroup("Phone Settings")] public Vector3 PhoneRotation;
		[FoldoutGroup("Phone Settings")] public float PhoneFOV;

		[FoldoutGroup("Tablet Settings")] public Vector3 TabletPosition;
		[FoldoutGroup("Tablet Settings")] public Vector3 TabletRotation;
		[FoldoutGroup("Tablet Settings")] public float TabletFOV;
	}
}