using System.Collections.Generic;
using System.Text;

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
        /// <remarks></remarks>
        public IntRange(int start, int end) : base(start, end)
        {

        }

        /// <summary>
        /// A list containing all items in the range.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override System.Collections.Generic.List<int> ToList()
        {
            List<int> lst = new List<int>();

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
            else {
                // They are equal, it's a one item list.
                lst.Add(this.Start);
            }

            return lst;
        }

        /// <summary>
        /// Returns a comma seperated list of all items in the range.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.ToString(",");
        }

        /// <summary>
        /// Returns a delimited list of all items in the range.
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ToString(string delimiter)
        {
            return this.ToString(delimiter, "");
        }

        /// <summary>
        /// Returns a delimited list of all items in the range with each item wrapped in a specified character on both sides.
        /// </summary>
        /// <param name="delimiter"></param>
        /// <param name="wrapCharacter"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ToString(string delimiter, string wrapCharacter)
        {
            StringBuilder sb = new StringBuilder();
            List<int> lst = this.ToList();

            foreach (int i in lst)
            {
                sb.AppendFormat("{0}{1}{0}{2}", wrapCharacter, i.ToString(), delimiter);
            }

            return sb.ToString().TrimEnd(",".ToCharArray());
        }

    }

}