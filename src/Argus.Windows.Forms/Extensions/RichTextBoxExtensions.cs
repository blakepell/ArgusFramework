/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-12-13
 * @last updated      : 2019-03-13
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System.Drawing;
using System.Windows.Forms;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="RichTextBox" />.
    /// </summary>
    public static class RichTextBoxExtensions
    {
        /// <summary>
        /// Scrolls and places the caret at the end of the RichTextBox.
        /// </summary>
        /// <param name="rtb"></param>
        public static void ScrollToEnd(this RichTextBox rtb)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.ScrollToCaret();
        }

        /// <summary>
        /// Scrolls and places the caret at the beginning of the RichTextBox.
        /// </summary>
        /// <param name="rtb"></param>
        public static void ScrollToBeginning(this RichTextBox rtb)
        {
            rtb.DeselectAll();
            rtb.Select(0, 0);
            rtb.ScrollToCaret();
        }

        /// <summary>
        /// Appends text at the current cursor position.
        /// </summary>
        /// <param name="rtb"></param>
        /// <param name="text"></param>
        public static void AppendAtCursor(this RichTextBox rtb, string text)
        {
            rtb.SelectionLength = 0;
            rtb.SelectedText = text;
        }

        /// <summary>
        /// Highlights a word in the RichTextBox with the specified foreground and background colors.
        /// </summary>
        /// <param name="rtb"></param>
        /// <param name="word"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        public static void HighlightWord(this RichTextBox rtb, string word, Color foregroundColor, Color backgroundColor)
        {
            int len = rtb.TextLength;
            int lastIndex = rtb.Text.LastIndexOf(word);
            int index = 0;

            while (index < lastIndex)
            {
                rtb.Find(word, index, len, RichTextBoxFinds.None);
                rtb.SelectionColor = foregroundColor;
                rtb.SelectionBackColor = backgroundColor;
                rtb.SelectionFont = new Font(rtb.SelectionFont, FontStyle.Bold);
                index = rtb.Text.IndexOf(word, index) + 1;
            }
        }

        /// <summary>
        /// Appends colored text to the current text of the RichTextBox.
        /// </summary>
        /// <param name="rtb"></param>
        /// <param name="text"></param>
        /// <param name="foregroundColor"></param>
        public static void AppendText(this RichTextBox rtb, string text, Color foregroundColor)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.SelectionColor = foregroundColor;
            rtb.AppendText(text);

            // Sets the color back.
            rtb.SelectionColor = rtb.ForeColor;
        }

        /// <summary>
        /// Appends colored text to the current text of the RichTextBox.
        /// </summary>
        /// <param name="rtb"></param>
        /// <param name="text"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        public static void AppendText(this RichTextBox rtb, string text, Color foregroundColor, Color backgroundColor)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.SelectionColor = foregroundColor;
            rtb.BackColor = backgroundColor;
            rtb.AppendText(text);

            // Sets the color back.
            rtb.SelectionColor = rtb.ForeColor;
        }

        /// <summary>
        /// Appends colored text to the current text of the RichTextBox.
        /// </summary>
        /// <param name="rtb"></param>
        /// <param name="text"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="font"></param>
        public static void AppendText(this RichTextBox rtb, string text, Color foregroundColor, Color backgroundColor, Font font)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.SelectionColor = foregroundColor;
            rtb.BackColor = backgroundColor;
            rtb.SelectionFont = font;
            rtb.AppendText(text);

            // Sets the color back.
            rtb.SelectionColor = rtb.ForeColor;

            // Sometimes required so the next appended text doesn't use the same font passed in here.
            rtb.SelectionFont = rtb.Font;
        }
    }
}