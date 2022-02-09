/*
 * @author            : Microsoft
 * @copyright         : Copyright (c) 2006-2008, All rights reserved.
 * @license           : Microsoft Public License
 */

// ZipEntry.cs
//
// Copyright (c) 2006, 2007, 2008 Microsoft Corporation.  All rights reserved.
//
// Part of an implementation of a zipfile class library. 
// See the file ZipFile.cs for the license and for further information.
//
// Tue, 27 Mar 2007  15:30

using System.IO.Compression;

namespace Argus.IO.Compression
{
    /// <summary>
    /// Represents a single entry in a ZipFile. Typically, applications
    /// get a ZipEntry by enumerating the entries within a ZipFile.
    /// </summary>
    public class ZipEntry
    {
        private readonly bool _Debug = false;
        private byte[] __filedata;
        private DeflateStream _CompressedStream;
        private int _Crc32;
        private byte[] _Extra;
        private int _LastModDateTime;
        private DateTime _LastModified;
        private int _RelativeOffsetOfHeader;
        private MemoryStream _UnderlyingMemoryStream;

        private ZipEntry()
        {
        }

        /// <summary>
        /// The time and date at which the file indicated by the ZipEntry was last modified.
        /// </summary>
        public DateTime LastModified => _LastModified;

        /// <summary>
        /// When this is set, this class trims the volume (eg C:\) from any
        /// fully-qualified pathname on the ZipEntry,
        /// before writing the ZipEntry into the ZipFile. This flag affects only
        /// zip creation.
        /// </summary>
        public bool TrimVolumeFromFullyQualifiedPaths { get; set; } = true;

        /// <summary>
        /// The name of the filesystem file, referred to by the ZipEntry.
        /// This may be different than the path used in the archive itself.
        /// </summary>
        public string LocalFileName { get; private set; }

        /// <summary>
        /// The name of the file contained in the ZipEntry.
        /// When writing a zip, this path has backslashes replaced with
        /// forward slashes, according to the zip spec, for compatibility
        /// with Unix and Amiga.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// The version of the zip engine needed to read the ZipEntry.  This is usually 0x14.
        /// </summary>
        public short VersionNeeded { get; private set; }

        /// <summary>
        /// The comment attached to the ZipEntry.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// a bitfield as defined in the zip spec.
        /// </summary>
        public short BitField { get; private set; }

        /// <summary>
        /// The compression method employed for this ZipEntry. 0x08 = Deflate.  0x00 = Store (no compression).
        /// </summary>
        public short CompressionMethod { get; private set; }

        /// <summary>
        /// The compressed size of the file, in bytes, within the zip archive.
        /// </summary>
        public int CompressedSize { get; private set; }

        /// <summary>
        /// The size of the file, in bytes, before compression, or after extraction.
        /// </summary>
        public int UncompressedSize { get; private set; }

        /// <summary>
        /// The ratio of compressed size to uncompressed size.
        /// </summary>
        public double CompressionRatio => 100 * (1.0 - 1.0 * this.CompressedSize / (1.0 * this.UncompressedSize));

        /// <summary>
        /// True if the entry is a directory (not a file).
        /// This is a readonly property on the entry.
        /// </summary>
        public bool IsDirectory { get; private set; }

        /// <summary>
        /// Specifies that the extraction should overwrite any existing files.
        /// This applies only when calling an Extract method.
        /// </summary>
        public bool OverwriteOnExtract { get; set; }

        private byte[] _FileData
        {
            get
            {
                if (__filedata == null)
                {
                }

                return __filedata;
            }
        }

        private DeflateStream CompressedStream
        {
            get
            {
                if (_CompressedStream == null)
                {
                    // we read from the underlying memory stream after data is written to the compressed stream
                    _UnderlyingMemoryStream = new MemoryStream();
                    bool LeaveUnderlyingStreamOpen = true;

                    // we write to the compressed stream, and compression happens as we write.
                    _CompressedStream = new DeflateStream(_UnderlyingMemoryStream,
                                                          CompressionMode.Compress,
                                                          LeaveUnderlyingStreamOpen);
                }

                return _CompressedStream;
            }
        }

        internal byte[] Header { get; private set; }

