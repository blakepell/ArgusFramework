/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-12-13
 * @last updated      : 2019-03-13
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Windows.Forms;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="System.Windows.Forms.ToolStripProgressBar" />.
    /// </summary>
    public static class ToolStripProgressBarExtensions
    {
        /// <summary>
        /// Sets the progress bar value, without using Windows Aero animation.
        /// </summary>
        /// <remarks>This is kind of a hack, but it works.</remarks>
        public static void SetProgressNoAnimation(this ToolStripProgressBar pb, int value)
        {
            // To get around this animation, we need to move the progress bar backwards.
            if (value == pb.Maximum)
            {
                // Special case (can't set value > Maximum).
                // Set the value THEN move it backwards one.
                pb.Value = value;
                pb.Value = value - 1;
            }
            else
            {
                // Move past
                pb.Value = value + 1;
            }

            // Move to correct value
            pb.Value = value;
        }
    }
}