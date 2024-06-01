/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-12-12
 * @last updated      : 2021-12-12
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Audio
{
    /// <summary>
    /// Logic for transposing chords.
    /// </summary>
    public static class ChordTransposer
    {
        /// <summary>
        /// A list of the chords (with sharps)
        /// </summary>
        private static readonly string[] _baseChords = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        /// <summary>
        /// A list of the chords (with flats)
        /// </summary>
        private static readonly string[] _baseChordsFlat = { "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B" };

        /// <summary>
        /// Transposes the chord into another chord.
        /// </summary>
        /// <param name="chord"></param>
        /// <param name="increment"></param>
        public static string Transpose(string chord, int increment)
        {
            if (string.IsNullOrWhiteSpace(chord))
            {
                return string.Empty;
            }

            // We're going to cleanup E# and B# if they come in to make them F & C.
            if (chord.Contains("E#"))
            {
                chord = chord.Replace("E#", "F");
            }

            if (chord.Contains("B#"))
            {
                chord = chord.Replace("B#", "C");
            }

            // Trim any white space
            if (chord.Contains(' '))
            {
                chord = chord.Replace(" ", "");
            }

            string baseChord;
            bool flat = false;

            // This will mostly work.  We're going to look at the first or first and second
            // characters to determine what the chord/note is to shift.  Anything else after
            // will just get concatenated on.
            if (chord.Length > 1 && chord[1] == '#')
            {
                baseChord = chord.Substring(0, 2).ToUpper();
            }
            else if (chord.Length > 1 && chord[1] == 'b')
            {
                flat = true;
                baseChord = $"{chord.Substring(0, 1).ToUpper()}b";
            }
            else
            {
                baseChord = chord.Substring(0, 1).ToUpper();
            }

            int index;
            int newIndex;

            if (flat)
            {
                index = Array.IndexOf(_baseChordsFlat, baseChord);
                newIndex = (index + increment + 12) % 12;
                return $"{_baseChordsFlat[newIndex]}{chord.Substring(baseChord.Length)}";
            }

            index = Array.IndexOf(_baseChords, baseChord);
            newIndex = (index + increment + 12) % 12;
            return $"{_baseChords[newIndex]}{chord.Substring(baseChord.Length)}";
        }

        /// <summary>
        /// Transposes all of the chords in brackets in a set of text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="increment"></param>
        public static string TransposeText(string text, int increment)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            var sbChord = new StringBuilder(32);

            var span = text.AsSpan();
            bool inBracket = false;

            for (int i = 0; i < span.Length; i++)
            {
                char c = span[i];

                if (c == '[')
                {
                    sb.Append('[');
                    inBracket = true;
                    continue;
                }

                if (inBracket && c != ']')
                {
                    sbChord.Append(c);
                    continue;
                }

                if (inBracket && c == ']')
                {
                    inBracket = false;
                    sb.Append(Transpose(sbChord.ToString(), increment)).Append(']');
                    sbChord.Clear();
                    continue;
                }

                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}