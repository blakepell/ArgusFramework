using Argus.Extensions;

namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Removes any non-ASCII characters.
    /// </summary>
    public class RemoveNonAsciiCharsTagDefinition : ContentTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Argus.Data.Mustache.TagDefinitions.RemoveNonAsciiCharsTagDefinition"/>.
        /// </summary>
        public RemoveNonAsciiCharsTagDefinition()
            : base("remove-non-ascii-chars")
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
            return Array.Empty<TagParameter>();
        }

        /// <inheritdoc cref="ConsolidateWriter"/>
        public override string ConsolidateWriter(TextWriter writer, Dictionary<string, object> arguments)
        {
            return writer?.ToString().RemoveNonAsciiCharacters();
        }
    }
}