using System.Collections.Concurrent;
using System.Threading;

namespace NF.Common.ObjectPool
{
    public class ObjectPoolConcurrent<T> : IObjectPool<T> where T : new()
    {
        ConcurrentBag<T> mBag = new ConcurrentBag<T>();
        volatile int mAllocatedSize = 0;

        public int UsedSize => mAllocatedSize;
        public int CachedSize => mBag.Count;

        public bool Return(T t)
        {
            mBag.Add(t);
            Interlocked.Decrement(ref mAllocatedSize);
            return true;
        }

        public bool TryTake(out T t)
        {
            if (!mBag.TryTake(out t))
            {
                t = new T();
            }

            Interlocked.Increment(ref mAllocatedSize);
            return true;
        }

        public ObjectPoolConcurrent(int initialBlockCount)
        {
            for (int i = 0; i < initialBlockCount; ++i)
            {
                mBag.Add(new T());
            }
        }
    }
}