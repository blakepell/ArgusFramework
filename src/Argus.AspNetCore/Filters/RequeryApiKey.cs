/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Argus.AspNetCore.Filters
{
    /// <summary>
    /// Attribute that will check that an API key has been provided.
    /// </summary>
    public class RequireApiKey : Attribute, IAuthorizationFilter
    {
        private readonly string _apiKeyName = "";
        private readonly string _queryField = "";
        private readonly string _unauthorizedMessage = "Error - Unauthorized";

        /// <summary>
        /// Constructor (compares against the authorization header/querystring/form field value).  This requires that the Configuration
        /// has been setup in the Startup and that the service has been registered via:
        /// <code>services.AddSingleton&lt;IConfigurationRoot&gt;(Configuration);</code>
        /// </summary>
        /// <param name="apiKeyName">The name of the setting in the config that holds the API key to validate.</param>
        public RequireApiKey(string apiKeyName)
        {
            _apiKeyName = apiKeyName;
            _queryField = "authorization";
        }

        /// <summary>
        /// Constructor (compares against the authorization header/querystring/form field value).  This requires that the Configuration
        /// has been setup in the Startup and that the service has been registered via:
        /// <code>services.AddSingleton&lt;IConfigurationRoot&gt;(Configuration);</code>
        /// </summary>
        /// <param name="apiKeyName">The name of the setting in the config that holds the API key to validate.</param>
        /// <param name="queryField">The name of the header/querystring/form element to compare the key value to.</param>
        public RequireApiKey(string apiKeyName, string queryField)
        {
            _apiKeyName = apiKeyName;
            _queryField = queryField;
        }

        /// <summary>
        /// Constructor (compares against the authorization header/querystring/form field value).  This requires that the Configuration
        /// has been setup in the Startup and that the service has been registered via:
        /// <code>services.AddSingleton&lt;IConfigurationRoot&gt;(Configuration);</code>
        /// </summary>
        /// <param name="apiKeyName">The name of the setting in the config that holds the API key to validate.</param>
        /// <param name="queryField">The name of the header/querystring/form element to compare the key value to.</param>
        /// <param name="unauthorizedMessage">The message to return with the 401 unauthorized result if the result is deemed invalid.</param>
        public RequireApiKey(string apiKeyName, string queryField, string unauthorizedMessage)
        {
            _apiKeyName = apiKeyName;
            _queryField = queryField;
            _unauthorizedMessage = unauthorizedMessage;
        }

        /// <summary>
        /// API Key Validation Event
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool result = false;

            var config = context.HttpContext.RequestServices.GetRequiredService<IConfigurationRoot>();

            string configApiKey = config[$"AppSettings:{_apiKeyName}"];

            if (context.HttpContext.Request.Headers.Keys.Contains(_queryField)
                && context.HttpContext.Request.Headers[_queryField] == configApiKey)
            {
                result = true;
            }

            if (context.HttpContext.Request.HasFormContentType
                && context.HttpContext.Request.Form.Keys.Contains(_queryField)
                && context.HttpContext.Request.Form[_queryField] == configApiKey)
            {
                result = true;
            }

            if (context.HttpContext.Request.Query.Keys.Contains(_queryField)
                && context.HttpContext.Request.Query[_queryField] == configApiKey)
            {
                result = true;
            }

            // Failure
            if (!result)
            {
                // Toss a 401 unauthorized and specify the error message
                context.Result = new ObjectResult(_unauthorizedMessage);
                context.HttpContext.Response.StatusCode = 401;
            }
        }
    }
}