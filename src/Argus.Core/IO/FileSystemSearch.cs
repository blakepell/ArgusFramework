/*
 * @author            : Blake Pell
 * @initial date      : 2006-12-10
 * @last updated      : 2025-05-10
 * @copyright         : Copyright (c) 2003-2025, All rights reserved.
 * @license           : MIT
 * @website           : http://www.blakepell.com
 */

using System.Collections.Concurrent;

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
        private readonly DirectoryInfo? _root;

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
        /// Cache for compiled Regex objects by pattern.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Regex> _regexCache = new();

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
            _patterns = new[] { pattern };
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
            if (_root == null || !_root.Exists || _patterns.Length == 0)
            {
                yield break;
            }

            foreach (var item in EnumerateFileSystem(_root, _patterns, _option))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Recursively enumerates the file system starting at the root directory.  Note that starting
        /// at the root directory of a drive will cause the enumerator to loop through every file on the
        /// drive if you're checking a False pattern.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="patterns"></param>
        /// <param name="option"></param>
        private IEnumerable<FileSystemInfo> EnumerateFileSystem(DirectoryInfo root, string[] patterns, SearchOption option)
        {
            IEnumerable<DirectoryInfo> directories = Enumerable.Empty<DirectoryInfo>();

            try
            {
                directories = root.EnumerateDirectories("*", SearchOption.TopDirectoryOnly);
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
                yield break;
            }

            // Yield directories if needed
            if (IncludeDirectories)
            {
                foreach (var dir in directories)
                {
                    if (!dir.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    {
                        yield return dir;
                    }
                }
            }

            if (patterns.Length > 1)
            {
                // Get all files in directory once
                IEnumerable<FileInfo> allFiles = Enumerable.Empty<FileInfo>();

                try
                {
                    allFiles = root.EnumerateFiles("*", SearchOption.TopDirectoryOnly);
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (PathTooLongException)
                {
                }
                catch (IOException)
                {
                }

                foreach (var file in allFiles)
                {
                    if (file.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    {
                        continue;
                    }

                    // Check if file matches any pattern
                    if (patterns.Any(pattern => PatternMatches(file.Name, pattern)))
                    {
                        yield return file;
                    }
                }
            }

            string pattern = patterns[0];

            // Yield files for each pattern: (Length can never be 0 here and greater than 1 is handled
            // above).  The below handles a pattern entry.
            IEnumerable<FileInfo> files = Enumerable.Empty<FileInfo>();

            try
            {
                files = root.EnumerateFiles(pattern, SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (PathTooLongException)
            {
            }
            catch (IOException)
            {
            }

            foreach (var file in files)
            {
                if (!file.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    yield return file;
                }
            }

            // Recurse into subdirectories if needed
            if (option == SearchOption.AllDirectories)
            {
                foreach (var dir in directories)
                {
                    if (dir.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    {
                        continue;
                    }

                    foreach (var item in EnumerateFileSystem(dir, patterns, option))
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Simple wildcard pattern matching with Regex caching.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="pattern"></param>
        public static bool PatternMatches(string filename, string pattern)
        {
            var regex = _regexCache.GetOrAdd(pattern, pat =>
                new Regex("^" + Regex.Escape(pat)
                                    .Replace(@"\*", ".*")
                                    .Replace(@"\?", ".") + "$",
                          RegexOptions.IgnoreCase | RegexOptions.Compiled));
            return regex.IsMatch(filename);
        }
    }
}
