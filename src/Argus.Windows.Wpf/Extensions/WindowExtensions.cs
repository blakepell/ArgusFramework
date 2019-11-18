using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Argus.Extensions
{
    /// <summary>
    ///     Extension methods for WPF Windows.
    /// </summary>
    public static class WindowExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  WindowExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  09/19/2019
        //      Last Updated:  11/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        #region "Remove Icon"

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
                                                int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_DLGMODALFRAME = 0x0001;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_FRAMECHANGED = 0x0020;
        private const uint WM_SETICON = 0x0080;

        /// <summary>
        ///     Removes the command icon from the upper left hand portion of the title bar.
        /// </summary>
        /// <param name="window">The WPF Window</param>
        /// <example>
        ///     this.RemoveIcon();
        /// </example>
        public static void RemoveIcon(this Window window)
        {
            // Get this window's handle
            var hwnd = new WindowInteropHelper(window).Handle;

            // Change the extended window style to not show a window icon
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

            // Update the window's non-client area to reflect the changes
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);

            SendMessage(hwnd, WM_SETICON, new IntPtr(1), IntPtr.Zero);
            SendMessage(hwnd, WM_SETICON, IntPtr.Zero, IntPtr.Zero);
        }

        #endregion

        /// <summary>
        /// Determines whether the calling thread is the thread associated with the given <see cref="T:System.Windows.Window" />.
        /// </summary>
        /// <param name="window">The <see cref="T:System.Windows.Window" /> to be checked.</param>
        /// <returns><see langword="true" /> if the calling thread is the thread associated with this <see cref="T:System.Windows.Window" />; otherwise, <see langword="false" />.</returns>
        public static bool CheckAccess(this Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));

            return window.Dispatcher?.CheckAccess() != false;
        }

        /// <summary>
        /// Executes the specified <see cref="T:System.Action" /> synchronously on the thread the given <see cref="T:System.Windows.Window" /> is associated with.
        /// </summary>
        /// <param name="window">A <see cref="T:System.Windows.Window" />.</param>
        /// <param name="callback">A delegate to invoke through the window thread.</param>
        public static void Invoke(this Window window, Action callback)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            if (CheckAccess(window))
            {
                callback();
            }
            else
            {
                window.Dispatcher.Invoke(callback);
            }
        }

        /// <summary>
        /// Executes the specified <see cref="T:System.Func`1" /> synchronously on the thread the given <see cref="T:System.Windows.Window" /> is associated with.
        /// </summary>
        /// <typeparam name="TResult">The return value type of the specified delegate.</typeparam>
        /// <param name="window">A <see cref="T:System.Windows.Window" />.</param>
        /// <param name="callback">A delegate to invoke through the window thread.</param>
        /// <returns>The result of the given callback.</returns>
        public static TResult Invoke<TResult>(this Window window, Func<TResult> callback)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            if (CheckAccess(window))
            {
                return callback();
            }
            else
            {
                return window.Dispatcher.Invoke<TResult>(callback);
            }
        }

        /// <summary>
        /// Executes the specified <see cref="T:System.Action" /> asynchronously on the thread the given <see cref="T:System.Windows.Window" /> is associated with.
        /// </summary>
        /// <param name="window">A <see cref="T:System.Windows.Window" />.</param>
        /// <param name="callback">A delegate to invoke through the window thread.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> created by this method.</returns>
        public static Task InvokeAsync(this Window window, Action callback)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var operation = window.Dispatcher.InvokeAsync(callback);

            return operation.Task;
        }

        /// <summary>
        /// Executes the specified <see cref="T:System.Func`1" /> asynchronously on the thread the given <see cref="T:System.Windows.Window" /> is associated with.
        /// </summary>
        /// <typeparam name="TResult">The return value type of the specified delegate.</typeparam>
        /// <param name="window">A <see cref="T:System.Windows.Window" />.</param>
        /// <param name="callback">A delegate to invoke through the window thread.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> created by this method.</returns>
        public static Task<TResult> InvokeAsync<TResult>(this Window window, Func<TResult> callback)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var operation = window.Dispatcher.InvokeAsync<TResult>(callback);

            return operation.Task;
        }
    }
}