        private static bool ReadHeader(Stream s, ZipEntry ze)
        {
            int signature = Shared.ReadSignature(s);

            // Return false if this is not a local file header signature.
            if (IsNotValidSig(signature))
            {
                s.Seek(-4, SeekOrigin.Current); // unread the signature

                // Getting "not a ZipEntry signature" is not always wrong or an error. 
                // This can happen when walking through a zipfile.  After the last compressed entry, 
                // we expect to read a ZipDirEntry signature.  When we get this is how we 
                // know we've reached the end of the compressed entries. 
                if (ZipDirEntry.IsNotValidSig(signature))
                {
                    throw new Exception(string.Format("  ZipEntry::Read(): Bad signature ({0:X8}) at position  0x{1:X8}", signature, s.Position));
                }

                return false;
            }

            var block = new byte[26];
            int n = s.Read(block, 0, block.Length);

            if (n != block.Length)
            {
                return false;
            }

            int i = 0;
            ze.VersionNeeded = (short) (block[i++] + block[i++] * 256);
            ze.BitField = (short) (block[i++] + block[i++] * 256);
            ze.CompressionMethod = (short) (block[i++] + block[i++] * 256);
            ze._LastModDateTime = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;

            // the PKZIP spec says that if bit 3 is set (0x0008), then the CRC, Compressed size, and uncompressed size
            // come directly after the file data.  The only way to find it is to scan the zip archive for the signature of 
            // the Data Descriptor, and presume that that signature does not appear in the (compressed) data of the compressed file.  

            if ((ze.BitField & 0x0008) != 0x0008)
            {
                ze._Crc32 = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
                ze.CompressedSize = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
                ze.UncompressedSize = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
            }
            else
            {
                // the CRC, compressed size, and uncompressed size are stored later in the stream.
                // here, we advance the pointer.
                i += 12;
            }

            short filenameLength = (short) (block[i++] + block[i++] * 256);
            short extraFieldLength = (short) (block[i++] + block[i++] * 256);

            block = new byte[filenameLength];
            n = s.Read(block, 0, block.Length);
            ze.FileName = Shared.StringFromBuffer(block, 0, block.Length);

            // when creating an entry by reading, the LocalFileName is the same as the FileNameInArchivre
            ze.LocalFileName = ze.FileName;

            ze._Extra = new byte[extraFieldLength];
            n = s.Read(ze._Extra, 0, ze._Extra.Length);

            // transform the time data into something usable
            ze._LastModified = Shared.PackedToDateTime(ze._LastModDateTime);

            // actually get the compressed size and CRC if necessary
            if ((ze.BitField & 0x0008) == 0x0008)
            {
                long posn = s.Position;
                long SizeOfDataRead = Shared.FindSignature(s, ZipConstants.ZipEntryDataDescriptorSignature);

                if (SizeOfDataRead == -1)
                {
                    return false;
                }

                // read 3x 4-byte fields (CRC, Compressed Size, Uncompressed Size)
                block = new byte[12];
                n = s.Read(block, 0, block.Length);

                if (n != 12)
                {
                    return false;
                }

                i = 0;
                ze._Crc32 = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
                ze.CompressedSize = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
                ze.UncompressedSize = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;

                if (SizeOfDataRead != ze.CompressedSize)
                {
                    throw new Exception("Data format error (bit 3 is set)");
                }

                // seek back to previous position, to read file data
                s.Seek(posn, SeekOrigin.Begin);
            }

            return true;
        }

        private static bool IsNotValidSig(int signature)
        {
            return signature != ZipConstants.ZipEntrySignature;
        }

        /// <summary>
        /// Reads one ZipEntry from the given stream.
        /// </summary>
        /// <param name="s">the stream to read from.</param>
        /// <returns>the ZipEntry read from the stream.</returns>
        internal static ZipEntry Read(Stream s)
        {
            var entry = new ZipEntry();

            if (!ReadHeader(s, entry))
            {
                return null;
            }

            entry.__filedata = new byte[entry.CompressedSize];
            int n = s.Read(entry._FileData, 0, entry._FileData.Length);

            if (n != entry._FileData.Length)
            {
                throw new Exception("badly formatted zip file.");
            }

            // finally, seek past the (already read) Data descriptor if necessary
            if ((entry.BitField & 0x0008) == 0x0008)
            {
                s.Seek(16, SeekOrigin.Current);
            }

            return entry;
        }

        internal static ZipEntry Create(string filename)
        {
            return Create(filename, null);
        }

