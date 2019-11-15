using System;
using System.Collections.Generic;
using System.Linq;
using Argus.Extensions;

namespace Argus.Data
{
    /// <summary>
    /// Shared methods for calculating and verifying MOD10 check digits.
    /// </summary>
    /// <remarks></remarks>
    public class Mod10CheckDigit
    {
        //*********************************************************************************************************************
        //
        //             Class:  Mod10CheckDigit
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  05/20/2013
        //      Last Updated:  04/13/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Returns a check digit for a specified string.
        /// </summary>
        /// <param name="txt">The string value to create a check digit for</param>
        /// <returns>An integer check digit</returns>
        public static int Calculate(string txt)
        {
            List<string> baseList = new List<string>();

            txt = txt.ToUpper();

            for (int x = 0; x <= txt.Length - 1; x++)
            {
                baseList.Add(GetValue(txt.Substring(x, 1)));
            }

            List<string> calcList = new List<string>();

            // Get the new values
            for (int x = 0; x <= baseList.Count - 1; x++)
            {
                if (x.IsEven() == true)
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
            List<int> digitList = new List<int>();

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
            else
            {
                return 10 - lastDigit;
            }

        }

        /// <summary>
        /// Whether or not the specified value is correct (e.g. the check digit matches the rest of the provided value).  The inputted value would
        /// be the string with the check digit (e.g., a full account number).
        /// </summary>
        /// <param name="val">The value with the check digit to validate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsValid(string val)
        {
            if (string.IsNullOrWhiteSpace(val) == true)
            {
                return false;
            }
            else if (val.Length == 1)
            {
                return false;
            }

            string originalCheckDigit = val.SafeRight(1);
            string originalWithoutCheckDigit = val.SafeLeft(val.Length - 1);

            string calculatedCheckDigit = Calculate(originalWithoutCheckDigit).ToString();

            if (calculatedCheckDigit == originalCheckDigit)
            {
                return true;
            }
            else
            {
                return false;
            }
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

            switch (val)
            {
                case "0":
                    return "00";
                case "1":
                    return "01";
                case "2":
                    return "02";
                case "3":
                    return "03";
                case "4":
                    return "04";
                case "5":
                    return "05";
                case "6":
                    return "06";
                case "7":
                    return "07";
                case "8":
                    return "08";
                case "9":
                    return "09";
                case "A":
                    return "10";
                case "B":
                    return "11";
                case "C":
                    return "12";
                case "D":
                    return "13";
                case "E":
                    return "14";
                case "F":
                    return "15";
                case "G":
                    return "16";
                case "H":
                    return "17";
                case "I":
                    return "18";
                case "J":
                    return "19";
                case "K":
                    return "20";
                case "L":
                    return "21";
                case "M":
                    return "22";
                case "N":
                    return "23";
                case "O":
                    return "24";
                case "P":
                    return "25";
                case "Q":
                    return "26";
                case "R":
                    return "27";
                case "S":
                    return "28";
                case "T":
                    return "29";
                case "U":
                    return "30";
                case "V":
                    return "31";
                case "W":
                    return "32";
                case "X":
                    return "33";
                case "Y":
                    return "34";
                case "Z":
                    return "35";
                case "*":
                    return "36";
                case "@":
                    return "37";
                case "#":
                    return "38";
                default:
                    throw new Exception($"Invalid input. '{val}' is an unsupported character.");
            }

        }

    }

}