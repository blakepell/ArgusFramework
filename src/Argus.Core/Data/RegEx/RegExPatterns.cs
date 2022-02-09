/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2010-07-03
 * @last updated      : 2016-04-08
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Text.RegularExpressions;

namespace Argus.Data.RegEx
{
    /// <summary>
    /// Commonly used regular expression patterns.
    /// </summary>
    public class RegExPatterns
    {
        public const string Alpha = "^[a-zA-Z]*$";
        public const string AlphaUpperCase = "^[A-Z]*$";
        public const string AlphaLowerCase = "^[a-z]*$";
        public const string AlphaNumeric = "^[a-zA-Z0-9]*$";
        public const string AlphaNumericSpace = "^[a-zA-Z0-9 ]*$";
        public const string AlphaNumericSpaceDash = "^[a-zA-Z0-9 \\-]*$";
        public const string AlphaNumericSpaceDashUnderscore = "^[a-zA-Z0-9 \\-_]*$";
        public const string AlphaNumericSpaceDashUnderscorePeriod = "^[a-zA-Z0-9\\. \\-_]*$";
        public const string Binary = "^([0-1])*$";
        public const string Numeric = "^\\-?[0-9]*\\.?[0-9]*$";
        public const string NumericWithCommaAndDecimal = "^(\\d|-)?(\\d|,)*\\.?\\d*$";
        public const string SocialSecurity = "\\d{3}[-]?\\d{2}[-]?\\d{4}";
        public const string Email = "^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";
        public const string Url = "^^(ht|f)tp(s?)\\:\\/\\/[0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*(:(0-9)*)*(\\/?)([a-zA-Z0-9\\-\\.\\?\\,\\'\\/\\\\\\+&%\\$#_=]*)?$";
        public const string ZipCodeUS = "\\d{5}";
        public const string ZipCodeUSWithFour = "\\d{5}[-]\\d{4}";
        public const string ZipCodeUSWithFourOptional = "\\d{5}([-]\\d{4})?";
        public const string PhoneUS = "\\d{3}[-]?\\d{3}[-]?\\d{4}";
        public const string IpAddress = "^([1-9]{1}\\d{1}|[1-9]{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1}|[1-9]{1}\\d{1}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1}|[1-9]{1}\\d{1}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1}|[1-9]{1}\\d{1}|1\\d\\d|2[0-4]\\d|25[0-5])$";

        /// <summary>
        /// All HTML (or XML) tags in a set of text.  This is both opening and closing tags.
        /// </summary>
        public const string HtmlTags = "<[^>]*>";

        /// <summary>
        /// Uses back references and word boundaries to match repeated words separated by whitespace
        /// without matching a word with the same ending as the next words beginning.
        /// </summary>
        public const string RepeatedWords = "\\b(\\w+)\\s+\\1\\b";

        /// <summary>
        /// Finds text that is in between quotes (including the quotes)
        /// </summary>
        public const string BetweenQuotes = "\"([^\"]*)\"";

        /// <summary>
        /// Finds text that is in between single quotes
        /// </summary>
        public const string BetweenSingleQuotes = "'([^']*)'";

        /// <summary>
        /// mm/dd/yyyy hh:MM:ss AM/PM
        /// </summary>
        public const string DateTimeFinder = "(?n:^(?=\\d)((?<month>(0?[13578])|1[02]|(0?[469]|11)(?!.31)|0?2(?(.29)(?=.29.((1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|(16|[2468][048]|[3579][26])00))|(?!.3[01])))(?<sep>[-./])(?<day>0?[1-9]|[12]\\d|3[01])\\k<sep>(?<year>(1[6-9]|[2-9]\\d)\\d{2})(?(?=\\x20\\d)\\x20|$))?(?<time>((0?[1-9]|1[012])(:[0-5]\\d){0,2}(?i:\\x20[AP]M))|([01]\\d|2[0-3])(:[0-5]\\d){";

        /// <summary>
        /// Finds dates in MM/DD/YYYY, MM-DD-YYYY, MM.DD.YYYY or MM DD YYYY format.  Three group variables are defined (case sensitive), these
        /// are 'month', 'day', and 'year'
        /// </summary>
        public const string DateFinderMMDDYYYY = "(?<month>\\d{1,2})[- /.](?<day>\\d{1,2})[- /.](?<year>\\d\\d\\d\\d)\\b";

        /// <summary>
        /// Finds dates in YYYY/MM/DD, YYYY-MM-DD, YYYY.MM.DD, YYYY MM DD format.  Three group variables are defined (case sensitive), these
        /// are 'month', 'day', and 'year'
        /// </summary>
        public const string DateFinderYYYYMMDD = "(?<year>\\d\\d\\d\\d)[- /.](?<month>\\d{1,2})[- /.](?<day>\\d{1,2})\\b";

        /// <summary>
        /// Finds the text that is in between markers provided the two markers.
        /// </summary>
        /// <param name="beginMarker"></param>
        /// <param name="endMarker"></param>
        public static object BetweenMarkers(string beginMarker, string endMarker)
        {
            return string.Format("{0}([^{0}^{1}]*){1}", Regex.Escape(beginMarker), Regex.Escape(endMarker));
        }
    }
}