/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2022-07-05
 * @last updated      : 2022-07-05
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System;
using Argus.Data;
using Argus.IO;
using Xunit;

namespace Argus.UnitTests.IO
{
    public class JsonFileServiceTests
    {
        public class Person
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }
        }

        [Fact]
        public void ReadWrite()
        {
            var p = new Person()
            {
                FirstName = "Blake",
                LastName = "Pell"
            };

            var file = new JsonFileService(Environment.SpecialFolder.LocalApplicationData, "Tests");
            file.Save(p, "Person.json");

            var p2 = file.Read<Person>("Person.json");

            Assert.Equal(p.FirstName, p2.FirstName);
            Assert.Equal(p.LastName, p2.LastName);
        }

        [Fact]
        public async void ReadWriteAsync()
        {
            var p = new Person()
            {
                FirstName = "Blake",
                LastName = "Pell"
            };

            var file = new JsonFileService(Environment.SpecialFolder.LocalApplicationData, "Tests");
            await file.SaveAsync(p, "Person.json");

            var p2 = await file.ReadAsync<Person>("Person.json");

            Assert.Equal(p.FirstName, p2.FirstName);
            Assert.Equal(p.LastName, p2.LastName);
        }

    }
}