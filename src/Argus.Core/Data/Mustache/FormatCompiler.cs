
// ReSharper disable RedundantEnumerableCastCall

namespace Argus.Data.Mustache
{
    /// <summary>
    /// Parses a format string and returns a text generator.
    /// </summary>
    public sealed class FormatCompiler
    {
        private readonly Dictionary<string, TagDefinition> _tagLookup = new();
        private readonly Dictionary<string, Regex> _regexLookup = new();
        private readonly MasterTagDefinition _masterDefinition = new();

        /// <summary>
        /// Initializes a new instance of a FormatCompiler.
        /// </summary>
        public FormatCompiler()
        {
            // Reflect over all of the ITagDefinition's that are available.
            var type = typeof(ITagDefinition);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                                 .SelectMany(x => x.GetTypes())
                                 .Where(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            foreach (var t in types)
            {
                var tag = (ITagDefinition)Activator.CreateInstance(t, true);

                if (!string.IsNullOrWhiteSpace(tag?.Name))
                {
                    _tagLookup.Add(tag.Name, (TagDefinition)tag);
                }
            }
        }

        /// <summary>
        /// Occurs when a placeholder is found in the template.
        /// </summary>
        public event EventHandler<PlaceholderFoundEventArgs> PlaceholderFound;

        /// <summary>
        /// Occurs when a variable is found in the template.
        /// </summary>
        public event EventHandler<VariableFoundEventArgs> VariableFound;

        /// <summary>
        /// Gets or sets whether newlines are removed from the template.
        /// </summary>
        public bool RemoveNewLines { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the compiler searches for tags using triple curly braces.
        /// </summary>
        public bool AreExtensionTagsAllowed { get; set; }

        /// <summary>
        /// Registers the given tag definition with the parser.
        /// </summary>
        /// <param name="definition">The tag definition to register.</param>
        /// <param name="isTopLevel">Specifies whether the tag is immediately in scope.</param>
        public void RegisterTag(TagDefinition definition, bool isTopLevel)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (_tagLookup.ContainsKey(definition.Name))
            {
                string message = $"The {definition.Name} tag has already been registered.";
                throw new ArgumentException(message, nameof(definition));
            }

            _tagLookup.Add(definition.Name, definition);
        }

        /// <summary>
        /// Builds a text generator based on the given format.
        /// </summary>
        /// <param name="format">The format to parse.</param>
        /// <returns>The text generator.</returns>
        public Generator Compile(string format)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            var generator = new CompoundGenerator(_masterDefinition, new ArgumentCollection());
            var context = new List<Context>() { new(_masterDefinition.Name, Array.Empty<ContextParameter>()) };
            int formatIndex = this.BuildCompoundGenerator(_masterDefinition, context, generator, format, 0);
            string trailing = format.Substring(formatIndex);
            generator.AddGenerator(new StaticGenerator(trailing, this.RemoveNewLines));
            return new Generator(generator);
        }

        private Match FindNextTag(TagDefinition definition, string format, int formatIndex)
        {
            var regex = this.PrepareRegex(definition);
            return regex.Match(format, formatIndex);
        }

        private Regex PrepareRegex(TagDefinition definition)
        {
            if (!_regexLookup.TryGetValue(definition.Name, out var regex))
            {
                var matches = new List<string>()
                {
                    GetKeyRegex(),
                    GetCommentTagRegex()
                };

                foreach (string closingTag in definition.ClosingTags)
                {
                    matches.Add(GetClosingTagRegex(closingTag));
                }

                foreach (var globalDefinition in _tagLookup.Values)
                {
                    if (!globalDefinition.IsContextSensitive)
                    {
                        matches.Add(GetTagRegex(globalDefinition));
                    }
                }

                foreach (string childTag in definition.ChildTags)
                {
                    var childDefinition = _tagLookup[childTag];
                    matches.Add(GetTagRegex(childDefinition));
                }

                matches.Add(GetUnknownTagRegex());
                string combined = string.Join("|", matches);
                string match = "{{(?<match>" + combined + ")}}";

                if (this.AreExtensionTagsAllowed)
                {
                    string tripleMatch = "{{{(?<extension>" + combined + ")}}}";
                    match = "(?:" + match + ")|(?:" + tripleMatch + ")";
                }

                regex = new Regex(match);
                _regexLookup.Add(definition.Name, regex);
            }

            return regex;
        }

