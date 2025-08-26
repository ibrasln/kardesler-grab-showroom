using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using IboshEngine.Runtime.Core.EventManagement;
using IboshEngine.Runtime.Utilities.Debugger;
using IboshEngine.Runtime.Utilities.Singleton;

namespace IboshEngine.Runtime.Systems.SaveSystem
{
	/// <summary>
	/// Base class for save systems that handle data persistence.
	/// Implements generic data handling and file I/O operations.
	/// </summary>
	/// <typeparam name="TData">The type of data to be saved</typeparam>
	/// <typeparam name="TWrapper">The wrapper type that contains the data collection</typeparam>
	public abstract class BaseSaveSystem<TData, TWrapper> : IboshSingleton<BaseSaveSystem<TData, TWrapper>>
		where TWrapper : IDataWrapper<TData>, new()
	{
		/// <summary>
		/// Collection of data items to be saved.
		/// </summary>
		public List<TData> DataCollection = new();

		/// <summary>
		/// Gets the file path where data should be saved.
		/// </summary>
		protected abstract string FilePath { get; }

		/// <summary>
		/// Gets the event to broadcast when data is updated.
		/// </summary>
		protected abstract DataEvent DataUpdatedEvent { get; }

		/// <summary>
		/// Gets the event to broadcast when data is saved.
		/// </summary>
		protected abstract DataEvent DataSavedEvent { get; }

		/// <summary>
		/// Gets the event to broadcast when data is loaded.
		/// </summary>
		protected abstract DataEvent DataLoadedEvent { get; }

		protected virtual void Start()
		{
			LoadDataAsync().Forget();
		}

		/// <summary>
		/// Updates the data collection and broadcasts the update event.
		/// </summary>
		/// <param name="dataCollection">New data collection to update with</param>
		public virtual void UpdateCollection(List<TData> dataCollection)
		{
			DataCollection = dataCollection;
			EventManagerProvider.Data.Broadcast(DataUpdatedEvent, DataCollection);
		}

		/// <summary>
		/// Saves the current data collection to a JSON file.
		/// </summary>
		public async void SaveData()
		{
			TWrapper wrapper = new() { Data = DataCollection };
			string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
			await File.WriteAllTextAsync(FilePath, json);

			EventManagerProvider.Data.Broadcast(DataSavedEvent, DataCollection);
			IboshDebugger.LogMessage($"{typeof(TData).Name} data has been saved.", "Save Manager",
				IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Cyan);
		}

		private async UniTask LoadDataAsync()
		{
			if (!File.Exists(FilePath)) return;

			string json = await File.ReadAllTextAsync(FilePath);
			TWrapper wrapper = JsonConvert.DeserializeObject<TWrapper>(json);

			if (wrapper is not null) DataCollection = wrapper.Data;

			EventManagerProvider.Data.Broadcast(DataLoadedEvent, DataCollection);
			IboshDebugger.LogMessage($"{typeof(TData).Name} data has been loaded.", "Save Manager",
				IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Cyan);
		}

		/// <summary>
		/// Resets all data and deletes the save file.
		/// Only available in Unity Editor.
		/// </summary>
		[GUIColor(1, .2f, .2f)]
		[Button]
		public void ResetData()
		{
			DataCollection = new();
			if (File.Exists(FilePath))
			{
				File.Delete(FilePath);
				IboshDebugger.LogMessage($"{typeof(TData).Name} JSON file has been reset.", "Save Manager",
					IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Magenta);
			}
			else
			{
				IboshDebugger.LogWarning($"No {typeof(TData).Name} JSON file found to delete.", "Save Manager",
					IboshDebugger.DebugColor.Gray, IboshDebugger.DebugColor.Yellow);
			}
		}
	}

	/// <summary>
	/// Interface for data wrapper classes that contain collections of data.
	/// </summary>
	/// <typeparam name="T">The type of data in the collection</typeparam>
	public interface IDataWrapper<T>
	{
		/// <summary>
		/// The collection of data items.
		/// </summary>
		List<T> Data { get; set; }
	}
}