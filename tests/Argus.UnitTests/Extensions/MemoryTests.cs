/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-07
 * @last updated      : 2022-08-26
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Argus.UnitTests
{
    public class MemoryTests
    {
        private readonly ITestOutputHelper output;

        public MemoryTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Left()
        {
            var span = "Blake Pell".AsSpan();
            Assert.True("Blake".AsSpan().Equals(span.Left(5), StringComparison.Ordinal));
        }

        [Fact]
        public void SafeLeft()
        {
            var span = "Blake Pell".AsSpan();
            Assert.True("Blake".AsSpan().Equals(span.SafeLeft(5), StringComparison.Ordinal));
            Assert.True("Blake Pell".AsSpan().Equals(span.SafeLeft(25), StringComparison.Ordinal));
        }

        [Fact]
        public void Right()
        {
            var span = "Blake Pell".AsSpan();
            Assert.True("Pell".AsSpan().Equals(span.Right(4), StringComparison.Ordinal));
        }

        [Fact]
        public void SafeRight()
        {
            var span = "Blake Pell".AsSpan();
            Assert.True("Pell".AsSpan().Equals(span.SafeRight(4), StringComparison.Ordinal));
            Assert.True("Blake Pell".AsSpan().Equals(span.SafeRight(25), StringComparison.Ordinal));
        }

        [Fact]
        public void SafeIndexOf()
        {
            Assert.Equal(6, "Blake Pell".AsSpan().SafeIndexOf("Pell".AsSpan(), 0));
            Assert.Equal(-1, "Blake Pell".AsSpan().SafeIndexOf("Brandon".AsSpan(), 0));
            Assert.NotEqual(5, "Blake Pell".AsSpan().SafeIndexOf("Pell".AsSpan(), 0));
            Assert.Equal(6, "Blake Pell".AsSpan().SafeIndexOf("Pell".AsSpan(), 6));
            Assert.Equal(-1, "Blake Pell".AsSpan().SafeIndexOf("Pell".AsSpan(), 7));
            Assert.Equal(-1, "Blake Pell".AsSpan().SafeIndexOf("Pell".AsSpan(), 25));
        }

        [Fact]
        public void IndexOf()
        {
            Assert.Equal(6, "Blake Pell".AsSpan().IndexOf("Pell".AsSpan(), 0));
            Assert.Equal(-1, "Blake Pell".AsSpan().IndexOf("Brandon".AsSpan(), 0));
            Assert.NotEqual(5, "Blake Pell".AsSpan().IndexOf("Pell".AsSpan(), 0));
            Assert.Equal(6, "Blake Pell".AsSpan().IndexOf("Pell".AsSpan(), 6));
            Assert.Equal(-1, "Blake Pell".AsSpan().IndexOf("Pell".AsSpan(), 7));
            Assert.Throws<ArgumentOutOfRangeException>(() => "Blake Pell".AsSpan().IndexOf("Pell".AsSpan(), 25));
        }

        /// <summary>
        /// StartsWith that supports <see cref="Char"/>.
        /// </summary>
        [Fact]
        public void StartsWith()
        {
            Assert.True("Blake Pell".AsSpan().StartsWith('B'));
            Assert.False("Blake Pell".AsSpan().StartsWith('C'));
            Assert.False("".AsSpan().StartsWith('B'));
        }

        /// <summary>
        /// EndsWith that supports <see cref="Char"/>.
        /// </summary>
        [Fact]
        public void EndsWith()
        {
            Assert.True("Blake Pell".AsSpan().EndsWith("l"));
            Assert.False("Blake Pell".AsSpan().EndsWith("q"));
            Assert.False("".AsSpan().EndsWith('B'));
        }

        [Fact]
        public void SplitNext()
        {
            var span = "One Two Three Four Five".AsSpan();
            Assert.True(span.SplitNext(' ').Equals("One".AsSpan(), StringComparison.Ordinal));
            Assert.Equal(19, span.Length);
            Assert.True(span.SplitNext(' ').Equals("Two".AsSpan(), StringComparison.Ordinal));
            Assert.True(span.SplitNext(' ').Equals("Three".AsSpan(), StringComparison.Ordinal));
            Assert.True(span.SplitNext(' ').Equals("Four".AsSpan(), StringComparison.Ordinal));
            Assert.True(span.SplitNext(' ').Equals("Five".AsSpan(), StringComparison.Ordinal));
            Assert.False(span.SplitNext(' ').Equals("Six".AsSpan(), StringComparison.Ordinal));
            Assert.Equal(0, span.Length);
        }

        [Fact]
        public void IsNullOrEmpty()
        {
            ReadOnlySpan<char> span = null;
            Assert.True(span.IsNullOrEmpty());
            span = "".AsSpan();
            Assert.True(span.IsNullOrEmpty());
            span = "Blake Pell".AsSpan();
            Assert.False(span.IsNullOrEmpty());
        }

        [Fact]
        public void IsNullOrEmptyOrWhiteSpace()
        {
            ReadOnlySpan<char> span = null;
            Assert.True(span.IsNullEmptyOrWhiteSpace());
            span = "".AsSpan();
            Assert.True(span.IsNullEmptyOrWhiteSpace());
            span = "    \r\n".AsSpan();
            Assert.True(span.IsNullEmptyOrWhiteSpace());
            span = "Blake Pell".AsSpan();
            Assert.False(span.IsNullEmptyOrWhiteSpace());
        }

        [Fact]
        public void TrimWhitespace()
        {
            var span = "    Blake Pell    ".AsSpan();
            span.TrimWhitespace();
            Assert.True(span.Equals("Blake Pell", StringComparison.Ordinal));

            span = "Blake Pell".AsSpan();
            span.TrimWhitespace();
            Assert.True(span.Equals("Blake Pell", StringComparison.Ordinal));

            span = "Blake Pell\r\n\t".AsSpan();
            span.TrimWhitespace();
            Assert.True(span.Equals("Blake Pell", StringComparison.Ordinal));
        }

        [Fact]
        public void IsAlphaNumeric()
        {
            Assert.True("lucypell".AsSpan().IsAlphaNumeric());
            Assert.True("lucypell12".AsSpan().IsAlphaNumeric());
            Assert.True("LucyPell".AsSpan().IsAlphaNumeric());
            Assert.True("LucyPell19".AsSpan().IsAlphaNumeric());
            Assert.False("lucy pell".AsSpan().IsAlphaNumeric());
            Assert.False("lucy-pell".AsSpan().IsAlphaNumeric());
            Assert.False("@LucyPell19".AsSpan().IsAlphaNumeric());
        }

        [Fact]
        public void FirstArgument()
        {
            // Test 1
            var span = "load obj 1000".AsSpan();
            var firstArg = span.FirstArgument();
            output.WriteLine(firstArg.ToString());

            Assert.True(firstArg.Equals("load", StringComparison.Ordinal));
            Assert.True(span.Equals("obj 1000", StringComparison.Ordinal));

            var secondArg = span.FirstArgument();

            Assert.True(secondArg.Equals("obj", StringComparison.Ordinal));
            Assert.True(span.Equals("1000", StringComparison.Ordinal));

            // Test 2
            span = "\"Blake Pell\" \"is cool\"".AsSpan();
            firstArg = span.FirstArgument();

            Assert.True(firstArg.Equals("Blake Pell", StringComparison.Ordinal));
            Assert.True(span.Equals("\"is cool\"", StringComparison.Ordinal));

            secondArg = span.FirstArgument();

            Assert.True(secondArg.Equals("is cool", StringComparison.Ordinal));

            // Test 3
            span = "string obj sword short \"a mithril broadsword\"".AsSpan();

            var arg1 = span.FirstArgument();
            var arg2 = span.FirstArgument();
            var arg3 = span.FirstArgument();
            var arg4 = span.FirstArgument();
            var arg5 = span.FirstArgument();
            var arg6 = span.FirstArgument();

            Assert.True(arg1.Equals("string", StringComparison.Ordinal));
            Assert.True(arg2.Equals("obj", StringComparison.Ordinal));
            Assert.True(arg3.Equals("sword", StringComparison.Ordinal));
            Assert.True(arg4.Equals("short", StringComparison.Ordinal));
            Assert.True(arg5.Equals("a mithril broadsword", StringComparison.Ordinal));
            Assert.True(arg6.IsEmpty);
        }
    }
}
