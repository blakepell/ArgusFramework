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
    /// This is an abstract class derived from AnimationTimeline. It defines basic behavior for animating a GridLength property.
    /// </summary>
    public abstract class GridLengthAnimationBase : AnimationTimeline
    {
        public override Type TargetPropertyType => typeof(GridLength);

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            GridLength origin;
            GridLength destination;

            if (defaultOriginValue is GridLength value)
            {
                origin = value;
            }
            else
            {
                throw new ArgumentException("Wrong argument type in GetCurrentValue", nameof(defaultOriginValue));
            }

            if (defaultDestinationValue is GridLength length)
            {
                destination = length;
            }
            else
            {
                throw new ArgumentException("Wrong argument type in GetCurrentValue", nameof(defaultDestinationValue));
            }

            return this.GetCurrentValueCore(origin, destination, animationClock);
        }

        protected abstract GridLength GetCurrentValueCore(GridLength defaultOriginValue, GridLength defaultDestinationValue, AnimationClock animationClock);
    }
}