/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2008-01-12
 * @last updated      : 2019-11-16
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.IO;

namespace Argus.Extensions
{
    /// <summary>
    /// Extensions to Stream based classes.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Converts the contents of a stream to a string.
        /// </summary>
        /// <param name="s"></param>
        public static string ToText(this Stream s)
        {
            s.Flush();
            s.Position = 0;

            using (var sr = new StreamReader(s))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Converts a MemoryStream into a Base64 encoded string.
        /// </summary>
        /// <param name="ms"></param>
        /// <remarks>
        /// The MemoryStream provides a ToArray() function which lends itself
        /// to converting to Base64 with Convert easier.
        /// </remarks>
        public static string ToBase64(this MemoryStream ms)
        {
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Converts a Stream into a Base64 encoded string.
        /// </summary>
        /// <param name="s"></param>
        /// <remarks>
        /// Stream does not have a ToArray, we will leverage the MemoryStream
        /// method to easily return Base64 for the Stream.
        /// </remarks>
        public static string ToBase64(this Stream s)
        {
            var ms = new MemoryStream();
            s.CopyTo(ms);

            return ToBase64(ms);
        }
    }
}