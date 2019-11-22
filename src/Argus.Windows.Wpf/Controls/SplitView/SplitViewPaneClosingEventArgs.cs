namespace Argus.Windows.Wpf.Controls
{
    /// <summary>
    ///     Provides event data for the SplitView.PaneClosing event.
    /// </summary>
    public sealed class SplitViewPaneClosingEventArgs
    {
        /// <summary>
        ///     Gets or sets a value that indicates whether the pane closing action should be
        ///     canceled.
        /// </summary>
        /// <value>true to cancel the pane closing action; otherwise, false.</value>
        public bool Cancel { get; set; }
    }
}