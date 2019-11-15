using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Argus.Extensions;
using System.Linq;

namespace Argus.IO
{

    /// <summary>
    /// Class to extract information about a file extension and/or parse lists to return lists containing specific categories of files.
    /// </summary>
    /// <remarks>
    /// Additional file path parsing methods are included in the System.IO.Path class of the .Net Framework.
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
        /// Returns the file extension, minus the period in lower case.  The file path can be either a location URL or a web hyper reference.  Reference
        /// System.IO.Path for managed .Net Framework file methods.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetFileExtension(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) == true)
            {
                return "";
            }
            else
            {
                return System.IO.Path.GetExtension(filePath).Trim(".").ToLower();
            }
        }

        /// <summary>
        /// Whether or not the path points to a valid image file deteremined by the extension.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsImage(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) == true)
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
                case "psd":
                case "psp":
                case "thm":
                case "ps":
                case "pdn":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Whether or not the path poitns to a valid video file deteremined by the extension.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsVideo(string filepath)
        {
            if (string.IsNullOrEmpty(filepath) == true)
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
        /// Whether or not the path poitns to a valid audio file deteremined by the extension.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsAudio(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) == true)
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

        /// <summary>
        /// Extracts image files from a list of strings that are filenames.
        /// </summary>
        /// <param name="ls">A generic list of strings that represent filenames or url's.</param>
        /// <param name="removeQueryStringFirst">Whether or not to attempt to remove a QueryString if it exists and these are web files.</param>
        /// <param name="removeDuplicates">Whether or not to remove duplicated items in the list.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> ExtractImageFiles(List<string> ls, bool removeQueryStringFirst, bool removeDuplicates)
        {

            if (removeQueryStringFirst == true)
            {
                for (int x = 0; x <= ls.Count - 1; x++)
                {
                    if (ls[x].Contains("?") == true)
                    {
                        ls[x] = ls[x].Substring(0, ls[x].IndexOf("?"));
                    }
                }
            }

            if (removeDuplicates == true)
            {
                ls = ls.Distinct().ToList();
            }

            var queryMatchingFiles = from item in ls where item.EndsWith("jpg", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("jpeg", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("gif", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("png", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("tiff", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("tif", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("ico", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("bmp", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("psd", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("psp", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("ps", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("pdn", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("thm", StringComparison.CurrentCultureIgnoreCase) select item;

            return queryMatchingFiles.ToList();
        }

        /// <summary>
        /// Extracts audio files from a list of strings that are filenames.
        /// </summary>
        /// <param name="ls">A generic list of strings that represent filenames or url's.</param>
        /// <param name="removeQueryStringFirst">Whether or not to attempt to remove a QueryString if it exists and these are web files.</param>
        /// <param name="removeDuplicates">Whether or not to remove duplicated items in the list.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> ExtractAudioFiles(List<string> ls, bool removeQueryStringFirst, bool removeDuplicates)
        {

            if (removeQueryStringFirst == true)
            {
                for (int x = 0; x <= ls.Count - 1; x++)
                {
                    if (ls[x].Contains("?") == true)
                    {
                        ls[x] = ls[x].Substring(0, ls[x].IndexOf("?"));
                    }
                }
            }

            if (removeDuplicates == true)
            {
                ls = ls.Distinct().ToList();
            }

            var queryMatchingFiles = from item in ls where item.EndsWith("aac", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("aif", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("iif", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("mid", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("mp3", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("mpa", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("ra", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("wav", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("wma", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("m4p", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("ogg", StringComparison.CurrentCultureIgnoreCase) select item;

            return queryMatchingFiles.ToList();

        }

        /// <summary>
        /// Extracts video files from a list of strings that are filenames.
        /// </summary>
        /// <param name="ls">A generic list of strings that represent filenames or url's.</param>
        /// <param name="removeQueryStringFirst">Whether or not to attempt to remove a QueryString if it exists and these are web files.</param>
        /// <param name="removeDuplicates">Whether or not to remove duplicated items in the list.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> ExtractVideoFiles(List<string> ls, bool removeQueryStringFirst, bool removeDuplicates)
        {

            if (removeQueryStringFirst == true)
            {
                for (int x = 0; x <= ls.Count - 1; x++)
                {
                    if (ls[x].Contains("?") == true)
                    {
                        ls[x] = ls[x].Substring(0, ls[x].IndexOf("?"));
                    }
                }
            }

            if (removeDuplicates == true)
            {
                ls = ls.Distinct().ToList();
            }

            var queryMatchingFiles = from item in ls where item.EndsWith("3g2", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("3gp", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("asf", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("asx", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("avi", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("flv", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("mov", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("mp4", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("m4v", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("mpg", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("rm", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("vob", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("wmv", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("rm", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("vob", StringComparison.CurrentCultureIgnoreCase) | item.EndsWith("mpeg", StringComparison.CurrentCultureIgnoreCase) select item;

            return queryMatchingFiles.ToList();

        }

    }

}