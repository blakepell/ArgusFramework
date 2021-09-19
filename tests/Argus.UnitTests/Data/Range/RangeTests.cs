/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-06
 * @last updated      : 2021-02-06
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using Argus.Data.Range;
using System;
using Xunit;

namespace Argus.UnitTests
{
    public class RangeTests
    {
        [Fact]
        public void DateRange()
        {
            var range = new DateRange(DateTime.Parse("1/1/2021"), DateTime.Parse("1/31/2021"));
            var list = range.ToList();
            int count = list.Count;

            Assert.Equal(31, list.Count);
            Assert.Equal(DateTime.Parse("1/1/2021"), list[0]);
            Assert.Equal(DateTime.Parse("1/31/2021"), list[30]);
        }

        [Fact]
        public void DateRangeIntervalMonth()
        {
            var range = new DateRange(DateTime.Parse("1/1/2021"), DateTime.Parse("12/31/2021"));
            range.Interval = Argus.Data.Range.DateRange.DateInterval.Month;
            var list = range.ToList();

            Assert.Equal(12, list.Count);
            Assert.Equal(DateTime.Parse("1/1/2021"), list[0]);
            Assert.Equal(DateTime.Parse("12/1/2021"), list[11]);
        }

        [Fact]
        public void DateRangeIntervalWeek()
        {
            var range = new DateRange(DateTime.Parse("1/1/2021"), DateTime.Parse("12/31/2021"));
            range.Interval = Argus.Data.Range.DateRange.DateInterval.Week;
            var list = range.ToList();

            Assert.Equal(53, list.Count);
            Assert.Equal(DateTime.Parse("1/1/2021"), list[0]);
            Assert.Equal(DateTime.Parse("12/31/2021"), list[52]);
        }

        [Fact]
        public void DateRangeIntervalYear()
        {
            var range = new DateRange(DateTime.Parse("7/1/2021"), DateTime.Parse("7/1/2025"));
            range.Interval = Argus.Data.Range.DateRange.DateInterval.Year;
            var list = range.ToList();

            Assert.Equal(5, list.Count);
            Assert.Equal(DateTime.Parse("7/1/2021"), list[0]);
            Assert.Equal(DateTime.Parse("7/1/2025"), list[4]);
        }

        [Fact]
        public void IntRange()
        {
            var range = new IntRange(100, 200);
            var list = range.ToList();

            Assert.Equal(101, list.Count);
            Assert.Equal(100, list[0]);
            Assert.Equal(150, list[50]);
            Assert.Equal(200, list[100]);

        }

    }
}
