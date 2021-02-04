/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Argus.Extensions
{
    /// <summary>
    /// Extensions for UWP Controls.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Sets the focus to the control as FocusState.Programmatic.
        /// </summary>
        /// <param name="ctrl"></param>
        public static bool Focus(this Control ctrl)
        {
            return ctrl.Focus(FocusState.Programmatic);
        }
    }
}