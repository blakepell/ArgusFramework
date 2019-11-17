using System.Security.Cryptography;
using System.Text;

namespace Argus.Cryptography
{
    /// <summary>
    ///     Various hashing methods and utilities.
    /// </summary>
    public class HashUtilities
    {
        //*********************************************************************************************************************
        //
        //             Class:  HashUtilities
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  06/12/2018
        //      Last Updated:  12/07/2018
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Shared function to output the hash from the specified hash algorithm.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="crypt"></param>
        /// <param name="enc"></param>
        private static string CreateHash(string str, HashAlgorithm crypt, Encoding enc)
        {
            var hash = new StringBuilder();
            var crypto = crypt.ComputeHash(enc.GetBytes(str));

            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }

        /// <summary>
        ///     Returns a SHA1 hash for the inputted string.  The default overload uses ASCII encoding.
        /// </summary>
        /// <param name="str"></param>
        public static string Sha1Hash(string str)
        {
            return Sha1Hash(str, Encoding.ASCII);
        }

        /// <summary>
        ///     Returns a SHA1 hash for the inputted string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The Encoding to use when reading the bytes from the input string.</param>
        public static string Sha1Hash(string str, Encoding enc)
        {
            var crypt = new SHA1Managed();

            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        ///     Returns a SHA384 hash for the inputted string.  The default overload uses ASCII encoding.
        /// </summary>
        /// <param name="str"></param>
        public static string Sha384Hash(string str)
        {
            return Sha384Hash(str, Encoding.ASCII);
        }

        /// <summary>
        ///     Returns a SHA384 hash for the inputted string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The Encoding to use when reading the bytes from the input string.</param>
        public static string Sha384Hash(string str, Encoding enc)
        {
            var crypt = new SHA384Managed();

            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        ///     Returns a SHA256 hash for the inputted string.  The default overload uses ASCII encoding.
        /// </summary>
        /// <param name="str"></param>
        public static string Sha256Hash(string str)
        {
            return Sha256Hash(str, Encoding.ASCII);
        }

        /// <summary>
        ///     Returns a SHA256 hash for the inputted string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The Encoding to use when reading the bytes from the input string.</param>
        public static string Sha256Hash(string str, Encoding enc)
        {
            var crypt = new SHA256Managed();

            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        ///     Returns a SHA512 hash for the inputted string.  The default overload uses ASCII encoding.
        /// </summary>
        /// <param name="str"></param>
        public static string Sha512Hash(string str)
        {
            return Sha512Hash(str, Encoding.ASCII);
        }

        /// <summary>
        ///     Returns a SHA512 hash for the inputted string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The Encoding to use when reading the bytes from the input string.</param>
        public static string Sha512Hash(string str, Encoding enc)
        {
            var crypt = new SHA512Managed();

            return CreateHash(str, crypt, enc);
        }

        /// <summary>
        ///     Returns a MD5 hash for the inputted string.  The default overload uses ASCII encoding.
        /// </summary>
        /// <param name="str"></param>
        public static string Md5Hash(string str)
        {
            return Md5Hash(str, Encoding.ASCII);
        }

        /// <summary>
        ///     Returns a MD5 hash for the inputted string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The Encoding to use when reading the bytes from the input string.</param>
        public static string Md5Hash(string str, Encoding enc)
        {
            var crypt = new MD5CryptoServiceProvider();

            return CreateHash(str, crypt, enc);
        }
    }
}