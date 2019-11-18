using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Argus.Extensions
{

    /// <summary>
    /// Extension methods for <see cref="HttpContext"/>.
    /// </summary>
    public static class HttpContextExtensions
    {
        //*********************************************************************************************************************
        //
        //             Class:  HttpContextExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  06/07/2017
        //      Last Updated:  11/17/2019
        //    Programmer(s):   Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Returns the value for a claim, or an empty string if none exists.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="claimType">String value from the constants in System.Security.Claims.ClaimTypes</param>
        /// <returns></returns>
        public static string GetClaim(this HttpContext context, string claimType)
        {
            return context.User?.Claims?.FirstOrDefault(x => x.Type == claimType)?.Value ?? "";
        }
    }
}