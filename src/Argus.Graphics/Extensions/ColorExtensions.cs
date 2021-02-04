/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2012-07-29
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Drawing;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Color" />
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Creates a random color and returns it.
        /// </summary>
        /// <param name="c"></param>
        public static Color RandomColor(this Color c)
        {
            var rand = new Random();

            return Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
        }

        /// <summary>
        /// Returns a Hex/HTML color from a System.Drawing.Color
        /// </summary>
        /// <param name="c"></param>
        public static string ToHex(this Color c)
        {
            return ColorTranslator.ToHtml(c);
        }
    }
}