/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2007-09-16
 * @last updated      : 2016-06-03
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Runtime.InteropServices;

namespace Argus.Windows
{
    /// <summary>
    /// Pause utility class to invoke the WinAPI Sleep function.
    /// </summary>
    public static class Pause
    {
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern void SleepKernel32(int dwMilliseconds);

        /// <summary>
        /// Invokes the operating system Sleep API in kernel32 to pause the current process for the given amount of time.
        /// All processing ceases for this time period (the process is effectively blocked).
        /// </summary>
        /// <param name="milliseconds"></param>
        public static void Sleep(int milliseconds)
        {
            if (milliseconds <= 0)
            {
                return;
            }

            SleepKernel32(milliseconds);
        }
    }
}