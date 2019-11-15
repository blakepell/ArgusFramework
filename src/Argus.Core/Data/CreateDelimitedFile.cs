using System;
using System.Data;
using System.Text;

namespace Argus.Data
{
    /// <summary>
    /// This file will take a DataReader that implements IDataReader and export it's contents to a file 
    /// or to a return string.  I will dynamically find out what's in the recordset and use the field names
    /// listed in the recordset as the header row (which can be disabled).
    /// </summary>
    /// <remarks>StringBuilder is used for speed.  Also, once the DataReader has been iteriated through once
    /// it won't be able to return to the beginning (the DataReader is forward only).  That means if you call
    /// toString then you'll need to refresh the DataReader before calling the ExportToFile.  If you only want
    /// to call it once then consider caching the StringBuilder in a property.  Using a DataTable does not have
    /// the same limitation to refresh.
    /// </remarks>
    public class CreateDelimitedFile
    {
        //*********************************************************************************************************************
        //
        //             Class:  CreateDelimitedFile
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  08/28/2007
        //      Last Updated:  02/09/2018
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Constructor:  Use the methods to invoked the options for this class
        /// </summary>
        /// <remarks></remarks>

        public CreateDelimitedFile()
        {
        }

        /// <summary>
        /// Constructor:  Sets the default values for ExportHeader and Deliminator.  The default values for ExportHeader is True and
        /// the default value for Delimiator is a Tab (ascii character 9).  If you're using these then you won't need to use this
        /// constructor overload.
        /// </summary>
        /// <param name="exportHeader"></param>
        /// <param name="deliminator"></param>
        /// <remarks></remarks>
        public CreateDelimitedFile(bool exportHeader, string deliminator)
        {
            this.ExportHeader = exportHeader;
            this.Deliminator = deliminator;
        }

        /// <summary>
        /// This overload accepts a Sql Data Reader and a file path to write the file out to.
        /// </summary>
        /// <param name="Dr"></param>
        /// <param name="FilePath"></param>
        /// <remarks></remarks>
        public void ExportToFile(IDataReader dr, string filePath)
        {
            System.IO.File.WriteAllText(filePath, this.GetDelimatedText(dr), System.Text.Encoding.ASCII);
        }

        /// <summary>
        /// This overload accepts a data table and a file path to write the file out to.
        /// </summary>
        /// <param name="Dt"></param>
        /// <param name="FilePath"></param>
        /// <remarks></remarks>
        public void ExportToFile(DataTable dt, string filePath)
        {
            DataTableReader dr = dt.CreateDataReader();
            ExportToFile(dr, filePath);
            dr.Close();
            dr = null;
        }

        /// <summary>
        /// Exports a delimited file to the provided stream.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="s"></param>
        /// <remarks></remarks>
        public void ExportToStream(IDataReader dr, System.IO.Stream s)
        {
            using (System.IO.StreamWriter sr = new System.IO.StreamWriter(s))
            {
                sr.Write(this.ToString(dr));
            }
        }

        /// <summary>
        /// Exports a delimited file to the provided stream.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="s"></param>
        /// <remarks></remarks>
        public void ExportToStream(DataTable dt, System.IO.Stream s)
        {
            using (System.IO.StreamWriter sr = new System.IO.StreamWriter(s))
            {
                sr.Write(this.ToString(dt));
            }
        }

        /// <summary>
        /// This overload uses an IDataReader to handle any other reader types that aren't covered here or haven't
        /// been anticipated.
        /// </summary>
        /// <param name="Dr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ToString(IDataReader dr)
        {
            return this.GetDelimatedText(dr);
        }

        /// <summary>
        /// This overload uses an DataTable to handle any other reader types that aren't covered here or haven't
        /// been anticipated.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ToString(DataTable dt)
        {
            return this.GetDelimatedText(dt);
        }

        /// <summary>
        /// This overload returns the type as a string, use one of the other overloads with inputs to view data.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.GetType().ToString();
        }

