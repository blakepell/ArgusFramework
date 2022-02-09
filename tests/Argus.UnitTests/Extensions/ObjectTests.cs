/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-2-8
 * @last updated      : 2021-2-8
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.ComponentModel;
using System.Text;
using Argus.Extensions;
using Xunit;

namespace Argus.UnitTests
{
    public class ObjectTests
    {
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

            string expectedXml = @"<?xml version=""1.0""?>
<Person xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <FirstName>Blake</FirstName>
  <LastName>Pell</LastName>
</Person>";

            string xml = person.ToXml();

            Assert.Equal(expectedXml, xml);
        }

        [Fact]
        public void FromXml()
        {
            string xml = @"<?xml version=""1.0""?>
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
