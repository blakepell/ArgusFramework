/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System.Diagnostics;
using Xunit;

namespace Argus.UnitTests
{
    public class StopwatchTests
    {
        [Fact]
        public void ToMinutesSecondsFormat_NullStopwatch_ReturnsDefault()
        {
            Stopwatch sw = null;

            Assert.Equal("0m 0s", sw.ToMinutesSecondsFormat());
        }

        [Fact]
        public void ToMinutesSecondsFormat_ReturnsFormattedString()
        {
            var sw = new Stopwatch();
            sw.Start();
            sw.Stop();

            var result = sw.ToMinutesSecondsFormat();

            Assert.Contains("m", result);
            Assert.Contains("s", result);
        }

        [Fact]
        public void ToMinutesSecondsMillisecondsFormat_NullStopwatch_ReturnsDefault()
        {
            Stopwatch sw = null;

            Assert.Equal("0m 0s 0ms", sw.ToMinutesSecondsMillisecondsFormat());
        }

        [Fact]
        public void ToHoursMinutesSecondsFormat_NullStopwatch_ReturnsDefault()
        {
            Stopwatch sw = null;

            Assert.Equal("0h 0m 0s", sw.ToHoursMinutesSecondsFormat());
        }

        [Fact]
        public void ToHoursMinutesSecondsFormat_ReturnsFormattedString()
        {
            var sw = new Stopwatch();
            sw.Start();
            sw.Stop();

            var result = sw.ToHoursMinutesSecondsFormat();

            Assert.Contains("h", result);
            Assert.Contains("m", result);
            Assert.Contains("s", result);
        }

        [Fact]
        public void ToHoursMinutesSecondsMillisecondsFormat_NullStopwatch_ReturnsDefault()
        {
            Stopwatch sw = null;

            Assert.Equal("0h 0m 0s 0ms", sw.ToHoursMinutesSecondsMillisecondsFormat());
        }

        [Fact]
        public void ToDaysHoursMinutesSecondsMillisecondsFormat_NullStopwatch_ReturnsDefault()
        {
            Stopwatch sw = null;

            Assert.Equal("0h 0m 0s 0ms", sw.ToDaysHoursMinutesSecondsMillisecondsFormat());
        }

        [Fact]
        public void ToDaysHoursMinutesSecondsMillisecondsFormat_ReturnsFormattedString()
        {
            var sw = new Stopwatch();
            sw.Start();
            sw.Stop();

            var result = sw.ToDaysHoursMinutesSecondsMillisecondsFormat();

            Assert.Contains("d", result);
            Assert.Contains("h", result);
            Assert.Contains("m", result);
            Assert.Contains("s", result);
            Assert.Contains("ms", result);
        }
    }
}
