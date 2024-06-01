/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2010-06-22
 * @last updated      : 2023-07-24
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
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
        
        /// <summary>
        /// Returns an MD5 hash based off of the exception's Message property.
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="includeInnerException">If the inner exception should be included when calculating the hash if it's not null.</param>
        /// <returns>MD5 Hash</returns>
        public static string Md5Hash(this Exception ex, bool includeInnerException = true)
        {
            var sb = Argus.Memory.StringBuilderPool.Take();
            
            // Append the primary exception's message.
            sb.Append(ex.Message);
                
            // If they also want to look at the inner exception and if the inner exception
            // isn't null then append it.
            if (includeInnerException && ex.InnerException != null)
            {
                sb.Append(ex.InnerException.Message);
            }

            try
            {
                return Argus.Cryptography.HashUtilities.MD5Hash(sb.ToString());
            }
            finally
            {
                // Return the StringBuilder to our memory pool.
                Argus.Memory.StringBuilderPool.Return(sb);
            }
        }
    }
}