namespace Argus.Data.Mustache.TagDefinitions
{
    /// <summary>
    /// Defines a tag that conditionally prints its content, based on whether the passed in values are equal
    /// </summary>
    internal sealed class EqTagDefinition : ConditionTagDefinition
    {
        private const string ConditionParameter = "condition";
        private const string TargetValueParameter = "targetValue";

        /// <summary>
        /// Initializes a new instance of a IfTagDefinition.
        /// </summary>
        public EqTagDefinition()
            : base("eq")
        { }

        /// <summary>
        /// Gets whether the tag only exists within the scope of its parent.
        /// </summary>
        protected override bool GetIsContextSensitive()
        {
            return false;
        }

        /// <summary>
        /// Gets the parameters that can be passed to the tag.
        /// </summary>
        /// <returns>The parameters.</returns>
        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new[] {  new TagParameter(ConditionParameter) { IsRequired = true },
                            new TagParameter(TargetValueParameter){IsRequired = true}  };
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
            object targetValue = arguments[TargetValueParameter];
            return IsConditionSatisfied(condition, targetValue);
        }

        private bool IsConditionSatisfied(object condition, object targetValue)
        {
            // If both are null, they are equal
            if (condition == null && targetValue == null)
            {
                return true;
            }

            // If one of them is null, they are not equal
            if (condition == null || targetValue == null)
            {
                return false;
            }

            // If both are of the same type, compare them directly
            if (condition.GetType() == targetValue.GetType())
            {
                return condition.Equals(targetValue);
            }

            // Check if both are numeric types, convert them to decimals and compare
            if (IsNumeric(condition) && IsNumeric(targetValue))
            {
                decimal decimalCondition = Convert.ToDecimal(condition);
                decimal decimalTarget = Convert.ToDecimal(targetValue);

                return decimalCondition == decimalTarget;
            }

            // If they are of different types and not both numeric, they are not equal
            return false;
        }

        /// <summary>
        /// If the type of the object is numeric based on type (not value, e.g. strings
        /// are not numeric even if they are numbers).
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNumeric(object obj)
        {
            switch (Type.GetTypeCode(obj.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
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
