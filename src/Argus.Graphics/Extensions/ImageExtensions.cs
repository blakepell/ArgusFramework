using System.Drawing;

namespace Argus.Extensions
{
    /// <summary>
    ///     Extension methods for Image objects.
    /// </summary>
    public static class ImageExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  ImageExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  02/04/2010
        //      Last Updated:  11/15/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Determines if a Bitmap is a single color.
        /// </summary>
        /// <param name="bm"></param>
        public static bool IsSingleColor(this Bitmap bm)
        {
            // If there is no height or width then there are technically no colors so false is being returned here.
            if (bm.Width == 0 && bm.Height == 0)
            {
                return false;
            }

            // So, this is going to get the first pixel and then the first time it finds a pixel that is a
            // different color it will return false (otherwise it will finish and return true).
            var c = bm.GetPixel(0, 0);

            for (int x = 0; x <= bm.Width - 1; x++)
            {
                for (int y = 0; y <= bm.Height - 1; y++)
                {
                    if (bm.GetPixel(x, y) != c)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}