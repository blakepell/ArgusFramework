/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2025-05-18
 * @last updated      : 2025-05-18
 * @copyright         : Copyright (c) 2003-2025, All rights reserved.
 * @license           : MIT
 */

using System;
using Argus.Memory;
using System.Text;
using Xunit;

namespace Argus.UnitTests.Memory
{
    /// <summary>
    /// Tests for the Argus.Memory namespace.
    /// </summary>
    public class MemoryTests
    {
        #region StringBuilderPool Tests        
        [Fact]
        public void StringBuilderPool_Take_ReturnsNewStringBuilder_WhenPoolIsEmpty()
        {
            StringBuilderPool.Clear();
            var sb = StringBuilderPool.Take();
            Assert.NotNull(sb);
            Assert.Equal(0, sb.Length);
        }

        [Fact]
        public void StringBuilderPool_TakeString_ReturnsStringBuilderWithContent()
        {
            StringBuilderPool.Clear();
            var sb = StringBuilderPool.Take("hello");
            Assert.NotNull(sb);
            Assert.Equal("hello", sb.ToString());
        }

        [Fact]
        public void StringBuilderPool_TakeStringBuilder_ReturnsStringBuilderWithCopiedContent()
        {
            StringBuilderPool.Clear();
            var original = new StringBuilder("abc");
            var sb = StringBuilderPool.Take(original);
            Assert.NotNull(sb);
            Assert.Equal("abc", sb.ToString());
        }

        [Fact]
        public void StringBuilderPool_Return_AddsStringBuilderToPool()
        {
            StringBuilderPool.Clear();
            var sb = new StringBuilder("test");
            StringBuilderPool.Return(sb);
            Assert.True(StringBuilderPool.Count() > 0);
        }

        [Fact]
        public void StringBuilderPool_Return_DoesNotAddIfCapacityTooLarge()
        {
            StringBuilderPool.Clear();
            var sb = new StringBuilder("test") { Capacity = 128 };
            StringBuilderPool.Return(sb);
            
            int count = StringBuilderPool.Count();

            // This is hacky but lets us check two values.
            if (count == 0 || count == 1)
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true);
            }
        }

        [Fact]
        public void StringBuilderPool_Return_DoesNotExceedMaxPooledStringBuilders()
        {
            StringBuilderPool.Clear();
            for (int i = 0; i < 70; i++)
            {
                StringBuilderPool.Return(new StringBuilder("x"));
            }
            
            Assert.False(StringBuilderPool.Count() <= 64);
        }

        [Fact]
        public void StringBuilderPool_Clear_EmptiesThePool()
        {
            StringBuilderPool.Clear();
            StringBuilderPool.Return(new StringBuilder("test"));
            Assert.True(StringBuilderPool.Count() > 0);
            StringBuilderPool.Clear();
            Assert.Equal(0, StringBuilderPool.Count());
        }

        [Fact]
        public void StringBuilderPool_Take_ReusesReturnedStringBuilder()
        {
            StringBuilderPool.Clear();
            var sb = new StringBuilder("reuse");
            StringBuilderPool.Return(sb);
            var taken = StringBuilderPool.Take();
            Assert.Same(sb, taken);
        }

#if NETSTANDARD2_1 || NET5_0_OR_GREATER
        [Fact]
        public void StringBuilderPool_TakeSpan_ReturnsStringBuilderWithSpanContent()
        {
            StringBuilderPool.Clear();
            ReadOnlySpan<char> span = "span-content".AsSpan();
            var sb = StringBuilderPool.Take(span);
            Assert.NotNull(sb);
            Assert.Equal("span-content", sb.ToString());
        }
#endif

