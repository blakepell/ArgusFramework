using System.Globalization;

// ReSharper disable MemberCanBePrivate.Global

namespace Argus.Data.Mustache
{
    /// <summary>
    /// Represents a scope of keys.
    /// </summary>
    public sealed class Scope
    {
        private readonly object _source;
        private readonly Scope _parent;

        /// <summary>
        /// Initializes a new instance of a KeyScope.
        /// </summary>
        /// <param name="source">The object to search for keys in.</param>
        internal Scope(object source)
            : this(source, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of a KeyScope.
        /// </summary>
        /// <param name="source">The object to search for keys in.</param>
        /// <param name="parent">The parent scope to search in if the value is not found.</param>
        internal Scope(object source, Scope parent)
        {
            _parent = parent;
            _source = source;
        }

        /// <summary>
        /// Occurs when a key/property is found in the object graph.
        /// </summary>
        public event EventHandler<KeyFoundEventArgs> KeyFound;

        /// <summary>
        /// Occurs when a key/property is not found in the object graph.
        /// </summary>
        public event EventHandler<KeyNotFoundEventArgs> KeyNotFound;

        /// <summary>
        /// Occurs when a setter is encountered and requires a value to be provided.
        /// </summary>
        public event EventHandler<ValueRequestEventArgs> ValueRequested;

        /// <summary>
        /// Creates a child scope that searches for keys in a default dictionary of key/value pairs.
        /// </summary>
        /// <returns>The new child scope.</returns>
        public Scope CreateChildScope()
        {
            return this.CreateChildScope(new Dictionary<string, object>());
        }

        /// <summary>
        /// Creates a child scope that searches for keys in the given object.
        /// </summary>
        /// <param name="source">The object to search for keys in.</param>
        /// <returns>The new child scope.</returns>
        public Scope CreateChildScope(object source)
        {
            var scope = new Scope(source, this);
            scope.KeyFound = this.KeyFound;
            scope.KeyNotFound = this.KeyNotFound;
            scope.ValueRequested = this.ValueRequested;
            return scope;
        }

        /// <summary>
        /// Attempts to find the value associated with the key with given name.
        /// </summary>
        /// <param name="name">The name of the key.</param>
        /// <param name="isExtension">Specifies whether the key appeared within triple curly braces.</param>
        /// <returns>The value associated with the key with the given name.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">A key with the given name could not be found.</exception>
        internal object Find(string name, bool isExtension)
        {
            var results = this.TryFind(name);
            if (results.Found)
            {
                return this.OnKeyFound(name, results.Value, isExtension);
            }

            if (this.OnKeyNotFound(name, results.Member, isExtension, out object value))
            {
                return value;
            }

            string message = string.Format(CultureInfo.CurrentCulture, "The key {0} could not be found.", results.Member);
            throw new KeyNotFoundException(message);
        }

        private object OnKeyFound(string name, object value, bool isExtension)
        {
            if (this.KeyFound == null)
            {
                return value;
            }

            var args = new KeyFoundEventArgs(name, value, isExtension);
            this.KeyFound(this, args);
            return args.Substitute;
        }

        private bool OnKeyNotFound(string name, string member, bool isExtension, out object value)
        {
            if (this.KeyNotFound == null)
            {
                value = null;
                return false;
            }

            var args = new KeyNotFoundEventArgs(name, member, isExtension);
            this.KeyNotFound(this, args);
            if (!args.Handled)
            {
                value = null;
                return false;
            }

            value = args.Substitute;
            return true;
        }

        private static IDictionary<string, object> ToLookup(object value)
        {
            var lookup = UpcastDictionary.Create(value) ?? new PropertyDictionary(value);
            return lookup;
        }

        internal void Set(string key)
        {
            var results = this.TryFind(key);
            if (this.ValueRequested == null)
            {
                Set(results, results.Value);
                return;
            }

            var e = new ValueRequestEventArgs();
            if (results.Found)
            {
                e.Value = results.Value;
            }

            this.ValueRequested(this, e);
            Set(results, e.Value);
        }

        internal void Set(string key, object value)
        {
            var results = this.TryFind(key);
            Set(results, value);
        }

        private static void Set(SearchResults results, object value)
        {
            // handle setting value in child scope
            while (results.MemberIndex < results.Members.Length - 1)
            {
                var context = new Dictionary<string, object>();
                results.Value = context;
                results.Lookup[results.Member] = results.Value;
                results.Lookup = context;
                ++results.MemberIndex;
            }

            results.Lookup[results.Member] = value;
        }

        public bool TryFind(string name, out object value)
        {
            var result = this.TryFind(name);
            value = result.Value;
            return result.Found;
        }

        private static char[] _delimiter = { '.' };
        
        private SearchResults TryFind(string name)
        {
            var results = new SearchResults
            {
                Members = name.Split(_delimiter),
                MemberIndex = 0
            };

            if (results.Member == "this")
            {
                results.Found = true;
                results.Lookup = ToLookup(_source);
                results.Value = _source;
            }
            else
            {
                this.TryFindFirst(results);
            }

            for (int index = 1; results.Found && index < results.Members.Length; ++index)
            {
                results.Lookup = ToLookup(results.Value);
                results.MemberIndex = index;
                results.Found = results.Lookup.TryGetValue(results.Member, out object value);
                results.Value = value;
            }

            return results;
        }

        private void TryFindFirst(SearchResults results)
        {
            results.Lookup = ToLookup(_source);
            if (results.Lookup.TryGetValue(results.Member, out object value))
            {
                results.Found = true;
                results.Value = value;
                return;
            }

            if (_parent == null)
            {
                results.Found = false;
                results.Value = null;
                return;
            }

            _parent.TryFindFirst(results);
        }
    }

    internal class SearchResults
    {
        public IDictionary<string, object> Lookup { get; set; }

        public string[] Members { get; set; }

        public int MemberIndex { get; set; }

        public string Member => this.Members[this.MemberIndex];

        public bool Found { get; set; }

        public object Value { get; set; }
    }
}