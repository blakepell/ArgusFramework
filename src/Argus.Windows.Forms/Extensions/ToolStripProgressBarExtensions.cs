using System.Windows.Forms;

namespace Argus.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="System.Windows.Forms.ToolStripProgressBar" />.
    /// </summary>
    public static class ToolStripProgressBarExtensions
    {
        //*********************************************************************************************************************
        //
        //             Class:  DataGridViewExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  12/13/2009
        //      Last Updated:  03/13/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Sets the progress bar value, without using Windows Aero animation.
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