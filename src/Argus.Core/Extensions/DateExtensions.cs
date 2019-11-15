using System;

namespace Argus.Extensions
{

    /// <summary>
    /// Extension methods for the DateTime type.
    /// </summary>
    public static class DateExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  DateExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  01/12/2008
        //      Last Updated:  07/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Returns the month padded with two characters, E.g. 02 instead of 2.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string MonthTwoCharacters(this DateTime d)
        {
            return d.Month.ToString("D2");
        }

        /// <summary>
        /// Returns the day padded with two characters.  E.g. 02 instead of 2.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DayTwoCharacters(this DateTime d)
        {
            return d.Day.ToString("D2");
        }

        /// <summary>
        /// Returns the first day of the month for the date.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime FirstDayOfMonth(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, 1);
        }

        /// <summary>
        /// Returns the first date of the year for the date specified (e.g. 1/1/2001 for a date that falls in 2001).
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime FirstDayOfYear(this DateTime d)
        {
            return new DateTime(d.Year, 1, 1);
        }

        /// <summary>
        /// Returns the last date of the year for the date specified (e.g. 12/31/2001 for a date that falls in 2001).
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime LastDayOfYear(this DateTime d)
        {
            return new DateTime(d.Year, 12, 31);
        }

        /// <summary>
        /// Returns a short date string with leading 0's (e.g. 08/29/2011)
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToShortDatePaddedString(this DateTime d)
        {
            return $"{MonthTwoCharacters(d)}/{DayTwoCharacters(d)}/{d.Year}";
        }

        /// <summary>
        /// Returns the last day of the month for the date.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime LastDayOfMonth(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, System.DateTime.DaysInMonth(d.Year, d.Month));
        }

        /// <summary>
        /// Returns the month as it's English equivalent.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string MonthToEnglish(this DateTime d)
        {
            switch (d.Month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return string.Format("Invalid Month - {0}", d.Month);
            }

        }

        /// <summary>
        /// Calculate the age of an individual.
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns></returns>
        /// <remarks>
        /// This calculates an age off of the current date time, not the value of the date.
        /// </remarks>
        public static int CalculateAge(this DateTime birthDate)
        {
            int years = DateTime.Now.Year - birthDate.Year;

            // Take another year off if it's before the birth date in the current year
            if (DateTime.Now.Month < birthDate.Month | (DateTime.Now.Month == birthDate.Month & DateTime.Now.Day < birthDate.Day))
            {
                years = years - 1;
            }

            return years;
        }

        /// <summary>
        /// Whether or not the date is a Saturday or a Sunday.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsWeekend(this DateTime d)
        {
            if (d.DayOfWeek == DayOfWeek.Saturday | d.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Whether or not the date is a weekday (Monday, Tuesday, Wednesday, Thursday or Friday)
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsWeekday(this DateTime d)
        {
            return !IsWeekend(d);
        }

        /// <summary>
        /// Returns the Unix timestamp for the specified date.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long ToUnixTimeStamp(this DateTime d)
        {
            var unixEpoc = new DateTime(1970, 1, 1, 0, 0, 0);
            var unixTimeSpan = d - unixEpoc;
            return Convert.ToInt64(unixTimeSpan.TotalSeconds);
        }

        /// <summary>
        /// Retuns a file friendly format of this name in YYYY-MM-DD format.  If the includeTime parameter is true then the format
        /// will be YYYY-MM-DD-HH.MM.SS.MS
        /// </summary>
        /// <param name="d"></param>
        /// <param name="includeTime"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToFileNameFriendlyFormat(this DateTime d, bool includeTime)
        {
            // Adding the leading 0 to the day or month
            string month = d.Month.ToString();
            string day = d.Day.ToString();

            if (d.Month < 10)
            {
                month = "0" + month;
            }

            if (d.Day < 10)
            {
                day = "0" + day;
            }

            string buf = $"{d.Year.ToString().Trim()}-{month}-{day}";

            if (includeTime == true)
            {
                buf += $"-{d.Hour.ToString()}.{d.Minute.ToString()}.{d.Second.ToString()}.{d.Millisecond.ToString()}";

            }

            return buf;
        }

        /// <summary>
        /// Returns the SQL Server formatted TimeStamp for the beginning of the day, leaving off the "time" portion, e.g. 2012-05-21 00:00:00.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToSqlServerTimeStampBeginningOfDay(this DateTime d)
        {
            return $"{d.ToString("yyyy-MM-dd")} 00:00:00";
        }

        /// <summary>
        /// Returns the SQL Server formatted TimeStamp as a string.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToSqlServerTimeStamp(this DateTime d)
        {
            return $"{d.ToString("yyyy - MM - dd")} {d.Hour.ToString("D2")}:{d.Minute.ToString("D2")}:{d.Second.ToString("D2")}";
        }

        /// <summary>
        /// Converts a date into a SQLite format timestamp suitable for storage (SQLite does not support an official date field at this time).
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToSqliteTimeStamp(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Returns a string formatted in an Oracle accepted format.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToOracleSqlDate(this DateTime d)
        {
            return $"to_date('{d.ToString("dd.MM.yyyy HH:mm:ss")}','dd.mm.yyyy hh24.mi.ss')";
        }

        /// <summary>
        /// Returns a date in a short date format (10/4/2012).
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks>The base class library does not include this commonly used method off of date, so it is being provided here as an extension.</remarks>
        public static string ToShortDateString(this DateTime d)
        {
            return $"{d.Month}/{d.Day}/{d.Year}";
        }

        /// <summary>
        /// Returns the DateTime of the next week day after the given DateTime.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime NextWeekDay(this DateTime d)
        {
            var dow = d.DayOfWeek;
            double daysToAdd = 1;

            if (dow == DayOfWeek.Friday)
            {
                daysToAdd = 3;
            }
            else if (dow == DayOfWeek.Saturday)
            {
                daysToAdd = 2;
            }

            return d.AddDays(daysToAdd);
        }

        /// <summary>
        /// Returns the DateTime of the previous week day before the given DateTime.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime LastWeekDay(this System.DateTime d)
        {
            var dow = d.DayOfWeek;
            double daysToAdd = -1;

            if (dow == DayOfWeek.Monday)
            {
                daysToAdd = -3;
            }
            else if (dow == DayOfWeek.Sunday)
            {
                daysToAdd = -2;
            }

            return d.AddDays(daysToAdd);
        }

        /// <summary>
        /// Returns the quarter end date for the DateTime value. Optionally, provide an int to add or subtract quarters 
        /// to return previous or future quarter dates.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="addQuarters"></param>
        /// <returns></returns>
        public static DateTime QuarterEnd(this DateTime d, int addQuarters = 0)
        {
            // Calculate date for requested quarter
            d = d.AddMonths(addQuarters * 3);

            // Calculate quarter end month
            var quarterNum = (int)(d.Month + 2) / 3;
            var quarterMonth = quarterNum * 3;

            // Build quarter end date
            return new DateTime(d.Year, quarterMonth, DateTime.DaysInMonth(d.Year, quarterMonth));
        }

        /// <summary>
        /// Returns the quarter start date for the DateTime value. Optionally, provide an int to add or subtract quarters 
        /// to return previous or future quarter dates.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="addQuarters"></param>
        /// <returns></returns>
        public static DateTime QuarterStart(this DateTime d, int addQuarters = 0)
        {
            // Calculate date for requested quarter
            d = d.AddMonths(addQuarters * 3);

            // Calculate quarter start month
            var quarterNum = (int)(d.Month + 2) / 3;
            var quarterMonth = (((quarterNum - 1) * 3) + 1);

            // Build quarter end date
            return new DateTime(d.Year, quarterMonth, 1);
        }

        /// <summary>
        /// Whether or not the date is a month end.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsMonthEnd(this DateTime d)
        {
            // Get rid of the time if it was passed in.
            var tempDate = new DateTime(d.Year, d.Month, d.Day);

            // Create a date that is the first day of the month of the requested date.  Add one month to the date, 
            // then subtract one day to let the framework get the last day of the month passed in.
            var actualMonthEnd = new DateTime(d.Year, d.Month, 1).AddMonths(1).AddDays(-1);

            // Compare the month end calculated to the date passed in
            return (tempDate == actualMonthEnd);
        }
    }
}