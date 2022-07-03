/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-03-04
 * @last updated      : 2021-03-09
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Buffers;
using System.Threading;

namespace Argus.Collections
{
    /// <summary>
    /// A thread safe list that implements locking via <see cref="ReaderWriterLockSlim"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreadSafeList<T> : IList<T>
    {
        /// <summary>
        /// The internal list that is protected by the <see cref="ReaderWriterLockSlim"/>
        /// </summary>
        private List<T> _list;

        /// <summary>
        /// The lock mechanism with support for recursion which allows <see cref="GetEnumerator"/> to be called without
        /// a <see cref="LockRecursionException"/> being thrown.
        /// </summary>
        private ReaderWriterLockSlim _lock = new(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreadSafeList()
        {
            _list = new List<T>();
        }

        /// <inheritdoc />
        public T this[int index]
        {
            get
            {
                _lock.EnterReadLock();

                try
                {
                    return _list[index];
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
            set => this.Add(value);
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            int count;
            var pool = ArrayPool<T>.Shared;
            T[] snapshot;

            // We only need the lock while we're creating the temporary snapshot, once
            // that's done we can release and then allow the enumeration to continue.  We
            // will get the count after the lock and then use it.
            try
            {
                _lock.EnterUpgradeableReadLock();

                count = this.Count;
                snapshot = pool.Rent(count);

                for (int i = 0; i < count; i++)
                {
                    snapshot[i] = _list[i];
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }

            // Since the array returned from the pool could be larger than we requested
            // we will use the saved count to only iterate over the items we know to be
            // in the range of the ones we requested.
            for (int i = 0; i < count; i++)
            {
                yield return snapshot[i];
            }

            pool.Return(snapshot, true);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            _lock.EnterWriteLock();

            try
            {
                _list.Add(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds the elements of the specified <see cref="IEnumerable"/> to the end of this <see cref="ThreadSafeList{T}"/>.
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<T> items)
        {
            _lock.EnterWriteLock();

            try
            {
                _list.AddRange(items);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            _lock.EnterWriteLock();

            try
            {
                _list.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                _lock.EnterReadLock();

                try
                {
                    return _list.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            _lock.EnterReadLock();

            try
            {
                return _list.Contains(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            _lock.EnterWriteLock();

            try
            {
                _list.CopyTo(array, arrayIndex);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Implements a comparable version of the Linq Find with read locking which searches via index and
        /// not IEnumerable and can offer performance improvements over FirstOrDefault when searching in
        /// memory objects.
        /// </summary>
        /// <param name="match"></param>
        public T Find(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("Predicate cannot be null.");
            }

            _lock.EnterReadLock();

            try
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (match(this[i]))
                    {
                        return this[i];
                    }
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return default;
        }

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            _lock.EnterReadLock();

            try
            {
                return _list.IndexOf(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            _lock.EnterWriteLock();

            try
            {
                _list.Insert(index, item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            _lock.EnterWriteLock();

            try
            {
                return _list.Remove(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            _lock.EnterWriteLock();

            try
            {
                _list.RemoveAt(index);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the range of elements from this <see cref="ThreadSafeList{T}"/>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public void RemoveRange(int index, int count)
        {
            _lock.EnterWriteLock();

            try
            {
                _list.RemoveRange(index, count);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}