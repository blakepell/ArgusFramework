﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Security;
using System.Globalization;

namespace Argus.Extensions
{

    /// <summary>
    /// String extension methods.
    /// </summary>
    public static class StringExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  StringExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  01/12/2008
        //      Last Updated:  09/26/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// This function will return the specified amount of characters from the left hand side of the string.  This is the equivalent of the Visual Basic Left function.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Left(this string str, int length)
        {            
            return str.Substring(0, length);
        }

        /// <summary>
        /// This function will return the specified amount of characters from the right hand side of the string.  This is the equivalent of the Visual Basic Right function.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Right(this string str, int length)
        {
            return str.Substring(str.Length - length, length);
        }

        /// <summary>
        /// Returns the specified number of characters from the left hand side of the string.  If the number asked for is longer the
        /// string then the entire string is returned without an exception.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SafeLeft(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }

            if (length >= str.Length)
            {
                return str;
            }
            else if (length < 0)
            {
                return "";
            }

            return Left(str, length);
        }

        /// <summary>
        /// Returns the specified number of characters from the right hand side of the string.  If the number asked for is longer the
        /// string then the entire string is returned without an exception.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SafeRight(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }

            if (length >= str.Length)
            {
                return str;
            }
            else if (length < 0)
            {
                return "";
            }

            return Right(str, length);
        }

        /// <summary>
        /// Retrieves a substring from this instance with error checking to prevent exceptions.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string SafeSubstring(this string str, int startIndex)
        {
            if (startIndex > str.Length)
            {
                return "";
            }

            if (startIndex < 0)
            {
                return str;
            }

            return str.Substring(startIndex);
        }

        /// <summary>
        /// Retrieves a substring from this instance with error checking to prevent exceptions.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SafeSubstring(this string str, int startIndex, int length)
        {
            if (startIndex > str.Length)
            {
                return "";
            }

            // We have a good start index but that index plus the requested lenth is longer than the string
            // so we will just return the string from that point.
            if ((startIndex + length) > str.Length)
            {
                return str.Substring(startIndex);
            }

            if (startIndex < 0)
            {
                return str;
            }

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// Simulates the same functionality provide by the traditional 1 based index Mid function.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startPos"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Mid(this string str, int startPos, int length)
        {
            return str.Substring(startPos - 1, length);
        }

        /// <summary>
        /// Returns a string that is truncated at a given length amount.  Ellipses are then added on at the end but only if
        /// the string needs to be trimmed.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimLengthWithEllipses(this string value, int length)
        {
            if (value.Length > length)
            {
                value = string.Format("{0}...", Left(value, length));
            }

            return value;
        }

        /// <summary>
        /// Trims whitespace from the beginning and ending of a string (coupling both Trim and replacing tabs)
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="includeLineTerminators">Whether or not to trim off the line terminators also.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimWhitespace(this string buf, bool includeLineTerminators)
        {
            if (includeLineTerminators == true)
            {
                char[] chars = { ' ', '\t', '\r', '\n' };
                return buf.Trim(chars);
            }
            else
            {
                char[] chars = { ' ', '\t' };
                return buf.Trim(chars);
            }
        }

        /// <summary>
        /// Trims whitespace from the beginning of a string (coupling both Trim and replacing tabs)
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="includeLineTerminators">Whether or not to trim off the line terminators also.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimWhitespaceStart(this string buf, bool includeLineTerminators)
        {
            if (includeLineTerminators == true)
            {
                char[] chars = { ' ', '\t', '\r', '\n' };
                return buf.TrimStart(chars);
            }
            else
            {
                char[] chars = { ' ', '\t' };
                return buf.TrimStart(chars);
            }
        }

        /// <summary>
        /// Trims whitespace from the beginning of a string (coupling both Trim and replacing tabs)
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="includeLineTerminators">Whether or not to trim off the line terminators also.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimWhitespaceEnd(this string buf, bool includeLineTerminators)
        {
            if (includeLineTerminators == true)
            {
                char[] chars = { ' ', '\t', '\r', '\n' };
                return buf.TrimEnd(chars);
            }
            else
            {
                char[] chars = { ' ', '\t' };
                return buf.TrimEnd(chars);
            }
        }

        /// <summary>
        /// Removes all trailing occurances of the specified string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimStr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimEnd(this string str, string trimStr)
        {
            return str.TrimEnd(trimStr.ToCharArray());
        }

        /// <summary>
        /// Removes all leading occurances of the specified string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimStr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimStart(this string str, string trimStr)
        {
            return str.TrimStart(trimStr.ToCharArray());
        }

        /// <summary>
        /// Removes all leading and trailing occurances of the specified string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimStr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Trim(this string str, string trimStr)
        {
            return str.Trim(trimStr.ToCharArray());
        }

        /// <summary>
        /// Trims whitespace from the beginning and ending of each line in a string if it contains multiple lines.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="includeLineTerminators">Whether or not to trim off the line terminators also.  If true, this will
        /// remove the line terminator from the string.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimEachLineWhitespace(this string buf, bool includeLineTerminators)
        {
            char[] charsWithLineTerminators = { ' ', '\t', '\r', '\n' };
            char[] charsWithoutLineTerminators = { ' ', '\t' };
            StringBuilder sb = new StringBuilder();

            string[] lines = buf.Split('\n');

            foreach (string line in lines)
            {
                if (includeLineTerminators == true)
                {
                    sb.Append(line.Trim(charsWithLineTerminators));
                }
                else
                {
                    sb.AppendLine(line.Trim(charsWithoutLineTerminators));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Trims whitespace from the beginning and ending of each line in a string if it contains multiple lines.  This overload will not
        /// remove line terminators.
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimEachLineWhitespace(this string buf)
        {
            return TrimEachLineWhitespace(buf, false);
        }

        /// <summary>
        /// Determines whether a string is a numeric value.  This implementation uses Decimal.TryParse to produce it's value.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsNumeric(this string str)
        {
            return decimal.TryParse(str, out decimal result);
        }

        /// <summary>
        /// Inserts a string after given number of characters specified in the interval property.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="textToInsert">The string to insert at the specified interval.  This could be a line break, an HTML tag, etc.</param>
        /// <param name="interval">The n'th number of characters to insert the textToInsert value at.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string InsertAtInterval(this string value, string textToInsert, int interval)
        {

            StringBuilder sb = new StringBuilder();


            for (int x = 0; x <= value.Length - 1; x++)
            {
                // Insert the text to insert at the given interval
                if (x % interval == 0 && x > 0)
                {
                    sb.Append(textToInsert);
                }

                sb.Append(value[x]);
            }

            return sb.ToString();

        }

        /// <summary>
        /// Removes non-numeric characters from a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string RemoveNonNumericCharacters(this string value)
        {
            return Regex.Replace(value, "[^0-9]", "");
        }

        /// <summary>
        /// Removes non-numeric characters from a string.  An option is available to allow for a period in case this is used with
        /// a money value or number that requires a decimal place.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allowPeriod">If true, any periods will be left, if false, all periods will also be removed.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string RemoveNonNumericCharacters(this string value, bool allowPeriod)
        {
            if (allowPeriod == false)
            {
                return RemoveNonNumericCharacters(value);
            }
            else
            {
                return Regex.Replace(value, "[^0-9|^.]", "");
            }
        }

        /// <summary>
        /// Removes non-numeric characters from a string.  An option is available to allow for a period and/or comma in case this is used with
        /// a money value or number that requires a decimal place or a value that requires keeping it's formatting with commas and peroids only.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allowPeriod">If true, any periods will be left, if false, all periods will also be removed.</param>
        /// <param name="allowComma">If true, any commas will be left, if false, all commas will also be removed.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string RemoveNonNumericCharacters(this string value, bool allowPeriod, bool allowComma)
        {
            if (allowComma == true && allowPeriod == true)
            {
                return Regex.Replace(value, "[^0-9|^.|^,]", "");
            }
            else if (allowPeriod == true && allowComma == false)
            {
                return RemoveNonNumericCharacters(value, true);
            }
            else if (allowComma == true && allowPeriod == false)
            {
                return Regex.Replace(value, "[^0-9|^,]", "");
            }
            else
            {
                return RemoveNonNumericCharacters(value);
            }
        }

        /// <summary>
        /// Returns the title case for the string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string value)
        {
            var ti = new CultureInfo("en-US", false).TextInfo;
            return ti.ToTitleCase(value);
        }

        /// <summary>
        /// Inverts the case of each character in a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToInvertCase(this string value)
        {
            var sb = new StringBuilder();

            foreach (var c in value.ToCharArray())
            {
                if (char.IsUpper(c))
                {
                    sb.Append(char.ToLower(c));
                }
                else
                {
                    sb.Append(char.ToUpper(c));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts each character in a string to a random upper or lower case.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToRandomCase(this string value)
        {
            var sb = new StringBuilder();
            var rnd = new Random();

            foreach (var c in value.ToCharArray())
            {
                if (rnd.Next(1, 100) > 50)
                {
                    sb.Append(char.ToLower(c));
                }
                else
                {
                    sb.Append(char.ToUpper(c));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes everything from a string that is not a letter or a digit.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string RemoveSpecialCharacters(this string s)
        {
            if ((s == null))
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder(s.Length);
            int i = 0;
            for (i = 0; i <= s.Length - 1; i++)
            {
                if (char.IsLetterOrDigit(s, i))
                {
                    builder.Append(s[i]);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Removes all numeric characters from a string value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string RemoveNumericCharacters(this string value)
        {
            return Regex.Replace(value, "[0-9]", "");
        }

        /// <summary>
        /// Removes non ASCII characters via a regular expression.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveNonAsciiCharacters(this string value)
        {
            return Regex.Replace(value, @"[^\u0000-\u007F]+", string.Empty);
        }

        /// <summary>
        /// Normalizes accent marks that some programs and web-sites replace for common characters.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NormalizeAccentMarks(this string value)
        {
            if (value.IndexOf('\u2013') > -1) value = value.Replace('\u2013', '-');
            if (value.IndexOf('\u2014') > -1) value = value.Replace('\u2014', '-');
            if (value.IndexOf('\u2015') > -1) value = value.Replace('\u2015', '-');
            if (value.IndexOf('\u2017') > -1) value = value.Replace('\u2017', '_');
            if (value.IndexOf('\u2018') > -1) value = value.Replace('\u2018', '\'');
            if (value.IndexOf('\u2019') > -1) value = value.Replace('\u2019', '\'');
            if (value.IndexOf('\u201a') > -1) value = value.Replace('\u201a', ',');
            if (value.IndexOf('\u201b') > -1) value = value.Replace('\u201b', '\'');
            if (value.IndexOf('\u201c') > -1) value = value.Replace('\u201c', '\"');
            if (value.IndexOf('\u201d') > -1) value = value.Replace('\u201d', '\"');
            if (value.IndexOf('\u201e') > -1) value = value.Replace('\u201e', '\"');
            if (value.IndexOf('\u2026') > -1) value = value.Replace("\u2026", "...");
            if (value.IndexOf('\u2032') > -1) value = value.Replace('\u2032', '\'');
            if (value.IndexOf('\u2033') > -1) value = value.Replace('\u2033', '\"');

            return value;
        }

        /// <summary>
        /// Removes blank lines including blank lines that just have whitespace on them.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveBlankLines(this string value)
        {
            return Regex.Replace(value, @"^\s*$[\n]*", string.Empty, RegexOptions.Multiline);
        }

        /// <summary>
        /// Creates a string repeated multiple times.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="repeatCount">Number of times to repeat the string</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Repeat(this string buf, int repeatCount)
        {
            if (string.IsNullOrEmpty(buf))
            {
                return buf;
            }

            var result = new StringBuilder(buf.Length * repeatCount);
            return result.Insert(0, buf, repeatCount).ToString();
        }

        /// <summary>
        /// Pad's the right side of a string to make sure it's a specified length.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string PadRightToLength(this string buf, int length)
        {
            return string.Format("{0,-" + length.ToString() + "}", buf);
        }

        /// <summary>
        /// Adds a value onto the end of the string if it does not already exist there.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string AddRightIfDoesntExist(this string buf, string value)
        {
            if (buf.SafeRight(value.Length) != value)
            {
                return buf + value;
            }
            else
            {
                return buf;
            }
        }

        /// <summary>
        /// Adds a value onto the beginning of a string if it does not already exist there.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string AddLeftIfDoesntExist(this string buf, string value)
        {
            if (buf.SafeLeft(value.Length) != value)
            {
                return value + buf;
            }
            else
            {
                return buf;
            }
        }

        /// <summary>
        /// Returns an empty string if the current string object is null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DefaultIfNull(this string str)
        {
            if (str != null)
            {
                return str;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the specified default value if the current string object is null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DefaultIfNull(this string str, string defaultValue)
        {
            if (str != null)
            {
                return str;
            }
            else
            {
                if (defaultValue != null)
                {
                    return defaultValue;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Returns the specified default value if the current string object is null or empty.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static object DefaultIfNullOrEmpty(this string str, string defaultValue)
        {
            if (string.IsNullOrEmpty(defaultValue))
            {
                defaultValue = "";
            }

            if (string.IsNullOrEmpty(str) == true)
            {
                return defaultValue;
            }
            else
            {
                return str;
            }

        }

        /// <summary>
        /// Indicates whether the regular expression finds a match in the current string object for the specified pattern.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="regExPattern"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsMatch(this string str, string regExPattern)
        {
            return new Regex(regExPattern).IsMatch(str);
        }

        /// <summary>
        /// Will take a string and split its contents into a list based off of a provided delimiter.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> ToList(this string buf, string delimiter)
        {
            string[] array = buf.Split(delimiter.ToCharArray());
            return array.ToList();
        }

        /// <summary>
        /// Will take a string and split its contents into a list based off of multiple delimters that are provided.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> ToList(this string buf, string[] delimiter)
        {
            string firstDelim = "";

            if (delimiter.Count() > 0)
            {
                firstDelim = delimiter[0];
            }

            // Convert all delimter characters into the same character
            foreach (string delim in delimiter)
            {
                buf = buf.Replace(delim, firstDelim);
            }

            string[] array = buf.Split(firstDelim.ToCharArray());
            return array.ToList();
        }

        /// <summary>
        /// Will take a string and split its contents into a string array based off of a provided delimiter.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] ToArray(this string buf, string delimiter)
        {
            return buf.Split(delimiter.ToCharArray());
        }

        /// <summary>
        /// Will take a string and split its contents into a list based off of multiple delimters that are provided.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] ToArray(this string buf, string[] delimiter)
        {
            string firstDelim = "";
            if (delimiter.Count() > 0)
            {
                firstDelim = delimiter[0];
            }

            // Convert all delimter characters into the same character
            foreach (string delim in delimiter)
            {
                buf = buf.Replace(delim, firstDelim);
            }

            return buf.Split(firstDelim.ToCharArray());
        }

        /// <summary>
        /// An extension method that replaces the first occurance of a specified string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="searchTxt"></param>
        /// <param name="replaceTxt"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ReplaceFirst(this string str, string searchTxt, string replaceTxt)
        {
            int pos = str.IndexOf(searchTxt);

            if (pos < 0)
            {
                return str;
            }

            return str.Substring(0, pos) + replaceTxt + str.Substring(pos + searchTxt.Length);
        }

        /// <summary>
        /// An extension method that replaces the last occurance of a specified string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="searchTxt"></param>
        /// <param name="replaceTxt"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ReplaceLast(this string str, string searchTxt, string replaceTxt)
        {
            int pos = str.LastIndexOf(searchTxt);

            if (pos < 0)
            {
                return str;
            }

            return str.Substring(0, pos) + replaceTxt + str.Substring(pos + searchTxt.Length);
        }

        /// <summary>
        /// Returns the string in between two markers.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="beginMarker">The beginning marker, such as a single or double quote.</param>
        /// <param name="endMarker">The end marker, such as a single or double quote.</param>
        /// <param name="includeMarkers">Whether or not to include the markers on each item in the list returned.</param>
        /// <returns>A list of string values that were found in between quotes in the original string.</returns>
        /// <remarks></remarks>
        public static List<string> GetStringInBetween(this string searchText, string beginMarker, string endMarker, bool includeMarkers)
        {
            string exp = string.Format("{0}([^{0}^{1}]*){1}", Regex.Escape(beginMarker), Regex.Escape(endMarker));
            List<string> returnList = new List<string>();

            foreach (Match m in Regex.Matches(searchText, exp, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant))
            {
                if (includeMarkers)
                {
                    returnList.Add(m.Value);
                }
                else
                {
                    // return the portion of the matched string without the markers
                    returnList.Add(m.Value.Substring(beginMarker.Length, m.Value.Length - beginMarker.Length - endMarker.Length));
                }
            }

            return returnList;
        }

        /// <summary>
        /// Returns the string in between two markers.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="marker">The beginning and ending marker, such as a single or double quote.</param>
        /// <param name="includeMarkers">Whether or not to include the markers on each item in the list returned.</param>
        /// <returns>A list of string values that were found in between quotes in the original string.</returns>
        /// <remarks></remarks>
        public static List<string> GetStringInBetween(this string searchText, string marker, bool includeMarkers)
        {
            return GetStringInBetween(searchText, marker, marker, includeMarkers);
        }

        /// <summary>
        /// A string value wrapped by a specified character at the beginning and end.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="wrapCharacter"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToWrappedString(this string str, string wrapCharacter)
        {
            return $"{wrapCharacter}{str}{wrapCharacter}";
        }

        /// <summary>
        /// A string value wrapped by a beginning and an ending character.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startWrapCharacter"></param>
        /// <param name="endWrapCharacter"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToWrappedString(this string str, string startWrapCharacter, string endWrapCharacter)
        {
            return $"{startWrapCharacter}{str}{endWrapCharacter}";
        }

        /// <summary>
        /// Converts the string into a Base64 string.  UTF8 is used by default for the encoding as supported by the portable class library.
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToBase64(this string buf)
        {
            var b = Encoding.UTF8.GetBytes(buf);
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// Converts the string from a Base64 string to a string.  UTF8 is used by default for the encoding as supported by the portable class library.
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FromBase64(this string buf)
        {
            var b = Convert.FromBase64String(buf);
            return Encoding.UTF8.GetString(b, 0, b.Length);
        }

        /// <summary>
        /// Truncates the string after the first occurance of the character or string provided.
        /// </summary>
        /// <param name="str">The source string.</param>
        /// <param name="searchFor">The marker to use to start the truncation.</param>
        /// <param name="keepSearchString">Whether or not to keep the searchFor marker in the return string.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TruncateAfter(this string str, string searchFor, bool keepSearchString)
        {
            if (str.Contains(searchFor) == false)
            {
                return str;
            }

            if (keepSearchString == true)
            {
                return str.Substring(0, str.IndexOf(searchFor) + searchFor.Length);
            }
            else
            {
                return str.Substring(0, str.IndexOf(searchFor));
            }
        }

        /// <summary>
        /// Truncates the string before the first occurance of the character or string provided.
        /// </summary>
        /// <param name="str">The source string.</param>
        /// <param name="searchFor">The marker to use to start the truncation.</param>
        /// <param name="keepSearchString">Whether or not to keep the searchFor marker in the return string.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TruncateBefore(this string str, string searchFor, bool keepSearchString)
        {
            if (str.Contains(searchFor) == false)
            {
                return str;
            }

            if (keepSearchString == true)
            {
                return str.Substring(str.IndexOf(searchFor), str.Length - str.IndexOf(searchFor));
            }
            else
            {
                return str.Substring(str.IndexOf(searchFor) + searchFor.Length, str.Length - str.IndexOf(searchFor) - searchFor.Length);
            }
        }

        /// <summary>
        /// Truncates a string after a specified position.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <param name="suffix">An optional string to append after the truncated position.</param>
        /// <returns></returns>
        public static string Truncate(this string value, int position, string suffix = "")
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= position ? value : value.Substring(0, position) + suffix;
        }

        /// <summary>
        /// Formats a number without decimal places.  If the string is not numeric it's full contents
        /// will be returned.  This will return no decimal places.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatIfNumber(this string str)
        {
            if (str.IsNumeric())
            {
                double x = Convert.ToDouble(str);
                return x.ToString("#,0");
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// Formats a number with commas and the specified number of decimal places.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatIfNumber(this string str, int decimalPlaces)
        {
            if (IsNumeric(str) == true)
            {
                string formatString = "#,0.";

                formatString = formatString.PadRight(formatString.Length + decimalPlaces, '0');

                double x = Convert.ToDouble(str);
                return x.ToString(formatString);
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// Returns a string array that contains the substrings in this instance that are delimited by elements of a specified Unicode string.  The
        /// string will be converted into a character array and passed on to the underlaying String.Split method provided by the .Net Framework.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] SplitPcl(this string str, string seperator)
        {
            return str.Split(seperator.ToCharArray());
        }

        /// <summary>
        /// Deletes the specified number of characters from the left hand side of the string.  If the number to delete is longer than
        /// the length of the string then a blank string will be returned.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DeleteLeft(this string str, int length)
        {
            if (length > str.Length)
            {
                return "";
            }

            return str.Right(str.Length - length);
        }

        /// <summary>
        /// Deletes the specified number of characters from the right hand side of the string.  If the number to delete is longer than
        /// the length of the string then a blank string will be returned.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DeleteRight(this string str, int length)
        {
            if (length > str.Length)
            {
                return "";
            }

            return str.Left(str.Length - length);
        }

        /// <summary>
        /// Returns all words in a string that start with a specified character (to return hashtags or twitter handles for instance).  This uses a regular expression
        /// that return words so it's looking for characters, for example if you were looking for @blakepell or #test.  It would not correctly work with #this.is.a.test.
        /// </summary>
        /// <param name="parentText">The text to search.</param>
        /// <param name="wordToSearchFor">This is the character or the start of the word to search for, e.g. #, @</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> GetWordsThatStartWithWord(this string parentText, string wordToSearchFor)
        {
            List<string> lst = new List<string>();
            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(parentText, string.Format("(?<!\\w){0}\\w+", wordToSearchFor)))
            {
                lst.Add(match.Value);
            }
            return lst;
        }

        /// <summary>
        /// Converts a string into a MemoryStream returned as a Stream.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static MemoryStream ToMemoryStream(this string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream(b);
            return ms;
        }


        /// <summary>
        /// Determines if the string is a guid format (via Guid.TryParse).
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsGuid(this string str)
        {
            if (str != null)
            {
                Guid testGuid = Guid.Empty;

                if (Guid.TryParse(str, out testGuid))
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// Determines if the string is a guid format and additionally is one that has dashes.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="requireDashes">Whether or not the potential guid should be validated to include dashes.</param>
        /// <returns></returns>
        public static bool IsGuid(this string str, bool requireDashes)
        {
            if (requireDashes == true)
            {
                if (str.Contains("-") & IsGuid(str))
                {
                    return true;
                }

                return false;
            }

            return IsGuid(str);

        }

        /// <summary>
        /// Normalizes line endings (makes \n\r -> \r\n, makes \n\n, \r\n\r\n) etc.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NormalizeLineEndings(this string str)
        {
            return Regex.Replace(str, @"\r\n|\n\r|\n|\r", "\r\n");
        }

        /// <summary>
        /// Will remove any ASCII characters that are not valid XML characters.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A new string with characters corresponding to invalid XML ASCII codes removed.</returns>
        public static string ToValidXmlAsciiCharacters(this string input)
        {
            var validCodes = new[] { 0, 9, 10, 13, 32 };

            for (int i = 0; i <= 32; i++)
            {
                if (!validCodes.Contains(i))
                {
                    input = input.Replace(Convert.ToString((char)i), "");
                }
            }

            return input;
        }

        /// <summary>
        /// Capitalizes the first letter of a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        /// <summary>
        /// HTML encodes a string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string text)
        {
            return System.Net.WebUtility.HtmlEncode(text);
        }

        /// <summary>
        /// Html Decodes a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string text)
        {
            return System.Net.WebUtility.HtmlDecode(text);
        }

        /// <summary>
        /// Url Encodes a string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UrlEncode(this string text)
        {
            return System.Net.WebUtility.UrlEncode(text);
        }

        /// <summary>
        /// Url Decodes a string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UrlDecode(this string text)
        {
            return System.Net.WebUtility.UrlDecode(text);
        }

        /// <summary>
        /// Attempts to identify the string as a boolean.  This will check the true cases, anything else returns
        /// a false.  True is identified as "true", "yes", "on", "1", "y", "t"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string value)
        {
            switch (value.ToLower())
            {
                case "true":
                case "yes":
                case "on":
                case "1":
                case "y":
                case "t":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns a DateTime if the string is a DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            DateTime.TryParse(value, out DateTime dt);
            return dt;
        }

        /// <summary>
        /// Returns a DateTime if the string is in a DateTime format.  If the string is not a DateTime 
        /// it will return the provided default value parameter instead.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value, DateTime defaultValue)
        {
            if (DateTime.TryParse(value, out var dt))
            {
                return dt;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns a DateTime if the string is in a DateTime format.  If not, it will attempt to create a
        /// new DateTime from the defaultValue.  If that fails an exception will be thrown.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value, string defaultValue)
        {
            if (DateTime.TryParse(value, out var dt))
            {
                return dt;
            }
            else
            {
                return ToDateTime(defaultValue);
            }
        }


        /// <summary>
        /// Converts a string to a secure string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static SecureString ConvertToSecureString(this string str)
        {
            var secureStr = new SecureString();

            foreach (char c in str.ToCharArray())
            {
                secureStr.AppendChar(c);
            }

            return secureStr;
        }

        /// <summary>
        /// Whether the string is a valid DateTime.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return (DateTime.TryParse(input, out DateTime dt));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Converts a string into it's hexadecimal representation.
        /// </summary>
        /// <param name="value">String to turn into hexadecimal.</param>
        /// <param name="includeSpace">Whether to include space in between the output values for display purposes.</param>
        /// <returns></returns>
        public static string ToHexadecimal(this string value, bool includeSpace)
        {
            var sb = new StringBuilder();
            var values = value.ToCharArray();

            foreach (char letter in values)
            {
                int hex = Convert.ToInt32(letter);

                if (includeSpace)
                {
                    sb.AppendFormat("{0:x} ", hex);
                }
                else
                {
                    sb.AppendFormat("{0:x}", hex);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a hexadecimal string into it's string representation.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string FromHexadecimal(this string hex)
        {
            var sb = new StringBuilder();
            var hexValuesSplit = hex.Split(' ');

            foreach (string item in hexValuesSplit)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }

                // Convert the number expressed in base-16 to an integer.
                int value = Convert.ToInt32(item, 16);

                // Get the character corresponding to the integral value.
                string stringValue = char.ConvertFromUtf32(value);

                sb.Append(stringValue);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a string into binary.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="includeSpace">Whether to include space in between the output values for display purposes.</param>
        /// <returns></returns>
        public static string ToBinary(this string value, bool includeSpace)
        {
            var sb = new StringBuilder();

            foreach (char c in value.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));

                if (includeSpace)
                {
                    sb.Append(" ");
                }

            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a string from binary into text.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FromBinary(this string value)
        {
            value = value.Replace(" ", "");

            var byteList = new List<byte>();

            for (int i = 0; i < value.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(value.Substring(i, 8), 2));
            }

            return Encoding.UTF8.GetString(byteList.ToArray());
        }

        /// <summary>
        /// Safely checks if a string is null or empty.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Safely checks if a string is null, empty or white space.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Pick off one argument from a string and return a tuple
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Tuple where Item1 is the first word and Item2 is the remainder</returns>
        /// <remarks>Was formerly known as one_argument</remarks>
        public static Tuple<string, string> FirstArgument(this string value)
            => new Tuple<string, string>(value.FirstWord(), value.RemoveWord(1));

        /// <summary>
        /// Returns a list of words as defined by splitting on a space char.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<string> ToWords(this string value)
        {
            return value.Split(' ').ToList();
        }

        /// <summary>
        /// Gets the first word in the string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FirstWord(this string value)
        {
            return value.ParseWord(1, " ");
        }

        /// <summary>
        /// Gets the second word in the string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SecondWord(this string value)
        {
            return value.ParseWord(2, " ");
        }

        /// <summary>
        /// Gets the third word in the string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ThirdWord(this string value)
        {
            return value.ParseWord(3, " ");
        }

        /// <summary>
        /// Parses the given word from the string
        /// </summary>
        public static string ParseWord(this string value, int wordNumber, string delimiter)
        {
            var strArray = value.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return strArray.Length >= wordNumber ? strArray[wordNumber - 1] : string.Empty;
        }

        /// <summary>
        /// Removes the word from the string at the given index
        /// </summary>
        public static string RemoveWord(this string value, int wordNumber)
        {
            var strArray = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var newString = string.Empty;
            int count = 0;

            foreach (string str in strArray)
            {
                if (count == wordNumber - 1)
                {
                    count++;
                    continue;
                }

                newString += str + " ";
                count++;
            }

            // strip the last space off the end
            if (!string.IsNullOrEmpty(newString))
            {
                newString = newString.Substring(0, newString.Length - 1);
            }

            return newString;
        }

        /// <summary>
        /// Whether or not the string contains a number anywhere in it's contents.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ContainsNumber(this string str)
        {
            foreach (char c in str)
            {
                if (char.IsNumber(c))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Searches through a string array and determines if any item contains the search string.
        /// </summary>
        public static bool AnyContains(this string[] list, string search)
        {
            if (list == null)
            {
                return false;
            }

            foreach (string item in list)
            {
                if (item.ToLower().Contains(search))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Searches through a string array and determines if any item starts with the search string.
        /// </summary>
        public static bool AnyStartsWith(this string[] list, string search)
        {
            if (list == null)
            {
                return false;
            }

            foreach (string item in list)
            {
                if (item.ToLower().StartsWith(search))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Searches through a string array and determines if any item matches the search string.
        /// </summary>
        public static bool AnyStartsEquals(this string[] list, string search)
        {
            if (list == null)
            {
                return false;
            }

            foreach (string item in list)
            {
                if (item.ToLower().Equals(search))
                {
                    return true;
                }
            }

            return false;
        }

    }

}