using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Argus.ComponentModel
{
    /// <summary>
    ///     An observable class that implements INotifyPropertyChanged and provides a Generic method to modify
    ///     properties.
    /// </summary>
    /// <example>
    ///     private int _length = 0;
    ///     public int Length
    ///     {
    ///     get
    ///     {
    ///     return _length;
    ///     }
    ///     set
    ///     {
    ///     Set(ref _length, value, "Length");
    ///     }
    ///     }
    /// </example>
    public class Observable : INotifyPropertyChanged
    {
        /// <summary>
        ///     Event that is fired when the property is changed via Set as part of INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Used to set the value of a property and fire PropertyChanged event of INotifyPropertyChanged.
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
        ///     OnPropertyChanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}