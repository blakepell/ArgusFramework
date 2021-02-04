/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.ExtendedExecution;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Argus.Windows.Uwp
{
    /// <summary>
    /// Windows Universal Utilities
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Makes a request to Windows to not have this App suspend when minimized or the computer is locked.  This
        /// will work for computers plugged in but may still be suspended on battery powered computers.  An exception
        /// is thrown from this method if the request is denied.
        /// </summary>
        /// <remarks>
        /// The session object is returned which should be held onto by the caller.  The caller can wire up the
        /// Revoked event to handle what to do if the extended session is revoked by Windows:
        /// <code>
        ///     session.Revoked += SessionRevoked;
        /// </code>
        /// </remarks>
        /// <param name="reason">The reason for the request: default value Unspecified</param>
        private static async Task<ExtendedExecutionSession> StartExtendedExecutionSessionAsync(string description = "Extended usage of this application for processing that must continue.", ExtendedExecutionReason reason = ExtendedExecutionReason.Unspecified)
        {
            var session = new ExtendedExecutionSession();
            session.Reason = reason;
            session.Description = description;

            var result = await session.RequestExtensionAsync();

            switch (result)
            {
                case ExtendedExecutionResult.Allowed:
                    return session;
                default:
                case ExtendedExecutionResult.Denied:
                    session.Dispose();

                    throw new Exception("Extended execution request was denied.");
            }
        }

        /// <summary>
        /// Opens a page given the page type as a new window.  It should be noted this runs in a new thread
        /// and any binding from another thread likely thrown an exception.
        /// </summary>
        /// <param name="t"></param>
        public static async Task<bool> OpenPageAsWindowAsync(Type t)
        {
            var view = CoreApplication.CreateNewView();
            int id = 0;

            await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(t, null);
                Window.Current.Content = frame;
                Window.Current.Activate();
                id = ApplicationView.GetForCurrentView().Id;
            });

            return await ApplicationViewSwitcher.TryShowAsStandaloneAsync(id);
        }
    }
}