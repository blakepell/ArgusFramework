
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Argus.Utilities
{
    /// <summary>
    /// Converstion utilities from one type to another.
    /// </summary>
    /// <remarks></remarks>
    public class Conversion
    {
        //*********************************************************************************************************************
        //
        //             Class:  Conversion
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  10/05/2009
        //      Last Updated:  08/24/2011
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Puts the contents of a MemoryString into a string.
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string MemoryStreamToString(MemoryStream ms)
        {

            if (ms == null)
            {
                return "";
            }

            if (ms.Length == 0)
            {
                return "";
            }

            ms.Flush();
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);

            string buf = sr.ReadToEnd();

            ms.Close();
            ms.Dispose();
            ms = null;
            sr.Close();
            ms.Dispose();
            ms = null;

            return buf;

        }

        /// <summary>
        /// Puts the contents of a string into a MemoryStream.
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static MemoryStream StringToMemoryString(string buf)
        {
            if (string.IsNullOrEmpty(buf))
            {
                return null;
            }

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            sw.Write(buf);
            sw.Flush();
            sw.Close();
            sw.Dispose();

            return ms;
        }

        /// <summary>
        /// Converts a string into a Base64 string.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="enc"></param>
        /// <returns></returns>
        /// <remarks>
        /// This conversion method allows for an encoding to be specified.
        /// </remarks>
        public static object StringToBase64String(string buf, System.Text.Encoding enc)
        {
            byte[] b = enc.GetBytes(buf);
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// Converts a Base64 string into a string.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="enc"></param>
        /// <returns></returns>
        /// <remarks>
        /// This conversion method allows for an encoding to be specified.
        /// </remarks>
        public static object Base64StringToString(string buf, System.Text.Encoding enc)
        {
            byte[] b = Convert.FromBase64String(buf);
            return enc.GetString(b);
        }
 
    }


}