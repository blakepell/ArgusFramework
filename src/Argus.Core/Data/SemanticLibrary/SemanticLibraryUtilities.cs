/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Data.SemanticLibrary
{
    /// <summary>
    /// Helps with serializing an object to XML and back again.
    /// </summary>
    public static class SemanticLibraryUtilities
    {
        /// <summary>
        /// Returns a string list of all keywords from the provided text.
        /// </summary>
        /// <param name="txt">The text to extract keywords from.</param>
        public static List<string> GetKeywords(string txt)
        {
            var ka = new KeywordAnalyzer();
            var g = ka.Analyze(txt);
            var keywordList = new List<string>();

            foreach (var key in g.Keywords)
            {
                keywordList.Add(key.Word);
            }

            return keywordList;
        }

        /// <summary>
        /// Returns a string list of all keywords from the provided text.
        /// </summary>
        /// <param name="txt">The text to extract keywords from.</param>
        /// <param name="rankLimit">The maximum number to return based on the rank provided by the SemanticLibrary.</param>
        public static List<string> GetKeywords(string txt, int rankLimit)
        {
            var ka = new KeywordAnalyzer();
            var g = ka.Analyze(txt);
            var keywordList = new List<string>();
            var gty = (from n in g.Keywords select n).Take(rankLimit);

            foreach (var key in gty)
            {
                keywordList.Add(key.Word);
            }

            return keywordList;
        }
    }
}