/*
 * @author            : Blake Pell
 * @initial date      : 2010-07-07
 * @last updated      : 2023-06-08
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

namespace Argus.IO
{
    /// <summary>
    /// Utility methods for dealing with common file system operations.
    /// </summary>
    public static class FileSystemUtilities
    {
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
        /// The supported sort orders.
        /// </summary>
        public enum SortOrder
        {
            /// <summary>
            /// Ascending sort from first to last.
            /// </summary>
            Ascending,

            /// <summary>
            /// Descending sort from last to first.
            /// </summary>
            Descending
        }

        /// <summary>
        /// The supported file sorts.
        /// </summary>
        public enum SortType
        {
            /// <summary>
            /// The last time the file was written to.
            /// </summary>
            LastWriteTime,

            /// <summary>
            /// The last time the file was accessed.
            /// </summary>
            LastAccessTime,

            /// <summary>
            /// The time the file was originally created.
            /// </summary>
            CreationTime
        }

        /// <summary>
        /// Creates the full directory tree of a specified path.  This method will create all parent directories necessary.
        /// </summary>
        /// <param name="di"></param>
        public static void CreateDirectory(DirectoryInfo di)
        {
            if (di.Parent != null)
            {
                CreateDirectory(di.Parent);
            }

            if (di.Exists == false)
            {
                di.Create();
            }
        }

        /// <summary>
        /// Creates the full directory tree of a specified path.  This method will create all parent directories necessary.
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            var di = new DirectoryInfo(path.TrimEnd('\\'));
            CreateDirectory(di);
        }

        /// <summary>
        /// Extracts the file name off of a path.  This function first looks for a \ character and if it's not found will then
        /// look for a front slash as the separator.
        /// </summary>
        /// <param name="fullPath"></param>
        public static string ExtractFileName(string fullPath)
        {
            int lastSlash = fullPath.LastIndexOf('\\');

            if (lastSlash == -1)
            {
                lastSlash = fullPath.LastIndexOf('/');
            }

            return lastSlash == -1 ? fullPath : fullPath.Substring(lastSlash + 1);
        }

        /// <summary>
        /// Replaces the name of the file while preserving the extension of the original
        /// filepath.
        /// </summary>
        /// <param name="filepath">The file path.  Can include the entire path including directory.</param>
        /// <param name="newFilename">A new file name.  Should not include a directory path or extension.  This is the part of the file before the extension.</param>
        public static string ReplaceFilename(string filepath, string newFilename)
        {
            // Get the original file extension
            string extension = Path.GetExtension(filepath);

            // Get the directory path
            string? directory = Path.GetDirectoryName(filepath);

            // Build the new file path using the directory, new filename, and original extension
            return directory != null ? Path.Combine(directory, $"{newFilename}{extension}") : $"{newFilename}{extension}";
        }

        /// <summary>
        /// Checks to see if a file exists before deleting it.  This will catch and eat any exceptions for cases when you want
        /// a silent delete.
        /// </summary>
        /// <param name="filePath"></param>
        public static void SafeFileDelete(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            try
            {
                File.Delete(filePath);
            }
            catch
            {
                // eat error
            }
        }

        /// <summary>
        /// Checks to see if a directory exists before deleting it.  This will catch and eat any exceptions for cases when you want
        /// a silent delete.
        /// </summary>
        /// <param name="dirPath"></param>
        public static void SafeDirectoryDelete(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                return;
            }

            try
            {
                Directory.Delete(dirPath);
            }
            catch
            {
                // eat error
            }
        }

        /// <summary>
        /// Deletes a directory and all of its contents.
        /// </summary>
        /// <param name="dirPath"></param>
        public static void DirectoryTreeDelete(string dirPath)
        {
            foreach (var subDirectoryPath in Directory.EnumerateDirectories(dirPath))
            {
                DirectoryTreeDelete(subDirectoryPath);
            }

            try
            {
                foreach (var filePath in Directory.EnumerateFiles(dirPath))
                {
                    var fileInfo = new FileInfo(filePath)
                    {
                        Attributes = FileAttributes.Normal
                    };
                    
                    fileInfo.Delete();
                }
                
                Directory.Delete(dirPath);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to delete directory \"{dirPath}\" => {e.Message}", e);
            }
        }

        /// <summary>
        /// Deletes files in a path by a specified pattern (e.g. *.txt)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        public static void DeleteFilesByPattern(string path, string pattern)
        {
            foreach (string f in Directory.GetFiles(path, pattern))
            {
                File.Delete(f);
            }
        }

        /// <summary>
        /// Gets all files in a directory and orders them in ascending or descending order by modified date or
        /// created date.
        /// </summary>
        /// <param name="dir">The directory to return files for.</param>
        /// <param name="st">Which attribute the directory should be sorted by.</param>
        /// <param name="so">The sort order.  Descending will sort newest to oldest.  Ascending will sort oldest to newest.</param>
        /// <returns>A generic string list.</returns>
        public static List<string> GetFilesByModifiedDate(string dir, SortType st, SortOrder so)
        {
            dir = dir.TrimEnd('\\');
            var di = new DirectoryInfo(dir);
            var files = di.GetFileSystemInfos();
            var orderedFiles = Enumerable.Empty<FileSystemInfo>();
            var returnList = new List<string>();

            switch (st)
            {
                case SortType.LastWriteTime:
                    if (so == SortOrder.Descending)
                    {
                        orderedFiles = files.OrderBy(f => f.LastWriteTime).Reverse();
                    }
                    else
                    {
                        orderedFiles = files.OrderBy(f => f.LastWriteTime);
                    }

                    break;
                case SortType.LastAccessTime:
                    if (so == SortOrder.Descending)
                    {
                        orderedFiles = files.OrderBy(f => f.LastAccessTime).Reverse();
                    }
                    else
                    {
                        orderedFiles = files.OrderBy(f => f.LastAccessTime);
                    }

                    break;
                case SortType.CreationTime:
                    if (so == SortOrder.Descending)
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
        /// Gets all files in a directory and orders them in ascending or descending order by modified date or
        /// created date.
        /// </summary>
        /// <param name="dir">The directory to return files for.</param>
        /// <param name="st">Which attribute the directory should be sorted by.</param>
        /// <param name="so">The sort order.  Descending will sort newest to oldest.  Ascending will sort oldest to newest.</param>
        /// <returns>A generic string list.</returns>
        public static IEnumerable<FileSystemInfo> GetFilesSystemInfosByModifiedDate(string dir, SortType st, SortOrder so)
        {
            dir = dir.Trim('\\');
            var di = new DirectoryInfo(dir);
            var files = di.GetFileSystemInfos();
            var orderedFiles = Enumerable.Empty<FileSystemInfo>();

            switch (st)
            {
                case SortType.LastWriteTime:
                    if (so == SortOrder.Descending)
                    {
                        orderedFiles = files.OrderBy(f => f.LastWriteTime).Reverse();
                    }
                    else
                    {
                        orderedFiles = files.OrderBy(f => f.LastWriteTime);
                    }

                    break;
                case SortType.LastAccessTime:
                    if (so == SortOrder.Descending)
                    {
                        orderedFiles = files.OrderBy(f => f.LastAccessTime).Reverse();
                    }
                    else
                    {
                        orderedFiles = files.OrderBy(f => f.LastAccessTime);
                    }

                    break;
                case SortType.CreationTime:
                    if (so == SortOrder.Descending)
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

        /// <summary>
        /// Keeps the X specified newest files in a directory.  The rest of the files are deleted over the specified threshold.  This overload
        /// uses the LastWriteTime to determine what files to keep.
        /// </summary>
        /// <param name="dir">The directory to truncate files in.  This does not recurse through child directories.</param>
        /// <param name="numberToKeep">The number of files to keep.</param>
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
        public static void TruncateFiles(string dir, int numberToKeep, SortType dateToUse)
        {
            dir = dir.Trim('\\');
            var di = new DirectoryInfo(dir);
            var files = di.GetFileSystemInfos();
            var orderedFiles = Enumerable.Empty<FileSystemInfo>();

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
                    File.Delete(fi.FullName);
                }
            }
        }

        /// <summary>
        /// Removes any illegal characters from the filename.
        /// </summary>
        /// <param name="filename"></param>
        public static string CleanupFilename(string filename)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
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
        /// <remarks>
        /// This should be used in cases when a specific line is needed but you want to disregard the rest of the file (e.g. don't use this to loop through a file as it opens
        /// and finds the specific line going through all previous lines).
        /// </remarks>
        public static string ReadSpecificLine(string filePath, int lineNumber)
        {
            using (var sr = new StreamReader(filePath))
            {
                // Skip these lines
                for (int i = 1; i <= lineNumber - 1; i++)
                {
                    if (sr.ReadLine() == null)
                    {
                        sr.Close();

                        throw new ArgumentOutOfRangeException($"The line number {lineNumber} is out of range.");
                    }
                }

                // This is the line we want, if it exists return it, otherwise throw an exception.
                string line = sr.ReadLine();

                if (line == null)
                {
                    throw new ArgumentOutOfRangeException($"The line number {lineNumber} is out of range.");
                }

                sr.Close();

                return line;
            }
        }

        /// <summary>
        /// Returns the requested file time with all exceptions handled, a null will be returned
        /// when no file time is accessible for any reason.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dt"></param>
        /// <returns>The requested file time or null if the file doesn't exist or an exception occurs.</returns>
        public static DateTime? SafeFileTime(string filePath, DateType dt)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            try
            {
                var fi = new FileInfo(filePath);

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

        /// <summary>
        /// Calculates the different in two long parameters representing file sizes and
        /// returns the english representation with KB, MB and GB listed.
        /// </summary>
        /// <param name="originalSize"></param>
        /// <param name="newSize"></param>
        public static string CalculateFileSizeDifference(long originalSize, long newSize)
        {
            long diffSize = originalSize - newSize;

            // Convert to KB
            double sizeInKb = (double)diffSize / 1024;

            if (sizeInKb < 1024)
            {
                return $"{sizeInKb:N2} KB";
            }

            // Convert to MB
            double sizeInMb = sizeInKb / 1024;

            if (sizeInMb < 1024)
            {
                return $"{sizeInMb:N2} MB";
            }

            // Convert to GB
            double sizeInGb = sizeInMb / 1024;
            return $"{sizeInGb:N2} GB";
        }
    }
}