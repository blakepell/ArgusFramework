namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Trims spaces from the end of the string.
    /// </summary>
    public class TrimEndTagDefinition : ContentTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrimEndTagDefinition"/> class.
        /// </summary>
        public TrimEndTagDefinition()
            : base("trim-end")
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
            return content.TrimEnd();
        }
    }
}