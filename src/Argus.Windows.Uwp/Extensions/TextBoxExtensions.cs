using System.ComponentModel;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Argus.Windows.Uwp.Models;

namespace Argus.Extensions
{
    /// <summary>
    ///     TextBox extension methods.
    /// </summary>
    public static class TextBoxExtensions
    {
        /// <summary>
        ///     Inserts text into the TextBox at the current selection point.
        /// </summary>
        /// <param name="text">The text to insert into the TextBox.</param>
        /// <param name="setFocus">Whether the TextBox should be set with the focus after the insertion.</param>
        public static void InsertText(this TextBox tb, string text, bool setFocus)
        {
            tb.SelectedText = text;

            if (setFocus)
            {
                tb.Focus();
            }
        }

        /// <summary>
        ///     Removes the empty lines in a text box.
        /// </summary>
        /// <param name="tb"></param>
        public static void RemoveEmptyLines(this TextBox tb)
        {
            RemoveEmptyLines(tb, true);
        }

        /// <summary>
        ///     Removes the empty lines in a text box.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="treatWhiteSpaceAsEmpty">Whether or not to remove lines that only have whitespace.</param>
        public static void RemoveEmptyLines(this TextBox tb, bool treatWhiteSpaceAsEmpty)
        {
            var lines = tb.Text.Split('\r');
            var sb = new StringBuilder();

            foreach (string item in lines)
            {
                if (treatWhiteSpaceAsEmpty)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        sb.AppendFormat("{0}\r", item);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        sb.AppendFormat("{0}\r", item);
                    }
                }
            }

