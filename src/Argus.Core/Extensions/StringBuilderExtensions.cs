/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2008-01-12
 * @last updated      : 2023-11-30
 * @copyright         : Copyright (c) 2003-2023, All rights reserved.
 * @license           : MIT
 */

using Cysharp.Text;

namespace Argus.Extensions
{
    /// <summary>
    /// StringBuilder extension methods.
    /// </summary>
    public static class StringBuilderExtensions
    {
#if NET5_0_OR_GREATER
        /// <summary>
        /// Creates a <see cref="Utf16ValueStringBuilder"/> from the chunks of the <see cref="StringBuilder"/>.  The 
        /// <see cref="Utf16ValueStringBuilder"/> must be disposed of when it is no longer needed.
        /// </summary>
        /// <param name="sb"></param>
        public static Utf16ValueStringBuilder ToUtf16ValueStringBuilder(this StringBuilder sb)
        {
            var zsb = ZString.CreateStringBuilder();
            
            foreach (ReadOnlyMemory<char> chunk in sb.GetChunks())
            {
                zsb.Append(chunk.Span);
            }
            
            return zsb;
        }

        /// <summary>
        /// Finds the first index of a char in a <see cref="StringBuilder"/>.  If not match is found
        /// a -1 is returned.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="c"></param>
        public static int IndexOf(this StringBuilder sb, char c)
        {
            int pos = 0;
            foreach (var chunk in sb.GetChunks())
            {
                var span = chunk.Span;

                for (int i = 0; i < span.Length; i++)
                {
                    if (span[i] == c)
                    {
                        return pos + i;
                    }
                }

                pos += span.Length;
            }

            return -1;
        }
#endif

        /// <summary>
        /// Calls the StringBuilder AppendFormat method and then also calls AppendLine to add the default line terminator to the end
        /// of the string
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        public static StringBuilder AppendLineFormat(this StringBuilder sb, string format, params object[] arguments)
        {
            sb.AppendFormat(format, arguments);
            sb.AppendLine();

            return sb;
        }

        /// <summary>
        /// Appends the provided value if the condition is true.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="condition"></param>
        /// <param name="value"></param>
        public static StringBuilder AppendIf(this StringBuilder sb, bool condition, string value)
        {
            if (condition)
            {
                sb.Append(value);
            }

            return sb;
        }

#if NETSTANDARD2_1 || NET5_0_OR_GREATER
        /// <summary>
        /// Appends the provided value if the condition is true.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="condition"></param>
        /// <param name="value"></param>
        public static StringBuilder AppendIf(this StringBuilder sb, bool condition, ReadOnlySpan<char> value)
        {
            if (condition)
            {
                sb.Append(value);
            }

            return sb;
        }
#endif

        /// <summary>
        /// Appends the provided formatted text if the condition is true.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="condition"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        public static StringBuilder AppendFormatIf(this StringBuilder sb, bool condition, string format, params object[] arguments)
        {
            if (condition)
            {
                sb.AppendFormat(format, arguments);
            }

            return sb;
        }

        /// <summary>
        /// Appends a line if the condition is true.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="condition"></param>
        public static StringBuilder AppendLineIf(this StringBuilder sb, bool condition)
        {
            if (condition)
            {
                sb.AppendLine();
            }

            return sb;
        }

        /// <summary>
        /// Appends a string if the check value is not null or white space.
        /// </summary>
        /// <param name="sb">The StringBuilder to append to.</param>
        /// <param name="checkValue">The value to check to see if the append takes place.</param>
        /// <param name="appendValue">The value to append.</param>
        public static void AppendIfNotNullOrEmpty(this StringBuilder sb, string? checkValue, string appendValue)
        {
            if (!string.IsNullOrWhiteSpace(checkValue))
            {
                sb.Append(appendValue);
            }
        }
        
