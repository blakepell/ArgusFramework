/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-06
 * @last updated      : 2021-02-06
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Collections.Generic;
using System.Text;
using Argus.Data.RegEx;
using Xunit;

namespace Argus.UnitTests.Data
{
    public class SimpleRegExTests
    {

        [Fact]
        public void MatchTests()
        {
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("".AsSpan(), "".AsSpan()));
            Assert.False(SimpleRegex.IsMatchWithStarWildcard("a".AsSpan(), "".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("a".AsSpan(), "*".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("a".AsSpan(), "a".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("A".AsSpan(), "a".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("a".AsSpan(), "A".AsSpan()));
            Assert.False(SimpleRegex.IsMatchWithStarWildcard("a".AsSpan(), "b".AsSpan()));
            Assert.False(SimpleRegex.IsMatchWithStarWildcard(" a".AsSpan(), "a".AsSpan()));
            Assert.False(SimpleRegex.IsMatchWithStarWildcard("a ".AsSpan(), "a".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("aaa".AsSpan(), "*".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("aaa".AsSpan(), "*****".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("example.com".AsSpan(), "*.com".AsSpan()));
            Assert.False(SimpleRegex.IsMatchWithStarWildcard("example.com".AsSpan(), "*.net".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("sub.example.com".AsSpan(), "*.com".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("sub.example.com".AsSpan(), "*.example.com".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("SuB.eXaMpLe.com".AsSpan(), "*.example.com".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("sub2.sub1.example.com".AsSpan(), "*.example.com".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("sub2.sub1.example.com".AsSpan(), "*.*.example.com".AsSpan()));
            Assert.False(SimpleRegex.IsMatchWithStarWildcard("sub.example.com".AsSpan(), "*.*.example.com".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("sub.example.com".AsSpan(), "*.*.*".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("sub.example.com".AsSpan(), "*".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("abcdefg".AsSpan(), "*a*b*c*d**e***f****g*****".AsSpan()));
            Assert.False(SimpleRegex.IsMatchWithStarWildcard("abcdefg".AsSpan(), "*a*b*c*de**e***f****g****".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard(".".AsSpan(), "*.*".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("ab.cde".AsSpan(), "*.*".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard(".cde".AsSpan(), "*.*".AsSpan()));
            Assert.False(SimpleRegex.IsMatchWithStarWildcard("cde".AsSpan(), "*.*".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("cde".AsSpan(), "cd*".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("192.168.1.123".AsSpan(), "192.168.1.*".AsSpan()));
            Assert.True(SimpleRegex.IsMatchWithStarWildcard("192.168.1.123".AsSpan(), "192.168.*".AsSpan()));
            Assert.False(SimpleRegex.IsMatchWithStarWildcard("192.168.2.123".AsSpan(), "192.168.1.*".AsSpan()));
        }

    }
}
