/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-08-13
 * @last updated      : 2021-08-13
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using Argus.Configuration;
using Xunit;

namespace Argus.UnitTests
{
    public class SettingsTests
    {
        [Fact]
        public void SettingsFileTest()
        {
            var oldPerson = new Person {FirstName = "Isaac", LastName = "Pell"};

            // Write it out
            var settings = new Settings();
            settings.Save(oldPerson, "Person.json");

            // Read it in
            var newPerson = settings.Load<Person>();

            // Cleanup the file
            settings.Delete();

            Assert.Equal(newPerson.FirstName, oldPerson.FirstName);
            Assert.Equal(newPerson.LastName, oldPerson.LastName);
        }
    }
}