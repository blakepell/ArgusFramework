/*
 * @author            : Blake Pell
 * @initial date      : 2019-09-26
 * @last updated      : 2019-09-26
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

namespace Argus.Math
{
    /// <summary>
    /// Various math related utilities.
    /// </summary>
    public static class MathUtilities
    {
        /// <summary>
        /// Returns the value if it falls in the range of the max and min.  Otherwise it returns
        /// the upper or lower boundary depending on which one the value has crossed.
        /// </summary>
        public static int Clamp(int value, int min, int max)
        {
            if (value > max)
            {
                return max;
            }

            return value < min ? min : value;
        }
    }
}