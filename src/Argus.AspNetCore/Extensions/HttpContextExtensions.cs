/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="HttpContext" />.
    /// </summary>
    public static class HttpContextExtensions
    {

        /// <summary>
        /// Returns the value for a claim, or an empty string if none exists.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="claimType">String value from the constants in System.Security.Claims.ClaimTypes</param>
        public static string GetClaim(this HttpContext context, string claimType)
        {
            return context.User?.Claims?.FirstOrDefault(x => x.Type == claimType)?.Value ?? "";
        }
    }
}