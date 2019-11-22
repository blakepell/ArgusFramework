using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Argus.Windows.Uwp.Converters
{
    /// <summary>
    ///     Converts between bool and TextWrapping values
    /// </summary>
    public class BoolToTextWrappingConverter : IValueConverter
    {
        /// <summary>
        ///     Converts TextWrapping.Wrap values to 'true' and TextWrapping.NoWrap to 'false'
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((TextWrapping) value == TextWrapping.NoWrap) ^ (parameter as string ?? string.Empty).Equals("Reverse");
        }

        /// <summary>
        ///     Converts 'true' values to TextWrapping.Wrap and 'false' values to TextWrapping.NoWrap
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (bool) value ^ (parameter as string ?? string.Empty).Equals("Reverse") ? TextWrapping.NoWrap : TextWrapping.Wrap;
        }
    }
}