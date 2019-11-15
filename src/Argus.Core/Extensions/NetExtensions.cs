using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Argus.Extensions
{

    /// <summary>
    /// Extensions to items Network related.
    /// </summary>
    public static class NetExtensions
    {

        //*********************************************************************************************************************
        //
        //            Module:  NetExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  09/29/2011
        //      Last Updated:  09/29/2011
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Returns the filename from the local path of the System.Uri.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static object FileName(this System.Uri uri)
        {
            return System.IO.Path.GetFileName(uri.LocalPath);
        }

    }

}