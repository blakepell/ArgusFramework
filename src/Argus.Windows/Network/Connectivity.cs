/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-04-07
 * @last updated      : 2019-11-18
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Runtime.InteropServices;

namespace Argus.Network
{
    /// <summary>
    /// Class to deal with network connectivity.
    /// </summary>
    public static class Connectivity
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int Description, int ReservedValue);

        /// <summary>
        /// Determines via the Windows API InternetGetConnectedState whether the Internet is connected or not.  This will check
        /// for more than the availability of a network.
        /// </summary>
        /// <remarks>This will return a NotSupportedException on non Windows systems.</remarks>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static bool IsInternetAvailable()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("IsInternetAvailable via wininet.dll is not supported on this platform.");
            }

            int desc;

            return InternetGetConnectedState(out desc, 0);
        }
    }
}