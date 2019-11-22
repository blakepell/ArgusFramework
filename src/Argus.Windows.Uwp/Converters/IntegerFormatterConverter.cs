using System;
using Windows.UI.Xaml.Data;
using Argus.Extensions;

namespace Argus.Windows.Uwp.Converters
{
    /// <summary>
    ///     Formats an integer with commas in the decimal place.
    /// </summary>
    public class IntegerFormatterConverter : IValueConverter
    {
        /// <summary>
        ///     Converts to the string representation with commas.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return System.Convert.ToInt32(value).ToString().FormatIfNumber(0);
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        ///     Converts back to the integer format.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return int.Parse((string) value);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}