﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Argus.Diagnostics
{
    /// <summary>
    /// Memory related diagnostic utilities.
    /// </summary>
    public static class Memory
    {
        /// <summary>
        /// Returns memory information about the current OS and process.
        /// </summary>
        /// <returns>
        /// This should work on both Windows and Linux to obtain bits of overhead data about memory
        /// consumption.  On Windows the memory information is obtained through the WinAPI and as a
        /// result likely won't be able to be called from applications that are heavily sandboxed like
        /// UWP apps (Windows Universal Apps).  I have been unable to find documentation on how to
        /// find the associated memory metrics for UWP.
        /// </returns>
        public static MemoryInfo CurrentSystemMemory()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return WindowsMemoryInfo();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return LinuxMemoryInfo();
            }
            else
            {
                throw new PlatformNotSupportedException("Currently memory state is only supported on Windows and Linux.");
            }
        }

        /// <summary>
        /// Returns the current memory information.  Framework objects are used where they exist.  The Windows API will
        /// where they do not exist.
        /// </summary>
        /// <returns></returns>
        private static MemoryInfo WindowsMemoryInfo()
        {
            var mi = new MemoryInfo();
            mi.ProcessUsed = Process.GetCurrentProcess().WorkingSet64 / 1024;

            var mem = new MEMORYSTATUSEX();
            mem.Init();

            if (!GlobalMemoryStatusEx(ref mem))
            {
                throw new Win32Exception("Could not obtain memory information due to internal error.");
            }

            mi.SystemTotal = mem.ullTotalPhys;
            mi.SystemFree = mem.ullAvailPhys;
            mi.SystemAvailable = mem.ullAvailPhys;
            mi.SystemUsed = mi.SystemTotal - mi.SystemFree;
            mi.VirtualTotal = mem.ullTotalVirtual;
            mi.VirtualAvailable = mem.ullAvailVirtual;
            mi.PageFileTotal = mem.ullTotalPageFile;
            mi.PageFileAvailable = mem.ullAvailPageFile;
            mi.MemoryLoad = mem.dwMemoryLoad;
            mi.ExtendedVirtualMemoryAvailable = mem.ullAvailExtendedVirtual;

            return mi;
        }

        /// <summary>
        /// Returns the current memory information as parsed from /proc/meminfo.
        /// </summary>
        /// <returns></returns>
        private static MemoryInfo LinuxMemoryInfo()
        {
            var mi = new MemoryInfo();

            // Read in all of the lines from the meminfo file.
            var lines = System.IO.File.ReadAllLines("/proc/meminfo");

            // Go through each of them, clean them up and keep the ones we want.
            foreach (string line in lines)
            {
                // Cleanup our values.  The first item here will be the key, the second item will
                // by the value.
                var items = line.Split(':');
                items[1] = items[1].Replace(" ", "").Replace("kB", "");

                if (items[0] == "MemTotal")
                {
                    mi.SystemTotal = long.Parse(items[1]);
                }
                else if (items[0] == "MemFree")
                {
                    mi.SystemFree = long.Parse(items[1]);
                }
                else if (items[0] == "MemAvailable")
                {
                    mi.SystemAvailable = long.Parse(items[1]);
                }

            }

            mi.SystemUsed = mi.SystemTotal - mi.SystemFree;
            mi.ProcessUsed = Process.GetCurrentProcess().WorkingSet64 / 1024;
            
            return mi;
        }

        /// <summary>
        /// Win32 API call for GlobalMemoryStatusEx
        /// </summary>
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

        /// <summary>
        /// Structure to hold data for the memory status returned from the Windows WPI.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct MEMORYSTATUSEX
        {
            internal int dwLength;
            internal int dwMemoryLoad;
            internal long ullTotalPhys;
            internal long ullAvailPhys;
            internal long ullTotalPageFile;
            internal long ullAvailPageFile;
            internal long ullTotalVirtual;
            internal long ullAvailVirtual;
            internal long ullAvailExtendedVirtual;
            internal void Init()
            {
                dwLength = Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

    }
}