/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-04-06
 * @last updated      : 2017-08-09
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.IO;
using System.Security.Principal;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DirectoryInfoExtensions" />
    /// </summary>
    public static class DirectoryInfoExtensions
    {
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