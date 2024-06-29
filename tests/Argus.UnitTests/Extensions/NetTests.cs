/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-07
 * @last updated      : 2024-06-29
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace Argus.UnitTests
{
    public class NetTests
    {
        private readonly ITestOutputHelper _output;
        public NetTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void Test1()
        {
            _output.WriteLine("Hello");
        }
        
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

        /// <summary>
        /// Test for the IP segments list.
        /// </summary>
        [Fact]
        public void IPv4SegmentsTest()
        {
            var list = new List<IPAddress>();
            
            foreach (var ip in Argus.Network.Utilities.IPv4Segments("192.168.1.1"))
            {
                list.Add(ip);
            }
            
            Assert.Equal(255, list.Count);
            Assert.Equal("192.168.1.1", list[0].ToString());
            Assert.Equal("192.168.1.255", list[254].ToString());
        }

        [Fact]
        public void IPv4SegmentsSubnetsTest()
        {
            var list = new List<IPAddress>();
            
            foreach (var ip in Argus.Network.Utilities.IPv4SegmentsSubnets("192.168.1.1"))
            {
                list.Add(ip);
            }
            
            Assert.Equal(65025, list.Count);
            Assert.Equal("192.168.1.1", list[0].ToString());
            Assert.Equal("192.168.255.255", list[65024].ToString());
        }
    }
}
