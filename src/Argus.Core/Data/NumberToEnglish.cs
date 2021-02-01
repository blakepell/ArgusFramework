/*
 * @initial date      : 2005-06-12
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 */

namespace Argus.Data
{
    /// <summary>
    /// Class with shared (static) methods to convert a number to it's english representation.
    /// </summary>
    public static class NumberToEnglish
    {
        /// <summary>
        /// Shared/static method to convert a number to it's english representation.
        /// </summary>
        /// <param name="number"></param>
        public static string Convert(int number)
        {
            if (number == 0)
            {
                return "zero";
            }

            if (number < 0)
            {
                return "negative " + Convert(System.Math.Abs(number));
            }

            string words = "";

            if (number / 1000000000 > 0)
            {
                words += Convert(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if (number / 1000000 > 0)
            {
                words += Convert(number / 1000000) + " million ";
                number %= 1000000;
            }

            if (number / 1000 > 0)
            {
                words += Convert(number / 1000) + " thousand ";
                number %= 1000;
            }

            if (number / 100 > 0)
            {
                words += Convert(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                {
                    words += "and ";
                }

                string[] unitsMap = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"};
                string[] tensMap = {"zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"};

                if (number < 20)
                {
                    words += unitsMap[number];
                }
                else
                {
                    words += tensMap[number / 10];

                    if (number % 10 > 0)
                    {
                        words += "-" + unitsMap[number % 10];
                    }
                }
            }

            return words.Replace("  ", " ");
        }
    }
}