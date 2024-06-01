/*
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT 
 */

namespace Argus.Diagnostics
{
    /// <summary>
    /// Model class for sharing workstation/server related resource information.
    /// </summary>
    public class ResourceInfo
    {
        /// <summary>
        /// The name of the workstation/computer/server.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// The string representation of the current Operating System.
        /// </summary>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// The string representation of the current Platform.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Whether the executing machine is a 64-bit machine.
        /// </summary>
        public bool Is64Bit { get; set; }

        /// <summary>
        /// The amount of processors on the current machine.
        /// </summary>
        public int ProcessorCount { get; set; }

        /// <summary>
        /// The current server time.
        /// </summary>
        public DateTime ServerTime { get; set; }

        /// <summary>
        /// The current memory information of the executing machine.
        /// </summary>
        public MemoryInfo MemoryInfo { get; set; }
    }
}