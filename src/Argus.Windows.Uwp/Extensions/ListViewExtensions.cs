using Windows.UI.Xaml.Controls;

namespace Argus.Extensions
{
    public static class ListViewExtensions
    {
        /// <summary>
        ///     Scrolls to the last record in a ListView.
        /// </summary>
        /// <param name="lv"></param>
        public static void ScrollToEnd(this ListView lv)
        {
            if (lv.Items.Count == 0)
            {
                return;
            }

            var lastItem = lv.Items[lv.Items.Count - 1];
            lv.ScrollIntoView(lastItem);
        }

        /// <summary>
        ///     Scrolls the ListView up by one page based on the size of the ListView.
        /// </summary>
        /// <param name="lv"></param>
        public static void PageUp(this ListView lv)
        {
            var sv = lv.FindFirstChildOfType<ScrollViewer>();
            sv.ChangeView(0, sv.VerticalOffset - sv.ViewportHeight, 1, true);
        }

        /// <summary>
        ///     Scrolls the ListView down by one page based on the size of the ListView.
        /// </summary>
        /// <param name="lv"></param>
        public static void PageDown(this ListView lv)
        {
            var sv = lv.FindFirstChildOfType<ScrollViewer>();
            sv.ChangeView(0, sv.VerticalOffset + sv.ViewportHeight, 1, true);
        }
    }
}