﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Argus.ComponentModel
{
    /// <summary>
    ///     A collection which is observable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpecialObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        ///     Delegate for when a list item changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ListItemChangedEventHandler(object sender, PropertyChangedEventArgs e);

        /// <summary>
        ///     Constructor
        /// </summary>
        public SpecialObservableCollection()
        {
            this.CollectionChanged += this.OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.AddOrRemoveListToPropertyChanged(e.NewItems, true);
            this.AddOrRemoveListToPropertyChanged(e.OldItems, false);
        }

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
        /// Implements a compariable version of the Linq Find which searches via index and not IEnumerable and can
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

            return default(T);
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
    }
}