        internal static ZipEntry Create(string filename, string DirectoryPathInArchive)
        {
            var entry = new ZipEntry();
            entry.LocalFileName = filename; // may include a path

            if (DirectoryPathInArchive == null)
            {
                entry.FileName = filename;
            }
            else
            {
                // explicitly specify a pathname for this file  
                entry.FileName =
                    Path.Combine(DirectoryPathInArchive, Path.GetFileName(filename));
            }

            entry._LastModified = File.GetLastWriteTime(filename);

            // adjust the time if the .NET BCL thinks it is in DST.  
            // see the note elsewhere in this file for more info. 
            if (entry._LastModified.IsDaylightSavingTime())
            {
                var AdjustedTime = entry._LastModified - new TimeSpan(1, 0, 0);
                entry._LastModDateTime = Shared.DateTimeToPacked(AdjustedTime);
            }
            else
            {
                entry._LastModDateTime = Shared.DateTimeToPacked(entry._LastModified);
            }

            // we don't actually slurp in the file until the caller invokes Write on this entry.

            return entry;
        }

        /// <summary>
        /// Extract the entry to the filesystem, starting at the current working directory.
        /// </summary>
        /// <overloads>This method has five overloads.</overloads>
        /// <remarks>
        /// <para>
        /// The last modified time of the created file may be adjusted
        /// during extraction to compensate
        /// for differences in how the .NET Base Class Library deals
        /// with daylight saving time (DST) versus how the Windows
        /// filesystem deals with daylight saving time.
        /// See http://blogs.msdn.com/oldnewthing/archive/2003/10/24/55413.aspx for more context.
        /// </para>
        /// <para>
        /// In a nutshell: Daylight savings time rules change regularly.  In
        /// 2007, for example, the inception week of DST changed.  In 1977,
        /// DST was in place all year round. in 1945, likewise.  And so on.
        /// Win32 does not attempt to guess which time zone rules were in
        /// effect at the time in question.  It will render a time as
        /// "standard time" and allow the app to change to DST as necessary.
        /// .NET makes a different choice.
        /// </para>
        /// <para>
        /// Compare the output of FileInfo.LastWriteTime.ToString("f") with
        /// what you see in the property sheet for a file that was last
        /// written to on the other side of the DST transition. For example,
        /// suppose the file was last modified on October 17, during DST but
        /// DST is not currently in effect. Explorer's file properties
        /// reports Thursday, October 17, 2003, 8:45:38 AM, but .NETs
        /// FileInfo reports Thursday, October 17, 2003, 9:45 AM.
        /// </para>
        /// <para>
        /// Win32 says, "Thursday, October 17, 2002 8:45:38 AM PST". Note:
        /// Pacific STANDARD Time. Even though October 17 of that year
        /// occurred during Pacific Daylight Time, Win32 displays the time as
        /// standard time because that's what time it is NOW.
        /// </para>
        /// <para>
        /// .NET BCL assumes that the current DST rules were in place at the
        /// time in question.  So, .NET says, "Well, if the rules in effect
        /// now were also in effect on October 17, 2003, then that would be
        /// daylight time" so it displays "Thursday, October 17, 2003, 9:45
        /// AM PDT" - daylight time.
        /// </para>
        /// <para>
        /// So .NET gives a value which is more intuitively correct, but is
        /// also potentially incorrect, and which is not invertible. Win32
        /// gives a value which is intuitively incorrect, but is strictly
        /// correct.
        /// </para>
        /// <para>
        /// With this adjustment, I add one hour to the tweaked .NET time, if
        /// necessary.  That is to say, if the time in question had occurred
        /// in what the .NET BCL assumed to be DST (an assumption that may be
        /// wrong given the constantly changing DST rules).
        /// </para>
        /// </remarks>
        public void Extract()
        {
            this.Extract(".");
        }

        /// <summary>
        /// Extract the entry to a file in the filesystem, potentially overwriting
        /// any existing file.
        /// </summary>
        /// <remarks>
        /// <para>
        /// See the remarks on the non-parameterized version of the extract() method,
        /// for information on the last modified time of the created file.
        /// </para>
        /// </remarks>
        /// <param name="WantOverwrite">true if the caller wants to overwrite an existing file by the same name in the filesystem.</param>
        public void Extract(bool WantOverwrite)
        {
            this.Extract(".", WantOverwrite);
        }

