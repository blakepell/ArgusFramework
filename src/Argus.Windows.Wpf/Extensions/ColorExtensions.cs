namespace Argus.Extensions
{
    /// <summary>
    ///     Extension methods for dealing with Colors in WPF.
    /// </summary>
    public static class WpfColorExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  WpfColorExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  08/30/2012
        //      Last Updated:  11/17/2019
        //    Programmer(s):   Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Converts a <see cref="System.Windows.Media.Color"/> to a <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="color"></param>
        public static System.Drawing.Color ToSystemDrawingColor(this System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        ///     Converts a <see cref="System.Drawing.Color"/> to a <see cref="System.Windows.Media.Color"/>.
        /// </summary>
        /// <param name="color"></param>
        public static System.Windows.Media.Color ToWindowsMediaColor(this System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

    }
}
