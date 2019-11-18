using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Argus.Windows
{
    public static class Pause
    {
        //*********************************************************************************************************************
        //
        //             Class:  Pause
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  09/16/2007
        //      Last Updated:  06/03/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern void SleepKernel32(int dwMilliseconds);

        /// <summary>
        /// Invokes the operating system Sleep API in kernel32 to pause the current process for the given amount of time.  
        /// All processing ceases for this time period (the process is effectivly blocked).
        /// </summary>
        /// <param name="milleseconds"></param>
        public static void Sleep(int milleseconds)
        {
            if (milleseconds <= 0)
            {
                return;
            }

            SleepKernel32(milleseconds);
        }

    }
}
