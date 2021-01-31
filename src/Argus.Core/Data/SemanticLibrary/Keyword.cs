/*
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System.Collections.Generic;

namespace Argus.Data.SemanticLibrary
{
    public class KeywordAnalysis
    {
        public string Content { get; set; }
        public int WordCount { get; set; }
        public IEnumerable<Keyword> Keywords { get; set; }
        public List<Paragraph> Paragraphs { get; set; }
        public IEnumerable<Title> Titles { get; set; }
    }

    public class Keyword
    {
        public string Word { get; set; }
        public decimal Rank { get; set; }
    }

    public class Word
    {
        public string Text { get; set; }
        public string Stem { get; set; }
    }

    public class Sentence
    {
        public Sentence()
        {
            this.Words = new List<Word>();
        }

        public List<Word> Words { get; set; }
    }

    public class Paragraph
    {
        public Paragraph()
        {
            this.Sentences = new List<Sentence>();
        }

        public List<Sentence> Sentences { get; set; }
    }
}