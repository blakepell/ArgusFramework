using Microsoft.AspNetCore.Builder;

namespace Argus.Extensions
{

    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        //*********************************************************************************************************************
        //
        //             Class:  ApplicationBuilderExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  06/07/2017
        //      Last Updated:  11/17/2019
        //    Programmer(s):   Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Allows all static files to be served, including those of an unknown type.
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
        ///     Allows all static files to be served, including those of an unknown type.
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