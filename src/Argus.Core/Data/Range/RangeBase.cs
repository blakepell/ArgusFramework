using System;
using System.Collections.Generic;

namespace Argus.Data.Range
{
    //*********************************************************************************************************************
    //
    //         Namespace:  Range
    //      Organization:  http://www.blakepell.com        
    //      Initial Date:  06/26/2012
    //      Last Updated:  04/08/2016
    //     Programmer(s):  Blake Pell, blakepell@hotmail.com
    //
    //*********************************************************************************************************************

    /// <summary>
    /// The base class for all ranges.  This implements the functionality for the properties and sets up the methods that must be
    /// overridden to use.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks></remarks>
    public abstract class RangeBase<T> : IRange<T> where T : IComparable
    {
        protected RangeBase(T start, T end)
        {
            this.Start = start;
            this.End = end;
        }

        public abstract List<T> ToList();

        public T Start { get; set; }
        public T End { get; set; }

    }

}