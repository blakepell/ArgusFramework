using System.Diagnostics.CodeAnalysis;

namespace Argus.Data.Mustache
{
    /// <summary>
    /// Provides utility methods that require regular expressions.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal static class RegexHelper
    {
        public const string Key = @"[-_\w][-_\w\d]*";
        public const string String = @"'.*?'";
        public const string Number = @"[-+]?\d*\.?\d+";
        public const string CompoundKey = "@?" + Key + @"(?:\." + Key + ")*";
        public const string Argument = @"(?:(?<arg_key>" + CompoundKey + @")|(?<arg_string>" + String +
                                       @")|(?<arg_number>" + Number + @"))";
        private static readonly Regex IsValidIdentifierRegex = new("^" + Key + "$");
        private static readonly Regex IsStringRegex = new("^" + String + "$");
        private static readonly Regex IsNumberRegex = new("^" + Number + "$");
        
        /// <summary>
        /// Determines whether the given name is a legal identifier.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns>True if the name is a legal identifier; otherwise, false.</returns>
        public static bool IsValidIdentifier(string name)
        {
            return name != null && IsValidIdentifierRegex.IsMatch(name);
        }
        
        public static bool IsString(string value)
        {
            return value != null && IsStringRegex.IsMatch(value);
        }

        public static bool IsNumber(string value)
        {
            return value != null && IsNumberRegex.IsMatch(value);
        }
    }
}