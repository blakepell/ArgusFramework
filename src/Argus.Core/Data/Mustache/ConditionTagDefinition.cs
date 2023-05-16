﻿namespace Argus.Data.Mustache
{
    /// <summary>
    /// Defines a tag that conditionally prints its content.
    /// </summary>
    public abstract class ConditionTagDefinition : ContentTagDefinition
    {
        private const string ConditionParameter = "condition";

        /// <summary>
        /// Initializes a new instance of a ConditionTagDefinition.
        /// </summary>
        /// <param name="tagName">The name of the tag.</param>
        protected ConditionTagDefinition(string tagName)
            : base(tagName, true)
        {
        }

        /// <summary>
        /// Gets the parameters that can be passed to the tag.
        /// </summary>
        /// <returns>The parameters.</returns>
        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new[] { new TagParameter(ConditionParameter) { IsRequired = true } };
        }

        /// <summary>
        /// Gets the tags that come into scope within the context of the current tag.
        /// </summary>
        /// <returns>The child tag definitions.</returns>
        protected override IEnumerable<string> GetChildTags()
        {
            return new[] { "elif", "else" };
        }

        /// <summary>
        /// Gets whether the given tag's generator should be used for a secondary (or substitute) text block.
        /// </summary>
        /// <param name="definition">The tag to inspect.</param>
        /// <returns>True if the tag's generator should be used as a secondary generator.</returns>
        public override bool ShouldCreateSecondaryGroup(TagDefinition definition)
        {
            return new[] { "elif", "else" }.Contains(definition.Name);
        }

        /// <summary>
        /// Gets whether the primary generator group should be used to render the tag.
        /// </summary>
        /// <param name="arguments">The arguments passed to the tag.</param>
        /// <returns>
        /// True if the primary generator group should be used to render the tag;
        /// otherwise, false to use the secondary group.
        /// </returns>
        public override bool ShouldGeneratePrimaryGroup(Dictionary<string, object> arguments)
        {
            object condition = arguments[ConditionParameter];
            return this.IsConditionSatisfied(condition);
        }

        private bool IsConditionSatisfied(object condition)
        {
            if (condition == null || condition == DBNull.Value)
            {
                return false;
            }
            if (condition is IEnumerable enumerable)
            {
                return enumerable.Cast<object>().Any();
            }
            if (condition is char)
            {
                return (char)condition != '\0';
            }
            try
            {
                decimal number = (decimal)Convert.ChangeType(condition, typeof(decimal));
                return number != 0.0m;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the parameters that are used to create a new child context.
        /// </summary>
        /// <returns>The parameters that are used to create a new child context.</returns>
        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return Array.Empty<TagParameter>();
        }
    }
}
