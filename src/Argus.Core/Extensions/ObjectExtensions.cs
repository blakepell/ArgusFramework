﻿/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2010-03-08
 * @last updated      : 2017-09-19
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension Methods for all Objects.
    /// </summary>
    public static class ObjectExtensions
    {
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
                var temp = (T) serializer.Deserialize(stream);
            }

            return obj;
        }
    }
}