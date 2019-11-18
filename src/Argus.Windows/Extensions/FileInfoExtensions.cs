using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="FileInfoExtensions"/>
    /// </summary>
    public static class FileInfoExtensions
    {
        //*********************************************************************************************************************
        //
        //             Class:  FileInfoExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  04/06/2009
        //      Last Updated:  11/18/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Returns the owner of the file.
        /// </summary>
        /// <param name="fi"></param>
        public static string GetOwner(this FileInfo fi)
        {
            if (fi == null)
            {
                return "";
            }

            var fs = fi.GetAccessControl();
            var ir = fs.GetOwner(typeof(NTAccount));

            return ir.Value;
        }

    }
}
