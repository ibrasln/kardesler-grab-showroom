using System;
using System.Collections.Generic;
using System.Linq;
using IboshEngine.Runtime.Utilities.Debugger;
using UnityEngine;

namespace IboshEngine.Runtime.Utilities.Extensions
{
    /// <summary>
    /// Extension methods for List.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        ///     Determines whether the specified list is null or has a length of zero.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to check.</param>
        /// <returns>
        ///     True if the list is null or has a length of zero; otherwise, false.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            if (list is null)
            {
                IboshDebugger.LogError("The list is null!", IboshDebugger.DebugColor.Red);
                return true;
            }

            if (list.Count == 0)
            {
                IboshDebugger.LogError("The list is empty!", IboshDebugger.DebugColor.Red);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Shuffles the elements of the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="ts">The list to shuffle.</param>
        public static void Shuffle<T>(this IList<T> ts)
        {
            int count = ts.Count;
            int last = count - 1;
            for (int i = 0; i < last; ++i)
            {
                int r = UnityEngine.Random.Range(i, count);
                (ts[i], ts[r]) = (ts[r], ts[i]);
            }
        }

        /// <summary>
        /// Removes elements from the list that are present in the removeableItemList.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to remove elements from.</param>
        /// <param name="removeableItemList">The list of items to remove from the list.</param> 
        public static void RemoveElements<T>(this List<T> list, List<T> removeableItemList)
        {
            foreach (T removeableItem in removeableItemList.Where(list.Contains)) list.Remove(removeableItem);
        }

        /// <summary>
        /// Gets a random element from the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to get a random element from.</param>
        /// <returns>A random element from the list.</returns>
        public static T GetRandom<T>(this List<T> list)
        {
            if (list.IsNullOrEmpty()) return default;
            int index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        /// <summary>
        /// Logs the elements of the list to the console.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to log the elements of.</param>
        public static void DebugElements<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count; i++) Debug.Log($"Element {i}: {list[i]}");
        }
    }
}