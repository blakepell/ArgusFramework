using System;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Argus.Windows.Forms.CommonForms
{

    /// <summary>
    /// Properties grid form that will save the selected object when the form is closed via the close button.
    /// </summary>
    public partial class Properties : Form
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public Properties()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Force refresh of the property grid.
        /// </summary>
        public void RefreshGrid()
        {
            propertyGrid1.Refresh();            
        }

        /// <summary>
        /// Saves the SelectedObject to the file system is the selected format.
        /// </summary>
        public void Save()
        {
            if (!string.IsNullOrWhiteSpace(SaveToFile))
            {
                try
                {
                    switch (SaveFormat)
                    {
                        case SaveType.Xml:
                        default:
                            string xml = Argus.Utilities.XmlSerialization.ObjectToXml(SelectedObject);
                            System.IO.File.WriteAllText(SaveToFile, xml);
                            break;
                        case SaveType.Json:                            
                            string json = JsonConvert.SerializeObject(SelectedObject, Formatting.Indented);
                            System.IO.File.WriteAllText(SaveToFile, json);
                            break;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "An error has occured.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        /// <summary>
        /// The location of the file to save.
        /// </summary>
        public string SaveToFile { get; set; } = "";


        /// <summary>
        /// The object that the property grid should render.
        /// </summary>
        public object SelectedObject
        {
            get { return propertyGrid1.SelectedObject; }
            set { propertyGrid1.SelectedObject = value; }
        }

        /// <summary>
        /// Close button that will save and then close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Save();
            Close();
        }

        /// <summary>
        /// The save formats available to serialize the saved object as.
        /// </summary>
        public enum SaveType
        {
            Xml = 0,
            Json = 1
        }

        /// <summary>
        /// The format that the object should be serialized as.
        /// </summary>
        public SaveType SaveFormat { get; set; } = SaveType.Json;

    }
}
