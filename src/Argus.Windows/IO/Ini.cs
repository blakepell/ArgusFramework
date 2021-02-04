/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2003-12-03
 * @last updated      : 2019-06-04
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System.Runtime.InteropServices;
using System.Text;

namespace Argus.IO
{
    /// <summary>
    /// Class to handle Windows style INI files.  INI files must be in ASCII/ANSI encoding otherwise the Windows API
    /// calls will return blank strings.  In other words, UTF encodings will not work.
    /// </summary>
    /// <remarks>
    /// This makes calls to the Windows API for parsing of the INI files.
    /// </remarks>
    public class Ini
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iniFileLocation"></param>
        public Ini(string iniFileLocation)
        {
            this.IniFileLocation = iniFileLocation;
        }

        /// <summary>
        /// Location of the INI file.
        /// </summary>
        public string IniFileLocation { get; set; } = "";

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32", EntryPoint = "WritePrivateProfileStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);

        /// <summary>
        /// Function to read from a specified INI file
        /// </summary>
        /// <param name="section"></param>
        /// <param name="entry"></param>
        public string GetFromIni(string section, string entry)
        {
            // The memory must be allocated in the StringBuilder for the API to write into it
            var sb = new StringBuilder(1024);

            GetPrivateProfileString(section, entry, "", sb, 1024, this.IniFileLocation);

            return sb.ToString();
        }

        /// <summary>
        /// Function to write to a specified INI file
        /// </summary>
        /// <param name="section"></param>
        /// <param name="entry"></param>
        /// <param name="buf"></param>
        public bool WriteToIni(string section, string entry, string buf)
        {
            int returnValue = WritePrivateProfileString(section, entry, buf, this.IniFileLocation);

            if (returnValue != 0)
            {
                return true;
            }

            return false;
        }
    }
}