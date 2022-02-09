/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-07
 * @last updated      : 2021-02-07
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System.Collections.Generic;
using Xunit;

namespace Argus.UnitTests
{
    public class NumericTests
    {
        [Fact]
        public void IsEven()
        {
            Assert.True(4.IsEven());
            Assert.False(5.IsEven());
        }

        [Fact]
        public void IsOdd()
        {
            Assert.True(3.IsOdd());
            Assert.False(4.IsOdd());
        }

        [Fact]
        public void IsInterval()
        {
            Assert.True(100.IsInterval(50));
            Assert.True(8.IsInterval(2));
            Assert.True(10000.IsInterval(1000));
            Assert.False(967.IsInterval(1000));
        }

        [Fact]
        public void Sort()
        {
            var list = new List<int>
            {
                5,
                15,
                26,
                34,
                2,
                1
            };

            list.Sort(NumericExtensions.SortOrder.Descending);

            Assert.Equal(34, list[0]);
            Assert.Equal(1, list[^1]);
            Assert.NotEqual(343, list[^1]);
        }

        [Fact]
        public void IsPrime()
        {
            Assert.True(2.IsPrime());
            Assert.True(3.IsPrime());
            Assert.True(5.IsPrime());
            Assert.True(7.IsPrime());
            Assert.True(11.IsPrime());
            Assert.True(13.IsPrime());
            Assert.True(17.IsPrime());
            Assert.True(19.IsPrime());
            Assert.True(23.IsPrime());
            Assert.True(29.IsPrime());
            Assert.True(31.IsPrime());
            Assert.False(14.IsPrime());
            Assert.False(24.IsPrime());
        }

        [Fact]
        public void ToEnglish()
        {
            int negative = -1;
            Assert.Equal("negative one", negative.ToEnglish().Trim());
            Assert.Equal("one", 1.ToEnglish().Trim());
            Assert.Equal("one hundred", 100.ToEnglish().Trim());
            Assert.Equal("one thousand", 1000.ToEnglish().Trim());
            Assert.Equal("one thousand two hundred and sixty-two", 1262.ToEnglish().Trim());
            Assert.Equal("eleven thousand four hundred and ninety-two", 11492.ToEnglish().Trim());
        }

        [Fact]
        public void Clamp()
        {
            Assert.Equal(5, 5.Clamp(1, 10));
            Assert.Equal(10, 64.Clamp(1, 10));
            Assert.Equal(1, 0.Clamp(1, 10));
        }

        [Fact]
        public void IsOrAre()
        {
            Assert.Equal("is", 1.IsOrAre());
            Assert.Equal("are", 15.IsOrAre());
        }
    }
}
