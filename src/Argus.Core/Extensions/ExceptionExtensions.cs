/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2010-06-22
 * @last updated      : 2023-06-08
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Memory;

namespace Argus.Extensions
{
    /// <summary>
    /// Extensions for Exceptions.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Returns the Exception's method and StackTrace.  If an InnerException exists, it also returns the information on it.
        /// </summary>
        /// <param name="ex"></param>
        public static string ToFormattedString(this Exception ex)
        {
            var sb = StringBuilderPool.Take();
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);

            if (ex.InnerException != null)
            {
                sb.AppendLine(ex.InnerException.Message);
                sb.AppendLine(ex.InnerException.StackTrace);
            }

            try
            {
                return sb.ToString();
            }
            finally
            {
                StringBuilderPool.Return(sb);
            }
        }
    }
}