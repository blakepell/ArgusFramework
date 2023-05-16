namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// If an object is an <see cref="IEnumerable"/> and contains any elements.
    /// </summary>
    public class AnyTagDefinition : ContentTagDefinition
    {
        private const string ConditionParameter = "condition";

        public AnyTagDefinition()
            : base("Any")
        { }

        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return Array.Empty<TagParameter>();
        }

        protected override bool GetIsContextSensitive()
        {
            return false;
        }

        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new[] { new TagParameter(ConditionParameter) { IsRequired = true } };
        }

        private static bool IsConditionSatisfied(object condition)
        {
            return condition is IEnumerable enumerable && enumerable.Cast<object>().Any();
        }

        public override bool ShouldGeneratePrimaryGroup(Dictionary<string, object> arguments)
        {
            var condition = arguments[ConditionParameter];
            return IsConditionSatisfied(condition);
        }
    }
}
