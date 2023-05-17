namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Repeats a character, number or string a specified number of times  {{#repeat '-' 20}}
    /// </summary>
    internal sealed class RepeatTagDefinition : InlineTagDefinition
    {
        private const string CountParameter = "count";
        private static readonly TagParameter Count = new(CountParameter) { IsRequired = true };
        private const string ValueParameter = "value";
        private static readonly TagParameter Value = new(ValueParameter) { IsRequired = true };

        public RepeatTagDefinition()
            : base("repeat", true)
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
            return new[] { Count, Value };
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
                arguments.TryGetValue("value", out object valueObj);

                if (valueObj == null)
                {
                    return;
                }
                
                for (int i = 0; i < length; i++)
                {
                    writer.Write(valueObj);
                }
            }
        }
    }
}