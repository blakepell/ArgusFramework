/*
 * @author            : Blake Pell
 * @initial date      : 2022-11-26
 * @last updated      : 2022-11-26
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System.Runtime.InteropServices;

namespace Argus.Windows
{
    /// <summary>
    /// Touch input methods.
    /// </summary>
    public static class Touch
    {
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        /// <summary>
        /// If touch exists and is enabled on the current workstation.
        /// </summary>
        public static bool IsTouchEnabled()
        {
            // ReSharper disable once IdentifierTypo
            const int MAXTOUCHES_INDEX = 95;
            int maxTouches = GetSystemMetrics(MAXTOUCHES_INDEX);
            return maxTouches > 0;
        }

    }
}
