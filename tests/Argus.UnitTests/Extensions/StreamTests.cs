/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-09
 * @last updated      : 2021-02-09
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System.IO;
using System.Text;
using Argus.Extensions;
using Xunit;

namespace Argus.UnitTests
{
    public class StreamTests
    {
        [Fact]
        public void ToText()
        {
            var ms = new MemoryStream();

            using (var sw = new StreamWriter(ms))
            {
                sw.Write("This is a test");
                sw.Flush();

                Assert.Equal("This is a test", ms.ToText());
            }

        }

        [Fact]
        public void ToBase64FromMemoryStream()
        {
            var ms = new MemoryStream();

            using (var sw = new StreamWriter(ms))
            {
                sw.Write("This is a test");
                sw.Flush();

                Assert.Equal("VGhpcyBpcyBhIHRlc3Q=", ms.ToBase64());
            }

            ms.Dispose();
        }

        [Fact]
        public void ToBase64FromStream()
        {
            Stream s = new MemoryStream();
            
            using (var sw = new StreamWriter(s))
            {
                sw.Write("This is a test");
                sw.Flush();

                Assert.Equal("VGhpcyBpcyBhIHRlc3Q=", s.ToBase64());
            }

            s.Dispose();
        }

    }
}
