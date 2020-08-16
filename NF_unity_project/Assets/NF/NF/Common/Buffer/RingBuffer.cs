using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NF.Common.Buffer
{
    public class RingBuffer : IDisposable
    {
        public RingBuffer(int capacity)
        {
            Init(capacity);
        }

        ~RingBuffer()
        {
            Dispose(false);
        }

        public void Init(int capacity)
        {
            byte[] buffer = new byte[capacity + 1];
            mBuffer = buffer;
            mBufferMemory = buffer;
            mSegment = new ArraySegment<byte>(buffer, 0, capacity + 1);
            Capacity = capacity;
            UsedSize = 0;
            mWriteIndex = 0;
            mReadIndex = 0;
        }

        public void ClearBuffer()
        {
            UsedSize = 0;
            mWriteIndex = 0;
            mReadIndex = 0;
        }

        public int Enqueue(ReadOnlySpan<byte> span)
        {
            int size = span.Length;
            if (size == 0)
            {
                return 0;
            }

            int available = Math.Min(size, Capacity - UsedSize);
            if (available <= Capacity - mWriteIndex)
            {
                span.Slice(0, available).CopyTo(mBufferMemory.Slice(mWriteIndex, size).Span);
                mWriteIndex += available;
                if (mWriteIndex == Capacity)
                {
                    mWriteIndex = 0;
                }
            }
            else
            {
                int fst = Capacity - mWriteIndex;
                int snd = available - fst;
                span.Slice(0, fst).CopyTo(mBufferMemory.Slice(mWriteIndex, fst).Span);
                span.Slice(fst, snd).CopyTo(mBufferMemory.Slice(0, snd).Span);
                mWriteIndex = snd;
            }
            UsedSize += available;
            return available;
        }

        public int Enqueue(byte[] byteArray) => Enqueue(new ReadOnlySpan<byte>(byteArray));

        public int Enqueue(ReadOnlyMemory<byte> memory) => Enqueue(memory, memory.Length);

        public int Enqueue(ReadOnlyMemory<byte> memory, int size)
        {
            if (size == 0)
            {
                return 0;
            }

            int available = Math.Min(size, Capacity - UsedSize);
            if (available <= Capacity - mWriteIndex)
            {
                memory.Slice(0, available).CopyTo(mBufferMemory.Slice(mWriteIndex));
                mWriteIndex += available;
                if (mWriteIndex == Capacity)
                {
                    mWriteIndex = 0;
                }
            }
            else
            {
                int fst = Capacity - mWriteIndex;
                int snd = available - fst;
                memory.Slice(0, fst).CopyTo(mBufferMemory.Slice(mWriteIndex));
                memory.Slice(fst, snd).CopyTo(mBufferMemory);
                mWriteIndex = snd;
            }
            UsedSize += available;
            return available;
        }

        public int Dequeue(Memory<byte> outBytes, int size)
        {
            if (size == 0)
            {
                return 0;
            }

            int available = Math.Min(size, UsedSize);
            if (available <= Capacity - mReadIndex)
            {
                mBufferMemory.Slice(mReadIndex, available).CopyTo(outBytes);
                mReadIndex += available;
                if (mReadIndex == Capacity)
                {
                    mReadIndex = 0;
                }
            }
            else
            {
                int fst = Capacity - mReadIndex;
                int snd = available - fst;
                mBufferMemory.Slice(mReadIndex, fst).CopyTo(outBytes);
                mBufferMemory.Slice(0, snd).CopyTo(outBytes.Slice(fst));
                mReadIndex = snd;
            }

            UsedSize -= available;
            return available;
        }

        public int Peek(Memory<byte> outBytes, int size)
        {
            if (size == 0)
            {
                return 0;
            }

            int available = Math.Min(size, UsedSize);
            if (available <= Capacity - mReadIndex)
            {
                mBufferMemory.Slice(mReadIndex, available).CopyTo(outBytes);
            }
            else
            {
                int fst = Capacity - mReadIndex;
                int snd = available - fst;
                mBufferMemory.Slice(mReadIndex, fst).CopyTo(outBytes);
                mBufferMemory.Slice(0, snd).CopyTo(outBytes.Slice(fst));
            }

            return available;
        }

        public int Peek<T>(ref T t) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));

            if (size == 0)
            {
                return 0;
            }
            unsafe
            {
                //Span<T> valSpan = MemoryMarshal.CreateSpan(ref t, 1); // dotnet standard 2.1
                Span<T> valSpan = new Span<T>(Unsafe.AsPointer(ref t), 1);

                Span<byte> x = MemoryMarshal.Cast<T, byte>(valSpan);

                int available = Math.Min(size, UsedSize);
                if (available <= Capacity - mReadIndex)
                {
                    mBufferMemory.Slice(mReadIndex, available).Span.CopyTo(x);
                }
                else
                {
                    int fst = Capacity - mReadIndex;
                    int snd = available - fst;
                    mBufferMemory.Slice(mReadIndex, fst).Span.CopyTo(x);
                    mBufferMemory.Slice(0, snd).Span.CopyTo(x.Slice(fst));
                }
                return available;
            }
        }

        public int MoveWrite(int size)
        {
            if (size == 0)
            {
                return 0;
            }
            int available = Math.Min(size, Capacity - UsedSize);
            if (available <= Capacity - mWriteIndex)
            {
                mWriteIndex += available;
                if (mWriteIndex == Capacity)
                {
                    mWriteIndex = 0;
                }
            }
            else
            {
                mWriteIndex = available - (Capacity - mWriteIndex);
            }
            UsedSize += available;
            return available;
        }

        public int MoveRead(int size)
        {
            if (size == 0)
            {
                return 0;
            }
            int available = Math.Min(size, UsedSize);
            if (available <= Capacity - mReadIndex)
            {
                mReadIndex += available;
                if (mReadIndex == Capacity)
                {
                    mReadIndex = 0;
                }
            }
            else
            {
                mReadIndex = available - (Capacity - mReadIndex);
            }

            UsedSize -= available;
            return available;
        }

        #region IDisposable
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ClearBuffer();
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion // IDisposable

        #region property
        public int Capacity { get; private set; }
        public int UsedSize { get; private set; }
        public int EnqueueableSize => Capacity - UsedSize;
        public int DirectEnqueueSize
        {
            get
            {
                if (mReadIndex <= mWriteIndex)
                {
                    return Capacity - mWriteIndex;
                }
                else
                {
                    return mReadIndex - mWriteIndex;
                }
            }
        }

        public int DirectDequeueSize
        {
            get
            {
                if (mReadIndex <= mWriteIndex)
                {
                    return mWriteIndex - mReadIndex;
                }
                else
                {
                    return Capacity - mReadIndex;
                }
            }
        }
        public Memory<byte> ReadBufferMemory => mBufferMemory.Slice(mReadIndex);
        public Memory<byte> WriteBufferMemory => mBufferMemory.Slice(mWriteIndex);

        //public ArraySegment<byte> SegmentFromWrite(int size) => Segment.Slice(mWriteIndex, size); // dotnet standard2.1
        //public ArraySegment<byte> SegmentFromBegin(int size) => Segment.Slice(0, size); // dotnet standard2.1
        public ArraySegment<byte> SegmentFromWrite(int size) => new ArraySegment<byte>(mBuffer, mWriteIndex, size);
        public ArraySegment<byte> SegmentFromBegin(int size) => new ArraySegment<byte>(mBuffer, 0, size);
        #endregion // property


        #region member
        private int mWriteIndex;
        private int mReadIndex;
        private byte[] mBuffer;
        private Memory<byte> mBufferMemory;
        ArraySegment<byte> mSegment;
        #endregion // member
    }
}
