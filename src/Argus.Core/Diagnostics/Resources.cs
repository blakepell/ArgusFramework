using System;
using System.Runtime.InteropServices;

namespace Argus.Diagnostics
{
    /// <summary>
    /// Information about current system resources.
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// Information about current system resource info.
        /// </summary>
        public static ResourceInfo CurrentResourceInfo()
        {
            var ri = new ResourceInfo();
            ri.MachineName = Environment.MachineName;
            ri.Is64Bit = Environment.Is64BitOperatingSystem;
            ri.Platform = Environment.OSVersion.VersionString;
            ri.ProcessorCount = Environment.ProcessorCount;
            ri.ServerTime = DateTime.Now;
            ri.MemoryInfo = Memory.CurrentSystemMemory();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ri.OperatingSystem = "Windows";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                ri.OperatingSystem = "Linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                ri.OperatingSystem = "OSX";
            }
#if NETCOREAPP3_0
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                ri.OperatingSystem = "FreeBSD";
            }
#endif
            else
            {
                ri.OperatingSystem = "Unknown";
            }

            return ri;
        }
    }
}
