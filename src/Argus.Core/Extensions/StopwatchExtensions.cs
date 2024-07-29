/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2023-10-22
 * @last updated      : 2024-07-28
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods to the <see cref="Stopwatch"/> class.
    /// </summary>
    public static class StopwatchExtensions
    {
        /// <summary>
        /// Converts the Stopwatch elapsed time to 5m 23s format.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <returns>A string in the format of 5m 23s</returns>
        /// <remarks>A null Stopwatch will not throw an exception and instead will return 0m 0s.</remarks>
        public static string ToMinutesSecondsFormat(this Stopwatch? stopwatch)
        {
            if (stopwatch == null)
            {
                return "0m 0s";
            }

            var elapsed = stopwatch.Elapsed;

            return $"{elapsed.Minutes + elapsed.Hours * 60:D2}m {elapsed.Seconds:D2}s";
        }

        /// <summary>
        /// Converts the Stopwatch elapsed time to 3m 24s 5ms format.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <returns>A string in the format of 5m 23s</returns>
        /// <remarks>A null Stopwatch will not throw an exception and instead will return 0m 0s.</remarks>
        public static string ToMinutesSecondsMillisecondsFormat(this Stopwatch? stopwatch)
        {
            if (stopwatch == null)
            {
                return "0m 0s 0ms";
            }

            var elapsed = stopwatch.Elapsed;

            return $"{elapsed.Minutes + elapsed.Hours * 60:D2}m {elapsed.Seconds:D2}s {elapsed.Milliseconds:D4}";
        }

        /// <summary>
        /// Converts the Stopwatch elapsed time to 1h 3m 24s format.
        /// </summary>
        /// <param name="sw"></param>
        public static string ToHoursMinutesSecondsFormat(this Stopwatch? sw)
        {
            if (sw == null)
            {
                return "0h 0m 0s";
            }

            var elapsed = sw.Elapsed;

            return $"{elapsed.Hours:D1}h {elapsed.Minutes:D2}m {elapsed.Seconds:D2}s";
        }
        
        /// <summary>
        /// Converts the Stopwatch elapsed time to 1h 3m 24s 5ms format.
        /// </summary>
        /// <param name="sw"></param>
        public static string ToHoursMinutesSecondsMillisecondsFormat(this Stopwatch? sw)
        {
            if (sw == null)
            {
                return "0h 0m 0s 0ms";
            }

            var elapsed = sw.Elapsed;

            return $"{elapsed.Hours:D2}h {elapsed.Minutes:D2}m {elapsed.Seconds:D2}s {elapsed.Milliseconds:D3}ms";
        }
    }
}