using System.Security;

namespace Argus.Data.Mustache
{
    /// <summary>
    /// Format compiler for use with HTML templates.
    /// </summary>
    public sealed class HtmlFormatCompiler
    {
        private readonly FormatCompiler _compiler = new();

        public HtmlFormatCompiler()
        {
            _compiler.AreExtensionTagsAllowed = true;
            _compiler.RemoveNewLines = true;
        }

        /// <summary>
        /// Occurs when a placeholder is found in the template.
        /// </summary>
        public event EventHandler<PlaceholderFoundEventArgs> PlaceholderFound
        {
            add => _compiler.PlaceholderFound += value;
            remove => _compiler.PlaceholderFound -= value;
        }

        /// <summary>
        /// Occurs when a variable is found in the template.
        /// </summary>
        public event EventHandler<VariableFoundEventArgs> VariableFound
        {
            add => _compiler.VariableFound += value;
            remove => _compiler.VariableFound -= value;
        }

        /// <summary>
        /// Registers the given tag definition with the parser.
        /// </summary>
        /// <param name="definition">The tag definition to register.</param>
        /// <param name="isTopLevel">Specifies whether the tag is immediately in scope.</param>
        public void RegisterTag(TagDefinition definition, bool isTopLevel)
        {
            _compiler.RegisterTag(definition, isTopLevel);
        }

        /// <summary>
        /// Builds a text generator based on the given format.
        /// </summary>
        /// <param name="format">The format to parse.</param>
        /// <returns>The text generator.</returns>
        public Generator Compile(string format)
        {
            var generator = _compiler.Compile(format);
            generator.TagFormatted += EscapeInvalidHtml;
            return generator;
        }

        private static void EscapeInvalidHtml(object sender, TagFormattedEventArgs e)
        {
            if (e.IsExtension)
            {
                // Do not escape text within triple curly braces
                return;
            }
            e.Substitute = SecurityElement.Escape(e.Substitute);
        }
    }
}
