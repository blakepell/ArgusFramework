/*
 * @author            : Blake Pell
 * @initial date      : 2013-05-20
 * @last updated      : 2023-06-08
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using Argus.Extensions;

namespace Argus.Data
{
    /// <summary>
    /// Shared methods for calculating and verifying MOD10 check digits.
    /// </summary>
    public static class Mod10CheckDigit
    {
        /// <summary>
        /// Returns a check digit for a specified string.
        /// </summary>
        /// <param name="txt">The string value to create a check digit for</param>
        /// <returns>An integer check digit</returns>
        public static int Calculate(string txt)
        {
            var baseList = new List<string>();

            txt = txt.ToUpper();

            for (int x = 0; x <= txt.Length - 1; x++)
            {
                baseList.Add(GetValue(txt.Substring(x, 1)));
            }

            var calcList = new List<string>();

            // Get the new values
            for (int x = 0; x <= baseList.Count - 1; x++)
            {
                if (x.IsEven())
                {
                    calcList.Add((Convert.ToInt32(baseList[x]) * 1).ToString());
                }
                else
                {
                    calcList.Add((Convert.ToInt32(baseList[x]) * 2).ToString());
                }
            }

            // Pad any single digits
            for (int x = 0; x <= calcList.Count - 1; x++)
            {
                if (calcList[x].Length == 1)
                {
                    calcList[x] = "0" + calcList[x];
                }
            }

            // Bust these all up into single digits
            var digitList = new List<int>();

            for (int x = 0; x <= calcList.Count - 1; x++)
            {
                digitList.Add(Convert.ToInt32(calcList[x].Left(1)));
                digitList.Add(Convert.ToInt32(calcList[x].Right(1)));
            }

            int sumOfDigits = digitList.Sum();
            int lastDigit = Convert.ToInt32(sumOfDigits.ToString().Right(1));

            // 5/17/2013 Norm specified that if the last digit is a 0 that 0 should be returned
            if (lastDigit == 0)
            {
                return 0;
            }

            return 10 - lastDigit;
        }

        /// <summary>
        /// Whether or not the specified value is correct (e.g. the check digit matches the rest of the provided value).  The inputted value would
        /// be the string with the check digit (e.g., a full account number).
        /// </summary>
        /// <param name="val">The value with the check digit to validate.</param>
        public static bool IsValid(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return false;
            }

            if (val.Length == 1)
            {
                return false;
            }

            string originalCheckDigit = val.SafeRight(1);
            string originalWithoutCheckDigit = val.SafeLeft(val.Length - 1);
            string calculatedCheckDigit = Calculate(originalWithoutCheckDigit).ToString();

            return calculatedCheckDigit == originalCheckDigit;
        }

        /// <summary>
        /// Returns the lookup value as specified in the MOD-10 documentation.
        /// </summary>
        public static string GetValue(string val)
        {
            if (val.Length > 1)
            {
                throw new Exception("Value has a length of greater than 1");
            }

            return val switch
            {
                "0" => "00",
                "1" => "01",
                "2" => "02",
                "3" => "03",
                "4" => "04",
                "5" => "05",
                "6" => "06",
                "7" => "07",
                "8" => "08",
                "9" => "09",
                "A" => "10",
                "B" => "11",
                "C" => "12",
                "D" => "13",
                "E" => "14",
                "F" => "15",
                "G" => "16",
                "H" => "17",
                "I" => "18",
                "J" => "19",
                "K" => "20",
                "L" => "21",
                "M" => "22",
                "N" => "23",
                "O" => "24",
                "P" => "25",
                "Q" => "26",
                "R" => "27",
                "S" => "28",
                "T" => "29",
                "U" => "30",
                "V" => "31",
                "W" => "32",
                "X" => "33",
                "Y" => "34",
                "Z" => "35",
                "*" => "36",
                "@" => "37",
                "#" => "38",
                _ => throw new Exception($"Invalid input. '{val}' is an unsupported character.")
            };
        }
    }
}