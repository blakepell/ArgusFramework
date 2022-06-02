/*
 * @author            : Blake Pell
 * @initial date      : 2011-11-16
 * @last updated      : 2017-11-14
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

namespace Argus.Utilities
{
    /// <summary>
    /// Various utility methods to deal with dates.
    /// </summary>
    public class DateUtilities
    {
        /// <summary>
        /// Returns a random date between the minimum value allowed in .Net and the maximum value allowed.  This uses
        /// DateTime.MinValue and DateTime.MaxValue.
        /// </summary>
        /// <returns>A random DateTime between DateTime.MinValue and DateTime.MaxValue.</returns>
        public static DateTime GetRandomDate()
        {
            return GetRandomDate(DateTime.MinValue, DateTime.MaxValue);
        }

        /// <summary>
        /// Returns a random date between the specified start and end date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public static DateTime GetRandomDate(DateTime startDate, DateTime endDate)
        {
            var rand = new Random();

            int year = rand.Next(startDate.Year, endDate.Year);
            int month = rand.Next(startDate.Month, endDate.Month);

            return new DateTime(year, month, rand.Next(1, DateTime.DaysInMonth(year, month)));
        }

        /// <summary>
        /// Returns a random date between the specified start and end date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public static DateTime GetRandomDate(string startDate, string endDate)
        {
            return GetRandomDate(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
        }

        /// <summary>
        /// Returns whether a string is a valid DateTime.
        /// </summary>
        /// <param name="strDate"></param>
        public static bool IsValidDateTime(string strDate)
        {
            return DateTime.TryParse(strDate, out _);
        }

        /// <summary>
        /// Formats date ranges for a string display.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date</param>
        /// <remarks>
        /// This is initially designed to display formatted date ranges for calendar event items
        /// based off of dates that incoming from Exchange.
        /// </remarks>
        public static string FormatDateRange(DateTime startDate, DateTime endDate)
        {
            // The dates are fully identical down to the time.
            if (startDate == endDate)
            {
                // If it's midnight exactly, just show the day else show the day and time.
                if (startDate.TimeOfDay.Hours == 0 && startDate.TimeOfDay.Minutes == 0)
                {
                    return $"{startDate.ToShortDateString()}";
                }

                return $"{startDate.ToShortDateString()} {startDate.ToShortTimeString()}";
            }

            // If the dates are equal (minus the time)
            if (startDate.Date == endDate.Date)
            {
                // If the times are equal, just show the date and time of one otherwise show the range.
                return startDate.TimeOfDay.Ticks == endDate.TimeOfDay.Ticks ? $"{startDate.ToShortDateString()} {startDate.ToShortTimeString()}" : $"{startDate.ToShortDateString()} {startDate.ToShortTimeString()} to {endDate.ToShortTimeString()}";
            }

            // Dates are different, but if they are both at midnight exactly don't show the time
            if (startDate.TimeOfDay.Hours == 0 && startDate.TimeOfDay.Minutes == 0 && endDate.TimeOfDay.Hours == 0 && endDate.TimeOfDay.Minutes == 0)
            {
                return $"{startDate.ToShortDateString()} - {endDate.ToShortDateString()}";
            }

            // Dates are different and times are different, show the full range.
            return $"{startDate.ToShortDateString()} {startDate.ToShortTimeString()} to {endDate.ToShortDateString()} {endDate.ToShortTimeString()}";
        }
    }
}