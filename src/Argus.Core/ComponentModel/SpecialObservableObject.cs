using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Argus.ComponentModel
{

    /// <summary>
    /// A collection which is observable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpecialObservableCollection<T> : ObservableCollection<T>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public SpecialObservableCollection()
        {
            this.CollectionChanged += OnCollectionChanged;
        }

        void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AddOrRemoveListToPropertyChanged(e.NewItems, true);
            AddOrRemoveListToPropertyChanged(e.OldItems, false);
        }

        private void AddOrRemoveListToPropertyChanged(IList list, bool add)
        {
            if (list == null)
            {
                return;
            }

            foreach (object item in list)
            {
                var o = item as INotifyPropertyChanged;
                if (o != null)
                {
                    if (add)
                    {
                        o.PropertyChanged += ListItemPropertyChanged;
                    }

                    if (!add)
                    {
                        o.PropertyChanged -= ListItemPropertyChanged;
                    }
                }
                else
                {
                    throw new Exception("INotifyPropertyChanged is required");
                }
            }
        }

        void ListItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnListItemChanged(this, e);
        }

        public delegate void ListItemChangedEventHandler(object sender, PropertyChangedEventArgs e);

        public event ListItemChangedEventHandler ListItemChanged;

        private void OnListItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ListItemChanged != null) { this.ListItemChanged(this, e); }
        }

    }
}
