/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2018-06-12
 * @last updated      : 2022-07-01
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using Cysharp.Text;
using System.Security.Cryptography;

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
            using (var sb = ZString.CreateStringBuilder())
            {
                foreach (byte theByte in crypt.ComputeHash(enc.GetBytes(str)))
                {
                    sb.Append(theByte.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Shared function to output the hash from the specified hash algorithm.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="crypt"></param>
        public static string CreateHash(byte[] b, HashAlgorithm crypt)
        {
            using (var sb = ZString.CreateStringBuilder())
            {
                foreach (byte theByte in crypt.ComputeHash(b))
                {
                    sb.Append(theByte.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Shared function to output the hash from the specified hash algorithm.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="crypt"></param>
        public static string CreateHash(Stream s, HashAlgorithm crypt)
        {
            using (var sb = ZString.CreateStringBuilder())
            {
                foreach (byte theByte in crypt.ComputeHash(s))
                {
                    sb.Append(theByte.ToString("x2"));
                }

                return sb.ToString();
            }
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
            using var crypt = SHA1.Create();
            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        /// Returns a SHA1 hash for the inputted string.
        /// </summary>
        /// <param name="b"></param>
        public static string Sha1Hash(byte[] b)
        {
            using var crypt = SHA1.Create();
            return CreateHash(b, crypt);
        }

        /// <summary>
        /// Returns a SHA1 hash for the inputted string.
        /// </summary>
        /// <param name="s"></param>
        public static string Sha1Hash(Stream s)
        {
            using var crypt = SHA1.Create();
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
            using var crypt = SHA384.Create();
            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        /// Returns a SHA384 hash for the inputted string.
        /// </summary>
        /// <param name="b"></param>
        public static string Sha384Hash(byte[] b)
        {
            using var crypt = SHA384.Create();
            return CreateHash(b, crypt);
        }

        /// <summary>
        /// Returns a SHA384 hash for the inputted string.
        /// </summary>
        /// <param name="s"></param>
        public static string Sha384(Stream s)
        {
            using var crypt = SHA384.Create();
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
            using var crypt = SHA256.Create();
            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        /// Returns a SHA256 hash for the inputted string.
        /// </summary>
        /// <param name="b"></param>
        public static string Sha256Hash(byte[] b)
        {
            using var crypt = SHA256.Create();
            return CreateHash(b, crypt);
        }

        /// <summary>
        /// Returns a SHA256 hash for the inputted string.
        /// </summary>
        /// <param name="s"></param>
        public static string Sha256Hash(Stream s)
        {
            using var crypt = SHA256.Create();
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
            using var crypt = SHA512.Create();
            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        /// Returns a SHA512 hash for the inputted string.
        /// </summary>
        /// <param name="b"></param>
        public static string Sha512Hash(byte[] b)
        {
            using var crypt = SHA512.Create();
            return CreateHash(b, crypt);
        }

        /// <summary>
        /// Returns a SHA512 hash for the inputted string.
        /// </summary>
        /// <param name="s"></param>
        public static string Sha512Hash(Stream s)
        {
            using var crypt = SHA512.Create();
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
            using var crypt = MD5.Create();
            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        /// Returns a MD5 hash for the inputted string.
        /// </summary>
        /// <param name="b"></param>
        public static string MD5Hash(byte[] b)
        {
            using var crypt = MD5.Create();
            return CreateHash(b, crypt);
        }

        /// <summary>
        /// Returns a MD5 hash for the inputted string.
        /// </summary>
        /// <param name="s"></param>
        public static string MD5Hash(Stream s)
        {
            using var crypt = MD5.Create();
            return CreateHash(s, crypt);
        }

        /// <summary>
        /// Returns the CRC32 (Cyclic redundancy check) hash for the specified <see cref="string"/>.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc"></param>
        public static uint CRC32(string str, Encoding enc)
        {
            using var ms = str.ToMemoryStream(enc);
            var crypt = new Argus.IO.Compression.CRC32();
            return crypt.GetCrc32(ms);
        }

        /// <summary>
        /// Returns the CRC32 (Cyclic redundancy check) hash for the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="s"></param>
        public static uint CRC32(Stream s)
        {
            var crypt = new Argus.IO.Compression.CRC32();
            return crypt.GetCrc32(s);
        }

        /// <summary>
        /// Returns the CRC32 (Cyclic redundancy check) hash for the specified <see cref="byte"/> array.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static uint CRC32(byte[] b)
        {
            using var ms = new MemoryStream(b);
            var crypt = new Argus.IO.Compression.CRC32();
            return crypt.GetCrc32(ms);
        }
    }
}