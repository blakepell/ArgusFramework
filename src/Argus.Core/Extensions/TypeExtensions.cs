/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2016-01-17
 * @last updated      : 2022-07-26
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Extensions
{
    /// <summary>
    /// <see cref="Type"/> Extensions
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns whether a type is a nullable enum.
        /// </summary>
        /// <param name="t"></param>
        /// <remarks>http://stackoverflow.com/questions/2723048/checking-if-type-instance-is-a-nullable-enum-in-c-sharp</remarks>
        public static bool IsNullableEnum(this Type t)
        {
            var u = Nullable.GetUnderlyingType(t);
            return u is { IsEnum: true };
        }

        /// <summary>
        /// If the type is <see cref="Nullable" />
        /// </summary>
        /// <param name="t"></param>
        public static bool IsNullable(this Type t)
        {
            if (t.IsValueType)
            {
                // Value types are only nullable if they are Nullable<T>
                return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            // Reference types are always nullable
            return true;
        }

        /// <summary>
        /// Returns a value indicating if this <paramref name="type"/> has the Attribute
        /// given in the <typeparamref name="TAttribute"/> type argument.
        /// </summary>
        /// <typeparam name="TAttribute">The Attribute type for which to make the determination.</typeparam>
        /// <param name="type">The type for which to make the determination.</param>
        /// <returns>
        /// True if this <paramref name="type"/> has the given <typeparamref name="TAttribute"/>,
        /// otherwise false.
        /// </returns>
        public static bool HasAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return type.GetTypeInfo().GetCustomAttribute<TAttribute>(inherit: false) != null;
        }
    }
}