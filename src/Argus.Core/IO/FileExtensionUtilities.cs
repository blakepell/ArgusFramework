/*
 * @author            : Blake Pell
 * @initial date      : 2010-03-01
 * @last updated      : 2022-07-01
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

namespace Argus.IO
{
    /// <summary>
    /// Class to extract information about a file extension and/or parse lists to return lists containing specific categories of files.
    /// </summary>
    /// <remarks>
    /// Additional file path parsing methods are included in the System.IO.Path class of the .Net Framework.
    /// </remarks>
    public static class FileExtensionUtilities
    {
        /// <summary>
        /// Returns the lower case file extension, minus the period.
        /// </summary>
        /// <param name="filePath"></param>
        public static string GetFileExtension(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return "";
            }

            return Path.GetExtension(filePath).Trim('.').ToLower();
        }
        
        /// <summary>
        /// Whether or not the path points to a valid image file determined by the extension.
        /// </summary>
        /// <param name="filePath"></param>
        public static bool IsImage(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            switch (GetFileExtension(filePath))
            {
                case "jpg":
                case "jpeg":
                case "gif":
                case "png":
                case "tiff":
                case "tif":
                case "ico":
                case "bmp":
                case "svg":
                case "webp":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Whether or not the path points to a valid video file determined by the extension.
        /// </summary>
        /// <param name="filepath"></param>
        public static bool IsVideo(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                return false;
            }

            switch (GetFileExtension(filepath))
            {
                case "3g2":
                case "3gp":
                case "asf":
                case "asx":
                case "avi":
                case "flv":
                case "mov":
                case "mp4":
                case "m4v":
                case "mpg":
                case "mpeg":
                case "rm":
                case "vob":
                case "wmv":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Whether or not the path points to a valid audio file as determined by the extension.
        /// </summary>
        /// <param name="filePath"></param>
        public static bool IsAudio(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            switch (GetFileExtension(filePath))
            {
                case "aac":
                case "aif":
                case "iif":
                case "mid":
                case "mp3":
                case "mpa":
                case "ra":
                case "wav":
                case "wma":
                case "m4p":
                case "ogg":
                    return true;
                default:
                    return false;
            }
        }
    }
}