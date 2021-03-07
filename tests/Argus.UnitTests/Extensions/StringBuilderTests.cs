/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-12
 * @last updated      : 2021-02-12
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Text;
using Argus.Extensions;
using Xunit;

namespace Argus.UnitTests
{
    public class StringBuilderTests
    {
        [Fact]
        public void AppendLineFormat()
        {
            var sb = new StringBuilder();
            sb.AppendLineFormat("{0}", "Line1");
            Assert.Equal($"Line1{Environment.NewLine}", sb.ToString());
        }

        [Fact]
        public void AppendIf()
        {
            var sb = new StringBuilder();
            sb.AppendIf(true, "This is a test");
            Assert.Equal("This is a test", sb.ToString());

            sb.Clear();
            sb.AppendIf(false, "This is a test");
            Assert.Equal(string.Empty, sb.ToString());
        }

        [Fact]
        public void AppendFormatIf()
        {
            var sb = new StringBuilder();
            sb.AppendFormatIf(true, "This is a test{0}", ".");
            Assert.Equal("This is a test.", sb.ToString());

            sb.Clear();
            sb.AppendFormatIf(false, "This is a test{0}", ".");
            Assert.Equal(string.Empty, sb.ToString());
        }

        [Fact]
        public void AppendLineIf()
        {
            var sb = new StringBuilder();
            sb.AppendLineIf(true);
            Assert.Equal(Environment.NewLine, sb.ToString());

            sb.Clear();
            sb.AppendLineIf(false);
            Assert.Equal(string.Empty, sb.ToString());
        }

        [Fact]
        public void ToUpper()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("This is a test");
            sb.ToUpper();
            Assert.Equal("THIS IS A TEST", sb.ToString());

            sb.ToLower();
            Assert.Equal("this is a test", sb.ToString());
        }

        [Fact]
        public void EndsWith()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("This is a test");
            Assert.True(sb.EndsWith("test"));
            Assert.True(sb.EndsWith("Test", true));
            Assert.True(sb.EndsWith('t'));
            Assert.True(sb.EndsWith('T', true));
            Assert.False(sb.EndsWith("Test"));
            Assert.False(sb.EndsWith("hello"));
            Assert.False(sb.EndsWith('T'));
        }

        [Fact]
        public void StartsWith()
        {
            var sb = new StringBuilder("This is a test");

            Assert.True(sb.StartsWith("This"));
            Assert.True(sb.StartsWith("this", true));
            Assert.True(sb.StartsWith('T'));
            Assert.True(sb.StartsWith('t', true));
            Assert.False(sb.StartsWith("this"));
            Assert.False(sb.StartsWith("hello", true));
            Assert.False(sb.StartsWith('t'));
        }

        [Fact]
        public void Remove()
        {
            var sb = new StringBuilder("This is a test");
            sb.Remove("abc".ToCharArray());
            Assert.Equal("This is  test", sb.ToString());

            sb.Clear();
            sb.Append("This is a test");
            sb.Remove(4);
            Assert.Equal("This", sb.ToString());
        }

        [Fact]
        public void Trim()
        {
            var sb = new StringBuilder("   This is a test.   ");
            sb.Trim();
            Assert.Equal("This is a test.", sb.ToString());
        }

        [Fact]
        public void TrimEnd()
        {
            var sb = new StringBuilder("   This is a test.   ");
            sb.TrimEnd();
            Assert.Equal("   This is a test.", sb.ToString());

            sb.Clear();
            sb.Append("   This is a test.   ");
            sb.TrimEnd(' ', '.');
            Assert.Equal("   This is a test", sb.ToString());
        }

        [Fact]
        public void TrimStart()
        {
            var sb = new StringBuilder("   This is a test.   ");
            sb.TrimStart();
            Assert.Equal("This is a test.   ", sb.ToString());

            sb.Clear();
            sb.Append("   This is a test.   ");
            sb.TrimStart(' ', 'T');
            Assert.Equal("his is a test.   ", sb.ToString());
        }

        [Fact]
        public void ContainsNumber()
        {
            var sb = new StringBuilder("123: Test");
            Assert.True(sb.ContainsNumber());

            sb.Clear();
            sb.Append("Test.");
            Assert.False(sb.ContainsNumber());
        }

    }
}
