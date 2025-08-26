using System.Collections.Generic;
using UnityEngine;

namespace IboshEngine.Runtime.Systems.PoolSystem
{
    /// <summary>
    /// ScriptableObject for managing pool data.
    /// </summary>
    [CreateAssetMenu(fileName = "PoolData", menuName = "Data/Pool")]
    public class PoolData : ScriptableObject
    {
        /// <summary>
        /// Represents a category of pool objects.
        /// </summary>
        [System.Serializable]
        public class PoolCategory
        {
            public PoolCategoryType CategoryType;
            public List<SourceObjects> Objects = new();
        }

        [SerializeField] private List<PoolCategory> _poolCategories = new();

        private Dictionary<PoolCategoryType, List<SourceObjects>> _categoryMap;
        private Dictionary<PoolObjectType, SourceObjects> _objectMap;

        public IEnumerable<PoolCategory> AllCategories => _poolCategories;

        /// <summary>
        /// Initializes the pool data.
        /// </summary>
        public void Initialize()
        {
            _categoryMap = new Dictionary<PoolCategoryType, List<SourceObjects>>();
            _objectMap = new Dictionary<PoolObjectType, SourceObjects>();

            foreach (var category in _poolCategories)
            {
                _categoryMap[category.CategoryType] = category.Objects;

                foreach (var obj in category.Objects)
                {
                    if (obj.SourcePrefab != null)
                    {
                        _objectMap[obj.Type] = obj;
                    }
                    else
                    {
                        Debug.LogError($"Null prefab found in pool data for type: {obj.Type}");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the source object for a given type.
        /// </summary>
        /// <param name="type">The type of the object.</param>
        /// <returns>The source object for the given type.</returns>
        public SourceObjects GetSourceObject(PoolObjectType type)
        {
            return _objectMap.TryGetValue(type, out var sourceObject) ? sourceObject : null;
        }

        /// <summary>
        /// Gets the objects by category.
        /// </summary>
        /// <param name="category">The category of the objects.</param>
        /// <returns>The objects for the given category.</returns>
        public List<SourceObjects> GetObjectsByCategory(PoolCategoryType category)
        {
            return _categoryMap.TryGetValue(category, out var objects) ? objects : null;
        }

        private void OnValidate()
        {
            var allTypes = new HashSet<PoolObjectType>();
            foreach (var category in _poolCategories)
            {
                foreach (var obj in category.Objects)
                {
                    if (!allTypes.Add(obj.Type))
                    {
                        Debug.LogError($"Duplicate pool object type found: {obj.Type}");
                    }
                }
            }
        }
    }
}