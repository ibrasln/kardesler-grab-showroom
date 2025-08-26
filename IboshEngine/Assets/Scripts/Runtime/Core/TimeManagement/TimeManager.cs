using System;
using Cysharp.Threading.Tasks;
using IboshEngine.Runtime.Utilities.Debugger;
using IboshEngine.Runtime.Utilities.Singleton;
using UnityEngine.Networking;

namespace IboshEngine.Runtime.Core.TimeManagement
{
	/// <summary>
	/// Manages time-related functionality, particularly fetching accurate time from Google's servers.
	/// Implements the Singleton pattern for global access.
	/// </summary>
	public static class TimeManager
	{
		/// <summary>
		/// URL used for fetching current time from Google.
		/// </summary>
		private static readonly string googleUrl = "https://www.google.com";

		/// <summary>
		/// Fetches the current time from Google's servers.
		/// Falls back to local UTC time if the request fails.
		/// </summary>
		/// <returns>Current UTC DateTime from Google or local system</returns>
		public static async UniTask<DateTime> GetGoogleTime()
		{
			using UnityWebRequest request = UnityWebRequest.Head(googleUrl);
			try
			{
				var operation = request.SendWebRequest();

				while (!operation.isDone)
					await UniTask.Yield();

				if (request.result != UnityWebRequest.Result.Success)
				{
					IboshDebugger.LogError("Error fetching time: " + request.error, "Time Manager", IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Red);
					return DateTime.UtcNow;
				}

				string dateHeader = request.GetResponseHeader("date");
				if (string.IsNullOrEmpty(dateHeader))
				{
					IboshDebugger.LogError("Google API did not return a Date header.", "Time Manager", IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Red);
					return DateTime.UtcNow;
				}

				DateTime googleTime = DateTime.Parse(dateHeader).ToUniversalTime();
				return googleTime;
			}
			catch (Exception e)
			{
				IboshDebugger.LogError("Exception fetching time: " + e.Message, "Time Manager", IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Red);
				return DateTime.UtcNow;
			}
		}
	}
}