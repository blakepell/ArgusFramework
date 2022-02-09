/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2012-06-26
 * @last updated      : 2016-04-28
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Data.Range
{
    /// <summary>
    /// Range interface that specifies the properties and methods that range's should carry.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRange<T>
    {
        /// <summary>
        /// The start element in the range
        /// </summary>
        T Start { get; set; }

        /// <summary>
        /// The ending element in the range
        /// </summary>
        T End { get; set; }

        /// <summary>
        /// A list that includes all elements in the range.
        /// </summary>
        List<T> ToList();
    }
}