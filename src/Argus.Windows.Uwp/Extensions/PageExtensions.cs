using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;

namespace Argus.Extensions
{
    /// <summary>
    ///     Extensions for UWP Pages.
    /// </summary>
    public static class PageExtensions
    {
        /// <summary>
        ///     Sets the text on the title bar for the current page.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static void TitleSet(this Page page, string title)
        {
            ApplicationViewExtensions.SetTitle(page, title);
        }

        /// <summary>
        ///     Gets the text on the title bar for the current page.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string TitleGet(this Page page)
        {
            return ApplicationViewExtensions.GetTitle(page);
        }

        /// <summary>
        ///     Gets the title bar for the current page.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApplicationViewTitleBar TitleBar(this Page page)
        {
            return ApplicationView.GetForCurrentView().TitleBar;
        }

        /// <summary>
        ///     Whether to extend the current view into the title bar via <see cref="CoreApplication.GetCurrentView()" />.
        /// </summary>
        /// <param name="extendViewIntoTitleBar"></param>
        public static void ExtendViewIntoTitleBar(this Page page, bool extendViewIntoTitleBar)
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = extendViewIntoTitleBar;
        }
    }
}