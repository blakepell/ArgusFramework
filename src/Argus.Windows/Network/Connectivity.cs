using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Argus.Network
{
    /// <summary>
    /// Class to deal with network connectivity.
    /// </summary>
    public static class Connectivity
    {
        //*********************************************************************************************************************
        //
        //             Class:  Connectivity
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  04/07/2009
        //      Last Updated:  11/18/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        #region IsInternetAvailable

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

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

        #endregion

    }
}
