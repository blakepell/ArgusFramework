/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-06
 * @last updated      : 2021-02-06
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using Xunit;

namespace Argus.UnitTests
{
    public class BoolTests
    {
        [Fact]
        public void IsTrue()
        {
            bool? value = true;
            Assert.True(value.IsTrue());

            value = false;
            Assert.False(value.IsTrue());

            value = null;
            Assert.False(value.IsTrue());
        }

        [Fact]
        public void IsFalse()
        {
            bool? value = false;
            Assert.True(value.IsFalse());

            value = true;
            Assert.False(value.IsFalse());

            value = null;
            Assert.True(value.IsFalse());
        }

        [Fact]
        public void ToBool()
        {
            bool? value = true;
            Assert.True(value.ToBool());

            value = false;
            Assert.False(value.ToBool());

            value = null;
            Assert.False(value.ToBool());
        }

    }
}
