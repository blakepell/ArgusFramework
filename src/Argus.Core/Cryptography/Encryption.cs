/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2005-09-28
 * @last updated      : 2020-04-29
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Argus.Cryptography
{
    /// <summary>
    /// Encrypts and decrypts strings and byte arrays.
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// A sixteen character salt.  This is populated by default with a value but can
        /// be changed by the caller.
        /// </summary>
        public string Salt { get; set; } = "59dcb39baddd44d6";

        /// <summary>
        /// A sixteen character vector.  This is populated by default with a value but can
        /// be changed by the caller.
        /// </summary>
        public string Vector { get; set; } = "2Y84dfd6b8d7z1b6";

        /// <summary>
        /// Constructor
        /// </summary>
        public Encryption()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="salt">A 16 digit salt value.</param>
        public Encryption(string salt)
        {
            if (salt.Length != 16)
            {
                throw new Exception("Salt must be 16 digits long.");
            }

            this.Salt = salt;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="salt">A 16 digit salt value.</param>
        /// <param name="vector">A 16 digit vector value.</param>
        public Encryption(string salt, string vector)
        {
            if (salt.Length != 16)
            {
                throw new Exception("The salt value must be 16 digits long.");
            }

            if (vector.Length != 16)
            {
                throw new Exception("The vector value must be 16 digits long.");
            }

            this.Salt = salt;
            this.Vector = vector;
        }

        /// <summary>
        /// Encrypts a string to an encrypted Base64 encoded string via AES.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="password"></param>
        /// <returns>An encrypted Base64 encoded string.</returns>
        public string EncryptToString(string value, string password)
        {
            return EncryptToString<AesManaged>(value, password);
        }

        /// <summary>
        /// Encrypts a string with the provider T to an encrypted Base64 encoded string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public string EncryptToString<T>(string value, string password) where T : SymmetricAlgorithm, new()
        {
            return EncryptToString<T>(Encoding.UTF8.GetBytes(value), password);
        }

        /// <summary>
        /// Encrypts a byte array to a Base64 string via AES.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="password"></param>
        /// <returns>An encrypted Base64 encoded string.</returns>
        public string EncryptToString(byte[] value, string password)
        {
            return EncryptToString<AesManaged>(value, password);
        }

        /// <summary>
        /// Encrypts a byte array to an encrypted Base64 encoded string with the provider T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public string EncryptToString<T>(byte[] value, string password) where T : SymmetricAlgorithm, new()
        {
            var vectorBytes = Encoding.ASCII.GetBytes(this.Vector);
            var saltBytes = Encoding.ASCII.GetBytes(this.Salt);
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
        /// Encrypts a byte array to an encrypted byte array with AES.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public byte[] EncryptToBytes(byte[] value, string password)
        {
            return EncryptToBytes<AesManaged>(value, password);
        }

        /// <summary>
        /// Encrypts a byte array to an encrypted byte array with the provider T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public byte[] EncryptToBytes<T>(byte[] value, string password) where T : SymmetricAlgorithm, new()
        {
            var vectorBytes = Encoding.ASCII.GetBytes(this.Vector);
            var saltBytes = Encoding.ASCII.GetBytes(this.Salt);
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

            return encrypted;
        }

        /// <summary>
        /// Decrypts an AES encrypted string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public string DecryptToString(string value, string password)
        {
            return DecryptToString<AesManaged>(value, password);
        }

        /// <summary>
        /// Decrypts a string with the provider T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public string DecryptToString<T>(string value, string password) where T : SymmetricAlgorithm, new()
        {
            return DecryptToString<T>(Convert.FromBase64String(value), password);
        }

        /// <summary>
        /// Decrypts a string with the provider T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public string DecryptToString<T>(byte[] value, string password) where T : SymmetricAlgorithm, new()
        {
            var vectorBytes = Encoding.ASCII.GetBytes(this.Vector);
            var saltBytes = Encoding.ASCII.GetBytes(this.Salt);
            byte[] decrypted;

            int decryptedByteCount;

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

        /// <summary>
        /// Decrypts a byte array with AES.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public byte[] DecryptToBytes(byte[] value, string password)
        {
            return DecryptToBytes<AesManaged>(value, password);
        }

        /// <summary>
        /// Decrypts a string with the provider T provided a Base64 encoded string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public byte[] DecryptToBytes<T>(string value, string password) where T : SymmetricAlgorithm, new()
        {
            return DecryptToBytes<T>(Convert.FromBase64String(value), password);
        }

        /// <summary>
        /// Decrypts a byte array with the provider T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public byte[] DecryptToBytes<T>(byte[] value, string password) where T : SymmetricAlgorithm, new()
        {
            var vectorBytes = Encoding.ASCII.GetBytes(this.Vector);
            var saltBytes = Encoding.ASCII.GetBytes(this.Salt);
            byte[] decrypted;

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
                                _ = reader.Read(decrypted, 0, decrypted.Length);
                            }
                        }
                    }
                }
                catch
                {
                    return new byte[0];
                }

                cipher.Clear();
            }

            return decrypted;
        }

    }
}