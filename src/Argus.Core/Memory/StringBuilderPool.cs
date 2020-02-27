using System.Collections.Concurrent;
using System.Text;

namespace Argus.Memory
{
    /// <summary>
    /// A thread safe static pool of StringBuilders.
    /// </summary>
    public static class StringBuilderPool
    {
        private const int MaxPooledStringBuilders = 64;
        
        private static readonly ConcurrentQueue<StringBuilder> Pool = new ConcurrentQueue<StringBuilder>();

        /// <summary>
        /// Returns a StringBuilder from the pool.  If no StringBuilder is available a new one will
        /// be allocated and returned.
        /// </summary>
        /// <returns></returns>
        public static StringBuilder Take()
        {
            if (Pool.TryDequeue(out var sb))
            {
                return sb;
            }

            return new StringBuilder();
        }

        /// <summary>
        /// Returns a StringBuilder to the pool.
        /// </summary>
        /// <param name="sb"></param>
        public static void Return(StringBuilder sb)
        {
            if (Pool.Count <= MaxPooledStringBuilders)
            {
                // There is a race condition here so the count could be off a little bit (but insignificantly)
                sb.Clear();
                Pool.Enqueue(sb);
            }
        }
    }
}