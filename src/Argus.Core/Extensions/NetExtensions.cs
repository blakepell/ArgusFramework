using System;
using System.IO;

namespace Argus.Extensions
{
    /// <summary>
    ///     Extensions to items Network related.
    /// </summary>
    public static class NetExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  NetExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  09/29/2011
        //      Last Updated:  11/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Returns the filename from the local path of the System.Uri.
        /// </summary>
        /// <param name="uri"></param>
        public static object FileName(this Uri uri)
        {
            return Path.GetFileName(uri.LocalPath);
        }
    }
}