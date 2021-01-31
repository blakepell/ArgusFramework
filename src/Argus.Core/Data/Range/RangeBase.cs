using System;
using System.Collections.Generic;

namespace Argus.Data.Range
{
    /*
     * @author            : Blake Pell
     * @website           : http://www.blakepell.com
     * @initial date      : 2012-06-26
     * @last updated      : 2016-04-28
     * @copyright         : Copyright (c) 2003-2021, All rights reserved.
     * @license           : MIT
     */

    /// <summary>
    /// The base class for all ranges.  This implements the functionality for the properties and sets up the methods that must be
    /// overridden to use.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RangeBase<T> : IRange<T> where T : IComparable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        protected RangeBase(T start, T end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Returns a <see cref="List{T}" /> of the specified <see cref="IRange{T}" />.
        /// </summary>
        public abstract List<T> ToList();

        /// <summary>
        /// The starting value of the range.
        /// </summary>
        public T Start { get; set; }

        /// <summary>
        /// The ending value of the range.
        /// </summary>
        public T End { get; set; }
    }
}