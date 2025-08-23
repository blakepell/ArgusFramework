/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2025-08-23
 * @last updated      : 2025-08-23
 * @copyright         : Copyright (c) 2003-2025, All rights reserved.
 * @license           : MIT
 */

using System.Collections.Concurrent;
using System.Threading;

namespace Argus.Collections
{
    /// <summary>
    /// Represents a fixed-size buffer that operates on a first-in, first-out (FIFO) basis.
    /// When the buffer reaches its capacity, the oldest item is removed to make room for a new item.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the buffer.</typeparam>
    public class ConcurrentSlidingBuffer<T> : IEnumerable<T>
    {
        private readonly ConcurrentQueue<T> _queue;
        private readonly int _maxCount;
        private int _count;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxCount"></param>
        public ConcurrentSlidingBuffer(int maxCount)
        {
            _maxCount = maxCount;
            _queue = new ConcurrentQueue<T>();
            _count = 0;
        }

        /// <summary>
        /// Adds an item to the buffer. If adding the item exceeds the maximum capacity of the buffer,
        /// the oldest item is automatically removed.
        /// </summary>
        /// <param name="item">The item to add to the buffer.</param>
        public void Add(T item)
        {
            _queue.Enqueue(item);
            
            if (Interlocked.Increment(ref _count) > _maxCount)
            {
                if (_queue.TryDequeue(out _))
                {
                    Interlocked.Decrement(ref _count);
                }
            }
        }

        /// <summary>
        /// Removes all objects from the buffer.
        /// </summary>
        public void Clear()
        {
            while (_queue.TryDequeue(out _)) { }
            Interlocked.Exchange(ref _count, 0);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the buffer.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the buffer.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}