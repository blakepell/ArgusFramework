/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2013-11-05
 * @last updated      : 2019-11-18
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for Byte arrays.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Converts a byte array to its string representation that is encoded with base-64 digits.
        /// </summary>
        /// <param name="bytes"></param>
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Converts a byte array into a string with the specified encoding.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="enc"></param>
        public static string ToString(this byte[] bytes, Encoding enc)
        {
            return enc.GetString(bytes);
        }

        /// <summary>
        /// Returns a string from the byte array.
        /// </summary>
        /// <param name="buf">Byte array to return a string for</param>
        /// <param name="useSystemDefaultEncoding">Whether to use the System Default encoding.  If false, UTF-8 will be used.  If ASCII
        /// is needed the overload that allows you to specify encoding should be used.</param>
        public static string ToString(this byte[] buf, bool useSystemDefaultEncoding)
        {
            return useSystemDefaultEncoding
                ? Encoding.Default.GetString(buf)
                : Encoding.UTF8.GetString(buf);
        }
    }
}