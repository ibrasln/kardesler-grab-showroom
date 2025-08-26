using IboshEngine.Runtime.Utilities.Debugger;
using UnityEngine;

namespace IboshEngine.Runtime.Utilities.Singleton
{
    /// <summary>
    ///     A generic singleton class for MonoBehaviour components in Unity.
    /// </summary>
    /// <remarks>
    ///     Ensures that only one instance of a MonoBehaviour-derived class exists at any time.
    ///     Useful for managing global game states or services that need to be accessed from multiple parts of the game.
    /// </remarks>
    [DefaultExecutionOrder(-5)]
    public class IboshSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        ///     The single instance of the class.
        /// </summary>
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                IboshDebugger.LogError($"There is an instance of the {typeof(T).FullName}", IboshDebugger.DebugColor.Red);

                if (Instance.gameObject) Destroy(Instance.gameObject);
                Instance = this as T;
            }
        }
    }
}
