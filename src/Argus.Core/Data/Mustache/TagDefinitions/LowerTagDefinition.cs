namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Converts text to lower case.
    /// </summary>
    public class LowerTagDefinition : ContentTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of <see cref="LowerTagDefinition"/>.
        /// </summary>
        public LowerTagDefinition()
            : base("lower")
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
            return new[] { new TagParameter("collection") };
        }

        /// <inheritdoc cref="ConsolidateWriter"/>
        public override string ConsolidateWriter(TextWriter writer, Dictionary<string, object> arguments)
        {
            return writer?.ToString()?.ToLowerInvariant() ?? "";
        }
    }
}