/*
 * @author            : Blake Pell
 * @initial date      : 2022-11-26
 * @last updated      : 2025-04-16
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Argus.Windows
{
    /// <summary>
    /// Touch input methods.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Touch
    {
        /// <summary>
        /// The maximum number of touches supported by the system.
        /// </summary>
        private const int MAX_TOUCHES_INDEX = 95;
        
        /// <summary>
        /// GetSystemMetrics Windows API call.
        /// </summary>
        /// <param name="nIndex"></param>
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        /// <summary>
        /// If touch exists and is enabled on the current workstation.
        /// </summary>
        public static bool IsTouchEnabled()
        {
            int maxTouches = GetSystemMetrics(MAX_TOUCHES_INDEX);
            return maxTouches > 0;
        }
    }
}