        /// <summary>
        /// Extracts the entry to the specified stream.
        /// For example, the caller could specify Console.Out, or a MemoryStream.
        /// </summary>
        /// <param name="s">the stream to which the entry should be extracted.  </param>
        public void Extract(Stream s)
        {
            this.Extract(null, s);
        }

        /// <summary>
        /// Extract the entry to the filesystem, starting at the specified base directory.
        /// </summary>
        /// <para>
        /// See the remarks on the non-parameterized version of the extract() method,
        /// for information on the last modified time of the created file.
        /// </para>
        /// <param name="BaseDirectory">the pathname of the base directory</param>
        public void Extract(string BaseDirectory)
        {
            this.Extract(BaseDirectory, null);
        }

        /// <summary>
        /// Extract the entry to the filesystem, starting at the specified base directory,
        /// and potentially overwriting existing files in the filesystem.
        /// </summary>
        /// <para>
        /// See the remarks on the non-parameterized version of the extract() method,
        /// for information on the last modified time of the created file.
        /// </para>
        /// <param name="BaseDirectory">the pathname of the base directory</param>
        /// <param name="Overwrite">If true, overwrite any existing files if necessary upon extraction.</param>
        public void Extract(string BaseDirectory, bool Overwrite)
        {
            this.OverwriteOnExtract = Overwrite;
            this.Extract(BaseDirectory, null);
        }

