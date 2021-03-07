/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-06
 * @last updated      : 2021-02-06
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System.Text;
using Argus.Extensions;
using Xunit;

namespace Argus.UnitTests
{
    public class ByteTests
    {
        [Fact]
        public void ToBase64StringTest()
        {
            var b = "Blake Pell".ToBytes(Encoding.UTF8);
            string base64 = b.ToBase64String();

            Assert.Equal("Qmxha2UgUGVsbA==", base64);
        }

        [Fact]
        public void ToStringTest()
        {
            var b = "Blake Pell".ToBytes(Encoding.UTF8);
            string value = b.ToString(Encoding.UTF8);

            Assert.Equal("Blake Pell", value);
        }

    }
}
