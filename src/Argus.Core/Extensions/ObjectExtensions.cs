/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2010-03-08
 * @last updated      : 2022-07-01
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

#if NET6_0
using System.Text.Json;
#endif

using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension Methods for all Objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Sets a property's value via reflection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void Set<T>(this T obj, string propertyName, object value)
        {
            var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            prop?.SetValue(obj, value, null);
        }

        /// <summary>
        /// Gets a property value via reflection for an object and attempts to use Convert.ChangeType
        /// to change it into the correct type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        public static T Get<T>(this object obj, string propertyName)
        {
            var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            return (T)Convert.ChangeType(prop?.GetValue(obj, null), typeof(T));
        }

        /// <summary>
        /// If any object is a nullable object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static bool IsNullable<T>(this T obj)
        {
            return default(T) == null;
        }

        /// <summary>
        /// If the object is a nullable value type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        static bool IsNullableValueType<T>(this T obj)
        {
            return default(T) == null && typeof(T).BaseType != null && "ValueType".Equals(typeof(T).BaseType.Name);
        }


#if NET5_0_OR_GREATER
        /// <summary>
        /// Serializes an object into it's JSON representation.
        /// </summary>
        /// <param name="obj"></param>
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        /// <summary>
        /// Serializes an object into it's JSON representation.
        /// </summary>
        /// <param name="obj"></param>
        public static string ToJson<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        /// <summary>
        /// Serializes an object into it's JSON representation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="writeIndented"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, bool writeIndented)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = writeIndented });
        }
#endif

#if NET6_0
        /// <summary>
        /// Serializes an object into it's JSON representation.
        /// </summary>
        /// <param name="obj"></param>
        public static async Task<string> ToJsonAsync<T>(this T obj)
        {
            using var s = new MemoryStream();
            await JsonSerializer.SerializeAsync(s, obj);
            s.Position = 0;
            using var reader = new StreamReader(s);
            return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Serializes an object into it's JSON representation.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="writeIndented"></param>
        public static async Task<string> ToJsonAsync<T>(this T obj, bool writeIndented)
        {
            using var s = new MemoryStream();
            await JsonSerializer.SerializeAsync(s, obj, new JsonSerializerOptions { WriteIndented = writeIndented });
            s.Position = 0;
            using var reader = new StreamReader(s);
            return await reader.ReadToEndAsync();
        }
#endif

        /// <summary>
        /// Converts a serializable object into it's XML representation.  This makes a call to the Argus.Utilities.Serialization.ObjectToXML function.
        /// Due to issues with generics, this will need to be re-read in with Argus.Utilities.Serialization and not from an extension since the (Of T)
        /// needs to be specified and cannot be obtained with GetType.  If an object cannot be serialized this method will throw an exception.
        /// </summary>
        /// <param name="obj"></param>
        public static string ToXml(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentException("Object cannot be null");
            }

            using (var stream = new MemoryStream())
            {
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(stream, obj);
                stream.Flush();

                return Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int) stream.Position);
            }
        }

        /// <summary>
        /// Deserializes an object into object format from XML.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="xml"></param>
        public static T FromXml<T>(this T obj, string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("XML cannot be null/empty");
            }

            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(xml)))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T) serializer.Deserialize(stream);
            }
        }
    }
}