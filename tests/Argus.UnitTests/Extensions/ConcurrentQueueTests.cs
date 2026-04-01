/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System.Collections.Concurrent;
using Xunit;

namespace Argus.UnitTests
{
    public class ConcurrentQueueTests
    {
        [Fact]
        public void Clear_RemovesAllItems()
        {
            var queue = new ConcurrentQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            queue.Clear();

            Assert.True(queue.IsEmpty);
            Assert.Equal(0, queue.Count);
        }

        [Fact]
        public void Clear_EmptyQueue_NoException()
        {
            var queue = new ConcurrentQueue<string>();

            queue.Clear();

            Assert.True(queue.IsEmpty);
        }
    }
}
