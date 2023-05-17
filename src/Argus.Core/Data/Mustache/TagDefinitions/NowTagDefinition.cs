namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// DateTime.Now
    /// </summary>
    internal sealed class NowTagDefinition : InlineTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="NowTagDefinition"/>.
        /// </summary>
        public NowTagDefinition()
            : base("now")
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
            writer.Write(DateTime.Now);
        }
    }
}