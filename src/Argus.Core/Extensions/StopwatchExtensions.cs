/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2023-10-22
 * @last updated      : 2023-10-22
 * @copyright         : Copyright (c) 2003-2023, All rights reserved.
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
        /// Converts the Stopwatch elapsed time to 00:00 format.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <returns>A string in the format of 5m 23s</returns>
        /// <remarks>A null Stopwatch will not throw an exception and instead will return 0m 0s.</remarks>
        public static string ToMinutesSecondsFormat(this Stopwatch stopwatch)
        {
            if (stopwatch == null)
            {
                return "0m 0s";
            }

            var elapsed = stopwatch.Elapsed;
            return $"{elapsed.Minutes + elapsed.Hours * 60:D2}m {elapsed.Seconds:D2}s";
        }
    }
}