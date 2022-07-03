/*
 * @author            : Matteo Pagani (qmatteoq)
 * @git               : https://github.com/qmatteoq/DesktopBridgeHelpers
 * @license           : MIT 
 */

using System.Runtime.InteropServices;

namespace Argus.Windows
{
    /// <summary>
    /// Windows Desktop Bridge Helpers
    /// </summary>
    public class DesktopBridge
    {
        private const long APPMODEL_ERROR_NO_PACKAGE = 15700L;

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder packageFullName);

        /// <summary>
        /// Whether the app is running in a UWP container or if it is running natively.
        /// </summary>
        public bool IsRunningAsUwp()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || IsWindows7OrLower)
            {
                return false;
            }

            int length = 0;
            var sb = new StringBuilder(0);
            _ = GetCurrentPackageFullName(ref length, sb);

            sb = new StringBuilder(length);
            int result = GetCurrentPackageFullName(ref length, sb);

            return result != APPMODEL_ERROR_NO_PACKAGE;
        }

        /// <summary>
        /// If the OS is Windows 7 or older.
        /// </summary>
        /// <remarks>
        /// UWP was not supported until Windows 8.
        /// </remarks>
        private bool IsWindows7OrLower
        {
            get
            {
                int versionMajor = Environment.OSVersion.Version.Major;
                int versionMinor = Environment.OSVersion.Version.Minor;
                double version = versionMajor + (double)versionMinor / 10;
                return version <= 6.1;
            }
        }
    }
}
