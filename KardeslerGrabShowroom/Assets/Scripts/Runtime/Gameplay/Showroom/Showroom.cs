using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Cysharp.Threading.Tasks;
using IboshEngine.Runtime.Core.EventManagement;

namespace KardeslerGrabShowroom.Gameplay.Showroom
{
	public class Showroom : MonoBehaviour 
	{
		[BoxGroup("Grab Properties")][ReadOnly]public Grab.Grab CurrentGrab;
		[SerializeField] private Grab.Grab[] grabs;

		[BoxGroup("Transforms")][SerializeField] private Transform previousGrabTransform;
		[BoxGroup("Transforms")][SerializeField] private Transform currentGrabTransform;
		[BoxGroup("Transforms")][SerializeField] private Transform nextGrabTransform;

		private int _currentGrabIndex = 0;

		#region Built-In

		private void Awake() 
		{
			grabs = GetComponentsInChildren<Grab.Grab>(true);
			foreach (var grab in grabs)
			{
				if (grab != null && grab.gameObject.activeSelf)
				{
					grab.gameObject.SetActive(false);
				}
			}
		}

		private void OnEnable()
		{
			SubscribeToEvents();
		}

		private void OnDisable()
		{
			UnsubscribeFromEvents();
		}

		#endregion

		#region Event Subscription

		private void SubscribeToEvents()
		{
			EventManagerProvider.Camera.AddListener(CameraEvent.OnShowroomCameraCompleted, HandleOnShowroomCameraStarted);
		}

		private void UnsubscribeFromEvents()
		{
			EventManagerProvider.Camera.RemoveListener(CameraEvent.OnShowroomCameraCompleted, HandleOnShowroomCameraStarted);
		}

		#endregion

		#region Event Handling

		private async void HandleOnShowroomCameraStarted()
		{
			await UniTask.Delay(250);
			Initialize();
		}

		#endregion

		#region Initialization & Disposal

		public void Initialize()
		{
			SetCurrentGrab(0, GrabDirection.Next);
		}

		public void Dispose()
		{
		}

		#endregion

		#region Grab Management

		private void SetCurrentGrab(int index, GrabDirection direction)
		{
			if (CurrentGrab != null)
			{
				if (direction == GrabDirection.Previous)
				{
					CurrentGrab.Dispose(currentGrabTransform, nextGrabTransform);
				}
				else
				{
					CurrentGrab.Dispose(currentGrabTransform, previousGrabTransform);
				}
			}

			_currentGrabIndex = index;
			CurrentGrab = grabs[_currentGrabIndex];
			CurrentGrab.gameObject.SetActive(true);

			if (direction == GrabDirection.Previous)
			{
				CurrentGrab.Initialize(previousGrabTransform, currentGrabTransform);
			}
			else
			{
				CurrentGrab.Initialize(nextGrabTransform, currentGrabTransform);
			}
		}

		public void GetPreviousGrab()
		{
			if (_currentGrabIndex <= 0) return;

			SetCurrentGrab(_currentGrabIndex - 1, GrabDirection.Previous);
		}

		public void GetNextGrab()
		{
			if (_currentGrabIndex >= grabs.Length - 1) return;

			SetCurrentGrab(_currentGrabIndex + 1, GrabDirection.Next);
		}

		#endregion
	}
}

public enum GrabDirection
{
	Previous,
	Next
}