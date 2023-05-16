namespace Argus.Data.Mustache
{
    /// <summary>
    /// Substitutes a key placeholder with the textual representation of the associated object.
    /// </summary>
    internal sealed class KeyGenerator : IGenerator
    {
        private readonly string _key;
        private readonly string _format;
        private readonly bool _isVariable;
        private readonly bool _isExtension;

        /// <summary>
        /// Initializes a new instance of a KeyGenerator.
        /// </summary>
        /// <param name="key">The key to substitute with its value.</param>
        /// <param name="alignment">The alignment specifier.</param>
        /// <param name="formatting">The format specifier.</param>
        /// <param name="isExtension">Specifies whether the key was found within triple curly braces.</param>
        public KeyGenerator(string key, string alignment, string formatting, bool isExtension)
        {
            if (key.StartsWith("@"))
            {
                _key = key.Substring(1);
                _isVariable = true;
            }
            else
            {
                _key = key;
                _isVariable = false;
            }
            _format = GetFormat(alignment, formatting);
            _isExtension = isExtension;
        }

        private static string GetFormat(string alignment, string formatting)
        {
            var formatBuilder = new StringBuilder();
            formatBuilder.Append("{0");
            if (!string.IsNullOrWhiteSpace(alignment))
            {
                formatBuilder.Append(",");
                formatBuilder.Append(alignment.TrimStart('+'));
            }
            if (!string.IsNullOrWhiteSpace(formatting))
            {
                formatBuilder.Append(":");
                formatBuilder.Append(formatting);
            }
            formatBuilder.Append("}");
            return formatBuilder.ToString();
        }

        void IGenerator.GetText(TextWriter writer, Scope scope, Scope context, Action<Substitution> postProcessor)
        {
            object value = _isVariable ? context.Find(_key, _isExtension) : scope.Find(_key, _isExtension);
            string result = string.Format(writer.FormatProvider, _format, value);
            var substitution = new Substitution()
            {
                Key = _key,
                Substitute = result,
                IsExtension = _isExtension
            };
            postProcessor(substitution);
            writer.Write(substitution.Substitute);
        }
    }
}
