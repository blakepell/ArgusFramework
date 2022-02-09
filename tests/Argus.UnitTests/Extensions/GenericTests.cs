/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-07
 * @last updated      : 2021-02-07
 * @copyright         : Copyright7(c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Argus.UnitTests
{
    public class GenericTests
    {
        [Fact]
        public void IsNull()
        {
            string buf = null;
            Assert.True(buf.IsNull());
            
            buf = "Testing";
            Assert.False(buf.IsNull());

            bool? b = null;
            Assert.True(b.IsNull());

            b = true;
            Assert.False(b.IsNull());
        }

        [Fact]
        public void Set()
        {
            var p = new Person();
            p.Set("FirstName", "Blake");
            Assert.Equal("Blake", p.FirstName);
        }

        [Fact]
        public void Get()
        {
            var p = new Person();
            p.LastName = "Pell";
            Assert.Equal("Pell", p.Get("LastName"));
        }

        [Fact]
        public void TakeLast()
        {
            var list = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                list.Add(i);
            }

            Assert.Equal(9, list.TakeLast(1).First());
            Assert.Equal(5, list.TakeLast(5).Count());
        }

        [Fact]
        public void CopyFrom()
        {
            var p = new Person();
            p.FirstName = "Blake";
            p.LastName = "Pell";

            var p2 = new Person();
            p2.CopyFrom(p);

            Assert.Equal("Blake", p2.FirstName);
            Assert.Equal("Pell", p2.LastName);
        }

        [Fact]
        public void AddIfDoesntExist()
        {
            var list = new List<string>();
            list.AddIfDoesntExist("Blake");
            list.AddIfDoesntExist("Pell");
            list.AddIfDoesntExist("Blake");
            list.AddIfDoesntExist("Pell");

            Assert.Equal(2, list.Count);
            Assert.Equal("Blake", list[0]);
            Assert.Equal("Pell", list[1]);
        }

        [Fact]
        public void AddIf()
        {
            var list = new List<string>();
            list.AddIf("Blake", true);
            list.AddIf("Pell", true);
            list.AddIf("Blake", false);
            list.AddIf("Pell", false);

            Assert.Equal(2, list.Count);
            Assert.Equal("Blake", list[0]);
            Assert.Equal("Pell", list[1]);
        }

        [Fact]
        public void In()
        {
            var list = new List<string>
            {
                "Blake",
                "Pell"
            };

            Assert.True("Blake".In(list));
            Assert.False("Taco Truck".In(list));

            var array = new[]
            {
                "Blake",
                "Pell"
            };

            Assert.True("Blake".In(array));
            Assert.False("Taco Truck".In(array));
        }

        [Fact]
        public void RemoveAt()
        {
            string[] array = {"One", "Two", "Three", "Four", "Five"};
            array = array.RemoveAt(2);
            Assert.Equal("One", array[0]);
            Assert.Equal("Two", array[1]);
            Assert.Equal("Four", array[2]);
            Assert.Equal("Five", array[3]);
        }

        private class Person
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }
        }

    }
}