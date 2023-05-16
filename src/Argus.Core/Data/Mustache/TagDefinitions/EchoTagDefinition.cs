using Argus.Extensions;

namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Echo's the string provided parameter back to the output.
    /// </summary>
    public class EchoTagDefinition : InlineTagDefinition
    {
        public EchoTagDefinition()
            : base("echo")
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
            return new[] { new TagParameter("text") };
        }

        /// <inheritdoc cref="GetText"/>
        public override void GetText(TextWriter writer, Dictionary<string, object> arguments, Scope context)
        {
            if (arguments.TryGetValue("text", out object obj))
            {
                writer.Write(obj.ToString());
            }
        }
    }
}