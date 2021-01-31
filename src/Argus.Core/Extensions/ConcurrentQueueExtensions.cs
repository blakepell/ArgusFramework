using System.Collections.Concurrent;

namespace Argus.Extensions
{
    /// <summary>
    /// ConcurrentQueue extension methods.
    /// </summary>
    public static class ConcurrentQueueExtensions
    {
        /// <summary>
        /// Clears the ConcurrentQueue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            if (queue.IsEmpty)
            {
                return;
            }

            // We need to dequeue the entire queue, we will do that but not do anything
            // inside of the loop.
            while (queue.TryDequeue(out T item))
            {
            }
        }
    }
}