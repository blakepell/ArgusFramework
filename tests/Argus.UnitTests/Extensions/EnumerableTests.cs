/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Argus.UnitTests
{
    public class EnumerableTests
    {
        [Fact]
        public void PickRandom_ReturnsSingleItem()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };

            var result = list.PickRandom();

            Assert.Contains(result, list);
        }

        [Fact]
        public void PickRandom_ReturnsRequestedCount()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };

            var result = list.PickRandom(3).ToList();

            Assert.Equal(3, result.Count);

            foreach (var item in result)
            {
                Assert.Contains(item, list);
            }
        }

        [Fact]
        public void Shuffle_ReturnsSameElements()
        {
            IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5 };

            var shuffled = source.Shuffle().ToList();

            Assert.Equal(5, shuffled.Count);

            foreach (var item in new[] { 1, 2, 3, 4, 5 })
            {
                Assert.Contains(item, shuffled);
            }
        }
    }
}
