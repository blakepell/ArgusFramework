/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2014-01-03
 * @last updated      : 2021-01-29
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for general arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Remove element from array at given index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            var destination = new T[source.Length - 1];

            if (index > 0)
            {
                Array.Copy(source, 0, destination, 0, index);
            }

            if (index < source.Length - 1)
            {
                Array.Copy(source, index + 1, destination, index, source.Length - index - 1);
            }

            return destination;
        }
    }
}