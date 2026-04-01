/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System;
using Xunit;

namespace Argus.UnitTests
{
    public class TimeSpanTests
    {
        [Fact]
        public void ToVerticalString_ContainsAllParts()
        {
            var ts = new TimeSpan(1, 2, 3, 4, 5);

            var result = ts.ToVerticalString();

            Assert.Contains("1 Days", result);
            Assert.Contains("2 Hours", result);
            Assert.Contains("3 Minutes", result);
            Assert.Contains("4 Seconds", result);
            Assert.Contains("5 Milliseconds", result);
        }

        [Fact]
        public void Multiply_CorrectlyMultiplies()
        {
            var ts = TimeSpan.FromMinutes(5);

            var result = ts.Multiply(3);

            Assert.Equal(TimeSpan.FromMinutes(15), result);
        }

        [Fact]
        public void Multiply_ByOne_ReturnsSame()
        {
            var ts = TimeSpan.FromHours(2);

            var result = ts.Multiply(1);

            Assert.Equal(ts, result);
        }

        [Fact]
        public void Divide_CorrectlyDivides()
        {
            var ts = TimeSpan.FromMinutes(15);

            var result = ts.Divide(3);

            Assert.Equal(TimeSpan.FromMinutes(5), result);
        }

        [Fact]
        public void Divide_ByOne_ReturnsSame()
        {
            var ts = TimeSpan.FromHours(2);

            var result = ts.Divide(1);

            Assert.Equal(ts, result);
        }
    }
}
