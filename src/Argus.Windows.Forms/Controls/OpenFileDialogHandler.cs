using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Argus.Windows.Forms.Controls
{
    /// <summary>
    ///     Basic file opening dialog handling.  This will allow you to prompt the user with a dialog box and then return the file they
    ///     select into an variable of Object type.  The class knows what type is in the Object because the programmer will specify it
    ///     via the FileType property.  Supported types are Text, Binary and DataTable.  The binary type is read in as a byte array.
    /// </summary>
    /// <remarks>
    ///     <code>
    /// 
    /// ' This will open a file that we expect to be handled as text.
    /// var od = new OpenFileDialogHandler(OpenFileDialogHandler.FileTypes.Text);
    /// od.Open();
    /// 
    /// ' Since we expect text, type it as a string.. for a binary such as a picture you could type as such.
    /// string buf = (string)od.OpenedObject;
    /// 
    /// </code>
    /// </remarks>
    public class OpenFileDialogHandler
    {
        //*********************************************************************************************************************
        //
        //             Class:  OpenFileDialogHandler
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  01/08/2009
        //      Last Updated:  03/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Types of files that are supported.
        /// </summary>
        public enum FileTypes
        {
            Text,
            Binary,
            DataTable
        }

        /// <summary>
        ///     A list of the file types that are supported.  E.g. "*.txt", "*.txt;*.sql", etc.
        /// </summary>
        public List<string> FileTypeFilterList = new List<string>();

        /// <summary>
        ///     Initializes, you have to tell it the type of file to open here.  Use the 'Open' procedure to handle the opening and the dialog.
        /// </summary>
        /// <param name="fileType"></param>
        public OpenFileDialogHandler(FileTypes fileType)
        {
            this.FileType = fileType;
        }

        /// <summary>
        ///     The type of the file that you're going to open.
        /// </summary>
        public FileTypes FileType { get; set; }

        /// <summary>
        ///     An object that contains the data that's opened.  We've used object so that many different types of data can be put
        ///     into it.  Since you know what you're reading in, you can then cast it.  If the user has specified an invalid file, it
        ///     may have already failed by this point at which point you would handle the exception there.
        /// </summary>
        public object OpenedObject { get; set; }

        /// <summary>
        ///     The full path to the file that was selected.
        /// </summary>
        public string SelectedFileFullPath { get; set; } = "";

        /// <summary>
        ///     Just the file name that was selected without the full path.
        /// </summary>
        public string SelectedFileName => Path.GetFileName(this.SelectedFileFullPath);

        /// <summary>
        ///     The initial directory that the dialog should default to whenever it is invoked.
        /// </summary>
        public string InitialDirectory { get; set; } = "";

        /// <summary>
        ///     Displays an open dialog box and reads in the data in type you specified in the constructor.  It will put the data of the file
        ///     the user selects in the 'OpenedObject' property.  You can simply cast that object to the property type you need it in.
        /// </summary>
        public void Open()
        {
            var ofd = new OpenFileDialog
            {
                Title = "Open File",
                ValidateNames = true,
                AutoUpgradeEnabled = true,
                Multiselect = false,
                Filter = this.FilterList(),
                InitialDirectory = this.InitialDirectory
            };

            ofd.ShowDialog();

            if (string.IsNullOrWhiteSpace(ofd.FileName))
            {
                return;
            }


            this.SelectedFileFullPath = ofd.FileName;

            switch (this.FileType)
            {
                case FileTypes.Text:
                {
                    string buf = File.ReadAllText(ofd.FileName);
                    this.OpenedObject = buf;

                    break;
                }

                case FileTypes.Binary:
                {
                    var buf = File.ReadAllBytes(ofd.FileName);
                    this.OpenedObject = buf;

                    break;
                }

                case FileTypes.DataTable:
                {
                    var dt = new DataTable();
                    dt.ReadXml(ofd.FileName);
                    this.OpenedObject = dt;

                    break;
                }
            }
        }

        /// <summary>
        ///     Returns a formatted list that can be used with the OpenFileDialog made up from values in the FileTypeList.
        /// </summary>
        private string FilterList()
        {
            var sb = new StringBuilder();

            foreach (string buf in FileTypeFilterList)
            {
                sb.AppendFormat("{0}|{0}|", buf);
            }

            return sb.ToString().Trim('|');
        }
    }
}