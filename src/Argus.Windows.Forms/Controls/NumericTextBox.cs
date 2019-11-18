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

        //*********************************************************************************************************************
        //
        //             Class:  NumericTextBox
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  07/25/2009
        //      Last Updated:  03/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

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
                else
                {
                    string text = Convert.ToString(data.GetData(DataFormats.StringFormat, true));

                    if (text == string.Empty)
                    {
                        return base.ProcessCmdKey(ref msg, keyData);
                    }
                    else
                    {
                        foreach (char ch in text.ToCharArray())
                        {
                            if (!char.IsNumber(ch))
                            {
                                return true;
                            }
                        }

                        return base.ProcessCmdKey(ref msg, keyData);
                    }
                }
            }
            else if (keyData == (Keys.Control | Keys.A))
            {
                // Process the select all
                this.SelectAll();
                return base.ProcessCmdKey(ref msg, keyData);
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

        }

    }
}
