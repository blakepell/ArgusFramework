using Argus.Extensions;

namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Adds the specified number of spaces.  {{#space 20}}
    /// </summary>
    internal sealed class SpaceTagDefinition : InlineTagDefinition
    {
        private const string CountParameter = "count";
        private static readonly TagParameter Count = new(CountParameter) { IsRequired = true };

        public SpaceTagDefinition()
            : base("space", true)
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
            return new[] { Count };
        }

        /// <summary>
        /// Gets the text to output.
        /// </summary>
        /// <param name="writer">The writer to write the output to.</param>
        /// <param name="arguments">The arguments passed to the tag.</param>
        /// <param name="context">Extra data passed along with the context.</param>
        public override void GetText(TextWriter writer, Dictionary<string, object> arguments, Scope context)
        {
            if (arguments.TryGetValue("count", out object obj) && int.TryParse(obj.ToString(), out int length))
            {
                for (int i = 0; i < length; i++)
                {
                    writer.Write(' ');
                }
            }
        }
    }
}