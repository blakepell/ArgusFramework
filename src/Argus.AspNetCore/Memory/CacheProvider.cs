/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Microsoft.Extensions.Caching.Memory;

namespace Argus.AspNetCore.Memory
{
    /// <summary>
    /// Wraps an IMemoryCache and provides tracking for the Keys that have been stored as methods
    /// to clear the cache.
    /// </summary>
    public class CacheProvider
    {
        private readonly IMemoryCache _innerCache;

        private static List<string> _keys = new List<string>();

        public CacheProvider(IMemoryCache memoryCache)
        {
            _innerCache = memoryCache;
        }

        /// <summary>
        /// Exposes the static _keys list
        /// </summary>
        public IEnumerable<string> Keys => _keys;

        /// <summary>
        /// Checks if an item is in the cache.  If it is, the item is returned.
        ///
        /// <para>
        ///     If the item is not found in cache, the <paramref name="getItemFunc"/> function will be
        ///     called to retrieve and store the item.
        /// </para>
        /// <para>
        ///     By default, the only <see cref="MemoryCacheEntryOptions"/> set is `AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)`.
        /// </para>
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="getItemFunc"></param>
        public T Get<T>(string cacheKey, Func<T> getItemFunc)
        {
            var defaultCacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            };

            return Get(cacheKey, defaultCacheOptions, getItemFunc);
        }

        /// <summary>
        /// Checks if an item is in the cache.  If it is, the item is returned.
        ///
        /// <para>
        ///     If the item is not found in cache, the <paramref name="getItemFunc"/> function will be
        ///     called to retrieve and store the item.
        /// </para>
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheOptions"></param>
        /// <param name="getItemFunc"></param>
        public T Get<T>(string cacheKey, MemoryCacheEntryOptions cacheOptions, Func<T> getItemFunc)
        {
            if (_innerCache.TryGetValue(cacheKey, out T item))
            {
                return item;
            }

            item = getItemFunc();

            if (item == null)
            {
                return default;
            }

            if (!_keys.Contains(cacheKey))
            {
                _keys.Add(cacheKey);
            }

            _innerCache.Set(cacheKey, item, cacheOptions);

            return item;
        }

        /// <summary>
        /// Checks if an item is in the cache.  If it is, the item is returned.
        ///
        /// <para>
        ///     If the item is not found in cache, the <paramref name="getItemFunc"/> function will be
        ///     called to retrieve and store the item.
        /// </para>
        /// /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="cacheOptions"></param>
        /// <param name="getItemFunc"></param>
        public async Task<T> GetAsync<T>(string cacheKey, MemoryCacheEntryOptions cacheOptions, Func<Task<T>> getItemFunc)
        {
            if (_innerCache.TryGetValue(cacheKey, out T item))
            {
                return item;
            }

            item = await getItemFunc();

            if (item == null)
            {
                return default;
            }

            if (!_keys.Contains(cacheKey))
            {
                _keys.Add(cacheKey);
            }

            _innerCache.Set(cacheKey, item, cacheOptions);

            return item;
        }

        /// <summary>
        /// Removes item from cache.
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _innerCache.Remove(key);
            _keys.Remove(key);
        }

        /// <summary>
        /// Removes all items tracked by this CacheProvider.
        /// </summary>
        public void Reset()
        {
            _keys.ForEach(_innerCache.Remove);
            _keys = new List<string>();
        }

        /// <summary>
        /// Whether or not the given key currently exists in the cache.  If it does not exist
        /// the static list if keys is checked to remove it from there. 
        /// </summary>
        public bool Exists(string key)
        {
            if (_innerCache.TryGetValue(key, out object item))
            {
                return true;
            }

            if (_keys.Contains(key))
            {
                _keys.Remove(key);
            }

            return false;
        }

        /// <summary>
        /// Refreshes the list of keys ensuring that all still exist.  If a key
        /// doesn't exist it's removed from the cache.
        /// </summary>
        public void RefreshKeys()
        {
            foreach (string key in _keys)
            {
                if (!_innerCache.TryGetValue(key, out object item))
                {
                    _keys.Remove(key);
                }
            }
        }
    }
}