        private static string GetClosingTagRegex(string tagName)
        {
            var regexBuilder = new StringBuilder();
            regexBuilder.Append(@"(?<close>(/(?<name>");
            regexBuilder.Append(tagName);
            regexBuilder.Append(@")\s*?))");
            return regexBuilder.ToString();
        }

        private static string GetCommentTagRegex()
        {
            return @"(?<comment>#!.*?)";
        }

        private static string GetKeyRegex()
        {
            return @"((?<key>" + RegexHelper.CompoundKey + @")(,(?<alignment>(\+|-)?[\d]+))?(:(?<format>.*?))?)";
        }

        private static string GetTagRegex(TagDefinition definition)
        {
            var regexBuilder = new StringBuilder();
            regexBuilder.Append(@"(?<open>(#(?<name>");
            regexBuilder.Append(definition.Name);
            regexBuilder.Append(@")");
            foreach (var parameter in definition.Parameters)
            {
                regexBuilder.Append(@"(\s+?");
                regexBuilder.Append(@"(?<argument>(");
                regexBuilder.Append(RegexHelper.Argument);
                regexBuilder.Append(@")))");
                if (!parameter.IsRequired)
                {
                    regexBuilder.Append("?");
                }
            }

            regexBuilder.Append(@"\s*?))");

            return regexBuilder.ToString();
        }

        private static string GetUnknownTagRegex()
        {
            return @"(?<unknown>(#.*?))";
        }

        private int BuildCompoundGenerator(
            TagDefinition tagDefinition,
            List<Context> context,
            CompoundGenerator generator,
            string format, int formatIndex)
        {
            while (true)
            {
                var match = this.FindNextTag(tagDefinition, format, formatIndex);

                if (!match.Success)
                {
                    if (tagDefinition.ClosingTags.Any())
                    {
                        string message = $"Expected a matching {tagDefinition.Name} tag but none was found.";
                        throw new FormatException(message);
                    }

                    break;
                }

                string leading = format.Substring(formatIndex, match.Index - formatIndex);

                if (match.Groups["key"].Success)
                {
                    generator.AddGenerator(new StaticGenerator(leading, this.RemoveNewLines));
                    formatIndex = match.Index + match.Length;
                    bool isExtension = match.Groups["extension"].Success;
                    string key = match.Groups["key"].Value;
                    string alignment = match.Groups["alignment"].Value;
                    string formatting = match.Groups["format"].Value;
                    if (key.StartsWith("@"))
                    {
                        var args = new VariableFoundEventArgs(key.Substring(1), alignment, formatting, isExtension,
                            context.ToArray());
                        if (this.VariableFound != null)
                        {
                            this.VariableFound(this, args);
                            key = "@" + args.Name;
                            alignment = args.Alignment;
                            formatting = args.Formatting;
                            isExtension = args.IsExtension;
                        }
                    }
                    else
                    {
                        var args = new PlaceholderFoundEventArgs(key, alignment, formatting, isExtension,
                            context.ToArray());
                        if (this.PlaceholderFound != null)
                        {
                            this.PlaceholderFound(this, args);
                            key = args.Key;
                            alignment = args.Alignment;
                            formatting = args.Formatting;
                            isExtension = args.IsExtension;
                        }
                    }

                    var keyGenerator = new KeyGenerator(key, alignment, formatting, isExtension);
                    generator.AddGenerator(keyGenerator);
                }
                else if (match.Groups["open"].Success)
                {
                    formatIndex = match.Index + match.Length;
                    string tagName = match.Groups["name"].Value;
                    var nextDefinition = _tagLookup[tagName];
                    if (nextDefinition == null)
                    {
                        string message = $"Encountered an unknown tag: {tagName}. It was either not registered or exists in a different context.";
                        throw new FormatException(message);
                    }

                    generator.AddGenerator(new StaticGenerator(leading, this.RemoveNewLines));
                    var arguments = this.GetArguments(nextDefinition, match, context);

                    if (nextDefinition.HasContent)
                    {
                        var compoundGenerator = new CompoundGenerator(nextDefinition, arguments);
                        var contextParameters = nextDefinition.GetChildContextParameters();
                        bool hasContext = contextParameters.Any();
                        if (hasContext)
                        {
                            var parameters = contextParameters
                                .Select(p => new ContextParameter(p.Name, arguments.GetKey(p))).ToArray();
                            context.Add(new Context(nextDefinition.Name, parameters));
                        }

                        formatIndex = this.BuildCompoundGenerator(nextDefinition, context, compoundGenerator, format,
                            formatIndex);
                        generator.AddGenerator(nextDefinition, compoundGenerator);
                        if (hasContext)
                        {
                            context.RemoveAt(context.Count - 1);
                        }
                    }
                    else
                    {
                        var inlineGenerator = new InlineGenerator(nextDefinition, arguments);
                        generator.AddGenerator(inlineGenerator);
                    }
                }
                else if (match.Groups["close"].Success)
                {
                    generator.AddGenerator(new StaticGenerator(leading, this.RemoveNewLines));
                    string tagName = match.Groups["name"].Value;
                    formatIndex = match.Index;
                    if (tagName == tagDefinition.Name)
                    {
                        formatIndex += match.Length;
                    }

                    break;
                }
                else if (match.Groups["comment"].Success)
                {
                    generator.AddGenerator(new StaticGenerator(leading, this.RemoveNewLines));
                    formatIndex = match.Index + match.Length;
                }
                else if (match.Groups["unknown"].Success)
                {
                    string tagName = match.Value;
                    string message = $"Encountered an unknown tag: {tagName}. It was either not registered or exists in a different context.";
                    throw new FormatException(message);
                }
            }

            return formatIndex;
        }

