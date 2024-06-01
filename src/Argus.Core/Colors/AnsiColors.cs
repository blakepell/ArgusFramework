/*
 * @author            : Blake Pell
 * @initial date      : 2022-08-27
 * @last updated      : 2022-08-27
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System.Diagnostics.CodeAnalysis;
using Cysharp.Text;

namespace Argus.Colors
{
    /// <summary>
    /// ANSI color code handling.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class AnsiColors
    {
        /// <summary>
        /// The escape character used to start ANSI sequences.
        /// </summary>
        private const char ANSI_ESCAPE = '\x1B';

        /// <summary>
        /// Escape character used in the user friendly color code mapping.
        /// </summary>
        public static char Escape = '{';

        /// <summary>
        /// Friendly color code mapping.  The key will be preceded by some custom
        /// escape character.
        /// </summary>
        public static readonly Dictionary<char, string> Colors = new()
        {
            { 'x', "\x1B[0m" },
            { 'D', "\x1B[1;30m" },
            { 'R', "\x1B[1;31m" },
            { 'r', "\x1B[0;31m" },
            { 'G', "\x1B[1;32m" },
            { 'g', "\x1B[0;32m" },
            { 'Y', "\x1B[1;33m" },
            { 'y', "\x1B[0;33m" },
            { 'B', "\x1B[1;34m" },
            { 'b', "\x1B[0;34m" },
            { 'M', "\x1B[1;35m" },
            { 'm', "\x1B[0;35m" },
            { 'C', "\x1B[1;36m" },
            { 'c', "\x1B[0;36m" },
            { 'W', "\x1B[1;37m" },
            { 'w', "\x1B[0;37m" },
            { 'n', "\x1B[38;5;130m" },
            { 'u', "\x1B[38;5;61m" },
            { 'o', "\x1B[38;5;166m" },
            { 'p', "\x1B[38;5;205m" },
            { '&', "\x1B[7m" },
            { '_', "\x1B[4m" }
        };

        /// <summary>
        /// Removes all ANSI codes from a string (including control codes).
        /// </summary>
        /// <param name="input"></param>
        public static string RemoveAnsiCodes(string input)
        {
            var span = input.AsSpan();

            if (span.IndexOf(ANSI_ESCAPE) == -1)
            {
                return input;
            }

            using (var sb = ZString.CreateStringBuilder())
            {
                bool flag = false;

                for (int i = 0; i < span.Length; i++)
                {
                    var c = span[i];

                    switch (flag)
                    {
                        case true when char.IsLetter(c):
                            flag = false;

                            break;
                        case false when c == ANSI_ESCAPE:
                            flag = true;

                            break;
                        case false:
                            sb.Append(c);

                            break;
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Converts the string containing the friendly color code format to
        /// it's ANSI colorized equivalent.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Colorize(string input)
        {
            var span = input.AsSpan();

            if (span.IndexOf(Escape) == -1)
            {
                return input;
            }

            using (var sb = ZString.CreateStringBuilder())
            {
                bool flag = false;

                for (int i = 0; i < span.Length; i++)
                {
                    var c = span[i];

                    // This supposed to be a color code.
                    if (flag == true)
                    {
                        if (Colors.TryGetValue(c, out string ansi))
                        {
                            sb.Append(ansi);
                        }

                        flag = false;
                        continue;
                    }

                    if (c == Escape)
                    {
                        flag = true;
                        continue;
                    }

                    sb.Append(c);
                }

                return sb.ToString();
            }
        }

        // Unsafe example with pointers.  Slightly faster.
        //public static unsafe string RemoveAnsiCodesUnsafe(string input)
        //{
        //    const char escapeChar = '\x1B';

        //    if (input.IndexOf(escapeChar) == -1)
        //    {
        //        return input;
        //    }

        //    using (var sb = ZString.CreateStringBuilder())
        //    {
        //        bool flag = false;

        //        fixed (char* ptr = input)
        //        {
        //            char* pChar = ptr;

        //            for (int i = 0; i < input.Length; i++)
        //            {
        //                var c = *pChar;

        //                switch (flag)
        //                {
        //                    case true when char.IsLetter(c):
        //                        flag = false;

        //                        break;
        //                    case false when c == escapeChar:
        //                        flag = true;

        //                        break;
        //                    case false:
        //                        sb.Append(c);

        //                        break;
        //                }

        //                pChar++;
        //            }
        //        }

        //        return sb.ToString();
        //    }
        //}
    }
}
