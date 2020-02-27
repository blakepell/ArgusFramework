using System.Collections.Concurrent;

namespace Argus.Memory
{

    namespace Iuf.Test.Console
    {
        /// <summary>
        ///     Represents a pool of objects that can be reused (Note that data in those objects are not cleared between uses).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ObjectPool<T> where T : new()
        {
            //*********************************************************************************************************************
            //
            //             Class:  ObjectPool
            //      Organization:  http://www.blakepell.com
            //      Initial Date:  02/27/2020
            //      Last Updated:  02/27/2020
            //
            //*********************************************************************************************************************

            /// <summary>
            ///     Holds the objects in the pool.
            /// </summary>
            private readonly ConcurrentBag<T> _items = new ConcurrentBag<T>();

            /// <summary>
            ///     The current internal counter of how many objects are in the ConcurrentBag.
            /// </summary>
            private int _counter = 0;

            /// <summary>
            ///     The maximum number of objects we will hold in the Pool.  Anything over this number is created and
            ///     returned on Get but not pooled.
            /// </summary>
            public int Max { get; set; } = 10;

            /// <summary>
            ///     Releases an object back into the pool.
            /// </summary>
            /// <param name="item">The item to release back into the pool.</param>
            public void Release(T item)
            {
                if (_counter < this.Max)
                {
                    _items.Add(item);
                    _counter++;
                }
            }

            /// <summary>
            ///     Gets an object from the pool if one is available.  If an object is not available a new object
            ///     is created and returned.
            /// </summary>
            public T Get()
            {
                if (_items.TryTake(out var item))
                {
                    _counter--;
                    return item;
                }

                var obj = new T();
                _items.Add(obj);
                _counter++;

                return obj;
            }

        }
    }
}