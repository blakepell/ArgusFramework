using System;
using System.Collections.Generic;

namespace Argus.Data
{
    /// <summary>
    ///     Represents an alphabet letter and provides utilities to work with the letters.
    /// </summary>
    /// <remarks>
    ///     The AlphabetLetter represents a single alpha letter.  It provides methods to move to the Next letter to provide
    ///     iteration, markers to identify when you are at the end of the alphabet as well as the ability to have Next restart
    ///     at the beginning of the alphabet.  A letter can be set equal to a character, a 1 length alpha string or a numeric value
    ///     that represents the letter (1-26, not the ASCII value though a setter method to load from ascii value is available).  When
    ///     a letter is a beginning market "a" or an end marker "z" the Next and Previous method will take you to the beginning and or the
    ///     end of the alphabet.  For example, if you use the "Previous" method and the letter value is "a", it will change the letter value
    ///     to "z".  If the current letter value is "z" and you use "Next" it will return to "a".<br /><br />
    ///     Notes:<br />
    ///     <list>
    ///         <item>
    ///             EOA will return True when the value is equal to "z", not when the last record has been read.  Since the list can
    ///             restart at the beginning there is technically no end to the list.  Be careful when using a Do While and the EOA property in
    ///             knowing what is happening.
    ///         </item>
    ///     </list>
    /// </remarks>
    public class AlphabetLetter
    {
        //*********************************************************************************************************************
        //
        //             Class:  AlphabetLetter
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  02/10/2010
        //      Last Updated:  11/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        private string _value = "";

        /// <summary>
        ///     Constructor:  Initializes the letter with a string.  The string must be a 1 character alpha letter, if not, an exception will
        ///     be thrown.
        /// </summary>
        /// <param name="letter"></param>
        public AlphabetLetter(string letter)
        {
            Value = letter;
        }

        /// <summary>
        ///     Constructor:  Initializes the letter with a Char.  The Char must be an alpha letter, if not, an exception will be thrown.
        /// </summary>
        /// <param name="c"></param>
        public AlphabetLetter(char c)
        {
            Value = c.ToString();
        }

        /// <summary>
        ///     Constructor:  Initializes the letter with an numeric value.  This is 1-26 corresponding to each letter form the alphabet.
        /// </summary>
        /// <param name="numericValue"></param>
        public AlphabetLetter(int numericValue)
        {
            var a = GetLetterFromNumericValue(numericValue);
            Value = a.Value;
        }

        /// <summary>
        ///     Constructor:  No parameters set.  You will need to set the AlphabetLetter equal to a value to initialize it if you use
        ///     this constructor overload method.<br />
        ///     <code>
        /// Dim myLetter As AlphabetLetter = "a"
        /// Dim myLetterTwo As AlphabetLetter = 4  ' Sets the letter equal to "d" which is the 4th character in the alphabet.
        /// </code>
        /// </summary>
        public AlphabetLetter()
        {
        }

        /// <summary>
        ///     The current letter.
        /// </summary>
        public string Value
        {
            get => _value;
            set
            {
                Validate(value);
                _value = value.ToLower();
            }
        }

        /// <summary>
        ///     The numeric value 1-26 of the current letter.
        /// </summary>
        public int NumericValue => GetNumericValueOfLetter(Value);

        /// <summary>
        ///     Returns the current character code value of the letter.  All AlphabetLetters are lower case and therefore will
        ///     return the lower case character code.
        /// </summary>
        public int CharacterCodeValue => Convert.ToInt32(Value);

        /// <summary>
        ///     Operator support for setting a string equal to an AlphabetLetter
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator string(AlphabetLetter a)
        {
            return a.ToString();
        }

        /// <summary>
        ///     Operator support for setting an AlphabetLetter equal to a string
        /// </summary>
        /// <param name="s"></param>
        public static implicit operator AlphabetLetter(string s)
        {
            return new AlphabetLetter(s);
        }

        /// <summary>
        ///     Operator support for setting an Integer equal to an AlphabetLetter
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator int(AlphabetLetter a)
        {
            return a.NumericValue;
        }

        /// <summary>
        ///     Operator support for setting an AlphabetLetter equal to a Integer
        /// </summary>
        /// <param name="i"></param>
        public static implicit operator AlphabetLetter(int i)
        {
            return new AlphabetLetter(i);
        }

        /// <summary>
        ///     Returns the string value of the AlphabetLetter.  E.g. "a", "b", "c", etc.
        /// </summary>
        public override string ToString()
        {
            return _value;
        }

        /// <summary>
        ///     Validates a string and ensures that it is a valid letter and only a letter.  If not, an exception will be thrown.
        /// </summary>
        private void Validate(string letter)
        {
            letter = letter.ToLower();

            if (letter.Length != 1)
            {
                throw new Exception("Cannot have a string length that is not equal to 1");
            }

            if (GetNumericValueOfLetter(letter) == -1)
            {
                throw new Exception($"'{letter}' is not a valid letter.");
            }
        }