        // pass in either basedir or s, but not both. 
        // In other words, you can extract to a stream or to a directory, but not both!
        private void Extract(string basedir, Stream s)
        {
            string TargetFile = null;

            if (basedir != null)
            {
                TargetFile = Path.Combine(basedir, this.FileName);

                // check if a directory
                if (this.IsDirectory || this.FileName.EndsWith("/"))
                {
                    if (!Directory.Exists(TargetFile))
                    {
                        Directory.CreateDirectory(TargetFile);
                    }

                    return;
                }
            }
            else if (s != null)
            {
                if (this.IsDirectory || this.FileName.EndsWith("/"))
                    // extract a directory to streamwriter?  nothing to do!
                {
                    return;
                }
            }
            else
            {
                throw new Exception("Invalid input.");
            }

            using (var memstream = new MemoryStream(this._FileData))
            {
                Stream input = null;

                try
                {
                    if (this.CompressedSize == this.UncompressedSize)
                    {
                        // the System.IO.Compression.DeflateStream class does not handle uncompressed data.
                        // so if an entry is not compressed, then we just translate the bytes directly.
                        input = memstream;
                    }
                    else
                    {
                        input = new DeflateStream(memstream, CompressionMode.Decompress);
                    }

                    if (TargetFile != null)
                    {
                        // ensure the target path exists
                        if (!Directory.Exists(Path.GetDirectoryName(TargetFile)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(TargetFile));
                        }
                    }

                    Stream output = null;

                    try
                    {
                        if (TargetFile != null)
                        {
                            if (this.OverwriteOnExtract && File.Exists(TargetFile))
                            {
                                File.Delete(TargetFile);
                            }

                            output = new FileStream(TargetFile, FileMode.CreateNew);
                        }
                        else
                        {
                            output = s;
                        }

                        var bytes = new byte[4096];
                        int n;

                        if (_Debug)
                        {
                            Console.WriteLine("{0}: _FileData.Length= {1}", TargetFile, this._FileData.Length);
                            Console.WriteLine("{0}: memstream.Position: {1}", TargetFile, memstream.Position);
                            n = this._FileData.Length;

                            if (n > 1000)
                            {
                                n = 500;
                                Console.WriteLine("{0}: truncating dump from {1} to {2} bytes...", TargetFile, this._FileData.Length, n);
                            }

                            for (int j = 0; j < n; j += 2)
                            {
                                if (j > 0 && j % 40 == 0)
                                {
                                    Console.WriteLine();
                                }

                                Console.Write(" {0:X2}", this._FileData[j]);

                                if (j + 1 < n)
                                {
                                    Console.Write("{0:X2}", this._FileData[j + 1]);
                                }
                            }

                            Console.WriteLine("\n");
                        }

                        n = 1; // anything non-zero

                        while (n != 0)
                        {
                            if (_Debug)
                            {
                                Console.WriteLine("{0}: about to read...", TargetFile);
                            }

                            n = input.Read(bytes, 0, bytes.Length);

                            if (_Debug)
                            {
                                Console.WriteLine("{0}: got {1} bytes", TargetFile, n);
                            }

                            if (n > 0)
                            {
                                if (_Debug)
                                {
                                    Console.WriteLine("{0}: about to write...", TargetFile);
                                }

                                output.Write(bytes, 0, n);
                            }
                        }
                    }
                    finally
                    {
                        // we only close the output stream if we opened it. 
                        if (output != null && TargetFile != null)
                        {
                            output.Close();
                            output.Dispose();
                        }
                    }

                    if (TargetFile != null)
                    {
                        // We may have to adjust the last modified time to compensate
                        // for differences in how the .NET Base Class Library deals
                        // with daylight saving time (DST) versus how the Windows
                        // filesystem deals with daylight saving time. See 
                        // http://blogs.msdn.com/oldnewthing/archive/2003/10/24/55413.aspx for some context. 

                        // in a nutshell: Daylight savings time rules change regularly.  In
                        // 2007, for example, the inception week of DST changed.  In 1977,
                        // DST was in place all year round. in 1945, likewise.  And so on.
                        // Win32 does not attempt to guess which time zone rules were in
                        // effect at the time in question.  It will render a time as
                        // "standard time" and allow the app to change to DST as necessary.
                        //  .NET makes a different choice.

                        // -------------------------------------------------------
                        // Compare the output of FileInfo.LastWriteTime.ToString("f") with
                        // what you see in the property sheet for a file that was last
                        // written to on the other side of the DST transition. For example,
                        // suppose the file was last modified on October 17, during DST but
                        // DST is not currently in effect. Explorer's file properties
                        // reports Thursday, October 17, 2003, 8:45:38 AM, but .NETs
                        // FileInfo reports Thursday, October 17, 2003, 9:45 AM.

                        // Win32 says, "Thursday, October 17, 2002 8:45:38 AM PST". Note:
                        // Pacific STANDARD Time. Even though October 17 of that year
                        // occurred during Pacific Daylight Time, Win32 displays the time as
                        // standard time because that's what time it is NOW.

                        // .NET BCL assumes that the current DST rules were in place at the
                        // time in question.  So, .NET says, "Well, if the rules in effect
                        // now were also in effect on October 17, 2003, then that would be
                        // daylight time" so it displays "Thursday, October 17, 2003, 9:45
                        // AM PDT" - daylight time.

                        // So .NET gives a value which is more intuitively correct, but is
                        // also potentially incorrect, and which is not invertible. Win32
                        // gives a value which is intuitively incorrect, but is strictly
                        // correct.
                        // -------------------------------------------------------

                        // With this adjustment, I add one hour to the tweaked .NET time, if
                        // necessary.  That is to say, if the time in question had occurred
                        // in what the .NET BCL assumed to be DST (an assumption that may be
                        // wrong given the constantly changing DST rules).

                        if (this.LastModified.IsDaylightSavingTime())
                        {
                            var AdjustedLastModified = this.LastModified + new TimeSpan(1, 0, 0);
                            File.SetLastWriteTime(TargetFile, AdjustedLastModified);
                        }
                        else
                        {
                            File.SetLastWriteTime(TargetFile, this.LastModified);
                        }
                    }
                }
                finally
                {
                    // we only close the output stream if we opened it. 
                    // we cannot use using() here because in some cases we do not want to Dispose the stream!
                    if (input != null && input != memstream)
                    {
                        input.Close();
                        input.Dispose();
                    }
                }
            }
        }

        internal void MarkAsDirectory()
        {
            this.IsDirectory = true;
        }

