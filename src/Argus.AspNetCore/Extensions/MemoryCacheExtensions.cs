/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Microsoft.Extensions.Caching.Memory;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IMemoryCache" />.
    /// </summary>
    public static class MemoryCacheExtensions
    {
        /// <summary>
        /// Checks if an item is in the cache.  If it is, the item is returned.
        /// <para>
        /// If the item is not found in cache, the <paramref name="getItemFunc" /> function will be
        /// called to retrieve and store the item.
        /// </para>
        /// <para>
        /// By default, the only <see cref="MemoryCacheEntryOptions" /> set is `AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)`.
        /// </para>
        /// </summary>
        /// <param name="memoryCache"></param>
        /// <param name="cacheKey"></param>
        /// <param name="getItemFunc"></param>
        public static T Get<T>(this IMemoryCache memoryCache, string cacheKey, Func<T> getItemFunc)
        {
            var defaultCacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            };

            return Get(memoryCache, cacheKey, defaultCacheOptions, getItemFunc);
        }

        /// <summary>
        /// Checks if an item is in the cache.  If it is, the item is returned.
        /// <para>
        /// If the item is not found in cache, the <paramref name="getItemFunc" /> function will be
        /// called to retrieve and store the item.
        /// </para>
        /// </summary>
        /// <param name="memoryCache"></param>
        /// <param name="cacheKey"></param>
        /// <param name="cacheOptions"></param>
        /// <param name="getItemFunc"></param>
        public static T Get<T>(this IMemoryCache memoryCache, string cacheKey, MemoryCacheEntryOptions cacheOptions, Func<T> getItemFunc)
        {
            T item;

            if (memoryCache.TryGetValue(cacheKey, out item))
            {
                return item;
            }

            item = getItemFunc();

            if (item == null)
            {
                return default;
            }

            memoryCache.Set(cacheKey, item, cacheOptions);

            return item;
        }
    }
}