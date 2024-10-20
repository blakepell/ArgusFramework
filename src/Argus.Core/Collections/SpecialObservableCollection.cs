/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2019-11-15
 * @last updated      : 2024-10-20
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Argus.Collections
{
    /// <summary>
    /// A collection which is observable including to the property level.  The collection offers
    /// an <see cref="EnumerateSnapshot"/> function to allow for iteration over a cached static
    /// list that won't be updated while any enumeration is outstanding on it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpecialObservableCollection<T> : ObservableCollection<T>, IDisposable
    {
        /// <summary>
        /// Whether this collection has had records added, deleted or moved.
        /// </summary>
        private bool _changed = false;

        /// <summary>
        /// Indicates whether the Snapshot is currently being enumerated.
        /// </summary>
        private bool _enumeratingSnapshot = false;

        /// <summary>
        /// Internal snapshot that can be enumerated over.
        /// </summary>
        private List<T> Snapshot { get; set; }

        /// <summary>
        /// Whether or not the current Snapshot is outdated compared to the <see cref="Snapshot"/>
        /// cached copy.
        /// </summary>
        public bool SnapshotOutdated => _changed;

        /// <summary>
        /// Delegate for when a list item changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ListItemChangedEventHandler(object sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Event that is raised when a list item changes, including properties.
        /// </summary>
        public event ListItemChangedEventHandler ListItemChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public SpecialObservableCollection()
        {
            this.CollectionChanged += this.OnCollectionChanged;
        }

        /// <summary>
        /// Event that fires when the collection changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    // Set the changed so the next time the EnumerateSnapshot is called it knows
                    // if CreateSnapshot needs to be refreshed (otherwise it will use the cached
                    // copy).
                    _changed = true;
                    break;
            }

            this.AddOrRemoveListToPropertyChanged(e.NewItems, true);
            this.AddOrRemoveListToPropertyChanged(e.OldItems, false);
        }

        /// <summary>
        /// Adds or removes from the property changed list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="add"></param>
        private void AddOrRemoveListToPropertyChanged(IEnumerable list, bool add)
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
        /// This will allow us to get an Enumerator that is a snapshot if the collection has changed
        /// but use the cached copy if it has not.  This is useful when programs need to iterate over
        /// the records but want to avoid enumeration changed exceptions.
        /// </summary>
        private void CreateSnapshot()
        {
            // If someone is currently enumerating the snapshot we're not going to update it or alter
            // the changed flag.  They get the slightly out of date Snapshot so their instance in time
            // iteration can continue.
            if (_enumeratingSnapshot)
            {
                return;
            }

            if (this.Snapshot == null || _changed)
            {
                this.Snapshot = this.ToList();
                _changed = false;
            }
        }

        /// <summary>
        /// Enumerates over a Snapshot.  The CreateSnapshot must have been called prior.
        /// </summary>
        public IEnumerator<T> EnumerateSnapshot()
        {
            // If the Snapshot hasn't been created, has changed AND there isn't a currently
            // enumeration happening then update the cached snapshot.  All enumerations on
            // the snapshot are forced through here so we know when we can and can't update
            // the underlying cached list.
            this.CreateSnapshot();

            _enumeratingSnapshot = true;

            try
            {
                foreach (T item in this.Snapshot)
                {
                    yield return item;
                }
            }
            finally
            {
                _enumeratingSnapshot = false;
            }
        }

        /// <summary>
        /// Implements a comparable version of the Linq Find which searches via index and not IEnumerable and can
        /// offer performance improvements over FirstOrDefault when searching in memory objects.
        /// </summary>
        /// <param name="match"></param>
        public T Find(Predicate<T> match)
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
        private void ListItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.ListItemChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Disposes of resources for the SpecialObservableCollection including the removal of
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