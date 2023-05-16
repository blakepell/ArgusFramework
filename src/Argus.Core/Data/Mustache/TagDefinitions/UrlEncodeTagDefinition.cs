using Argus.Extensions;
using System.Web;

namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// URL encodes a string.
    /// </summary>
    public class UrlEncodeTagDefinition : ContentTagDefinition
    {
        public UrlEncodeTagDefinition()
            : base("url-encode")
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
            return writer?.ToString()?.UrlDecode() ?? "";
        }
    }
}