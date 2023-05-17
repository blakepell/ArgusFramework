﻿namespace Argus.Data.Mustache
{
    /// <summary>
    /// A newline / line break.
    /// </summary>
    internal sealed class LineBreakTagDefinition : InlineTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of an NewlineTagDefinition.
        /// </summary>
        public LineBreakTagDefinition()
            : base("br")
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
            writer.Write(Environment.NewLine);
        }
    }
}