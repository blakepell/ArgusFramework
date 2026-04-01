/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Argus.UnitTests
{
    public class IntPtrTests
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct TestStruct
        {
            public int X;
            public int Y;
        }

        [Fact]
        public void MarshalAs_RoundTrip()
        {
            var original = new TestStruct { X = 10, Y = 20 };

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<TestStruct>());

            try
            {
                ptr.MarshalAs(original);
                var result = ptr.MarshalAs<TestStruct>();

                Assert.NotNull(result);
                Assert.Equal(10, result.X);
                Assert.Equal(20, result.Y);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
