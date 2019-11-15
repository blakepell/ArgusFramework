using System;
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
    /// A range of date's without times.
    /// </summary>
    /// <remarks></remarks>
    public class DateRange : RangeBase<DateTime>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <remarks></remarks>
        public DateRange(DateTime start, DateTime end) : base(start, end)
        {
            this.Start = new DateTime(start.Year, start.Month, start.Day);
            this.End = new DateTime(end.Year, end.Month, end.Day);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="interval"></param>
        /// <remarks></remarks>
        public DateRange(DateTime start, DateTime end, DateInterval interval) : base(start, end)
        {
            this.Interval = interval;
            this.Start = new DateTime(start.Year, start.Month, start.Day);
            this.End = new DateTime(end.Year, end.Month, end.Day);
        }

        /// <summary>
        /// A list containing all items in the range.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override System.Collections.Generic.List<System.DateTime> ToList()
        {
            List<DateTime> lst = new List<DateTime>();
            DateTime counter = this.Start;

            switch (this.Interval)
            {
                case DateInterval.Day:
                    while (counter <= this.End)
                    {
                        lst.Add(counter);
                        counter = counter.AddDays(1);
                    }
                    break;
                case DateInterval.Week:
                    while (counter <= this.End)
                    {
                        lst.Add(counter);
                        counter = counter.AddDays(7);
                    }
                    break;
                case DateInterval.Month:
                    while (counter <= this.End)
                    {
                        lst.Add(counter);
                        counter = counter.AddMonths(1);
                    }
                    break;
                case DateInterval.Year:
                    while (counter <= this.End)
                    {
                        lst.Add(counter);
                        counter = counter.AddYears(1);
                    }
                    break;
            }

            return lst;

        }

        /// <summary>
        /// Returns a comma delimited list of all items in the range.
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
            List<DateTime> lst = this.ToList();

            foreach (DateTime d in lst)
            {
                string shortDate = string.Format("{0}/{1}/{2}", d.Month, d.Day, d.Year);
                sb.AppendFormat("{0}{1}{0}{2}", wrapCharacter, shortDate, delimiter);
            }

            return sb.ToString().TrimEnd(",".ToCharArray());
        }

        /// <summary>
        /// The date intervals that the range supports.
        /// </summary>
        /// <remarks></remarks>
        public enum DateInterval
        {
            Day,
            Week,
            Month,
            Year
        }

        /// <summary>
        /// The date interval in between items in the range.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateInterval Interval { get; set; }

    }

}