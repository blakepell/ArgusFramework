namespace Argus.Collections
{
    /// <summary>
    /// Represents a fixed-size buffer that operates on a first-in, first-out (FIFO) basis.
    /// When the buffer reaches its capacity, the oldest item is removed to make room for a new item.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the buffer.</typeparam>
    public class SlidingBuffer<T> : IEnumerable<T>
    {
        private readonly Queue<T> _queue;
        private readonly int _maxCount;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxCount"></param>
        public SlidingBuffer(int maxCount)
        {
            _maxCount = maxCount;
            _queue = new(maxCount);
        }

        /// <summary>
        /// Adds an item to the buffer. If adding the item exceeds the maximum capacity of the buffer,
        /// the oldest item is automatically removed.
        /// </summary>
        /// <param name="item">The item to add to the buffer.</param>
        public void Add(T item)
        {
            if (_queue.Count == _maxCount)
            {
                _queue.Dequeue();
            }

            _queue.Enqueue(item);
        }

        /// <summary>
        /// Removes all objects from the buffer.
        /// </summary>
        public void Clear()
        {
            _queue.Clear();
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