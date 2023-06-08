﻿using Argus.Extensions;

namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Normalizes any unicode accent marks.
    /// </summary>
    public class NormalizeAccentMarksTagDefinition : ContentTagDefinition
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Argus.Data.Mustache.TagDefinitions.NormalizeAccentMarksTagDefinition"/>.
        /// </summary>
        public NormalizeAccentMarksTagDefinition()
            : base("normalize-accent-marks")
        {
        }

        /// <inheritdoc cref="GetChildContext"/>
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
            return Array.Empty<TagParameter>();
        }

        /// <inheritdoc cref="ConsolidateWriter"/>
        public override string ConsolidateWriter(TextWriter writer, Dictionary<string, object> arguments)
        {
            return writer?.ToString().NormalizeAccentMarks();
        }
    }
}