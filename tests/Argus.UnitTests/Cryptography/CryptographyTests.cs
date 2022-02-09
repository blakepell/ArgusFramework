/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-06
 * @last updated      : 2021-02-06
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Cryptography;
using System.Security.Cryptography;
using Xunit;

namespace Argus.UnitTests
{
    public class CryptographyTests
    {
        [Fact]
        public void Md5Test()
        {
            string hash = HashUtilities.MD5Hash("Avalon Mud Client");
            Assert.Equal("9ff452c9e9cf45cdb36e041fb4814f71", hash);
        }

        [Fact]
        public void Sha1Test()
        {
            string hash = HashUtilities.Sha1Hash("Avalon Mud Client");
            Assert.Equal("86ee3d007e14ea8bbd76ef7f4cbd483a842dbf1c", hash);
        }

        [Fact]
        public void Sha256Test()
        {
            string hash = HashUtilities.Sha256Hash("Avalon Mud Client");
            Assert.Equal("2d0527450a34963b8a8b6c5111d6bbd88f2546c4ce5f04f8c89d2b940101e151", hash);
        }

        [Fact]
        public void Sha384Test()
        {
            string hash = HashUtilities.Sha384Hash("Avalon Mud Client");
            Assert.Equal("fd1faaea2178599562501ee09c223d8785e1a687440984cb2078fd4c04fba4ef493d329926377f76e0607633bceab3ec", hash);
        }

        [Fact]
        public void Sha512Test()
        {
            string hash = HashUtilities.Sha512Hash("Avalon Mud Client");
            Assert.Equal("6c625feda3d935d6dc980e1a12dbf6917f1fe5b01300253b55b6995ffc30e4c6cfb5a2548c4a526cd72a9e88beaf59ff4e7dac90405dd6d9a34899f8caada428", hash);
        }

        [Fact]
        public void EncryptToStringAesBase64()
        {
            var crypt = new Encryption();
            string value = crypt.EncryptToString("Avalon Mud Client", "is a great mud client");
            Assert.Equal("sRhsHpicn6Q8LFR0YOMdE3YsKBiHUKtmRt0yUCsibPw=", value);
        }

        [Fact]
        public void EncryptToStringRijndaelBase64()
        {
            var crypt = new Encryption();
            string value = crypt.EncryptToString<RijndaelManaged>("Avalon Mud Client", "is a great mud client");
            Assert.Equal("sRhsHpicn6Q8LFR0YOMdE3YsKBiHUKtmRt0yUCsibPw=", value);
        }
    }
}
