/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System.Text.RegularExpressions;
using Xunit;

namespace Argus.UnitTests
{
    public class RegexTests
    {
        [Fact]
        public void TrySuccess_SuccessfulMatch_ReturnsTrue()
        {
            var match = Regex.Match("hello world", @"hello");

            Assert.True(match.TrySuccess());
        }

        [Fact]
        public void TrySuccess_FailedMatch_ReturnsFalse()
        {
            var match = Regex.Match("hello world", @"xyz");

            Assert.False(match.TrySuccess());
        }

        [Fact]
        public void TrySuccess_NullMatch_ReturnsFalse()
        {
            Match match = null;

            Assert.False(match.TrySuccess());
        }
    }
}
