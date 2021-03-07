/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-11
 * @last updated      : 2021-02-11
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System;
using Xunit;

namespace Argus.UnitTests
{
    public class CharTests
    {
        [Fact]
        public void Left()
        {
            var buf = "This is a test".ToCharArray();

            Assert.Equal("This".ToCharArray(), buf.Left(4));
            Assert.Equal("This is a test".ToCharArray(), buf.Left(14));
            Assert.Equal(buf.Left(0), Array.Empty<char>());

        }

        [Fact]
        public void SafeLeft()
        {
            var buf = "This is a test".ToCharArray();

            Assert.Equal("This".ToCharArray(), buf.SafeLeft(4));
            Assert.Equal("This is a test".ToCharArray(), buf.SafeLeft(14));
            Assert.Equal(Array.Empty<char>(), buf.SafeLeft(0));
            Assert.Equal(Array.Empty<char>(), buf.SafeLeft(-1));
            Assert.Equal("This is a test".ToCharArray(), buf.SafeLeft(25));
        }

        [Fact]
        public void Right()
        {
            var buf = "This is a test".ToCharArray();

            Assert.Equal("test".ToCharArray(), buf.Right(4));
            Assert.Equal("This is a test".ToCharArray(), buf.Right(14));
            Assert.Equal(buf.Right(0), Array.Empty<char>());

        }

        [Fact]
        public void SafeRight()
        {
            var buf = "This is a test".ToCharArray();

            Assert.Equal("test".ToCharArray(), buf.SafeRight(4));
            Assert.Equal("This is a test".ToCharArray(), buf.SafeRight(14));
            Assert.Equal(Array.Empty<char>(), buf.SafeRight(0));
            Assert.Equal(Array.Empty<char>(), buf.SafeRight(-1));
            Assert.Equal("This is a test".ToCharArray(), buf.SafeRight(25));
        }

        [Fact]
        public void SubChar()
        {
            var buf = "This is a test".ToCharArray();

            Assert.Equal("is a".ToCharArray(), buf.SubChar(5, 4));
            Assert.Equal("This".ToCharArray(), buf.SubChar(0, 4));
            Assert.Equal("test".ToCharArray(), buf.SubChar(10, 4));
        }

        [Fact]
        public void SafeSubChar()
        {
            var buf = "This is a test".ToCharArray();

            Assert.Equal("is a".ToCharArray(), buf.SafeSubChar(5, 4));
            Assert.Equal("This".ToCharArray(), buf.SafeSubChar(0, 4));
            Assert.Equal("test".ToCharArray(), buf.SafeSubChar(10, 4));

            Assert.Equal(Array.Empty<char>(), buf.SafeSubChar(0,0));
            Assert.Equal(Array.Empty<char>(), buf.SafeSubChar(0, -1));

            Assert.Equal("This is a test".ToCharArray(), buf.SafeSubChar(0,14));
            Assert.Equal("This is a test".ToCharArray(), buf.SafeSubChar(0, 15));
        }

        [Fact]
        public void IndexOf()
        {
            var buf = "This is a test".ToCharArray();
            Assert.Equal(2, buf.IndexOf('i'));
            Assert.Equal(-1, buf.IndexOf('z'));
            Assert.Equal(5, buf.IndexOf('i', 4));
            Assert.Equal(-1, buf.IndexOf('i', 3, 1));
            Assert.Equal(2, buf.IndexOf("is".ToCharArray()));
            Assert.Equal(-1, buf.IndexOf("xyz".ToCharArray()));
            Assert.Equal(5, buf.IndexOf("is".ToCharArray(), 3));
            Assert.Equal(-1, buf.IndexOf("is".ToCharArray(), 7));
        }

        [Fact]
        public void LastIndexOf()
        {
            var buf = "This is a test".ToCharArray();
            Assert.Equal(5, buf.LastIndexOf('i'));
            Assert.Equal(-1, buf.LastIndexOf('z'));
            Assert.Equal(5, buf.LastIndexOf('i', 9));
            Assert.Equal(2, buf.LastIndexOf('i', 3));
        }

        [Fact]
        public void StartsWith()
        {
            var buf = "This is a test".ToCharArray();
            Assert.True(buf.StartsWith('T'));
            Assert.False(buf.StartsWith('t'));
            Assert.True(buf.StartsWith("This".ToCharArray()));
            Assert.False(buf.StartsWith("this".ToCharArray()));
            Assert.True(buf.StartsWith("This"));
            Assert.False(buf.StartsWith("this"));
        }

        [Fact]
        public void EndsWith()
        {
            var buf = "This is a test".ToCharArray();
            Assert.True(buf.EndsWith('t'));
            Assert.False(buf.EndsWith('T'));
            Assert.True(buf.EndsWith("test".ToCharArray()));
            Assert.False(buf.EndsWith("Test".ToCharArray()));
            Assert.True(buf.EndsWith("test"));
            Assert.False(buf.EndsWith("Test"));
        }

        [Fact]
        public void ToUpper()
        {
            var buf = "This is a test".ToCharArray();
            Assert.Equal(buf.ToUpper(), "THIS IS A TEST".ToCharArray());
            Assert.NotEqual(buf.ToUpper(), "this is a test".ToCharArray());
        }

        [Fact]
        public void ToLower()
        {
            var buf = "This is a test".ToCharArray();
            Assert.NotEqual(buf.ToLower(), "THIS IS A TEST".ToCharArray());
            Assert.Equal(buf.ToLower(), "this is a test".ToCharArray());
        }

        [Fact]
        public void IsWhiteSpace()
        {
            var buf = "This is a test".ToCharArray();
            Assert.False(buf.IsWhiteSpace());

            buf = "  ".ToCharArray();
            Assert.True(buf.IsWhiteSpace());

            buf = "  \t\t\r\n ".ToCharArray();
            Assert.True(buf.IsWhiteSpace());

            buf = "this is a test".ToCharArray();
            Assert.False(buf.IsWhiteSpace());
        }

        [Fact]
        public void ToAsciiChar()
        {
            Assert.Equal(32, ' '.ToAsciiCode());
            Assert.Equal(10, '\n'.ToAsciiCode());
            Assert.Equal(13, '\r'.ToAsciiCode());
            Assert.NotEqual(100, 'b'.ToAsciiCode());
        }

        [Fact]
        public void RepeatToString()
        {
            string buf = '0'.RepeatToString(5);
            Assert.Equal("00000", buf);
        }

        [Fact]
        public void RepeatToChar()
        {
            var buf = '0'.RepeatToChar(5);
            var temp = new char[] {'0', '0', '0', '0', '0'};

            Assert.Equal(temp, buf);
        }

        [Fact]
        public void IsVowel()
        {
            Assert.True('a'.IsVowel());
            Assert.True('e'.IsVowel());
            Assert.True('i'.IsVowel());
            Assert.True('o'.IsVowel());
            Assert.True('u'.IsVowel());
            Assert.False('b'.IsVowel());
            Assert.False('m'.IsVowel());
            Assert.False('q'.IsVowel());
            Assert.False('@'.IsVowel());
            Assert.False('1'.IsVowel());
        }

        [Fact]
        public void Trim()
        {
            var buf = "   this is a test to see if this works   ".ToCharArray();
            var output = buf.Trim();
            var success = "this is a test to see if this works".ToCharArray();
            var fail = "".ToCharArray();

            Assert.Equal(success, output);
            Assert.NotEqual(success, fail);
        }

    }
}
