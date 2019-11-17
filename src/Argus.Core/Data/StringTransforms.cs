using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Argus.Data
{
    /// <summary>
    ///     Class with static methods for transforming string values.
    /// </summary>
    public class StringTransforms
    {
        //*********************************************************************************************************************
        //
        //             Class:  StringTransforms
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  05/07/2008
        //      Last Updated:  11/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Wraps each line of text with a start and an end value.  For example, this could be a start &lt;li&gt; and end &lt;/li&gt; tag if
        ///     each line was an HTML list item.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="lineTerminator"></param>
        public static string WrapLines(string text, string startValue, string endValue, string lineTerminator)
        {
            var lines = text.Split(lineTerminator.ToCharArray());
            var sb = new StringBuilder();

            foreach (string line in lines)
            {
                sb.AppendFormat("{0}{1}{2}{3}", startValue, line, endValue, lineTerminator);
            }

            // If we had lines trim the final line terminator that was provided.
            if (lines.Length > 0 && sb.Length > lineTerminator.Length)
            {
                sb.Length -= lineTerminator.Length;
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Wraps each line of text with a start and an end value.  For example, this could be a start &lt;li&gt; and end &lt;/li&gt; tag if
        ///     each line was an HTML list item.  This overload uses the carriage return line feed (ASCII 13 and 10) as the line terminator.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        public static string WrapLines(string text, string startValue, string endValue)
        {
            return WrapLines(text, startValue, endValue, "\r\n");
        }

        /// <summary>
        ///     Wraps each line of text with a start and an end value.  For example, this could be a start &lt;li&gt; and end &lt;/li&gt; tag if
        ///     each line was an HTML list item.  This also includes parameters to put once before the selection and once after the selection, for instance
        ///     a start list tag and an end list tag.
        /// </summary>
        /// <param name="text">The entire text to parse</param>
        /// <param name="startLineValue">The value to insert at the beginning of every line</param>
        /// <param name="endLineValue">The value to insert at the end of every line</param>
        /// <param name="startSelectionValue">The value to insert once before the text starts, e.g. a start unordered list HTML tag.</param>
        /// <param name="endSelectionValue">The value to insert once after the text ends, e.g. a end unordered list HTML tag.</param>
        /// <param name="lineTerminator">The line terminator to use to parse, typically a carriage return/line feed.</param>
        public static string WrapLines(string text, string startLineValue, string endLineValue, string startSelectionValue, string endSelectionValue, string lineTerminator)
        {
            var lines = text.Split(lineTerminator.ToCharArray());
            var sb = new StringBuilder();

            sb.AppendFormat("{0}{1}", startSelectionValue, lineTerminator);

            foreach (string line in lines)
            {
                sb.AppendFormat("{0}{1}{2}{3}", startLineValue, line, endLineValue, lineTerminator);
            }

            sb.AppendFormat("{0}{1}", endSelectionValue, lineTerminator);

            // If we had lines trim the final line terminator that was provided.
            if (lines.Length > 0 && sb.Length > lineTerminator.Length)
            {
                sb.Length -= lineTerminator.Length;
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Unescapes a string using regular expressions.  This means a \n would be turned into a new line, a \r into a carriage return,
        ///     a \t into a tab, etc.
        /// </summary>
        /// <param name="text"></param>
        public static string Unescape(string text)
        {
            return Regex.Unescape(text);
        }

        /// <summary>
        ///     Escapes text with the regular expression escape values (Regex.Escape).  This means that a tab would be turned into a \t, a newline into
        ///     a \n, a carriage return into a \r, etc.
        /// </summary>
        /// <param name="text"></param>
        public static string Escape(string text)
        {
            return Regex.Escape(text);
        }

        /// <summary>
        ///     This will format a text delimited string for viewing.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="deliminator"></param>
        public static string FormatDelimitedString(string text, string deliminator)
        {
            var lines = text.Split(Environment.NewLine.ToCharArray());
            var fields = lines[0].Split(deliminator.ToCharArray());
            int fieldCount = fields.GetUpperBound(0);

            // field count has to be consistant
            // Loop and find the max length for each field
            var fieldLength = new int[fieldCount + 1];

            foreach (string line in lines)
            {
                fields = line.Split(deliminator.ToCharArray());
                int counter = 0;

                foreach (string field in fields)
                {
                    if (field.Length > fieldLength[counter])
                    {
                        fieldLength[counter] = field.Length;
                    }

                    counter += 1;
                }
            }

            // Loop and recreate the file with padding
            var outputSb = new StringBuilder();

            foreach (string line in lines)
            {
                fields = line.Split(deliminator.ToCharArray());
                int counter = 0;

                foreach (string field in fields)
                {
                    outputSb.AppendFormat("{0,-" + (fieldLength[counter] + 4) + "}", field.Trim());
                    counter = counter + 1;
                }

                outputSb.Append("\r\n");
            }

            return outputSb.ToString();
        }
    }
}