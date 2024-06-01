/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2022-10-01
 * @last updated      : 2022-10-01
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Extensions
{
    /// <summary>
    /// Enumerable extension methods.
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// Picks one random item from an <see cref="IEnumerable"/>.  If the enumerable has no items an exception is thrown.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        /// <summary>
        /// Shuffles an <see cref="IEnumerable"/> and then returns the requested number of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        /// <summary>
        /// Shuffles / randomizes the items in an <see cref="IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}