        /// <summary>
        ///     Returns the numeric value of a letter (1-26).  This is not the ASCII value.
        /// </summary>
        /// <param name="letter"></param>
        public static int GetNumericValueOfLetter(string letter)
        {
            switch (letter.ToLower())
            {
                case "a":
                    return 1;
                case "b":
                    return 2;
                case "c":
                    return 3;
                case "d":
                    return 4;
                case "e":
                    return 5;
                case "f":
                    return 6;
                case "g":
                    return 7;
                case "h":
                    return 8;
                case "i":
                    return 9;
                case "j":
                    return 10;
                case "k":
                    return 11;
                case "l":
                    return 12;
                case "m":
                    return 13;
                case "n":
                    return 14;
                case "o":
                    return 15;
                case "p":
                    return 16;
                case "q":
                    return 17;
                case "r":
                    return 18;
                case "s":
                    return 19;
                case "t":
                    return 20;
                case "u":
                    return 21;
                case "v":
                    return 22;
                case "w":
                    return 23;
                case "x":
                    return 24;
                case "y":
                    return 25;
                case "z":
                    return 26;
                default:
                    return -1;
            }
        }

        /// <summary>
        ///     Returns the numeric value of a letter (1-26).  This is not the ASCII value.
        /// </summary>
        /// <param name="c"></param>
        public static int GetNumericValueOfLetter(char c)
        {
            return GetNumericValueOfLetter(c.ToString());
        }

        /// <summary>
        ///     Returns the numeric value of a letter (1-26).  This is not the ASCII value.
        /// </summary>
        /// <param name="a"></param>
        public static int GetNumericValueOfLetter(AlphabetLetter a)
        {
            return GetNumericValueOfLetter(a.Value);
        }

        /// <summary>
        ///     Returns a new AlphabetLetter class based off of the numeric value (e.g. 1-26).  This is not the ASCII value.  If you want
        ///     to load by the ASCII value then use the LoadByCharacterCode method.
        /// </summary>
        /// <param name="numericValue"></param>
        public static AlphabetLetter GetLetterFromNumericValue(int numericValue)
        {
            switch (numericValue)
            {
                case 1:
                    return new AlphabetLetter("a");
                case 2:
                    return new AlphabetLetter("b");
                case 3:
                    return new AlphabetLetter("c");
                case 4:
                    return new AlphabetLetter("d");
                case 5:
                    return new AlphabetLetter("e");
                case 6:
                    return new AlphabetLetter("f");
                case 7:
                    return new AlphabetLetter("g");
                case 8:
                    return new AlphabetLetter("h");
                case 9:
                    return new AlphabetLetter("i");
                case 10:
                    return new AlphabetLetter("j");
                case 11:
                    return new AlphabetLetter("k");
                case 12:
                    return new AlphabetLetter("l");
                case 13:
                    return new AlphabetLetter("m");
                case 14:
                    return new AlphabetLetter("n");
                case 15:
                    return new AlphabetLetter("o");
                case 16:
                    return new AlphabetLetter("p");
                case 17:
                    return new AlphabetLetter("q");
                case 18:
                    return new AlphabetLetter("r");
                case 19:
                    return new AlphabetLetter("s");
                case 20:
                    return new AlphabetLetter("t");
                case 21:
                    return new AlphabetLetter("u");
                case 22:
                    return new AlphabetLetter("v");
                case 23:
                    return new AlphabetLetter("w");
                case 24:
                    return new AlphabetLetter("x");
                case 25:
                    return new AlphabetLetter("y");
                case 26:
                    return new AlphabetLetter("z");
                default:
                    throw new Exception($"Invalid numeric value supplied '{numericValue}'.  The numeric value must be between 1 and 26.");
            }
        }

        /// <summary>
        ///     Sets the Value property equal to the character code for the specified value.  For instance with ASCII, 97 would be "a", 98 would be
        ///     "b", etc.  Valid character codes will be those that are equal to the lower or upper case values for an alpha letter.
        /// </summary>
        /// <param name="characterCode"></param>
        public void LoadByCharacterCode(int characterCode)
        {
            Value = Convert.ToChar(characterCode).ToString().ToLower();
        }

        /// <summary>
        ///     Gets the next letter in the alphabet.  If the current letter is "z" then "a" will be returned.
        /// </summary>
        public AlphabetLetter GetNextLetter()
        {
            int value = NumericValue;

            if (value == 26)
            {
                value = 1;
            }
            else if (value < 1)
            {
                value = 1;
            }
            else
            {
                value += 1;
            }

            return GetLetterFromNumericValue(value);
        }

        /// <summary>
        ///     Gets the previous letter in the alphabet.  If the current letter is "a" then "z" will be returned.
        /// </summary>
        public AlphabetLetter GetPreviousLetter()
        {
            int value = NumericValue;

            if (value == 1)
            {
                value = 26;
            }
            else if (value < 1)
            {
                value = 1;
            }
            else
            {
                value -= 1;
            }

            return GetLetterFromNumericValue(value);
        }

        /// <summary>
        ///     Sets this letter equal to the next letter in the alphabet.  If the current letter is "z" then "a" will be returned.
        /// </summary>
        public void Next()
        {
            var a = GetNextLetter();
            Value = a.Value;
        }

