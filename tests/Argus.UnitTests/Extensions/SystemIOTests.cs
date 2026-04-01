/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System.IO;
using Xunit;

namespace Argus.UnitTests
{
    public class SystemIOTests
    {
        [Fact]
        public void FormattedFileSize_ReturnsNonEmptyString()
        {
            var tempFile = Path.GetTempFileName();

            try
            {
                File.WriteAllText(tempFile, "Hello, World!");
                var fi = new FileInfo(tempFile);

                var result = fi.FormattedFileSize();

                Assert.False(string.IsNullOrWhiteSpace(result));
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void CreateSha256Hash_ReturnsDeterministicHash()
        {
            var tempFile = Path.GetTempFileName();

            try
            {
                File.WriteAllText(tempFile, "Test content for hashing");
                var fi = new FileInfo(tempFile);

                var hash1 = fi.CreateSha256Hash();
                var hash2 = fi.CreateSha256Hash();

                Assert.False(string.IsNullOrWhiteSpace(hash1));
                Assert.Equal(hash1, hash2);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void CreateSha512Hash_ReturnsDeterministicHash()
        {
            var tempFile = Path.GetTempFileName();

            try
            {
                File.WriteAllText(tempFile, "Test content for hashing");
                var fi = new FileInfo(tempFile);

                var hash1 = fi.CreateSha512Hash();
                var hash2 = fi.CreateSha512Hash();

                Assert.False(string.IsNullOrWhiteSpace(hash1));
                Assert.Equal(hash1, hash2);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void CreateMD5_ReturnsDeterministicHash()
        {
            var tempFile = Path.GetTempFileName();

            try
            {
                File.WriteAllText(tempFile, "Test content for hashing");
                var fi = new FileInfo(tempFile);

                var hash1 = fi.CreateMD5();
                var hash2 = fi.CreateMD5();

                Assert.False(string.IsNullOrWhiteSpace(hash1));
                Assert.Equal(hash1, hash2);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void Hashes_DifferentAlgorithms_ProduceDifferentHashes()
        {
            var tempFile = Path.GetTempFileName();

            try
            {
                File.WriteAllText(tempFile, "Test content for hashing");
                var fi = new FileInfo(tempFile);

                var sha256 = fi.CreateSha256Hash();
                var sha512 = fi.CreateSha512Hash();
                var md5 = fi.CreateMD5();

                Assert.NotEqual(sha256, sha512);
                Assert.NotEqual(sha256, md5);
                Assert.NotEqual(sha512, md5);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }
}
