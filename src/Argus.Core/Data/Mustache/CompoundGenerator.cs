namespace Argus.Data.Mustache
{
    /// <summary>
    /// Builds text by combining the output of other generators.
    /// </summary>
    internal sealed class CompoundGenerator : IGenerator
    {
        private readonly TagDefinition _definition;
        private readonly ArgumentCollection _arguments;
        private readonly List<IGenerator> _primaryGenerators = new();
        private IGenerator _subGenerator;

        /// <summary>
        /// Initializes a new instance of a CompoundGenerator.
        /// </summary>
        /// <param name="definition">The tag that the text is being generated for.</param>
        /// <param name="arguments">The arguments that were passed to the tag.</param>
        public CompoundGenerator(TagDefinition definition, ArgumentCollection arguments)
        {
            _definition = definition;
            _arguments = arguments;
        }

        /// <summary>
        /// Adds the given generator. 
        /// </summary>
        /// <param name="generator">The generator to add.</param>
        public void AddGenerator(IGenerator generator)
        {
            this.AddGenerator(generator, false);
        }

        /// <summary>
        /// Adds the given generator, determining whether the generator should
        /// be part of the primary generators or added as an secondary generator.
        /// </summary>
        /// <param name="definition">The tag that the generator is generating text for.</param>
        /// <param name="generator">The generator to add.</param>
        public void AddGenerator(TagDefinition definition, IGenerator generator)
        {
            bool isSubGenerator = _definition.ShouldCreateSecondaryGroup(definition);
            this.AddGenerator(generator, isSubGenerator);
        }

        private void AddGenerator(IGenerator generator, bool isSubGenerator)
        {
            if (isSubGenerator)
            {
                _subGenerator = generator;
            }
            else
            {
                _primaryGenerators.Add(generator);
            }
        }

        void IGenerator.GetText(TextWriter writer, Scope keyScope, Scope contextScope, Action<Substitution> postProcessor)
        {
            var arguments = _arguments.GetArguments(keyScope, contextScope);
            var contexts = _definition.GetChildContext(writer, keyScope, arguments, contextScope);
            List<IGenerator> generators;
            if (_definition.ShouldGeneratePrimaryGroup(arguments))
            {
                generators = _primaryGenerators;
            }
            else
            {
                generators = new List<IGenerator>();
                if (_subGenerator != null)
                {
                    generators.Add(_subGenerator);
                }
            }
            foreach (var context in contexts)
            {
                foreach (var generator in generators)
                {
                    generator.GetText(context.Writer ?? writer, context.KeyScope ?? keyScope, context.ContextScope, postProcessor);
                }
                if (context.WriterNeedsConsolidated)
                {
                    writer.Write(_definition.ConsolidateWriter(context.Writer ?? writer, arguments));
                }
            }
        }
    }
}