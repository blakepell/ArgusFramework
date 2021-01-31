/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2012-06-26
 * @last updated      : 2021-01-29
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System.Collections.Generic;
using System.Text;

namespace Argus.Data.Range
{
    /// <summary>
    /// An integer range
    /// </summary>
    /// <remarks>Add Interval support so the range can be run for intervals of a number and not just by 1's.</remarks>
    public class IntRange : RangeBase<int>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public IntRange(int start, int end) : base(start, end)
        {
        }

        /// <summary>
        /// A list containing all items in the range.
        /// </summary>
        public override List<int> ToList()
        {
            var lst = new List<int>();

            if (this.Start < this.End)
            {
                // Low to high
                for (int x = this.Start; x <= this.End; x++)
                {
                    lst.Add(x);
                }
            }
            else if (this.Start > this.End)
            {
                // High to low
                for (int x = this.End; x >= this.Start; x += -1)
                {
                    lst.Add(x);
                }
            }
            else
            {
                // They are equal, it's a one item list.
                lst.Add(this.Start);
            }

            return lst;
        }

        /// <summary>
        /// Returns a comma separated list of all items in the range.
        /// </summary>
        public override string ToString()
        {
            return this.ToString(",");
        }

        /// <summary>
        /// Returns a delimited list of all items in the range.
        /// </summary>
        /// <param name="delimiter"></param>
        public string ToString(string delimiter)
        {
            return this.ToString(delimiter, "");
        }

        /// <summary>
        /// Returns a delimited list of all items in the range with each item wrapped in a specified character on both sides.
        /// </summary>
        /// <param name="delimiter"></param>
        /// <param name="wrapCharacter"></param>
        public string ToString(string delimiter, string wrapCharacter)
        {
            var sb = new StringBuilder();
            var lst = this.ToList();

            foreach (int i in lst)
            {
                sb.AppendFormat("{0}{1}{0}{2}", wrapCharacter, i.ToString(), delimiter);
            }

            return sb.ToString().TrimEnd(',');
        }
    }
}