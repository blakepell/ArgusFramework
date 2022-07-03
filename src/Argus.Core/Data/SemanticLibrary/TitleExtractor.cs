/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Data.SemanticLibrary
{
    public class Title
    {
        public string Text { get; set; }
        public int Count { get; set; }
    }

    public static class TitleExtractor
    {
        private static readonly Regex regtitle = new Regex(@"(?<=(\s|^))"
                                                           + @"[A-Z\.0-9][A-Za-z0-9]*?[\.\-]*[A-Za-z0-9]+?"
                                                           + @"((\s[a-z]{1,3}){0,2}\s[A-Z\.0-9][A-Za-z0-9]*?[\.\-]*[A-Za-z0-9]+?){1,4}"
                                                           + @"(?=(\.|\?|!|\s|$))", RegexOptions.Compiled | RegexOptions.Multiline);

        public static IEnumerable<Title> Extract(string content)
        {
            var titles = new Dictionary<string, int>();
            var mc = regtitle.Matches(content);

            foreach (Match m in mc)
            {
                if (!titles.ContainsKey(m.Value))
                {
                    titles.Add(m.Value, 0);
                }

                titles[m.Value]++;
            }

            var list = from n in titles select new Title {Text = n.Key, Count = n.Value};

            return list;
        }
    }
}