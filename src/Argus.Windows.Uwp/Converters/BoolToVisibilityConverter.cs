/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Argus.Windows.Uwp.Converters
{
    /// <summary>
    /// Converts between bool and visibility.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public BooleanToVisibilityConverter()
        {
            this.OnFalse = Visibility.Collapsed;
            this.OnTrue = Visibility.Visible;
        }

        public Visibility OnTrue { get; set; }
        public Visibility OnFalse { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool v = (bool) value;

            return v ? this.OnTrue : this.OnFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility == false)
            {
                return DependencyProperty.UnsetValue;
            }

            if ((Visibility) value == this.OnTrue)
            {
                return true;
            }

            return false;
        }
    }
}