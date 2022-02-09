/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-12-13
 * @last updated      : 2019-03-13
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Text;
using System.Windows.Forms;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="System.Windows.Forms.WebBrowser" />.
    /// </summary>
    public static class WebBrowserExtensions
    {
        /// <summary>
        /// Sets a specified element on all frames
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public static void SetElement(this WebBrowser wb, string id, string value)
        {
            if (wb.Document == null)
            {
                return;
            }

            if (wb.Document.Window.Frames.Count > 0)
            {
                foreach (HtmlWindow hw in wb.Document.Window.Frames)
                {
                    hw.Document.GetElementById(id).SetAttribute("value", value);
                }
            }
            else
            {
                wb.Document.Window.Document.GetElementById(id).SetAttribute("value", value);
            }

            Application.DoEvents();
        }

        /// <summary>
        /// Clicks the HTML elements.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="wb"></param>
        public static void ClickElement(this WebBrowser wb, string id)
        {
            if (wb.Document == null)
            {
                return;
            }

            if (wb.Document.Window.Frames.Count > 0)
            {
                foreach (HtmlWindow hw in wb.Document.Window.Frames)
                {
                    hw.Document.GetElementById(id).InvokeMember("click");
                }
            }
            else
            {
                wb.Document.Window.Document.GetElementById(id).InvokeMember("click");
            }

            Application.DoEvents();
        }

        /// <summary>
        /// Sets an HTML element's Inner Text property
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="id"></param>
        /// <param name="innerText"></param>
        public static void SetElementInnerText(this WebBrowser wb, string id, string innerText)
        {
            if (wb.Document == null)
            {
                return;
            }

            if (wb.Document.Window.Frames.Count > 0)
            {
                foreach (HtmlWindow hw in wb.Document.Window.Frames)
                {
                    hw.Document.GetElementById(id).InnerText = innerText;
                }
            }
            else
            {
                wb.Document.Window.Document.GetElementById(id).InnerText = innerText;
            }

            Application.DoEvents();
        }

        /// <summary>
        /// Returns all HTML for inside all body tags in all frames of a WebBrowser control.  This is for particular use when parsing for
        /// text.
        /// </summary>
        /// <param name="wb"></param>
        public static string AllFramesBodyText(this WebBrowser wb)
        {
            var sb = new StringBuilder();

            if (wb.Document.Window.Frames.Count > 0)
            {
                foreach (HtmlWindow win in wb.Document.Window.Frames)
                {
                    sb.Append(win.Document.Body.OuterHtml);
                }

                return sb.ToString();
            }

            return wb.Document.Body.OuterHtml;
        }

        /// <summary>
        /// Cuts the current selected text from the HtmlDocument and puts it on the clipboard.
        /// </summary>
        /// <param name="wb"></param>
        public static void Cut(this WebBrowser wb)
        {
            wb.Document.ExecCommand("Cut", false, "");
        }

        /// <summary>
        /// Copys the current selected text from the HtmlDocument and puts it on the clipboard.
        /// </summary>
        /// <param name="wb"></param>
        public static void Copy(this WebBrowser wb)
        {
            wb.Document.ExecCommand("Copy", false, "");
        }

        /// <summary>
        /// Deletes the current selected text from the HtmlDocument.
        /// </summary>
        /// <param name="wb"></param>
        public static void Delete(this WebBrowser wb)
        {
            wb.Document.ExecCommand("Delete", false, "");
        }

        /// <summary>
        /// Pastes the contents of the clipboard into the current HtmlDocument
        /// </summary>
        /// <param name="wb"></param>
        public static void Paste(this WebBrowser wb)
        {
            wb.Document.ExecCommand("Paste", false, "");
        }

        /// <summary>
        /// Invokes the SaveAs dialog.
        /// </summary>
        /// <param name="wb"></param>
        public static void SaveAs(this WebBrowser wb)
        {
            wb.Document.ExecCommand("SaveAs", true, "");
        }

        /// <summary>
        /// Selects all text on the current HtmlDocument.
        /// </summary>
        /// <param name="wb"></param>
        public static void SelectAll(this WebBrowser wb)
        {
            wb.Document.ExecCommand("SelectAll", false, "");
        }

        /// <summary>
        /// Undo the previous command.
        /// </summary>
        /// <param name="wb"></param>
        public static void Undo(this WebBrowser wb)
        {
            wb.Document.ExecCommand("Undo", false, "");
        }

        /// <summary>
        /// Removes any selection on the HtmlDocument
        /// </summary>
        /// <param name="wb"></param>
        /// <remarks>
        /// Runs the ExecCommand("Unselect")
        /// </remarks>
        public static void Deselect(this WebBrowser wb)
        {
            wb.Document.ExecCommand("Unselect", false, "");
        }

        /// <summary>
        /// Opens the print dialog for the current document.
        /// </summary>
        /// <param name="wb"></param>
        public static void Print(this WebBrowser wb)
        {
            wb.Document.ExecCommand("Print", true, "");
        }

        /// <summary>
        /// Sets the progress bar value, without using Windows Aero animation.
        /// </summary>
        public static void SetProgressNoAnimation(this ProgressBar pb, int value)
        {
            // To get around this animation, we need to move the progress bar backwards.
            if (value == pb.Maximum)
            {
                // Special case (can't set value > Maximum).
                // Set the value
                pb.Value = value;
                // Move it backwards
                pb.Value = value - 1;
            }
            else
            {
                // Move past
                pb.Value = value + 1;
            }

            // Move to correct value
            pb.Value = value;
        }
    }
}