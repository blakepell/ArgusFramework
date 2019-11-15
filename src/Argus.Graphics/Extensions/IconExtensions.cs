using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for the System.Drawing.Icon class.
    /// </summary>
    /// <remarks>
    public static class IconExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  IconExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  03/14/2009
        //      Last Updated:  11/15/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Returns the bit depth of the icon
        /// </summary>
        /// <param name="icon"></param>
        public static int GetIconBitDepth(this Icon icon)
        {            
            if (icon == null)
            {
                throw new ArgumentNullException("The Icon provided cannot be null.");
            }

            // Save the data from the icon to a MemoryStream, convert it to a byte array and use the
            // BitConverter on it to return the BitDepth.
            using (var ms = new System.IO.MemoryStream())
            {
                icon.Save(ms);
                return Convert.ToInt32(BitConverter.ToInt16(ms.ToArray(), 12));
            }

        }

        /// <summary>
        /// Returns the dimensions formatted like 16x16 for display purposes.
        /// </summary>
        /// <param name="icon"></param>
        public static string FormattedDimensions(this Icon icon)
        {
            if (icon == null)
            {
                throw new ArgumentNullException("The Icon provided cannot be null.");
            }

            return $"{icon.Height}x{icon.Width}";
        }

        /// <summary>
        /// This will convert the icon to a bitmap and then save it as an icon.  If coupled with the ExtractAssociatedIcon
        /// this will allow you to save the extracted Icon at a higher color resolution than 8 colors.
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="saveFileName"></param>
        /// <remarks></remarks>
        public static void SaveHiRes(this Icon icon, string saveFileName)
        {
            if (icon == null)
            {
                throw new ArgumentNullException("The Icon provided cannot be null.");
            }

            Image img = icon.ToBitmap();
            img.Save(saveFileName, ImageFormat.Icon);
        }
    }
}