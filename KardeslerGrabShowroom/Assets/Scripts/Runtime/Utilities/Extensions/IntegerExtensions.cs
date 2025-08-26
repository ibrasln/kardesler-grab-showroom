using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace IboshEngine.Runtime.Utilities.Extensions
{
    /// <summary>
    /// Extension methods for integer values.
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Converts an integer value to a boolean.
        /// </summary>
        public static bool ToBool(this int value)
        {
            return value == 1;
        }

        /// <summary>
        /// Animates an integer value asynchronously.
        /// </summary>
        /// <param name="startValue">The starting value.</param>
        /// <param name="endValue">The ending value.</param>
        /// <param name="onValueUpdated">The action to perform when the value is updated.</param>
        public static async UniTask AnimateValueAsync(this int startValue, int endValue,
            Action<int> onValueUpdated, float duration = 1f, Action onComplete = null)
        {
            await DOTween.To(() => startValue,
                    x =>
                    {
                        startValue = Mathf.RoundToInt(x);

                        if (startValue <= 0)
                        {
                            startValue = 0;
                            onValueUpdated?.Invoke(startValue);
                            onComplete?.Invoke();
                            return;
                        }

                        onValueUpdated?.Invoke(startValue);
                    },
                    endValue,
                    duration)
                .SetEase(Ease.Linear)
                .ToUniTask();
        }
    }
}