using NF.Common.ObjectPool;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NF.Collections.Generic
{
    public class LinkedList<T> : ICollection<T>, ICollection, IReadOnlyCollection<T>
    {
        public int Count { get; private set; }

        bool ICollection<T>.IsReadOnly => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        ObjectPool<LinkedListNode<T>> mPool = new ObjectPool<LinkedListNode<T>>();

        LinkedListNode<T> mHead;
        LinkedListNode<T> mTail;

        public LinkedList()
        {

        }

        public LinkedList(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                AddTail(item);
            }
        }

        public void Add(T item)
        {
            AddTail(item);
        }

        public void AddHead(T item)
        {
            mPool.TryTake(out var node);
            node.Init(item);
            node.Next = mHead;

            if (mHead != null)
            {
                mHead.Prev = node;
            }

            mHead = node;

            if (mTail == null)
            {
                mTail = mHead;
            }
            Count++;
        }

        public LinkedListNode<T> GetHeadNode()
        {
            return mHead;
        }

        public void AddTail(T item)
        {
            mPool.TryTake(out var node);
            node.Init(item);
            node.Prev = mTail;

            if (mTail != null)
            {
                mTail.Next = node;
            }

            mTail = node;

            if (mHead == null)
            {
                mHead = mTail;
            }
            Count++;
        }

        public bool TryRemoveHead(out T val)
        {
            if (mHead == null)
            {
                val = default(T);
                return false;
            }

            val = mHead.Item;
            var next = mHead.Next;
            mPool.Return(mHead);
            mHead = next;

            if (mHead == null)
            {
                mTail = null;
            }
            Count--;
            return true;
        }

        public bool TryRemoveTail(out T val)
        {
            if (mTail == null)
            {
                val = default(T);
                return false;
            }

            val = mTail.Item;
            var prev = mTail.Prev;
            mPool.Return(mTail);
            mTail = prev;

            if (mTail == null)
            {
                mHead = null;
            }
            Count--;
            return true;
        }

        public void Clear()
        {
            while (mHead != null)
            {
                var next = mHead.Next;
                mPool.Return(mHead);
                mHead = next;
            }
            mTail = null;
            Count = 0;
        }

        public bool Contains(T item)
        {
            var curr = mHead;
            while (curr != null)
            {
                if (curr.Item.Equals(item))
                {
                    return true;
                }
                curr = curr.Next;
            }
            return false;
        }

        public void CopyTo(T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, "Non-negative number required.");
            }

            if (index > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, "Must be less than or equal to the size of the collection.");
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException("Insufficient space in the target location to copy the information.");
            }

            var curr = mHead;
            while (curr != null)
            {
                array[index++] = curr.Item;
                curr = curr.Next;
            }
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException("Only single dimensional arrays are supported for the requested action.", nameof(array));
            }

            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("The lower bound of target array must be zero.", nameof(array));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, "Non-negative number required.");
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException("Insufficient space in the target location to copy the information.");
            }

            if (array is T[] tArray)
            {
                CopyTo(tArray, index);
            }
            else
            {
                if (!(array is object[] objects))
                {
                    throw new ArgumentException("Target array type is not compatible with the type of items in the collection.", nameof(array));
                }

                var curr = mHead;
                try
                {

                    while (curr != null)
                    {
                        objects[index++] = curr.Item;
                        curr = curr.Next;
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException("Target array type is not compatible with the type of items in the collection.", nameof(array));
                }
            }
        }

        public bool Remove(T item)
        {
            var curr = mHead;
            while (curr != null)
            {
                if (curr.Item.Equals(item))
                {
                    if (curr == mHead)
                    {
                        mHead = mHead.Next;
                        if (mHead == null)
                        {
                            mTail = null;
                        }
                    }
                    else if (curr == mTail)
                    {
                        mTail = mTail.Prev;
                        if (mTail == null)
                        {
                            mHead = null;
                        }
                    }
                    else
                    {
                        curr.Prev.Next = curr.Next;
                    }

                    mPool.Return(curr);
                    Count--;
                    return true;
                }
                curr = curr.Next;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Enumerator
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            LinkedList<T> mList;
            int mIndex;
            LinkedListNode<T> mNode;

            public T Current { get; private set; }

            object IEnumerator.Current
            {
                get
                {
                    if (mIndex == 0 || (mIndex == mList.Count + 1))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    return Current;
                }
            }

            public Enumerator(LinkedList<T> list)
            {
                mList = list;
                mNode = list.mHead;
                Current = default;
                mIndex = 0;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (mNode == null)
                {
                    mIndex = mList.Count + 1;
                    return false;
                }

                mIndex++;
                Current = mNode.Item;
                mNode = mNode.Next;
                return true;
            }

            public void Reset()
            {
                Current = default;
                mIndex = 0;
            }
        }
        #endregion Enumerator
    }


    #region Node
    public sealed class LinkedListNode<T>
    {
        public LinkedListNode<T> Next { get; set; }
        public LinkedListNode<T> Prev { get; set; }
        public T Item { get; private set; }

        public LinkedListNode()
        {
        }

        public void Init(T item)
        {
            Next = null;
            Prev = null;
            Item = item;
        }
    }
    #endregion Node
}