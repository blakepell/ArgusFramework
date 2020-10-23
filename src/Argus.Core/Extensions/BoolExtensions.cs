namespace Argus.Extensions
{
    /// <summary>
    ///     bool and nullable bool extension methods.
    /// </summary>
    public static class BoolExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  BoolExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  09/28/2020
        //      Last Updated:  09/28/2020
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

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