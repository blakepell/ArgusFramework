namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Trims spaces from the start of the string.
    /// </summary>
    public class TrimStartTagDefinition : ContentTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrimStartTagDefinition"/> class.
        /// </summary>
        public TrimStartTagDefinition()
            : base("trim-start")
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

        /// <inheritdoc cref="GetChildContext"/>
        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return new[] { new TagParameter("collection") };
        }

        /// <inheritdoc cref="GetParameters"/>
        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new[] { new TagParameter("collection") };
        }

        /// <inheritdoc cref="ConsolidateWriter"/>
        public override string ConsolidateWriter(TextWriter writer, Dictionary<string, object> arguments)
        {
            string content = writer?.ToString() ?? "";
            return content.TrimStart();
        }
    }
}
