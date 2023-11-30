/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2010-07-01
 * @last updated      : 2023-11-30
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
        /// Checks if an object has a specified property.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        private static bool HasProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }
        
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

        /// <summary>
        /// Returns a value indicating whether this <paramref name="property"/> is readable.
        /// </summary>
        /// <param name="property">The property for which to make the determination.</param>
        /// <returns>True if this <paramref name="property"/> is readable, otherwise false.</returns>
        public static bool IsReadable(this PropertyInfo property) => property.GetGetter() != null;

        /// <summary>
        /// Returns a value indicating whether this <paramref name="property"/> is writable.
        /// </summary>
        /// <param name="property">The property for which to make the determination.</param>
        /// <returns>True if this <paramref name="property"/> is writable, otherwise false.</returns>
        public static bool IsWritable(this PropertyInfo property) => property.GetSetter() != null;

        /// <summary>
        /// Returns the public or non-public get accessor for this <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The property for which to retrieve the accessor.</param>
        /// <param name="nonPublic">
        /// Whether a non-public get accessor should be returned: true if a non-public accessor should be 
        /// returned, otherwise false.
        /// </param>
        /// <returns>
        /// A MethodInfo representing the get accessor for this <paramref name="property"/>, if 
        /// <paramref name="nonPublic"/> is true. Returns null if <paramref name="nonPublic"/> is false and 
        /// the get accessor is non-public, or if <paramref name="nonPublic"/> is true but no get accessors exist.
        /// </returns>
        public static MethodInfo GetGetter(this PropertyInfo property, bool nonPublic = false)
        {
            return property.GetGetMethod(nonPublic);
        }

        /// <summary>
        /// Returns the public or non-public set accessor for this <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The property for which to retrieve the accessor.</param>
        /// <param name="nonPublic">
        /// Whether a non-public set accessor should be returned: true if a non-public accessor should be 
        /// returned, otherwise false.
        /// </param>
        /// <returns>
        /// A MethodInfo representing the set accessor for this <paramref name="property"/>, if 
        /// <paramref name="nonPublic"/> is true. Returns null if <paramref name="nonPublic"/> is false and 
        /// the set accessor is non-public, or if <paramref name="nonPublic"/> is true but no set accessors exist.
        /// </returns>
        public static MethodInfo GetSetter(this PropertyInfo property, bool nonPublic = false)
        {
            return property.GetSetMethod(nonPublic);
        }
    }
}