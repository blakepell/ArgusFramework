using System;
using System.Drawing;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Color"/>
    /// </summary>
    public static class ColorExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  ColorExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  07/29/2012
        //      Last Updated:  11/11/2015
        //    Programmer(s):   Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

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