using System;

namespace Argus.Diagnostics
{
    /// <summary>
    /// Model class for sharing workstation/server related resource information.
    /// </summary>
    public class ResourceInfo
    {
        public string MachineName { get; set; }
        public string OperatingSystem { get; set; }
        public string Platform { get; set; }
        public bool Is64Bit { get; set; }
        public int ProcessorCount { get; set; }        
        public DateTime ServerTime { get; set; }
        public MemoryInfo MemoryInfo { get; set; }
    }
}
