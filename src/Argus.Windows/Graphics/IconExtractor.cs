/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Argus.Graphics
{
    /// <summary>
    /// Class to extract all icons from executable or DLLs.
    /// </summary>
    public class IconExtractor : IDisposable
    {
        #region Win32 interop.

        #region Unmanaged Types

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Auto)]
        private delegate bool EnumResNameProc(IntPtr hModule, int lpszType, IntPtr lpszName, IconResInfo lParam);

        #endregion

        #region Consts.

        private const int LOAD_LIBRARY_AS_DATAFILE = 0x00000002;
        private const int RT_ICON = 3;
        private const int RT_GROUP_ICON = 14;
        private const int MAX_PATH = 260;
        private const int ERROR_SUCCESS = 0;
        private const int ERROR_FILE_NOT_FOUND = 2;
        private const int ERROR_BAD_EXE_FORMAT = 193;
        private const int ERROR_RESOURCE_TYPE_NOT_FOUND = 1813;
        private const int sICONDIR = 6; // sizeof(ICONDIR) 
        private const int sICONDIRENTRY = 16; // sizeof(ICONDIRENTRY)
        private const int sGRPICONDIRENTRY = 14; // sizeof(GRPICONDIRENTRY)

        #endregion

        #region API Functions

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, int dwFlags);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool EnumResourceNames(
            IntPtr hModule, int lpszType, EnumResNameProc lpEnumFunc, IconResInfo lParam);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, int lpType);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr LockResource(IntPtr hResData);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern int SizeofResource(IntPtr hModule, IntPtr hResInfo);

        #endregion

        #endregion

        #region Managed Types

        private class IconResInfo
        {
            public readonly List<ResourceName> IconNames = new List<ResourceName>();
        }

        private class ResourceName
        {
            private IntPtr _bufPtr = IntPtr.Zero;

            public ResourceName(IntPtr lpName)
            {
                if ((uint) lpName >> 16 == 0)
                {
                    this.Id = lpName;
                    this.Name = null;
                }
                else
                {
                    this.Id = IntPtr.Zero;
                    this.Name = Marshal.PtrToStringAuto(lpName);
                }
            }

            public IntPtr Id { get; }
            public string Name { get; }

            public IntPtr GetValue()
            {
                if (this.Name == null)
                {
                    return this.Id;
                }

                _bufPtr = Marshal.StringToHGlobalAuto(this.Name);

                return _bufPtr;
            }

            public void Free()
            {
                if (_bufPtr != IntPtr.Zero)
                {
                    try
                    {
                        Marshal.FreeHGlobal(_bufPtr);
                    }
                    catch
                    {
                    }

                    _bufPtr = IntPtr.Zero;
                }
            }
        }

        #endregion

        #region Private Fields

        private IntPtr _hModule = IntPtr.Zero;
        private readonly IconResInfo _resInfo;
        private Icon[] _iconCache;

        #endregion

        #region Public Properties

        // Full path 
        public string Filename { get; }
        public int IconCount => _resInfo.IconNames.Count;

        #endregion

        #region Contructor/Destructor and relatives

        /// <summary>
        /// Load the specified executable file or DLL, and get ready to extract the icons.
        /// </summary>
        /// <param name="filename">The name of a file from which icons will be extracted.</param>
        public IconExtractor(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            _hModule = LoadLibrary(filename);

            if (_hModule == IntPtr.Zero)
            {
                _hModule = LoadLibraryEx(filename, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);

                if (_hModule == IntPtr.Zero)
                {
                    switch (Marshal.GetLastWin32Error())
                    {
                        case ERROR_FILE_NOT_FOUND:
                            throw new FileNotFoundException("Specified file '" + filename + "' not found.");

                        case ERROR_BAD_EXE_FORMAT:
                            throw new ArgumentException("Specified file '" + filename + "' is not an executable file or DLL.");

                        default:
                            throw new Win32Exception();
                    }
                }
            }

            var buf = new StringBuilder(MAX_PATH);
            int len = GetModuleFileName(_hModule, buf, buf.Capacity + 1);

            if (len != 0)
            {
                this.Filename = buf.ToString();
            }
            else
            {
                switch (Marshal.GetLastWin32Error())
                {
                    case ERROR_SUCCESS:
                        this.Filename = filename;

                        break;

                    default:
                        throw new Win32Exception();
                }
            }

            _resInfo = new IconResInfo();
            bool success = EnumResourceNames(_hModule, RT_GROUP_ICON, this.EnumResNameCallBack, _resInfo);

            if (!success)
            {
                throw new Win32Exception();
            }

            _iconCache = new Icon[this.IconCount];
        }

        ~IconExtractor()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (_hModule != IntPtr.Zero)
            {
                try
                {
                    FreeLibrary(_hModule);
                }
                catch
                {
                }

                _hModule = IntPtr.Zero;
            }

            if (_iconCache != null)
            {
                foreach (var i in _iconCache)
                {
                    if (i != null)
                    {
                        try
                        {
                            i.Dispose();
                        }
                        catch
                        {
                        }
                    }
                }

                _iconCache = null;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Extract an icon from the loaded executable file or DLL.
        /// </summary>
        /// <param name="iconIndex">The zero-based index of the icon to be extracted.</param>
        /// <returns>A System.Drawing.Icon object which may consists of multiple icons.</returns>
        /// <remarks>Always returns new copy of the Icon. It should be disposed by the user.</remarks>
        public Icon GetIcon(int iconIndex)
        {
            if (_hModule == IntPtr.Zero)
            {
                throw new ObjectDisposedException("IconExtractor");
            }

            if (iconIndex < 0 || this.IconCount <= iconIndex)
            {
                throw new ArgumentException(
                    "iconIndex is out of range. It should be between 0 and " + (this.IconCount - 1) + ".");
            }

            if (_iconCache[iconIndex] == null)
            {
                _iconCache[iconIndex] = this.CreateIcon(iconIndex);
            }

            return (Icon) _iconCache[iconIndex].Clone();
        }

        /// <summary>
        /// Split an Icon consists of multiple icons into an array of Icon each consist of single icons.
        /// </summary>
        /// <param name="icon">The System.Drawing.Icon to be split.</param>
        /// <returns>An array of System.Drawing.Icon each consist of single icons.</returns>
        public static Icon[] SplitIcon(Icon icon)
        {
            if (icon == null)
            {
                throw new ArgumentNullException("icon");
            }

            // Get multiple .ico file image.
            byte[] srcBuf = null;

            using (var stream = new MemoryStream())
            {
                icon.Save(stream);
                srcBuf = stream.ToArray();
            }

            var splitIcons = new List<Icon>();

            {
                int count = BitConverter.ToInt16(srcBuf, 4); // ICONDIR.idCount

                for (int i = 0; i < count; i++)
                {
                    using (var destStream = new MemoryStream())
                    {
                        using (var writer = new BinaryWriter(destStream))
                        {
                            // Copy ICONDIR and ICONDIRENTRY.
                            writer.Write(srcBuf, 0, sICONDIR - 2);
                            writer.Write((short) 1); // ICONDIR.idCount == 1;

                            writer.Write(srcBuf, sICONDIR + sICONDIRENTRY * i, sICONDIRENTRY - 4);
                            writer.Write(sICONDIR + sICONDIRENTRY); // ICONDIRENTRY.dwImageOffset = sizeof(ICONDIR) + sizeof(ICONDIRENTRY)

                            // Copy picture and mask data.
                            int imgSize = BitConverter.ToInt32(srcBuf, sICONDIR + sICONDIRENTRY * i + 8); // ICONDIRENTRY.dwBytesInRes
                            int imgOffset = BitConverter.ToInt32(srcBuf, sICONDIR + sICONDIRENTRY * i + 12); // ICONDIRENTRY.dwImageOffset
                            writer.Write(srcBuf, imgOffset, imgSize);

                            // Create new icon.
                            destStream.Seek(0, SeekOrigin.Begin);
                            splitIcons.Add(new Icon(destStream));
                        }
                    }
                }
            }

            return splitIcons.ToArray();
        }

        public override string ToString()
        {
            string text = string.Format("IconExtractor (Filename: '{0}', IconCount: {1})", this.Filename, this.IconCount);

            return text;
        }

        #endregion

        #region Private Methods

        private bool EnumResNameCallBack(IntPtr hModule, int lpszType, IntPtr lpszName, IconResInfo lParam)
        {
            // Callback function for EnumResourceNames().

            if (lpszType == RT_GROUP_ICON)
            {
                lParam.IconNames.Add(new ResourceName(lpszName));
            }

            return true;
        }

        private Icon CreateIcon(int iconIndex)
        {
            // Get group icon resource.
            var srcBuf = this.GetResourceData(_hModule, _resInfo.IconNames[iconIndex], RT_GROUP_ICON);

            // Convert the resouce into an .ico file image.
            using (var destStream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(destStream))
                {
                    int count = BitConverter.ToUInt16(srcBuf, 4); // ICONDIR.idCount
                    int imgOffset = sICONDIR + sICONDIRENTRY * count;

                    // Copy ICONDIR.
                    writer.Write(srcBuf, 0, sICONDIR);

                    for (int i = 0; i < count; i++)
                    {
                        // Copy GRPICONDIRENTRY converting into ICONDIRENTRY.
                        writer.BaseStream.Seek(sICONDIR + sICONDIRENTRY * i, SeekOrigin.Begin);
                        writer.Write(srcBuf, sICONDIR + sGRPICONDIRENTRY * i, sICONDIRENTRY - 4); // Common fields of structures
                        writer.Write(imgOffset); // ICONDIRENTRY.dwImageOffset

                        // Get picture and mask data, then copy them.
                        var nID = (IntPtr) BitConverter.ToUInt16(srcBuf, sICONDIR + sGRPICONDIRENTRY * i + 12); // GRPICONDIRENTRY.nID
                        var imgBuf = this.GetResourceData(_hModule, nID, RT_ICON);

                        writer.BaseStream.Seek(imgOffset, SeekOrigin.Begin);
                        writer.Write(imgBuf, 0, imgBuf.Length);

                        imgOffset += imgBuf.Length;
                    }

                    destStream.Seek(0, SeekOrigin.Begin);

                    return new Icon(destStream);
                }
            }
        }

        private byte[] GetResourceData(IntPtr hModule, IntPtr lpName, int lpType)
        {
            // Get binary image of the specified resource.

            var hResInfo = FindResource(hModule, lpName, lpType);

            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            var hResData = LoadResource(hModule, hResInfo);

            if (hResData == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            var hGlobal = LockResource(hResData);

            if (hGlobal == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            int resSize = SizeofResource(hModule, hResInfo);

            if (resSize == 0)
            {
                throw new Win32Exception();
            }

            var buf = new byte[resSize];
            Marshal.Copy(hGlobal, buf, 0, buf.Length);

            return buf;
        }

        private byte[] GetResourceData(IntPtr hModule, ResourceName name, int lpType)
        {
            try
            {
                var lpName = name.GetValue();

                return this.GetResourceData(hModule, lpName, lpType);
            }
            finally
            {
                name.Free();
            }
        }

        #endregion
    }
}