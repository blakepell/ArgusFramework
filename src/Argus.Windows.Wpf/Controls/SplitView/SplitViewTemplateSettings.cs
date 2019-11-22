using System.Windows;

namespace Argus.Windows.Wpf.Controls
{
    public class SplitViewTemplateSettings : DependencyObject
    {
        private readonly SplitView splitView;

        public SplitViewTemplateSettings(SplitView splitView)
        {
            this.splitView = splitView;
        }

        public GridLength OpenPaneGridLength => new GridLength(this.OpenPaneLength);
        public double OpenPaneLength => splitView.OpenPaneLength;
        public double OpenPaneLengthMinusCompactLength => this.OpenPaneLength - splitView.CompactPaneLength;
        public GridLength CompactPaneGridLength => new GridLength(splitView.CompactPaneLength);
        public double NegativeOpenPaneLength => -this.OpenPaneLength;
        public double NegativeOpenPaneLengthMinusCompactLength => -this.OpenPaneLengthMinusCompactLength;
    }
}