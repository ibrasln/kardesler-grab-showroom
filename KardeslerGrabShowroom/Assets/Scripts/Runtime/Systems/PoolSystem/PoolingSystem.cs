using System;
using System.Collections.Generic;
using UnityEngine;
using IboshEngine.Runtime.Utilities.Singleton;
using Cysharp.Threading.Tasks;

namespace IboshEngine.Runtime.Systems.PoolSystem
{
    /// <summary>
    /// Singleton class for managing object pooling.
    /// </summary>
    [DefaultExecutionOrder(-15)]
    public class PoolingSystem : IboshSingleton<PoolingSystem>
    {
        private PoolData _poolData;
        [SerializeField] private Canvas canvas;
        private readonly Dictionary<PoolObjectType, Queue<GameObject>> _pools = new();
        private readonly Dictionary<PoolObjectType, Transform> _poolParents = new();

        private const int DEFAULT_POOL_SIZE = 10;

        #region Built-In
        protected override void Awake()
        {
            base.Awake();
            _poolData = Resources.Load<PoolData>("Pools/PoolData");

            InitializePools();
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the pools.
        /// </summary>
        private void InitializePools()
        {
            _poolData.Initialize();

            foreach (var category in _poolData.AllCategories)
            {
                foreach (var sourceObject in category.Objects)
                {
                    InitializePool(sourceObject);
                }
            }
        }

        /// <summary>
        /// Initializes a pool for a given source object.
        /// </summary>
        /// <param name="sourceObject">The source object to initialize the pool for.</param>
        private void InitializePool(SourceObjects sourceObject)
        {
            if (sourceObject.SourcePrefab == null)
            {
                Debug.LogError($"Source prefab is missing for pool type: {sourceObject.Type}");
                return;
            }

            _pools[sourceObject.Type] = new Queue<GameObject>();

            var parent = new GameObject($"Pool_{sourceObject.Type}");
            parent.transform.SetParent(transform);
            _poolParents[sourceObject.Type] = parent.transform;

            int initialCount = Mathf.Max(sourceObject.MinNumberOfObject, DEFAULT_POOL_SIZE);
            PrewarmPool(sourceObject, initialCount);
        }
        #endregion

        /// <summary>
        /// Prewarms a pool for a given source object.
        /// </summary>
        /// <param name="sourceObject">The source object to prewarm the pool for.</param>
        /// <param name="count">The number of objects to prewarm.</param>
        private void PrewarmPool(SourceObjects sourceObject, int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateInstance(sourceObject);
            }
        }

        /// <summary>
        /// Creates an instance of a given source object.
        /// </summary>
        /// <param name="sourceObject">The source object to create an instance of.</param>
        /// <returns>The created instance.</returns>
        private GameObject CreateInstance(SourceObjects sourceObject)
        {
            if (!_poolParents.TryGetValue(sourceObject.Type, out Transform parent))
            {
                Debug.LogError($"Pool parent not found for type: {sourceObject.Type}");
                return null;
            }

            GameObject instance = Instantiate(sourceObject.SourcePrefab, parent);
            instance.SetActive(false);

            if (sourceObject.AutoDestroy)
            {
                instance.AddComponent<PoolObject>();
            }

            _pools[sourceObject.Type].Enqueue(instance);
            return instance;
        }

        /// <summary>
        /// Pulls an instance of a given type.
        /// </summary>
        /// <param name="type">The type of the object to pull.</param>
        /// <param name="autoPushDelay">The delay before the object is pushed back to the pool.</param>
        /// <param name="isUIPool">Whether the object is a UI pool.</param>
        public GameObject Pull(PoolObjectType type, float? autoPushDelay = null, bool isUIPool = false, bool isLocalPool = false)
        {
            var instance = PullInternal(type, null, null, isUIPool, isLocalPool);
            if (instance != null && autoPushDelay.HasValue)
            {
                AutoPush(instance, type, autoPushDelay.Value);
            }
            return instance;
        }

