/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-08
 * @last updated      : 2022-08-29
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using Xunit;

namespace Argus.UnitTests
{
    public class ObjectTests
    {
        [Fact]
        public void IsNull()
        {
            object o = null;
            Assert.True(o.IsNull());

            o = new();
            Assert.False(o.IsNull());
        }

        [Fact]
        public void ReferenceEqualsTest()
        {
            object a = new();
            object b = new();
            Assert.False(a.ReferenceEquals(b));

            a = b;
            Assert.True(a.ReferenceEquals(b));

            a = "This is a test";
            b = "This is a testx";
            Assert.False(a.ReferenceEquals(b));

            // Because these strings (jammed into object) are the same the reference
            // should match because the framework will point the same reference to the
            // single immutable copy.
            a = "This is a test";
            b = "This is a test";
            Assert.True(a.ReferenceEquals(b));
        }

        [Fact]
        public void ToJson()
        {
            var person = new Person
            {
                FirstName = "Blake",
                LastName = "Pell"
            };

            string json = person.ToJson();

            Assert.Equal("{\"FirstName\":\"Blake\",\"LastName\":\"Pell\"}", json);
        }

        [Fact]
        public void ToXml()
        {
            var person = new Person
            {
                FirstName = "Blake",
                LastName = "Pell"
            };

            string expectedXml = @"﻿<?xml version=""1.0"" encoding=""utf-8""?><Person xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><FirstName>Blake</FirstName><LastName>Pell</LastName></Person>";

            string xml = person.ToXml();

            Assert.Equal(expectedXml, xml);
        }

        [Fact]
        public void FromXml()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Person xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <FirstName>Blake</FirstName>
  <LastName>Pell</LastName>
</Person>";

            // I don't like this extension, it's weird to call it on a null.
            Person p = null;
            p = p.FromXml(xml);

            Assert.Equal("Blake", p.FirstName);
            Assert.Equal("Pell", p.LastName);
        }

        public class Person
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

        }

    }
}
