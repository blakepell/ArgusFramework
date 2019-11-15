using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.ComponentModel;
using System.Text;

namespace Argus.Utilities
{

    /// <summary>
    /// Utilities for use when dealing with reflection and runtime getting, setting and displaying of properties
    /// of an object.
    /// </summary>
    /// <remarks></remarks>
    public class Reflection
    {
        //*********************************************************************************************************************
        //
        //             Class:  Reflection
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  07/01/2010
        //      Last Updated:  09/29/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Determines whether a property of a specific type is Browsable.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsBrowsable(Type t, string propertyName)
        {
            AttributeCollection attributes = TypeDescriptor.GetProperties(t)[propertyName].Attributes;

            // Only show the properties that can be set and whether it's browsable or not.                        
            if (attributes[typeof(BrowsableAttribute)].Equals(BrowsableAttribute.Yes))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether a property of a specific type is Browsable.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="platform"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsBrowsable(string typeName, Platforms platform, string propertyName)
        {
            return IsBrowsable(GetAppropriateType(platform, typeName), propertyName);
        }

        /// <summary>
        /// Determines whether a property of a specific type is Browsable and can be written to.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsBrowsableAndWritable(Type t, string propertyName)
        {
            PropertyInfo pi = t.GetProperty(propertyName);

            AttributeCollection attributes = TypeDescriptor.GetProperties(t)[propertyName].Attributes;

            // Only show the properties that can be set and whether it's browsable or not.                        
            if (attributes[typeof(BrowsableAttribute)].Equals(BrowsableAttribute.Yes) & pi.CanWrite)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether a property of a specific type is Browsable and can be written to.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="platform"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsBrowsableAndWritable(string typeName, Platforms platform, string propertyName)
        {
            return IsBrowsableAndWritable(GetAppropriateType(platform, typeName), propertyName);
        }

        /// <summary>
        /// Gets browsable properties from a type that you can also write to.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<System.Reflection.PropertyInfo> GetBrowsableWritableProperties(Type t)
        {
            List<System.Reflection.PropertyInfo> pList = new List<System.Reflection.PropertyInfo>();

            System.Reflection.PropertyInfo[] piList = t.GetProperties();

            foreach (System.Reflection.PropertyInfo pi in piList)
            {
                // Checks to see if the value of the BrowsableAttribute is Yes.
                System.ComponentModel.AttributeCollection attributes = TypeDescriptor.GetProperties(t)[pi.Name].Attributes;

                // Only show the properties that can be set and whether it's browsable or not.                        
                if (pi.CanWrite & attributes[typeof(BrowsableAttribute)].Equals(BrowsableAttribute.Yes))
                {
                    pList.Add(pi);
                }
            }

            return pList;
        }

        /// <summary>
        /// Gets browsable properties from a type that you can also write to.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<System.Reflection.PropertyInfo> GetBrowsableWritableProperties(string typeName, Platforms platform)
        {
            return GetBrowsableWritableProperties(GetAppropriateType(platform, typeName));
        }

        /// <summary>
        /// Gets just browsable properties from a type whether they are read only or writable.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<System.Reflection.PropertyInfo> GetBrowsableProperties(Type t)
        {
            List<System.Reflection.PropertyInfo> pList = new List<System.Reflection.PropertyInfo>();

            System.Reflection.PropertyInfo[] piList = t.GetProperties();

            foreach (System.Reflection.PropertyInfo pi in piList)
            {
                // Only show the properties that can be set and whether it's browsable or not.                        
                if (pi.CanWrite)
                {
                    pList.Add(pi);
                }
            }

            return pList;
        }

        /// <summary>
        /// Gets just browsable properties from a type whether they are read only or writable.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<System.Reflection.PropertyInfo> GetBrowsableProperties(string typeName, Platforms platform)
        {
            return GetBrowsableProperties(GetAppropriateType(platform, typeName));
        }

        /// <summary>
        /// Platforms
        /// </summary>
        /// <remarks></remarks>
        public enum Platforms
        {
            /// <summary>
            /// A client application (Console, WinForms, Wpf)
            /// </summary>
            /// <remarks></remarks>
            Client,
            /// <summary>
            /// A ASP.NET Web application
            /// </summary>
            /// <remarks></remarks>
            Web
        }

        /// <summary>
        /// Returns a type for the specified typeName.  The platform input will determine whether the function uses System.Type.GetType
        /// or System.Web.Compliation.BuildManager.GetType (which can read from classes in the ASP.Net's App_Code directory).
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Type GetAppropriateType(Platforms platform, string typeName)
        {
            switch (platform)
            {
                case Platforms.Client:
                    return System.Type.GetType(typeName);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns System.Type classes for all of the types found in the calling assembly.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<Type> GetTypesInAssembly(AssemblyTypes assemblyType)
        {
            List<Type> list = new List<Type>();
            Assembly asm = null;

            switch (assemblyType)
            {
                case AssemblyTypes.CallingAssembly:
                    asm = Assembly.GetCallingAssembly();
                    break;
                case AssemblyTypes.EntryAssembly:
                    asm = Assembly.GetEntryAssembly();
                    break;
                case AssemblyTypes.ExecutingAssembly:
                    asm = Assembly.GetExecutingAssembly();
                    break;
            }

            if (asm == null)
            {
                return new List<Type>();
            }

            foreach (Type t in asm.GetTypes())
            {
                list.Add(t);
            }

            return list;
        }

        /// <summary>
        /// Returns System.Type classes for all of the types found in the calling assembly filtered by the namespaceFilter parameter.
        /// </summary>
        /// <param name="namespaceFilter"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<Type> GetTypesInAssembly(AssemblyTypes assemblyType, string namespaceFilter)
        {
            List<Type> list = new List<Type>();
            Assembly asm = null;

            switch (assemblyType)
            {
                case AssemblyTypes.CallingAssembly:
                    asm = Assembly.GetCallingAssembly();
                    break;
                case AssemblyTypes.EntryAssembly:
                    asm = Assembly.GetEntryAssembly();
                    break;
                case AssemblyTypes.ExecutingAssembly:
                    asm = Assembly.GetExecutingAssembly();
                    break;
            }

            if (asm == null)
            {
                return new List<Type>();
            }

            foreach (Type t in asm.GetTypes())
            {
                if (t.Namespace == namespaceFilter)
                {
                    list.Add(t);
                }
            }

            return list;
        }

        /// <summary>
        /// The assembly types supported through Argus.Utilities.Reflection
        /// </summary>
        /// <remarks></remarks>
        public enum AssemblyTypes
        {
            CallingAssembly,
            EntryAssembly,
            ExecutingAssembly,
        }

        /// <summary>
        /// Returns a string containing the contents of the specified embedded resource from the executing assembly.
        /// </summary>
        /// <param name="name">The name of the file of the embedded resource.  This should include the root namespace preceding the file name.</param>
        /// <returns>A string with the contents of the embedded resource.</returns>
        /// <remarks></remarks>
        public static string GetEmbeddedResource(string name)
        {
            return GetEmbeddedResource(System.Reflection.Assembly.GetExecutingAssembly(), name);
        }

        /// <summary>
        /// Returns a string containing the contents of the specified embedded resource from the provided assembly.
        /// </summary>
        /// <param name="assembly">The System.Reflection.Assembly object you want to get the embedded resource from.</param>
        /// <param name="name">The name of the file of the embedded resource.  This should include the root namespace preceding the file name.</param>
        /// <returns>A string with the contents of the embedded resource.</returns>
        /// <remarks></remarks>
        public static string GetEmbeddedResource(System.Reflection.Assembly assembly, string name)
        {
            string buf = "";
            using (System.IO.Stream s = assembly.GetManifestResourceStream(name))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    buf = sr.ReadToEnd();
                    sr.Close();
                }

                s?.Close();
            }
            return buf;
        }

        /// <summary>
        /// Returns a CacheKey that is comprised off of the calling methods reflected namespace, class and
        /// method name plus the arguments that are passed in.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="args">Arguments unique to the cache item</param>
        /// <returns>String cache key used to cache and item.</returns>
        public static string GetCacheKey(params object[] args)
        {
            StackTrace stack = new StackTrace();
            MethodBase method = stack.GetFrame(1).GetMethod();
            StringBuilder cacheKey = new StringBuilder();

            // Append the calling namespace, class and method
            cacheKey.AppendFormat("{0}.{1}", method.ReflectedType?.FullName, method.Name);

            // Append the args, handle incoming types and clean them up
            foreach (object item in args)
            {
                if (item is string)
                {
                    cacheKey.AppendFormat("-{0}", item);
                }
                else if (item is DateTime)
                {
                    cacheKey.AppendFormat("-{0}", ((DateTime)item).ToShortDateString());
                }
                else
                {
                    cacheKey.AppendFormat("-{0}", item.ToString());
                }
            }

            return cacheKey.ToString();
        }

    }

}