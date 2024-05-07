/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2024-05-07
 * @last updated      : 2024-05-07
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Cryptography
{
    /// <summary>
    /// Represents a string value protected with using System.Security.Cryptography.ProtectedData.
    /// </summary>
    public class ProtectedString
    {
        /// <summary>
        /// Represents a string value protected with using System.Security.Cryptography.ProtectedData.
        /// </summary>
        public ProtectedString()
        {
            
        }

        /// <summary>
        /// Represents a string value protected with using System.Security.Cryptography.ProtectedData.
        /// </summary>
        /// <param name="value">The unprotected string value that will be stored protected.</param>
        public ProtectedString(string value)
        {
            this.Value = ProtectedDataManager.Protect(value);
        }

        /// <summary>
        /// Implicit conversion from ProtectedString to string
        /// </summary>
        /// <param name="ps"></param>
        public static implicit operator string?(ProtectedString ps)
        {
            if (ps.Value == null)
            {
                return null;
            }

            return ProtectedDataManager.Unprotect(ps.Value);
        }

        /// <summary>
        /// Implicit conversion from string to ProtectedString
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ProtectedString(string value)
        {
            return new ProtectedString(value);
        }

        /// <summary>
        /// Equality operator to compare ProtectedString and string
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="str"></param>
        public static bool operator ==(ProtectedString ps, string str)
        {
            return ps?.Value == ProtectedDataManager.Protect(str);
        }

        /// <summary>
        /// Inequality operator to compare ProtectedString and string
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool operator !=(ProtectedString ps, string str)
        {
            return ps?.Value != ProtectedDataManager.Protect(str);
        }

        /// <summary>
        /// The protected string.  This value in this property is always the base64 encoding of the encrypted value.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Returns the unprotected value.
        /// </summary>
        /// <returns>The unprotected value.</returns>
        public override string? ToString()
        {
            if (this.Value == null)
            {
                return null;
            }

            return ProtectedDataManager.Unprotect(this.Value);
        }

        /// <summary>
        /// Returns the Base64 value of the encrypted/protected string.
        /// </summary>
        public string? ToProtectedBase64()
        {
            return this.Value;
        }

        /// <summary>
        /// <inheritdoc cref="Equals"/>
        /// </summary>
        /// <param name="obj"></param>
        public override bool Equals(object? obj)
        {
            if (obj is ProtectedString ps)
            {
                return this.Value == ps.Value;
            }

            if (obj is string str)
            {
                return this.Value == ProtectedDataManager.Protect(str);
            }

            return false;
        }

        /// <summary>
        /// <inheritdoc cref="GetHashCode"/>
        /// </summary>
        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(this.Value))
            {
                return string.Empty.GetHashCode();
            }

            return this.Value.GetHashCode();
        }
    }
}
