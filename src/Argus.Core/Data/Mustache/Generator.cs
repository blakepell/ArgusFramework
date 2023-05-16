using System.Globalization;

namespace Argus.Data.Mustache
{
    /// <summary>
    /// Generates text by substituting an object's values for placeholders.
    /// </summary>
    public sealed class Generator
    {
        private readonly IGenerator _generator;

        private readonly List<EventHandler<KeyFoundEventArgs>> _foundHandlers = new();

        private readonly List<EventHandler<KeyNotFoundEventArgs>> _notFoundHandlers = new();

        private readonly List<EventHandler<ValueRequestEventArgs>> _valueRequestedHandlers = new();

        /// <summary>
        /// Initializes a new instance of a Generator.
        /// </summary>
        /// <param name="generator">The text generator to wrap.</param>
        internal Generator(IGenerator generator)
        {
            _generator = generator;
        }

        /// <summary>
        /// Occurs when a key/property is found.
        /// </summary>
        public event EventHandler<KeyFoundEventArgs> KeyFound
        {
            add => _foundHandlers.Add(value);
            remove => _foundHandlers.Remove(value);
        }

        /// <summary>
        /// Occurs when a key/property is not found in the object graph.
        /// </summary>
        public event EventHandler<KeyNotFoundEventArgs> KeyNotFound
        {
            add => _notFoundHandlers.Add(value);
            remove => _notFoundHandlers.Remove(value);
        }

        /// <summary>
        /// Occurs when a setter is encountered and requires a value to be provided.
        /// </summary>
        public event EventHandler<ValueRequestEventArgs> ValueRequested
        {
            add => _valueRequestedHandlers.Add(value);
            remove => _valueRequestedHandlers.Remove(value);
        }

        /// <summary>
        /// Occurs when a tag is replaced by its text.
        /// </summary>
        public event EventHandler<TagFormattedEventArgs> TagFormatted;

        /// <summary>
        /// Gets the text that is generated for the given object.
        /// </summary>
        /// <param name="source">The object to generate the text with.</param>
        /// <returns>The text generated for the given object.</returns>
        public string Render(object source)
        {
            return this.RenderInternal(CultureInfo.CurrentCulture, source);
        }

        /// <summary>
        /// Gets the text that is generated for the given object.
        /// </summary>
        /// <param name="provider">The format provider to use.</param>
        /// <param name="source">The object to generate the text with.</param>
        /// <returns>The text generated for the given object.</returns>
        public string Render(IFormatProvider provider, object source)
        {
            if (provider == null)
            {
                provider = CultureInfo.CurrentCulture;
            }

            return this.RenderInternal(provider, source);
        }

        private string RenderInternal(IFormatProvider provider, object source)
        {
            var keyScope = new Scope(source);
            var contextScope = new Scope(new Dictionary<string, object>());

            foreach (var handler in _foundHandlers)
            {
                keyScope.KeyFound += handler;
                contextScope.KeyFound += handler;
            }

            foreach (var handler in _notFoundHandlers)
            {
                keyScope.KeyNotFound += handler;
                contextScope.KeyNotFound += handler;
            }

            foreach (var handler in _valueRequestedHandlers)
            {
                contextScope.ValueRequested += handler;
            }

            var writer = new StringWriter(provider);
            _generator.GetText(writer, keyScope, contextScope, this.PostProcess);
            return writer.ToString();
        }

        private void PostProcess(Substitution substitution)
        {
            if (this.TagFormatted == null)
            {
                return;
            }

            var args = new TagFormattedEventArgs(substitution.Key, substitution.Substitute, substitution.IsExtension);
            this.TagFormatted(this, args);
            substitution.Substitute = args.Substitute;
        }
    }
}