/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-07
 * @last updated      : 2021-02-07
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System;
using Xunit;

namespace Argus.UnitTests
{
    public class NetTests
    {
        [Fact]
        public void FileName()
        {
            var uri = new Uri("https://www.blakepell.com/Test.aspx");
            Assert.Equal("Test.aspx", uri.FileName());

            uri = new Uri("C:\\TempFolder\\This is a filename.txt");
            Assert.Equal("This is a filename.txt", uri.FileName());

            uri = new Uri("C:\\TempFolder\\This is a filename.txt");
            Assert.NotEqual("This is another filename.txt", uri.FileName());
        }
    }
}
