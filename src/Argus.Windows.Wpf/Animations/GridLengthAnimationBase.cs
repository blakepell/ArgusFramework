﻿/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Argus.Windows.Wpf.Animations
{
    public abstract class GridLengthAnimationBase : AnimationTimeline
    {
        public override Type TargetPropertyType => typeof(GridLength);

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            GridLength origin;
            GridLength destination;

            if (defaultOriginValue is GridLength)
            {
                origin = (GridLength) defaultOriginValue;
            }
            else
            {
                throw new ArgumentException("Wrong argument type in GetCurrentValue", "OriginValue");
            }

            if (defaultDestinationValue is GridLength)
            {
                destination = (GridLength) defaultDestinationValue;
            }
            else
            {
                throw new ArgumentException("Wrong argument type in GetCurrentValue", "DestinationValue");
            }

            return this.GetCurrentValueCore(origin, destination, animationClock);
        }

        protected abstract GridLength GetCurrentValueCore(GridLength defaultOriginValue, GridLength defaultDestinationValue, AnimationClock animationClock);
    }
}