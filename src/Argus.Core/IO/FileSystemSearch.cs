/*
 * @author            : Blake Pell
 * @initial date      : 2006-12-10
 * @last updated      : 2021-09-19
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

namespace Argus.IO
{
    /// <summary>
    /// Searches the file system but also handles common exceptions that cause the .NET
    /// provided enumerable to fail in cases such as accessing junctions, symbolic links, etc.
    /// </summary>
    public class FileSystemSearch : IEnumerable<FileSystemInfo>
    {
        /// <summary>
        /// If directories should be included.
        /// </summary>
        public bool IncludeDirectories { get; set; } = true;

        /// <summary>
        /// The <see cref="DirectoryInfo"/> of the root directory to search.
        /// </summary>
        private readonly DirectoryInfo _root;

        /// <summary>
        /// A list of the search patterns that should be searched if multiple patterns exist.
        /// </summary>
        private readonly string[] _patterns;

        /// <summary>
        /// Whether the search should look in the top directory only or recurse through
        /// all of the folders in the tree.
        /// </summary>
        private readonly SearchOption _option;

        /// <summary>
        /// Constructor: All files in a directory or directory tree.
        /// </summary>
        /// <param name="directoryPath">The path to the directory to search.</param>
        /// <param name="option">Whether to search only the top directory or all directories.</param>
        public FileSystemSearch(string directoryPath, SearchOption option)
        {
            _root = new DirectoryInfo(directoryPath);
            _patterns = new[] { "*" };
            _option = option;
        }

        /// <summary>
        /// Constructor: All files that meet a pattern in a directory or directory tree.
        /// </summary>
        /// <param name="directoryPath">The path to the directory to search.</param>
        /// <param name="pattern">The pattern to search, e.g. "*"</param>
        /// <param name="option">Whether to search only the top directory or all directories.</param>
        public FileSystemSearch(string directoryPath, string pattern, SearchOption option)
        {
            _root = new DirectoryInfo(directoryPath);
            _patterns = new[] { pattern };
            _option = option;
        }

        /// <summary>
        /// Constructor: All files that meet a pattern in a directory or directory tree.
        /// </summary>
        /// <param name="root">The directory to search via a <see cref="DirectoryInfo"/></param>
        /// <param name="pattern">The pattern to search, e.g. "*"</param>
        /// <param name="option">Whether to search only the top directory or all directories.</param>
        public FileSystemSearch(DirectoryInfo root, string pattern, SearchOption option)
        {
            _root = root;
            _patterns = new [] { pattern };
            _option = option;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="root">The directory to search via a <see cref="DirectoryInfo"/></param>
        /// <param name="patterns">A list of patterns to to search, e.g. "*"</param>
        /// <param name="option">Whether to search only the top directory or all directories.</param>
        public FileSystemSearch(DirectoryInfo root, string[] patterns, SearchOption option)
        {
            _root = root;
            _patterns = patterns;
            _option = option;
        }

        /// <summary>
        /// Returns an enumerable of <see cref="FileSystemInfo"/> patterns for the search criteria.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<FileSystemInfo> GetEnumerator()
        {
            if (_root == null || !_root.Exists)
            {
                yield break;
            }

            IEnumerable<FileSystemInfo> matches = new List<FileSystemInfo>();

            try
            {
                foreach (var pattern in _patterns)
                {
                    // All directories but filter the files.
                    matches = matches.Concat(_root.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                                     .Concat(_root.EnumerateFiles(pattern, SearchOption.TopDirectoryOnly));
                }
            }
            catch (UnauthorizedAccessException)
            {
                yield break;
            }
            catch (PathTooLongException)
            {
                yield break;
            }
            catch (IOException)
            {
                // "The symbolic link cannot be followed because its type is disabled."
                // "The specified network name is no longer available."
                yield break;
            }

            foreach (var file in matches)
            {
                // Skip reparse points and optionally directories.
                if (file.Attributes.HasFlag(FileAttributes.ReparsePoint)
                    || (!this.IncludeDirectories && file.Attributes.HasFlag(FileAttributes.Directory)))
                {
                    continue;
                }

                yield return file;
            }

            if (_option == SearchOption.AllDirectories)
            {
                foreach (var dir in _root.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    var fileSystemInfos = new FileSystemSearch(dir, _patterns, _option);

                    foreach (var match in fileSystemInfos)
                    {
                        // Skip reparse points and optionally directories.
                        if (match.Attributes.HasFlag(FileAttributes.ReparsePoint)
                            || (!this.IncludeDirectories && match.Attributes.HasFlag(FileAttributes.Directory)))
                        {
                            continue;
                        }

                        yield return match;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}