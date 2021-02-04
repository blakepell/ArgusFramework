/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Argus.Windows.Uwp
{
    /// <summary>
    /// Supported dialog results for use with the Dialog Helper.
    /// </summary>
    public enum DialogResult
    {
        Yes,
        No,
        Cancel
    }

    /// <summary>
    /// Dialog box helpers for common scenarios.
    /// </summary>
    public static class Dialog
    {
        /// <summary>
        /// Display a message box.
        /// </summary>
        /// <param name="message"></param>
        public static async Task Show(string message)
        {
            await Show(message, Package.Current.DisplayName);
        }

        /// <summary>
        /// Display a message box.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        public static async Task Show(string message, string title)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok"
            };

            await dialog.ShowAsync();
        }

        /// <summary>
        /// Shows a yes/no dialog box that will return true for yes and false for no.
        /// </summary>
        /// <param name="message"></param>
        public static async Task<DialogResult> ShowYesNo(string message)
        {
            return await ShowYesNo(message, Package.Current.DisplayName);
        }

        /// <summary>
        /// Shows a yes/no dialog box that will return true for yes and false for no.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static async Task<DialogResult> ShowYesNo(string message, string title)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };

            var signal = new TaskCompletionSource<DialogResult>();

            dialog.PrimaryButtonClick += (o, e) =>
            {
                dialog.Hide();
                signal.SetResult(DialogResult.Yes);
            };

            dialog.SecondaryButtonClick += (o, e) =>
            {
                dialog.Hide();
                signal.SetResult(DialogResult.No);
            };

            // https://stackoverflow.com/questions/55305898/contentdialog-delay-after-ok-clicked
            // We will not await the dialog, we will await the signal so the caller page gets the result instantly without
            // a lagged delay that is noticable.
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            dialog.ShowAsync();
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            await signal.Task;

            return signal.Task.Result;
        }

        /// <summary>
        /// Shows a yes/no dialog box that will return true for yes and false for no.
        /// </summary>
        /// <param name="message"></param>
        public static async Task<DialogResult> ShowYesNoCancel(string message)
        {
            return await ShowYesNoCancel(message, Package.Current.DisplayName);
        }

        /// <summary>
        /// Shows a yes/no dialog box that will return true for yes and false for no.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static async Task<DialogResult> ShowYesNoCancel(string message, string title)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                CloseButtonText = "Cancel"
            };

            var signal = new TaskCompletionSource<DialogResult>();

            dialog.PrimaryButtonClick += (o, e) =>
            {
                dialog.Hide();
                signal.SetResult(DialogResult.Yes);
            };

            dialog.SecondaryButtonClick += (o, e) =>
            {
                dialog.Hide();
                signal.SetResult(DialogResult.No);
            };

            dialog.CloseButtonClick += (o, e) =>
            {
                dialog.Hide();
                signal.SetResult(DialogResult.Cancel);
            };

            // https://stackoverflow.com/questions/55305898/contentdialog-delay-after-ok-clicked
            // We will not await the dialog, we will await the signal so the caller page gets the result instantly without
            // a lagged delay that is noticable.
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            dialog.ShowAsync();
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            await signal.Task;

            return signal.Task.Result;
        }

        /// <summary>
        /// Shows an input box to get a string value in return.
        /// </summary>
        /// <param name="message"></param>
        public static async Task<string> ShowInput(string message)
        {
            return await ShowInput(message, "", Package.Current.DisplayName);
        }

        /// <summary>
        /// Shows an input box to get a string value in return.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="defaultValue"></param>
        public static async Task<string> ShowInput(string message, string defaultValue)
        {
            return await ShowInput(message, defaultValue, Package.Current.DisplayName);
        }

        /// <summary>
        /// Shows an simple input box to get a string value in return.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="defaultValue"></param>
        public static async Task<string> ShowInput(string message, string defaultValue, string title)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                CloseButtonText = "Cancel"
            };

            var panel = new StackPanel();
            panel.Children.Add(new TextBlock {Text = message, TextWrapping = TextWrapping.Wrap});

            var textBox = new TextBox
            {
                Text = defaultValue
            };

            textBox.SelectAll();

            var signal = new TaskCompletionSource<string>();

            textBox.KeyUp += (o, e) =>
            {
                if (e.Key == VirtualKey.Enter)
                {
                    dialog.Hide();
                    signal.SetResult(textBox.Text);
                }

                e.Handled = true;
            };

            dialog.PrimaryButtonClick += (o, e) =>
            {
                dialog.Hide();
                signal.SetResult(textBox.Text);
            };

            dialog.CloseButtonClick += (o, e) =>
            {
                dialog.Hide();
                signal.SetResult("");
            };

            panel.Children.Add(textBox);

            // https://stackoverflow.com/questions/55305898/contentdialog-delay-after-ok-clicked
            // We will not await the dialog, we will await the signal so the caller page gets the result instantly without
            // a lagged delay that is noticable.
            dialog.Content = panel;
            dialog.PrimaryButtonText = "OK";
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            dialog.ShowAsync();
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            await signal.Task;

            return signal.Task.Result;
        }

        /// <summary>
        /// Shows an password input dialog box.
        /// </summary>
        /// <param name="message"></param>
        public static async Task<string> ShowInputPassword(string message)
        {
            return await ShowInputPassword(message, Package.Current.DisplayName, true);
        }

        /// <summary>
        /// Shows an password input dialog box.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        public static async Task<string> ShowInputPassword(string message, string title)
        {
            return await ShowInputPassword(message, title, true);
        }

        /// <summary>
        /// Shows an password input dialog box.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="allowPeek"></param>
        public static async Task<string> ShowInputPassword(string message, string title, bool allowPeek)
        {
            var dialog = new ContentDialog
            {
                Title = title
            };

            var panel = new StackPanel();
            panel.Children.Add(new TextBlock {Text = message, TextWrapping = TextWrapping.Wrap});

            var passwordBox = new PasswordBox();

            if (allowPeek)
            {
                passwordBox.PasswordRevealMode = PasswordRevealMode.Peek;
            }

            var signal = new TaskCompletionSource<string>();

            passwordBox.KeyUp += (o, e) =>
            {
                if (e.Key == VirtualKey.Enter)
                {
                    dialog.Hide();
                    signal.SetResult(passwordBox.Password);
                }

                e.Handled = true;
            };

            panel.Children.Add(passwordBox);

            // https://stackoverflow.com/questions/55305898/contentdialog-delay-after-ok-clicked
            // We will not await the dialog, we will await the signal so the caller page gets the result instantly without
            // a lagged delay that is noticable.
            dialog.Content = panel;
            dialog.PrimaryButtonText = "OK";
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            dialog.ShowAsync();
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            await signal.Task;

            return signal.Task.Result;
        }

        /// <summary>
        /// Shows the open file picker and returns the selected file (or null).  This uses the default location as the
        /// Documents Library.
        /// </summary>
        public static async Task<StorageFile> ShowOpenPicker()
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            picker.FileTypeFilter.Add("*");

            return await picker.PickSingleFileAsync();
        }

        /// <summary>
        /// Shows the open file picker and returns the selected file (or null).  This uses the default location as the
        /// Documents Library.
        /// </summary>
        /// <param name="fileTypeFilters"></param>
        /// <param name="defaultLocation"></param>
        public static async Task<StorageFile> ShowOpenPicker(List<string> fileTypeFilters, PickerLocationId defaultLocation)
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = defaultLocation
            };

            foreach (string item in fileTypeFilters)
            {
                picker.FileTypeFilter.Add(item);
            }

            return await picker.PickSingleFileAsync();
        }

        /// <summary>
        /// Shows the save file picker and returns the selected file (or null).  This uses the default location as the
        /// Documents Library.
        /// </summary>
        public static async Task<StorageFile> ShowSavePicker()
        {
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            picker.FileTypeChoices.Add("Any File", new List<string> {"."});
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            return await picker.PickSaveFileAsync();
        }

        /// <summary>
        /// Shows the save file picker and returns the selected file (or null).  This uses the default location as the
        /// Documents Library.
        /// </summary>
        public static async Task<StorageFile> ShowSavePicker(string fileTypeCategory, List<string> fileTypeChoices, PickerLocationId defaultLocation)
        {
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = defaultLocation
            };

            picker.FileTypeChoices.Add(fileTypeCategory, fileTypeChoices);
            picker.FileTypeChoices.Add("Any File", new List<string> {"."});

            return await picker.PickSaveFileAsync();
        }

        /// <summary>
        /// Shows a page as a ContentDialog.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="title"></param>
        /// <param name="showCancel"></param>
        public static async Task<ContentDialogResult> ShowPage(Page page, string title, bool showCancel = true)
        {
            var d = new ContentDialog
            {
                Title = title,
                Content = page,
                PrimaryButtonText = "Ok"
            };

            if (showCancel)
            {
                d.CloseButtonText = "Cancel";
            }

            return await d.ShowAsync();
        }

        /// <summary>
        /// Returns whether a ContentDialog is open or not.  This uses VisualTreeHelper.GetOpenPopups to look
        /// for ContentDialog's that are open.
        /// </summary>
        public static bool IsContentDialogOpen()
        {
            var contentDialogs = VisualTreeHelper.GetOpenPopups(Window.Current);
            bool contentDialogVisible = false;

            foreach (var dialog in contentDialogs)
            {
                if (dialog.Child != null && dialog.Child is ContentDialog)
                {
                    contentDialogVisible = true;

                    break;
                }
            }

            return contentDialogVisible;
        }
    }
}