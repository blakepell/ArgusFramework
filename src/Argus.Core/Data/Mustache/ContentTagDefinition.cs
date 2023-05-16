namespace Argus.Data.Mustache
{
    /// <summary>
    /// Defines a tag that can contain inner text.  A content tag will have an open
    /// tag and end tag generally with inner text being used as the text to process.
    /// </summary>
    public abstract class ContentTagDefinition : TagDefinition
    {
        /// <summary>
        /// Initializes a new instance of a ContentTagDefinition.
        /// </summary>
        /// <param name="tagName">The name of the tag being defined.</param>
        protected ContentTagDefinition(string tagName)
            : base(tagName)
        {
        }

        /// <summary>
        /// Initializes a new instance of a ContentTagDefinition.
        /// </summary>
        /// <param name="tagName">The name of the tag being defined.</param>
        /// <param name="isBuiltin">Specifies whether the tag is a built-in tag.</param>
        internal ContentTagDefinition(string tagName, bool isBuiltin)
            : base(tagName, isBuiltin)
        {
        }

        /// <summary>
        /// Gets or sets whether the tag can have content.
        /// </summary>
        /// <returns>True if the tag can have a body; otherwise, false.</returns>
        protected override bool GetHasContent()
        {
            return true;
        }
    }
}
