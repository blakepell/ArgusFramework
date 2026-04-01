/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System;
using System.ComponentModel;
using Xunit;

namespace Argus.UnitTests
{
    public class TypeTests
    {
        private enum TestEnum
        {
            Value1,
            Value2
        }

        [Browsable(true)]
        private class DecoratedClass { }

        private class PlainClass { }

        [Fact]
        public void IsNullableEnum_NullableEnum_ReturnsTrue()
        {
            Assert.True(typeof(TestEnum?).IsNullableEnum());
        }

        [Fact]
        public void IsNullableEnum_NonNullableEnum_ReturnsFalse()
        {
            Assert.False(typeof(TestEnum).IsNullableEnum());
        }

        [Fact]
        public void IsNullableEnum_NullableInt_ReturnsFalse()
        {
            Assert.False(typeof(int?).IsNullableEnum());
        }

        [Fact]
        public void IsNullable_NullableInt_ReturnsTrue()
        {
            Assert.True(typeof(int?).IsNullable());
        }

        [Fact]
        public void IsNullable_Int_ReturnsFalse()
        {
            Assert.False(typeof(int).IsNullable());
        }

        [Fact]
        public void IsNullable_ReferenceType_ReturnsTrue()
        {
            Assert.True(typeof(string).IsNullable());
        }

        [Fact]
        public void HasAttribute_WithAttribute_ReturnsTrue()
        {
            Assert.True(typeof(DecoratedClass).HasAttribute<BrowsableAttribute>());
        }

        [Fact]
        public void HasAttribute_WithoutAttribute_ReturnsFalse()
        {
            Assert.False(typeof(PlainClass).HasAttribute<BrowsableAttribute>());
        }
    }
}
