using System;
using System.Collections.Concurrent;
using System.Linq;
using UnityEngine;

namespace NFRuntime.ObjectPool
{
    public class PrefabPool<T> : IGameObjectPool<T>, IDisposable where T : Component
    {
        public Transform RootGameObjectTransform { get; private set; }
        public PrefabPoolConfig Config { get; private set; }
        public int UsedSize => mSpawned.Count;
        public int CachedSize => mPool.Count;

        ConcurrentStack<T> mPool = new ConcurrentStack<T>();

        // TODO(pyoung): ConcurrentSet<T>
        // TOOD(pyoung): MaxCapacity
        ConcurrentDictionary<T, T> mSpawned = new ConcurrentDictionary<T, T>();

        int mNumber = 0;

        public PrefabPool(PrefabPoolConfig config)
        {
            this.Config = config;

            Clear();

            if (string.IsNullOrEmpty(config.RootName))
            {
                config.RootName = $"{nameof(PrefabPool<T>)} - {typeof(T)}";
            }
            GameObject go = new GameObject(config.RootName);
            RootGameObjectTransform = go.transform;
            if (config.IsUsingInspector)
            {
                go.AddComponent<GameObjectPoolInspector>().pool = this;
            }

            for (int i = 0; i < config.InitialCacheSize; ++i)
            {
                mPool.Push(SpawnComponent());
            }
        }

        public void Clear()
        {
            var keys = mSpawned.Keys.ToArray();
            foreach(var key in keys)
            {
                while (mSpawned.TryRemove(key, out _))
                {
                    UnityEngine.Object.Destroy(key.gameObject);
                }
            }

            while (mPool.TryPop(out var t))
            {
                UnityEngine.Object.Destroy(t.gameObject);
            }
        }

        public bool Return(T t)
        {
            if (!mSpawned.TryRemove(t, out _))
            {
                throw new Exception($"{t} is not spawned");
            }

            t.transform.SetParent(RootGameObjectTransform, true);
            t.gameObject.SetActive(false);
            mPool.Push(t);
            return true;
        }

        public bool TryTake(out T t)
        {
            if (mPool.Count == 0)
            {
                t = SpawnComponent();
            }
            else
            {
                if (!mPool.TryPop(out t))
                {
                    t = SpawnComponent();
                }
            }

            if (Config.IsNumbering)
            {
                mNumber++;
                t.gameObject.name = $"{typeof(T)} - {mNumber}";
            }

            mSpawned.TryAdd(t, t);
            return true;
        }

        T SpawnComponent()
        {
            var t = UnityEngine.Object.Instantiate(Config.RegisteredGameObject).AddComponent<T>();
            t.transform.SetParent(RootGameObjectTransform, true);
            t.gameObject.name = $"{typeof(T)}";
            t.gameObject.SetActive(false);
            return t;
        }

        public void Dispose()
        {
            Clear();
            if (this.RootGameObjectTransform != null)
            {
                UnityEngine.Object.Destroy(this.RootGameObjectTransform.gameObject);
            }
        }
    }
}