        /// <summary>
        /// Pulls an instance of a given type at a specific position.
        /// </summary>
        /// <param name="type">The type of the object to pull.</param>
        /// <param name="position">The position to pull the object to.</param>
        /// <param name="autoPushDelay">The delay before the object is pushed back to the pool.</param>
        public GameObject Pull(PoolObjectType type, Vector3 position, float? autoPushDelay = null, bool isUIPool = false, bool isLocalPool = false)
        {
            var instance = PullInternal(type, null, position, isUIPool, isLocalPool);
            if (instance != null && autoPushDelay.HasValue)
            {
                AutoPush(instance, type, autoPushDelay.Value);
            }
            return instance;
        }

        /// <summary>
        /// Pulls an instance of a given type at a specific parent.
        /// </summary>
        /// <param name="type">The type of the object to pull.</param>
        /// <param name="parent">The parent to pull the object to.</param>
        /// <param name="autoPushDelay">The delay before the object is pushed back to the pool.</param>
        public GameObject Pull(PoolObjectType type, Transform parent, float? autoPushDelay = null, bool isUIPool = false, bool isLocalPool = false)
        {
            var instance = PullInternal(type, parent, null, isUIPool, isLocalPool);
            if (instance != null && autoPushDelay.HasValue)
            {
                AutoPush(instance, type, autoPushDelay.Value);
            }
            return instance;
        }

        /// <summary>
        /// Pulls an instance of a given type at a specific position and parent.
        /// </summary>
        /// <param name="type">The type of the object to pull.</param>
        /// <param name="position">The position to pull the object to.</param>
        /// <param name="parent">The parent to pull the object to.</param>
        /// <param name="autoPushDelay">The delay before the object is pushed back to the pool.</param>
        public GameObject Pull(PoolObjectType type, Vector3 position, Transform parent, float? autoPushDelay = null, bool isUIPool = false, bool isLocalPool = false)
        {
            var instance = PullInternal(type, parent, position, isUIPool, isLocalPool);
            if (instance != null && autoPushDelay.HasValue)
            {
                AutoPush(instance, type, autoPushDelay.Value);
            }
            return instance;
        }

        /// <summary>
        /// Pulls an instance of a given type.
        /// </summary>
        /// <typeparam name="T">The type of the object to pull.</typeparam>
        /// <param name="type">The type of the object to pull.</param>
        /// <param name="autoPushDelay">The delay before the object is pushed back to the pool.</param> 
        public T Pull<T>(PoolObjectType type, float? autoPushDelay = null, bool isUIPool = false, bool isLocalPool = false) where T : Component
        {
            GameObject go = Pull(type, autoPushDelay, isUIPool, isLocalPool);
            return go != null ? go.GetComponent<T>() : null;
        }

        /// <summary>
        /// Pulls an instance of a given type at a specific position.
        /// </summary>
        /// <typeparam name="T">The type of the object to pull.</typeparam>
        /// <param name="type">The type of the object to pull.</param>
        /// <param name="position">The position to pull the object to.</param>
        public T Pull<T>(PoolObjectType type, Vector3 position, float? autoPushDelay = null, bool isUIPool = false, bool isLocalPool = false) where T : Component
        {
            GameObject go = Pull(type, position, autoPushDelay, isUIPool, isLocalPool);
            return go != null ? go.GetComponent<T>() : null;
        }

        /// <summary>
        /// Pulls an instance of a given type at a specific parent.
        /// </summary>
        /// <typeparam name="T">The type of the object to pull.</typeparam>
        /// <param name="type">The type of the object to pull.</param>
        /// <param name="parent">The parent to pull the object to.</param>
        /// <param name="autoPushDelay">The delay before the object is pushed back to the pool.</param>
        public T Pull<T>(PoolObjectType type, Transform parent, float? autoPushDelay = null, bool isUIPool = false, bool isLocalPool = false) where T : Component
        {
            GameObject go = Pull(type, parent, autoPushDelay, isUIPool, isLocalPool);
            return go != null ? go.GetComponent<T>() : null;
        }

        /// <summary>
        /// Pulls an instance of a given type at a specific position and parent.
        /// </summary>
        /// <typeparam name="T">The type of the object to pull.</typeparam>
        /// <param name="type">The type of the object to pull.</param>
        /// <param name="position">The position to pull the object to.</param>
        /// <param name="parent">The parent to pull the object to.</param>
        public T Pull<T>(PoolObjectType type, Vector3 position, Transform parent, float? autoPushDelay = null, bool isUIPool = false, bool isLocalPool = false) where T : Component
        {
            GameObject go = Pull(type, position, parent, autoPushDelay, isUIPool, isLocalPool);
            return go != null ? go.GetComponent<T>() : null;
        }

