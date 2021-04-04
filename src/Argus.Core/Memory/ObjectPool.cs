/*
 * @author            : Blake Pell
 * @initial date      : 2020-02-27
 * @last updated      : 2021-04-04
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Argus.Memory
{
    /// <summary>
    /// Represents a pool of objects that can be reused (Note that data in those objects are not cleared between uses).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// If the <see cref="CounterNewObjects"/> counter is considerably higher than the <see cref="CounterReusedObjects"/> then it
    /// is possible that the pool <see cref="Max"/> is set too low.  In some cases the speed at which incoming requests might be
    /// occurring might outpace a small pool size.  Increasing the ceiling might allow a considerably better reuse rate.
    /// </remarks>
    public class ObjectPool<T> where T : new()
    {
        /// <summary>
        /// Holds the objects in the pool.
        /// </summary>
        private readonly ConcurrentBag<T> _items = new ConcurrentBag<T>();

        /// <summary>
        /// The maximum number of objects we will hold in the Pool.  Anything over this number is created and
        /// returned on Get but not pooled.
        /// </summary>
        public int Max { get; set; } = 10;

        /// <summary>
        /// The number of new objects that were created, either because the pool limit wasn't met or
        /// because the requests can in while the pool was at it's max limit.
        /// </summary>
        public int CounterNewObjects { get; private set; } = 0;

        /// <summary>
        /// The number of times an object from the pool was reused.
        /// </summary>
        public int CounterReusedObjects { get; private set; } = 0;

        /// <summary>
        /// Returns an object back into the pool.  If the item is not returned to the pool and
        /// it implements <see cref="IDisposable"/> then Dispose() will be called.  If an exception
        /// occurs in a <see cref="ReturnAction"/> the item is not returned to the pool and the item
        /// is Disposed of if it implements <see cref="IDisposable"/> before the exception is rethrown.
        /// </summary>
        /// <param name="item">The item to release back into the pool.</param>
        public void Return(T item)
        {
            // Don't allow multiple references to the same object to live on the Pool.  We don't want to hand
            // out what we think are unique object references and then find out they're being edited all over the
            // place.
            if (item == null || _items.Contains(item))
            {
                return;
            }

            // Only return the item the pool if the pool has spaces available.
            if (_items.Count < this.Max)
            {
                try
                {
                    // If this can't run, dispose of the item and then re-throw the exception
                    // so the caller can handle it as they see fit (and they know it occurred).
                    this.ReturnAction?.Invoke(item);
                }
                catch
                {
                    DisposeItem(item);
                    throw;
                }

                _items.Add(item);

                return;
            }

            // If item gets here it was not returned to the pool, as a result, if it needs
            // to be disposed of then we're going to do that before it goes into the ether.
            this.DisposeItem(item);
        }

        /// <summary>
        /// Gets an object from the pool if one is available.  If an object is not available a new object
        /// is created and returned.
        /// </summary>
        public T Get()
        {
            if (_items.TryTake(out var item))
            {
                this.CounterReusedObjects++;
                return item;
            }

            this.CounterNewObjects++;
            var newItem = new T();

            // Invoke the init action if it needs to be initialized.
            this.InitAction?.Invoke(newItem);

            return newItem;
        }

        /// <summary>
        /// Clears the <see cref="ConcurrentBag{T}" />.
        /// </summary>
        public void Clear()
        {
            if (_items.IsEmpty)
            {
                return;
            }

            // We need to dequeue the entire queue, we will do that but not do anything
            // inside of the loop.
            while (_items.TryTake(out var item))
            {
                this.DisposeItem(item);
            }
        }

        /// <summary>
        /// Invokes an action on all items in the underlying <see cref="ConcurrentBag{T}"/>.
        /// </summary>
        /// <param name="action"></param>
        public void InvokeAll(Action<T> action)
        {
            if (action == null)
            {
                return;
            }

            foreach (var item in _items)
            {
                action.Invoke(item);
            }
        }

        /// <summary>
        /// Disposes of the item <see cref="T"/> if it is <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="item"></param>
        private void DisposeItem(T item)
        {
            // If item gets here it was not returned to the pool, as a result, if it needs
            // to be disposed of then we're going to do that before it goes into the ether.
            if (item is IDisposable d)
            {
                d?.Dispose();
            }
        }

        /// <summary>
        /// The number of items currently available/idle in the <see cref="ConcurrentBag{T}" />.
        /// </summary>
        public int Count()
        {
            return _items.Count;
        }

        /// <summary>
        /// An action that can be invoked to execute code for the memory pool that should be executed
        /// when an item is returned to the pool.
        /// </summary>
        public Action<T> ReturnAction { get; set; }

        /// <summary>
        /// An action that will be invoked as the first step after a new <see cref="T"/> is initialized.
        /// </summary>
        public Action<T> InitAction { get; set; }
    }
}