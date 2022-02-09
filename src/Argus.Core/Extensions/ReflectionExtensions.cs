/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2010-07-01
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Utilities;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for reflection based operations.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Determines whether a property of a specific type is Browsable.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propertyName"></param>
        public static bool IsBrowsable(this Type t, string propertyName)
        {
            return Reflection.IsBrowsable(t, propertyName);
        }

        /// <summary>
        /// Determines whether a property of a specific type is Browsable and can be written to.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propertyName"></param>
        public static bool IsBrowsableAndWritable(this Type t, string propertyName)
        {
            return Reflection.IsBrowsableAndWritable(t, propertyName);
        }

        /// <summary>
        /// Gets browsable properties from a type that you can also write to.
        /// </summary>
        /// <param name="t"></param>
        public static List<PropertyInfo> GetBrowsableWritableProperties(this Type t)
        {
            return Reflection.GetBrowsableWritableProperties(t);
        }

        /// <summary>
        /// Gets just browsable properties from a type whether they are read only or writable.
        /// </summary>
        /// <param name="t"></param>
        public static List<PropertyInfo> GetBrowsableProperties(this Type t)
        {
            return Reflection.GetBrowsableProperties(t);
        }

        /// <summary>
        /// Returns a string containing the contents of the specified embedded resource from the provided assembly.
        /// </summary>
        /// <param name="assembly">The System.Reflection.Assembly object you want to get the embedded resource from.</param>
        /// <param name="name">The name of the file of the embedded resource.  This should include the root namespace preceding the file name.</param>
        /// <returns>A string with the contents of the embedded resource.</returns>
        public static string GetEmbeddedResource(this Assembly assembly, string name)
        {
            return Reflection.GetEmbeddedResource(assembly, name);
        }
    }
}