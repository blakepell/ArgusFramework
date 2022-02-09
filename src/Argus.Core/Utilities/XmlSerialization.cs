/*
 * @author            : Blake Pell
 * @initial date      : 2009-01-24
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System.Xml.Serialization;

namespace Argus.Utilities
{
    /// <summary>
    /// Helps with serializing an object to XML and back again.
    /// </summary>
    public static class XmlSerialization
    {
        /// <summary>
        /// Converts an object to xml
        /// </summary>
        /// <param name="obj">Object to convert</param>
        public static string ObjectToXml(object obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException("Object to serialize cannot be null");
            }

            using (var ms = new MemoryStream())
            {
                var xs = new XmlSerializer(obj.GetType());
                xs.Serialize(ms, obj);
                ms.Flush();

                return Encoding.UTF8.GetString(ms.ToArray(), 0, (int) ms.Position);
            }
        }

        /// <summary>
        /// Converts an xml string to an object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="xml">XML string</param>
        public static T XmlToObject<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("XML cannot be null/empty");
            }

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                var xs = new XmlSerializer(typeof(T));

                return (T) xs.Deserialize(stream);
            }
        }
    }
}