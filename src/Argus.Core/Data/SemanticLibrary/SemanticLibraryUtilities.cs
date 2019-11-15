using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <returns></returns>
        public static List<string> GetKeywords(String txt)
        {
            KeywordAnalyzer ka = new KeywordAnalyzer();
            var g = ka.Analyze(txt);
            List<string> keywordList = new List<string>();

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
        /// <returns></returns>
        public static List<string> GetKeywords(String txt, int rankLimit)
        {

            KeywordAnalyzer ka = new KeywordAnalyzer();
            var g = ka.Analyze(txt);
            List<string> keywordList = new List<string>();
            var gty = (from n in g.Keywords select n).Take(rankLimit);

            foreach (var key in gty)
            {
                keywordList.Add(key.Word);
            }

            return keywordList;
        }

    }

}