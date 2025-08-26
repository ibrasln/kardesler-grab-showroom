namespace IboshEngine.Runtime.Systems.PoolSystem
{
    /// <summary>
    /// Interface for objects that can be pooled.
    /// </summary>
    public interface IPoolable
    {
        void Initialize();
        void Dispose();
    }
}