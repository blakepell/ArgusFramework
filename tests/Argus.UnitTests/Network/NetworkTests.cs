/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2025-05-18
 * @last updated      : 2025-05-18
 * @copyright         : Copyright (c) 2003-2025, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Linq;
using System.Net;
using Argus.Network;
using Xunit;

namespace Argus.UnitTests.Network
{
    public class NetworkTests
    {
        [Fact]
        public void StatusCodeTests()
        {
            Assert.Equal("Bad Request", HttpStatusCodes.StatusDescription(400));
            Assert.NotEqual("Bad Request", HttpStatusCodes.StatusDescription(404));
            Assert.Equal("Unknown", HttpStatusCodes.StatusDescription(1024));
        }

        [Fact]
        public void IPv4Segments_String_ReturnsCorrectNumberOfAddresses()
        {
            var result = Argus.Network.Utilities.IPv4Segments("127.0.0.1").ToList();
            Assert.Equal(255, result.Count);
        }

        [Fact]
        public void IPv4Segments_IPAddress_ReturnsCorrectNumberOfAddresses()
        {
            var ip = IPAddress.Parse("127.0.0.1");
            var result = Argus.Network.Utilities.IPv4Segments(ip).ToList();

            Assert.Equal(255, result.Count);
        }

        [Fact]
        public void IPv4Segments_PreservesFirstThreeOctets()
        {
            var result = Argus.Network.Utilities.IPv4Segments("127.0.0.1").ToList();

            foreach (var address in result)
            {
                var octets = address.GetAddressBytes();
                Assert.Equal(192, octets[0]);
                Assert.Equal(168, octets[1]);
                Assert.Equal(1, octets[2]);
            }
        }

        [Fact]
        public void IPv4Segments_GeneratesAllLastOctetValues()
        {
            var result = Argus.Network.Utilities.IPv4Segments("10.0.0.1").ToList();
            Assert.Equal(255, result.Count);
            Assert.Equal("10.0.0.1", result.First().ToString());
            Assert.Equal("10.0.0.255", result.Last().ToString());
        }

        [Fact]
        public void IPv4Segments_StringAndIPAddressOverloads_ReturnSameResults()
        {
            var ipString = "172.16.5.10";
            var ipAddress = IPAddress.Parse(ipString);

            var resultFromString = Argus.Network.Utilities.IPv4Segments(ipString).ToList();
            var resultFromIPAddress = Argus.Network.Utilities.IPv4Segments(ipAddress).ToList();

            Assert.Equal(resultFromString, resultFromIPAddress);
        }

        [Theory]
        [InlineData("192.168.1.1")]
        [InlineData("10.0.0.255")]
        [InlineData("172.16.0.128")]
        public void IPv4Segments_DifferentInputs_GeneratesCorrectSegments(string ipString)
        {
            var ip = IPAddress.Parse(ipString);
            var octets = ip.GetAddressBytes();

            var results = Argus.Network.Utilities.IPv4Segments(ipString).ToList();

            Assert.Equal(255, results.Count);

            foreach (var address in results)
            {
                var resultOctets = address.GetAddressBytes();
                Assert.Equal(octets[0], resultOctets[0]);
                Assert.Equal(octets[1], resultOctets[1]);
                Assert.Equal(octets[2], resultOctets[2]);
                Assert.InRange(resultOctets[3], (byte)1, (byte)255);
            }
        }
    }
}