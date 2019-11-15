﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension Methods for all Objects.
    /// </summary>
    public static class ObjectExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  ObjectExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  03/08/2010
        //      Last Updated:  09/19/2017
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

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
            return JsonConvert.SerializeObject(obj, null, Formatting.None, new JsonSerializerSettings { MaxDepth = recursionDepth });
        }

        /// <summary>
        /// Returns the Description attribute that is attached to the object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DescriptionAttribute(this object value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Converts a serializble object into it's XML representation.  This makes a call to the Argus.Utilities.Serialization.ObjectToXML function.
        /// Due to issues with generics, this will need to be re-read in with Argus.Utilities.Serialization and not from an extension since the (Of T)
        /// needs to be specified and cannot be obtained with GetType.  If an object cannot be serialized this method will throw an excpeption.
        /// </summary>
        /// <param name="obj"></param>
        public static string ToXml(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentException("Object cannot be null");
            }
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(stream, obj);
                    stream.Flush();
                    return Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Position);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deserializes an object into object format from XML.
        /// <code>
        /// Dim listTwo As New Argus.Web.LinkItem
        /// listTwo.FromXml(Of Argus.Web.LinkItem)(myXml)
        /// </code>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="xml"></param>
        /// <remarks>
        /// ByRef was used to what it could set the value of the current object.
        /// </remarks>
        public static void FromXml<T>(this object obj, string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("XML cannot be null/empty");
            }
            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    obj = (T)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}