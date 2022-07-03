/*
  * @author            : Blake Pell
  * @initial date      : 2007-06-09
  * @last updated      : 2022-07-03
  * @copyright         : Copyright (c) 2003-2022, All rights reserved.
  * @license           : MIT
  * @website           : http://www.blakepell.com
  */

using System.Runtime.InteropServices;

namespace Argus.Windows
{
    /// <summary>
    /// This class exposes shared methods that will interact with different Windows through the Windows API.
    /// </summary>
    public static class Window
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindWindowByClass(string lpClassName, IntPtr zero);

        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindWindowByCaption(IntPtr zero, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern bool SetForegroundWindowEx(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetForegroundWindow();

        [DllImport("user32", EntryPoint = "GetWindowTextLengthA", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetWindowTextLength(int hwnd);

        [DllImport("user32", EntryPoint = "GetWindowText", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetWindowText(int hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetParent(int hwnd);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        /// <summary>
        /// A rectangle for use with the WinAPI.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            /// <summary>
            /// x position of upper-left corner
            /// </summary>
            public int Left;
            /// <summary>
            /// y position of upper-left corner
            /// </summary>
            public int Top;
            /// <summary>
            /// x position of lower-right corner
            /// </summary>
            public int Right;
            /// <summary>
            /// y position of lower-right corner
            /// </summary>
            public int Bottom;
        }

        /// <summary>
        /// Finds whether the specified window currently exists or not.
        /// </summary>
        /// <param name="name"></param>
        public static bool WindowExists(string name) => !(FindWindow(name, name) == IntPtr.Zero);

        /// <summary>
        /// Returns an IntPtr to the specified window if it exists.  If it does not
        /// exist, IntPtr.Zero is returned.
        /// </summary>
        /// <param name="name"></param>
        public static IntPtr GetWindowHandle(string name) => FindWindow(null, name);

        /// <summary>
        /// Returns the title of the active window.  If returnParent is true then the parent window is
        /// returned if the active window is a MDI or child window.
        /// </summary>
        /// <param name="returnParent">
        /// If true will return the parent window if an app has multiple windows open.  If
        /// false, it returns the name of the child window that is active.
        /// </param>
        public static string GetActiveWindowTitle(bool returnParent)
        {
            int num = 0;
            int hwnd = GetForegroundWindow();

            if (returnParent)
            {
                for (; hwnd != 0; hwnd = GetParent(hwnd))
                {
                    num = hwnd;
                }

                hwnd = num;
            }

            return GetWindowTitle(hwnd);
        }

        /// <summary>
        /// Returns the title of the window by the window handle parameter.
        /// </summary>
        /// <param name="hwnd"></param>
        public static string GetWindowTitle(int hwnd)
        {
            int windowTextLength = GetWindowTextLength(hwnd);
            var sb = new StringBuilder(windowTextLength + 1);

            GetWindowText(hwnd, sb, windowTextLength + 1);

            return sb.ToString();
        }

        /// <summary>
        /// Returns the title of the window by the window handle parameter.
        /// </summary>
        /// <param name="hwnd"></param>
        public static string GetWindowTitle(IntPtr hwnd)
        {
            return GetWindowTitle(hwnd.ToInt32());
        }

        /// <summary>
        /// Returns the <see cref="Process"/> for the first window that matches the criteria.
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="exactMatch"></param>
        public static Process GetProcessByWindowTitle(string windowTitle, bool exactMatch)
        {
            var proc = !exactMatch ? Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle.Contains(windowTitle)) : Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle.Equals(windowTitle, StringComparison.Ordinal));
            return proc;
        }

        /// <summary>
        /// Returns the handle for the window or control, this will search in a parent container provided.
        /// </summary>
        /// <param name="parentHandle">The parent container or window.</param>
        /// <param name="windowTitle"></param>
        /// <param name="className"></param>
        public static IntPtr GetWindowEx(IntPtr parentHandle, string windowTitle, string className)
        {
            return FindWindowEx(parentHandle, IntPtr.Zero, className, windowTitle);
        }

        /// <summary>Sets the focus to the specified window.</summary>
        /// <param name="hwnd"></param>
        public static bool SetForegroundWindow(IntPtr hwnd) => SetForegroundWindowEx(hwnd);

        /// <summary>Sets the focus to the specified window.</summary>
        /// <param name="name"></param>
        public static bool SetForegroundWindow(string name) => SetForegroundWindowEx(FindWindow(name, name));

        /// <summary>Sets the position and size of a window specified by name</summary>
        /// <param name="windowName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetWindowPositionSize(string windowName, int x, int y, int width, int height)
        {
            var hWndInsertAfter = IntPtr.Zero;
            SetWindowPos(FindWindow(null, windowName), hWndInsertAfter, x, y, width, height, 0U);
        }

        /// <summary>Sets the position and the size of the active window.</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetWindowPositionSize(int x, int y, int width, int height)
        {
            var hWndInsertAfter = IntPtr.Zero;
            SetWindowPos(FindWindow(null, GetActiveWindowTitle(true)), hWndInsertAfter, x, y, width, height, 0U);
        }

        /// <summary>
        /// Returns the window position for the given handle.
        /// </summary>
        /// <param name="hwnd"></param>
        public static Rect GetWindowPosition(IntPtr hwnd)
        {
            var rect = new Rect();
            GetWindowRect(hwnd, ref rect);
            return rect;
        }
    }
}