/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2010-02-04
 * @last updated      : 2020-10-23
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for Image objects.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Determines if a Bitmap is a single color.
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

        /// <summary>
        /// Returns a hash that identify like images based on a down scaled mapping of pixels
        /// above or below the a median brightness threshold.
        /// </summary>
        /// <param name="bmp"></param>
        public static string MedianBrightnessHash(this Bitmap bmp)
        {
            float sum = 0;
            var sb = new StringBuilder();

            // Create new image down scaled to 16x16 pixels.
            using var bmpMin = new Bitmap(bmp, new Size(16, 16));

            // Calculate the median brightness of the image.
            for (int j = 0; j < bmpMin.Height; j++)
            {
                for (int i = 0; i < bmpMin.Width; i++)
                {
                    sum += bmpMin.GetPixel(i, j).GetBrightness();
                }
            }

            const int pixelCount = 256;
            float avg = sum / pixelCount;

            // Create the bit hash based off of each pixels brightness being over the median
            // brightness threshold of the entire image.
            for (int j = 0; j < bmpMin.Height; j++)
            {
                for (int i = 0; i < bmpMin.Width; i++)
                {
                    // Reduce colors to true / false                
                    if (bmpMin.GetPixel(i, j).GetBrightness() < avg)
                    {
                        sb.Append(0);
                    }
                    else
                    {
                        sb.Append(1);
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Counts the unique colors in a <see cref="Bitmap" />.
        /// </summary>
        /// <param name="bmp"></param>
        public static int CountColors(this Bitmap bmp)
        {
            var colors = new HashSet<Color>();

            for (int x = 0; x < bmp.Size.Width; x++)
            {
                for (int y = 0; y < bmp.Size.Height; y++)
                {
                    colors.Add(bmp.GetPixel(x, y));
                }
            }

            return colors.Count;
        }
    }
}