        /// <summary>
        /// Returns the text for the delimated file.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        /// <remarks>
        /// This function uses a StringBuilder for drastic performance improvements.  A String is immutable in .Net and therefore a new String
        /// must be created everytime that a concatenation is done.  A StringBuilder on the other hand only allocates more memory when it's buffer
        /// is full (then it will double in size).  For performance, never call StringBuilder.ToString in a large loop otherwise it will allocate
        /// memory every iteration and great degrade performance.
        /// </remarks>
        private string GetDelimatedText(IDataReader dr)
        {
            StringBuilder sb = new StringBuilder();

            if (_exportHeader == true)
            {
                for (int x = 0; x <= dr.FieldCount - 1; x++)
                {
                    if (x == dr.FieldCount - 1)
                    {
                        // Last Record, No Delimiter, but end the record
                        sb.AppendFormat("{0}{1}", dr.GetName(x).ToString(), Environment.NewLine);
                    }
                    else
                    {
                        sb.AppendFormat("{0}{1}", dr.GetName(x).ToString(), _deliminator);
                    }
                }
            }

            if (this.EscapeFields == true)
            {
                while (dr.Read())
                {
                    for (int x = 0; x <= dr.FieldCount - 1; x++)
                    {
                        if (x == dr.FieldCount - 1)
                        {
                            // Last Record, No Delimiter, but end the record
                            if (dr[x].ToString().Contains(_deliminator))
                            {
                                sb.AppendFormat("{0}{1}", dr[x].ToString().Replace(_deliminator, this.EscapeCharacter), Environment.NewLine);
                            }
                            else
                            {
                                sb.AppendFormat("{0}{1}", dr[x].ToString(), Environment.NewLine);
                            }
                        }
                        else {
                            if (dr[x].ToString().Contains(_deliminator))
                            {
                                sb.AppendFormat("{0}{1}", dr[x].ToString().Replace(_deliminator, this.EscapeCharacter), _deliminator);
                            }
                            else
                            {
                                sb.AppendFormat("{0}{1}", dr[x].ToString(), _deliminator);
                            }
                        }
                    }
                }
            }
            else {
                // Duplicated code, but we won't have to do the boolean check every iteration of the loop this way.
                while (dr.Read())
                {
                    for (int x = 0; x <= dr.FieldCount - 1; x++)
                    {
                        if (x == dr.FieldCount - 1)
                        {
                            // Last Record, No Delimiter, but end the record
                            if (dr[x] != null)
                            {
                                sb.AppendFormat("{0}{1}", dr[x].ToString(), Environment.NewLine);
                            }
                            else {
                                sb.AppendFormat("{0}", Environment.NewLine);
                            }

                        }
                        else {
                            if (dr[x] != null)
                            {
                                sb.AppendFormat("{0}{1}", dr[x].ToString(), _deliminator);
                            }
                            else
                            {
                                sb.AppendFormat("{0}", _deliminator);
                            }
                        }
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the text for the delimated file.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string GetDelimatedText(DataTable dt)
        {
            IDataReader dr = dt.CreateDataReader();
            return this.GetDelimatedText(dr);
        }

        private bool _exportHeader = true;
        /// <summary>
        /// Whether or not a header with the field values should be exported also.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ExportHeader
        {
            get { return _exportHeader; }
            set { _exportHeader = value; }
        }

        private string _deliminator = "\t";
        /// <summary>
        /// The deliminator to use to split the file up.  The default value is a Tab.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Deliminator
        {
            get { return _deliminator; }
            set { _deliminator = value; }
        }

        private bool _escapeFields = false;
        /// <summary>
        /// Whether or not to escape the delimter for a field so it does not throw off end of record marker.  E.g. changing a tab character into
        /// a \t in the field so it doesn't end prematurely.  This is false by default
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool EscapeFields
        {
            get { return _escapeFields; }
            set { _escapeFields = value; }
        }

        private string _escapeCharacters = "\\t";
        /// <summary>
        /// The escape characters that will be used if the EscapeFields property is true and the set delimiter is found in a field.  This is
        /// a "\t" by default to correspond with the default delimiter set.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string EscapeCharacter
        {
            get { return _escapeCharacters; }
            set { _escapeCharacters = value; }
        }

        private string _responseStreamFilename = "";
        /// <summary>
        /// The output filename is the name that should be exported with the file minus the extension that will be added by the
        /// method that is exporting it.  If no filename has been specified for this property then a guid will be produced.  This property
        /// is ONLY used on the ResponseStream output methods where a file path is not specified.  
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ResponseStreamFilename
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_responseStreamFilename) == true)
                {
                    return Guid.NewGuid().ToString();
                }
                else
                {
                    return _responseStreamFilename;
                }
            }
            set { _responseStreamFilename = value; }
        }

    }

}