        internal void WriteCentralDirectoryEntry(Stream s)
        {
            var bytes = new byte[4096];
            int i = 0;
            // signature
            bytes[i++] = ZipConstants.ZipDirEntrySignature & 0x000000FF;
            bytes[i++] = (ZipConstants.ZipDirEntrySignature & 0x0000FF00) >> 8;
            bytes[i++] = (ZipConstants.ZipDirEntrySignature & 0x00FF0000) >> 16;
            bytes[i++] = (byte) ((ZipConstants.ZipDirEntrySignature & 0xFF000000) >> 24);

            // Version Made By
            bytes[i++] = this.Header[4];
            bytes[i++] = this.Header[5];

            // Version Needed, Bitfield, compression method, lastmod,
            // crc, compressed and uncompressed sizes, filename length and extra field length -
            // are all the same as the local file header. So just copy them
            int j = 0;

            for (j = 0; j < 26; j++)
            {
                bytes[i + j] = this.Header[4 + j];
            }

            i += j; // positioned at next available byte

            int commentLength = 0;

            // File (entry) Comment Length
            if (this.Comment == null || this.Comment.Length == 0)
            {
                // no comment!
                bytes[i++] = 0;
                bytes[i++] = 0;
            }
            else
            {
                commentLength = this.Comment.Length;

                // the size of our buffer defines the max length of the comment we can write
                if (commentLength + i > bytes.Length)
                {
                    commentLength = bytes.Length - i;
                }

                bytes[i++] = (byte) (commentLength & 0x00FF);
                bytes[i++] = (byte) ((commentLength & 0xFF00) >> 8);
            }

            // Disk number start
            bytes[i++] = 0;
            bytes[i++] = 0;

            // internal file attrs
            bytes[i++] = (byte) (this.IsDirectory ? 0 : 1);
            bytes[i++] = 0;

            // external file attrs
            bytes[i++] = (byte) (this.IsDirectory ? 0x10 : 0x20);
            bytes[i++] = 0;
            bytes[i++] = 0xb6; // ?? not sure, this might also be zero
            bytes[i++] = 0x81; // ?? ditto

            // relative offset of local header (I think this can be zero)
            bytes[i++] = (byte) (_RelativeOffsetOfHeader & 0x000000FF);
            bytes[i++] = (byte) ((_RelativeOffsetOfHeader & 0x0000FF00) >> 8);
            bytes[i++] = (byte) ((_RelativeOffsetOfHeader & 0x00FF0000) >> 16);
            bytes[i++] = (byte) ((_RelativeOffsetOfHeader & 0xFF000000) >> 24);

            if (_Debug)
            {
                Console.WriteLine("\ninserting filename into CDS: (length= {0})", this.Header.Length - 30);
            }

            // actual filename (starts at offset 34 in header) 
            for (j = 0; j < this.Header.Length - 30; j++)
            {
                bytes[i + j] = this.Header[30 + j];

                if (_Debug)
                {
                    Console.Write(" {0:X2}", bytes[i + j]);
                }
            }

            if (_Debug)
            {
                Console.WriteLine();
            }

            i += j;

            // "Extra field"
            // in this library, it is always nothing

            // file (entry) comment
            if (commentLength != 0)
            {
                var c = this.Comment.ToCharArray();

                // now actually write the comment itself into the byte buffer
                for (j = 0; j < commentLength && i + j < bytes.Length; j++)
                {
                    bytes[i + j] = BitConverter.GetBytes(c[j])[0];
                }

                i += j;
            }

            s.Write(bytes, 0, i);
        }

