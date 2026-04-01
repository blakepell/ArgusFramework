/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System;
using Xunit;

namespace Argus.UnitTests
{
    public class EnumTests
    {
        [Flags]
        private enum TestFlags
        {
            None = 0,
            Read = 1,
            Write = 2,
            Execute = 4,
            All = Read | Write | Execute
        }

        private enum TestIntEnum
        {
            Zero = 0,
            One = 1,
            Two = 2,
            FortyTwo = 42
        }

        [Fact]
        public void IsBitSet_ReturnsTrueWhenBitIsSet()
        {
            var value = TestFlags.Read | TestFlags.Write;

            Assert.True(value.IsBitSet(TestFlags.Read));
            Assert.True(value.IsBitSet(TestFlags.Write));
        }

        [Fact]
        public void IsBitSet_ReturnsFalseWhenBitNotSet()
        {
            var value = TestFlags.Read;

            Assert.False(value.IsBitSet(TestFlags.Write));
            Assert.False(value.IsBitSet(TestFlags.Execute));
        }

        [Fact]
        public void SetBit_AddsBitToValue()
        {
            var value = TestFlags.Read;

            var result = value.SetBit(TestFlags.Write);

            Assert.True(result.IsBitSet(TestFlags.Read));
            Assert.True(result.IsBitSet(TestFlags.Write));
        }

        [Fact]
        public void RemoveBit_RemovesBitFromValue()
        {
            var value = TestFlags.Read | TestFlags.Write;

            var result = value.RemoveBit(TestFlags.Write);

            Assert.True(result.IsBitSet(TestFlags.Read));
            Assert.False(result.IsBitSet(TestFlags.Write));
        }

        [Fact]
        public void ToggleBit_TogglesOn()
        {
            var value = TestFlags.Read;

            var result = value.ToggleBit(TestFlags.Write);

            Assert.True(result.IsBitSet(TestFlags.Read));
            Assert.True(result.IsBitSet(TestFlags.Write));
        }

        [Fact]
        public void ToggleBit_TogglesOff()
        {
            var value = TestFlags.Read | TestFlags.Write;

            var result = value.ToggleBit(TestFlags.Write);

            Assert.True(result.IsBitSet(TestFlags.Read));
            Assert.False(result.IsBitSet(TestFlags.Write));
        }

        [Fact]
        public void AsInt_ReturnsCorrectValue()
        {
            Assert.Equal(0, TestIntEnum.Zero.AsInt());
            Assert.Equal(1, TestIntEnum.One.AsInt());
            Assert.Equal(42, TestIntEnum.FortyTwo.AsInt());
        }

        [Fact]
        public void AsUInt_ReturnsCorrectValue()
        {
            Assert.Equal(2u, TestIntEnum.Two.AsUInt());
            Assert.Equal(42u, TestIntEnum.FortyTwo.AsUInt());
        }
    }
}
