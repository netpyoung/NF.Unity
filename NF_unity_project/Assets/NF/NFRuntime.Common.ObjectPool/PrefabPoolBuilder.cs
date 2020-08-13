using System.Data;
using UnityEngine;

namespace NFRuntime.Common.ObjectPool
{
    public class PrefabPoolBuilder<T> where T : Component
    {
        PrefabPoolConfig mConfig = new PrefabPoolConfig();

        public PrefabPoolBuilder<T> Register(GameObject obj)
        {
            mConfig.RegisteredGameObject = obj;
            return this;
        }

        public PrefabPoolBuilder<T> SetInitialCacheSize(int initialCacheSize)
        {
            mConfig.InitialCacheSize = initialCacheSize;
            return this;
        }

        public PrefabPoolBuilder<T> SetInitialRootName(string rootName)
        {
            mConfig.RootName = rootName;
            return this;
        }

        public PrefabPoolBuilder<T> SetNumbering(bool isNumbering)
        {
            mConfig.IsNumbering = isNumbering;
            return this;
        }
        public PrefabPoolBuilder<T> SetInspector(bool isUsingInspector)
        {
            mConfig.IsUsingInspector = isUsingInspector;
            return this;
        }

        public PrefabPool<T> Build()
        {
            if (mConfig.RegisteredGameObject == null)
            {
                throw new DataException("RegisteredGameObject is null");
            }

            return new PrefabPool<T>(mConfig);
        }
    }
}