namespace Argus.Data.Mustache
{
    /// <summary>
    /// Defines a tag that outputs a tab.
    /// </summary>
    internal sealed class TabTagDefinition : InlineTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of an <see cref="TabTagDefinition"/>.
        /// </summary>
        public TabTagDefinition()
            : base("tab")
        {
        }

        /// <summary>
        /// Gets the text to output.
        /// </summary>
        /// <param name="writer">The writer to write the output to.</param>
        /// <param name="arguments">The arguments passed to the tag.</param>
        /// <param name="context">Extra data passed along with the context.</param>
        public override void GetText(TextWriter writer, Dictionary<string, object> arguments, Scope context)
        {
            writer.Write('\t');
        }
    }
}