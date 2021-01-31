/*
 * @author            : Blake Pell
 * @initial date      : 2010-07-01
 * @last updated      : 2019-11-16
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Argus.Utilities
{
    /// <summary>
    /// Utilities for use when dealing with reflection and runtime getting, setting and displaying of properties
    /// of an object.
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// The assembly types supported through via this library.
        /// </summary>
        public enum AssemblyTypes
        {
            /// <summary>
            /// The Assembly of the method that invoked the currently executing method.
            /// </summary>
            CallingAssembly,

            /// <summary>
            /// The process executable in the default application domain. In other application
            /// domains, this is the first executable that was executed by ExecuteAssembly(String).
            /// </summary>
            EntryAssembly,

            /// <summary>
            /// The assembly that contains the code that is currently executing.
            /// </summary>
            ExecutingAssembly
        }

        /// <summary>
        /// Determines whether a property of a specific type is browsable.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propertyName"></param>
        public static bool IsBrowsable(Type t, string propertyName)
        {
            var attributes = TypeDescriptor.GetProperties(t)[propertyName].Attributes;

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
        /// <param name="propertyName"></param>
        public static bool IsBrowsable(string typeName, string propertyName)
        {
            var t = Type.GetType(typeName);

            return IsBrowsable(t, propertyName);
        }

        /// <summary>
        /// Determines whether a property of a specific type is Browsable and can be written to.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propertyName"></param>
        public static bool IsBrowsableAndWritable(Type t, string propertyName)
        {
            var pi = t.GetProperty(propertyName);

            var attributes = TypeDescriptor.GetProperties(t)[propertyName].Attributes;

            // Only show the properties that can be set and whether it's browsable or not.                        
            if (attributes[typeof(BrowsableAttribute)].Equals(BrowsableAttribute.Yes) & pi.CanWrite)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether a property of a specific type is Browsable and can be written to.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyName"></param>
        public static bool IsBrowsableAndWritable(string typeName, string propertyName)
        {
            var t = Type.GetType(typeName);

            return IsBrowsableAndWritable(t, propertyName);
        }

        /// <summary>
        /// Gets browsable properties from a type that you can also write to.
        /// </summary>
        /// <param name="t"></param>
        public static List<PropertyInfo> GetBrowsableWritableProperties(Type t)
        {
            var pList = new List<PropertyInfo>();
            var piList = t.GetProperties();

            foreach (var pi in piList)
            {
                // Checks to see if the value of the BrowsableAttribute is Yes.
                var attributes = TypeDescriptor.GetProperties(t)[pi.Name].Attributes;

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
        public static List<PropertyInfo> GetBrowsableWritableProperties(string typeName)
        {
            var t = Type.GetType(typeName);

            return GetBrowsableWritableProperties(t);
        }

        /// <summary>
        /// Gets just browsable properties from a type whether they are read only or writable.
        /// </summary>
        /// <param name="t"></param>
        public static List<PropertyInfo> GetBrowsableProperties(Type t)
        {
            var pList = new List<PropertyInfo>();

            var piList = t.GetProperties();

            foreach (var pi in piList)
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
        public static List<PropertyInfo> GetBrowsableProperties(string typeName)
        {
            var t = Type.GetType(typeName);

            return GetBrowsableProperties(t);
        }

        /// <summary>
        /// Returns System.Type classes for all of the types found in the calling assembly.
        /// </summary>
        public static List<Type> GetTypesInAssembly(AssemblyTypes assemblyType)
        {
            var list = new List<Type>();
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

            foreach (var t in asm.GetTypes())
            {
                list.Add(t);
            }

            return list;
        }

        /// <summary>
        /// Returns System.Type classes for all of the types found in the calling assembly filtered by the namespaceFilter parameter.
        /// </summary>
        /// <param name="assemblyType"></param>
        /// <param name="namespaceFilter"></param>
        public static List<Type> GetTypesInAssembly(AssemblyTypes assemblyType, string namespaceFilter)
        {
            var list = new List<Type>();
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

            foreach (var t in asm.GetTypes())
            {
                if (t.Namespace == namespaceFilter)
                {
                    list.Add(t);
                }
            }

            return list;
        }

        /// <summary>
        /// Returns a string containing the contents of the specified embedded resource from the executing assembly.
        /// </summary>
        /// <param name="name">The name of the file of the embedded resource.  This should include the root namespace preceding the file name.</param>
        /// <returns>A string with the contents of the embedded resource.</returns>
        public static string GetEmbeddedResource(string name)
        {
            return GetEmbeddedResource(Assembly.GetExecutingAssembly(), name);
        }

        /// <summary>
        /// Returns a string containing the contents of the specified embedded resource from the provided assembly.
        /// </summary>
        /// <param name="assembly">The System.Reflection.Assembly object you want to get the embedded resource from.</param>
        /// <param name="name">The name of the file of the embedded resource.  This should include the root namespace preceding the file name.</param>
        /// <returns>A string with the contents of the embedded resource.</returns>
        public static string GetEmbeddedResource(Assembly assembly, string name)
        {
            using (var s = assembly.GetManifestResourceStream(name))
            {
                using (var sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Returns a CacheKey that is comprised off of the calling methods reflected namespace, class and
        /// method name plus the arguments that are passed in.
        /// </summary>
        /// <param name="args">Arguments unique to the cache item</param>
        /// <returns>String cache key used to cache and item.</returns>
        public static string GetCacheKey(params object[] args)
        {
            var stack = new StackTrace();
            var method = stack.GetFrame(1).GetMethod();
            var cacheKey = new StringBuilder();

            // Append the calling namespace, class and method
            cacheKey.AppendFormat("{0}.{1}", method.ReflectedType?.FullName, method.Name);

            // Append the args, handle incoming types and clean them up
            foreach (var item in args)
            {
                if (item is string)
                {
                    cacheKey.AppendFormat("-{0}", item);
                }
                else if (item is DateTime)
                {
                    cacheKey.AppendFormat("-{0}", ((DateTime) item).ToShortDateString());
                }
                else
                {
                    cacheKey.AppendFormat("-{0}", item);
                }
            }

            return cacheKey.ToString();
        }
    }
}