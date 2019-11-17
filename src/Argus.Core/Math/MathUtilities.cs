namespace Argus.Math
{
    /// <summary>
    ///     Various math related utilities.
    /// </summary>
    public static class MathUtilities
    {
        //*********************************************************************************************************************
        //
        //             Class:  MathUtilities
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  09/26/2019
        //      Last Updated:  09/26/2019
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Returns the value if it falls in the range of the max and min.  Otherwise it returns
        ///     the upper or lower boundary depending on which one the value has crossed.
        /// </summary>
        public static int Clamp(int value, int min, int max)
        {
            if (value > max)
            {
                return max;
            }

            if (value < min)
            {
                return min;
            }

            return value;
        }
    }
}