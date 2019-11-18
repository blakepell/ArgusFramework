using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="System.Windows.Forms.Form"/>.
    /// </summary>
    public static class FormExtensions
    {
        //*********************************************************************************************************************
        //
        //             Class:  FormExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  12/13/2009
        //      Last Updated:  11/18/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Pauses via a Task.Delay.
        /// </summary>
        /// <param name="milleseconds"></param>
        /// <remarks>This does not technically require the form but makes it convenient to access it via the IDE.</remarks>
        public static async Task PauseAsync(this Form form, int milleseconds)
        {
            await Task.Delay(2000);
        }

        #region "Flash Form"

        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        /// <summary>
        /// Windows API Call to make a window Flash
        /// </summary>
        /// <param name="pfwi"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static extern Int32 FlashWindowEx(ref FLASHWINFO pfwi);

        private const Int32 FLASHW_CAPTION = 0x1;
        private const Int32 FLASHW_TRAY = 0x2;

        private const Int32 FLASHW_ALL = (FLASHW_CAPTION | FLASHW_TRAY);
        public struct FLASHWINFO
        {
            public Int32 cbSize;
            public IntPtr hwnd;
            public Int32 dwFlags;
            public Int32 uCount;
            public Int32 dwTimeout;
        }

        /// <summary>
        /// Causes the form to flash both the caption bar and the tray.
        /// </summary>
        /// <param name="frm"></param>
        /// <remarks>
        /// </remarks>
        public static void FlashForm(this Form frm)
        {
            FLASHWINFO flash = new FLASHWINFO();
            flash.cbSize = Marshal.SizeOf(flash);
            ///// size of structure in bytes
            flash.hwnd = frm.Handle;
            ///// Handle to the window to be flashed
            flash.dwFlags = FLASHW_ALL;
            ///// to flash both the caption bar + the tray
            flash.uCount = 5;
            ///// the number of flashes
            flash.dwTimeout = 1000;
            ///// speed of flashes in MilliSeconds ( can be left out )
            FlashWindowEx(ref flash);
            ///// flash the window you have specified the handle for...
        }

        #endregion

    }
}
