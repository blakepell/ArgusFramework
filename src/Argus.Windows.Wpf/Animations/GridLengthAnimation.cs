/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Argus.Windows.Wpf.Animations
{
    /// <summary>
    /// This class inherits from GridLengthAnimationBase and implements the specifics of the animation.
    /// </summary>
    /// <remarks>
    /// <code>
    /// Grid grid = new Grid();
    /// grid.RowDefinitions.Add(new RowDefinition());
    /// GridLengthAnimation gla = new GridLengthAnimation();
    /// gla.From = new GridLength(100);  // Starting from 100 units height
    /// gla.To = new GridLength(200);    // Animate to 200 units height
    /// gla.Duration = new Duration(TimeSpan.FromSeconds(2)); // Over 2 seconds
    /// // Optionally, add an easing function
    /// gla.EasingFunction = new BounceEase();
    /// // Apply the animation to the grid's first row height
    /// grid.RowDefinitions[0].BeginAnimation(RowDefinition.HeightProperty, gla);
    /// </code>
    /// </remarks>
    public class GridLengthAnimation : GridLengthAnimationBase
    {
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(GridLength?), typeof(GridLengthAnimation), new PropertyMetadata(null));

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(GridLength?), typeof(GridLengthAnimation), new PropertyMetadata(null));

        public static readonly DependencyProperty ByProperty =
            DependencyProperty.Register("By", typeof(double?), typeof(GridLengthAnimation), new PropertyMetadata(null));

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(GridLengthAnimation));

        public GridLengthAnimation()
        {
        }

        public GridLengthAnimation(GridLength toValue, Duration duration)
            : this()
        {
            this.To = toValue;
            this.Duration = duration;
        }

        public GridLengthAnimation(GridLength toValue, Duration duration, FillBehavior fillBehavior)
            : this(toValue, duration)
        {
            this.FillBehavior = fillBehavior;
        }

        public GridLengthAnimation(GridLength fromValue, GridLength toValue, Duration duration)
            : this(toValue, duration)
        {
            this.From = fromValue;
        }

        public GridLengthAnimation(GridLength fromValue, GridLength toValue, Duration duration, FillBehavior fillBehavior)
            : this(fromValue, toValue, duration)
        {
            this.FillBehavior = fillBehavior;
        }

        public override bool IsDestinationDefault => false;

        /// <summary>
        /// The starting value of the animation.
        /// </summary>
        public GridLength? From
        {
            get => (GridLength?) this.GetValue(FromProperty);
            set => this.SetValue(FromProperty, value);
        }

        /// <summary>
        /// The ending value of the animation.
        /// </summary>
        public GridLength? To
        {
            get => (GridLength?) this.GetValue(ToProperty);
            set => this.SetValue(ToProperty, value);
        }

        public double? By
        {
            get => (double?) this.GetValue(ByProperty);
            set => this.SetValue(ByProperty, value);
        }

        /// <summary>
        /// Provides a way to customize the speed progression of the animation, making it non-linear if desired.
        /// </summary>
        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction) this.GetValue(EasingFunctionProperty);
            set => this.SetValue(EasingFunctionProperty, value);
        }

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        protected override GridLength GetCurrentValueCore(GridLength defaultOriginValue, GridLength defaultDestinationValue, AnimationClock animationClock)
        {
            if (this.From == null && this.To == null && this.By == null)
            {
                throw new Exception("Unknown animation type");
            }

            var from = this.From ?? defaultOriginValue;
            var to = this.To ?? defaultDestinationValue;

            var progress = animationClock.CurrentProgress; // What if it is null? Is GetCurrentValueCore ever called?

            //https://msdn.microsoft.com/en-us/library/system.windows.media.animation.clock.currentprogress(v=vs.110).aspx
            if (this.EasingFunction != null)
            {
                progress = this.EasingFunction.Ease((double) progress);
            }

            if (this.To == null && this.By != null)
            {
                this.To = this.From == null ? new GridLength(defaultOriginValue.Value + (double) this.By) : new GridLength(this.From.Value.Value + (double) this.By);
            }

            if (from.GridUnitType != to.GridUnitType)
            {
                return to;
            }

            return new GridLength(from.Value + (to.Value - from.Value) * progress.Value, from.GridUnitType);
        }
    }
}