        /// <summary>
        /// Converts a StringBuilder to uppercase.
        /// </summary>
        /// <param name="sb">A <see cref="StringBuilder" /> to convert to uppercase.</param>
        /// <returns>The <see cref="StringBuilder" /> converted to uppercase.</returns>
        public static StringBuilder ToUpper(this StringBuilder sb)
        {
            if (sb == null)
            {
                return null;
            }

            for (int i = 0; i < sb.Length; i++)
            {
                sb[i] = char.ToUpper(sb[i]);
            }

            return sb;
        }

        /// <summary>
        /// Returns a <see cref="StringBuilder" /> converted to lowercase.  This will alter the
        /// </summary>
        /// <param name="sb">A <see cref="StringBuilder" /> to convert to lowercase.</param>
        /// <returns>The <see cref="StringBuilder" /> converted to lowercase.</returns>
        public static StringBuilder ToLower(this StringBuilder sb)
        {
            if (sb == null)
            {
                return null;
            }

            for (int i = 0; i < sb.Length; i++)
            {
                sb[i] = char.ToLower(sb[i]);
            }

            return sb;
        }

        /// <summary>
        /// Determines whether this instance of <see cref="StringBuilder" /> ends with the specified string.
        /// </summary>
        /// <param name="sb">A <see cref="StringBuilder" /> to compare.</param>
        /// <param name="value">The string to compare to the substring at the end of this instance.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>
        /// true if the <paramref name="value" /> parameter matches the beginning of this string; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is null.</exception>
        public static bool EndsWith(this StringBuilder sb, string value, bool ignoreCase = false)
        {
            if (sb == null)
            {
                return false;
            }

            int length = value.Length;
            int maxSbIndex = sb.Length - 1;
            int maxValueIndex = length - 1;

            if (length > sb.Length)
            {
                return false;
            }

            if (ignoreCase == false)
            {
                for (int i = 0; i < length; i++)
                {
                    if (sb[maxSbIndex - i] != value[maxValueIndex - i])
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int j = length - 1; j >= 0; j--)
                {
                    if (char.ToLower(sb[maxSbIndex - j]) != char.ToLower(value[maxValueIndex - j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance of <see cref="StringBuilder" /> starts with the specified string.
        /// </summary>
        /// <param name="sb">A <see cref="StringBuilder" /> to compare.</param>
        /// <param name="value">The string to compare.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>
        /// true if the <paramref name="value" /> parameter matches the beginning of this string; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is null.</exception>
        public static bool StartsWith(this StringBuilder sb, string value, bool ignoreCase = false)
        {
            if (sb == null)
            {
                return false;
            }

            int length = value.Length;

            if (length > sb.Length)
            {
                return false;
            }

            if (ignoreCase == false)
            {
                for (int i = 0; i < length; i++)
                {
                    if (sb[i] != value[i])
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int j = 0; j < length; j++)
                {
                    if (char.ToLower(sb[j]) != char.ToLower(value[j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance of <see cref="StringBuilder" /> starts with the specified <see cref="char"/>.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        public static bool StartsWith(this StringBuilder sb, char value, bool ignoreCase = false)
        {
            if (sb.Length == 0)
            {
                return false;
            }

            if (ignoreCase)
            {
                return char.ToLower(sb[0]) == char.ToLower(value);
            }

            return sb[0] == value;
        }

        /// <summary>
        /// Determines whether this instance of <see cref="StringBuilder" /> ends with the specified <see cref="char"/>.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        public static bool EndsWith(this StringBuilder sb, char value, bool ignoreCase = false)
        {
            if (sb.Length == 0)
            {
                return false;
            }

            if (ignoreCase)
            {
                return char.ToLower(sb[sb.Length - 1]) == char.ToLower(value);
            }

            return sb[sb.Length - 1] == value;
        }

        /// <summary>
        /// Removes all occurrences of specified characters from <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="sb">A <see cref="StringBuilder" /> to remove from.</param>
        /// <param name="removeChars">A Unicode characters to remove.</param>
        public static StringBuilder Remove(this StringBuilder sb, params char[] removeChars)
        {
            if (sb == null || removeChars == null)
            {
                return sb;
            }

            for (int i = 0; i < sb.Length;)
            {
                if (removeChars.Any(ch => ch == sb[i]))
                {
                    sb.Remove(i, 1);
                }
                else
                {
                    i++;
                }
            }

            return sb;
        }

        /// <summary>
        /// Removes the range of characters from the specified zero based index to the end of <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="sb">A <see cref="StringBuilder" /> to remove from.</param>
        /// <param name="startIndex">The zero-based position to begin deleting characters.</param>
        /// <returns>A reference to this instance after the excise operation has completed.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If <paramref name="startIndex" /> is less than zero, or <paramref name="startIndex" /> is greater
        /// than the length - 1 of this instance.
        /// </exception>
        public static StringBuilder Remove(this StringBuilder sb, int startIndex)
        {
            if (sb == null)
            {
                return sb;
            }

            return sb.Remove(startIndex, sb.Length - startIndex);
        }

        /// <summary>
        /// Trims the white space off the start and end of a StringBuilder.
        /// </summary>
        /// <param name="sb"></param>
        public static StringBuilder Trim(this StringBuilder sb)
        {
            if (sb == null || sb.Length == 0)
            {
                return sb;
            }

            int length = 0;
            int num2 = sb.Length;

            while (sb[length] == ' ' && length < num2)
            {
                length++;
            }

            if (length > 0)
            {
                sb.Remove(0, length);
                num2 = sb.Length;
            }

            length = num2 - 1;

            while (sb[length] == ' ' && length > -1)
            {
                length--;
            }

            if (length < num2 - 1)
            {
                sb.Remove(length + 1, num2 - length - 1);
            }

            return sb;
        }

        /// <summary>
        /// Trims white space off the end of a StringBuilder.
        /// </summary>
        /// <param name="sb"></param>
        public static StringBuilder TrimEnd(this StringBuilder sb)
        {
            if (sb == null || sb.Length == 0)
            {
                return sb;
            }

            int i = sb.Length - 1;

            for (; i >= 0; i--)
            {
                if (!char.IsWhiteSpace(sb[i]))
                {
                    break;
                }
            }

            if (i < sb.Length - 1)
            {
                sb.Length = i + 1;
            }

            return sb;
        }

        /// <summary>
        /// Trims a specified set of characters off the end of a StringBuilder.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="trimChars"></param>
        public static StringBuilder TrimEnd(this StringBuilder sb, params char[] trimChars)
        {
            if (sb == null || sb.Length == 0)
            {
                return sb;
            }

            int i = sb.Length - 1;

            for (; i >= 0; i--)
            {
                if (trimChars.All(ch => ch != sb[i]))
                {
                    break;
                }
            }

            if (i < sb.Length - 1)
            {
                sb.Length = i + 1;
            }

            return sb;
        }

        /// <summary>
        /// Trims white space off the start of a StringBuilder.
        /// </summary>
        /// <param name="sb"></param>
        public static StringBuilder TrimStart(this StringBuilder sb)
        {
            if (sb == null || sb.Length == 0)
            {
                return sb;
            }

            int length = 0;

            while (char.IsWhiteSpace(sb[length]) && length < sb.Length)
            {
                length++;
            }

            if (length > 0)
            {
                sb.Remove(0, length);
            }

            return sb;
        }

        /// <summary>
        /// Trims a specified set of characters off the start of a StringBuilder.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="trimChars"></param>
        public static StringBuilder TrimStart(this StringBuilder sb, params char[] trimChars)
        {
            if (sb == null || sb.Length == 0)
            {
                return sb;
            }

            int length = 0;

            while (trimChars.Any(ch => ch == sb[length]) && length < sb.Length)
            {
                length++;
            }

            if (length > 0)
            {
                sb.Remove(0, length);
            }

            return sb;
        }

        /// <summary>
        /// Whether or not the StringBuilder contains a number anywhere in it's contents.
        /// </summary>
        /// <param name="sb"></param>
        public static bool ContainsNumber(this StringBuilder sb)
        {
            for (int i = 0; i < sb.Length; i++)
            {
                if (char.IsNumber(sb[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}