﻿/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2008-01-12
 * @last updated      : 2025-05-09
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

using Argus.Cryptography;
using Cysharp.Text;
using System.ComponentModel;
using System.Globalization;
using System.IO.Compression;
using System.Security;
using System.Threading.Tasks;

namespace Argus.Extensions
{
    /// <summary>
    /// String extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// This function will return the specified amount of characters from the left hand side of the string. This is the equivalent of the Visual Basic Left function.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        public static string Left(this string str, int length)
        {
            if (length >= str.Length)
            {
                return str;
            }

            return str.AsSpan(0, length).ToString();
        }

        /// <summary>
        /// This function will return the specified amount of characters from the right hand side of the string. This is the equivalent of the Visual Basic Right function.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        public static string Right(this string str, int length)
        {
            if (length >= str.Length)
            {
                return str;
            }

            return str.AsSpan(str.Length - length, length).ToString();
        }

        /// <summary>
        /// Returns the specified number of characters from the left hand side of the string.  If the number asked for is longer the
        /// string then the entire string is returned without an exception.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        public static string SafeLeft(this string? str, int length)
        {
            if (string.IsNullOrEmpty(str) || length <= 0)
            {
                return "";
            }

            if (length >= str.Length)
            {
                return str;
            }

            // Left uses spans
            return Left(str, length);
        }

        /// <summary>
        /// Returns the specified number of characters from the right hand side of the string.  If the number asked for is longer the
        /// string then the entire string is returned without an exception.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        public static string SafeRight(this string? str, int length)
        {
            if (string.IsNullOrEmpty(str) || length <= 0)
            {
                return "";
            }

            if (length >= str.Length)
            {
                return str;
            }

            // Right uses spans
            return Right(str, length);
        }

        /// <summary>
        /// Retrieves a substring from this instance with error checking to prevent exceptions.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        public static string SafeSubstring(this string? str, int startIndex)
        {
            if (string.IsNullOrEmpty(str) || startIndex > str.Length)
            {
                return "";
            }

            return startIndex < 0 ? str : str.Substring(startIndex);
        }

        /// <summary>
        /// Retrieves a substring from this instance with error checking to prevent exceptions.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public static string SafeSubstring(this string? str, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(str) || startIndex > str.Length || length <= 0)
            {
                return "";
            }

            // We have a good start index but that index plus the requested length is longer than the string
            // so we will just return the string from that point.
            if (startIndex + length > str.Length)
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
        /// Reports the zero based index of the first occurrence of a matching string.
        /// </summary>
        /// <param name="str">The string to search.</param>
        /// <param name="value">The string to search for.</param>
        /// <param name="startIndex"></param>
        /// <returns>
        /// Returns the zero based index or a -1 if the string isn't found or the startIndex is greater than the length of the string.
        /// </returns>
        public static int SafeIndexOf(this string? str, string value, int startIndex)
        {
            if (str == null)
            {
                return -1;
            }

            if (startIndex > str.Length - 1)
            {
                return -1;
            }

            return str.IndexOf(value, startIndex, StringComparison.Ordinal);
        }

        /// <summary>
        /// Reports the zero based index of the first occurrence of a matching string.
        /// </summary>
        /// <param name="str">The string to search.</param>
        /// <param name="value">The string to search for.</param>
        /// <param name="startIndex"></param>
        /// <param name="length">The number of positions to examine.</param>
        /// <returns>
        /// Returns the zero based index or a -1 if the string isn't found or the startIndex is greater than the length of the string.
        /// </returns>
        public static int SafeIndexOf(this string? str, string value, int startIndex, int length)
        {
            if (str == null)
            {
                return -1;
            }

            if (startIndex > str.Length - 1 || startIndex + length > str.Length - 1)
            {
                return -1;
            }

            return str.IndexOf(value, startIndex, length, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns the index of the specified <see cref="char" />.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <param name="startIndex"></param>
        /// <returns>
        /// Returns the zero based index or a -1 if the char isn't found or the startIndex is greater than the length of the string.
        /// </returns>
        public static int SafeIndexOf(this string? str, char c, int startIndex)
        {
            if (str == null || startIndex > str.Length - 1)
            {
                return -1;
            }

            return str.IndexOf(c, startIndex);
        }

        /// <summary>
        /// Returns true if the specified <see cref="char" /> is found at the start of the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        public static bool SafeStartsWith(this string? str, char c)
        {
            return !string.IsNullOrEmpty(str) && str.StartsWith(c);
        }

        /// <summary>
        /// Returns true if the specified <see cref="char" /> is found at the end of the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        public static bool SafeEndsWith(this string? str, char c)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            return str.Length > 0 && str[str.Length - 1].Equals(c);
        }

        /// <summary>
        /// Simulates the same functionality provide by the traditional 1 based index Mid function.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startPos"></param>
        /// <param name="length"></param>
        public static string Mid(this string str, int startPos, int length)
        {
            return str.Substring(startPos - 1, length);
        }

        /// <summary>
        /// Deletes the specified number of characters from the left hand side of the string.  If the number to delete is longer than
        /// the length of the string then a blank string will be returned.  If the string provided is null a blank string will be returned.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        public static string DeleteLeft(this string str, int length)
        {
            if (string.IsNullOrEmpty(str) || length >= str.Length)
            {
                return string.Empty;
            }

            var span = str.AsSpan();
            var resultSpan = span.Slice(length);
            return resultSpan.ToString();
        }

        /// <summary>
        /// Deletes the specified number of characters from the right hand side of the string.  If the number to delete is longer than
        /// the length of the string then a blank string will be returned.  If the string provided is null a blank string will be returned.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        public static string DeleteRight(this string str, int length)
        {
            if (string.IsNullOrEmpty(str) || length >= str.Length)
            {
                return string.Empty;
            }

            var span = str.AsSpan();
            var resultSpan = span.Slice(0, span.Length - length);
            return resultSpan.ToString();
        }

        /// <summary>
        /// Removes all line endings from a string.
        /// </summary>
        /// <param name="s"></param>
        public static string RemoveLineEndings(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            #if NETSTANDARD2_1 || NET5_0_OR_GREATER
                // stackalloc with small strings helps to avoid heap allocation
                Span<char> buffer = s.Length <= 1024 
                    ? stackalloc char[s.Length] 
                    : new char[s.Length];
                
                int length = 0;
                
                foreach (char c in s)
                {
                    if (c != '\r' && c != '\n')
                    {
                        buffer[length++] = c;
                    }
                }
                
                return new string(buffer.Slice(0, length));
            #else
                int len = s.Length;
                var output = new char[len];
                int i2 = 0;

                for (int i = 0; i < len; i++)
                {
                    char c = s[i];

                    if (c != '\r' && c != '\n')
                    {
                        output[i2++] = c;
                    }
                }

                return new string(output, 0, i2);
            #endif
        }

        /// <summary>
        /// Returns a string that is truncated at a given length amount.  Ellipses are then added on at the end but only if
        /// the string needs to be trimmed.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        public static string? TrimLengthWithEllipses(this string? value, int length)
        {
            if (value == null)
            {
                return value;
            }

            if (value.Length > length)
            {
                value = $"{Left(value, length)}...";
            }

            return value;
        }

        /// <summary>
        /// Removes all trailing occurrences of the specified string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimStr"></param>
        public static string TrimEnd(this string str, string trimStr)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(trimStr))
            {
                return str;
            }

