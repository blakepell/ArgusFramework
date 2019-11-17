using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Argus.Extensions
{
    /// <summary>
    ///     List extensions.
    /// </summary>
    public static class ListExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  ListExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  11/19/2012
        //      Last Updated:  11/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        private static readonly Random _shuffleRng = new Random();

        /// <summary>
        ///     Removes an item from the end of the list and returns it.
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
        ///     Removes an item from the beginning of the list and returns it.
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
        ///     Returns the first non null value in the list.  If no non null values are found a null is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        public static object Coalesce<T>(this List<T> lst)
        {
            foreach (object obj in lst)
            {
                if (obj != null)
                {
                    return obj;
                }
            }

            return null;
        }

        /// <summary>
        ///     Returns the first non null value in the list.  If no non null values are found the default value
        ///     specified is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="defaultValue"></param>
        public static object Coalesce<T>(this List<T> lst, object defaultValue)
        {
            foreach (object obj in lst)
            {
                if (obj != null)
                {
                    return obj;
                }
            }

            return defaultValue;
        }

        /// <summary>
        ///     Will concatenate all items in a string list.  The optional delimiter can be used to put a space in or any other delimiter required.
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="delimiter"></param>
        public static string ToConcatString(this List<string> lst, string delimiter)
        {
            var sb = new StringBuilder();

            foreach (string item in lst)
            {
                sb.AppendFormat("{0}{1}", item, delimiter);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Populates a list with the contents of a delimited string.  Note: This will remove blank entries from the list if they exist.
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="buf"></param>
        /// <param name="delimiter"></param>
        public static List<string> FromDelimitedString(this List<string> ls, string buf, string delimiter)
        {
            var items = buf.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var itemList = items.ToList();

            // Remove delimiter and then remove blank entries.
            for (int x = itemList.Count - 1; x >= 0; x += -1)
            {
                itemList[x] = itemList[x].Trim(delimiter.ToCharArray());

                if (string.IsNullOrWhiteSpace(itemList[x]))
                {
                    itemList.RemoveAt(x);
                }
            }

            return itemList;
        }

        /// <summary>
        ///     Returns a delimited string from the list.
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="delimiter"></param>
        /// <remarks>
        /// </remarks>
        public static string ToDelimitedString(this List<string> ls, string delimiter)
        {
            var sb = new StringBuilder();

            foreach (string buf in ls)
            {
                sb.Append(buf);
                sb.Append(delimiter);
            }

            // The final delimiter is trimmed off since there is no record after that item
            return sb.ToString().Trim(delimiter);
        }

        /// <summary>
        ///     Returns a delimited string from the list with each item wrapped by a specified character.
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="delimiter"></param>
        /// <param name="wrapCharacter">A character to append and prepend to each item.  As an example, this would be used for wrapping SQL parameters with single quotes.</param>
        /// <remarks>
        /// </remarks>
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
        ///     Sorts the string list is either ascending or descending order.
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="order"></param>
        public static void Sort(this List<string> ls, ListSortDirection order)
        {
            if (order == ListSortDirection.Ascending)
            {
                ls.Sort((p1, p2) => p1.CompareTo(p2));
            }
            else
            {
                ls.Sort((p1, p2) => p2.CompareTo(p1));
            }
        }

        /// <summary>
        ///     Shuffles a list.  Note, this is not thread safe due to Random not being thread safe.
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
        ///     Returns the length of the longest line in the string list.
        /// </summary>
        /// <param name="list"></param>
        public static int LengthLongestLine(this List<string> list)
        {
            return list.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
        }
    }
}