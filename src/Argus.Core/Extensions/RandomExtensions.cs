using System;

namespace Argus.Extensions
{
    /// <summary>
    /// Extensions to the random number generator.
    /// </summary>
    public static class RandomExtensions
    {
        //*********************************************************************************************************************
        //
        //             Class:  RandomExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  N/A
        //      Last Updated:  05/26/2016
        //     Programmer(s):  Dave DeMeulenaere <dmdemeul@iu.edu> 
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Returns the next Int32.
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static int NextInt32(this Random rng)
        {
            var firstBits = rng.Next(0, 1 << 4) << 28;
            var lastBits = rng.Next(0, 1 << 28);
            return firstBits | lastBits;
        }

        /// <summary>
        /// Returns the next decimal.
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static decimal NextDecimal(this Random rng)
        {
            var scale = (byte)rng.Next(29);
            var sign = rng.Next(2) == 1;
            return new decimal(rng.NextInt32(),
                rng.NextInt32(),
                rng.NextInt32(),
                sign,
                scale);
        }
    }
}
