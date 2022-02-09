/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-01-08
 * @last updated      : 2019-03-17
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Argus.Windows.Forms.Controls
{
    /// <summary>
    /// Basic save dialog handling.  This will allow you to save a string to a file or a data table to a file.  The user will be
    /// prompted with a dialog box as to where to save that file.  This is only for WinForms or WPF applications.  This can save
    /// text files or a DataTable object.
    /// </summary>
    /// <remarks>
    /// <code>
    /// string buf = "Test Text";
    /// var SaveFileDialogHandler sd = new SaveFileDialogHandler(buf, System.Text.Encoding.ASCII);
    /// sd.Save();
    /// </code>
    /// </remarks>
    public class SaveFileDialogHandler
    {
        /// <summary>
        /// Initializes with the text to save and the type of encoding to use.  If you don't know what type of encoding to use
        /// then use System.Text.Encoding.ASCII.
        /// </summary>
        /// <param name="textToSave"></param>
        /// <param name="encoding"></param>
        public SaveFileDialogHandler(string textToSave, Encoding encoding)
        {
            this.TextToSave = textToSave;
            this.Encoding = encoding;
        }

        /// <summary>
        /// Initializes with the data table that you want to save the XML data for.
        /// </summary>
        /// <param name="dataTableToSave"></param>
        public SaveFileDialogHandler(DataTable dataTableToSave)
        {
            this.DataTableToSave = dataTableToSave;
        }

        /// <summary>
        /// Initializes with the binary data that you want to save.
        /// </summary>
        /// <param name="binaryDataToSave"></param>
        public SaveFileDialogHandler(byte[] binaryDataToSave)
        {
            this.BinaryDataToSave = binaryDataToSave;
        }

        /// <summary>
        /// Encoding of the file to save.
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.ASCII;

        /// <summary>
        /// Text of the file to save.
        /// </summary>
        public string TextToSave { get; set; } = "";

        /// <summary>
        /// Data table to save.
        /// </summary>
        public DataTable DataTableToSave { get; set; }

        /// <summary>
        /// Binary data to save.
        /// </summary>
        public byte[] BinaryDataToSave { get; set; }

        /// <summary>
        /// The full path to the file that was selected.
        /// </summary>
        public string SelectedFileFullPath { get; set; } = "";

        /// <summary>
        /// Just the file name that was selected without the full path.
        /// </summary>
        public string SelectedFileName => Path.GetFileName(this.SelectedFileFullPath);

        /// <summary>
        /// The initial directory that the SafeFileDialog should display when it is invoked.
        /// </summary>
        public string InitialDirectory { get; set; } = "";

        /// <summary>
        /// Shows the save dialog and if the user selects a file, saves that file with the data already provided.
        /// </summary>
        public void Save()
        {
            var sfd = new SaveFileDialog
            {
                Title = "Save File",
                ValidateNames = true,
                AutoUpgradeEnabled = true,
                InitialDirectory = this.InitialDirectory
            };

            sfd.ShowDialog();

            if (string.IsNullOrWhiteSpace(sfd.FileName))
            {
                return;
            }

            this.SelectedFileFullPath = sfd.FileName;

            // 10 years later I'm not sure I like this implementation.  There could be a valid reason to save a blank text file.
            if (this.TextToSave != "")
            {
                File.WriteAllText(sfd.FileName, this.TextToSave, Encoding.ASCII);
            }
            else if (this.BinaryDataToSave != null)
            {
                if (this.BinaryDataToSave.Length > 0)
                {
                    File.WriteAllBytes(sfd.FileName, this.BinaryDataToSave);
                }
            }
            else if (this.DataTableToSave != null)
            {
                this.DataTableToSave.WriteXml(sfd.FileName);
            }
            else
            {
                MessageBox.Show("No save data was provided.  Contact the programmer of this software.", "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}