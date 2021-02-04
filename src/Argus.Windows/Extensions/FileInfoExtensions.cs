/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-04-06
 * @last updated      : 2019-11-18
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System.IO;
using System.Security.Principal;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="FileInfoExtensions" />
    /// </summary>
    public static class FileInfoExtensions
    {
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