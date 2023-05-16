using System.ComponentModel;

namespace Argus.Data.Mustache
{
    /// <summary>
    /// An UpcastDictionary
    /// </summary>
    public static class UpcastDictionary
    {
        public static IDictionary<string, object> Create(object source)
        {
            if (source == null)
            {
                return null;
            }
            if (source is IDictionary<string, object> sourceDictionary)
            {
                return sourceDictionary;
            }
            var sourceType = source.GetType();
            var types = GetTypes(sourceType);
            return GetDictionary(types, source);
        }

        private static IEnumerable<Type> GetTypes(Type sourceType)
        {
            var pending = new Queue<Type>();
            var visited = new HashSet<Type>();
            pending.Enqueue(sourceType);

            while (pending.Count != 0)
            {
                var type = pending.Dequeue();
                visited.Add(type);
                yield return type;

                if (type.BaseType != null)
                {
                    if (!visited.Contains(type.BaseType))
                    {
                        pending.Enqueue(type.BaseType);
                    }
                }

                foreach (var interfaceType in type.GetInterfaces())
                {
                    if (!visited.Contains(interfaceType))
                    {
                        pending.Enqueue(interfaceType);
                    }
                }
            }
        }

        private static IDictionary<string, object> GetDictionary(IEnumerable<Type> types, object source)
        {
            var dictionaries = from type in types
                               let valueType = GetValueType(type)
                               where valueType != null
                               let upcastType = typeof(UpcastDictionary<>).MakeGenericType(valueType)
                               select (IDictionary<string, object>)Activator.CreateInstance(upcastType, source);
            return dictionaries.FirstOrDefault();
        }

        private static Type GetValueType(Type type)
        {
            if (!type.IsGenericType)
            {
                return null;
            }
            var argumentTypes = type.GetGenericArguments();
            if (argumentTypes.Length != 2)
            {
                return null;
            }
            var keyType = argumentTypes[0];
            if (keyType != typeof(string))
            {
                return null;
            }
            var valueType = argumentTypes[1];
            var genericType = typeof(IDictionary<,>).MakeGenericType(typeof(string), valueType);
            if (!genericType.IsAssignableFrom(type))
            {
                return null;
            }
            return valueType;
        }
    }

    public class UpcastDictionary<TValue> : IDictionary<string, object>
    {
        private readonly IDictionary<string, TValue> _dictionary;

        public UpcastDictionary(IDictionary<string, TValue> dictionary)
        {
            this._dictionary = dictionary;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        void IDictionary<string, object>.Add(string key, object value)
        {
            throw new NotSupportedException();
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys => _dictionary.Keys;

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool IDictionary<string, object>.Remove(string key)
        {
            throw new NotSupportedException();
        }

        public bool TryGetValue(string key, out object value)
        {
            if (_dictionary.TryGetValue(key, out var result))
            {
                value = result;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public ICollection<object> Values => _dictionary.Values.Cast<object>().ToArray();

        public object this[string key]
        {
            get => _dictionary[key];
            [EditorBrowsable(EditorBrowsableState.Never)]
            set => throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            if (!(item.Value is TValue))
            {
                return false;
            }
            var pair = new KeyValuePair<string,TValue>(item.Key, (TValue)item.Value);
            return _dictionary.Contains(pair);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            var pairs = _dictionary.Select(p => new KeyValuePair<string, object>(p.Key, p.Value)).ToArray();
            pairs.CopyTo(array, arrayIndex);
        }

        public int Count => _dictionary.Count;

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => true;

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.Select(p => new KeyValuePair<string, object>(p.Key, p.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
