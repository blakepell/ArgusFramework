using Argus.Extensions;

namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Formats the value if it's a number to the specified number of decimal places.
    /// </summary>
    public class FormatIfNumberDefinition : ContentTagDefinition
    {
        public FormatIfNumberDefinition()
            : base("format-number")
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
            return new[] { new TagParameter("decimalPlaces") };
        }

        /// <inheritdoc cref="ConsolidateWriter"/>
        public override string ConsolidateWriter(TextWriter writer, Dictionary<string, object> arguments)
        {
            if (arguments.TryGetValue("decimalPlaces", out object obj) && int.TryParse(obj.ToString(), out int decimalPlaces))
            {
                string content = writer?.ToString()?.FormatIfNumber(decimalPlaces);
                return content;
            }

            return "";
        }
    }
}