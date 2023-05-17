﻿namespace Argus.Data.Mustache
{
    /// <summary>
    /// Holds the information describing a variable that is found in a template.
    /// </summary>
    public class VariableFoundEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of a VariableFoundEventArgs.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="alignment">The alignment that will be applied to the substitute value.</param>
        /// <param name="formatting">The formatting that will be applied to the substitute value.</param>
        /// <param name="isExtension">Specifies whether the variable was found within triple curly braces.</param>
        /// <param name="context">The context where the placeholder was found.</param>
        internal VariableFoundEventArgs(string name, string alignment, string formatting, bool isExtension, Context[] context)
        {
            this.Name = name;
            this.Alignment = alignment;
            this.Formatting = formatting;
            this.IsExtension = isExtension;
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets the key that was found.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the alignment that will be applied to the substitute value.
        /// </summary>
        public string Alignment { get; set; }

        /// <summary>
        /// Gets or sets the formatting that will be applied to the substitute value.
        /// </summary>
        public string Formatting { get; set; }

        /// <summary>
        /// Gets or sets whether variable was found within triple curly braces.
        /// </summary>
        public bool IsExtension { get; set; }

        /// <summary>
        /// Gets the context where the placeholder was found.
        /// </summary>
        public Context[] Context { get; }
    }
}