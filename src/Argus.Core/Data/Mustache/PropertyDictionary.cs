using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Argus.Data.Mustache
{
    /// <summary>
    /// Provides methods for creating instances of PropertyDictionary.
    /// </summary>
    [SuppressMessage("ReSharper", "InvertIf")]
    internal sealed class PropertyDictionary : IDictionary<string, object>
    {
        private static readonly Dictionary<Type, Dictionary<string, Func<object, object>>> Cache = new();

        private readonly Dictionary<string, Func<object, object>> _typeCache;

        /// <summary>
        /// Initializes a new instance of a PropertyDictionary.
        /// </summary>
        /// <param name="instance">The instance to wrap in the PropertyDictionary.</param>
        public PropertyDictionary(object instance)
        {
            this.Instance = instance;
            if (instance == null)
            {
                _typeCache = new Dictionary<string, Func<object, object>>();
            }
            else
            {
                lock (Cache)
                {
                    _typeCache = GetCacheType(instance);
                }
            }
        }

        private static Dictionary<string, Func<object, object>> GetCacheType(object instance)
        {
            var type = instance.GetType();

            if (!Cache.TryGetValue(type, out var typeCache))
            {
                typeCache = new Dictionary<string, Func<object, object>>();

                const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;

                var properties = GetMembers(type, type.GetProperties(flags).Where(p => !p.IsSpecialName));
                foreach (var propertyInfo in properties)
                {
                    typeCache.Add(propertyInfo.Name, i => propertyInfo.GetValue(i, null));
                }

                var fields = GetMembers(type, type.GetFields(flags).Where(f => !f.IsSpecialName));
                foreach (var fieldInfo in fields)
                {
                    typeCache.Add(fieldInfo.Name, i => fieldInfo.GetValue(i));
                }

                Cache.Add(type, typeCache);
            }

            return typeCache;
        }

        private static IEnumerable<TMember> GetMembers<TMember>(Type type, IEnumerable<TMember> members)
            where TMember : MemberInfo
        {
            var singles = from member in members
                          group member by member.Name
                          into nameGroup
                          where nameGroup.Count() == 1
                          from property in nameGroup
                          select property;
            var multiples = from member in members
                            group member by member.Name
                            into nameGroup
                            where nameGroup.Count() > 1
                            select
                            (
                                from member in nameGroup
                                orderby GetDistance(type, member)
                                select member
                            ).First();
            var combined = singles.Concat(multiples);
            return combined;
        }

        private static int GetDistance(Type type, MemberInfo memberInfo)
        {
            int distance = 0;
            for (; type != null && type != memberInfo.DeclaringType; type = type.BaseType)
            {
                ++distance;
            }

            return distance;
        }

        /// <summary>
        /// Gets the underlying instance.
        /// </summary>
        public object Instance { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        void IDictionary<string, object>.Add(string key, object value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Determines whether a property with the given name exists.
        /// </summary>
        /// <param name="key">The name of the property.</param>
        /// <returns>True if the property exists; otherwise, false.</returns>
        public bool ContainsKey(string key)
        {
            return _typeCache.ContainsKey(key);
        }

        /// <summary>
        /// Gets the name of the properties in the type.
        /// </summary>
        public ICollection<string> Keys => _typeCache.Keys;

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool IDictionary<string, object>.Remove(string key)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Tries to get the value for the given property name.
        /// </summary>
        /// <param name="key">The name of the property to get the value for.</param>
        /// <param name="value">The variable to store the value of the property or the default value if the property is not found.</param>
        /// <returns>True if a property with the given name is found; otherwise, false.</returns>
        /// <exception cref="System.ArgumentNullException">The name of the property was null.</exception>
        public bool TryGetValue(string key, out object value)
        {
            if (!_typeCache.TryGetValue(key, out var getter))
            {
                value = null;
                return false;
            }

            value = getter(this.Instance);
            return true;
        }

        /// <summary>
        /// Gets the values of all of the properties in the object.
        /// </summary>
        public ICollection<object> Values
        {
            get
            {
                ICollection<Func<object, object>> getters = _typeCache.Values;
                var values = new List<object>();

                foreach (var getter in getters)
                {
                    object value = getter(this.Instance);
                    values.Add(value);
                }

                return values.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets or sets the value of the property with the given name.
        /// </summary>
        /// <param name="key">The name of the property to get or set.</param>
        /// <returns>The value of the property with the given name.</returns>
        /// <exception cref="System.ArgumentNullException">The property name was null.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">The type does not have a property with the given name.</exception>
        /// <exception cref="System.ArgumentException">The property did not support getting or setting.</exception>
        /// <exception cref="System.ArgumentException">
        /// The object does not match the target type, or a property is a value type but the value is null.
        /// </exception>
        public object this[string key]
        {
            get
            {
                var getter = _typeCache[key];
                return getter(this.Instance);
            }
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
            if (!_typeCache.TryGetValue(item.Key, out var getter))
            {
                return false;
            }

            object value = getter(this.Instance);
            return Equals(item.Value, value);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            var pairs = new List<KeyValuePair<string, object>>();
            ICollection<KeyValuePair<string, Func<object, object>>> collection = _typeCache;
            foreach (var pair in collection)
            {
                var getter = pair.Value;
                object value = getter(this.Instance);
                pairs.Add(new KeyValuePair<string, object>(pair.Key, value));
            }

            pairs.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of properties in the type.
        /// </summary>
        public int Count => _typeCache.Count;

        /// <summary>
        /// Gets or sets whether updates will be ignored.
        /// </summary>
        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => true;

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the property name/value pairs in the object.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (var pair in _typeCache)
            {
                var getter = pair.Value;
                object value = getter(this.Instance);
                yield return new KeyValuePair<string, object>(pair.Key, value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}