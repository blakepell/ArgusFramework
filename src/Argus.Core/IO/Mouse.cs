/*
 * @author            : Blake Pell
 * @initial date      : 2007-06-09
 * @last updated      : 2022-06-04
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Argus.IO
{
    /// <summary>
    /// This class offers access to mouse information and manipulation procedures
    /// on Windows OS's.
    /// </summary>
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Mouse
    {
        /// <summary>
        /// Left button down.
        /// </summary>
        private const int MOUSEEVENTF_LEFTDOWN = 2;

        /// <summary>
        /// Left button up.
        /// </summary>
        private const int MOUSEEVENTF_LEFTUP = 4;

        /// <summary>
        /// Middle button down.
        /// </summary>
        private const int MOUSEEVENTF_MIDDLEDOWN = 32;

        /// <summary>
        /// Middle button up.
        /// </summary>
        private const int MOUSEEVENTF_MIDDLEUP = 64;

        /// <summary>
        /// Right button down.
        /// </summary>
        private const int MOUSEEVENTF_RIGHTDOWN = 8;

        /// <summary>
        /// Right button up.
        /// </summary>
        private const int MOUSEEVENTF_RIGHTUP = 16;

        /// <summary>
        /// Mouse move event.
        /// </summary>
        private const int MOUSEEVENTF_MOVE = 1;

        /// <summary>
        /// Mouse wheel movement event. 
        /// </summary>
        private const int MOUSEEVENTF_WHEEL = 0x0800;

        /// <summary>Windows API declaration for mouse_event in user32.dll.</summary>
        /// <param name="dwFlags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="cButtons"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        /// <summary>Windows API declaration for SetCursorPos in user32.dll.</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [DllImport("User32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetCursorPos(int x, int y);

        /// <summary>
        /// Windows API declaration for GetCursorPos in user32.dll.
        /// </summary>
        /// <param name="lpPoint"></param>
        [DllImport("User32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetCursorPos(ref Point lpPoint);

        /// <summary>
        /// Windows API declaration for ShowCursor in user32.dll.
        /// </summary>
        /// <param name="bShow"></param>
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ShowCursor(bool bShow);

        /// <summary>
        /// The current X position of the mouse as located on the screen.
        /// </summary>
        public static int X()
        {
            Point lpPoint = default;
            GetCursorPos(ref lpPoint);
            return lpPoint.X;
        }

        /// <summary>
        /// The current Y position of the mouse as located on the screen.
        /// </summary>
        public static int Y()
        {
            Point lpPoint = default;
            GetCursorPos(ref lpPoint);
            return lpPoint.Y;
        }

        /// <summary>
        /// Send a left mouse click.
        /// </summary>
        public static void LeftClick()
        {
            LeftDown();
            LeftUp();
        }

        /// <summary>
        /// Press down on the left mouse button.
        /// </summary>
        public static void LeftDown() => mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);

        /// <summary>
        /// Let up on the left mouse button.
        /// </summary>
        public static void LeftUp() => mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

        /// <summary>
        /// A middle mouse click.
        /// </summary>
        public static void MiddleClick()
        {
            MiddleDown();
            MiddleUp();
        }

        /// <summary>
        /// Press down on the middle mouse button.
        /// </summary>
        public static void MiddleDown() => mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0);

        /// <summary>
        /// Let up on the middle mouse button.
        /// </summary>
        public static void MiddleUp() => mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);

        /// <summary>
        /// A right mouse click.
        /// </summary>
        public static void RightClick()
        {
            RightDown();
            RightUp();
        }

        /// <summary>
        /// Press down on the right mouse button.
        /// </summary>
        public static void RightDown() => mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);

        /// <summary>
        /// Let up on the right mouse button.
        /// </summary>
        public static void RightUp() => mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);

        /// <summary>
        /// Move the mouse to the specified X and Y coordinate on the screen.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Move(int x, int y) => mouse_event(MOUSEEVENTF_MOVE, x, y, 0, 0);

        /// <summary>
        /// Scrolls the mouse wheel a positive or negative amount of pixels.
        /// </summary>
        /// <param name="pixels">A negative value scrolls down and a positive value to scrolls up.</param>
        public static void ScrollWheel(int pixels) => mouse_event(MOUSEEVENTF_WHEEL, 0, 0, pixels, 0);

        /// <summary>
        /// Sets the mouse's position.  This is different than the mouse move event in that mouse relocates
        /// without firing the mouse move event.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SetMousePos(int x, int y) => SetCursorPos(x, y);

        /// <summary>
        /// This will hide the mouse over the a window you own, not over Windows as a whole.
        /// </summary>
        public static void MouseHide() => ShowCursor(false);

        /// <summary>
        /// This will un-hide/show the mouse over a window you own, not over Windows as a whole.
        /// </summary>
        public static void MouseShow() => ShowCursor(true);
    }
}