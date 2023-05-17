namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// DateTime.Now
    /// </summary>
    internal sealed class NowDateOnlyTagDefinition : InlineTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="NowDateOnlyTagDefinition"/>.
        /// </summary>
        public NowDateOnlyTagDefinition()
            : base("now-date-only")
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
            writer.Write(DateTime.Now.ToShortDateString());
        }
    }
}