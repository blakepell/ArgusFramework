﻿using Argus.Extensions;

namespace Argus.Data.Mustache.TagDefinitions
{

    /// <summary>
    /// Tag to capitalize the first character of the first word.
    /// </summary>
    public class CapitalizeTagDefinition : ContentTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CapitalizeTagDefinition"/>.
        /// </summary>
        public CapitalizeTagDefinition()
            : base("capitalize")
        {
        }

        public override IEnumerable<NestedContext> GetChildContext(TextWriter writer, Scope keyScope, Dictionary<string, object> arguments, Scope contextScope)
        {
            var context = new NestedContext()
            {
                KeyScope = keyScope,
                Writer = new StringWriter(),
                WriterNeedsConsolidated = true,
            };

            yield return context;
        }

        /// <inheritdoc cref="GetChildContextParameters"/>
        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return this.GetParameters();
        }

        /// <inheritdoc cref="GetParameters"/>
        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new[] { new TagParameter("collection") };
        }

        /// <inheritdoc cref="ConsolidateWriter"/>
        public override string ConsolidateWriter(TextWriter writer, Dictionary<string, object> arguments)
        {
            return writer?.ToString()?.Capitalize() ?? "";
        }
    }
}