﻿/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2018-06-12
 * @last updated      : 2020-04-28
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Argus.Cryptography
{
    /// <summary>
    /// Various hashing methods and utilities.
    /// </summary>
    public static class HashUtilities
    {
        /// <summary>
        /// Shared function to output the hash from the specified hash algorithm.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="crypt"></param>
        /// <param name="enc"></param>
        public static string CreateHash(string str, HashAlgorithm crypt, Encoding enc)
        {
            var hash = new StringBuilder();

            foreach (byte theByte in crypt.ComputeHash(enc.GetBytes(str)))
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }

        /// <summary>
        /// Shared function to output the hash from the specified hash algorithm.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="crypt"></param>
        public static string CreateHash(byte[] b, HashAlgorithm crypt)
        {
            var hash = new StringBuilder();

            foreach (byte theByte in crypt.ComputeHash(b))
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }

        /// <summary>
        /// Shared function to output the hash from the specified hash algorithm.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="crypt"></param>
        public static string CreateHash(Stream s, HashAlgorithm crypt)
        {
            var hash = new StringBuilder();

            foreach (byte theByte in crypt.ComputeHash(s))
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }

        /// <summary>
        /// Returns a SHA1 hash for the provided string.  The default overload uses ASCII encoding.
        /// </summary>
        /// <param name="str"></param>
        public static string Sha1Hash(string str)
        {
            return Sha1Hash(str, Encoding.ASCII);
        }

        /// <summary>
        /// Returns a SHA1 hash for the inputted string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The Encoding to use when reading the bytes from the input string.</param>
        public static string Sha1Hash(string str, Encoding enc)
        {
            var crypt = new SHA1Managed();

            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        /// Returns a SHA1 hash for the inputted string.
        /// </summary>
        /// <param name="b"></param>
        public static string Sha1Hash(byte[] b)
        {
            var crypt = new SHA1Managed();

            return CreateHash(b, crypt);
        }

        /// <summary>
        /// Returns a SHA1 hash for the inputted string.
        /// </summary>
        /// <param name="s"></param>
        public static string Sha1Hash(Stream s)
        {
            var crypt = new SHA1Managed();

            return CreateHash(s, crypt);
        }

        /// <summary>
        /// Returns a SHA384 hash for the inputted string.  The default overload uses ASCII encoding.
        /// </summary>
        /// <param name="str"></param>
        public static string Sha384Hash(string str)
        {
            return Sha384Hash(str, Encoding.ASCII);
        }

        /// <summary>
        /// Returns a SHA384 hash for the inputted string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The Encoding to use when reading the bytes from the input string.</param>
        public static string Sha384Hash(string str, Encoding enc)
        {
            var crypt = new SHA384Managed();

            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        /// Returns a SHA384 hash for the inputted string.
        /// </summary>
        /// <param name="b"></param>
        public static string Sha384Hash(byte[] b)
        {
            var crypt = new SHA384Managed();

            return CreateHash(b, crypt);
        }

        /// <summary>
        /// Returns a SHA384 hash for the inputted string.
        /// </summary>
        /// <param name="s"></param>
        public static string Sha384Hash(Stream s)
        {
            var crypt = new SHA384Managed();

            return CreateHash(s, crypt);
        }

        /// <summary>
        /// Returns a SHA256 hash for the inputted string.  The default overload uses ASCII encoding.
        /// </summary>
        /// <param name="str"></param>
        public static string Sha256Hash(string str)
        {
            return Sha256Hash(str, Encoding.ASCII);
        }

        /// <summary>
        /// Returns a SHA256 hash for the inputted string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The Encoding to use when reading the bytes from the input string.</param>
        public static string Sha256Hash(string str, Encoding enc)
        {
            var crypt = new SHA256Managed();

            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        /// Returns a SHA256 hash for the inputted string.
        /// </summary>
        /// <param name="b"></param>
        public static string Sha256Hash(byte[] b)
        {
            var crypt = new SHA256Managed();

            return CreateHash(b, crypt);
        }

        /// <summary>
        /// Returns a SHA256 hash for the inputted string.
        /// </summary>
        /// <param name="s"></param>
        public static string Sha256Hash(Stream s)
        {
            var crypt = new SHA256Managed();

            return CreateHash(s, crypt);
        }

        /// <summary>
        /// Returns a SHA512 hash for the inputted string.  The default overload uses ASCII encoding.
        /// </summary>
        /// <param name="str"></param>
        public static string Sha512Hash(string str)
        {
            return Sha512Hash(str, Encoding.ASCII);
        }

        /// <summary>
        /// Returns a SHA512 hash for the inputted string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The Encoding to use when reading the bytes from the input string.</param>
        public static string Sha512Hash(string str, Encoding enc)
        {
            var crypt = new SHA512Managed();

            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        /// Returns a SHA512 hash for the inputted string.
        /// </summary>
        /// <param name="b"></param>
        public static string Sha512Hash(byte[] b)
        {
            var crypt = new SHA512Managed();

            return CreateHash(b, crypt);
        }

        /// <summary>
        /// Returns a SHA512 hash for the inputted string.
        /// </summary>
        /// <param name="s"></param>
        public static string Sha512Hash(Stream s)
        {
            var crypt = new SHA512Managed();

            return CreateHash(s, crypt);
        }

        /// <summary>
        /// Returns a MD5 hash for the inputted string.  The default overload uses ASCII encoding.
        /// </summary>
        /// <param name="str"></param>
        public static string MD5Hash(string str)
        {
            return MD5Hash(str, Encoding.ASCII);
        }

        /// <summary>
        /// Returns a MD5 hash for the inputted string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The Encoding to use when reading the bytes from the input string.</param>
        public static string MD5Hash(string str, Encoding enc)
        {
            var crypt = new MD5CryptoServiceProvider();

            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        /// Returns a MD5 hash for the inputted string.
        /// </summary>
        /// <param name="b"></param>
        public static string MD5Hash(byte[] b)
        {
            var crypt = new MD5CryptoServiceProvider();

            return CreateHash(b, crypt);
        }

        /// <summary>
        /// Returns a MD5 hash for the inputted string.
        /// </summary>
        /// <param name="s"></param>
        public static string MD5Hash(Stream s)
        {
            var crypt = new MD5CryptoServiceProvider();

            return CreateHash(s, crypt);
        }
    }
}