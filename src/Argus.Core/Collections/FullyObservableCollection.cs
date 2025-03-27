/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2018-02-22
 * @last updated      : 2025-03-27
 * @copyright         : Copyright (c) 2003-2025, All rights reserved.
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
        private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.SupportsRecursion);

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
                _lock.EnterReadLock();

                try
                {
                    return base[index];
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
            set
            {
                _lock.EnterWriteLock();

                try
                {
                    // Remove event handler from existing item if it implements INotifyPropertyChanged
                    if (index < base.Count)
                    {
                        var oldItem = base[index];

                        if (oldItem is INotifyPropertyChanged oldNpc)
                        {
                            oldNpc.PropertyChanged -= ListItemPropertyChanged;
                        }
                    }

                    // Set the new item
                    base[index] = value;

                    // Add event handler to new item if it implements INotifyPropertyChanged
                    if (value is INotifyPropertyChanged newNpc)
                    {
                        newNpc.PropertyChanged += ListItemPropertyChanged;
                    }
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        public new IEnumerator<T> GetEnumerator()
        {
            int count;
            var pool = ArrayPool<T>.Shared;
            T[] snapshot;

            // We only need the lock while we're creating the temporary snapshot, once
            // that's done we can release and then allow the enumeration to continue.  We
            // will get the count after the lock and then use it.
            _lock.EnterReadLock();

            try
            {
                count = base.Count;
                snapshot = pool.Rent(count);

                for (int i = 0; i < count; i++)
                {
                    snapshot[i] = base[i];
                }
            }
            finally
            {
                _lock.ExitReadLock();
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
            _lock.EnterWriteLock();

            try
            {
                base.InsertItem(index, item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// <inheritdoc cref="RemoveItem"/>
        /// </summary>
        protected override void RemoveItem(int index)
        {
            _lock.EnterWriteLock();

            try
            {
                // Access base collection directly to avoid recursive locking
                var item = base[index];

                // Remove the handler if the item implements INotifyPropertyChanged
                if (item is INotifyPropertyChanged npc)
                {
                    npc.PropertyChanged -= ListItemPropertyChanged;
                }

                base.RemoveItem(index);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// <inheritdoc cref="ClearItems"/>
        /// </summary>
        protected override void ClearItems()
        {
            _lock.EnterWriteLock();

            try
            {
                base.ClearItems();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Returns the number of elements in the collection.
        /// </summary>
        public new int Count
        {
            get
            {
                _lock.EnterReadLock();

                try
                {
                    return base.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Whether an item currently exists in the collection.
        /// </summary>
        /// <param name="item"></param>
        public new bool Contains(T item)
        {
            _lock.EnterReadLock();

            try
            {
                return base.Contains(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Copies all the items from the index on into the array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public new void CopyTo(T[] array, int arrayIndex)
        {
            _lock.EnterWriteLock();

            try
            {
                base.CopyTo(array, arrayIndex);
            }
            finally
            {
                _lock.ExitWriteLock();
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
                if (match == null)
                {
                    throw new ArgumentNullException("Predicate cannot be null", nameof(match));
                }
            #else
                ArgumentNullException.ThrowIfNull(match, nameof(match));
            #endif

            _lock.EnterReadLock();
            try
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (match(base[i]))
                    {
                        return base[i];
                    }
                }
            }
            finally
            {
                _lock.ExitReadLock();
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

            _lock.Dispose();
        }
    }
}