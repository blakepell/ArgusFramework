/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-06
 * @last updated      : 2021-02-06
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Text;
using Argus.Extensions;
using Xunit;

namespace Argus.UnitTests
{
    public class DateTests
    {
        [Fact]
        public void MonthTwoCharacters()
        {
            var dt = new DateTime(2021, 2, 6);
            string value = dt.MonthTwoCharacters();
            Assert.True(value == "02");
        }

        [Fact]
        public void DayTwoCharacters()
        {
            var dt = new DateTime(2021, 2, 6);
            string value = dt.MonthTwoCharacters();
            Assert.True(value == "02");
        }

        [Fact]
        public void FirstDayOfMonth()
        {
            var dt = new DateTime(2021, 2, 6);
            Assert.True(dt.FirstDayOfMonth() == DateTime.Parse("2/1/2021"));
        }

        [Fact]
        public void LastDayOfMonth()
        {
            var dt = new DateTime(2021, 2, 6);
            Assert.True(dt.LastDayOfMonth() == DateTime.Parse("2/28/2021"));
        }

        [Fact]
        public void FirstDayOfYear()
        {
            var dt = new DateTime(2021, 2, 6);
            Assert.True(dt.FirstDayOfYear() == DateTime.Parse("1/1/2021"));
        }

        [Fact]
        public void LastDayOfYear()
        {
            var dt = new DateTime(2021, 2, 6);
            Assert.True(dt.LastDayOfYear() == DateTime.Parse("12/31/2021"));
        }

        [Fact]
        public void ToShortDatePaddedString()
        {
            var dt = new DateTime(2021, 2, 6);
            Assert.True(dt.ToShortDatePaddedString() == "02/06/2021");
        }

        [Fact]
        public void MonthToEnglish()
        {
            var dt = new DateTime(2021, 2, 6);
            Assert.True(dt.MonthToEnglish() == "February");
        }

        [Fact]
        public void IsWeekend()
        {
            // Sunday
            var dt = new DateTime(2021, 2, 7);
            Assert.True(dt.IsWeekend());

            // Monday
            dt = new DateTime(2021, 2, 8);
            Assert.False(dt.IsWeekend());
        }

        [Fact]
        public void IsWeekday()
        {
            // Sunday
            var dt = new DateTime(2021, 2, 7);
            Assert.False(dt.IsWeekday());

            // Monday
            dt = new DateTime(2021, 2, 8);
            Assert.True(dt.IsWeekday());
        }

        [Fact]
        public void UnixEpoch()
        {
            long epoch = 1612656000;
            var dt = new DateTime(2021, 2, 7, 0, 0, 0);
            Assert.Equal(epoch, dt.UnixEpoch());
        }

        [Fact]
        public void ToFileNameFriendlyFormat()
        {
            var dt = new DateTime(2021, 2, 7, 12, 30, 20, 500);
            Assert.Equal("2021-02-07-12.30.20.500", dt.ToFileNameFriendlyFormat(true));
            Assert.Equal("2021-02-07", dt.ToFileNameFriendlyFormat(false));
        }

        [Fact]
        public void ToSqlServerTimeStampBeginningOfDay()
        {
            var dt = new DateTime(2021, 2, 7, 15, 27, 15, 350);
            Assert.Equal("2021-02-07 00:00:00", dt.ToSqlServerTimeStampBeginningOfDay());
        }

        [Fact]
        public void ToSqlServerTimeStamp()
        {
            var dt = new DateTime(2021, 2, 7, 15, 27, 15, 350);
            Assert.Equal("2021-02-07 15:27:15", dt.ToSqlServerTimeStamp());
        }

        [Fact]
        public void ToSqliteTimeStamp()
        {
            var dt = new DateTime(2021, 2, 7, 15, 27, 15, 350);
            Assert.Equal("2021-02-07 15:27:15", dt.ToSqliteTimeStamp());
        }

        [Fact]
        public void ToOracleSqlDate()
        {
            var dt = new DateTime(2021, 2, 7, 15, 27, 15, 350);
            Assert.Equal("to_date('07.02.2021 15:27:15','dd.mm.yyyy hh24.mi.ss')", dt.ToOracleSqlDate());
        }

        [Fact]
        public void ToShortDateString()
        {
            var dt = new DateTime(2021, 2, 7, 15, 27, 15, 350);
            Assert.Equal("2/7/2021", dt.ToShortDateString());
        }

        [Fact]
        public void NextWeekDay()
        {
            var dt = new DateTime(2021, 2, 6);
            var dtNextWeekDay = new DateTime(2021, 2, 8);
            Assert.Equal(dtNextWeekDay, dt.NextWeekDay());
        }

        [Fact]
        public void LastWeekDay()
        {
            var dt = new DateTime(2021, 2, 7);
            var dtLastWeekDay = new DateTime(2021, 2, 5);
            Assert.Equal(dtLastWeekDay, dt.LastWeekDay());
        }

        [Fact]
        public void QuarterEnd()
        {
            var dt = new DateTime(2021, 2, 7);
            Assert.Equal(new DateTime(2021, 1, 1), dt.QuarterStart());
        }

        [Fact]
        public void QuarterStart()
        {
            var dt = new DateTime(2021, 2, 7);
            Assert.Equal(new DateTime(2021, 3, 31), dt.QuarterEnd());
        }

        [Fact]
        public void IsMonthEnd()
        {
            var dt = new DateTime(2021, 1, 31);
            Assert.True(dt.IsMonthEnd());

            dt = new DateTime(2021, 2, 28);
            Assert.True(dt.IsMonthEnd());

            dt = new DateTime(2021, 3, 31);
            Assert.True(dt.IsMonthEnd());

            dt = new DateTime(2021, 4, 30);
            Assert.True(dt.IsMonthEnd());

            dt = new DateTime(2021, 5, 31);
            Assert.True(dt.IsMonthEnd());

            dt = new DateTime(2021, 6, 30);
            Assert.True(dt.IsMonthEnd());

            dt = new DateTime(2021, 7, 31);
            Assert.True(dt.IsMonthEnd());

            dt = new DateTime(2021, 8, 31);
            Assert.True(dt.IsMonthEnd());

            dt = new DateTime(2021, 9, 30);
            Assert.True(dt.IsMonthEnd());

            dt = new DateTime(2021, 10, 31);
            Assert.True(dt.IsMonthEnd());

            dt = new DateTime(2021, 11, 30);
            Assert.True(dt.IsMonthEnd());

            dt = new DateTime(2021, 12, 31);
            Assert.True(dt.IsMonthEnd());

            dt = new DateTime(2021, 12, 15);
            Assert.False(dt.IsMonthEnd());

            dt = new DateTime(2021, 2, 27);
            Assert.False(dt.IsMonthEnd());
        }

    }
}