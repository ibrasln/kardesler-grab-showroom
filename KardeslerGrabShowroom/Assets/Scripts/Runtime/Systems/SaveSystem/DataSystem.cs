using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using IboshEngine.Runtime.Core.EventManagement;
using IboshEngine.Runtime.Utilities.Debugger;
using IboshEngine.Runtime.Utilities.Singleton;
using UniRx;
using UnityEngine;

namespace IboshEngine.Runtime.Systems.SaveSystem
{
	/// <summary>
	/// Manages and persists player-related data.
	/// </summary>
	public class DataSystem : IboshSingleton<DataSystem>
	{
		[ShowInInspector, ReadOnly] public ReactiveProperty<int> CoinAmount { get; private set; } = new ReactiveProperty<int>();
		[ShowInInspector, ReadOnly] public ReactiveProperty<int> GemAmount { get; private set; } = new ReactiveProperty<int>();

		/// <summary>
		/// Gets the file path for saving player data.
		/// </summary>
		private string FilePath => Path.Combine(Application.persistentDataPath, "DataSystem.json");

		#region Built-In

		private void Start()
		{
			LoadDataAsync().Forget();
		}

		private void OnEnable()
		{
			SubscribeToEvents();
		}

		private void OnDisable()
		{
			UnsubscribeFromEvents();
		}

		private void OnApplicationQuit()
		{
			SaveData();
		}

		#endregion

		#region Event Subscription
		private void SubscribeToEvents()
		{
		}

		private void UnsubscribeFromEvents()
		{
		}

		#endregion

		#region Data Modifiers

		private void IncreaseCoinAmount(int amount)
		{
			CoinAmount.Value += amount;
		}

		private void IncreaseGemAmount(int amount)
		{
			GemAmount.Value += amount;
		}

		#endregion

		#region Save & Load

		/// <summary>
		/// Saves player data to file.
		/// </summary>
		private void SaveData()
		{
			DataSystemModel data = new()
			{
				CoinAmount = CoinAmount.Value,
				GemAmount = GemAmount.Value,
			};

			string json = JsonConvert.SerializeObject(data, Formatting.Indented);
			File.WriteAllText(FilePath, json);

			IboshDebugger.LogMessage("Player data saved.", "Player Data",
				IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Cyan);
		}

		/// <summary>
		/// Loads player data from file.
		/// </summary>
		private async UniTask LoadDataAsync()
		{
			if (File.Exists(FilePath))
			{
				string json = await File.ReadAllTextAsync(FilePath);
				DataSystemModel data = JsonConvert.DeserializeObject<DataSystemModel>(json);

				CoinAmount.Value = data.CoinAmount;
				GemAmount.Value = data.GemAmount;

				IboshDebugger.LogMessage("Player data loaded.", "Player Data",
					IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Cyan);
			}
		}

		[GUIColor(1, .2f, .2f)]
		[Button]
		public void ResetData()
		{
#if UNITY_EDITOR
			if (File.Exists(FilePath))
			{
				File.Delete(FilePath);
				IboshDebugger.LogMessage("Player data JSON file has been reset.", "Player Data",
					IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Magenta);
			}
			else
			{
				IboshDebugger.LogWarning("No player data JSON file found to delete.", "Player Data",
					IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Yellow);
			}
#endif
		}
		#endregion
	}

	/// <summary>
	/// Data model for serializing player data to JSON.
	/// </summary>
	[Serializable]
	public class DataSystemModel
	{
		public int CoinAmount;
		public int GemAmount;
	}
}