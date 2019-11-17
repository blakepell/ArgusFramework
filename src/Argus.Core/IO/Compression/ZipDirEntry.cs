// ZipDirEntry.cs
//
// Copyright (c) 2006, 2007, 2008 Microsoft Corporation.  All rights reserved.
//
// Part of an implementation of a zipfile class library. 
// See the file ZipFile.cs for the license and for further information.
//
// Tue, 27 Mar 2007  15:30

using System;
using System.IO;

namespace Argus.IO.Compression
{
    /// <summary>
    ///     This class models an entry in the directory contained within the zip file.
    ///     The class is generally not used from within application code, though it is
    ///     used by the ZipFile class.
    /// </summary>
    public class ZipDirEntry
    {
        private short _BitField;

        //private bool _Debug = false;
        private int _Crc32;
        private int _ExternalFileAttrs;
        private byte[] _Extra;
        private short _InternalFileAttrs;
        private int _LastModDateTime;

        private ZipDirEntry()
        {
        }

        internal ZipDirEntry(ZipEntry ze)
        {
        }

        /// <summary>
        ///     The time at which the file represented by the given entry was last modified.
        /// </summary>
        public DateTime LastModified { get; private set; }

        /// <summary>
        ///     The filename of the file represented by the given entry.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        ///     Any comment associated to the given entry. Comments are generally optional.
        /// </summary>
        public string Comment { get; private set; }

        /// <summary>
        ///     The version of the zip engine this archive was made by.
        /// </summary>
        public short VersionMadeBy { get; private set; }

        /// <summary>
        ///     The version of the zip engine this archive can be read by.
        /// </summary>
        public short VersionNeeded { get; private set; }

        /// <summary>
        ///     The compression method used to generate the archive.  Deflate is our favorite!
        /// </summary>
        public short CompressionMethod { get; private set; }

        /// <summary>
        ///     The size of the file, after compression. This size can actually be
        ///     larger than the uncompressed file size, for previously compressed
        ///     files, such as JPG files.
        /// </summary>
        public int CompressedSize { get; private set; }

        /// <summary>
        ///     The size of the file before compression.
        /// </summary>
        public int UncompressedSize { get; private set; }

        /// <summary>
        ///     True if the referenced entry is a directory.
        /// </summary>
        public bool IsDirectory => _InternalFileAttrs == 0 && (_ExternalFileAttrs & 0x0010) == 0x0010;

        /// <summary>
        ///     The calculated compression ratio for the given file.
        /// </summary>
        public double CompressionRatio => 100 * (1.0 - 1.0 * this.CompressedSize / (1.0 * this.UncompressedSize));

        /// <summary>
        ///     Reads one entry from the zip directory structure in the zip file.
        /// </summary>
        /// <param name="s">the stream from which to read.</param>
        /// <returns>the entry read from the archive.</returns>
        public static ZipDirEntry Read(Stream s)
        {
            int signature = Shared.ReadSignature(s);

            // return null if this is not a local file header signature
            if (IsNotValidSig(signature))
            {
                s.Seek(-4, SeekOrigin.Current);

                // Getting "not a ZipDirEntry signature" here is not always wrong or an error. 
                // This can happen when walking through a zipfile.  After the last ZipDirEntry, 
                // we expect to read an EndOfCentralDirectorySignature.  When we get this is how we 
                // know we've reached the end of the central directory. 
                if (signature != ZipConstants.EndOfCentralDirectorySignature)
                {
                    throw new Exception(string.Format("  ZipDirEntry::Read(): Bad signature ({0:X8}) at position 0x{1:X8}", signature, s.Position));
                }

                return null;
            }

            var block = new byte[42];
            int n = s.Read(block, 0, block.Length);

            if (n != block.Length)
            {
                return null;
            }

            int i = 0;
            var zde = new ZipDirEntry();

            zde.VersionMadeBy = (short) (block[i++] + block[i++] * 256);
            zde.VersionNeeded = (short) (block[i++] + block[i++] * 256);
            zde._BitField = (short) (block[i++] + block[i++] * 256);
            zde.CompressionMethod = (short) (block[i++] + block[i++] * 256);
            zde._LastModDateTime = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
            zde._Crc32 = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
            zde.CompressedSize = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
            zde.UncompressedSize = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;

            zde.LastModified = Shared.PackedToDateTime(zde._LastModDateTime);

            short filenameLength = (short) (block[i++] + block[i++] * 256);
            short extraFieldLength = (short) (block[i++] + block[i++] * 256);
            short commentLength = (short) (block[i++] + block[i++] * 256);
            short diskNumber = (short) (block[i++] + block[i++] * 256);
            zde._InternalFileAttrs = (short) (block[i++] + block[i++] * 256);
            zde._ExternalFileAttrs = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
            int Offset = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;

            block = new byte[filenameLength];
            n = s.Read(block, 0, block.Length);
            zde.FileName = Shared.StringFromBuffer(block, 0, block.Length);

            if (extraFieldLength > 0)
            {
                zde._Extra = new byte[extraFieldLength];
                n = s.Read(zde._Extra, 0, zde._Extra.Length);
            }

            if (commentLength > 0)
            {
                block = new byte[commentLength];
                n = s.Read(block, 0, block.Length);
                zde.Comment = Shared.StringFromBuffer(block, 0, block.Length);
            }

            return zde;
        }

        /// <summary>
        ///     Returns true if the passed-in value is a valid signature for a ZipDirEntry.
        /// </summary>
        /// <param name="signature">the candidate 4-byte signature value.</param>
        /// <returns>true, if the signature is valid according to the PKWare spec.</returns>
        public static bool IsNotValidSig(int signature)
        {
            return signature != ZipConstants.ZipDirEntrySignature;
        }
    }
}