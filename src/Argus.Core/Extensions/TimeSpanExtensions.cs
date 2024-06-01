/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2012-10-18
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="TimeSpan" />
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Displays a formatted vertical string with line breaks that contains the contents of the current TimeSpan
        /// in a human-readable form.
        /// </summary>
        /// <param name="ts"></param>
        public static string ToVerticalString(this TimeSpan ts)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} Days{1}", ts.Days, Environment.NewLine);
            sb.AppendFormat("{0} Hours{1}", ts.Hours, Environment.NewLine);
            sb.AppendFormat("{0} Minutes{1}", ts.Minutes, Environment.NewLine);
            sb.AppendFormat("{0} Seconds{1}", ts.Seconds, Environment.NewLine);
            sb.AppendFormat("{0} Milliseconds{1}", ts.Milliseconds, Environment.NewLine);

            return sb.ToString();
        }

        /// <summary>
        /// Multiplies the TimeSpan by the provided value.
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="value"></param>
        public static TimeSpan Multiply(this TimeSpan ts, long value)
        {
            return new TimeSpan(ts.Ticks * value);
        }

        /// <summary>
        /// Divides the TimeSpan by the provided value.
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="value"></param>
        public static TimeSpan Divide(this TimeSpan ts, long value)
        {
            return new TimeSpan(ts.Ticks / value);
        }
    }
}