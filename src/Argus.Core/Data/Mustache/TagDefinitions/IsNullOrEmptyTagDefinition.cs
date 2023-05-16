namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// If a string is null or empty.
    /// </summary>
    public class IsNullOrEmptyTagDefinition : ConditionTagDefinition
    {
        private const string ConditionParameter = "condition";

        public IsNullOrEmptyTagDefinition()
            : base("is-null-or-empty")
        { }

        protected override bool GetIsContextSensitive()
        {
            return false;
        }

        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new[] { new TagParameter(ConditionParameter) { IsRequired = true } };
        }

        private bool IsConditionSatisfied(object condition)
        {
            if (condition == null)
            {
                return true;
            }

            return condition is string str && string.IsNullOrEmpty(str);
        }

        public override bool ShouldGeneratePrimaryGroup(Dictionary<string, object> arguments)
        {
            object condition = arguments[ConditionParameter];
            return IsConditionSatisfied(condition);
        }

        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return Array.Empty<TagParameter>();
        }
    }
}
