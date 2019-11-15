using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Argus.Graphics
{

    /// <summary>
    /// Various image utilities
    /// </summary>
    /// <remarks></remarks>
    public class ImageUtilities
    {
        //*********************************************************************************************************************
        //
        //             Class:  ImageUtils
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  11/20/2007
        //      Last Updated:  11/15/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Extracts operating system's associated icon (in the highest resolution possible) for a given file and returns it as an Image object.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>Returns a <see cref="Image"/> of the associated icon.</returns>
        /// <remarks></remarks>
        public static Image ExtractAssociatedIconHiRes(string filePath)
        {
            using (var ico = Icon.ExtractAssociatedIcon(filePath))
            {
                return ico.ToBitmap();
            }
        }

        /// <summary>
        /// Adds a border around a bitmap.  The new bitmap will be larger than the original because of the border that is added.
        /// </summary>
        /// <param name="bm"></param>
        /// <param name="borderColor"></param>
        /// <param name="borderWidthInPixels"></param>
        /// <returns>Returns a <see cref="Bitmap"/> of the original image with a border added of the specified pixel width.</returns>
        public static Bitmap AddBorder(Bitmap bm, Color borderColor, int borderWidthInPixels)
        {

            var newBitmap = new Bitmap(bm.Width + (borderWidthInPixels * 2), bm.Height + (borderWidthInPixels * 2));

            // Draw the top border
            for (int i = 0; i <= newBitmap.Width - 1; i++)
            {
                for (int y = 0; y <= borderWidthInPixels; y++)
                {
                    newBitmap.SetPixel(i, y, borderColor);
                }
            }

            // Draw the bottom border
            for (int i = 0; i <= newBitmap.Width - 1; i++)
            {
                for (int y = newBitmap.Height - 1; y >= (newBitmap.Height - 1) - borderWidthInPixels; y += -1)
                {
                    newBitmap.SetPixel(i, y, borderColor);
                }
            }

            // Draw the left border
            for (int i = 0; i <= borderWidthInPixels; i++)
            {
                for (int y = 0; y <= newBitmap.Height - 1; y++)
                {
                    newBitmap.SetPixel(i, y, borderColor);
                }
            }

            // Draw the right border.
            for (int i = newBitmap.Width - 1; i >= (newBitmap.Width - 1) - borderWidthInPixels; i += -1)
            {
                for (int y = 0; y <= newBitmap.Height - 1; y++)
                {
                    newBitmap.SetPixel(i, y, borderColor);
                }
            }

            // Insert the old image into the bitmap
            using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(newBitmap))
            {
                gr.DrawImage(bm, borderWidthInPixels, borderWidthInPixels, bm.Width, bm.Height);
            }

            return newBitmap;
        }

        /// <summary>
        /// Gets the codec info for the specified ImageFormat.
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Returns a <see cref="ImageCodecInfo" /> if found, otherwise a null will be returned.</returns>
        /// <remarks>
        /// This method was from the MSDN public library located at:
        /// <seealso>https://docs.microsoft.com/en-us/dotnet/api/system.drawing.imaging.encoder.quality?redirectedfrom=MSDN&view=netframework-4.8</seealso>
        /// </remarks>
        public static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();

            int i = 0;
            while (i < encoders.Length)
            {
                if (encoders[i].FormatID == format.Guid)
                {
                    return encoders[i];
                }

                i += 1;
            }

            return null;
        }

        /// <summary>
        /// Converts a Bitmap to grayscale.
        /// </summary>
        /// <param name="bm"></param>
        public static void ConvertToGrayscale(Bitmap bm)
        {
            for (int y = 0; y <= bm.Height - 1; y++)
            {
                for (int x = 0; x <= bm.Width - 1; x++)
                {
                    var c = bm.GetPixel(x, y);
                    int luma = Convert.ToInt32(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    bm.SetPixel(x, y, Color.FromArgb(luma, luma, luma));
                }
            }
        }

        /// <summary>
        /// Converts a Bitmap to sepia tone.
        /// </summary>
        /// <param name="bm"></param>
        public static void ConvertToSepia(Bitmap bm)
        {
            for (int y = 0; y <= bm.Height - 1; y++)
            {
                for (int x = 0; x <= bm.Width - 1; x++)
                {
                    var c = bm.GetPixel(x, y);
                    int red = Convert.ToInt32((c.R * 0.393) + (c.G * 0.769) + (c.B * 0.189));
                    int green = Convert.ToInt32((c.R * 0.349) + (c.G * 0.686) + (c.B * 0.168));
                    int blue = Convert.ToInt32((c.R * 0.272) + (c.G * 0.534) + (c.B * 0.131));

                    if (red > 255)
                    {
                        red = 255;
                    }

                    if (green > 255)
                    {
                        green = 255;
                    }

                    if (blue > 255)
                    {
                        blue = 255;
                    }

                    bm.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }
        }

        /// <summary>
        /// Converts a Bitmap to black and white.
        /// </summary>
        /// <param name="bm"></param>
        public static void ConvertToBlackWhite(Bitmap bm)
        {
            ConvertToBlackWhite(bm, Convert.ToSingle(0.5));
        }

        /// <summary>
        /// Converts a Bitmap to black and white.  A tolerance property will determine when to use black or white.  For example, if a pixels
        /// brightness is greater than .5 then White will be used, otherwise black.  Adjusting this will change the black/white balance of the
        /// photo.
        /// </summary>
        /// <param name="bm"></param>
        /// <param name="tolerance"></param>
        public static void ConvertToBlackWhite(Bitmap bm, float tolerance)
        {
            for (int y = 0; y <= bm.Height - 1; y++)
            {
                for (int x = 0; x <= bm.Width - 1; x++)
                {
                    var c = bm.GetPixel(x, y);

                    if (c.GetBrightness() > tolerance)
                    {
                        c = Color.White;
                    }
                    else
                    {
                        c = Color.Black;
                    }

                    bm.SetPixel(x, y, c);
                }
            }
        }

        /// <summary>
        /// Inverts the color of each pixel in a Bitmap
        /// </summary>
        /// <param name="bm"></param>
        public static void ConvertToInverted(Bitmap bm)
        {
            for (int y = 0; y <= bm.Height - 1; y++)
            {
                for (int x = 0; x <= bm.Width - 1; x++)
                {
                    Color c = bm.GetPixel(x, y);
                    c = Color.FromArgb(255, 255 - c.R, 255 - c.G, 255 - c.B);
                    bm.SetPixel(x, y, c);
                }
            }
        }

        /// <summary>
        /// Converts a TIFF file into a JPEG or multiple JPEGs depending on how many frames exist in the TIFF.  An array of the files created
        /// will be returned.
        /// </summary>
        /// <param name="fileName"></param>
        public static string[] ConvertTiffToJpeg(string fileName)
        {
            using (var imageFile = Image.FromFile(fileName))
            {
                var frameDimensions = new FrameDimension(imageFile.FrameDimensionsList[0]);

                // Gets the number of pages from the tiff image (if multipage) 
                int frameNum = imageFile.GetFrameCount(frameDimensions);
                string[] jpegPaths = new string[frameNum];

                for (int frame = 0; frame <= frameNum - 1; frame++)
                {
                    // Selects one frame at a time and save as jpeg. 
                    imageFile.SelectActiveFrame(frameDimensions, frame);
                    using (var bmp = new Bitmap(imageFile))
                    {
                        jpegPaths[frame] = String.Format("{0}\\{1}{2}.jpg", Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName), frame);
                        bmp.Save(jpegPaths[frame], ImageFormat.Jpeg);
                    }
                }

                return jpegPaths;
            }
        }

        /// <summary>
        /// Loads an <see cref="Image"/> from a byte array.
        /// </summary>
        /// <param name="b">A byte array containing information for the <see cref="Image"/>.</param>
        public static Image ImageFromByteArray(byte[] b)
        {
            using (var ms = new MemoryStream(b))
            {
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// Loads an <see cref="Bitmap"/> from a byte array.
        /// </summary>
        /// <param name="b">A byte array containing information for the <see cref="Bitmap"/>.</param>
        public static Bitmap BitmapFromByteArray(byte[] b)
        {
            using (var ms = new MemoryStream(b))
            {
                return new Bitmap(ms);
            }
        }

    }

}