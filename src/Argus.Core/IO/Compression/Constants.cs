/*
 * @author            : Microsoft
 * @copyright         : Copyright (c) 2006-2008, All rights reserved.
 * @license           : Microsoft Public License
 */

namespace Argus.IO.Compression
{
    internal class ZipConstants
    {
        public const uint EndOfCentralDirectorySignature = 0x06054b50;
        public const int ZipEntrySignature = 0x04034b50;
        public const int ZipEntryDataDescriptorSignature = 0x08074b50;
        public const int ZipDirEntrySignature = 0x02014b50;
    }
}