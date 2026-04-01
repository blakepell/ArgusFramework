/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System;
using Argus.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Argus.UnitTests
{
    public class ActionTests
    {
        [Fact]
        public async Task GetAwaiter_ActionExecutes()
        {
            bool executed = false;
            Action action = () => executed = true;

            await action;

            Assert.True(executed);
        }

        [Fact]
        public async Task GetAwaiter_ActionSetsValue()
        {
            int result = 0;
            Action action = () => result = 42;

            await action;

            Assert.Equal(42, result);
        }
    }
}
