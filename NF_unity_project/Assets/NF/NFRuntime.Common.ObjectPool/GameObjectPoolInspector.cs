using UnityEngine;

namespace NFRuntime.Common.ObjectPool
{
    public class GameObjectPoolInspector : MonoBehaviour
    {
        public IGameObjectPool pool;

        public int UsedSize => pool.UsedSize;

        public int CachedSize => pool.CachedSize;
    }
}