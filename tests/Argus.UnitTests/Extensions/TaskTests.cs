/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Argus.UnitTests
{
    public class TaskTests
    {
        [Fact]
        public async Task TimeoutAfter_CompletesBeforeTimeout_ReturnsResult()
        {
            var task = Task.FromResult(42);

            var result = await task.TimeoutAfter(TimeSpan.FromSeconds(5));

            Assert.Equal(42, result);
        }

        [Fact]
        public async Task TimeoutAfter_TaskTimesOut_ThrowsTimeoutException()
        {
            var task = Task.Delay(TimeSpan.FromSeconds(30)).ContinueWith(_ => 42);

            await Assert.ThrowsAsync<TimeoutException>(
                () => task.TimeoutAfter(TimeSpan.FromMilliseconds(50)));
        }

        [Fact]
        public async Task TimeoutAfter_NonGeneric_CompletesBeforeTimeout()
        {
            var task = Task.CompletedTask;

            await task.TimeoutAfter(TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task TimeoutAfter_NonGeneric_ThrowsTimeoutException()
        {
            var task = Task.Delay(TimeSpan.FromSeconds(30));

            await Assert.ThrowsAsync<TimeoutException>(
                () => task.TimeoutAfter(TimeSpan.FromMilliseconds(50)));
        }

        [Fact]
        public void FireAndForget_DoesNotThrow()
        {
            var task = Task.CompletedTask;

            // Should not throw
            task.FireAndForget();
        }
    }
}
