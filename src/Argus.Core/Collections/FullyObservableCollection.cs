/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2018-02-22
 * @last updated      : 2024-10-20
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

using System.Buffers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace Argus.Collections
{
    /// <summary>
    /// Represents a thread safe ObservableCollection that sends notify messages down
    /// to the property level.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FullyObservableCollection<T> : ObservableCollection<T>, IDisposable, IEnumerable
    {
        /// <summary>
        /// The lock mechanism with support for recursion which allows <see cref="GetEnumerator"/> to be called without
        /// a <see cref="LockRecursionException"/> being thrown.
        /// </summary>
        public ReaderWriterLockSlim Lock = new(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// Delegate for when a list item changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ListItemChangedEventHandler(object sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Event that is raised when a list item changes, including properties.
        /// </summary>
        public event ListItemChangedEventHandler? ListItemChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public FullyObservableCollection()
        {
            this.CollectionChanged += this.OnCollectionChanged;
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public new T this[int index]
        {
            get
            {
                Lock.EnterReadLock();

                try
                {
                    return base[index];
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
            set => this.Add(value);
        }

        public new IEnumerator<T> GetEnumerator()
        {
            int count;
            var pool = ArrayPool<T>.Shared;
            T[] snapshot;

            // We only need the lock while we're creating the temporary snapshot, once
            // that's done we can release and then allow the enumeration to continue.  We
            // will get the count after the lock and then use it.
            try
            {
                Lock.EnterReadLock();

                count = this.Count;
                snapshot = pool.Rent(count);

                for (int i = 0; i < count; i++)
                {
                    snapshot[i] = this[i];
                }
            }
            finally
            {
                Lock.ExitReadLock();
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

        /// <summary>
        /// <inheritdoc cref="InsertItem"/>
        /// </summary>
        protected override void InsertItem(int index, T item)
        {
            Lock.EnterWriteLock();

            try
            {
                base.InsertItem(index, item);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// <inheritdoc cref="RemoveItem"/>
        /// </summary>
        protected override void RemoveItem(int index)
        {
            Lock.EnterWriteLock();

            try
            {
                var item = this[index];
                base.RemoveItem(index);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// <inheritdoc cref="ClearItems"/>
        /// </summary>
        protected override void ClearItems()
        {
            Lock.EnterWriteLock();

            try
            {
                base.ClearItems();
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Returns the number of elements in the collection.
        /// </summary>
        public new int Count
        {
            get
            {
                Lock.EnterReadLock();

                try
                {
                    return base.Count;
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Whether an item currently exists in the collection.
        /// </summary>
        /// <param name="item"></param>
        public new bool Contains(T item)
        {
            Lock.EnterReadLock();

            try
            {
                return base.Contains(item);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Copies all the items from the index on into the array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public new void CopyTo(T[] array, int arrayIndex)
        {
            Lock.EnterWriteLock();

            try
            {
                base.CopyTo(array, arrayIndex);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Event that fires when the collection changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            this.AddOrRemoveListToPropertyChanged(e.NewItems, true);
            this.AddOrRemoveListToPropertyChanged(e.OldItems, false);
        }

        /// <summary>
        /// Adds or removes from the property changed list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="add"></param>
        private void AddOrRemoveListToPropertyChanged(IEnumerable? list, bool add)
        {
            if (list == null)
            {
                return;
            }

            foreach (var item in list)
            {
                if (item is INotifyPropertyChanged o)
                {
                    if (add)
                    {
                        o.PropertyChanged += this.ListItemPropertyChanged;
                    }

                    if (!add)
                    {
                        o.PropertyChanged -= this.ListItemPropertyChanged;
                    }
                }
                else
                {
                    throw new Exception("INotifyPropertyChanged is required");
                }
            }
        }

        /// <summary>
        /// Implements a comparable version of the Linq Find which searches via index and not IEnumerable and can
        /// offer performance improvements over FirstOrDefault when searching in memory objects.
        /// </summary>
        /// <param name="match"></param>
        public T? Find(Predicate<T> match)
        {
            #if NETSTANDARD2_0 || NETSTANDARD2_1
                throw new ArgumentNullException("Predicate cannot be null", nameof(match));
            #else
                ArgumentNullException.ThrowIfNull(match, nameof(match));
            #endif

            for (int i = 0; i < this.Count; i++)
            {
                if (match(this[i]))
                {
                    return this[i];
                }
            }

            return default;
        }

        /// <summary>
        /// Raised when a property on one of the items in the collection changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.ListItemChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Disposes of resources for the FullyObservableCollection including the removal of
        /// event handlers.
        /// </summary>
        public void Dispose()
        {
            this.CollectionChanged -= this.OnCollectionChanged;

            foreach (var item in this)
            {
                if (item is INotifyPropertyChanged o)
                {
                    o.PropertyChanged -= this.ListItemPropertyChanged;
                }
            }
        }
    }
}