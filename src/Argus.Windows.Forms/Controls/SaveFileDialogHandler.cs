using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data;

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
        //*********************************************************************************************************************
        //
        //             Class:  SaveFileDialogHandler
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  01/08/2009
        //      Last Updated:  03/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Initializes with the text to save and the type of encoding to use.  If you don't know what type of encoding to use
        /// then use System.Text.Encoding.ASCII.
        /// </summary>
        /// <param name="textToSave"></param>
        /// <param name="encoding"></param>
        /// <remarks></remarks>
        public SaveFileDialogHandler(string textToSave, Encoding encoding)
        {
            TextToSave = textToSave;
            Encoding = encoding;
        }

        /// <summary>
        /// Initializes with the data table that you want to save the XML data for.
        /// </summary>
        /// <param name="dataTableToSave"></param>
        /// <remarks></remarks>
        public SaveFileDialogHandler(DataTable dataTableToSave)
        {
            DataTableToSave = dataTableToSave;
        }

        /// <summary>
        /// Initializes with the binary data that you want to save.
        /// </summary>
        /// <param name="binaryDataToSave"></param>
        /// <remarks></remarks>
        public SaveFileDialogHandler(byte[] binaryDataToSave)
        {
            BinaryDataToSave = binaryDataToSave;
        }

        /// <summary>
        /// Shows the save dialog and if the user selects a file, saves that file with the data already provided.
        /// </summary>
        /// <remarks></remarks>
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
            if (TextToSave != "")
            {
                File.WriteAllText(sfd.FileName, TextToSave, Encoding.ASCII);
            }
            else if (BinaryDataToSave != null)
            {
                if (BinaryDataToSave.Length > 0)
                {
                    File.WriteAllBytes(sfd.FileName, BinaryDataToSave);
                }
            }
            else if (DataTableToSave != null)
            {
                DataTableToSave.WriteXml(sfd.FileName);
            }
            else
            {
                MessageBox.Show("No save data was provided.  Contact the programmer of this software.", "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Encoding of the file to save.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Encoding Encoding { get; set; } = Encoding.ASCII;

        /// <summary>
        /// Text of the file to save.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string TextToSave { get; set; } = "";

        /// <summary>
        /// Data table to save.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable DataTableToSave { get; set; }

        /// <summary>
        /// Binary data to save.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte[] BinaryDataToSave { get; set; }

        /// <summary>
        /// The full path to the file that was selected.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SelectedFileFullPath { get; set; } = "";

        /// <summary>
        /// Just the file name that was selected without the full path.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SelectedFileName
        {
            get
            {
                return System.IO.Path.GetFileName(this.SelectedFileFullPath);
            }
        }

        /// <summary>
        /// The initial directory that the SafeFileDialog should display when it is invoked.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string InitialDirectory { get; set; } = "";

    }

}