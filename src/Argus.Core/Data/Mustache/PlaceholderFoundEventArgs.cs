
// ReSharper disable UnusedParameter.Local

namespace Argus.Data.Mustache
{
    /// <summary>
    /// Holds the information describing a key that is found in a template.
    /// </summary>
    public class PlaceholderFoundEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of a PlaceholderFoundEventArgs.
        /// </summary>
        /// <param name="key">The key that was found.</param>
        /// <param name="alignment">The alignment that will be applied to the substitute value.</param>
        /// <param name="formatting">The formatting that will be applied to the substitute value.</param>
        /// <param name="isExtension">Indicates whether the placeholder was found within triple curly braces.</param>
        /// <param name="context">The context where the placeholder was found.</param>
        internal PlaceholderFoundEventArgs(string key, string alignment, string formatting, bool isExtension, Context[] context)
        {
            this.Key = key;
            this.Alignment = alignment;
            this.Formatting = formatting;
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets the key that was found.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the alignment that will be applied to the substitute value.
        /// </summary>
        public string Alignment { get; set; }

        /// <summary>
        /// Gets or sets the formatting that will be applied to the substitute value.
        /// </summary>
        public string Formatting { get; set; }

        /// <summary>
        /// Gets or sets whether the placeholder was found within triple curly braces.
        /// </summary>
        public bool IsExtension { get; set; }

        /// <summary>
        /// Gets the context where the placeholder was found.
        /// </summary>
        public Context[] Context { get; }
    }
}
