/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2024-05-07
 * @last updated      : 2024-05-07
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

using System.Security.Cryptography;

namespace Argus.Cryptography
{
    /// <summary>
    /// Protects strings via System.Security.Cryptography.ProtectedDataManager.
    /// </summary>
    public static class ProtectedDataManager
    {
        /// <summary>
        /// Protects a string.
        /// </summary>
        /// <param name="str">The string to protect.</param>
        /// <param name="optionalEntropy">Entropy to make the protection unique.  In the absence of a provided value the current users username is used.</param>
        /// <param name="encoding">The default encoding is ASCII in the absence of a provided encoding.</param>
        /// <returns>The protected string.</returns>
        public static string Protect(string str, string? optionalEntropy = null, Encoding? encoding = null)
        {
            encoding ??= Encoding.ASCII;
            byte[] entropy = encoding.GetBytes(optionalEntropy ?? Environment.UserName);
            byte[] data = encoding.GetBytes(str);
            string protectedData = Convert.ToBase64String(System.Security.Cryptography.ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser));
            return protectedData;
        }

        /// <summary>
        /// Unprotects a string.
        /// </summary>
        /// <param name="str">The string to protect.</param>
        /// <param name="optionalEntropy">Entropy to undo the protection that should match the entropy used when
        /// Protect was called.  In the absence of a provided value the current users username is used.</param>
        /// <param name="encoding">The default encoding is ASCII in the absence of a provided encoding.</param>
        /// <returns>The unprotected string.</returns>
        public static string Unprotect(string str, string? optionalEntropy = null, Encoding? encoding = null)
        {
            encoding ??= Encoding.ASCII;
            byte[] protectedData = Convert.FromBase64String(str);
            byte[] entropy = encoding.GetBytes(optionalEntropy ?? Environment.UserName);
            string data = encoding.GetString(System.Security.Cryptography.ProtectedData.Unprotect(protectedData, entropy, DataProtectionScope.CurrentUser));
            return data;
        }
    }
}