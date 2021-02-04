/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Argus.AspNetCore.Environment
{
    /// <summary>
    /// Extension methods for use with the <see cref="ResourceMonitorApi" />.  This will handle wiring
    /// up the resources monitor API's.  Either all resources can be added to be served or they can
    /// be added one by one for the ones that are needed.
    /// </summary>
    public static class ResourceMonitorExtensions
    {
        /// <summary>
        /// Maps all of the routes available for resource monitoring.
        /// </summary>
        /// <param name="app"></param>
        public static void MapResourceMonitor(this IApplicationBuilder app)
        {
            app.Map("/resource-monitor/detailed", ResourceMonitorApi.Detailed);
            app.Map("/resource-monitor/ping", ResourceMonitorApi.Ping);
            app.Map("/resource-monitor/machine-name", ResourceMonitorApi.MachineName);
        }

        /// <summary>
        /// Maps all the routes available for resource monitoring if a provided key and value match.  It is important
        /// that when this method is used that it is done so over HTTPS.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="keyName"></param>
        /// <param name="keyValue"></param>
        public static void MapResourceMonitor(this IApplicationBuilder app, string keyName, string keyValue)
        {
            app.MapWhen(context =>
            {
                return context.Request.Path.StartsWithSegments("/resource-monitor/detailed") && ValidateKey(context, keyName, keyValue);
            }, ResourceMonitorApi.Detailed);

            app.MapWhen(context =>
            {
                return context.Request.Path.StartsWithSegments("/resource-monitor/ping") && ValidateKey(context, keyName, keyValue);
            }, ResourceMonitorApi.Ping);

            app.MapWhen(context =>
            {
                return context.Request.Path.StartsWithSegments("/resource-monitor/machine-name") && ValidateKey(context, keyName, keyValue);
            }, ResourceMonitorApi.MachineName);
        }

        /// <summary>
        /// Validates a key/value against a provided key that should be in the HttpContext.Request.
        /// </summary>
        /// <param name="context">The HttpContext for use with Requests header/query string/form values.</param>
        /// <param name="keyName">The name of the key to check.</param>
        /// <param name="keyValue">The value of the key you want the requester to match.</param>
        /// <returns></returns>
        private static bool ValidateKey(HttpContext context, string keyName, string keyValue)
        {
            string providedKey = "";

            // ToString() is necessary to get the value here (the other option is to cast the headers
            // as a FrameRequestHeaders and use it's HeaderAuthorization property)
            if (context.Request.Headers.ContainsKey(keyName))
            {
                providedKey = context.Request.Headers[keyName].ToString().ToLower().Replace("bearer", "").Trim();
            }
            else if (context.Request.Query.ContainsKey(keyName))
            {
                providedKey = context.Request.Query[keyName];
            }
            else if (context.Request.HasFormContentType && context.Request.Form.ContainsKey(keyName))
            {
                providedKey = context.Request.Form[keyName];
            }

            // Check their API key to see if it matches, if it doesn't then send them a 401.
            return providedKey.ToLower() == keyValue.ToLower();
        }
    }
}