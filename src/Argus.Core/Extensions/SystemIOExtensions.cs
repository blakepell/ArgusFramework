using System.IO;
using Argus.Data;

namespace Argus.Extensions
{
    /// <summary>
    ///     Extensions for methods in classes of the System.IO namespace.
    /// </summary>
    public static class SystemIOExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  SystemIOExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  04/06/2009
        //      Last Updated:  08/09/2017
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Returns the file size formatted, such as 10 KB, 120 MB, 1.2 GB, 1.4 TB, etc.  This supports sizes up to and including Terabytes.
        /// </summary>
        /// <param name="fi"></param>
        public static string FormattedFileSize(this FileInfo fi)
        {
            return Formatting.FormattedFileSize(fi.Length);
        }
    }
}