/*
 * @author            : Blake Pell
 * @initial date      : 2007-08-28
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System.Data;

namespace Argus.Data
{
    /// <summary>
    ///     This file will take a DataReader that implements IDataReader and export it's contents to a file
    ///     or to a return string.  I will dynamically find out what's in the record set and use the field names
    ///     listed in the record set as the header row (which can be disabled).
    /// </summary>
    /// <remarks>
    ///     StringBuilder is used for speed.  Also, once the DataReader has been iterated through once
    ///     it won't be able to return to the beginning (the DataReader is forward only).  That means if you call
    ///     toString then you'll need to refresh the DataReader before calling the ExportToFile.  If you only want
    ///     to call it once then consider caching the StringBuilder in a property.  Using a DataTable does not have
    ///     the same limitation to refresh.
    /// </remarks>
    public class CreateDelimitedFile
    {
        private string _responseStreamFilename = "";

        /// <summary>
        ///     Constructor:  Use the methods to invoked the options for this class
        /// </summary>
        public CreateDelimitedFile()
        {
        }

        /// <summary>
        ///     Constructor:  Sets the default values for ExportHeader and Delimiter.  The default values for ExportHeader is True and
        ///     the default value for Delimiter is a Tab (ASCII character 9).  If you're using these then you won't need to use this
        ///     constructor overload.
        /// </summary>
        /// <param name="exportHeader"></param>
        /// <param name="deliminator"></param>
        public CreateDelimitedFile(bool exportHeader, string deliminator)
        {
            this.ExportHeader = exportHeader;
            this.Deliminator = deliminator;
        }

        /// <summary>
        ///     Whether or not a header with the field values should be exported also.
        /// </summary>
        public bool ExportHeader { get; set; } = true;

        /// <summary>
        ///     The deliminator to use to split the file up.  The default value is a Tab.
        /// </summary>
        public string Deliminator { get; set; } = "\t";

        /// <summary>
        ///     Whether or not to escape the delimiter for a field so it does not throw off end of record marker.  E.g. changing a tab character into
        ///     a \t in the field so it doesn't end prematurely.  This is false by default
        /// </summary>
        public bool EscapeFields { get; set; } = false;

        /// <summary>
        ///     The escape characters that will be used if the EscapeFields property is true and the set delimiter is found in a field.  This is
        ///     a "\t" by default to correspond with the default delimiter set.
        /// </summary>
        public string EscapeCharacter { get; set; } = "\\t";

        /// <summary>
        ///     The output filename is the name that should be exported with the file minus the extension that will be added by the
        ///     method that is exporting it.  If no filename has been specified for this property then a guid will be produced.  This property
        ///     is ONLY used on the ResponseStream output methods where a file path is not specified.
        /// </summary>
        public string ResponseStreamFilename
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_responseStreamFilename))
                {
                    return Guid.NewGuid().ToString();
                }

                return _responseStreamFilename;
            }
            set => _responseStreamFilename = value;
        }

        /// <summary>
        ///     This overload accepts a Sql Data Reader and a file path to write the file out to.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="filePath"></param>
        public void ExportToFile(IDataReader dr, string filePath)
        {
            File.WriteAllText(filePath, this.GetDelimitedText(dr), Encoding.ASCII);
        }

        /// <summary>
        ///     This overload accepts a data table and a file path to write the file out to.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public void ExportToFile(DataTable dt, string filePath)
        {
            var dr = dt.CreateDataReader();
            this.ExportToFile(dr, filePath);
            dr.Close();
        }

        /// <summary>
        ///     Exports a delimited file to the provided stream.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="s"></param>
        public void ExportToStream(IDataReader dr, Stream s)
        {
            using (var sr = new StreamWriter(s))
            {
                sr.Write(this.ToString(dr));
            }
        }

        /// <summary>
        ///     Exports a delimited file to the provided stream.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="s"></param>
        public void ExportToStream(DataTable dt, Stream s)
        {
            using (var sr = new StreamWriter(s))
            {
                sr.Write(this.ToString(dt));
            }
        }

        /// <summary>
        ///     This overload uses an IDataReader to handle any other reader types that aren't covered here or haven't
        ///     been anticipated.
        /// </summary>
        /// <param name="dr"></param>
        public string ToString(IDataReader dr)
        {
            return this.GetDelimitedText(dr);
        }

        /// <summary>
        ///     This overload uses an DataTable to handle any other reader types that aren't covered here or haven't
        ///     been anticipated.
        /// </summary>
        /// <param name="dt"></param>
        public string ToString(DataTable dt)
        {
            return this.GetDelimitedText(dt);
        }

        /// <summary>
        ///     This overload returns the type as a string, use one of the other overloads with inputs to view data.
        /// </summary>
        public override string ToString()
        {
            return this.GetType().ToString();
        }

        /// <summary>
        ///     Returns the text for the delimited file.
        /// </summary>
        /// <param name="dr"></param>
        /// <remarks>
        ///     This function uses a StringBuilder for drastic performance improvements.  A String is immutable in .Net and therefore a new String
        ///     must be created every time that a concatenation is done.  A StringBuilder on the other hand only allocates more memory when it's buffer
        ///     is full (then it will double in size).  For performance, never call StringBuilder.ToString in a large loop otherwise it will allocate
        ///     memory every iteration and great degrade performance.
        /// </remarks>
        private string GetDelimitedText(IDataReader dr)
        {
            var sb = new StringBuilder();

            if (this.ExportHeader)
            {
                for (int x = 0; x <= dr.FieldCount - 1; x++)
                {
                    if (x == dr.FieldCount - 1)
                    {
                        // Last Record, No Delimiter, but end the record
                        sb.AppendFormat("{0}{1}", dr.GetName(x), Environment.NewLine);
                    }
                    else
                    {
                        sb.AppendFormat("{0}{1}", dr.GetName(x), this.Deliminator);
                    }
                }
            }

            if (this.EscapeFields)
            {
                while (dr.Read())
                {
                    for (int x = 0; x <= dr.FieldCount - 1; x++)
                    {
                        if (x == dr.FieldCount - 1)
                        {
                            // Last Record, No Delimiter, but end the record
                            if (dr[x].ToString().Contains(this.Deliminator))
                            {
                                sb.AppendFormat("{0}{1}", dr[x].ToString().Replace(this.Deliminator, this.EscapeCharacter), Environment.NewLine);
                            }
                            else
                            {
                                sb.AppendFormat("{0}{1}", dr[x], Environment.NewLine);
                            }
                        }
                        else
                        {
                            if (dr[x].ToString().Contains(this.Deliminator))
                            {
                                sb.AppendFormat("{0}{1}", dr[x].ToString().Replace(this.Deliminator, this.EscapeCharacter), this.Deliminator);
                            }
                            else
                            {
                                sb.AppendFormat("{0}{1}", dr[x], this.Deliminator);
                            }
                        }
                    }
                }
            }
            else
            {
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
                                sb.AppendFormat("{0}{1}", dr[x], Environment.NewLine);
                            }
                            else
                            {
                                sb.AppendFormat("{0}", Environment.NewLine);
                            }
                        }
                        else
                        {
                            if (dr[x] != null)
                            {
                                sb.AppendFormat("{0}{1}", dr[x], this.Deliminator);
                            }
                            else
                            {
                                sb.AppendFormat("{0}", this.Deliminator);
                            }
                        }
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Returns the text for the delimited file.
        /// </summary>
        /// <param name="dt"></param>
        private string GetDelimitedText(DataTable dt)
        {
            IDataReader dr = dt.CreateDataReader();

            return this.GetDelimitedText(dr);
        }
    }
}