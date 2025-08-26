using UnityEngine;

namespace IboshEngine.Runtime.Utilities.Extensions
{
	/// <summary>
	/// Extension methods for CanvasGroup.
	/// </summary>
	public static class CanvasGroupExtensions
	{
		/// <summary>
		/// Shows the CanvasGroup with the specified interactable and blocksRaycasts values.
		/// </summary>
		/// <param name="canvasGroup">The CanvasGroup to show.</param>
		public static void Show(this CanvasGroup canvasGroup, bool interactable = true, bool blocksRaycasts = true)
		{
			canvasGroup.alpha = 1;
			canvasGroup.interactable = interactable;
			canvasGroup.blocksRaycasts = blocksRaycasts;
		}

		/// <summary>
		/// Hides the CanvasGroup with the specified interactable and blocksRaycasts values.
		/// </summary>
		/// <param name="canvasGroup">The CanvasGroup to hide.</param>
		public static void Hide(this CanvasGroup canvasGroup, bool interactable = false, bool blocksRaycasts = false)
		{
			canvasGroup.alpha = 0;
			canvasGroup.interactable = interactable;
			canvasGroup.blocksRaycasts = blocksRaycasts;
		}
	}
}