using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Argus.Windows.Wpf
{
    /// <summary>
    ///     WPF Utility methods.
    /// </summary>
    public static class Utilities
    {
        //*********************************************************************************************************************
        //
        //             Class:  Utilities
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  09/19/2019
        //      Last Updated:  11/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Returns a BitmapSource from the icon registered to the current path.
        /// </summary>
        /// <param name="path"></param>
        public static BitmapSource GetIconFromExePath(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            using var icon = Icon.ExtractAssociatedIcon(path);

            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        /// <summary>
        ///     Simulates an action similar to the WinForms DoEvents().
        /// </summary>
        /// <remarks>Ya, I know.</remarks>
        public static void DoEvents()
        {
            Application.Current?.Dispatcher?.Invoke(DispatcherPriority.Background,
                                                    new Action(delegate
                                                    {
                                                    }));
        }
    }
}