        /// <summary>
        ///     Sets this letter equal to the previous letter in the alphabet.  If the current letter is "a" then "z" will be returned.
        /// </summary>
        public void Previous()
        {
            var a = GetPreviousLetter();
            Value = a.Value;
        }

        /// <summary>
        ///     End of Alphabet:  Determines if the current letter is the last letter in the alphabet (e.g. "z").  This is not after "z"
        ///     has been read, it is if the current value is "z"
        /// </summary>
        public bool Eoa()
        {
            if (Value == "z")
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    ///     Various helper utilities to work with the alphabet
    /// </summary>
    public class AlphabetUtilities
    {
        //*********************************************************************************************************************
        //
        //             Class:  Alphabet.AlphabetUtilities
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  02/10/2010
        //      Last Updated:  04/07/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Returns a list of all 26 letters as AlphabetLetter classes.
        /// </summary>
        public static List<AlphabetLetter> AlphabetList()
        {
            var returnList = new List<AlphabetLetter>();

            for (int x = 1; x <= 26; x++)
            {
                returnList.Add(new AlphabetLetter(x));
            }

            return returnList;
        }

        /// <summary>
        ///     Returns each letter of the alphabet as a string in the list.
        /// </summary>
        public static List<string> AlphabetStringList()
        {
            var returnList = new List<string>();

            for (int x = 1; x <= 26; x++)
            {
                var a = new AlphabetLetter(x);
                returnList.Add(a.Value);
            }

            return returnList;
        }

        /// <summary>
        ///     Returns a sequence string based off of a numeric value.  This only uses lower case and numeric values.  Use GetNumericValueFromSimpleSequence
        ///     to decode the value.
        /// </summary>
        /// <param name="numericValue"></param>
        /// <returns></returns>
        /// <remarks>
        ///     1 Letter (http://www.blakepell.com/r/a) 36 available links
        ///     2 Letters (http://www.blakepell.com/r/ab) 1,296 available links
        ///     3 Letters (http://www.blakepell.com/r/abc) 46,656 available links
        ///     4 Letters (http://www.blakepell.com/r/abcd) 1,679,616 available links
        ///     5 Letters (http://www.blakepell.com/r/abcde) 60,466,176 available links
        ///     6 Letters (http://www.blakepell.com/r/abcdef) 2,176,782,336 available links
        /// </remarks>
        public static string GetSequence(double numericValue)
        {
            string seq = "0123456789abcdefghijklmnopqrstuvwxyz";
            string converted = "";

            while (numericValue > 0)
            {
                converted += seq.Substring((int) numericValue % seq.Length, 1);
                numericValue = System.Math.Floor(numericValue / seq.Length);
            }

            return converted;
        }

        /// <summary>
        ///     Returns a sequence string based off of a numeric value.  You must provide a custom sequence with this overload, a sequence would look like
        ///     "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".  The same sequence would be needed to decode the value.
        /// </summary>
        /// <param name="numericValue"></param>
        /// <param name="seq"></param>
        public static string GetSequence(double numericValue, string seq)
        {
            string converted = "";

            while (numericValue > 0)
            {
                converted += seq.Substring((int) numericValue % seq.Length, 1);
                numericValue = System.Math.Floor(numericValue / seq.Length);
            }

            return converted;
        }

        /// <summary>
        ///     Returns a numeric value based off of a shortened string using GetSequenceSimple which supports lower case and numeric values.
        /// </summary>
        /// <param name="converted"></param>
        /// <returns></returns>
        /// <remarks>
        ///     1 Letter (http://www.blakepell.com/r/a) 36 available links
        ///     2 Letters (http://www.blakepell.com/r/ab) 1,296 available links
        ///     3 Letters (http://www.blakepell.com/r/abc) 46,656 available links
        ///     4 Letters (http://www.blakepell.com/r/abcd) 1,679,616 available links
        ///     5 Letters (http://www.blakepell.com/r/abcde) 60,466,176 available links
        ///     6 Letters (http://www.blakepell.com/r/abcdef) 2,176,782,336 available links
        /// </remarks>
        public static double GetValueFromSequence(string converted)
        {
            string seq = "0123456789abcdefghijklmnopqrstuvwxyz";
            double numericValue = 0;

            for (int x = 0; x <= converted.Length - 1; x++)
            {
                int len = seq.IndexOf(converted[x]);
                double power = System.Math.Pow(seq.Length, x);
                numericValue += len * power;
            }

            return numericValue;
        }

        /// <summary>
        ///     Returns a numeric value based off of a converted string.  You must provide a custom sequence with this overload, a sequence would look like
        ///     "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".  The same sequence would be needed to decode the value.
        /// </summary>
        /// <param name="converted"></param>
        /// <param name="seq"></param>
        public static double GetValueFromSequence(string converted, string seq)
        {
            double numericValue = 0;

            for (int x = 0; x <= converted.Length - 1; x++)
            {
                int len = seq.IndexOf(converted[x]);
                double power = System.Math.Pow(seq.Length, x);
                numericValue += len * power;
            }

            return numericValue;
        }
    }
}