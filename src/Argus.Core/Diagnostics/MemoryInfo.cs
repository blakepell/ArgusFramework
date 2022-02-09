/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 */

namespace Argus.Diagnostics
{
    /// <summary>
    /// Model class for sharing memory related data.  Not all properties are supported on all
    /// operating systems.
    /// </summary>
    public class MemoryInfo
    {
        /// <summary>
        /// The total amount of system memory available in KB.
        /// </summary>
        public long SystemTotal { get; set; }

        /// <summary>
        /// The amount of system memory that is used in KB.
        /// </summary>
        public long SystemUsed { get; set; }

        /// <summary>
        /// An estimate of how much system memory in KB is available for use before swapping occurs.
        /// </summary>
        public long SystemAvailable { get; set; }

        /// <summary>
        /// The amount of physical memory in KB that is unused by the system.
        /// </summary>
        public long SystemFree { get; set; }

        /// <summary>
        /// How much memory is used by the current process in KB.  This value is derived from the processes
        /// WorkingSet64 value.  It is the amount of memory that is allocated to the process.
        /// </summary>
        public long ProcessUsed { get; set; }

        /// <summary>
        /// Total amount of virtual memory (Windows only).
        /// </summary>
        public long VirtualTotal { get; set; }

        /// <summary>
        /// Virtual memory that's available (Windows only).
        /// </summary>
        public long VirtualAvailable { get; set; }

        /// <summary>
        /// Total size of the page file (Windows only).
        /// </summary>
        public long PageFileTotal { get; set; }

        /// <summary>
        /// Available memory for the page file (Windows only).
        /// </summary>
        public long PageFileAvailable { get; set; }

        /// <summary>
        /// Percent of memory in use (Windows only).  This is drawn from the WinApi but is also able
        /// to be calculated by (SystemUsed / SystemTotal).  (Windows only).
        /// </summary>
        public long MemoryLoad { get; set; }

        /// <summary>
        /// Extended virtual memory that is available.
        /// </summary>
        public long ExtendedVirtualMemoryAvailable { get; set; }
    }
}