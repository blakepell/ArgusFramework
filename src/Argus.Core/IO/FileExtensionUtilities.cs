using System.IO;
using Argus.Extensions;

namespace Argus.IO
{
    /// <summary>
    ///     Class to extract information about a file extension and/or parse lists to return lists containing specific categories of files.
    /// </summary>
    /// <remarks>
    ///     Additional file path parsing methods are included in the System.IO.Path class of the .Net Framework.
    /// </remarks>
    public class FileExtensionUtilities
    {
        //*********************************************************************************************************************
        //
        //             Class:  FileExtensionUtilities
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  03/01/2010
        //      Last Updated:  03/01/2010
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Returns the file extension, minus the period in lower case.  The file path can be either a location URL or a web hyper reference.  Reference
        ///     System.IO.Path for managed .Net Framework file methods.
        /// </summary>
        /// <param name="filePath"></param>
        public static string GetFileExtension(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return "";
            }

            return Path.GetExtension(filePath).Trim(".").ToLower();
        }

        /// <summary>
        ///     Whether or not the path points to a valid image file determined by the extension.
        /// </summary>
        /// <param name="filePath"></param>
        public static bool IsImage(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            string extension = GetFileExtension(filePath);

            switch (extension.ToLower())
            {
                case "jpg":
                case "jpeg":
                case "gif":
                case "png":
                case "tiff":
                case "tif":
                case "ico":
                case "bmp":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        ///     Whether or not the path poitns to a valid video file deteremined by the extension.
        /// </summary>
        /// <param name="filepath"></param>
        public static bool IsVideo(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                return false;
            }

            string extension = GetFileExtension(filepath);

            switch (extension.ToLower())
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
        ///     Whether or not the path poitns to a valid audio file deteremined by the extension.
        /// </summary>
        /// <param name="filePath"></param>
        public static bool IsAudio(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            string extension = GetFileExtension(filePath);

            switch (extension.ToLower())
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