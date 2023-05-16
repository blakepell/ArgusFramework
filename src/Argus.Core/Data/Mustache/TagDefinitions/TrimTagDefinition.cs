namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Trims spaces from both ends of the string.
    /// </summary>
    public class TrimTagDefinition : ContentTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrimTagDefinition"/> class.
        /// </summary>
        public TrimTagDefinition()
            : base("trim")
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
            return content.Trim();
        }
    }
}