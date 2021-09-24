/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2020-02-12
 * @last updated      : 2021-09-24
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

#if NETSTANDARD2_1 || NET5_0
using System;

namespace Argus.Extensions
{
    /// <summary>
    /// Extensions for use with System.Memory types and classes.
    /// </summary>
    public static class MemoryExtensions
    {
        /// <summary>
        /// Returns the left most portion of the span of the specified length.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="length"></param>
        public static ReadOnlySpan<char> Left(this ReadOnlySpan<char> span, int length)
        {
            return span.Slice(0, length);
        }

        /// <summary>
        /// Returns the left most portion of the span of the specified length.  If the length specified is
        /// longer than the span the entire span is returned without an exception.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="length"></param>
        public static ReadOnlySpan<char> SafeLeft(this ReadOnlySpan<char> span, int length)
        {
            if (length >= span.Length)
            {
                return span;
            }

            return span.Slice(0, length);
        }

        /// <summary>
        /// Returns the right most portion of the span of the specified length.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="length"></param>
        public static ReadOnlySpan<char> Right(this ReadOnlySpan<char> span, int length)
        {
            return span.Slice(span.Length - length, length);
        }

        /// <summary>
        /// Returns the right  most portion of the span of the specified length.  If the length specified is
        /// longer than the span the entire span is returned without an exception.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="length"></param>
        public static ReadOnlySpan<char> SafeRight(this ReadOnlySpan<char> span, int length)
        {
            if (length >= span.Length)
            {
                return span;
            }

            return span.Slice(span.Length - length, length);
        }

        /// <summary>
        /// Reports the zero based index of the first occurrence of a matching string.
        /// </summary>
        /// <param name="span">The Span to search.</param>
        /// <param name="value">The string to search for.</param>
        /// <param name="startIndex"></param>
        /// <returns>
        /// Returns the zero based index or a -1 if the string isn't found or the
        /// startIndex greater than the length of the string.
        /// </returns>
        public static int SafeIndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, int startIndex)
        {
            if (startIndex > span.Length - 1)
            {
                return -1;
            }

            return span.IndexOf(value, startIndex);
        }

        /// <summary>
        /// Reports the zero based index of the first occurrence of a matching char.
        /// </summary>
        /// <param name="span">The Span to search.</param>
        /// <param name="value">The string to search for.</param>
        /// <param name="startIndex"></param>
        /// <returns>
        /// Returns the zero based index or a -1 if the char isn't found or the
        /// startIndex greater than the length of the string.
        /// </returns>
        public static int SafeIndexOf(this ReadOnlySpan<char> span, char value, int startIndex)
        {
            if (startIndex > span.Length - 1)
            {
                return -1;
            }

            return span.IndexOf(value, startIndex);
        }

        /// <summary>
        /// Returns the zero based index of the first occurrence of a matching string.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        public static int IndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, int startIndex)
        {
            int indexInSlice = span.Slice(startIndex).IndexOf(value, StringComparison.Ordinal);

            if (indexInSlice == -1)
            {
                return -1;
            }

            return startIndex + indexInSlice;
        }

        /// <summary>
        /// Returns the zero based index of the first occurrence of a matching char.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        public static int IndexOf(this ReadOnlySpan<char> span, char value, int startIndex)
        {
            int indexInSlice = span.Slice(startIndex).IndexOf(value);

            if (indexInSlice == -1)
            {
                return -1;
            }

            return startIndex + indexInSlice;
        }

        /// <summary>
        /// If the ReadOnlySpan starts with a specified <see cref="char" />.  0 length strings return false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        public static bool StartsWith(this ReadOnlySpan<char> value, char c)
        {
            return value.Length > 0 && value[0].Equals(c);
        }

        /// <summary>
        /// If the current string ends with a specific <see cref="char" />.  0 length strings return false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        public static bool EndsWith(this ReadOnlySpan<char> value, char c)
        {
            return value.Length > 0 && value[^1].Equals(c);
        }

        /// <summary>
        /// Split the next part of this span with the given separator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="span">A reference to the span</param>
        /// <param name="separator">A separator that delimits the values</param>
        /// <returns>The first split value</returns>
        public static ReadOnlySpan<T> SplitNext<T>(this ref ReadOnlySpan<T> span, T separator) where T : IEquatable<T>
        {
            int pos = span.IndexOf(separator);

            if (pos > -1)
            {
                var part = span.Slice(0, pos);
                span = span.Slice(pos + 1);

                return part;
            }
            else
            {
                var part = span;
                span = span.Slice(span.Length);

                return part;
            }
        }

        /// <summary>
        /// Whether a span is null or empty.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsNullOrEmpty(this ReadOnlySpan<char> value)
        {
            return value == null || value.IsEmpty;
        }

        /// <summary>
        /// Whether a span is null, empty or white space.
        /// </summary>
        /// <param name="value"></param>
        public static bool IsNullEmptyOrWhiteSpace(this ReadOnlySpan<char> value)
        {
            return value.IsNullOrEmpty() || value.IsWhiteSpace();
        }

        /// <summary>
        /// Trims Whitespace off of a ReadOnlySpan.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="startAt"></param>
        public static void TrimWhitespace(this ref ReadOnlySpan<char> content, int startAt = 0)
        {
            content.WhitespaceIndexes(out int startIndex, out int endIndex, startAt);

            if (startIndex != startAt || endIndex != content.Length - 1)
            {
                content = content.Slice(startIndex, endIndex - startIndex + 1);
            }
        }

        /// <summary>
        /// Supporting extension for TrimWhitespace.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <remarks>Do not make public.</remarks>
        private static void WhitespaceIndexes(this ref ReadOnlySpan<char> content, out int startIndex, out int endIndex, int start = 0, int? end = null)
        {
            int contentStartsAt = start;

            for (; contentStartsAt < content.Length; ++contentStartsAt)
            {
                if (!char.IsWhiteSpace(content[contentStartsAt]))
                {
                    break;
                }
            }

            int contentEndsAt = end ?? content.Length - 1;

            for (; contentEndsAt > contentStartsAt; --contentEndsAt)
            {
                if (!char.IsWhiteSpace(content[contentEndsAt]))
                {
                    break;
                }
            }

            startIndex = contentStartsAt;
            endIndex = contentEndsAt;
        }

        /// <summary>
        /// Whether an entire string is alphanumeric.
        /// </summary>
        /// <param name="span"></param>
        public static bool IsAlphaNumeric(this ReadOnlySpan<char> span)
        {
            foreach (var c in span)
            {
                if (!c.IsLetterOrDigit())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
#endif