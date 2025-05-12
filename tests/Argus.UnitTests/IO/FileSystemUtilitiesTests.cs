/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2024-11-16
 * @last updated      : 2024-11-16
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

using Argus.IO;
using Xunit;

namespace Argus.UnitTests.IO
{
    public class FileSystemUtilityTests
    {
        [Fact]
        public void ExtractFilename()
        {
            Assert.Equal("test.txt", FileSystemUtilities.ExtractFileName(@"C:\Temp\test.txt"));
            Assert.Equal("test.txt", FileSystemUtilities.ExtractFileName(@"C:\test.txt"));
            Assert.Equal("test.txt", FileSystemUtilities.ExtractFileName(@"C:\Program Files (x86)\test.txt"));
        }

        [Fact]
        public void ExtractDirectorhPath()
        {
            Assert.Equal(@"C:\Temp\", FileSystemUtilities.ExtractDirectoryPath(@"C:\Temp\test.txt"));
            Assert.Equal(@"C:\", FileSystemUtilities.ExtractDirectoryPath(@"C:\test.txt"));
            Assert.Equal(@"C:\Program Files (x86)\", FileSystemUtilities.ExtractDirectoryPath(@"C:\Program Files (x86)\test.txt"));
        }
    }
}