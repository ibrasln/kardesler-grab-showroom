using System.Collections.Generic;
using UnityEngine;

namespace IboshEngine.Runtime.Utilities.Extensions
{
    /// <summary>
    /// Extension methods for collections.
    /// </summary>
    public static class CollectionExtensions
    {
        public static T GetRandom<T>(this ICollection<T> collection)
        {
            if (collection == null)
                return default;
            int t = Random.Range(0, collection.Count);
            foreach (T element in collection)
            {
                if (t == 0)
                    return element;
                t--;
            }

            return default;
        }
    }

}