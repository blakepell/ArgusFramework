/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-12-13
 * @last updated      : 2019-11-18
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="System.Windows.Forms.Form" />.
    /// </summary>
    public static class FormExtensions
    {
        /// <summary>
        /// Pauses via a Task.Delay.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="milliseconds"></param>
        /// <remarks>This does not technically require the form but makes it convenient to access it via the IDE.</remarks>
        public static async Task PauseAsync(this Form form, int milliseconds)
        {
            await Task.Delay(2000);
        }

        #region "Flash Form"

        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int FlashWindowEx(ref FLASHWINFO pfwi);

        private const int FLASHW_CAPTION = 0x1;
        private const int FLASHW_TRAY = 0x2;
        private const int FLASHW_ALL = FLASHW_CAPTION | FLASHW_TRAY;

        public struct FLASHWINFO
        {
            public int cbSize;
            public IntPtr hwnd;
            public int dwFlags;
            public int uCount;
            public int dwTimeout;
        }

        /// <summary>
        /// Causes the form to flash both the caption bar and the tray.
        /// </summary>
        /// <param name="frm"></param>
        public static void FlashForm(this Form frm)
        {
            var flash = new FLASHWINFO();
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