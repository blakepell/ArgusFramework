﻿/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2013-11-05
 * @last updated      : 2019-11-18
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Text;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for Byte arrays.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Converts a byte array to it's string representation that is encoded with base-64 digits.
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
    }
}