using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Argus.Windows.Wpf.Extensions
{
    public static class ImageExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  ImageExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  07/19/2010
        //      Last Updated:  07/19/2010
        //    Programmer(s):   Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Loads an image file into the Image control.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="filePath"></param>
        public static void LoadFile(this Image img, string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                throw new FileNotFoundException();
            }

            var bi = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            bi.BeginInit();
            bi.UriSource = new Uri(filePath);
            bi.EndInit();

            // Set the image's Source
            img.Source = bi;
        }

        /// <summary>
        ///     Loads an image file into the Image control.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="filePath"></param>
        /// <param name="decodePixelWidth"></param>
        /// <param name="decodePixelHeight"></param>
        public static void LoadFile(this Image img, string filePath, int decodePixelWidth, int decodePixelHeight)
        {
            if (File.Exists(filePath) == false)
            {
                throw new FileNotFoundException();
            }

            var bi = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            bi.BeginInit();
            bi.UriSource = new Uri(filePath);
            bi.DecodePixelWidth = decodePixelWidth;
            bi.DecodePixelHeight = decodePixelHeight;
            bi.EndInit();

            // Set the image's Source
            img.Source = bi;
        }
    }
}