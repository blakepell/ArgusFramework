/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-14
 * @last updated      : 2021-03-07
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using Xunit;

namespace Argus.UnitTests
{
    public class StringTests
    {
        [Fact]
        public void Left()
        {
            string buf = "This is a test".Left(4);
            Assert.Equal("This", buf);
        }

        [Fact]
        public void Right()
        {
            string buf = "This is a test".Right(4);
            Assert.Equal("test", buf);
        }

        [Fact]
        public void SafeLeft()
        {
            string nullBuf = null;
            string buf = "This is a test".SafeLeft(4);
            Assert.Equal("This", buf);
            Assert.Equal("", nullBuf.SafeLeft(4));
            Assert.Equal("", nullBuf.SafeLeft(-1));
        }

        [Fact]
        public void SafeRight()
        {
            string nullBuf = null;
            string buf = "This is a test".SafeRight(4);
            Assert.Equal("test", buf);
            Assert.Equal("", nullBuf.SafeRight(4));
            Assert.Equal("", nullBuf.SafeRight(-1));
        }

        [Fact]
        public void SafeSubstring()
        {
            string nullBuf = null;
            string buf = "This is a test";
            Assert.Equal("is", buf.SafeSubstring(5, 2));
            Assert.Equal("", nullBuf.SafeSubstring(20, 5));
            Assert.Equal("", nullBuf.SafeSubstring(-1));
            Assert.Equal("is a test", buf.SafeSubstring(5, 30));
        }

        [Fact]
        public void SafeIndexOf()
        {
            string nullBuf = null;
            string buf = "This is a test";
            Assert.Equal(2, buf.SafeIndexOf("is", 0));
            Assert.Equal(5, buf.SafeIndexOf("is", 4));
            Assert.Equal(-1, buf.SafeIndexOf("is", 10));
            Assert.Equal(5, buf.SafeIndexOf("is", 5, 2));
            Assert.Equal(2, buf.SafeIndexOf("is", 0, 5));
            Assert.Equal(-1, nullBuf.SafeIndexOf("is", 4));
            Assert.Equal(-1, nullBuf.SafeIndexOf("is", 4, 5));
            Assert.Equal(2, buf.SafeIndexOf('i', 0));
            Assert.Equal(5, buf.SafeIndexOf('i', 5));
            Assert.Equal(-1, buf.SafeIndexOf('i', 7));
            Assert.Equal(-1, nullBuf.SafeIndexOf('i', 4));
        }

        [Fact]
        public void SafeStartsWith()
        {
            string nullBuf = null;
            string buf = "This is a test";

            Assert.True(buf.SafeStartsWith('T'));
            Assert.False(buf.SafeStartsWith('t'));
            Assert.False(nullBuf.SafeStartsWith('T'));
        }

        [Fact]
        public void SafeEndsWith()
        {
            string nullBuf = null;
            string buf = "This is a test";

            Assert.True(buf.SafeEndsWith('t'));
            Assert.False(buf.SafeEndsWith('T'));
            Assert.False(nullBuf.SafeEndsWith('T'));
        }

        [Fact]
        public void Mid()
        {
            string buf = "This is a test";
            Assert.Equal("is", buf.Mid(6, 2));
            Assert.Equal("This", buf.Mid(1, 4));
        }

        [Fact]
        public void DeleteLeft()
        {
            string nullBuf = null;
            string buf = "This is a test";
            Assert.Equal("is a test", buf.DeleteLeft(5));
            Assert.Equal("", nullBuf.DeleteLeft(5));
            Assert.Equal("", buf.DeleteLeft(14));
            Assert.Equal("", buf.DeleteLeft(25));
        }

        [Fact]
        public void DeleteRight()
        {
            string nullBuf = null;
            string buf = "This is a test";
            Assert.Equal("This is a", buf.DeleteRight(5));
            Assert.Equal("", nullBuf.DeleteRight(5));
            Assert.Equal("", buf.DeleteRight(14));
            Assert.Equal("", buf.DeleteRight(25));
        }

        [Fact]
        public void RemoveLineEndings()
        {
            string buf = "This is a test\r\nThis is a test\r\nThis is a test.\r\n";
            Assert.Equal("This is a testThis is a testThis is a test.", buf.RemoveLineEndings());
        }

        [Fact]
        public void TrimLengthWithEllipses()
        {
            string buf = "This is a test";
            Assert.Equal("This...", buf.TrimLengthWithEllipses(4));
            Assert.Equal("This is a test", buf.TrimLengthWithEllipses(25));
        }

        [Fact]
        public void TrimStart()
        {
            Assert.Equal("is a test", "This is a test".TrimStart("This "));
            Assert.Equal("This is a test", "This is a test".TrimStart("test "));

        }

        [Fact]
        public void TrimEnd()
        {
            Assert.Equal("This is a", "This is a".TrimEnd(" test"));
            Assert.Equal("This is a test", "This is a test".TrimEnd("asdf"));
        }

        [Fact]
        public void Trim()
        {
            Assert.Equal("one two one", "one two one".Trim(" two "));
            Assert.Equal("one two one", "one two one".Trim("asdf"));
            Assert.Equal(" two ", "one two one".Trim("one"));
        }

        [Fact]
        public void TrimEachLineWhitespace()
        {
            string buf = " One\r\n Two \r\n\tThree";
            Assert.Equal("One\r\nTwo\r\nThree", buf.TrimEachLineWhitespace());
        }

        [Fact]
        public void IsNumeric()
        {
            Assert.True("1000".IsNumeric());
            Assert.True("-1000".IsNumeric());
            Assert.True("1,000".IsNumeric());
            Assert.True("1.523".IsNumeric());
            Assert.False("$1.53".IsNumeric());
            Assert.False("One Hundred".IsNumeric());
            Assert.False("1: This is a test".IsNumeric());
        }

        [Fact]
        public void IsAlphaNumeric()
        {
            Assert.True("lucypell".IsAlphaNumeric());
            Assert.True("lucypell12".IsAlphaNumeric());
            Assert.True("LucyPell".IsAlphaNumeric());
            Assert.True("LucyPell19".IsAlphaNumeric());
            Assert.False("lucy pell".IsAlphaNumeric());
            Assert.False("lucy-pell".IsAlphaNumeric());
            Assert.False("@LucyPell19".IsAlphaNumeric());
        }
    }
}
