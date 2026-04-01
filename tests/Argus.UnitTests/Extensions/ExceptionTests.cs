/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System;
using Xunit;

namespace Argus.UnitTests
{
    public class ExceptionTests
    {
        [Fact]
        public void ToFormattedString_ContainsMessage()
        {
            var ex = new InvalidOperationException("Test error message");

            var result = ex.ToFormattedString();

            Assert.Contains("Test error message", result);
        }

        [Fact]
        public void ToFormattedString_ContainsInnerException()
        {
            var inner = new ArgumentException("Inner error");
            var ex = new InvalidOperationException("Outer error", inner);

            var result = ex.ToFormattedString();

            Assert.Contains("Outer error", result);
            Assert.Contains("Inner error", result);
        }

        [Fact]
        public void Md5Hash_ReturnsSameHashForSameMessage()
        {
            var ex1 = new Exception("Same message");
            var ex2 = new Exception("Same message");

            Assert.Equal(ex1.Md5Hash(), ex2.Md5Hash());
        }

        [Fact]
        public void Md5Hash_ReturnsDifferentHashForDifferentMessage()
        {
            var ex1 = new Exception("Message one");
            var ex2 = new Exception("Message two");

            Assert.NotEqual(ex1.Md5Hash(), ex2.Md5Hash());
        }

        [Fact]
        public void Md5Hash_IncludesInnerExceptionByDefault()
        {
            var ex1 = new Exception("Same", new Exception("Inner1"));
            var ex2 = new Exception("Same", new Exception("Inner2"));

            Assert.NotEqual(ex1.Md5Hash(), ex2.Md5Hash());
        }

        [Fact]
        public void Md5Hash_ExcludesInnerExceptionWhenSpecified()
        {
            var ex1 = new Exception("Same", new Exception("Inner1"));
            var ex2 = new Exception("Same", new Exception("Inner2"));

            Assert.Equal(ex1.Md5Hash(includeInnerException: false), ex2.Md5Hash(includeInnerException: false));
        }
    }
}