        private void WriteHeader(Stream s, byte[] bytes)
        {
            // write the header info

            int i = 0;
            // signature
            bytes[i++] = ZipConstants.ZipEntrySignature & 0x000000FF;
            bytes[i++] = (ZipConstants.ZipEntrySignature & 0x0000FF00) >> 8;
            bytes[i++] = (ZipConstants.ZipEntrySignature & 0x00FF0000) >> 16;
            bytes[i++] = (byte) ((ZipConstants.ZipEntrySignature & 0xFF000000) >> 24);

            // version needed
            short FixedVersionNeeded = 0x14; // from examining existing zip files
            bytes[i++] = (byte) (FixedVersionNeeded & 0x00FF);
            bytes[i++] = (byte) ((FixedVersionNeeded & 0xFF00) >> 8);

            // bitfield
            short BitField = 0x00; // from examining existing zip files
            bytes[i++] = (byte) (BitField & 0x00FF);
            bytes[i++] = (byte) ((BitField & 0xFF00) >> 8);

            short CompressionMethod = 0x00; // 0x08 = Deflate, 0x00 == No Compression

            // compression for directories = 0x00 (No Compression)

            if (!this.IsDirectory)
            {
                CompressionMethod = 0x08;

                // CRC32 (Int32)
                if (this._FileData != null)
                {
                    // If we have FileData, that means we've read this entry from an
                    // existing zip archive. We must just copy the existing file data, 
                    // CRC, compressed size, and uncompressed size 
                    // over to the new (updated) archive.  
                }
                else
                {
                    // special case zero-length files
                    var fi = new FileInfo(this.LocalFileName);

                    if (fi.Length == 0)
                    {
                        CompressionMethod = 0x00;
                        this.UncompressedSize = 0;
                        this.CompressedSize = 0;
                        _Crc32 = 0;
                    }
                    else
                    {
                        // Read in the data from the file in the filesystem, compress it, and 
                        // calculate a CRC on it as we read. 

                        var crc32 = new CRC32();

                        using (Stream input = File.OpenRead(this.LocalFileName))
                        {
                            uint crc = crc32.GetCrc32AndCopy(input, this.CompressedStream);
                            _Crc32 = (int) crc;
                        }

                        this.CompressedStream.Close(); // to get the footer bytes written to the underlying stream
                        _CompressedStream = null;

                        this.UncompressedSize = crc32.TotalBytesRead;
                        this.CompressedSize = (int) _UnderlyingMemoryStream.Length;

                        // It is possible that applying this stream compression on a previously compressed
                        // file will actually increase the size of the data.  In that case, we back-off
                        // and just store the uncompressed (really, already compressed) data.
                        // We need to recompute the CRC, and point to the right data.
                        if (this.CompressedSize > this.UncompressedSize)
                        {
                            using (Stream input = File.OpenRead(this.LocalFileName))
                            {
                                _UnderlyingMemoryStream = new MemoryStream();
                                uint crc = crc32.GetCrc32AndCopy(input, _UnderlyingMemoryStream);
                                _Crc32 = (int) crc;
                            }

                            this.UncompressedSize = crc32.TotalBytesRead;
                            this.CompressedSize = (int) _UnderlyingMemoryStream.Length;

                            if (this.CompressedSize != this.UncompressedSize)
                            {
                                throw new Exception("No compression but unequal stream lengths!");
                            }

                            CompressionMethod = 0x00;
                        }
                    }
                }
            }

            // compression method         
            bytes[i++] = (byte) (CompressionMethod & 0x00FF);
            bytes[i++] = (byte) ((CompressionMethod & 0xFF00) >> 8);

            // LastMod
            bytes[i++] = (byte) (_LastModDateTime & 0x000000FF);
            bytes[i++] = (byte) ((_LastModDateTime & 0x0000FF00) >> 8);
            bytes[i++] = (byte) ((_LastModDateTime & 0x00FF0000) >> 16);
            bytes[i++] = (byte) ((_LastModDateTime & 0xFF000000) >> 24);

            // calculated above
            bytes[i++] = (byte) (_Crc32 & 0x000000FF);
            bytes[i++] = (byte) ((_Crc32 & 0x0000FF00) >> 8);
            bytes[i++] = (byte) ((_Crc32 & 0x00FF0000) >> 16);
            bytes[i++] = (byte) ((_Crc32 & 0xFF000000) >> 24);

            // CompressedSize (Int32)
            bytes[i++] = (byte) (this.CompressedSize & 0x000000FF);
            bytes[i++] = (byte) ((this.CompressedSize & 0x0000FF00) >> 8);
            bytes[i++] = (byte) ((this.CompressedSize & 0x00FF0000) >> 16);
            bytes[i++] = (byte) ((this.CompressedSize & 0xFF000000) >> 24);

            // UncompressedSize (Int32)
            if (_Debug)
            {
                Console.WriteLine("Uncompressed Size: {0}", this.UncompressedSize);
            }

            bytes[i++] = (byte) (this.UncompressedSize & 0x000000FF);
            bytes[i++] = (byte) ((this.UncompressedSize & 0x0000FF00) >> 8);
            bytes[i++] = (byte) ((this.UncompressedSize & 0x00FF0000) >> 16);
            bytes[i++] = (byte) ((this.UncompressedSize & 0xFF000000) >> 24);

            // filename length (Int16)
            short filenameLength = (short) this.FileName.Length;

            // see note below about TrimVolumeFromFullyQualifiedPaths.
            if (this.TrimVolumeFromFullyQualifiedPaths && this.FileName[1] == ':' && this.FileName[2] == '\\')
            {
                filenameLength -= 3;
            }

            // apply upper bound to the length
            if (filenameLength + i > bytes.Length)
            {
                filenameLength = (short) (bytes.Length - (short) i);
            }

            bytes[i++] = (byte) (filenameLength & 0x00FF);
            bytes[i++] = (byte) ((filenameLength & 0xFF00) >> 8);

            // extra field length (short)
            short ExtraFieldLength = 0x00;
            bytes[i++] = (byte) (ExtraFieldLength & 0x00FF);
            bytes[i++] = (byte) ((ExtraFieldLength & 0xFF00) >> 8);

            // Tue, 27 Mar 2007  16:35

            // Creating a zip that contains entries with "fully qualified" pathnames
            // can result in a zip archive that is unreadable by Windows Explorer.
            // Such archives are valid according to other tools but not to explorer.
            // To avoid this, we can trim off the leading volume name and slash (eg
            // c:\) when creating (writing) a zip file.  We do this by default and we
            // leave the old behavior available with the
            // TrimVolumeFromFullyQualifiedPaths flag - set it to false to get the old
            // behavior.  It only affects zip creation.

            // Tue, 05 Feb 2008  12:25
            // Replace backslashes with forward slashes in the archive

            // the filename written to the archive
            var c = this.TrimVolumeFromFullyQualifiedPaths && this.FileName[1] == ':' && this.FileName[2] == '\\'
                ? this.FileName.Substring(3).Replace("\\", "/").ToCharArray()
                : // trim off volume letter, colon, and slash
                this.FileName.Replace("\\", "/").ToCharArray();

            int j = 0;

            if (_Debug)
            {
                Console.WriteLine("local header: writing filename, {0} chars", c.Length);
                Console.WriteLine("starting offset={0}", i);
            }

            for (j = 0; j < c.Length && i + j < bytes.Length; j++)
            {
                bytes[i + j] = BitConverter.GetBytes(c[j])[0];

                if (_Debug)
                {
                    Console.Write(" {0:X2}", bytes[i + j]);
                }
            }

            if (_Debug)
            {
                Console.WriteLine();
            }

            i += j;

            // extra field (we always write nothing in this implementation)
            // ;;

            // remember the file offset of this header
            _RelativeOffsetOfHeader = (int) s.Length;

            if (_Debug)
            {
                Console.WriteLine("\nAll header data:");

                for (j = 0; j < i; j++)
                {
                    Console.Write(" {0:X2}", bytes[j]);
                }

                Console.WriteLine();
            }

            // finally, write the header to the stream
            s.Write(bytes, 0, i);

            // preserve this header data for use with the central directory structure.
            this.Header = new byte[i];

            if (_Debug)
            {
                Console.WriteLine("preserving header of {0} bytes", this.Header.Length);
            }

            for (j = 0; j < i; j++)
            {
                this.Header[j] = bytes[j];
            }
        }

