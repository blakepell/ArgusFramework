using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Argus.IO
{
    public class FileSystemUtilities
    {
        //*********************************************************************************************************************
        //
        //             Class:  FileSystemUtilities
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  07/07/2010
        //      Last Updated:  07/05/2017
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Creates the full directory tree of a specified path.  This method will create all parent directories necessary.
        /// </summary>
        /// <param name="di"></param>
        /// <remarks></remarks>
        public static void CreateDirectory(DirectoryInfo di)
        {
            if (di.Parent != null)
                CreateDirectory(di.Parent);
            if (di.Exists == false)
                di.Create();
        }

        /// <summary>
        /// Creates the full directory tree of a specified path.  This method will create all parent directories necessary.
        /// </summary>
        /// <param name="path"></param>
        /// <remarks></remarks>
        public static void CreateDirectory(string path)
        {
            path = path.TrimEnd('\\');
            DirectoryInfo di = new DirectoryInfo(path);
            CreateDirectory(di);
        }

        /// <summary>
        /// Extracts the file name off of a path.  This function first looks for a \ character and if it's not found will then
        /// look for a front slash as the seperator.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ExtractFileName(string fullPath)
        {
            int lastSlash = fullPath.LastIndexOf("\\");

            if (lastSlash == -1)
            {
                lastSlash = fullPath.LastIndexOf("/");
            }

            if (lastSlash == -1)
            {
                return fullPath;
            }
            else
            {
                return fullPath.Substring(lastSlash + 1);
            }
        }

        /// <summary>
        /// Checks to see if a file exists before deleting it.  This will catch and eat any excpetions for cases when you want
        /// a silent delete.
        /// </summary>
        /// <param name="filePath"></param>
        /// <remarks></remarks>
        public static void SafeFileDelete(string filePath)
        {
            if (System.IO.File.Exists(filePath) == true)
            {
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch
                {
                    // eat error
                }
            }

        }

        /// <summary>
        /// Checks to see if a directory exists before deleting it.  This will catch and eat any excpetions for cases when you want
        /// a silent delete.
        /// </summary>
        /// <param name="dirPath"></param>
        /// <remarks></remarks>
        public static void SafeDirectoryDelete(string dirPath)
        {
            if (System.IO.Directory.Exists(dirPath) == true)
            {
                try
                {
                    System.IO.Directory.Delete(dirPath);
                }
                catch
                {
                    // eat error
                }
            }
        }

        /// <summary>
        /// Deletes files in a path by a specified pattern (e.g. *.txt)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <remarks></remarks>
        public static void DeleteFilesByPattern(string path, string pattern)
        {
            string[] files = System.IO.Directory.GetFiles(path, pattern);

            foreach (string f in files)
            {
                System.IO.File.Delete(f);
            }

        }


        /// <summary>
        /// Gets all files in a directory and orders them in ascending or decending order by modified date or
        /// created date.
        /// </summary>
        /// <param name="dir">The directory to return files for.</param>
        /// <param name="st">Which attribute the directory should be sorted by.</param>
        /// <param name="so">The sort order.  Decending will sort newest to oldest.  Ascending will sort oldest to newest.</param>
        /// <returns>A generic string list.</returns>
        /// <remarks></remarks>
        public static List<string> GetFilesByModifiedDate(string dir, SortType st, SortOrder so)
        {
            dir = dir.TrimEnd('\\');
            DirectoryInfo di = new DirectoryInfo(dir);
            FileSystemInfo[] files = di.GetFileSystemInfos();
            IEnumerable<FileSystemInfo> orderedFiles = null;
            List<string> returnList = new List<string>();

            switch (st)
            {
                case SortType.LastWriteTime:
                    if (so == SortOrder.Decending)
                    {
                        orderedFiles = files.OrderBy(f => f.LastWriteTime).Reverse();
                    }
                    else
                    {
                        orderedFiles = files.OrderBy(f => f.LastWriteTime);
                    }
                    break;
                case SortType.LastAccessTime:
                    if (so == SortOrder.Decending)
                    {
                        orderedFiles = files.OrderBy(f => f.LastAccessTime).Reverse();
                    }
                    else
                    {
                        orderedFiles = files.OrderBy(f => f.LastAccessTime);
                    }
                    break;
                case SortType.CreationTime:
                    if (so == SortOrder.Decending)
                    {
                        orderedFiles = files.OrderBy(f => f.CreationTime).Reverse();
                    }
                    else
                    {
                        orderedFiles = files.OrderBy(f => f.CreationTime);
                    }
                    break;
                default:
                    orderedFiles = files.OrderBy(f => f.LastWriteTime).Reverse();
                    break;
            }

            foreach (var fi in orderedFiles)
            {
                returnList.Add(fi.Name);
            }

            return returnList;
        }

        /// <summary>
        /// Gets all files in a directory and orders them in ascending or decending order by modified date or
        /// created date.
        /// </summary>
        /// <param name="dir">The directory to return files for.</param>
        /// <param name="st">Which attribute the directory should be sorted by.</param>
        /// <param name="so">The sort order.  Decending will sort newest to oldest.  Ascending will sort oldest to newest.</param>
        /// <returns>A generic string list.</returns>
        /// <remarks></remarks>
        public static IEnumerable<FileSystemInfo> GetFilesSystemInfosByModifiedDate(string dir, SortType st, SortOrder so)
        {
            dir = dir.Trim('\\');
            DirectoryInfo di = new DirectoryInfo(dir);
            FileSystemInfo[] files = di.GetFileSystemInfos();
            IEnumerable<FileSystemInfo> orderedFiles = Enumerable.Empty<FileSystemInfo>();

            switch (st)
            {
                case SortType.LastWriteTime:
                    if (so == SortOrder.Decending)
                    {
                        orderedFiles = files.OrderBy(f => f.LastWriteTime).Reverse();
                    }
                    else
                    {
                        orderedFiles = files.OrderBy(f => f.LastWriteTime);
                    }
                    break;
                case SortType.LastAccessTime:
                    if (so == SortOrder.Decending)
                    {
                        orderedFiles = files.OrderBy(f => f.LastAccessTime).Reverse();
                    }
                    else
                    {
                        orderedFiles = files.OrderBy(f => f.LastAccessTime);
                    }
                    break;
                case SortType.CreationTime:
                    if (so == SortOrder.Decending)
                    {
                        orderedFiles = files.OrderBy(f => f.CreationTime).Reverse();
                    }
                    else
                    {
                        orderedFiles = files.OrderBy(f => f.CreationTime);
                    }
                    break;
                default:
                    orderedFiles = files.OrderBy(f => f.LastWriteTime).Reverse();
                    break;
            }

            return orderedFiles;
        }

        public enum SortOrder
        {
            Ascending,
            Decending
        }

        public enum SortType
        {
            LastWriteTime,
            LastAccessTime,
            CreationTime
        }

        /// <summary>
        /// Keeps the X specified newest files in a directory.  The rest of the files are deleted over the specified threshold.  This overload
        /// uses the LastWriteTime to determine what files to keep.
        /// </summary>
        /// <param name="dir">The directory to truncate files in.  This does not recurse through child directories.</param>
        /// <param name="numberToKeep">The number of files to keep.</param>
        /// <remarks></remarks>
        public static void TruncateFiles(string dir, int numberToKeep)
        {
            TruncateFiles(dir, numberToKeep, SortType.LastWriteTime);
        }

        /// <summary>
        /// Keeps the X specified newest files in a directory.  The rest of the files are deleted over the specified threshold.
        /// </summary>
        /// <param name="dir">The directory to truncate files in.  This does not recurse through child directories.</param>
        /// <param name="numberToKeep">The number of files to keep.</param>
        /// <param name="dateToUse">The date attribute to use.</param>
        /// <remarks></remarks>
        public static void TruncateFiles(string dir, int numberToKeep, SortType dateToUse)
        {
            dir = dir.Trim('\\');
            DirectoryInfo di = new DirectoryInfo(dir);
            FileSystemInfo[] files = di.GetFileSystemInfos();
            IEnumerable<FileSystemInfo> orderedFiles = null;

            switch (dateToUse)
            {
                case SortType.CreationTime:
                    orderedFiles = files.OrderBy(f => f.CreationTime).Reverse();
                    break;
                case SortType.LastAccessTime:
                    orderedFiles = files.OrderBy(f => f.LastAccessTime).Reverse();
                    break;
                case SortType.LastWriteTime:
                    orderedFiles = files.OrderBy(f => f.LastWriteTime).Reverse();
                    break;
                default:
                    orderedFiles = files.OrderBy(f => f.LastWriteTime).Reverse();
                    break;
            }

            int counter = 0;

            foreach (var fi in orderedFiles)
            {
                counter += 1;

                if (counter > numberToKeep)
                {
                    System.IO.File.Delete(fi.FullName);
                }
            }
        }

        /// <summary>
        /// Removes any illegal characters from the filename.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string CleanupFilename(string filename)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c.ToString(), "");
            }
            return filename;
        }

        /// <summary>
        /// Reads a specified line from a text file.  Each call to this opens and loops until the specific line is found (since lines are variable length an index cannot be used).
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        /// <remarks>This should be used in cases when a specific line is needed but you want to disregard the rest of the file (e.g. don't use this to loop through a file as it opens
        /// and finds the specific line going through all previous lines).</remarks>
        public static string ReadSpecificLine(string filePath, int lineNumber)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                // Skip these lines
                for (int i = 1; i <= lineNumber - 1; i++)
                {
                    if (sr.ReadLine() == null)
                    {
                        sr.Close();
                        throw new ArgumentOutOfRangeException(string.Format("The line number {0} is out of range.", lineNumber));
                    }
                }

                // This is the line we want, if it exists return it, otherwise throw an exception.
                string line = sr.ReadLine();
                if (line == null)
                {
                    throw new ArgumentOutOfRangeException(string.Format("The line number {0} is out of range.", lineNumber));
                }

                sr.Close();

                return line;
            }

        }

        /// <summary>
        /// Date types supported for the SafeFileType extension method.
        /// </summary>
        public enum DateType
        {
            /// <summary>
            /// The creation time for the file.
            /// </summary>
            CreationTime,
            /// <summary>
            /// The UTC creation time for the file.
            /// </summary>
            CreationTimeUtc,
            /// <summary>
            /// The last write time for the file.
            /// </summary>
            LastWriteTime,
            /// <summary>
            /// The UTC last write time for the file.
            /// </summary>
            LastWriteTimeUtc,
            /// <summary>
            /// The last access time for the file.
            /// </summary>
            LastAccessTime,
            /// <summary>
            /// The UTC last access time for the file.
            /// </summary>
            LastAccessTimeUtc
        }

        /// <summary>
        /// Returns the requested file time with all exceptions handled, a null will be returned
        /// when no file time is accessible for any reason.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime? SafeFileTime(string filePath, DateType dt)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }

            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(filePath);

                switch (dt)
                {
                    case DateType.LastAccessTime:
                        return fi.LastAccessTime;
                    case DateType.LastAccessTimeUtc:
                        return fi.LastAccessTimeUtc;
                    case DateType.LastWriteTime:
                        return fi.LastWriteTime;
                    case DateType.LastWriteTimeUtc:
                        return fi.LastWriteTimeUtc;
                    case DateType.CreationTime:
                        return fi.CreationTime;
                    case DateType.CreationTimeUtc:
                        return fi.CreationTimeUtc;
                    default:
                        return null;
                }

            }
            catch
            {
                return null;
            }

        }

    }

}