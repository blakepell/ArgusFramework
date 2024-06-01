/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2020-09-28
 * @last updated      : 2020-09-28
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
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
        /// Returns a true only if the value is true.  Null is considered false.
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
        /// Returns a true if the value is False or null.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsFalse(this bool? value)
        {
            if (!value.GetValueOrDefault(false))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a bool, true if true and false if false or null.
        /// </summary>
        /// <param name="value"></param>
        public static bool ToBool(this bool? value)
        {
            return value.GetValueOrDefault(false);
        }

    }
}