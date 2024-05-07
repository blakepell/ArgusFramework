/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2024-05-07
 * @last updated      : 2024-05-07
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */


#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
using Argus.Cryptography;

namespace Argus.IO.Json
{
    /// <summary>
    /// JSON converter that will encrypt a string value on serialization and then decrypt it on read.
    /// </summary>
    public class ProtectedStringConverter : JsonConverter<ProtectedString>
    {
        /// <summary>
        /// Reads in a decrypts a string value that was protected.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        public override ProtectedString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? encryptedValue = reader.GetString();
            
            if (encryptedValue == null)
            {
                return "";
            }

            var ps = new ProtectedString
            {
                Value = encryptedValue
            };

            return ps;
        }

        /// <summary>
        /// Writes an encrypted/protected value into the string key.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, ProtectedString value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToProtectedBase64());
        }
    }
}

#endif