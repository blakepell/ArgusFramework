/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Controller" />.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Renders a View to a string.
        /// </summary>
        /// <param name="controller">The current controller.</param>
        /// <param name="serviceProvider">The service provider required to get a composition engine.</param>
        /// <param name="viewName">The name of the view that should be loaded.</param>
        /// <param name="model">The object model that should be passed to the view.</param>
        public static string RenderViewToString(this Controller controller, IServiceProvider serviceProvider, string viewName, object model)
        {
            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var engine = serviceProvider.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                var viewResult = engine.FindView(controller.ControllerContext, viewName, false);

                var viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions() //Added this parameter in
                );

                var t = viewResult.View.RenderAsync(viewContext);
                t.Wait();

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}