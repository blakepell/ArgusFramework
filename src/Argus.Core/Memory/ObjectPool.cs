/*
 * @author            : Blake Pell
 * @initial date      : 2020-02-27
 * @last updated      : 2021-06-18
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using Argus.Math;
using System.Collections.Concurrent;

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
    ///
    /// The pool by default creates objects on demand.  To initially load a set of objects for the pool the <see cref="Fill()" /> method
    /// can be called.  The Fill method will honor the <see cref="GetAction"/> when creating the objects.
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
        /// Gets an object from the pool if one is available.  If it is the initial creation of
        /// the object <see cref="GetAction"/> will indicate that it is the initialize instantiation
        /// of the object.
        /// </summary>
        public T Get()
        {
            if (_items.TryTake(out var item))
            {
                this.GetAction?.Invoke(item, false);
                this.CounterReusedObjects++;
                return item;
            }

            this.CounterNewObjects++;
            var newItem = new T();

            // Invoke the the GetAction
            this.GetAction?.Invoke(newItem, true);

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
        /// Invokes an action on all items in the underlying <see cref="ConcurrentBag{T}"/>.  This executes
        /// the action without removing the items from the underlying <see cref="ConcurrentBag{T}"/>.
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
        /// Invokes an action on all items in the underlying <see cref="ConcurrentBag{T}"/> first
        /// checking out each item so that it can't be accessed by other callers.  The items will
        /// be returned in a batch after the action has been run against all of them.
        /// </summary>
        /// <param name="action"></param>
        /// <remarks>
        /// There is a chance that a new item could be created via Get after all of the items are
        /// removed and that item wouldn't be included in this call.  This function does not block
        /// getting new items.
        /// </remarks>
        public void InvokeAllWithCheckout(Action<T> action)
        {
            int count = _items.Count;
            var items = new T[_items.Count];

            // Remove the items one by one and run the action for them.  Don't return
            // them in this loop since we don't want to return one and then get that
            // reference immediately back.
            for (int i = 0; i < count; i++)
            {
                items[i] = this.Get();
                action.Invoke(items[i]);
            }

            // Now that all of the actions have been run, return them.
            foreach (var item in items)
            {
                this.Return(item);
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
        /// Fills the <see cref="ObjectPool{T}"/> to the number of items specified in the <see cref="Max"/>
        /// property.  Fill ensures that the <see cref="GetAction"/> is called if one was set.
        /// </summary>
        public void Fill()
        {
            this.Fill(this.Max);
        }

        /// <summary>
        /// Fills the <see cref="ObjectPool{T}"/> will the specified number of items up until
        /// the value set in the <see cref="Max"/> property.  Fill ensures that the <see cref="GetAction"/>
        /// is called if one was set.
        /// </summary>
        /// <param name="count">The number of items to fill.  The fill will cease processing if it
        /// exceeds the value set in the <see cref="Max"/> property.
        /// </param>
        public void Fill(int count)
        {
            if (count <= 0)
            {
                return;
            }

            // Honor the maximum specified.  Use the Get() to have the item initialized
            // correctly by calling InitAction if one was set.
            int actualCount = MathUtilities.Clamp(1, count, this.Max);
            var list = new T[actualCount];

            // Save all of the items created or returned in an array that we will return
            // in the end.  If this is used just after the object was created they will
            // all be new items.  If for some reason it was called after the pool has been
            // in use we would effectively just request the items and return them back.
            for (int i = 0; i < actualCount; i++)
            {
                list[i] = this.Get();
            }

            // Return the items to the pool.
            foreach (T item in list)
            {
                this.Return(item);
            }
        }

        /// <summary>
        /// An action that can be invoked to execute code for the memory pool that should be executed
        /// when an item is returned to the pool.
        /// </summary>
        public Action<T> ReturnAction { get; set; }

        /// <summary>
        /// An action that can be invoked to execute code when an item is retrieved from the memory
        /// pool.  If it's a new object the bool value of (T, bool) will be true otherwise false if
        /// it was an existing item.
        /// </summary>
        public Action<T, bool> GetAction { get; set; }
    }
}