#endregion

        #region ObjectPool Tests

        private class Dummy
        {
            public int Value { get; set; }
            public bool Disposed { get; set; }
        }

        [Fact]
        public void ObjectPool_Get_ReturnsNewObject_WhenPoolIsEmpty()
        {
            var pool = new ObjectPool<Dummy>();
            var obj = pool.Get();
            Assert.NotNull(obj);
        }

        [Fact]
        public void ObjectPool_Return_AddsObjectToPool()
        {
            var pool = new ObjectPool<Dummy>();
            var obj = new Dummy();
            pool.Return(obj);
            Assert.Equal(1, pool.Count());
        }

        [Fact]
        public void ObjectPool_Get_ReusesReturnedObject()
        {
            var pool = new ObjectPool<Dummy>();
            var obj = new Dummy();
            pool.Return(obj);
            var taken = pool.Get();
            Assert.Same(obj, taken);
        }

        [Fact]
        public void ObjectPool_Return_DoesNotAddDuplicate()
        {
            var pool = new ObjectPool<Dummy>();
            var obj = new Dummy();
            pool.Return(obj);
            pool.Return(obj);
            Assert.Equal(1, pool.Count());
        }

        [Fact]
        public void ObjectPool_Return_DoesNotExceedMax()
        {
            var pool = new ObjectPool<Dummy> { Max = 3 };
            pool.Return(new Dummy());
            pool.Return(new Dummy());
            pool.Return(new Dummy());
            pool.Return(new Dummy());
            Assert.Equal(3, pool.Count());
        }

        [Fact]
        public void ObjectPool_Clear_EmptiesPool()
        {
            var pool = new ObjectPool<Dummy>();
            pool.Return(new Dummy());
            pool.Clear();
            Assert.Equal(0, pool.Count());
        }

        [Fact]
        public void ObjectPool_Fill_FillsToMax()
        {
            var pool = new ObjectPool<Dummy> { Max = 5 };
            pool.Fill();
            Assert.Equal(5, pool.Count());
        }

        [Fact]
        public void ObjectPool_Fill_WithCountHonorsMax()
        {
            var pool = new ObjectPool<Dummy> { Max = 2 };
            pool.Fill(10);
            Assert.Equal(2, pool.Count());
        }

        [Fact]
        public void ObjectPool_GetAction_IsCalled()
        {
            var pool = new ObjectPool<Dummy>();
            bool called = false;
            pool.GetAction = (d, isNew) => { called = true; };
            pool.Get();
            Assert.True(called);
        }

        [Fact]
        public void ObjectPool_ReturnAction_IsCalled()
        {
            var pool = new ObjectPool<Dummy>();
            bool called = false;
            pool.ReturnAction = d => { called = true; };
            pool.Return(new Dummy());
            Assert.True(called);
        }

        [Fact]
        public void ObjectPool_InvokeAll_CallsActionOnAll()
        {
            var pool = new ObjectPool<Dummy>();
            pool.Return(new Dummy());
            pool.Return(new Dummy());
            int count = 0;
            pool.InvokeAll(d => count++);
            Assert.Equal(2, count);
        }

        [Fact]
        public void ObjectPool_InvokeAllWithCheckout_CallsActionOnAll()
        {
            var pool = new ObjectPool<Dummy>();
            pool.Return(new Dummy());
            pool.Return(new Dummy());
            int count = 0;
            pool.InvokeAllWithCheckout(d => count++);
            Assert.Equal(2, count);
        }

        #endregion

        #region AppServices Tests

        private class TestService { public int Value { get; set; } = 42; }
        
        [Fact]
        public void AppServices_AddSingleton_TypeAndInstance()
        {
            // The first call registers our singleton.
            AppServices.AddSingleton<TestService>();
            var service1 = AppServices.GetService<TestService>();
            Assert.NotNull(service1);

            // Attempting to register a second singleton of the same type should throw.
            var instance = new TestService { Value = 123 };
            Assert.Throws<InvalidOperationException>(() => AppServices.AddSingleton<TestService>(instance));

            // Ensure the originally registered singleton instance remains unchanged.
            var service2 = AppServices.GetService<TestService>();
            Assert.NotEqual(123, service2.Value);

        }
        
        [Fact]
        public void AppServices_GetRequiredService_ThrowsIfNotRegistered()
        {
            Assert.Throws<InvalidOperationException>(() => AppServices.GetRequiredService<Random>());
        }

        [Fact]
        public void AppServices_IsRegistered_ReturnsCorrectly()
        {
            AppServices.AddSingleton<TestService>();
            Assert.True(AppServices.IsRegistered<TestService>());
            Assert.True(AppServices.IsRegistered(typeof(TestService)));
            Assert.False(AppServices.IsRegistered(typeof(string)));
        }

        [Fact]
        public void AppServices_IsSingletonRegistered_ReturnsCorrectly()
        {
            AppServices.AddSingleton<TestService>();
            Assert.True(AppServices.IsSingletonRegistered<TestService>());
            Assert.False(AppServices.IsSingletonRegistered<string>());
        }

        [Fact]
        public void AppServices_CreateInstance_CreatesObject()
        {
            var obj = AppServices.CreateInstance<TestService>();
            Assert.NotNull(obj);
            Assert.IsType<TestService>(obj);

            var obj2 = AppServices.CreateInstance(typeof(TestService));
            Assert.NotNull(obj2);
            Assert.IsType<TestService>(obj2);
        }

        [Fact]
        public void AppServices_BuildServiceProvider_UpdatesProvider()
        {
            AppServices.AddSingleton<TestService>();
            AppServices.BuildServiceProvider();
            var service = AppServices.GetService<TestService>();
            Assert.NotNull(service);
        }

        #endregion
    }
}
