/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2010-03-08
 * @last updated      : 2022-04-17
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

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

        /// <summary>
        /// Serializes an object into it's JSON (JavaScript Object Notation) representation.
        /// </summary>
        /// <param name="obj"></param>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Serializes an object into it's JSON (JavaScript Object Notation) representation.  This method is using
        /// Microsoft's JavaScriptSerializer.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="recursionDepth"></param>
        public static string ToJson(this object obj, int recursionDepth)
        {
            return JsonConvert.SerializeObject(obj, null, Formatting.None, new JsonSerializerSettings {MaxDepth = recursionDepth});
        }

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