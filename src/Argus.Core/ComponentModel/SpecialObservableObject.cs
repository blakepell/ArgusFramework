/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2019-11-15
 * @last updated      : 2021-01-29
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Argus.ComponentModel
{
    /// <summary>
    /// A collection which is observable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpecialObservableCollection<T> : ObservableCollection<T>, IDisposable
    {
        /// <summary>
        /// Delegate for when a list item changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ListItemChangedEventHandler(object sender, PropertyChangedEventArgs e);

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
        /// Implements a comparable version of the Linq Find which searches via index and not IEnumerable and can
        /// offer performance improvements over FirstOrDefault when searching in memory objects.
        /// </summary>
        /// <param name="match"></param>
        public T Find(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("Predicate cannot be null.");
            }

            for (int i = 0; i < this.Count; i++)
            {
                if (match(this[i]))
                {
                    return this[i];
                }
            }

            return default;
        }

        private void ListItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnListItemChanged(this, e);
        }

        public event ListItemChangedEventHandler ListItemChanged;

        private void OnListItemChanged(object sender, PropertyChangedEventArgs e)
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