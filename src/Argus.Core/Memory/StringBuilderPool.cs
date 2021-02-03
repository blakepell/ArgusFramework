/*
 * @author            : Blake Pell
 * @initial date      : 2020-02-27
 * @last updated      : 2021-01-31
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using Argus.Extensions;
using System;
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
        public static StringBuilder Take()
        {
            if (Pool.TryDequeue(out var sb))
            {
                return sb;
            }

            return new StringBuilder();
        }

        /// <summary>
        /// Returns a StringBuilder from the pool and populates it with the specified value.  If no
        /// StringBuilder is available a new one will be allocated and returned.
        /// </summary>
        /// <param name="buf"></param>
        public static StringBuilder Take(string buf)
        {
            if (Pool.TryDequeue(out var sb))
            {
                sb.Append(buf);
                return sb;
            }

            return new StringBuilder(buf);
        }

        /// <summary>
        /// Returns a StringBuilder from the pool and populates it with the specified value.  If no
        /// StringBuilder is available a new one will be allocated and returned.
        /// </summary>
        /// <param name="sbCopy"></param>
        public static StringBuilder Take(StringBuilder sbCopy)
        {
            if (Pool.TryDequeue(out var sb))
            {
                sb.Append(sbCopy);
                return sb;
            }

            var sbNew = new StringBuilder();
            sbNew.Append(sbCopy);

            return sbNew;
        }

        #if NETSTANDARD2_1 || NET5_0
        /// <summary>
        /// Returns a StringBuilder from the pool and populates it with the specified ReadOnlySpan.  If no
        /// StringBuilder is available a new one will be allocated and returned.        
        /// </summary>
        /// <param name="span"></param>
        public static StringBuilder Take(ReadOnlySpan<char> span)
        {
            if (Pool.TryDequeue(out var sb))
            {
                sb.Append(span);
                return sb;
            }

            var sbSpan = new StringBuilder();
            sbSpan.Append(span);
            return sbSpan;
        }
        #endif

        /// <summary>
        /// Returns a StringBuilder to the pool and clears its contents.
        /// </summary>
        /// <param name="sb"></param>
        public static void Return(StringBuilder sb)
        {
            if (Pool.Count > MaxPooledStringBuilders)
            {
                return;
            }

            // There is a race condition here so the count could be off a little bit (but insignificantly)
            sb.Clear();
            Pool.Enqueue(sb);
        }

        /// <summary>
        /// Clears the <see cref="StringBuilder" /> <see cref="ConcurrentQueue{T}" /> pool.
        /// </summary>
        public static void Clear()
        {
            Pool.Clear();
        }

        /// <summary>
        /// The count of items currently the StringBuilder pool.
        /// </summary>
        public static int Count()
        {
            return Pool.Count;
        }
    }
}