using Argus.Extensions;

namespace Argus.Data.Mustache.TagDefinitions
{

    /// <summary>
    /// Tag to return the right specified number of characters from a string.
    /// </summary>
    public class RightTagDefinition : ContentTagDefinition
    {
        public RightTagDefinition()
            : base("right")
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
            if (arguments.TryGetValue("length", out object obj) && int.TryParse(obj.ToString(), out int length))
            {
                string content = writer?.ToString();
                return content.SafeRight(length);
            }

            return "";
        }
    }
}