/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-07-25
 * @last updated      : 2019-03-17
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Windows.Forms;

namespace Argus.Windows.Forms.Controls
{
    /// <summary>
    /// Text box that only accepts numeric values.
    /// </summary>
    /// <remarks>
    /// The style ES_NUMBER only allows numeric characters enforced by Windows.  We however have to handle
    /// those characters that are pasted in therefore we will make it require the same criteria.  This does
    /// not handle decimal points.
    /// </remarks>
    public class NumericTextBox : TextBox
    {
        private const int ES_NUMBER = 0x2000;

        protected override CreateParams CreateParams
        {
            get
            {
                var @params = base.CreateParams;
                @params.Style = @params.Style | ES_NUMBER;

                return @params;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Prevent pasting of non-numeric characters
            if (keyData == (Keys.Shift | Keys.Insert) || keyData == (Keys.Control | Keys.V))
            {
                var data = Clipboard.GetDataObject();

                if (data == null)
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }

                string text = Convert.ToString(data.GetData(DataFormats.StringFormat, true));

                if (text == string.Empty)
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }

                foreach (char ch in text)
                {
                    if (!char.IsNumber(ch))
                    {
                        return true;
                    }
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }

            if (keyData == (Keys.Control | Keys.A))
            {
                // Process the select all
                this.SelectAll();

                return base.ProcessCmdKey(ref msg, keyData);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}