#region Using Statements
using UnityEngine;
#endregion

namespace IboshEngine.Runtime.Core.AudioManagement
{
	/// <summary>
	/// ScriptableObject for storing audio data and configuration.
	/// Used to manage both music and sound effects in the game.
	/// </summary>
	[CreateAssetMenu(fileName = "Audio_", menuName = "Data/Audio")]
	public class AudioSO : ScriptableObject
	{
		/// <summary>
		/// The unique identifier for the audio asset.
		/// </summary>
		public string Name;

		/// <summary>
		/// Determines whether this audio is music or sound effect.
		/// </summary>
		public AudioType Type = AudioType.SFX;

		/// <summary>
		/// The actual audio clip asset.
		/// </summary>
		public AudioClip Clip;

		/// <summary>
		/// Whether to apply random pitch variation when playing this audio.
		/// </summary>
		public bool HasRandomPitch = true;
	}

	/// <summary>
	/// Defines the types of audio that can be managed in the game.
	/// </summary>
	public enum AudioType
	{
		/// <summary>
		/// Background music or ambient tracks.
		/// </summary>
		Music,

		/// <summary>
		/// Sound effects for game events and interactions.
		/// </summary>
		SFX
	}
}