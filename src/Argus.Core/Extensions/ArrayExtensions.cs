using System;

namespace Argus.Extensions
{
    /// <summary>
    ///     Extension methods for general arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  ArrayExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  01/03/2014
        //      Last Updated:  04/07/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Remove element from array at given index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            var destination = new T[source.Length - 1];

            if (index > 0)
            {
                Array.Copy(source, 0, destination, 0, index);
            }

            if (index < source.Length - 1)
            {
                Array.Copy(source, index + 1, destination, index, source.Length - index - 1);
            }

            return destination;
        }

        /// <summary>
        ///     Searches through a string array and determines if any item contains the search string.
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
        ///     Searches through a string array and determines if any item starts with the search string.
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
        ///     Searches through a string array and determines if any item matches the search string.
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