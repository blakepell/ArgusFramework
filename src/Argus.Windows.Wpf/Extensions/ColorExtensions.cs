/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Drawing;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for dealing with Colors in WPF.
    /// </summary>
    public static class WpfColorExtensions
    {
        /// <summary>
        /// Converts a <see cref="System.Windows.Media.Color" /> to a <see cref="System.Drawing.Color" />.
        /// </summary>
        /// <param name="color"></param>
        public static Color ToSystemDrawingColor(this System.Windows.Media.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Color" /> to a <see cref="System.Windows.Media.Color" />.
        /// </summary>
        /// <param name="color"></param>
        public static System.Windows.Media.Color ToWindowsMediaColor(this Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}