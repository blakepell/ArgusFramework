using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DirectoryInfoExtensions"/>
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        //*********************************************************************************************************************
        //
        //             Class:  DirectoryInfoExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  04/06/2009
        //      Last Updated:  08/09/2017
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Returns the owner of the directory.
        /// </summary>
        /// <param name="di"></param>
        public static string GetOwner(this DirectoryInfo di)
        {
            if (di == null)
            {
                return "";
            }

            var ds = di.GetAccessControl();            
            var ir = ds.GetOwner(typeof(NTAccount));
            
            return ir.Value;
        }

    }
}
