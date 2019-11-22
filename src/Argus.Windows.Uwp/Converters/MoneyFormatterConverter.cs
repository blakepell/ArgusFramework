using System;
using Windows.UI.Xaml.Data;
using Argus.Extensions;

namespace Argus.Windows.Uwp.Converters
{
    /// <summary>
    ///     Formats an dollar amount with commas in the decimal place.
    /// </summary>
    public class MoneyFormatterConverter : IValueConverter
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
                return $"${System.Convert.ToDecimal(value).ToString().FormatIfNumber(2)}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        ///     Converts back to the money format.
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
                string buf = ((string) value).Replace("$", "");

                return decimal.Parse(buf);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}