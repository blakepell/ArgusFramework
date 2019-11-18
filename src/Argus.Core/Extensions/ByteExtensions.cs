using System;
using System.Text;

namespace Argus.Extensions
{
    /// <summary>
    ///     Extension methods for Byte arrays.
    /// </summary>
    public static class ByteExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  ByteExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  11/05/2013
        //      Last Updated:  11/18/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Converts a byte array to it's string representation that is encoded with base-64 digits.
        /// </summary>
        /// <param name="bytes"></param>
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        ///     Converts a byte array into a string with the specified encoding.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="enc"></param>
        public static string ToString(this byte[] bytes, Encoding enc)
        {
            return enc.GetString(bytes);
        }

    }
}