/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Microsoft.AspNetCore.Builder;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder" />.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Allows all static files to be served, including those of an unknown type.
        /// </summary>
        /// <param name="app"></param>
        public static void UseAllStaticFiles(this IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            });
        }

        /// <summary>
        /// Allows all static files to be served, including those of an unknown type.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="requestPath">The relative request path that maps to the static resources.</param>
        public static void UseAllStaticFiles(this IApplicationBuilder app, string requestPath)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                RequestPath = requestPath
            });
        }
    }
}