using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Argus.Utilities
{
    /// <summary>
    /// Helps with serializing an object to XML and back again.
    /// </summary>
    public static class XmlSerialization
    {
        //*********************************************************************************************************************
        //
        //            Module:  XmlSerialization
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  01/27/2009
        //      Last Updated:  04/04/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

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
                return Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Position);
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
                return (T)xs.Deserialize(stream);
            }
        }

    }
}