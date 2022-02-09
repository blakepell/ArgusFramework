/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2012-11-19
 * @last updated      : 2021-02-07
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Argus.Extensions
{
    /// <summary>
    /// List extensions.
    /// </summary>
    public static class ListExtensions
    {
        private static readonly Random _shuffleRng = new Random();

        /// <summary>
        /// Removes an item from the end of the list and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns>The last item in a list, if there are no items in the list then a null value is returned.</returns>
        public static T PopLast<T>(this List<T> lst)
        {
            if (lst.Count == 0)
            {
                return default;
            }

            var local = lst[lst.Count - 1];
            lst.RemoveAt(lst.Count - 1);

            return local;
        }

        /// <summary>
        /// Removes an item from the beginning of the list and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns>The first item in the list, if there are no items in the list then a null value is returned.</returns>
        public static T PopFirst<T>(this List<T> lst)
        {
            if (lst.Count > 0)
            {
                var local = lst[0];
                lst.RemoveAt(0);

                return local;
            }

            return default;
        }

        /// <summary>
        /// Returns the first non null value in the list.  If no non null values are found default(T) is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        public static T Coalesce<T>(this List<T> lst)
        {
            foreach (T obj in lst)
            {
                if (obj != null)
                {
                    return obj;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Returns the first non null value in the list.  If no non null values are found the default value
        /// specified is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="defaultValue"></param>
        public static T Coalesce<T>(this List<T> lst, T defaultValue)
        {
            foreach (T obj in lst)
            {
                if (obj != null)
                {
                    return obj;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Populates a list with the contents of a delimited string.  This will remove blank entries from the list if they exist.
        /// </summary>
        /// <param name="list">The source list.</param>
        /// <param name="buf">The text that should be split.</param>
        /// <param name="delimiter">The delimiter to split on.</param>
        /// <param name="removeEmptyEntries">If empty entries should be removed from the list.</param>
        /// <param name="clearListFirst">If the list should be cleared first.</param>
        public static List<string> FromDelimitedString(this List<string> list, string buf, string delimiter, bool removeEmptyEntries = true, bool clearListFirst = false)
        {
            if (clearListFirst && list.Count > 0)
            {
                list.Clear();
            }

            string[] items;

            if (removeEmptyEntries)
            {
                items = buf.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                items = buf.Split(delimiter.ToCharArray());
            }

            list.AddRange(items);

            // Get out, we're done.
            if (!removeEmptyEntries)
            {
                return list;
            }

            // Because RemoveEmptyEntries leaves entries with whitespace, let's remove those.
            for (int x = list.Count - 1; x >= 0; x--)
            {
                if (string.IsNullOrWhiteSpace(list[x]))
                {
                    list.RemoveAt(x);
                }
            }

            return list;
        }

        /// <summary>
        /// Returns a delimited string from the list.
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="delimiter"></param>
        public static string ToDelimitedString(this List<string> ls, string delimiter)
        {
            var sb = new StringBuilder();

            foreach (string buf in ls)
            {
                sb.Append(buf).Append(delimiter);
            }

            // The final delimiter is trimmed off since there is no record after that item
            return sb.ToString().Trim(delimiter);
        }

        /// <summary>
        /// Returns a delimited string from the list.
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="delimiter"></param>
        public static string ToDelimitedString(this List<string> ls, char delimiter)
        {
            var sb = new StringBuilder();

            foreach (string buf in ls)
            {
                sb.Append(buf).Append(delimiter);
            }

            // The final delimiter is trimmed off since there is no record after that item
            return sb.TrimEnd(delimiter).ToString();
        }

        /// <summary>
        /// Returns a delimited string from the list with each item wrapped by a specified character.
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="delimiter"></param>
        /// <param name="wrapCharacter">A character to append and prepend to each item.  As an example, this would be used for wrapping SQL parameters with single quotes.</param>
        public static string ToDelimitedString(this List<string> ls, string delimiter, string wrapCharacter)
        {
            var sb = new StringBuilder();

            foreach (string buf in ls)
            {
                sb.AppendFormat("{0}{1}{0}", wrapCharacter, buf);
                sb.Append(delimiter);
            }

            // The final delimiter is trimmed off since there is no record after that item
            return sb.ToString().Trim(delimiter);
        }

        /// <summary>
        /// Sorts the string list is either ascending or descending order.
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="order"></param>
        public static void StringSort(this List<string> ls, ListSortDirection order)
        {
            if (order == ListSortDirection.Ascending)
            {
                ls.Sort((p1, p2) => string.Compare(p1, p2, StringComparison.Ordinal));
            }
            else
            {
                ls.Sort((p1, p2) => string.Compare(p2, p1, StringComparison.Ordinal));
            }
        }

        /// <summary>
        /// Shuffles a list.  Note, this is not thread safe due to Random not being thread safe.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = _shuffleRng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Returns the length of the longest line in the string list.
        /// </summary>
        /// <param name="list"></param>
        public static int LengthLongestLine(this List<string> list)
        {
            return list.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}" /> into an <see cref="ObservableCollection{T}" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ie"></param>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> ie)
        {
            var c = new ObservableCollection<T>();

            foreach (var item in ie)
            {
                c.Add(item);
            }

            return c;
        }
    }
}