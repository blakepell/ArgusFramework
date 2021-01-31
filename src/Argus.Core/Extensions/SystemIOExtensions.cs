/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-04-06
 * @last updated      : 2019-11-18
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
            return HashUtilities.Sha256Hash(ReadFile(fi));
        }

        /// <summary>
        /// Returns a cryptographic SHA512 hash for the bytes in a given file.
        /// </summary>
        /// <param name="fi"></param>
        public static string CreateSha512Hash(this FileInfo fi)
        {
            return HashUtilities.Sha512Hash(ReadFile(fi));
        }

        /// <summary>
        /// Returns a cryptographic MD5 hash for the bytes in a given file.
        /// </summary>
        /// <param name="fi"></param>
        public static string CreateMD5(this FileInfo fi)
        {
            return HashUtilities.MD5Hash(ReadFile(fi));
        }

        /// <summary>
        /// This opens up a string that is able to read a locked file if the file is locked but shareable.
        /// </summary>
        /// <param name="fi"></param>
        private static byte[] ReadFile(FileInfo fi)
        {
            byte[] fileContents = null;

            using (var fs = File.Open(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int fileLength = Convert.ToInt32(fs.Length);
                fileContents = new byte[fileLength];
                fs.Read(fileContents, 0, fileLength);
            }

            return fileContents;
        }
    }
}