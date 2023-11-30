/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2023-11-30
 * @last updated      : 2023-11-30
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Extensions
{
    /// <summary>
    /// Linq extensions.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Runs an <see cref="Action"/> against all items in an <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <remarks>
        /// It should be noted that this is last option extension because it will force a second
        /// enumeration on the <see cref="IQueryable{T}"/>.
        /// </remarks>
        public static IQueryable<T> Update<T>(this IQueryable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }

            return source;
        }   
    }
}