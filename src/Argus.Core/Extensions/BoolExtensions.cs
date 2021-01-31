/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2020-09-28
 * @last updated      : 2020-09-28
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Extensions
{
    /// <summary>
    /// bool and nullable bool extension methods.
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// Returns a True only if the value is true.  Null is considered false.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsTrue(this bool? value)
        {
            if (!value.GetValueOrDefault(false))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a False if the value is False or null.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsFalse(this bool? value)
        {
            if (value.GetValueOrDefault(false))
            {
                return true;
            }

            return false;
        }
    }
}