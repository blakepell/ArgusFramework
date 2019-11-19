using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Argus.Cryptography
{
    /// <summary>
    ///     Encrypts and decrypts strings and byte arrays.
    /// </summary>
    public class Encryption
    {
        //*********************************************************************************************************************
        //
        //             Class:  Encryption
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  03/26/2008
        //      Last Updated:  11/18/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        private static readonly string _salt = "aselrias38490a32";
        private static readonly string _vector = "8947az34awl34kjq";

        /// <summary>
        ///     Encrypts a string via AES.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public static string Encrypt(string value, string password)
        {
            return Encrypt<AesManaged>(value, password);
        }

        /// <summary>
        ///     Encrypts a string with the provider T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public static string Encrypt<T>(string value, string password) where T : SymmetricAlgorithm, new()
        {
            return Encrypt<T>(Encoding.UTF8.GetBytes(value), password);
        }

        /// <summary>
        ///     Encrypts a string with the provider T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public static string Encrypt<T>(byte[] value, string password) where T : SymmetricAlgorithm, new()
        {
            var vectorBytes = Encoding.ASCII.GetBytes(_vector);
            var saltBytes = Encoding.ASCII.GetBytes(_salt);
            byte[] encrypted;

            using (var cipher = new T())
            {
                var passwordBytes = new PasswordDeriveBytes(password, saltBytes, "SHA512", 2);
                var keyBytes = passwordBytes.GetBytes(256 / 8);

                cipher.Mode = CipherMode.CBC;
                cipher.GenerateIV();

                using (var encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                {
                    using (var to = new MemoryStream())
                    {
                        using (var writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(value, 0, value.Length);
                            writer.FlushFinalBlock();
                            encrypted = to.ToArray();
                        }
                    }
                }

                cipher.Clear();
            }

            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        ///     Decrypts an AES encrypted string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public static string Decrypt(string value, string password)
        {
            return Decrypt<AesManaged>(value, password);
        }

        /// <summary>
        ///     Decrypts a string with the provider T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public static string Decrypt<T>(string value, string password) where T : SymmetricAlgorithm, new()
        {
            return Decrypt<T>(Convert.FromBase64String(value), password);
        }

        /// <summary>
        ///     Decrypts a string with the provider T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public static string Decrypt<T>(byte[] value, string password) where T : SymmetricAlgorithm, new()
        {
            var vectorBytes = Encoding.ASCII.GetBytes(_vector);
            var saltBytes = Encoding.ASCII.GetBytes(_salt);
            byte[] decrypted;

            int decryptedByteCount = 0;

            using (var cipher = new T())
            {
                var passwordBytes = new PasswordDeriveBytes(password, saltBytes, "SHA512", 2);
                var keyBytes = passwordBytes.GetBytes(256 / 8);

                cipher.Mode = CipherMode.CBC;
                cipher.GenerateIV();

                try
                {
                    using (var decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                    {
                        using (var from = new MemoryStream(value))
                        {
                            using (var reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                            {
                                decrypted = new byte[value.Length];
                                decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                            }
                        }
                    }
                }
                catch
                {
                    return string.Empty;
                }

                cipher.Clear();
            }

            return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
        }
    }
}