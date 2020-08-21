using UnityEngine;

namespace NFRuntime.ObjectPool
{
    public interface IGameObjectPool
    {
        int UsedSize { get; }
        int CachedSize { get; }
        void Clear();
    }

    public interface IGameObjectPool<T> : IGameObjectPool where T : Component
    {
        bool TryTake(out T t);
        bool Return(T t);
    }
}