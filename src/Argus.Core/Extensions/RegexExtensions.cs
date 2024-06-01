/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-03-09
 * @last updated      : 2021-03-09
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="System.Text.RegularExpressions" /> namespace.
    /// </summary>
    public static class RegexExtensions
    {
        /// <summary>
        /// Returns a true or false with a null value returned as a false.
        /// </summary>
        /// <param name="match"></param>
        public static bool TrySuccess(this Match match)
        {
            return match?.Success ?? false;
        }
    }
}
