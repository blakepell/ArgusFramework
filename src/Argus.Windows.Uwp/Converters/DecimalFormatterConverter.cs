/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using Windows.UI.Xaml.Data;
using Argus.Extensions;

namespace Argus.Windows.Uwp.Converters
{
    /// <summary>
    /// Formats a decimal amount with commas and the provided decimal places.
    /// </summary>
    public class DecimalFormatterConverter : IValueConverter
    {
        /// <summary>
        /// Converts to the string representation with commas.
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
                int decimalPlaces = 2;

                if (parameter != null)
                {
                    decimalPlaces = int.Parse((string) parameter);
                }

                return System.Convert.ToDecimal(value).ToString().FormatIfNumber(decimalPlaces);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Converts back to the decimal format.
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
                return decimal.Parse((string) value);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}