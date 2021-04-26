///*
// * @author            : Blake Pell
// * @website           : http://www.blakepell.com
// * @initial date      : 2021-03-04
// * @last updated      : 2021-03-19
// * @copyright         : Copyright (c) 2003-2021, All rights reserved.
// * @license           : MIT
// */

//using System;
//using System.Buffers;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;

//namespace Argus.Collections
//{
//    /// <summary>
//    /// A thread safe Dictionary that implements locking via <see cref="ReaderWriterLockSlim"/>.
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public class ThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
//    {
//        /// <summary>
//        /// The internal list that is protected by the <see cref="ReaderWriterLockSlim"/>
//        /// </summary>
//        private Dictionary<TKey, TValue> _list;

//        /// <summary>
//        /// The lock mechanism with support for recursion which allows <see cref="GetEnumerator"/> to be called without
//        /// a <see cref="LockRecursionException"/> being thrown.
//        /// </summary>
//        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        public ThreadSafeDictionary()
//        {
//            _list = new Dictionary<TKey, TValue>();
//        }

//        /// <inheritdoc />
//        public TValue this[TKey index]
//        {
//            get
//            {
//                _lock.EnterReadLock();

//                try
//                {
//                    return _list[index];
//                }
//                finally
//                {
//                    _lock.ExitReadLock();
//                }
//            }
//            set => this.Add(index, value);
//        }

//        /// <inheritdoc />
//        public void Add(TKey key, TValue value)
//        {
//            _lock.EnterWriteLock();

//            try
//            {
//                _list.Add(key, value);
//            }
//            finally
//            {
//                _lock.ExitWriteLock();
//            }
//        }

//        /// <inheritdoc />
//        public void Add(KeyValuePair<TKey, TValue> item)
//        {
//            _lock.EnterWriteLock();

//            try
//            {
//                _list.Add(item.Key, item.Value);
//            }
//            finally
//            {
//                _lock.ExitWriteLock();
//            }
//        }

//        /// <inheritdoc />
//        public bool Remove(KeyValuePair<TKey, TValue> item)
//        {
//            _lock.EnterWriteLock();

//            try
//            {
//                return _list.Remove(item.Key);
//            }
//            finally
//            {
//                _lock.ExitWriteLock();
//            }
//        }

//        /// <inheritdoc />
//        public bool Remove(TKey key)
//        {
//            _lock.EnterWriteLock();

//            try
//            {
//                return _list.Remove(key);
//            }
//            finally
//            {
//                _lock.ExitWriteLock();
//            }
//        }

//        /// <inheritdoc />
//        public bool ContainsKey(TKey key)
//        {
//            _lock.EnterReadLock();

//            try
//            {
//                return _list.ContainsKey(key);
//            }
//            finally
//            {
//                _lock.ExitReadLock();
//            }
//        }

//        /// <inheritdoc />
//        public bool ContainsValue(TValue value)
//        {
//            _lock.EnterReadLock();

//            try
//            {
//                return _list.ContainsValue(value);
//            }
//            finally
//            {
//                _lock.ExitReadLock();
//            }
//        }

//        /// <inheritdoc />
//        public bool TryGetValue(TKey key, out TValue value)
//        {
//            _lock.EnterReadLock();

//            try
//            {
//                return _list.TryGetValue(key, out value);
//            }
//            finally
//            {
//                _lock.ExitReadLock();
//            }
//        }

//        /// <inheritdoc />
//        public void Clear()
//        {
//            _lock.EnterWriteLock();

//            try
//            {
//                _list.Clear();
//            }
//            finally
//            {
//                _lock.ExitWriteLock();
//            }
//        }

//        /// <inheritdoc />
//        public bool Contains(KeyValuePair<TKey, TValue> item)
//        {
//            _lock.EnterReadLock();

//            try
//            {
//                return _list.Contains(item);
//            }
//            finally
//            {
//                _lock.ExitReadLock();
//            }
//        }

//        /// <inheritdoc />
//        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
//        {
//            int count;
//            var pool = ArrayPool<KeyValuePair<TKey, TValue>>.Shared;
//            KeyValuePair<TKey, TValue>[] snapshot;

//            // We only need the lock while we're creating the temporary snapshot, once
//            // that's done we can release and then allow the enumeration to continue.  We
//            // will get the count after the lock and then use it.
//            try
//            {
//                _lock.EnterUpgradeableReadLock();

//                count = this.Count;
//                snapshot = pool.Rent(count);
//                int index = 0;

//                foreach (var key in _list.Keys)
//                {
//                    snapshot[index] = new KeyValuePair<TKey, TValue>(key, _list[key]);
//                    index++;
//                }
//            }
//            finally
//            {
//                _lock.ExitUpgradeableReadLock();
//            }

//            // Since the array returned from the pool could be larger than we requested
//            // we will use the saved count to only iterate over the items we know to be
//            // in the range of the ones we requested.
//            for (int i = 0; i < count; i++)
//            {
//                yield return snapshot[i];
//            }

//            pool.Return(snapshot, true);
//        }

//        /// <inheritdoc />
//        public int Count
//        {
//            get
//            {
//                _lock.EnterReadLock();

//                try
//                {
//                    return _list.Count;
//                }
//                finally
//                {
//                    _lock.ExitReadLock();
//                }
//            }
//        }

//        /// <inheritdoc />
//        public ICollection<TKey> Keys()
//        {
//            _lock.EnterReadLock();

//            try
//            {
//                return _list.Keys;
//            }
//            finally
//            {
//                _lock.ExitReadLock();
//            }
//        }

//        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
//        {
//            throw new NotImplementedException();
//        }

//        /// <inheritdoc />
//        public ICollection<TValue> Values()
//        {
//            _lock.EnterReadLock();

//            try
//            {
//                return _list.Values;
//            }
//            finally
//            {
//                _lock.ExitReadLock();
//            }
//        }

//        public IEnumerator GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }

//        /// <inheritdoc />
//        public bool IsReadOnly => false;

//        ICollection<TKey> IDictionary<TKey, TValue>.Keys => throw new NotImplementedException();

//        ICollection<TValue> IDictionary<TKey, TValue>.Values => throw new NotImplementedException();
//    }
//}