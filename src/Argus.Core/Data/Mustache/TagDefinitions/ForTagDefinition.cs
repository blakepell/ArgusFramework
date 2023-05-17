namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// For loop: {{#for 1 10 1}}{{index}}{{/for}}
    /// </summary>
    internal sealed class ForTagDefinition : ContentTagDefinition
    {
        private const string StartParameter = "start";
        private const string EndParameter = "end";
        private const string StepParameter = "step";
        private static readonly TagParameter Start = new(StartParameter) { IsRequired = true };
        private static readonly TagParameter End = new(EndParameter) { IsRequired = true };
        private static readonly TagParameter Step = new(StepParameter) { IsRequired = true };
        private const int LOOP_GUARD_UPPER = 2_147_483_647;
        private const int LOOP_GUARD_LOWER = -2_147_483_647;

        public ForTagDefinition()
            : base("for", true)
        {
        }

        protected override bool GetIsContextSensitive()
        {
            return false;
        }

        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new[] { Start, End, Step };
        }

        public override IEnumerable<NestedContext> GetChildContext(
            TextWriter writer,
            Scope keyScope,
            Dictionary<string, object> arguments,
            Scope contextScope)
        {
            int start = int.Parse(arguments[StartParameter].ToString());
            int end = int.Parse(arguments[EndParameter].ToString());
            int step = int.Parse(arguments[StepParameter].ToString());
            int counter = 0;

            if (end > start && step > 0)
            {
                // Forwards
                for (int i = start; i <= end; i += step)
                {
                    counter++;

                    var childContext = new NestedContext()
                    {
                        KeyScope = keyScope.CreateChildScope(i),
                        Writer = writer,
                        ContextScope = contextScope.CreateChildScope(),
                    };

                    // Accessed with @index
                    childContext.ContextScope.Set("index", i);

                    if (counter >= LOOP_GUARD_UPPER)
                    {
                        break;
                    }

                    yield return childContext;
                }
            }
            else
            {
                // Backwards
                for (int i = start; i >= end; i += step)
                {
                    counter++;

                    var childContext = new NestedContext()
                    {
                        KeyScope = keyScope.CreateChildScope(i),
                        Writer = writer,
                        ContextScope = contextScope.CreateChildScope(),
                    };

                    if (counter <= LOOP_GUARD_LOWER)
                    {
                        break;
                    }

                    // Accessed with @index
                    childContext.ContextScope.Set("index", i);
                    yield return childContext;
                }
            }
        }

        protected override IEnumerable<string> GetChildTags()
        {
            return new[] { "index" };
        }

        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return new[] { Start, End, Step };
        }
    }
}