        private ArgumentCollection GetArguments(TagDefinition definition, Match match, List<Context> context)
        {
            // make sure we don't have too many arguments
            var captures = match.Groups["argument"].Captures.Cast<Capture>().ToList();
            var parameters = definition.Parameters.ToList();
            if (captures.Count > parameters.Count)
            {
                string message = $"The wrong number of arguments were passed to an {definition.Name} tag.";
                throw new FormatException(message);
            }

            // provide default values for missing arguments
            if (captures.Count < parameters.Count)
            {
                captures.AddRange(Enumerable.Repeat((Capture)null, parameters.Count - captures.Count));
            }

            // pair up the parameters to the given arguments
            // provide default for parameters with missing arguments
            // throw an error if a missing argument is for a required parameter
            var arguments = new Dictionary<TagParameter, string>();
            foreach (var pair in parameters.Zip(captures, (p, c) => new { Capture = c, Parameter = p }))
            {
                string value = null;
                if (pair.Capture != null)
                {
                    value = pair.Capture.Value;
                }
                else if (pair.Parameter.IsRequired)
                {
                    string message = $"The wrong number of arguments were passed to an {definition.Name} tag.";
                    throw new FormatException(message);
                }

                arguments.Add(pair.Parameter, value);
            }

            // indicate that a key/variable has been encountered
            // update the key/variable name
            var collection = new ArgumentCollection();
            foreach (var pair in arguments)
            {
                string placeholder = pair.Value;
                IArgument argument = null;
                if (placeholder != null)
                {
                    if (placeholder.StartsWith("@"))
                    {
                        string variableName = placeholder.Substring(1);
                        var args = new VariableFoundEventArgs(placeholder.Substring(1), string.Empty, string.Empty,
                            false, context.ToArray());
                        if (this.VariableFound != null)
                        {
                            this.VariableFound(this, args);
                            variableName = args.Name;
                        }

                        argument = new VariableArgument(variableName);
                    }
                    else if (RegexHelper.IsString(placeholder))
                    {
                        string value = placeholder.Trim('\'');
                        argument = new StringArgument(value);
                    }
                    else if (RegexHelper.IsNumber(placeholder))
                    {
                        if (decimal.TryParse(placeholder, out decimal number))
                        {
                            argument = new NumberArgument(number);
                        }
                    }
                    else
                    {
                        string placeholderName = placeholder;
                        var args = new PlaceholderFoundEventArgs(placeholder, string.Empty, string.Empty, false,
                            context.ToArray());
                        if (this.PlaceholderFound != null)
                        {
                            this.PlaceholderFound(this, args);
                            placeholderName = args.Key;
                        }

                        argument = new PlaceholderArgument(placeholderName);
                    }
                }

                collection.AddArgument(pair.Key, argument);
            }

            return collection;
        }
    }
}