        /// <summary>
        /// Pulls an instance of a given type.
        /// </summary>
        /// <param name="type">The type of the object to pull.</param>
        /// <param name="parent">The parent to pull the object to.</param>
        /// <param name="position">The position to pull the object to.</param>
        /// <param name="isUIPool">Whether the object is a UI pool.</param>
        private GameObject PullInternal(PoolObjectType type, Transform parent = null, Vector3? position = null, bool isUIPool = false, bool isLocalPool = false)
        {
            if (!_pools.TryGetValue(type, out Queue<GameObject> pool))
            {
                Debug.LogWarning($"Pool not found for type: {type}");
                return null;
            }

            GameObject instance = GetInactiveObject(pool);

            if (instance == null)
            {
                var sourceObject = _poolData.GetSourceObject(type);
                if (sourceObject?.AllowGrow == true)
                {
                    instance = CreateInstance(sourceObject);
                }
                else
                {
                    Debug.LogWarning($"Pool depleted for type: {type} and growth is not allowed");
                    return null;
                }
            }

            SetupPulledInstance(instance, parent, position, isUIPool, isLocalPool);
            return instance;
        }

        /// <summary>
        /// Gets an inactive object from the pool.
        /// </summary>
        /// <param name="pool">The pool to get the inactive object from.</param>
        /// <returns>The inactive object.</returns>
        private GameObject GetInactiveObject(Queue<GameObject> pool)
        {
            while (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                if (obj != null && !obj.activeInHierarchy)
                {
                    return obj;
                }
            }
            return null;
        }

        /// <summary>
        /// Sets up a pulled instance.
        /// </summary>
        /// <param name="instance">The instance to set up.</param>
        /// <param name="parent">The parent to set up the instance to.</param>
        /// <param name="position">The position to set up the instance to.</param>
        private void SetupPulledInstance(GameObject instance, Transform parent, Vector3? position, bool isUIPool, bool isLocalPool)
        {
            if (instance == null) return;

            if (isUIPool && canvas != null)
            {
                instance.transform.SetParent(canvas.transform, false);
            }

            if (parent != null)
            {
                instance.transform.SetParent(parent, isLocalPool ? false : true);
            }

            if (position.HasValue)
            {
                if (isLocalPool)
                {
                    instance.transform.localPosition = position.Value;
                }
                else
                {
                    instance.transform.position = position.Value;
                }
            }

            instance.transform.localScale = Vector3.one;

            instance.SetActive(true);

            if (instance.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.Initialize();
            }
        }

        /// <summary>
        /// Pushes an instance of a given type back to the pool.
        /// </summary>
        /// <param name="instance">The instance to push back to the pool.</param>
        /// <param name="type">The type of the object to push back to the pool.</param>
        /// <param name="isLocalPool">Whether the object is a local pool.</param>
        public void Push(GameObject instance, PoolObjectType type, bool isLocalPool = false)
        {
            if (instance == null) return;

            if (!_pools.ContainsKey(type))
            {
                Debug.LogError($"Attempting to push object to non-existent pool: {type}");
                return;
            }

            if (instance.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.Dispose();
            }

            if (_poolParents.TryGetValue(type, out Transform parent))
            {
                instance.transform.SetParent(parent);
                if (isLocalPool)
                {
                    instance.transform.localPosition = Vector3.zero;
                }
                else
                {
                    instance.transform.position = Vector3.zero;
                }
                instance.transform.localRotation = Quaternion.identity;
            }

            instance.SetActive(false);
            _pools[type].Enqueue(instance);
        }

        /// <summary>
        /// Pushes an instance of a given type back to the pool after a delay.
        /// </summary>
        /// <param name="instance">The instance to push back to the pool.</param>
        /// <param name="type">The type of the object to push back to the pool.</param>
        /// <param name="delay">The delay before the object is pushed back to the pool.</param>
        private async void AutoPush(GameObject instance, PoolObjectType type, float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            Push(instance, type);
        }
    }
}