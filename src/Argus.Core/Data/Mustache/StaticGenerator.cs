namespace Argus.Data.Mustache
{
    /// <summary>
    /// Generates a static block of text.
    /// </summary>
    internal sealed class StaticGenerator : IGenerator
    {
        /// <summary>
        /// Initializes a new instance of a StaticGenerator.
        /// </summary>
        public StaticGenerator(string value, bool removeNewLines)
        {
            this.Value = removeNewLines ? value.Replace(Environment.NewLine, string.Empty) : value;
        }

        /// <summary>
        /// Gets or sets the static text.
        /// </summary>
        public string Value { get; }

        void IGenerator.GetText(TextWriter writer, Scope scope, Scope context, Action<Substitution> postProcessor)
        {
            writer.Write(this.Value);
        }
    }
}
