/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-11
 * @last updated      : 2021-02-11
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Linq;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="char"/> and char[].
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// Returns the specified amount of characters from the start of the char array as
        /// a new char[].  If a length greater than the original char array is requested an
        /// out of bounds exception will be thrown.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        public static char[] Left(this char[] value, int length)
        {
            var buf = new char[length];

            for (int i = 0; i < length; i++)
            {
                buf[i] = value[i];
            }

            return buf;
        }

        /// <summary>
        /// Returns the specified amount of characters from the start of the char array as
        /// a new char[].  If a length greater than the original char array is requested a
        /// an array returning a copy of the full original array is returned.  If the length
        /// requested is less than zero an empty char[] is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        public static char[] SafeLeft(this char[] value, int length)
        {
            char[] buf;

            if (length < 0)
            {
                return Array.Empty<char>();
            }

            if (value.Length < length)
            {
                buf = new char[value.Length];
                value.CopyTo(buf, 0);

                return buf;
            }

            buf = new char[length];

            for (int i = 0; i < length; i++)
            {
                buf[i] = value[i];
            }

            return buf;
        }

        /// <summary>
        /// Returns the specified amount of characters from the end of the char array as
        /// a new char[].  If a length greater than the original char array is requested an
        /// out of bounds exception will be thrown.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        public static char[] Right(this char[] value, int length)
        {
            var buf = new char[length];
            int startIndex = value.Length - length;
            int x = 0;

            for (int i = startIndex; i < value.Length; i++)
            {
                buf[x++] = value[i];
            }

            return buf;
        }

        /// <summary>
        /// Returns the specified amount of characters from the end of the char array as
        /// a new char[].  If a length greater than the original char array is requested a
        /// an array returning a copy of the full original array is returned.  If the length
        /// requested is less than zero an empty char[] is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        public static char[] SafeRight(this char[] value, int length)
        {
            char[] buf;

            if (length < 0)
            {
                return Array.Empty<char>();
            }

            if (value.Length < length)
            {
                buf = new char[value.Length];
                value.CopyTo(buf, 0);

                return buf;
            }

            buf = new char[length];
            int startIndex = value.Length - length;
            int x = 0;

            for (int i = startIndex; i < value.Length; i++)
            {
                buf[x++] = value[i];
            }

            return buf;
        }

        /// <summary>
        /// Returns the specified segment of characters from the middle of a char array as
        /// a new char[].  No bounds checking is performed.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public static char[] SubChar(this char[] value, int startIndex, int length)
        {
            var buf = new char[length];
            int x = 0;

            for (int i = startIndex; i < startIndex + length; i++)
            {
                buf[x++] = value[i];
            }

            return buf;
        }

        /// <summary>
        /// Returns the specified segment of characters from the middle of a char array as
        /// a new char[].  If a length that exceeds the right bounds is requested a char array
        /// from the start index to the end of the char[] is returned.  If a length less than zero
        /// is requested an empty char[] is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public static char[] SafeSubChar(this char[] value, int startIndex, int length)
        {
            if (length < 0)
            {
                return Array.Empty<char>();
            }
            
            char[] buf;
            int x = 0;

            if ((startIndex + length) <= value.Length)
            {
                buf = new char[length];

                for (int i = startIndex; i < startIndex + length; i++)
                {
                    buf[x++] = value[i];
                }

                return buf;
            }

            if (startIndex < value.Length)
            {
                buf = new char[value.Length];

                for (int i = startIndex; i < value.Length; i++)
                {
                    buf[x++] = value[i];
                }

                return buf;
            }

            return Array.Empty<char>();
        }

        /// <summary>
        /// Returns the index of the search <see cref="char"/> if found.  If not found
        /// a -1 is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        public static int IndexOf(this char[] value, char c)
        {
            return Array.IndexOf(value, c);
        }

        /// <summary>
        /// Returns the index of the search <see cref="char"/> starting at the specified index if found.
        /// If not found a -1 is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        /// <param name="startIndex"></param>
        public static int IndexOf(this char[] value, char c, int startIndex)
        {
            return Array.IndexOf(value, c, startIndex);
        }

        /// <summary>
        /// Returns the index of the search <see cref="char"/> starting at the specified index for the specified
        /// number of characters.  If not found a -1 is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public static int IndexOf(this char[] value, char c, int startIndex, int count)
        {
            return Array.IndexOf(value, c, startIndex, count);
        }

        /// <summary>
        /// Returns the first index of the search array in the original array.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="search"></param>
        public static int IndexOf(this char[] value, char[] search)
        {
            if (search.Length == 0)
            {
                return -1;
            }

            int lengthOfArrayToFind = search.Length;
            int lengthOfArrayToSearchIn = value.Length;

            for (int i = 0; i < lengthOfArrayToSearchIn; i++)
            {
                if (lengthOfArrayToSearchIn - i < lengthOfArrayToFind)
                {
                    return -1;
                }

                if (value[i] != search[0])
                {
                    continue;
                }

                int arrayToFindCounter = 0;
                bool found = true;

                for (int j = i; j < i + lengthOfArrayToFind; j++)
                {
                    if (search[arrayToFindCounter] == value[j])
                    {
                        arrayToFindCounter++;
                    }
                    else
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns the first index of the search array in the original array starting at the
        /// specified index.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="search"></param>
        /// <param name="startIndex"></param>
        public static int IndexOf(this char[] value, char[] search, int startIndex)
        {
            if (search.Length == 0)
            {
                return -1;
            }

            int lengthOfArrayToFind = search.Length;
            int lengthOfArrayToSearchIn = value.Length;

            for (int i = startIndex; i < lengthOfArrayToSearchIn; i++)
            {
                if (lengthOfArrayToSearchIn - i < lengthOfArrayToFind)
                {
                    return -1;
                }

                if (value[i] != search[0])
                {
                    continue;
                }

                int arrayToFindCounter = 0;
                bool found = true;

                for (int j = i; j < i + lengthOfArrayToFind; j++)
                {
                    if (search[arrayToFindCounter] == value[j])
                    {
                        arrayToFindCounter++;
                    }
                    else
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns the index of the search <see cref="char"/> if found starting from the end and moving left.  If
        /// not found a -1 is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        public static int LastIndexOf(this char[] value, char c)
        {
            return Array.LastIndexOf(value, c);
        }

        /// <summary>
        /// Returns the index of the search <see cref="char"/> if found starting from the specified start index
        /// and moving left.  If not found a -1 is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        /// <param name="startIndex"></param>
        public static int LastIndexOf(this char[] value, char c, int startIndex)
        {
            return Array.LastIndexOf(value, c, startIndex);
        }

        /// <summary>
        /// Returns the index of the search <see cref="char"/> if found starting from the specified start index
        /// and moving left for the specified number of characters.  If not found a -1 is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public static int LastIndexOf(this char[] value, char c, int startIndex, int count)
        {
            return Array.LastIndexOf(value, c, startIndex, count);
        }

        /// <summary>
        /// If the char[] starts with another specified <see cref="char"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        public static bool StartsWith(this char[] value, char c)
        {
            if (value.Length == 0)
            {
                return false;
            }

            return (value[0].Equals(c));
        }

        /// <summary>
        /// If the char[] starts with another specified char[].
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        public static bool StartsWith(this char[] value, char[] c)
        {
            if (value.Length == 0)
            {
                return false;
            }

            return value.Take(c.Length).SequenceEqual(c);
        }

        /// <summary>
        /// If the char[] starts with a specified string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="str"></param>
        public static bool StartsWith(this char[] value, string str)
        {
            if (value.Length == 0)
            {
                return false;
            }

            return value.Take(str.Length).SequenceEqual(str);
        }

        /// <summary>
        /// If the char[] ends with another specified <see cref="char"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        public static bool EndsWith(this char[] value, char c)
        {
            if (value.Length == 0)
            {
                return false;

            }

            return (value[value.Length - 1].Equals(c));
        }

        /// <summary>
        /// If the char[] ends with another specified char[].
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        public static bool EndsWith(this char[] value, char[] c)
        {
            if (value.Length == 0)
            {
                return false;
            }

            return value.TakeLast(c.Length).SequenceEqual(c);
        }

        /// <summary>
        /// If the char[] ends with a specified string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="str"></param>
        public static bool EndsWith(this char[] value, string str)
        {
            if (value.Length == 0)
            {
                return false;
            }

            return value.TakeLast(str.Length).SequenceEqual(str);
        }

        /// <summary>
        /// Converts the entire existing char array to upper case as well as returns
        /// that reference for chaining.
        /// </summary>
        /// <param name="value"></param>
        public static char[] ToUpper(this char[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = char.ToUpper(value[i]);
            }

            return value;
        }

        /// <summary>
        /// Converts the entire existing char array to lower case as well as returns
        /// that reference for chaining.
        /// </summary>
        /// <param name="value"></param>
        public static char[] ToLower(this char[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = char.ToLower(value[i]);
            }

            return value;
        }

        /// <summary>
        /// If the entire char array is WhiteSpace.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsWhiteSpace(this char[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Whether or not the <see cref="char"/> is a white space character.
        /// </summary>
        /// <param name="c"></param>
        public static bool IsWhiteSpace(this char c)
        {
            return char.IsWhiteSpace(c);
        }

        /// <summary>
        /// Whether or not the <see cref="char"/> is a lower case.
        /// </summary>
        /// <param name="c"></param>
        public static bool IsLower(this char c)
        {
            return char.IsLower(c);
        }

        /// <summary>
        /// Whether or not the <see cref="char"/> is a upper case.
        /// </summary>
        /// <param name="c"></param>
        public static bool IsUpper(this char c)
        {
            return char.IsUpper(c);
        }

        /// <summary>
        /// Whether or not the <see cref="char"/> is a control character.
        /// </summary>
        /// <param name="c"></param>
        public static bool IsControl(this char c)
        {
            return char.IsControl(c);
        }

        /// <summary>
        /// Whether or not the <see cref="char"/> is a digit.
        /// </summary>
        /// <param name="c"></param>
        public static bool IsDigit(this char c)
        {
            return char.IsDigit(c);
        }

        /// <summary>
        /// Whether or not the <see cref="char"/> is a digit or letter.
        /// </summary>
        /// <param name="c"></param>
        public static bool IsLetterOrDigit(this char c)
        {
            return char.IsLetterOrDigit(c);
        }

        /// <summary>
        /// Whether or not the <see cref="char"/> is a letter.
        /// </summary>
        /// <param name="c"></param>
        public static bool IsLetter(this char c)
        {
            return char.IsLetter(c);
        }

        /// <summary>
        /// Whether or not the <see cref="char"/> is a number.
        /// </summary>
        /// <param name="c"></param>
        public static bool IsNumber(this char c)
        {
            return char.IsNumber(c);
        }

        /// <summary>
        /// Whether or not the <see cref="char"/> is a punctuation character.
        /// </summary>
        /// <param name="c"></param>
        public static bool IsPunctuation(this char c)
        {
            return char.IsPunctuation(c);
        }

        /// <summary>
        /// Whether or not the <see cref="char"/> is a symbol.
        /// </summary>
        /// <param name="c"></param>
        public static bool IsSymbol(this char c)
        {
            return char.IsSymbol(c);
        }

        /// <summary>
        /// Returns the numeric ASCII code for the <see cref="char"/>.
        /// </summary>
        /// <param name="c"></param>
        public static int ToAsciiCode(this char c)
        {
            return c;
        }

        /// <summary>
        /// Repeat the given char the specified number of times into a new string.
        /// </summary>
        /// <param name="c">The char to repeat.</param>
        /// <param name="count">The number of times to repeat the string.</param>
        public static string RepeatToString(this char c, int count)
        {
            return new string(c, count);
        }

        /// <summary>
        /// Repeat the give char the specified number of times into a new char[].
        /// </summary>
        /// <param name="c"></param>
        /// <param name="count"></param>
        public static char[] RepeatToChar(this char c, int count)
        {
            var buf = new char[count];

            for (int i = 0; i < count; i++)
            {
                buf[i] = c;
            }
            
            return buf;
        }

        /// <summary>
        /// Whether the <see cref="char"/> is a vowel.
        /// </summary>
        /// <param name="c"></param>
        public static bool IsVowel(this char c)
        {
            return (c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u');
        }

        /// <summary>
        /// Trims whitespace off of the char[] returning a new char[];
        /// </summary>
        /// <param name="c"></param>
        public static char[] Trim(this char[] c)
        {
            if (c.Length == 0)
            {
                return Array.Empty<char>();
            }

            int start = 0;
            int end = c.Length;

            // Find the starting spot on the left.
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i].IsWhiteSpace())
                {
                    start++;
                }
                else
                {
                    break;
                }
            }

            // Find the ending spot on the right.
            for (int i = c.Length - 1; i >= 0; i--)
            {
                if (c[i].IsWhiteSpace())
                {
                    end--;
                }
                else
                {
                    break;
                }
            }

            // Now that we have the start and the end, we can calculate the new size and
            // copy the bits we need into the new array.
            var newArray = new char[end - start];
            int x = 0;

            for (int i = 0; i <= end; i++)
            {
                if (i >= start && i < end)
                {
                    newArray[x++] = c[i];
                }
            }

            return newArray;
        }
    }
}