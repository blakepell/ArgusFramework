/*
 * @author            : Blake Pell
 * @initial date      : 2019-11-15
 * @last updated      : 2021-01-31
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 * @website           : http://www.blakepell.com
 */

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Argus.ComponentModel
{
    /// <summary>
    /// An observable class that implements INotifyPropertyChanged and provides a Generic method to modify
    /// properties.
    /// </summary>
    /// <example>
    ///     <code>
    ///         private int _length = 0;
    ///     
    ///         public int Length
    ///         {
    ///             get =&gt; _length;
    ///             set =&gt; Set(ref _length, value, "Length");
    ///         }
    ///     </code>
    /// </example>
    public class Observable : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that is fired when the property is changed via Set as part of INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Used to set the value of a property and fire PropertyChanged event of INotifyPropertyChanged.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            this.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// OnPropertyChanged Event.
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}