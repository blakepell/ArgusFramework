using Argus.Extensions;

namespace Argus.Data.Mustache.TagDefinitions
{

    /// <summary>
    /// Tag to return a specified number of characters from the middle of a string.
    /// </summary>
    public class MidTagDefinition : ContentTagDefinition
    {
        public MidTagDefinition()
            : base("mid")
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
            return new[] { new TagParameter("startPosition"), new TagParameter("length") };
        }

        /// <inheritdoc cref="ConsolidateWriter"/>
        public override string ConsolidateWriter(TextWriter writer, Dictionary<string, object> arguments)
        {
            if (arguments.TryGetValue("startPosition", out object arg1)
                && int.TryParse(arg1.ToString(), out int startPosition)
                && arguments.TryGetValue("length", out object arg2) 
                && int.TryParse(arg2.ToString(), out int length))
            {
                string content = writer?.ToString();
                return content.Mid(startPosition, length);
            }

            return "";
        }
    }
}