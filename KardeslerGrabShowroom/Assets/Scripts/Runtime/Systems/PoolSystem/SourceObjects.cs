using UnityEngine;
using Sirenix.OdinInspector;

namespace IboshEngine.Runtime.Systems.PoolSystem
{
    /// <summary>
    /// A class representing a source object for a pool.
    /// </summary>
    [System.Serializable]
    public class SourceObjects
    {
        public PoolObjectType Type;
        [AssetsOnly] public GameObject SourcePrefab;
        [MinValue(0)] public int MinNumberOfObject = 0;
        public bool AllowGrow = true;
        public bool AutoDestroy = true;
    }
}