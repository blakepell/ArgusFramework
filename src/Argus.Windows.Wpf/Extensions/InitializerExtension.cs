/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

namespace Argus.Extensions
{
    [MarkupExtensionReturnType(typeof(object))]
    [ContentProperty("Path")]
    public class InitializerExtension : MarkupExtension
    {
        public PropertyPath Path { get; set; }
        public IValueConverter Converter { get; set; }
        public object Default { get; set; }
        public string ElementName { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                return this.DefaultOrValue(this);
            }

            Type targetType = null;

            try
            {
                targetType = this.GetTargetType(serviceProvider);
            }
            catch (NullReferenceException)
            {
                return this.DefaultOrValue(this);
            }

            object source = null;

            try
            {
                source = this.GetSourceObject(serviceProvider);
            }
            catch (NullReferenceException)
            {
                return this.DefaultOrValue(this.GetDefault(targetType));
            }

            source = this.GetSourcePropertyValue(source);

            if (this.Converter != null)
            {
                return this.Converter.Convert(source, targetType, null, CultureInfo.CurrentUICulture);
            }

            return source;
        }

        private object GetSourceObject(IServiceProvider serviceProvider)
        {
            var rootObjectProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            var rootObject = rootObjectProvider?.RootObject as FrameworkElement;

            if (rootObject == null)
            {
                throw new NullReferenceException();
            }

            if (this.ElementName != null)
            {
                return rootObject.FindName(this.ElementName);
            }

            return rootObject;
        }

        private object GetSourcePropertyValue(object sourceObject)
        {
            IEnumerable<string> properties = this.Path.Path.Split('.');

            if (this.Path != null)
            {
                foreach (string property in properties)
                {
                    sourceObject = this.GetPropertyValue(sourceObject, property);
                }
            }

            return sourceObject;
        }

        private Type GetTargetType(IServiceProvider serviceProvider)
        {
            var provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (provideValueTarget == null)
            {
                throw new NullReferenceException();
            }

            var targetObject = provideValueTarget.TargetObject as DependencyObject;

            if (targetObject == null)
            {
                throw new NullReferenceException();
            }

            var targetProperty = provideValueTarget.TargetProperty;
            var targetType = targetProperty.GetType();

            if (targetProperty is DependencyProperty)
            {
                targetType = ((DependencyProperty) targetProperty).PropertyType;
            }

            return targetType;
        }

        public object DefaultOrValue(object value)
        {
            if (this.Default != null)
            {
                return this.Default;
            }

            return value;
        }

        public object GetPropertyValue(object source, string propertyName)
        {
            return source.GetType().GetProperty(propertyName).GetValue(source, null);
        }

        private object GetDefault(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }

            return null;
        }
    }
}