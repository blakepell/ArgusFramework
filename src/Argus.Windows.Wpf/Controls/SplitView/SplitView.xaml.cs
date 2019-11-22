using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Argus.Windows.Wpf.Controls
{
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "Closed")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "ClosedCompactLeft")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "ClosedCompactRight")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "OpenOverlayLeft")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "OpenOverlayRight")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "OpenInlineLeft")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "OpenInlineRight")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "OpenCompactOverlayLeft")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "OpenCompactOverlayRight")]
    [TemplatePart(Name = "LightDismissLayer", Type = typeof(UIElement))]
    [DefaultProperty("Content")]
    [ContentProperty("Content")]
    public partial class SplitView : Control
    {
        public static readonly DependencyProperty PaneProperty =
            DependencyProperty.Register("Pane", typeof(UIElement), typeof(SplitView), new PropertyMetadata(null));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement), typeof(SplitView), new PropertyMetadata(null));

        public static readonly DependencyProperty CompactPaneLengthProperty =
            DependencyProperty.Register("CompactPaneLength", typeof(double), typeof(SplitView), new PropertyMetadata((double) 48, OnLengthChanged));

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(SplitViewDisplayMode), typeof(SplitView), new PropertyMetadata(SplitViewDisplayMode.Overlay, OnVisualStateChanged));

        public static readonly DependencyProperty IsPaneOpenProperty =
            DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(SplitView), new PropertyMetadata(false, OnIsPaneOpenChanged));

        public static readonly DependencyProperty LightDismissOverlayModeProperty =
            DependencyProperty.Register("LightDismissOverlayMode", typeof(LightDismissOverlayMode), typeof(SplitView), new PropertyMetadata(LightDismissOverlayMode.Auto));

        public static readonly DependencyProperty OpenPaneLengthProperty =
            DependencyProperty.Register("OpenPaneLength", typeof(double), typeof(SplitView), new PropertyMetadata((double) 320, OnLengthChanged));

        public static readonly DependencyProperty PaneBackgroundProperty =
            DependencyProperty.Register("PaneBackground", typeof(Brush), typeof(SplitView), new PropertyMetadata(Brushes.DimGray));

        public static readonly DependencyProperty PanePlacementProperty =
            DependencyProperty.Register("PanePlacement", typeof(SplitViewPanePlacement), typeof(SplitView), new PropertyMetadata(SplitViewPanePlacement.Left, OnVisualStateChanged));

        public static readonly DependencyProperty TemplateSettingsProperty =
            DependencyProperty.Register("TemplateSettings", typeof(SplitViewTemplateSettings), typeof(SplitView), new PropertyMetadata());

        public SplitView()
        {
            this.TemplateSettings = new SplitViewTemplateSettings(this);
            this.InitializeComponent();
        }

        public UIElement Pane
        {
            get => (UIElement) this.GetValue(PaneProperty);
            set => this.SetValue(PaneProperty, value);
        }

        public UIElement Content
        {
            get => (UIElement) this.GetValue(ContentProperty);
            set => this.SetValue(ContentProperty, value);
        }

        public double CompactPaneLength
        {
            get => (double) this.GetValue(CompactPaneLengthProperty);
            set => this.SetValue(CompactPaneLengthProperty, value);
        }

        public SplitViewDisplayMode DisplayMode
        {
            get => (SplitViewDisplayMode) this.GetValue(DisplayModeProperty);
            set => this.SetValue(DisplayModeProperty, value);
        }

        public bool IsPaneOpen
        {
            get => (bool) this.GetValue(IsPaneOpenProperty);
            set => this.SetValue(IsPaneOpenProperty, value);
        }

        public LightDismissOverlayMode LightDismissOverlayMode
        {
            get => (LightDismissOverlayMode) this.GetValue(LightDismissOverlayModeProperty);
            set => this.SetValue(LightDismissOverlayModeProperty, value);
        }

        public double OpenPaneLength
        {
            get => (double) this.GetValue(OpenPaneLengthProperty);
            set => this.SetValue(OpenPaneLengthProperty, value);
        }

        public Brush PaneBackground
        {
            get => (Brush) this.GetValue(PaneBackgroundProperty);
            set => this.SetValue(PaneBackgroundProperty, value);
        }

        public SplitViewPanePlacement PanePlacement
        {
            get => (SplitViewPanePlacement) this.GetValue(PanePlacementProperty);
            set => this.SetValue(PanePlacementProperty, value);
        }

        private bool IsCompact
        {
            get
            {
                if (this.DisplayMode == SplitViewDisplayMode.CompactInline || this.DisplayMode == SplitViewDisplayMode.CompactOverlay)
                {
                    return true;
                }

                return false;
            }
        }

        private bool IsInline
        {
            get
            {
                if (this.DisplayMode == SplitViewDisplayMode.CompactInline || this.DisplayMode == SplitViewDisplayMode.Inline)
                {
                    return true;
                }

                return false;
            }
        }

        public SplitViewTemplateSettings TemplateSettings
        {
            get => (SplitViewTemplateSettings) this.GetValue(TemplateSettingsProperty);
            set => this.SetValue(TemplateSettingsProperty, value);
        }

        public override void OnApplyTemplate()
        {
            this.TemplateSettings = new SplitViewTemplateSettings(this);

            base.OnApplyTemplate();

            UIElement lightDismissLayer;

            lightDismissLayer = this.GetTemplateChild("LightDismissLayer") as UIElement;

            if (lightDismissLayer != null)
            {
                lightDismissLayer.PreviewTouchDown += this.OnLightDismiss;
                lightDismissLayer.PreviewStylusDown += this.OnLightDismiss;
                lightDismissLayer.PreviewMouseDown += this.OnLightDismiss;
            }

            this.OnVisualStateChanged();
        }

        protected virtual void OnLightDismiss()
        {
            if (this.LightDismissOverlayMode == LightDismissOverlayMode.On || this.LightDismissOverlayMode == LightDismissOverlayMode.Auto)
            {
                if (this.IsPaneOpen && !this.IsInline)
                {
                    this.IsPaneOpen = false;
                }
            }
        }

        private void OnLightDismiss(object sender, MouseButtonEventArgs e)
        {
            this.OnLightDismiss();
        }

        private void OnLightDismiss(object sender, StylusDownEventArgs e)
        {
            this.OnLightDismiss();
        }

        private void OnLightDismiss(object sender, TouchEventArgs e)
        {
            this.OnLightDismiss();
        }

        private static void OnLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SplitView;
            sv.OnLengthChanged();
        }

        private void OnLengthChanged()
        {
            this.TemplateSettings = new SplitViewTemplateSettings(this);
        }

        private static void OnIsPaneOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SplitView;
            sv.OnIsPaneOpenChanged((bool) e.NewValue);
        }

        protected virtual void OnIsPaneOpenChanged(bool newValue)
        {
            if (newValue)
            {
                this.OnVisualStateChanged();
            }
            else
            {
                if (this.PaneClosing != null)
                {
                    var args = new SplitViewPaneClosingEventArgs();

                    foreach (TypedEventHandler<SplitView, SplitViewPaneClosingEventArgs> handler in this.PaneClosing.GetInvocationList())
                    {
                        handler(this, args);

                        if (args.Cancel)
                        {
                            this.IsPaneOpen = true;

                            return;
                        }
                    }
                }

                this.OnVisualStateChanged();
                this.PaneClosed?.Invoke(this, null);
            }
        }

        private static void OnVisualStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SplitView;
            sv.OnVisualStateChanged();
        }

        protected virtual void OnVisualStateChanged(bool useTransitions = true)
        {
            VisualStateManager.GoToState(this, this.GetVisualState(), useTransitions);
        }

        protected virtual string GetVisualState()
        {
            string state = string.Empty;

            if (this.IsPaneOpen)
            {
                state = "Open";
                state += this.IsInline ? "Inline" : this.DisplayMode.ToString();
            }
            else
            {
                state = "Closed";

                if (this.IsCompact)
                {
                    state += "Compact";
                }
                else
                {
                    return state;
                }
            }

            state += this.PanePlacement.ToString();

            return state;
        }

        public event TypedEventHandler<SplitView, object> PaneClosed;
        public event TypedEventHandler<SplitView, SplitViewPaneClosingEventArgs> PaneClosing;
    }
}