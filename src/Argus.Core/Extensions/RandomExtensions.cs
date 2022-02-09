/*
 * @author            : Dave DeMeulenaere
 * @last updated      : 2016-05-26
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Extensions
{
    /// <summary>
    /// Extensions to the random number generator.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns the next Int32.
        /// </summary>
        /// <param name="rng"></param>
        public static int NextInt32(this Random rng)
        {
            int firstBits = rng.Next(0, 1 << 4) << 28;
            int lastBits = rng.Next(0, 1 << 28);

            return firstBits | lastBits;
        }

        /// <summary>
        /// Returns the next decimal.
        /// </summary>
        /// <param name="rng"></param>
        public static decimal NextDecimal(this Random rng)
        {
            byte scale = (byte) rng.Next(29);
            bool sign = rng.Next(2) == 1;

            return new decimal(rng.NextInt32(),
                               rng.NextInt32(),
                               rng.NextInt32(),
                               sign,
                               scale);
        }
    }
}