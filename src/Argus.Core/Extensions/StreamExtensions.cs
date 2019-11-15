using System;
using System.IO;
using System.Text;

namespace Argus.Extensions
{

    public static class StreamExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  StreamExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  01/12/2008
        //      Last Updated:  11/07/2017
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Converts the contents of a stream to string text.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToText(this Stream s)
        {
            string txt = "";
            using (StreamReader sr = new StreamReader(s))
            {
                txt = sr.ReadToEnd();
            }
            return txt;
        }

        /// <summary>
        /// Converts a MemoryStream into a Base64 encoded string.
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static string ToBase64(this MemoryStream ms)
        {
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Converts a Stream into a Base64 encoded string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToBase64(this Stream s)
        {
            var ms = new MemoryStream();
            s.CopyTo(ms);
            return ToBase64(ms);
        }

    }

}