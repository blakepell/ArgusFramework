namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Creates a new <see cref="Guid"/>.
    /// </summary>
    internal sealed class GuidTagDefinition : InlineTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuidTagDefinition"/>.
        /// </summary>
        public GuidTagDefinition()
            : base("guid")
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
            writer.Write(Guid.NewGuid().ToString());
        }
    }
}