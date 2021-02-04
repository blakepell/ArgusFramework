/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2007-09-16
 * @last updated      : 2016-0607
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GraphicsFx = System.Drawing.Graphics;

namespace Argus.Windows.Forms
{
    /// <summary>
    /// This class can be used to take a screenshot of the current window, the desktop, multiple desktops or specified
    /// sections of the desktop.  It returns all screenshots as a System.Drawing.Bitmap
    /// </summary>
    /// <remarks>
    /// <code>
    /// ' Example of GraphicsFx.CopyFromScreen with a MemoryStream
    /// Dim ms As New System.IO.MemoryStream
    /// Dim bm As New System.Drawing.Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb)
    /// Dim screenShot As GraphicsFx = GraphicsFx.FromImage(bm)
    /// screenShot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy)
    /// bm.Save(ms, ImageFormat.Jpeg)
    /// Dim imageBytes As Byte() = ms.ToArray
    /// Return imageBytes
    /// </code>
    /// </remarks>
    public static class Screenshot
    {

        /// <summary>
        /// The method that is used to take the screenshot.
        /// </summary>
        public enum ScreenshotMethod
        {
            /// <summary>
            /// The BitBlt method using the BitBlt Windows API in the gdi32 library file.  Using the Windows API may provide
            /// benefits but also may break with future OS releases.
            /// </summary>
            BitBlt,

            /// <summary>
            /// The CopyFromScreen method uses .Net's GraphicsFx class to take a screenshot.  This method uses all managed code
            /// from the .Net Framework and should be sheltered from changes in the OS.
            /// </summary>
            /// <remarks>
            /// At the writing of this code, the GraphicsFx.CopyFromScreen method had some issues with copying pixels in regards
            /// to Aero's transparency which is why both the BitBlt and this method are provided.
            /// </remarks>
            CopyFromScreen
        }

        private const int SRCCOPY = 0xcc0020;

        [DllImport("gdi32")]
        public static extern bool BitBlt(IntPtr hDestDC, int X, int Y, int nWidth, int nHeight, IntPtr hSrcDC, int SrcX, int SrcY, int Rop);

        [DllImport("user32.dll")]
        private static extern int GetWindowDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        /// <summary>
        /// Takes a screenshot of the primary screen and returns it as a Sysem.Drawing.Bitmap.
        /// This function uses the BitBlt Windows API to take the screenshot.
        /// </summary>
        public static Bitmap GetScreenshotPrimaryScreen()
        {
            GraphicsFx g = default;
            var hdcDest = IntPtr.Zero;
            var desktopHandleDC = IntPtr.Zero;
            var desktopHandle = GetDesktopWindow();
            var bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            g = GraphicsFx.FromImage(bmp);
            desktopHandleDC = (IntPtr) GetWindowDC(desktopHandle);
            hdcDest = g.GetHdc();
            BitBlt(hdcDest, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, desktopHandleDC, 0, 0, SRCCOPY);

            g.ReleaseHdc(hdcDest);
            ReleaseDC(desktopHandle, desktopHandleDC);

            g.Dispose();
            g = null;

            return bmp;
        }

        /// <summary>
        /// Returns a list of bitmaps that contain a bitmap for every display screen.  This method uses the GraphicsFx.CopyFromScreen
        /// method.
        /// </summary>
        public static List<Bitmap> GetScreenshotAllScreens()
        {
            var bmpList = new List<Bitmap>();

            foreach (var sc in Screen.AllScreens)
            {
                var g = default(GraphicsFx);
                var bmp = new Bitmap(sc.Bounds.Width, sc.Bounds.Height);

                g = GraphicsFx.FromImage(bmp);
                g.CopyFromScreen(sc.Bounds.Left, sc.Bounds.Top, 0, 0, new Size(sc.Bounds.Width, sc.Bounds.Height));

                bmpList.Add(bmp);
                g.Dispose();
                g = null;
            }

            return bmpList;
        }

        /// <summary>
        /// Takes a screenshot of the current window and return it as a System.Drawing.Bitmap.
        /// This function uses the BitBlt Windows API to take the screenshot.
        /// </summary>
        public static Bitmap GetScreenshotCurrentWindow()
        {
            var g = default(GraphicsFx);
            var hdcDest = IntPtr.Zero;
            var windowHandleDC = IntPtr.Zero;
            var windowHandle = GetForegroundWindow();

            var windowRect = default(RECT);
            var bmp = default(Bitmap);

            if (GetWindowRect(windowHandle, out windowRect))
            {
                bmp = new Bitmap(windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top);
            }
            else
            {
                return null;
            }

            g = GraphicsFx.FromImage(bmp);
            windowHandleDC = (IntPtr) GetWindowDC(windowHandle);
            hdcDest = g.GetHdc();
            BitBlt(hdcDest, 0, 0, bmp.Width, bmp.Height, windowHandleDC, 0, 0, SRCCOPY);

            g.ReleaseHdc(hdcDest);
            ReleaseDC(windowHandle, windowHandleDC);

            g.Dispose();
            g = null;

            return bmp;
        }

        /// <summary>
        /// Takes a screenshot associated with the given handle (be it a window or control) and return it
        /// as a System.Drawing.Bitmap.
        /// This function uses the BitBlt Windows API to take the screenshot.
        /// </summary>
        /// <param name="handle"></param>
        public static Bitmap GetScreenshotByHandle(IntPtr handle)
        {
            var g = default(GraphicsFx);
            var hdcDest = IntPtr.Zero;
            var windowHandleDC = IntPtr.Zero;

            var windowRect = default(RECT);
            var bmp = default(Bitmap);

            if (GetWindowRect(handle, out windowRect))
            {
                bmp = new Bitmap(windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top);
            }
            else
            {
                return null;
            }

            g = GraphicsFx.FromImage(bmp);
            windowHandleDC = (IntPtr) GetWindowDC(handle);
            hdcDest = g.GetHdc();
            BitBlt(hdcDest, 0, 0, bmp.Width, bmp.Height, windowHandleDC, 0, 0, SRCCOPY);

            g.ReleaseHdc(hdcDest);
            ReleaseDC(handle, windowHandleDC);

            g.Dispose();
            g = null;

            return bmp;
        }

        /// <summary>
        /// Takes a screenshot of the specified location and returns it as a System.Drawing.Bitmap.
        /// This function uses the BitBlt Windows API to take the screenshot.
        /// </summary>
        /// <param name="rectLoc"></param>
        public static Bitmap GetScreenshotByLocation(Rectangle rectLoc)
        {
            var g = default(GraphicsFx);
            var hdcDest = IntPtr.Zero;
            var windowHandleDC = IntPtr.Zero;

            var windowRect = default(RECT);

            windowRect.Left = rectLoc.Left;
            windowRect.Right = rectLoc.Right;
            windowRect.Top = rectLoc.Top;
            windowRect.Bottom = rectLoc.Bottom;

            var bmp = new Bitmap(windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top);

            g = GraphicsFx.FromImage(bmp);
            windowHandleDC = GetDesktopWindow();
            hdcDest = g.GetHdc();
            BitBlt(hdcDest, 0, 0, bmp.Width, bmp.Height, windowHandleDC, windowRect.Left, windowRect.Top, SRCCOPY);

            g.ReleaseHdc(hdcDest);
            ReleaseDC(GetDesktopWindow(), windowHandleDC);

            g.Dispose();
            g = null;

            return bmp;
        }

        /// <summary>
        /// Rectangle structure to pass to the Windows APIs
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}