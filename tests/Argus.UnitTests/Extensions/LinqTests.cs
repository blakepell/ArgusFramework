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
    public class LinqTests
    {
        [Fact]
        public void Update_AppliesActionToAllItems()
        {
            var list = new List<TestItem>
            {
                new TestItem { Value = 1 },
                new TestItem { Value = 2 },
                new TestItem { Value = 3 }
            };

            list.AsQueryable().Update(x => x.Value *= 2);

            Assert.Equal(2, list[0].Value);
            Assert.Equal(4, list[1].Value);
            Assert.Equal(6, list[2].Value);
        }

        [Fact]
        public void Update_ReturnsSourceQueryable()
        {
            var list = new List<TestItem>
            {
                new TestItem { Value = 1 }
            };

            var queryable = list.AsQueryable();
            var result = queryable.Update(x => x.Value = 10);

            Assert.Equal(queryable, result);
        }

        [Fact]
        public void Update_EmptySource_NoException()
        {
            var list = new List<TestItem>();

            var result = list.AsQueryable().Update(x => x.Value = 99).ToList();

            Assert.Empty(result);
        }

        private class TestItem
        {
            public int Value { get; set; }
        }
    }
}
