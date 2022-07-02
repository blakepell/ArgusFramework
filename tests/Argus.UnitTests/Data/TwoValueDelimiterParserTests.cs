/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2022-07-02
 * @last updated      : 2022-07-02
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System;
using Argus.Data;
using Xunit;

namespace Argus.UnitTests.Data
{
    public class TwoValueDelimiterParserTests
    {
        [Fact]
        public void StringMatchTests()
        {
            string text = "left_right";
            var parser = new TwoValueDelimiterParser(text);
            Assert.False(parser.LeftValue().Equals("left"));
            Assert.False(parser.RightValue().Equals("right"));

            parser = "left,right";
            Assert.True(parser.LeftValue().Equals("left"));
            Assert.True(parser.RightValue().Equals("right"));

            parser.Delimiter = '=';
            Assert.False(parser.LeftValue().Equals("left"));
            Assert.False(parser.RightValue().Equals("right"));

            parser.Delimiter = ',';
            Assert.True(parser.LeftValue().Equals("left"));
            Assert.True(parser.RightValue().Equals("right"));

            Assert.True(parser.LeftValueAsSpan().SequenceEqual("left"));
            Assert.True(parser.RightValueAsSpan().SequenceEqual("right"));

            parser = "";
            Assert.False(parser.LeftValue().Equals("left"));
            Assert.False(parser.RightValue().Equals("right"));
            Assert.True(parser.RightValue().Equals(""));
        }
    }
}
