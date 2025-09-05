using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using IboshEngine.Runtime.Systems.UISystem.Panels;
using IboshEngine.Runtime.Core.EventManagement;
using IboshEngine.Runtime.Utilities;

namespace KardeslerGrabShowroom.Systems.UISystem.Panels
{
	public class GrabDetailsPanel : PopupPanel 
	{
		#region Event Subscription

		protected override void SubscribeToEvents()
		{
			base.SubscribeToEvents();
			EventManagerProvider.UI.AddListener(UIEvent.OnGrabDetailsButtonClicked, Show);
		}

		protected override void UnsubscribeFromEvents()
		{
			base.UnsubscribeFromEvents();
			EventManagerProvider.UI.RemoveListener(UIEvent.OnGrabDetailsButtonClicked, Show);
		}

		#endregion
	}
}