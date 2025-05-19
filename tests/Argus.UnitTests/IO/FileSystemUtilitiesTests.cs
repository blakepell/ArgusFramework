/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2024-11-16
 * @last updated      : 2025-05-18
 * @copyright         : Copyright (c) 2003-2025, All rights reserved.
 * @license           : MIT
 */

using System;
using Argus.IO;
using Xunit;

namespace Argus.UnitTests.IO
{
    public class FileSystemUtilityTests
    {
        [Fact]
        public void AnySegmentEquals_NullOrEmptyFolderPath_ReturnsFalse()
        {
            Assert.False(FileSystemUtilities.AnySegmentEquals(null, StringComparison.OrdinalIgnoreCase, "test"));
            Assert.False(FileSystemUtilities.AnySegmentEquals("", StringComparison.OrdinalIgnoreCase, "test"));
        }

        [Fact]
        public void AnySegmentEquals_NullOrEmptySearchDirectoryNames_ReturnsFalse()
        {
            Assert.False(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path", StringComparison.OrdinalIgnoreCase, null));
            Assert.False(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path", StringComparison.OrdinalIgnoreCase, new string[0]));
        }

        [Fact]
        public void AnySegmentEquals_MatchFound_ReturnsTrue()
        {
            Assert.True(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.OrdinalIgnoreCase, "Path"));
            Assert.True(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.OrdinalIgnoreCase, "Test"));
            Assert.True(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.OrdinalIgnoreCase, "NonMatch", "Path"));
        }

        [Fact]
        public void AnySegmentEquals_NoMatchFound_ReturnsFalse()
        {
            Assert.False(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.OrdinalIgnoreCase, "NotInPath"));
            Assert.False(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.OrdinalIgnoreCase, "Files", "NotInPath"));
        }

        [Fact]
        public void AnySegmentEquals_CaseSensitivity_WorksCorrectly()
        {
            // Case insensitive comparison should match regardless of case
            Assert.True(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.OrdinalIgnoreCase, "test"));
            Assert.True(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.OrdinalIgnoreCase, "PATH"));

            // Case sensitive comparison should only match exact case
            Assert.False(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.Ordinal, "test"));
            Assert.False(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.Ordinal, "PATH"));
            Assert.True(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.Ordinal, "Test"));
            Assert.True(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.Ordinal, "Path"));
        }

        [Fact]
        public void AnySegmentEquals_DifferentPathSeparators_WorksCorrectly()
        {
            // Test with forward slashes
            Assert.True(FileSystemUtilities.AnySegmentEquals("C:/Test/Path/File.txt", StringComparison.OrdinalIgnoreCase, "Test"));

            // Test with mixed separators
            Assert.True(FileSystemUtilities.AnySegmentEquals("C:/Test\\Path/File.txt", StringComparison.OrdinalIgnoreCase, "Path"));
        }

        [Fact]
        public void AnySegmentEquals_EmptySegmentInSearchDirectory_HandledCorrectly()
        {
            Assert.True(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.OrdinalIgnoreCase, "Test", ""));
            Assert.True(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.OrdinalIgnoreCase, "Test", null, ""));
            Assert.False(FileSystemUtilities.AnySegmentEquals(@"C:\Test\Path\File.txt", StringComparison.OrdinalIgnoreCase, "Gits", null, ""));
        }

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