/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2011-09-29
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.IO;

namespace Argus.Extensions
{
    /// <summary>
    /// Extensions to items Network related.
    /// </summary>
    public static class NetExtensions
    {
        /// <summary>
        /// Returns the filename from the local path of the System.Uri.
        /// </summary>
        /// <param name="uri"></param>
        public static object FileName(this Uri uri)
        {
            return Path.GetFileName(uri.LocalPath);
        }
    }
}