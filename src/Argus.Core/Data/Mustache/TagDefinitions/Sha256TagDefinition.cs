using Argus.Cryptography;

namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// SHA256 Hash
    /// </summary>
    public class Sha256TagDefinition : ContentTagDefinition
    {
        public Sha256TagDefinition()
            : base("sha256")
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
            return HashUtilities.Sha256Hash(writer?.ToString());
        }
    }
}