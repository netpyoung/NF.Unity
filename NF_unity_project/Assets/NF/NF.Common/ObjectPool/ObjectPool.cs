using System.Collections.Generic;

namespace NF.Common.ObjectPool
{
    public class ObjectPool<T> : IObjectPool<T> where T : new()
    {
        Stack<T> mStack = new Stack<T>();

        public int UsedSize { get; private set; }
        public int CachedSize => mStack.Count;

        public bool Return(T t)
        {
            mStack.Push(t);
            UsedSize--;
            return true;
        }

        public bool TryTake(out T t)
        {
            if (mStack.Count > 0)
            {
                t = mStack.Pop();
            }
            else
            {
                t = new T();
            }

            UsedSize++;
            return true;
        }

        public ObjectPool()
        {
        }

        public ObjectPool(int initialBlockCount)
        {
            Init(initialBlockCount);
        }

        public void Init(int initialBlockCount)
        {
            mStack.Clear();
            for (int i = 0; i < initialBlockCount; ++i)
            {
                mStack.Push(new T());
            }
            UsedSize = initialBlockCount;
        }
    }
}