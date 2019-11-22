using Windows.UI.Xaml;

namespace Argus.Windows.Uwp
{
    /// <summary>
    ///     Utilities to assist with color themes.
    /// </summary>
    public static class Theme
    {
        /// <summary>
        ///     Supported OS themes.
        /// </summary>
        public enum SystemColorTheme
        {
            Light = 0,
            Dark = 1
        }

        /// <summary>
        ///     Returns the current system color theme, either light or dark.
        /// </summary>
        /// <returns></returns>
        public static SystemColorTheme CurrentColorTheme()
        {
            bool isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;

            if (isDark)
            {
                return SystemColorTheme.Dark;
            }

            return SystemColorTheme.Light;
        }
    }
}