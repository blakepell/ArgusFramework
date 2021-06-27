/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-04-06
 * @last updated      : 2021-06-26
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.IO;
using Argus.Cryptography;
using Argus.Data;

namespace Argus.Extensions
{
    /// <summary>
    /// Extensions for methods in classes of the System.IO namespace.
    /// </summary>
    public static class SystemIOExtensions
    {
        /// <summary>
        /// Returns the file size formatted, such as 10 KB, 120 MB, 1.2 GB, 1.4 TB, etc.  This supports sizes up to and including Terabytes.
        /// </summary>
        /// <param name="fi"></param>
        public static string FormattedFileSize(this FileInfo fi)
        {
            return Formatting.FormattedFileSize(fi.Length);
        }

        /// <summary>
        /// Returns a cryptographic SHA256 hash for the bytes in a given file.
        /// </summary>
        /// <param name="fi"></param>
        public static string CreateSha256Hash(this FileInfo fi)
        {
            using (var fs = File.Open(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return HashUtilities.Sha256Hash(fs);
            }
        }

        /// <summary>
        /// Returns a cryptographic SHA512 hash for the bytes in a given file.
        /// </summary>
        /// <param name="fi"></param>
        public static string CreateSha512Hash(this FileInfo fi)
        {
            using (var fs = File.Open(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return HashUtilities.Sha512Hash(fs);
            }
        }

        /// <summary>
        /// Returns a cryptographic MD5 hash for the bytes in a given file.
        /// </summary>
        /// <param name="fi"></param>
        public static string CreateMD5(this FileInfo fi)
        {
            using (var fs = File.Open(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return HashUtilities.MD5Hash(fs);
            }
        }
    }
}