        internal void Write(Stream s)
        {
            var bytes = new byte[4096];
            int n;

            // write the header:
            this.WriteHeader(s, bytes);

            if (this.IsDirectory)
            {
                return; // nothing more to do! (need to close memory stream?)
            }


            if (_Debug)
            {
                Console.WriteLine("{0}: writing compressed data to zipfile...", this.FileName);
                Console.WriteLine("{0}: total data length: {1}", this.FileName, this.CompressedSize);
            }

            if (this.CompressedSize == 0)
            {
                // nothing more to write. 
                //(need to close memory stream?)
                if (_UnderlyingMemoryStream != null)
                {
                    _UnderlyingMemoryStream.Close();
                    _UnderlyingMemoryStream = null;
                }

                return;
            }

            // write the actual file data: 
            if (this._FileData != null)
            {
                // use the existing compressed data we read from the extant zip archive
                s.Write(this._FileData, 0, this._FileData.Length);
            }
            else
            {
                // rely on the compressed data we created in WriteHeader
                _UnderlyingMemoryStream.Position = 0;

                while ((n = _UnderlyingMemoryStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    if (_Debug)
                    {
                        Console.WriteLine("{0}: transferring {1} bytes...", this.FileName, n);

                        for (int j = 0; j < n; j += 2)
                        {
                            if (j > 0 && j % 40 == 0)
                            {
                                Console.WriteLine();
                            }

                            Console.Write(" {0:X2}", bytes[j]);

                            if (j + 1 < n)
                            {
                                Console.Write("{0:X2}", bytes[j + 1]);
                            }
                        }

                        Console.WriteLine("\n");
                    }

                    s.Write(bytes, 0, n);
                }

                //_CompressedStream.Close();
                //_CompressedStream= null;
                _UnderlyingMemoryStream.Close();
                _UnderlyingMemoryStream = null;
            }
        }
    }
}