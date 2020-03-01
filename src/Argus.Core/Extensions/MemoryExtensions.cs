#if NETSTANDARD2_1
using System;

namespace Argus.Extensions
{
    /// <summary>
    ///     Extensions for use with System.Memory types and classes.
    /// </summary>
    public static class MemoryExtensions
    {
        //*********************************************************************************************************************
        //
        //             Class:  MemoryExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  02/12/2020
        //      Last Updated:  03/01/2020
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Reports the zero based index of the first occurence of a matching string.
        /// </summary>
        /// <param name="span">The Span to search.</param>
        /// <param name="value">The string to search for.</param>
        /// <param name="startIndex"></param>
        /// <returns>
        ///     Returns the zero based index or a -1 if the string isn't found or the
        ///     startIndex greater than the length of the string.
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
        ///     Returns the zero based index of the first occurence of a matching string.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        public static int IndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, int startIndex)
        {
            var indexInSlice = span.Slice(startIndex).IndexOf(value, StringComparison.Ordinal);

            if (indexInSlice == -1)
            {
                return -1;
            }

            return startIndex + indexInSlice;
        }

        /// <summary>
        ///     Split the next part of this span with the given separator. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="span">A reference to the span</param>
        /// <param name="seperator">A seperator that delimits the values</param>
        /// <returns>The first splitted value</returns>
        public static ReadOnlySpan<T> SplitNext<T>(this ref ReadOnlySpan<T> span, T seperator) where T : IEquatable<T>
        {
            int pos = span.IndexOf(seperator);
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

    }
}
#endif