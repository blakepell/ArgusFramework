using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Argus.Windows.Uwp
{
    /// <summary>
    ///     Wrapper utilities for dealing with the Clipboard in UWP.
    /// </summary>
    public static class Clipboard
    {
        public static void SetText(string text)
        {
            var dp = new DataPackage();
            dp.RequestedOperation = DataPackageOperation.Copy;
            dp.SetText(text);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.Flush();
        }

        /// <summary>
        ///     TODO - Test This.
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetTextAsync()
        {
            var dp = global::Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();

            if (dp.Contains(StandardDataFormats.Text)
                || dp.Contains(StandardDataFormats.Rtf)
                || dp.Contains(StandardDataFormats.Html))
            {
                return await dp.GetTextAsync();
            }

            return "";
        }

        public static void SetApplicationLink(Uri uri)
        {
            var dp = new DataPackage();
            dp.RequestedOperation = DataPackageOperation.Copy;
            dp.SetApplicationLink(uri);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.Flush();
        }

        public static void SetBitmap(RandomAccessStreamReference stream)
        {
            var dp = new DataPackage();
            dp.RequestedOperation = DataPackageOperation.Copy;
            dp.SetBitmap(stream);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.Flush();
        }

        public static void SetApplicationLink(string formatId, DataProviderHandler dataProviderHandler)
        {
            var dp = new DataPackage();
            dp.RequestedOperation = DataPackageOperation.Copy;
            dp.SetDataProvider(formatId, dataProviderHandler);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.Flush();
        }

        public static void SetHtmlFormat(string html)
        {
            var dp = new DataPackage();
            dp.RequestedOperation = DataPackageOperation.Copy;
            dp.SetHtmlFormat(html);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.Flush();
        }

        public static void SetRtf(string rtf)
        {
            var dp = new DataPackage();
            dp.RequestedOperation = DataPackageOperation.Copy;
            dp.SetRtf(rtf);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.Flush();
        }

        public static void SetStorageItems(IEnumerable<IStorageItem> storageItems, bool readOnly = false)
        {
            var dp = new DataPackage();
            dp.RequestedOperation = DataPackageOperation.Copy;
            dp.SetStorageItems(storageItems, readOnly);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.Flush();
        }

        public static void SetWebLink(Uri uri)
        {
            var dp = new DataPackage();
            dp.RequestedOperation = DataPackageOperation.Copy;
            dp.SetWebLink(uri);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
            global::Windows.ApplicationModel.DataTransfer.Clipboard.Flush();
        }

        public static void Clear()
        {
            global::Windows.ApplicationModel.DataTransfer.Clipboard.Clear();
            global::Windows.ApplicationModel.DataTransfer.Clipboard.ClearHistory();
        }

        public static bool IsRoamingEnabled()
        {
            return global::Windows.ApplicationModel.DataTransfer.Clipboard.IsRoamingEnabled();
        }

        public static bool IsHistoryEnabled()
        {
            return global::Windows.ApplicationModel.DataTransfer.Clipboard.IsHistoryEnabled();
        }
    }
}