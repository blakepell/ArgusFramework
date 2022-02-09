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

        public GridLength? From
        {
            get => (GridLength?) this.GetValue(FromProperty);
            set => this.SetValue(FromProperty, value);
        }

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
                if (this.From == null)
                {
                    this.To = new GridLength(defaultOriginValue.Value + (double) this.By);
                }
                else
                {
                    this.To = new GridLength(this.From.Value.Value + (double) this.By);
                }
            }

            if (from.GridUnitType != to.GridUnitType)
            {
                return to;
            }

            return new GridLength(from.Value + (to.Value - from.Value) * progress.Value, from.GridUnitType);
        }
    }
}