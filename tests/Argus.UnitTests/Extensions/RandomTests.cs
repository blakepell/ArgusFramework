/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System;
using Xunit;

namespace Argus.UnitTests
{
    public class RandomTests
    {
        [Fact]
        public void NextInt32_ReturnsValue()
        {
            var rng = new Random(42);

            // Should not throw and should return a value
            int result = rng.NextInt32();

            // Just verify it ran without exception; any int is valid
            Assert.IsType<int>(result);
        }

        [Fact]
        public void NextInt32_ProducesDifferentValues()
        {
            var rng = new Random(12345);

            int val1 = rng.NextInt32();
            int val2 = rng.NextInt32();

            // With a good random generator, two consecutive calls are very unlikely to be equal
            Assert.NotEqual(val1, val2);
        }

        [Fact]
        public void NextDecimal_ReturnsValue()
        {
            var rng = new Random(42);

            decimal result = rng.NextDecimal();

            Assert.IsType<decimal>(result);
        }

        [Fact]
        public void NextDecimal_ProducesDifferentValues()
        {
            var rng = new Random(12345);

            decimal val1 = rng.NextDecimal();
            decimal val2 = rng.NextDecimal();

            Assert.NotEqual(val1, val2);
        }
    }
}
