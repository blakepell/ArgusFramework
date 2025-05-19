using System.ComponentModel;

namespace Argus.Collections
{
    /// <summary>
    /// A circular buffer that will remove items from the front of the list when new items are added and
    /// the capacity has been reached.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CircularObservableCollection<T> : ICollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Internal linked list to allow for O(1) adding to the list and O(1) removing from the list.
        /// </summary>
        private readonly LinkedList<T> _items = new();

        /// <summary>
        /// The capacity at which the oldest items begin to be trimmed off the front of the list.
        /// </summary>
        public int Capacity { get; set; } = 1000;

        /// <summary>
        /// Event handler for when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// Event handler for when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Event for when the collection changes.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);

        /// <summary>
        /// Event for when a property changes.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Number of items in the collection.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// If the collection is currently readonly.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// A circular buffer that will remove items from the front of the list when new items are added and
        /// the capacity has been reached.
        /// </summary>
        public CircularObservableCollection()
        {

        }

        /// <summary>
        /// A circular buffer that will remove items from the front of the list when new items are added and
        /// the capacity has been reached.
        /// </summary>
        /// <param name="capacity">The capacity at which the oldest items will begin to be removed.</param>
        public CircularObservableCollection(int capacity)
        {
            this.Capacity = capacity;
        }

        private bool _updating = false;

        /// <summary>
        /// Starts a bulk update of items where the change notifications are suppressed until the EndUpdate when a single
        /// notification is done.
        /// </summary>
        public void BeginUpdate()
        {
            _updating = true;
        }

        /// <summary>
        /// Ends a bulk update of items where the change notifications are suppressed until this function is called where
        /// a single change notification is sent.
        /// </summary>
        public void EndUpdate()
        {
            _updating = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(nameof(Count));
        }

        /// <summary>
        /// Adds an item into the collection.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            _items.AddLast(item);

            if (!_updating)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, _items.Count - 1));
                OnPropertyChanged(nameof(Count));
            }

            if (_items.Count > this.Capacity)
            {   
                // There was no first item for some reason, ditch out.
                if (_items?.First == null)
                {
                    return;
                }

                var removedItem = _items.First.Value;
                _items.RemoveFirst();

                if (!_updating)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, 0));
                    OnPropertyChanged(nameof(Count));
                }
            }
        }

        /// <summary>
        /// Clears the entire collection.
        /// </summary>
        public void Clear()
        {
            _items.Clear();

            if (!_updating)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                OnPropertyChanged(nameof(Count));
            }
        }

        /// <summary>
        /// If an item is contained within the collection.
        /// </summary>
        /// <param name="item"></param>
        public bool Contains(T item) => _items.Contains(item);

        /// <summary>
        /// Copies an item into the collection at the specified index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        /// <summary>
        /// Gets an enumerator of T for the collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index < 0)
            {
                return false;
            }

            var node = _items.Find(item);
            if (node != null)
            {
                _items.Remove(node);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
                OnPropertyChanged(nameof(Count));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns the item at the specified index.
        /// </summary>
        /// <param name="item"></param>
        private int IndexOf(T item)
        {
            int index = 0;
            foreach (var currentItem in _items)
            {
                if (Equals(currentItem, item))
                {
                    return index;
                }

                index++;
            }
            return -1;
        }
    }
}

