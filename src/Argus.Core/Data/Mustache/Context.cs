﻿namespace Argus.Data.Mustache
{
    /// <summary>
    /// Represents a context within a template.
    /// </summary>
    public sealed class Context
    {
        /// <summary>
        /// Initializes a new instance of a Context.
        /// </summary>
        /// <param name="tagName">The name of the tag that created the context.</param>
        /// <param name="parameters"></param>
        internal Context(string tagName, ContextParameter[] parameters)
        {
            this.TagName = tagName;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets the tag that created the context.
        /// </summary>
        public string TagName { get; }

        /// <summary>
        /// Gets the argument used to create the context.
        /// </summary>
        public ContextParameter[] Parameters { get; }
    }
}
