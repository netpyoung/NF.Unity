using System;
using System.Collections.Concurrent;
using System.Threading;

namespace NF.Common.ObjectPool
{
    public class ObjectPoolTLS<T> : IObjectPool<T> where T : class, new()
    {
        public const int CHUNK_SIZE = 100;

        ConcurrentBag<ObjectPoolTLSChunk> mBag = new ConcurrentBag<ObjectPoolTLSChunk>();
        ConcurrentDictionary<T, ObjectPoolTLSChunk> mDic = new ConcurrentDictionary<T, ObjectPoolTLSChunk>();
        ThreadLocal<ObjectPoolTLSChunk> mTLSChunk = new ThreadLocal<ObjectPoolTLSChunk>();

        volatile int mUsedSize = 0;
        volatile int mMaxChunkBagSize = 0;

        public int UsedSize => mUsedSize;

        public int CachedSize => mBag.Count * CHUNK_SIZE;
        public int MaxCapacity => mMaxChunkBagSize * CHUNK_SIZE;

        public ObjectPoolTLS(int initialBlockCount)
        {
            for (int i = 0; i < initialBlockCount; ++i)
            {
                mBag.Add(new ObjectPoolTLSChunk());
            }
            mMaxChunkBagSize = initialBlockCount;
        }

        ~ObjectPoolTLS()
        {
            if (mTLSChunk.IsValueCreated)
            {
                mBag.Add(mTLSChunk.Value);
            }
            mDic.Clear();
            mTLSChunk.Dispose();
        }

        public bool TryTake(out T t)
        {
            ObjectPoolTLSChunk chunk;
            if (mTLSChunk.IsValueCreated)
            {
                chunk = mTLSChunk.Value;
            }
            else
            {
                if (mBag.TryTake(out chunk))
                {
                    chunk.Init();
                    mTLSChunk.Value = chunk;
                }
                else
                {
                    Interlocked.Increment(ref mMaxChunkBagSize);
                    chunk = new ObjectPoolTLSChunk();
                    chunk.Init();
                    mTLSChunk.Value = chunk;

                }
            }

            chunk.BufferAllocIndex--;

            if (chunk.BufferAllocIndex == 0)
            {
                ObjectPoolTLSChunk nextChunk;
                if (mBag.TryTake(out nextChunk))
                {
                    nextChunk.Init();
                    mTLSChunk.Value = nextChunk;
                }
                else
                {
                    Interlocked.Increment(ref mMaxChunkBagSize);
                    nextChunk = new ObjectPoolTLSChunk();
                    nextChunk.Init();
                    mTLSChunk.Value = nextChunk;
                }
            }

            t = chunk.BlockBuffer[chunk.BufferAllocIndex];
            mDic[t] = chunk;
            Interlocked.Increment(ref mUsedSize);
            return true;
        }

        public bool Return(T t)
        {
            ObjectPoolTLSChunk chunk;
            if (!mDic.TryRemove(t, out chunk))
            {
                return false;
            }

            int freeCount = Interlocked.Decrement(ref chunk.AvailableFreeCount);
            if (freeCount == 0)
            {
                mBag.Add(chunk);
            }

            Interlocked.Decrement(ref mUsedSize);
            return true;
        }

        class ObjectPoolTLSChunk
        {
            public ObjectPoolTLSChunk()
            {
                for (int i = 0; i < CHUNK_SIZE; ++i)
                {
                    BlockBuffer[i] = new T();
                }
            }

            public void Init()
            {
                BufferAllocIndex = CHUNK_SIZE;
                AvailableFreeCount = CHUNK_SIZE;
            }

            public T[] BlockBuffer { get; } = new T[CHUNK_SIZE];
            public int BufferAllocIndex;
            public volatile int AvailableFreeCount;
        }
    }
}