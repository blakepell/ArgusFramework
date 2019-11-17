﻿using System.Collections.Generic;
using Argus.Data;
using Argus.Math;

namespace Argus.Extensions
{
    /// <summary>
    ///     Extensions for numeric data types, Integer (Int32), Int16, Long (Int64), Double
    /// </summary>
    public static class NumericExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  NumericExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  07/03/2010
        //      Last Updated:  09/26/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     The order a list should be sorted in, either ascending or descending.
        /// </summary>
        public enum SortOrder
        {
            /// <summary>
            /// Ascending order from lowest to highest.
            /// </summary>
            Ascending,
            /// <summary>
            /// Descending order from highest to lowest.
            /// </summary>
            Descending
        }

        /// <summary>
        ///     Whether the number is even or not.
        /// </summary>
        /// <param name="num"></param>
        public static bool IsEven(this int num)
        {
            return num % 2 == 0;
        }

        /// <summary>
        ///     Whether the number is even or not.  The Long data type is the same as Int64.
        /// </summary>
        /// <param name="num"></param>
        public static bool IsEven(this long num)
        {
            return num % 2 == 0;
        }

        /// <summary>
        ///     Whether the number is odd or not.
        /// </summary>
        /// <param name="num"></param>
        public static bool IsOdd(this int num)
        {
            return num % 2 != 0;
        }

        /// <summary>
        ///     Whether the number is odd or not.
        /// </summary>
        /// <param name="num"></param>
        public static bool IsOdd(this long num)
        {
            return num % 2 != 0;
        }

        /// <summary>
        ///     Determines if the Integer is of the specified interval (a factor).  E.g. if the interval is 100 and the integer is 400, it
        ///     would return true, it would not for 401.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="interval"></param>
        public static bool IsInterval(this int num, int interval)
        {
            return num % interval == 0;
        }

        /// <summary>
        ///     Sorts the list of integers  either ascending or descending order.
        /// </summary>
        /// <param name="ls"></param>
        public static void Sort(this List<int> ls, SortOrder so)
        {
            if (so == SortOrder.Ascending)
            {
                ls.Sort((p1, p2) => p1.CompareTo(p2));
            }
            else
            {
                ls.Sort((p1, p2) => p2.CompareTo(p1));
            }
        }

        /// <summary>
        ///     Whether or not a number is prime or not.
        /// </summary>
        /// <param name="x"></param>
        public static bool IsPrime(this int x)
        {
            if (x == 1)
            {
                return false;
            }

            if (x % 2 == 0)
            {
                return x == 2;
            }

            double max = System.Math.Sqrt(x);

            for (double i = 3; i <= max; i += 2)
            {
                if (x % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Returns the english representation of the integer.
        /// </summary>
        /// <param name="num"></param>
        public static string ToEnglish(this int num)
        {
            return NumberToEnglish.Convert(num);
        }

        /// <summary>
        ///     Returns the value if it falls in the range of the max and min.  Otherwise it returns
        ///     the upper or lower boundary depending on which one the value has crossed.
        /// </summary>
        public static int Clamp(this int value, int min, int max)
        {
            return MathUtilities.Clamp(value, min, max);
        }
    }
}