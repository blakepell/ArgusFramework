/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System;
using System.ComponentModel;
using System.Reflection;
using Xunit;

namespace Argus.UnitTests
{
    public class ReflectionTests
    {
        private class SampleClass
        {
            [Browsable(true)]
            public string BrowsableProp { get; set; } = "";

            [Browsable(false)]
            public string NonBrowsableProp { get; set; } = "";

            [Browsable(true)]
            public string ReadOnlyBrowsable { get; } = "";

            public string NoBrowsableAttr { get; set; } = "";
        }

        [Fact]
        public void IsBrowsable_BrowsableProperty_ReturnsTrue()
        {
            Assert.True(typeof(SampleClass).IsBrowsable("BrowsableProp"));
        }

        [Fact]
        public void IsBrowsable_NonBrowsableProperty_ReturnsFalse()
        {
            Assert.False(typeof(SampleClass).IsBrowsable("NonBrowsableProp"));
        }

        [Fact]
        public void IsBrowsableAndWritable_BrowsableWritable_ReturnsTrue()
        {
            Assert.True(typeof(SampleClass).IsBrowsableAndWritable("BrowsableProp"));
        }

        [Fact]
        public void IsBrowsableAndWritable_BrowsableReadOnly_ReturnsFalse()
        {
            Assert.False(typeof(SampleClass).IsBrowsableAndWritable("ReadOnlyBrowsable"));
        }

        [Fact]
        public void GetBrowsableProperties_ReturnsWritableProperties()
        {
            var props = typeof(SampleClass).GetBrowsableProperties();

            // Implementation returns writable properties
            Assert.Contains(props, p => p.Name == "BrowsableProp");
            Assert.Contains(props, p => p.Name == "NonBrowsableProp");
            Assert.Contains(props, p => p.Name == "NoBrowsableAttr");
            Assert.DoesNotContain(props, p => p.Name == "ReadOnlyBrowsable");
        }

        [Fact]
        public void GetBrowsableWritableProperties_ExcludesReadOnly()
        {
            var props = typeof(SampleClass).GetBrowsableWritableProperties();

            Assert.Contains(props, p => p.Name == "BrowsableProp");
            Assert.DoesNotContain(props, p => p.Name == "ReadOnlyBrowsable");
        }

        [Fact]
        public void IsReadable_ReadableProperty_ReturnsTrue()
        {
            var prop = typeof(SampleClass).GetProperty("BrowsableProp");

            Assert.True(prop!.IsReadable());
        }

        [Fact]
        public void IsWritable_WritableProperty_ReturnsTrue()
        {
            var prop = typeof(SampleClass).GetProperty("BrowsableProp");

            Assert.True(prop!.IsWritable());
        }

        [Fact]
        public void IsWritable_ReadOnlyProperty_ReturnsFalse()
        {
            var prop = typeof(SampleClass).GetProperty("ReadOnlyBrowsable");

            Assert.False(prop!.IsWritable());
        }

        [Fact]
        public void GetGetter_ReturnsMethodInfo()
        {
            var prop = typeof(SampleClass).GetProperty("BrowsableProp");

            var getter = prop!.GetGetter();

            Assert.NotNull(getter);
        }

        [Fact]
        public void GetSetter_ReturnsMethodInfo()
        {
            var prop = typeof(SampleClass).GetProperty("BrowsableProp");

            var setter = prop!.GetSetter();

            Assert.NotNull(setter);
        }

        [Fact]
        public void GetSetter_ReadOnlyProperty_ReturnsNull()
        {
            var prop = typeof(SampleClass).GetProperty("ReadOnlyBrowsable");

            var setter = prop!.GetSetter();

            Assert.Null(setter);
        }
    }
}