            var span = str.AsSpan();

            while (span.Length >= trimStr.Length && span.Slice(span.Length - trimStr.Length).SequenceEqual(trimStr.AsSpan()))
            {
                span = span.Slice(0, span.Length - trimStr.Length);
            }

            return span.ToString();
        }

        /// <summary>
        /// Removes all leading occurrences of the specified string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimStr"></param>
        public static string TrimStart(this string str, string trimStr)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(trimStr))
            {
                return str;
            }

            var span = str.AsSpan();

            while (span.StartsWith(trimStr.AsSpan()))
            {
                span = span.Slice(trimStr.Length);
            }

            return span.ToString();
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of the specified string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimStr"></param>
        public static string Trim(this string str, string trimStr)
        {
            var strSpan = str.AsSpan();
            var trimSpan = trimStr.AsSpan();

            while (strSpan.StartsWith(trimSpan))
            {
                strSpan = strSpan.Slice(trimSpan.Length);
            }

            while (strSpan.EndsWith(trimSpan))
            {
                strSpan = strSpan.Slice(0, strSpan.Length - trimSpan.Length);
            }

            return strSpan.ToString();
        }

        /// <summary>
        /// Trims whitespace from the beginning and ending of each line in a string if it contains multiple lines.
        /// </summary>
        /// <param name="buf"></param>
        public static string TrimEachLineWhitespace(this string buf)
        {
            var sb = new StringBuilder();

            foreach (string line in buf.Split('\n'))
            {
                sb.AppendLine(line.Trim());
            }

            // If the original string didn't have a newline at the end, remove the final \r\n
            if (!buf.EndsWith(Environment.NewLine))
            {
                sb.TrimEnd('\r', '\n');
            }

            return sb.ToString();
        }

        /// <summary>
        /// Determines whether a string is a numeric value.  This implementation uses Decimal.TryParse to produce it's value.
        /// </summary>
        /// <param name="str"></param>
        public static bool IsNumeric(this string str)
        {
            return decimal.TryParse(str, out decimal result);
        }

        /// <summary>
        /// Inserts a string after given number of characters specified in the interval property.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="textToInsert">The string to insert at the specified interval.  This could be a line break, an HTML tag, etc.</param>
        /// <param name="interval">The N'th number of characters to insert the textToInsert value at.</param>
        public static string InsertAtInterval(this string value, string textToInsert, int interval)
        {
            var sb = new StringBuilder();

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
        public static string RemoveNonNumericCharacters(this string value, bool allowPeriod)
        {
            if (allowPeriod == false)
            {
                return RemoveNonNumericCharacters(value);
            }

            return Regex.Replace(value, "[^0-9|^.]", "");
        }

        /// <summary>
        /// Removes non-numeric characters from a string.  An option is available to allow for a period and/or comma in case this is used with
        /// a money value or number that requires a decimal place or a value that requires keeping it's formatting with commas and periods only.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allowPeriod">If true, any periods will be left, if false, all periods will also be removed.</param>
        /// <param name="allowComma">If true, any commas will be left, if false, all commas will also be removed.</param>
        public static string RemoveNonNumericCharacters(this string value, bool allowPeriod, bool allowComma)
        {
            if (allowComma && allowPeriod)
            {
                return Regex.Replace(value, "[^0-9|^.|^,]", "");
            }

            if (allowPeriod && !allowComma)
            {
                return RemoveNonNumericCharacters(value, true);
            }

            if (allowComma && !allowPeriod)
            {
                return Regex.Replace(value, "[^0-9|^,]", "");
            }

            return RemoveNonNumericCharacters(value);
        }

        /// <summary>
        /// Returns the title case for the string.
        /// </summary>
        /// <param name="value"></param>
        public static string ToTitleCase(this string value)
        {
            var ti = new CultureInfo("en-US", false).TextInfo;

            return ti.ToTitleCase(value);
        }

        /// <summary>
        /// Inverts the case of each character in a string.
        /// </summary>
        /// <param name="value"></param>
        public static string ToInvertCase(this string value)
        {
            var sb = new StringBuilder();

            foreach (char c in value)
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
        public static string ToRandomCase(this string value)
        {
            var sb = new StringBuilder();
            var rnd = new Random();

            foreach (char c in value)
            {
                sb.Append(rnd.Next(1, 100) > 50 ? char.ToLower(c) : char.ToUpper(c));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes everything from a string that is not a letter or a digit.
        /// </summary>
        /// <param name="s"></param>
        public static string RemoveSpecialCharacters(this string s)
        {
            if (s == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder(s.Length);

            for (int i = 0; i <= s.Length - 1; i++)
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
        public static string RemoveNumericCharacters(this string value)
        {
            return Regex.Replace(value, "[0-9]", "");
        }

        /// <summary>
        /// Removes non ASCII characters via a regular expression.
        /// </summary>
        /// <param name="value"></param>
        public static string RemoveNonAsciiCharacters(this string value)
        {
            return Regex.Replace(value, @"[^\u0000-\u007F]+", string.Empty);
        }

        /// <summary>
        /// Normalizes accent marks that some programs and web-sites replace for common characters.
        /// </summary>
        /// <param name="value"></param>
        public static string NormalizeAccentMarks(this string value)
        {
            if (value.IndexOf('\u2013') > -1)
            {
                value = value.Replace('\u2013', '-');
            }

            if (value.IndexOf('\u2014') > -1)
            {
                value = value.Replace('\u2014', '-');
            }

            if (value.IndexOf('\u2015') > -1)
            {
                value = value.Replace('\u2015', '-');
            }

            if (value.IndexOf('\u2017') > -1)
            {
                value = value.Replace('\u2017', '_');
            }

            if (value.IndexOf('\u2018') > -1)
            {
                value = value.Replace('\u2018', '\'');
            }

            if (value.IndexOf('\u2019') > -1)
            {
                value = value.Replace('\u2019', '\'');
            }

            if (value.IndexOf('\u201a') > -1)
            {
                value = value.Replace('\u201a', ',');
            }

            if (value.IndexOf('\u201b') > -1)
            {
                value = value.Replace('\u201b', '\'');
            }

            if (value.IndexOf('\u201c') > -1)
            {
                value = value.Replace('\u201c', '\"');
            }

            if (value.IndexOf('\u201d') > -1)
            {
                value = value.Replace('\u201d', '\"');
            }

            if (value.IndexOf('\u201e') > -1)
            {
                value = value.Replace('\u201e', '\"');
            }

            if (value.IndexOf('\u2026') > -1)
            {
                value = value.Replace("\u2026", "...");
            }

            if (value.IndexOf('\u2032') > -1)
            {
                value = value.Replace('\u2032', '\'');
            }

            if (value.IndexOf('\u2033') > -1)
            {
                value = value.Replace('\u2033', '\"');
            }

            return value;
        }

        /// <summary>
        /// Removes blank lines including blank lines that just have whitespace on them.
        /// </summary>
        /// <param name="value"></param>
        public static string RemoveBlankLines(this string value)
        {
            return Regex.Replace(value, @"^\s*$[\n]*", string.Empty, RegexOptions.Multiline);
        }

        /// <summary>
        /// Creates a string repeated multiple times.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="repeatCount">Number of times to repeat the string</param>
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
        public static string PadRightToLength(this string buf, int length)
        {
            return string.Format("{0,-" + length + "}", buf);
        }

        /// <summary>
        /// Adds a value onto the end of the string if it does not already exist there.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="value"></param>
        public static string AddRightIfDoesntExist(this string buf, string value)
        {
            if (buf.SafeRight(value.Length) != value)
            {
                return $"{buf}{value}";
            }

            return buf;
        }

        /// <summary>
        /// Adds a value onto the beginning of a string if it does not already exist there.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="value"></param>
        public static string AddLeftIfDoesntExist(this string buf, string value)
        {
            if (buf.SafeLeft(value.Length) != value)
            {
                return $"{value}{buf}";
            }

            return buf;
        }

        /// <summary>
        /// Returns an empty string if the current string object is null.
        /// </summary>
        /// <param name="str"></param>
        public static string DefaultIfNull(this string str)
        {
            return str ?? string.Empty;
        }

        /// <summary>
        /// Returns the specified default value if the current string object is null.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        public static string DefaultIfNull(this string str, string defaultValue)
        {
            if (str != null)
            {
                return str;
            }

            return defaultValue ?? string.Empty;
        }

        /// <summary>
        /// Returns the specified default value if the current string object is null or empty.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        public static object DefaultIfNullOrEmpty(this string str, string defaultValue)
        {
            if (string.IsNullOrEmpty(defaultValue))
            {
                defaultValue = "";
            }

            return string.IsNullOrEmpty(str) ? defaultValue : str;
        }

        /// <summary>
        /// Indicates whether the regular expression finds a match in the current string object for the specified pattern.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="regExPattern"></param>
        public static bool IsMatch(this string str, string regExPattern)
        {
            return new Regex(regExPattern).IsMatch(str);
        }

        /// <summary>
        /// Indicates whether the string is a valid regular expression pattern.
        /// </summary>
        /// <param name="pattern"></param>
        public static bool IsValidRegex(this string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return false;
            }

            try
            {
                _ = Regex.Match("", pattern);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Indicates whether the string is a valid regular expression pattern.  If the pattern is
        /// not valid the exceptionText out parameter will be populated with the syntax error.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="exceptionText"></param>
        public static bool IsValidRegex(this string pattern, out string exceptionText)
        {
            try
            {
                _ = Regex.Match("", pattern);
            }
            catch (Exception ex)
            {
                exceptionText = ex.Message;

                return false;
            }

            exceptionText = "";

            return true;
        }

        /// <summary>
        /// Will take a string and split its contents into a list based off of a provided delimiter.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="delimiter"></param>
        public static List<string> ToList(this string buf, string delimiter)
        {
            var array = buf.Split(delimiter.ToCharArray());

            return array.ToList();
        }

        /// <summary>
        /// Will take a string and split its contents into a list based off of multiple delimiters that are provided.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="delimiter"></param>
        public static List<string> ToList(this string buf, string[] delimiter)
        {
            string firstDelim = "";

            if (delimiter.Any())
            {
                firstDelim = delimiter[0];
            }

            // Convert all delimiter characters into the same character
            foreach (string delim in delimiter)
            {
                buf = buf.Replace(delim, firstDelim);
            }

            var array = buf.Split(firstDelim.ToCharArray());

            return array.ToList();
        }

        /// <summary>
        /// Will take a string and split its contents into a string array based off of a provided delimiter.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="delimiter"></param>
        public static string[] ToArray(this string buf, string delimiter)
        {
            return buf.Split(delimiter.ToCharArray());
        }

        /// <summary>
        /// Will take a string and split its contents into a list based off of multiple delimiter that are provided.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="delimiter"></param>
        public static string[] ToArray(this string buf, string[] delimiter)
        {
            string firstDelim = "";

            if (delimiter.Any())
            {
                firstDelim = delimiter[0];
            }

            // Convert all delimiter characters into the same character
            foreach (string delim in delimiter)
            {
                buf = buf.Replace(delim, firstDelim);
            }

            return buf.Split(firstDelim.ToCharArray());
        }

        /// <summary>
        /// An extension method that replaces the first occurrence of a specified string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="searchText"></param>
        /// <param name="replaceText"></param>
        public static string ReplaceFirst(this string str, string searchText, string replaceText)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }

            int pos = str.IndexOf(searchText, StringComparison.Ordinal);

            if (pos < 0)
            {
                return str;
            }

            return str.Substring(0, pos) + replaceText + str.Substring(pos + searchText.Length);
        }

        /// <summary>
        /// An extension method that replaces the last occurrence of a specified string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="searchText"></param>
        /// <param name="replaceText"></param>
        public static string ReplaceLast(this string str, string searchText, string replaceText)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }

            int pos = str.LastIndexOf(searchText, StringComparison.Ordinal);

            if (pos < 0)
            {
                return str;
            }

            return str.Substring(0, pos) + replaceText + str.Substring(pos + searchText.Length);
        }

        /// <summary>
        /// Returns the string in between two markers.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="beginMarker">The beginning marker, such as a single or double quote.</param>
        /// <param name="endMarker">The end marker, such as a single or double quote.</param>
        /// <param name="includeMarkers">Whether or not to include the markers on each item in the list returned.</param>
        /// <returns>A list of string values that were found in between quotes in the original string.</returns>
        public static List<string> GetStringInBetween(this string searchText, string beginMarker, string endMarker, bool includeMarkers)
        {
            string exp = string.Format("{0}([^{0}^{1}]*){1}", Regex.Escape(beginMarker), Regex.Escape(endMarker));
            var returnList = new List<string>();

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
        public static List<string> GetStringInBetween(this string searchText, string marker, bool includeMarkers)
        {
            return GetStringInBetween(searchText, marker, marker, includeMarkers);
        }

        /// <summary>
        /// A string value wrapped by a specified character at the beginning and end.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="wrapCharacter"></param>
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
        public static string ToWrappedString(this string str, string startWrapCharacter, string endWrapCharacter)
        {
            return $"{startWrapCharacter}{str}{endWrapCharacter}";
        }

        /// <summary>
        /// Converts the string into a Base64 string.  UTF8 is used by default for the encoding as supported by the portable class library.
        /// </summary>
        /// <param name="buf"></param>
        public static string ToBase64(this string buf)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(buf));
        }

        /// <summary>
        /// Converts a string into a Base64 string.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="enc">The encoding to use for the Base64 conversion.</param>
        public static string ToBase64(this string buf, Encoding enc)
        {
            return Convert.ToBase64String(enc.GetBytes(buf));
        }

        /// <summary>
        /// Converts the string from a Base64 string to a string.  UTF8 is used by default for the encoding as supported by the portable class library.
        /// </summary>
        /// <param name="buf"></param>
        public static string FromBase64(this string buf)
        {
            var b = Convert.FromBase64String(buf);

            return Encoding.UTF8.GetString(b, 0, b.Length);
        }

        /// <summary>
        /// Converts a Base64 string into a string.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="enc">The encoding to use for the Base64 conversion.</param>
        public static string FromBase64(this string buf, Encoding enc)
        {
            return enc.GetString(Convert.FromBase64String(buf));
        }

        /// <summary>
        /// Truncates the string after the first occurrence of the character or string provided.
        /// </summary>
        /// <param name="str">The source string.</param>
        /// <param name="searchFor">The marker to use to start the truncation.</param>
        /// <param name="keepSearchString">Whether or not to keep the searchFor marker in the return string.</param>
        public static string TruncateAfter(this string str, string searchFor, bool keepSearchString)
        {
            if (str.Contains(searchFor) == false)
            {
                return str;
            }

            if (keepSearchString)
            {
                return str.Substring(0, str.IndexOf(searchFor, StringComparison.Ordinal) + searchFor.Length);
            }

            return str.Substring(0, str.IndexOf(searchFor, StringComparison.Ordinal));
        }

        /// <summary>
        /// Truncates the string before the first occurrence of the character or string provided.
        /// </summary>
        /// <param name="str">The source string.</param>
        /// <param name="searchFor">The marker to use to start the truncation.</param>
        /// <param name="keepSearchString">Whether or not to keep the searchFor marker in the return string.</param>
        public static string TruncateBefore(this string str, string searchFor, bool keepSearchString)
        {
            if (str.Contains(searchFor) == false)
            {
                return str;
            }

            if (keepSearchString)
            {
                return str.Substring(str.IndexOf(searchFor, StringComparison.Ordinal), str.Length - str.IndexOf(searchFor, StringComparison.Ordinal));
            }

            return str.Substring(str.IndexOf(searchFor, StringComparison.Ordinal) + searchFor.Length, str.Length - str.IndexOf(searchFor, StringComparison.Ordinal) - searchFor.Length);
        }

        /// <summary>
        /// Truncates a string after a specified position.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <param name="suffix">An optional string to append after the truncated position.</param>
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
        public static string FormatIfNumber(this string str)
        {
            if (str.IsNumeric())
            {
                double x = Convert.ToDouble(str);

                return x.ToString("#,0");
            }

            return str;
        }

        /// <summary>
        /// Formats a number with commas and the specified number of decimal places.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="decimalPlaces"></param>
        public static string FormatIfNumber(this string str, int decimalPlaces)
        {
            if (IsNumeric(str))
            {
                string formatString = "#,0.";

                formatString = formatString.PadRight(formatString.Length + decimalPlaces, '0');

                double x = Convert.ToDouble(str);

                return x.ToString(formatString);
            }

            return str;
        }

        /// <summary>
        /// Returns a string array that contains the substrings in this instance that are delimited by elements of a specified Unicode string.  The
        /// string will be converted into a character array and passed on to the underlying String.Split method provided by the .Net Framework.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="delimiter"></param>
        public static string[] SplitPcl(this string str, string delimiter)
        {
            return str.Split(delimiter.ToCharArray());
        }

        /// <summary>
        /// Returns all words in a string that start with a specified character (to return hashtags or twitter handles for instance).  This uses a regular expression
        /// that return words so it's looking for characters, for example if you were looking for @blakepell or #test.  It would not correctly work with #this.is.a.test.
        /// </summary>
        /// <param name="parentText">The text to search.</param>
        /// <param name="wordToSearchFor">This is the character or the start of the word to search for, e.g. #, @</param>
        public static List<string> GetWordsThatStartWithWord(this string parentText, string wordToSearchFor)
        {
            var lst = new List<string>();

            foreach (Match match in Regex.Matches(parentText, string.Format("(?<!\\w){0}\\w+", wordToSearchFor)))
            {
                lst.Add(match.Value);
            }

            return lst;
        }

        /// <summary>
        /// Converts a string into a MemoryStream using the specified encoding.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The encoding to use with the string.</param>
        public static MemoryStream ToMemoryStream(this string str, Encoding enc)
        {
            return new MemoryStream(enc.GetBytes(str));
        }

        /// <summary>
        /// Converts a string into a MemoryStream.
        /// </summary>
        /// <param name="buf"></param>
        public static MemoryStream ToMemoryString(this string buf)
        {
            var ms = new MemoryStream();

            using (var sw = new StreamWriter(ms))
            {
                sw.Write(buf);
                sw.Flush();
            }

            return ms;
        }

        /// <summary>
        /// Determines if the string is a guid format (via Guid.TryParse).
        /// </summary>
        /// <param name="str"></param>
        public static bool IsGuid(this string str)
        {
            if (str != null && Guid.TryParse(str, out _))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if the string is a guid format and additionally is one that has dashes.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="requireDashes">Whether or not the potential guid should be validated to include dashes.</param>
        public static bool IsGuid(this string str, bool requireDashes)
        {
            if (requireDashes)
            {
                return str.Contains('-') && IsGuid(str);
            }

            return IsGuid(str);
        }

        /// <summary>
        /// Normalizes line endings (makes \n\r -> \r\n, makes \n\n, \r\n\r\n) etc.
        /// </summary>
        /// <param name="str"></param>
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
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            // Length will be greater than 0 if it gets here.
            var chars = input.ToCharArray();

            if (char.IsLower(chars[0]))
            {
                chars[0] = char.ToUpper(chars[0]);
            }

            return new string(chars);
        }

        /// <summary>
        /// HTML encodes a string.
        /// </summary>
        /// <param name="text"></param>
        public static string HtmlEncode(this string text)
        {
            return WebUtility.HtmlEncode(text);
        }

        /// <summary>
        /// Html Decodes a string
        /// </summary>
        /// <param name="text"></param>
        public static string HtmlDecode(this string text)
        {
            return WebUtility.HtmlDecode(text);
        }

        /// <summary>
        /// Url Encodes a string.
        /// </summary>
        /// <param name="text"></param>
        public static string? UrlEncode(this string text)
        {
            return WebUtility.UrlEncode(text);
        }

        /// <summary>
        /// Url Decodes a string.
        /// </summary>
        /// <param name="text"></param>
        public static string? UrlDecode(this string text)
        {
            return WebUtility.UrlDecode(text);
        }

        /// <summary>
        /// Attempts to identify the string as a boolean.  This will check the true cases, anything else returns
        /// a false.  True is identified as "true", "yes", "on", "1", "y", "t", "checked"
        /// </summary>
        /// <param name="value"></param>
        public static bool ToBoolean(this string value)
        {
            switch (value.ToLower())
            {
                case "true":
                case "yes":
                case "on":
                case "1":
                case "y":
                case "t:":
                case "checked":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns a DateTime if the string is a DateTime
        /// </summary>
        /// <param name="value"></param>
        public static DateTime ToDateTime(this string value)
        {
            DateTime.TryParse(value, out var dt);

            return dt;
        }

        /// <summary>
        /// Returns a DateTime if the string is in a DateTime format.  If the string is not a DateTime
        /// it will return the provided default value parameter instead.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public static DateTime ToDateTime(this string value, DateTime defaultValue)
        {
            if (DateTime.TryParse(value, out var dt))
            {
                return dt;
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns a DateTime if the string is in a DateTime format.  If not, it will attempt to create a
        /// new DateTime from the defaultValue.  If that fails an exception will be thrown.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public static DateTime ToDateTime(this string value, string defaultValue)
        {
            if (DateTime.TryParse(value, out var dt))
            {
                return dt;
            }

            return ToDateTime(defaultValue);
        }

        /// <summary>
        /// Converts a string to a secure string.
        /// </summary>
        /// <param name="str"></param>
        public static SecureString ConvertToSecureString(this string str)
        {
            var secureStr = new SecureString();

            foreach (char c in str)
            {
                secureStr.AppendChar(c);
            }

            return secureStr;
        }

        /// <summary>
        /// Converts a string into it's hexadecimal representation.
        /// </summary>
        /// <param name="value">String to turn into hexadecimal.</param>
        /// <param name="includeSpace">Whether to include space in between the output values for display purposes.</param>
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
        public static string ToBinary(this string value, bool includeSpace)
        {
            var sb = new StringBuilder();

            foreach (char c in value)
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
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Safely checks if a string is null, empty or white space.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsNullOrEmptyOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        ///// <summary>
        ///// Pick off one argument from a string and return a tuple
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns>Tuple where Item1 is the first word and Item2 is the remainder</returns>
        ///// <remarks>Was formerly known as one_argument</remarks>
        //public static Tuple<string, string> FirstArgument(this string value)
        //{
        //    return new Tuple<string, string>(value.FirstWord(), value.RemoveWord(1));
        //}

        /// <summary>
        /// Returns a list of words as defined by splitting on a space char.
        /// </summary>
        /// <param name="value"></param>
        public static List<string> ToWords(this string value)
        {
            return value.Split(' ').ToList();
        }

        /// <summary>
        /// Gets the first word in the string
        /// </summary>
        /// <param name="value"></param>
        public static string FirstWord(this string value)
        {
            return value.ParseWord(1, ' ');
        }

        /// <summary>
        /// Gets the second word in the string
        /// </summary>
        /// <param name="value"></param>
        public static string SecondWord(this string value)
        {
            return value.ParseWord(2, ' ');
        }

        /// <summary>
        /// Gets the third word in the string
        /// </summary>
        /// <param name="value"></param>
        public static string ThirdWord(this string value)
        {
            return value.ParseWord(3, ' ');
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
        /// Parses the given word from the string
        /// </summary>
        public static string ParseWord(this string value, int wordNumber, char delimiter = ' ')
        {
            var span = value.AsSpan();
            int count = 1;

            while (span.Length > 0)
            {
                var word = span.SplitNext(delimiter);

                // Skip empty words, this kind of mimics StringSplitOptions.RemoveEmptyEntries.
                if (word.Length == 0)
                {
                    continue;
                }

                if (count == wordNumber)
                {
                    return word.ToString();
                }

                count++;
            }

            // Length was zero, return an empty string.
            return string.Empty;
        }

        /// <summary>
        /// Returns a word at the given index (0 index).  A null is returned if the index exceeds the number of words.
        /// </summary>
        /// <param name="s">The input string.</param>
        /// <param name="index">The 0 based index of the requested word.</param>
        /// <param name="delimiter">The delimiter to split the input on.  The default value is ' '</param>
        public static string? Word(this ReadOnlySpan<char> s, int index, char delimiter = ' ')
        {
            for (int i = 0, word = 0; i < s.Length; i++)
            {
                if (s[i] == delimiter)
                {
                    word++;
                }
                else if (word == index)
                {
                    int start = i;

                    while (i < s.Length && s[i] != delimiter)
                    {
                        i++;
                    }

                    return s.Slice(start, i - start).ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a word at the given index (0 index).  A null is returned if the index exceeds the number of words.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="index"></param>
        /// <param name="delimiter"></param>
        public static string? Word(this string s, int index, char delimiter = ' ')
        {
            return Word(s.AsSpan(), index, delimiter);
        }

        /// <summary>
        /// Returns a word at the given index (0 index).  A null is returned if the index exceeds the number of words.fs
        /// </summary>
        /// <param name="s"></param>
        /// <param name="index"></param>
        /// <param name="delimiter"></param>
        public static ReadOnlySpan<char> WordAsSpan(this ReadOnlySpan<char> s, int index, char delimiter = ' ')
        {
            for (int i = 0, word = 0; i < s.Length; i++)
            {
                if (s[i] == delimiter)
                {
                    word++;
                }
                else if (word == index)
                {
                    int start = i;

                    while (i < s.Length && s[i] != delimiter)
                    {
                        i++;
                    }

                    return s.Slice(start, i - start);
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified string consists only of alphabetic characters (not unicode aware).
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>true if the string consists only of alphabetic characters; otherwise, false.</returns>
        public static bool IsAlpha(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
        
            return str.All(char.IsLetter);
        }

        /// <summary>
        /// Whether an entire string is alphanumeric.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsAlphaNumeric(this string value)
        {
            var span = value.AsSpan();

            foreach (var c in span)
            {
                if (!c.IsLetterOrDigit())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Removes the word from the string at the given index
        /// </summary>
        public static string RemoveWord(this string value, int wordNumber)
        {
            var span = value.AsSpan();

            int count = 1;

            using (var sb = ZString.CreateStringBuilder())
            {
                while (span.Length > 0)
                {
                    var word = span.SplitNext(' ');

                    // Skip empty words, this kind of mimics StringSplitOptions.RemoveEmptyEntries.
                    if (word.Length == 0)
                    {
                        continue;
                    }

                    if (count == wordNumber)
                    {
                        count++;
                        continue;
                    }

                    sb.Append(word);
                    sb.Append(' ');
                    count++;
                }

                // Strip off the last space
                if (sb.Length > 0)
                {
                    return sb.AsSpan().Left(sb.Length - 1).ToString();
                }

                // Length was zero, return an empty string.
                return string.Empty;
            }
        }

        /// <summary>
        /// Removes the word from the string at the given index
        /// </summary>
        public static ReadOnlySpan<char> RemoveWordNewAsSpan(this string value, int wordNumber)
        {
            var span = value.AsSpan();

            int count = 1;

            using (var sb = ZString.CreateStringBuilder())
            {
                while (span.Length > 0)
                {
                    var word = span.SplitNext(' ');

                    // Skip empty words, this kind of mimics StringSplitOptions.RemoveEmptyEntries.
                    if (word.Length == 0)
                    {
                        continue;
                    }

                    if (count == wordNumber)
                    {
                        count++;
                        continue;
                    }

                    sb.Append(word);
                    sb.Append(' ');
                    count++;
                }

                // Strip off the last space
                if (sb.Length > 0)
                {
                    Span<char> spanChar = new char[sb.Length - 1];
                    sb.AsSpan().Left(sb.Length - 1).TryCopyTo(spanChar);
                    return spanChar;
                }

                // Length was zero, return an empty span.
                return ReadOnlySpan<char>.Empty;
            }
        }

        /// <summary>
        /// Returns the word count in the current string accounting for whitespace.
        /// </summary>
        /// <param name="text"></param>
        public static int WordCount(this string text)
        {
            int wordCount = 0;
            int index = 0;

            // Skip whitespace until first word.
            while (index < text.Length && char.IsWhiteSpace(text[index]))
            {
                index++;
            }

            while (index < text.Length)
            {
                // Check if current char is part of a word.
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                {
                    index++;
                }

                wordCount++;

                // Skip whitespace until next word.
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                {
                    index++;
                }
            }

            return wordCount;
        }

        /// <summary>
        /// Whether or not the string contains a number anywhere in it's contents.
        /// </summary>
        /// <param name="str"></param>
        public static bool ContainsNumber(this string str)
        {
            return str.Any(c => char.IsNumber(c));
        }

        /// <summary>
        /// Returns a <see cref="byte" /> array in the specified encoding.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="enc"></param>
        public static byte[] ToBytes(this string value, Encoding enc)
        {
            return enc.GetBytes(value);
        }

        /// <summary>
        /// If the current string starts with a specific <see cref="char" />.  0 length strings return false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        public static bool StartsWith(this string value, char c)
        {
            return value.Length > 0 && value[0].Equals(c);
        }

        /// <summary>
        /// If the current string ends with a specific <see cref="char" />.  0 length strings return false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        public static bool EndsWith(this string value, char c)
        {
            return value.Length > 0 && value[value.Length - 1].Equals(c);
        }


        /// <summary>
        /// Returns a string between the first occurrence of two markers.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="beginMarker"></param>
        /// <param name="endMarker"></param>
        /// <returns></returns>
        public static string Between(this string str, string beginMarker, string endMarker)
        {
            return Between(str.AsSpan(), beginMarker.AsSpan(), endMarker.AsSpan());
        }

        /// <summary>
        /// Returns a string between the first occurrence two markers.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="beginMarker"></param>
        /// <param name="endMarker"></param>
        public static string Between(this ReadOnlySpan<char> span, ReadOnlySpan<char> beginMarker, ReadOnlySpan<char> endMarker)
        {
            int pos1 = span.IndexOf(beginMarker) + beginMarker.Length;
            int pos2 = span.Slice(pos1).IndexOf(endMarker);

            // Nope, we got nothing.
            if (pos2 <= pos1)
            {
                return "";
            }

            return span.Slice(pos1, pos2).ToString();
        }

        /// <summary>
        /// Encrypts a string with AES encryption.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        public static string Encrypt(this string str, string key)
        {
            return Encrypt(str, key, "ijJsJuy%487sDrz&");
        }

        /// <summary>
        /// Decrypts a string with AES encryption.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        public static string Decrypt(this string str, string key)
        {
            // A specific note that the salt is passed in which can be public.
            return Decrypt(str, key, "ijJsJuy%487sDrz&");
        }

        /// <summary>
        /// Encrypts a string with AES encryption.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <param name="salt">A 16 character salt value.</param>
        public static string Encrypt(this string str, string key, string salt)
        {
            // A specific note that the salt is passed in which can be public.
            var crypt = new Encryption(salt);

            return crypt.EncryptToString(str, key);
        }

        /// <summary>
        /// Decrypts a string with AES encryption.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <param name="salt">A 16 character salt value.</param>
        public static string Decrypt(this string str, string key, string salt)
        {
            // A specific note that the salt is passed in which can be public.
            var crypt = new Encryption(salt);

            return crypt.DecryptToString(str, key);
        }

        /// <summary>
        /// Returns the plural form which is provided if the count is 0 or greater than 1.  Otherwise
        /// the string itself is returned which should be the singular form.  This allows you to return
        /// 0 items, 2 items or 1 item depending on the count.
        /// </summary>
        /// <param name="singularForm"></param>
        /// <param name="count"></param>
        /// <param name="pluralForm"></param>
        public static string IfCountPluralize(this string singularForm, int count, string pluralForm)
        {
            // 0 items, 5 items, 1 item
            if (count == 0 || count > 1)
            {
                return pluralForm;
            }

            return singularForm;
        }

        /// <summary>
        /// Converts a camel cased string to snake case. Works for upper and lower camel case.
        /// Example: 'FirstName' to 'first_name'
        /// </summary>
        /// <param name="str"></param>
        public static string CamelToSnakeCase(this string str)
        {
            return Regex.Replace(str, "[A-Z]", "_$0").TrimStart('_').ToLower();
        }

        /// <summary>
        /// Converts a snake case string to camel case. Default is upper camel case.
        /// Example: 'first_name' to 'FirstName'
        /// </summary>
        /// <param name="str"></param>
        /// <param name="lowerFirst">If true, first letter will be left lower case. Default is false.</param>
        public static string SnakeToCamelCase(this string str, bool lowerFirst = false)
        {
            var parts = str.Split('_');
            var sb = new StringBuilder();

            if (!lowerFirst)
            {
                foreach (var p in parts)
                {
                    sb.Append(char.ToUpper(p[0])).Append(p.Substring(1));
                }
            }
            else
            {
                sb.Append(parts[0]);

                for (var i = 1; i < parts.Length; i++)
                {
                    sb.Append(char.ToUpper(parts[i][0])).Append(parts[i].Substring(1));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates a slug from the provided string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength">The length the slug should be truncated at.</param>
        /// <remarks>Valid characters are letters, digits and spaces (which will be converted to dashes).</remarks>
        public static string GenerateSlug(this string str, int maxLength = 128)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            // No white space at the start or end of the string.
            str = str.Trim();

            var builder = new StringBuilder(maxLength);

            for (int i = 0; i <= str.Length - 1; i++)
            {
                if (i > maxLength)
                {
                    break;
                }

                if (char.IsLetterOrDigit(str, i))
                {
                    builder.Append(char.ToLower(str[i]));
                }

                if (str[i] == ' ')
                {
                    builder.Append('-');
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns an <see cref="int"/> via <see cref="int.TryParse(string, out int)" />.  0 is
        /// returned if the value does not convert.
        /// </summary>
        /// <param name="value"></param>
        public static int AsInt(this string value)
        {
            return AsInt(value, 0);
        }

        /// <summary>
        /// Returns an <see cref="int"/> via <see cref="int.TryParse(string, out int)" />.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public static int AsInt(this string value, int defaultValue)
        {
            return Int32.TryParse(value, out int result) ? result : defaultValue;
        }

        /// <summary>
        /// Returns a <see cref="decimal"/> for the specified string.
        /// </summary>
        /// <param name="value"></param>
        public static decimal AsDecimal(this string value)
        {
            return As<Decimal>(value);
        }

        /// <summary>
        /// Returns a <see cref="decimal"/> for the specified string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public static decimal AsDecimal(this string value, decimal defaultValue)
        {
            return As<Decimal>(value, defaultValue);
        }

        /// <summary>
        /// Returns a <see cref="float"/> for the specified string.
        /// </summary>
        /// <param name="value"></param>
        public static float AsFloat(this string value)
        {
            return AsFloat(value, default(float));
        }

        /// <summary>
        /// Returns a <see cref="float"/> for the specified string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public static float AsFloat(this string value, float defaultValue)
        {
            return Single.TryParse(value, out float result) ? result : defaultValue;
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> for the specified string.
        /// </summary>
        /// <param name="value"></param>
        public static DateTime AsDateTime(this string value)
        {
            return AsDateTime(value, default(DateTime));
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> for the specified string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public static DateTime AsDateTime(this string value, DateTime defaultValue)
        {
            return DateTime.TryParse(value, out DateTime result) ? result : defaultValue;
        }

        /// <summary>
        /// Attempts to return a value of the specified generic type converted via <see cref="TypeConverter"/>.  If
        /// a conversion fails the default for the generic type will be returned.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <remarks>This assumes the value will always parse.</remarks>
        public static TValue As<TValue>(this string value)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(TValue));

                if (converter.CanConvertFrom(typeof(string)))
                {
                    return (TValue)converter.ConvertFrom(value);
                }

                // Try the other direction
                converter = TypeDescriptor.GetConverter(typeof(string));

                if (converter.CanConvertTo(typeof(TValue)))
                {
                    return (TValue)converter.ConvertTo(value, typeof(TValue));
                }
            }
            catch { }

            return default;
        }

        /// <summary>
        /// Attempts to return a value of the specified generic type converted via <see cref="TypeConverter"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <remarks>This assumes the value will always parse.</remarks>
        public static TValue As<TValue>(this string value, TValue defaultValue)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(TValue));

                if (converter.CanConvertFrom(typeof(string)))
                {
                    return (TValue)converter.ConvertFrom(value);
                }

                // Try the other direction
                converter = TypeDescriptor.GetConverter(typeof(string));

                if (converter.CanConvertTo(typeof(TValue)))
                {
                    return (TValue)converter.ConvertTo(value, typeof(TValue));
                }
            }
            catch { }

            return defaultValue;
        }

        /// <summary>
        /// If the string will parse as a <see cref="bool"/>.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsBool(this string value)
        {
            return bool.TryParse(value, out bool _);
        }

        /// <summary>
        /// If the string will parse as a <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsInt(this string value)
        {
            return int.TryParse(value, out int _);
        }

        /// <summary>
        /// If the string will parse as a <see cref="decimal"/>.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsDecimal(this string value)
        {
            return decimal.TryParse(value, out decimal _);
        }

        /// <summary>
        /// If the string will parse as a <see cref="float"/>.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsFloat(this string value)
        {
            return float.TryParse(value, out float _);
        }

        /// <summary>
        /// If the string will parse as a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsDateTime(this string value)
        {
            return DateTime.TryParse(value, out DateTime _);
        }

        /// <summary>
        /// Attempts to determine if the value is of the specified generic type via <see cref="TypeConverter"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        public static bool Is<TValue>(this string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(TValue));

            if (converter != null)
            {
                try
                {
                    if ((value == null) || converter.CanConvertFrom(null, value.GetType()))
                    {
                        converter.ConvertFrom(null, CultureInfo.CurrentCulture, value);
                        return true;
                    }
                }
                catch { }
            }

            return false;
        }

        /// <summary>
        /// Returns the string back or a blank string if the provided string was null.
        /// </summary>
        /// <param name="text"></param>
        public static string ToSafeString(this string? text)
        {
            return text ?? string.Empty;
        }

        /// <summary>
        /// Returns the string back or the required default value if the provided string was null.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        public static string ToSafeStringOrDefault(this string? text, string defaultValue)
        {
            return text ?? defaultValue;
        }

#if NET6_0_OR_GREATER || NETSTANDARD2_1

        /// <summary>
        /// Compresses a string with Brotli and encodes it as Base64.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="level"></param>
        /// <remarks>Based on: https://khalidabuhakmeh.com/compress-strings-with-dotnet-and-csharp</remarks>
        public static async Task<string> ToBrotliAsync(this string value, CompressionLevel level = CompressionLevel.Fastest)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            await using var input = new MemoryStream(bytes);
            await using var output = new MemoryStream();
            await using var stream = new BrotliStream(output, level);

            await input.CopyToAsync(stream);
            await stream.FlushAsync();

            var result = output.ToArray();

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Compresses a string with Brotli and encodes it as Base64.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="level"></param>
        public static string ToBrotli(this string value, CompressionLevel level = CompressionLevel.Fastest)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            using var input = new MemoryStream(bytes);
            using var output = new MemoryStream();
            using var stream = new BrotliStream(output, level);

            input.CopyTo(stream);
            stream.Flush();

            var result = output.ToArray();

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Decompresses a Base64 encoded Brotli compressed string.
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>Based on: https://khalidabuhakmeh.com/compress-strings-with-dotnet-and-csharp</remarks>
        public static async Task<string> FromBrotliAsync(this string value)
        {
            var bytes = Convert.FromBase64String(value);
            await using var input = new MemoryStream(bytes);
            await using var output = new MemoryStream();
            await using var stream = new BrotliStream(input, CompressionMode.Decompress);

            await stream.CopyToAsync(output);

            return Encoding.Unicode.GetString(output.ToArray());
        }

        /// <summary>
        /// Decompresses a Base64 encoded Brotli compressed string.
        /// </summary>
        /// <param name="value"></param>
        public static string FromBrotli(this string value)
        {
            var bytes = Convert.FromBase64String(value);
            using var input = new MemoryStream(bytes);
            using var output = new MemoryStream();
            using var stream = new BrotliStream(input, CompressionMode.Decompress);

            stream.CopyTo(output);

            return Encoding.Unicode.GetString(output.ToArray());
        }

#endif

    }
}