using UnityEngine;

namespace NFRuntime.ObjectPool
{
    public class PrefabPoolConfig
    {
        public GameObject RegisteredGameObject;
        public string RootName;
        public int InitialCacheSize;
        public bool IsNumbering;
        public bool IsUsingInspector;
    }
}