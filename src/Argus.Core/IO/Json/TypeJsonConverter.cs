/*
 * @author            : Microsoft, Blake Pell
 * @initial date      : 2022-07-05
 * @last updated      : 2022-07-05
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT 
 */

#if NET5_0_OR_GREATER

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Argus.IO.Json
{
    /// <summary>
    /// A <see cref="JsonConverter"/> for <see cref="Type?"/> that ignores serializing it.  The
    /// System.Text.Json serializer will throw an exception for security reasons if type is serialized so
    /// this acts as a stub that does nothing when type is found.
    /// </summary>
    public class NullableTypeJsonConverter : JsonConverter<Type?>
    {
        public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Type? value, JsonSerializerOptions options)
        {
            
        }
    }

    /// <summary>
    /// A <see cref="JsonConverter"/> for <see cref="Type"/> that ignores serializing it.  The
    /// System.Text.Json serializer will throw an exception for security reasons if type is serialized so
    /// this acts as a stub that does nothing when type is found.
    /// </summary>
    public class TypeJsonConverter : JsonConverter<Type>
    {
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {

        }
    }
}

#endif