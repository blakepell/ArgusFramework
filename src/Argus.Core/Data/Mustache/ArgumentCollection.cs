﻿namespace Argus.Data.Mustache
{
    /// <summary>
    /// Associates parameters to their argument values.
    /// </summary>
    internal sealed class ArgumentCollection
    {
        private readonly Dictionary<TagParameter, IArgument> _argumentLookup = new();

        /// <summary>
        /// Associates the given parameter to the key placeholder.
        /// </summary>
        /// <param name="parameter">The parameter to associate the key with.</param>
        /// <param name="argument">The argument.</param>
        /// <remarks>If the key is null, the default value of the parameter will be used.</remarks>
        public void AddArgument(TagParameter parameter, IArgument argument)
        {
            _argumentLookup.Add(parameter, argument);
        }

        /// <summary>
        /// Gets the key that will be used to find the substitute value.
        /// </summary>
        public string GetKey(TagParameter parameter)
        {
            if (_argumentLookup.TryGetValue(parameter, out var argument) && argument != null)
            {
                return argument.GetKey();
            }
            return null;
        }

        /// <summary>
        /// Substitutes the key placeholders with their respective values.
        /// </summary>
        /// <param name="keyScope">The key/value pairs in the current lexical scope.</param>
        /// <param name="contextScope">The key/value pairs in current context.</param>
        /// <returns>A dictionary associating the parameter name to the associated value.</returns>
        public Dictionary<string, object> GetArguments(Scope keyScope, Scope contextScope)
        {
            var arguments = new Dictionary<string,object>();
            foreach (var pair in _argumentLookup)
            {
                object value;
                value = pair.Value == null ? pair.Key.DefaultValue : pair.Value.GetValue(keyScope, contextScope);
                arguments.Add(pair.Key.Name, value);
            }
            return arguments;
        }

        public Dictionary<string, object> GetArgumentKeyNames()
        {
            return _argumentLookup.ToDictionary(p => p.Key.Name, p => (object)this.GetKey(p.Key));
        }
    }
}
