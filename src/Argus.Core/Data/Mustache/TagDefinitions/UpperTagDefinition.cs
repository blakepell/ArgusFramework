namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Converts text to upper case.
    /// </summary>
    public class UpperTagDefinition : ContentTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of <see cref="UpperTagDefinition"/>.
        /// </summary>
        public UpperTagDefinition()
            : base("upper")
        {
        }

        /// <inheritdoc cref="GetChildContext"/>
        public override IEnumerable<NestedContext> GetChildContext(TextWriter writer, Scope keyScope, Dictionary<string, object> arguments, Scope contextScope)
        {
            var context = new NestedContext()
            {
                KeyScope = keyScope,
                Writer = new StringWriter(),
                WriterNeedsConsolidated = true,
            };

            yield return context;
        }

        /// <inheritdoc cref="GetChildContextParameters"/>
        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return this.GetParameters();
        }

        /// <inheritdoc cref="GetParameters"/>
        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new[] { new TagParameter("length") };
        }

        /// <inheritdoc cref="ConsolidateWriter"/>
        public override string ConsolidateWriter(TextWriter writer, Dictionary<string, object> arguments)
        {
            return writer?.ToString()?.ToUpperInvariant() ?? "";
        }
    }
}