            tb.Text = sb.ToString().TrimEnd('\r');
        }

        /// <summary>
        ///     Goes to the specified line number.
        /// </summary>
        /// <param name="lineNumber"></param>
        public static void GotoLine(this TextBox tb, int lineNumber)
        {
            var lines = tb.Text.Split('\r');
            int lineCount = lines.Count();
            int position = 0;

            if (lineNumber > lineCount)
            {
                lineNumber = lineCount;
            }

            // Switch the user input to a 0 based index.
            lineNumber -= 1;

            for (int i = 0; i < lineNumber; i++)
            {
                if (lineNumber > lineCount - 1)
                {
                    break;
                }

                position += lines[i].Length + 1;
            }

            tb.Select(position, 0);
            tb.Focus();
        }

        /// <summary>
        ///     Sorts the lines in a list alphabetically in either ascending or descending order.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="order"></param>
        public static void SortLines(this TextBox tb, ListSortDirection order)
        {
            var sb = new StringBuilder();
            var lines = tb.Text.Split('\r');
            var list = lines.ToList();

            list.Sort(order);

            foreach (string item in list)
            {
                sb.AppendFormat("{0}\r", item);
            }

            tb.Text = sb.ToString().TrimEnd('\r');
        }

        /// <summary>
        ///     Returns the number of lines in the text box.
        /// </summary>
        /// <remarks>This performed better for me than iterating over the characters.</remarks>
        public static int LineCount(this TextBox tb)
        {
            var lines = tb.Text.Split('\r');

            return lines.Count();
        }

        /// <summary>
        ///     Returns the current column position on the current line the cursor is on.
        /// </summary>
        public static CursorPosition CursorPosition(this TextBox tb)
        {
            int endMarker = tb.SelectionStart;

            if (endMarker == 0)
            {
                return new CursorPosition(1, 1, 1);
            }

            int i = 0;
            int col = 1;
            int row = 1;

            foreach (char c in tb.Text)
            {
                i++;
                col++;

                if (c == '\r')
                {
                    row++;
                    col = 1;
                }

                if (i == endMarker)
                {
                    return new CursorPosition(row, col, endMarker + 1);
                }
            }

            return new CursorPosition(row, col, endMarker + 1);
        }

        /// <summary>
        ///     Appends text to each line in the document.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="appendText"></param>
        public static void AppendToEachLine(this TextBox tb, string appendText)
        {
            if (string.IsNullOrWhiteSpace(appendText))
            {
                return;
            }

            var lines = tb.Text.Split('\r');
            var sb = new StringBuilder();

            foreach (string item in lines)
            {
                sb.AppendFormat("{0}{1}\r", item, appendText);
            }

            tb.Text = sb.ToString().TrimEnd('\r');
        }

        /// <summary>
        ///     Prepends text to each line in the document.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="prependText"></param>
        public static void PrependToEachLine(this TextBox tb, string prependText)
        {
            if (string.IsNullOrWhiteSpace(prependText))
            {
                return;
            }

            var lines = tb.Text.Split('\r');
            var sb = new StringBuilder();

            foreach (string item in lines)
            {
                sb.AppendFormat("{0}{1}\r", prependText, item);
            }

            tb.Text = sb.ToString().TrimEnd('\r');
        }

        /// <summary>
        ///     Scrolls to the end of the TextBox.
        /// </summary>
        public static void ScrollToEnd(this TextBox tb, bool setFocus)
        {
            var grid = (Grid) VisualTreeHelper.GetChild(tb, 0);

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
            {
                object obj = VisualTreeHelper.GetChild(grid, i);

                if (!(obj is ScrollViewer))
                {
                    continue;
                }

                ((ScrollViewer) obj).ChangeView(0.0f, ((ScrollViewer) obj).ExtentHeight, 1.0f, true);

                break;
            }

            // Set the caret to the last position in the Text property.
            tb.Select(tb.Text.Length, 0);

            if (setFocus)
            {
                tb.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        ///     Scrolls to the start of the TextBox.
        /// </summary>
        public static void ScrollToStart(this TextBox tb, bool setFocus)
        {
            var grid = (Grid) VisualTreeHelper.GetChild(tb, 0);

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
            {
                object obj = VisualTreeHelper.GetChild(grid, i);

                if (!(obj is ScrollViewer))
                {
                    continue;
                }

                ((ScrollViewer) obj).ChangeView(0.0f, 0, 1.0f, true);

                break;
            }

            // Set the caret to the first position in the Text property.
            tb.Select(0, 0);

            if (setFocus)
            {
                tb.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        ///     Scrolls to the caret in the TextBox.
        /// </summary>
        /// <param name="tb"></param>
        public static void ScrollToCaret(this TextBox tb, bool setFocus)
        {
            var grid = (Grid) VisualTreeHelper.GetChild(tb, 0);

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
            {
                object obj = VisualTreeHelper.GetChild(grid, i);

                if (!(obj is ScrollViewer))
                {
                    continue;
                }

                // So, this wonky.  In older versions of UWP line endings were \r\n in the TextBox, but in newer
                // versions it's just \r and in future versions it will be settable.  So, ya know, just know that.
                // In the future version where it's settable it's supposed to be a property and if it is the '\r'
                // will change to whatever that property name is and then all will be right in the world.
                int lineCount = tb.Text.Split('\r').Count();

                var cp = CursorPosition(tb);
                double percent = ((double) cp.Row - 1) / lineCount;

                double verticalHeight = ((ScrollViewer) obj).ExtentHeight * percent;

                ((ScrollViewer) obj).ChangeView(0.0f, verticalHeight, 1.0f, true);

                break;
            }

            if (setFocus)
            {
                tb.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        ///     Clears any selected text in the TextBox.
        /// </summary>
        /// <param name="tb"></param>
        public static void ClearSelection(this TextBox tb)
        {
            tb.SelectionLength = 0;
        }

        /// <summary>
        ///     Sets the cursor to the end of the TextBox.
        /// </summary>
        /// <param name="tb"></param>
        public static void CaretToEnd(this TextBox tb)
        {
            tb.Select(tb.Text.Length, 0);
        }
    }
}