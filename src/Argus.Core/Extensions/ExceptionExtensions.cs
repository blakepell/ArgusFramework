using System;
using System.Text;

namespace Argus.Extensions
{
    /// <summary>
    ///     Extensions for Exceptions.
    /// </summary>
    public static class ExceptionExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  ExceptionExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  06/22/2010
        //      Last Updated:  04/03/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Returns the Exception's method and StackTrace.  If an InnerException exists, it also returns the information on it.
        /// </summary>
        /// <param name="ex"></param>
        public static string ToFormattedString(this Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);

            if (ex.InnerException != null)
            {
                sb.AppendLine(ex.InnerException.Message);
                sb.AppendLine(ex.InnerException.StackTrace);
            }

            return sb.ToString();
        }
    }
}