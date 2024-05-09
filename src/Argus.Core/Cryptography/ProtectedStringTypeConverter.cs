/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2024-05-09
 * @last updated      : 2024-05-09
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

using System.ComponentModel;
using System.Globalization;

namespace Argus.Cryptography
{
    /// <summary>
    /// ProtectedString type converter.
    /// </summary>
    public class ProtectedStringTypeConverter : TypeConverter
    {
        /// <summary>
        /// Can convert from a string to a ProtectedString
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Can convert from a ProtectedString to a string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Convert from a string to a ProtectedString
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        public override object? ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object? value)
        {
            if (value is string str)
            {
                return new ProtectedString(str);
            }
            
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Convert from a ProtectedString to a string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        public override object? ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is ProtectedString pStr)
            {
                // Implicit conversion from a ProtectedString to a string.
                string? str = pStr;
                return str;
            }
            
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }}