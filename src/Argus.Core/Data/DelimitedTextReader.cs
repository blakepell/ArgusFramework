using System;
using System.Data;
using System.IO;
using System.Text;

namespace Argus.Data
{
    /// <summary>
    ///     This class will read delimited text either that is provided or from a file on the file system and render that
    ///     data through the IDataReader interface.  Optionally, CreateDataTable and ExecuteReader methods are included for backwards
    ///     compatibility.
    /// </summary>
    public class DelimitedTextReader : IDataReader
    {
        //*********************************************************************************************************************
        //
        //             Class:  DelimitedTextReader
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  05/24/2010
        //      Last Updated:  11/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        // To detect redundant calls
        private bool _disposedValue;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="text">The delimited text to parse</param>
        /// <param name="delimiter">The character or characters that come between each record</param>
        /// <param name="containsHeaderRow">Whether or not the text contains a header row.</param>
        public DelimitedTextReader(string text, string delimiter, bool containsHeaderRow)
        {
            this.Text = text;
            this.Delimiter = delimiter;
            this.ContainsHeaderRow = containsHeaderRow;
            this.DataTable = this.CreateDataTable();
            this.DataReader = this.DataTable.CreateDataReader();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="text">The delimited text to parse</param>
        /// <param name="delimiter">The character or characters that come between each record</param>
        public DelimitedTextReader(string text, string delimiter)
        {
            this.Text = text;
            this.Delimiter = delimiter;
            this.DataTable = this.CreateDataTable();
            this.DataReader = this.DataTable.CreateDataReader();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="text">The delimited text to parse</param>
        /// <param name="delimiter">The character or characters that come between each record</param>
        /// <param name="lineTerminator">The line terminator to use.</param>
        public DelimitedTextReader(string text, string delimiter, string lineTerminator)
        {
            this.Text = text;
            this.Delimiter = delimiter;
            this.LineTerminator = lineTerminator;
            this.DataTable = this.CreateDataTable();
            this.DataReader = this.DataTable.CreateDataReader();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="text">The delimited text to parse</param>
        public DelimitedTextReader(string text)
        {
            this.Text = text;
            this.DataTable = this.CreateDataTable();
            this.DataReader = this.DataTable.CreateDataReader();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="localFilePath">The path to the file on the local file system or mapped drive.</param>
        /// <param name="enc">The encoding to use when reading the file.</param>
        /// <param name="delimiter">The character or characters that come between each record</param>
        /// <param name="containsHeaderRow">Whether or not the text contains a header row.</param>
        public DelimitedTextReader(string localFilePath, Encoding enc, string delimiter, bool containsHeaderRow) : this(File.ReadAllText(localFilePath, enc), delimiter, containsHeaderRow)
        {
        }

        /// <summary>
        ///     The delimited text that should be parsed.
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        ///     This indicates whether the file contains a header row or not.  If it does the class methods will put the first rows
        ///     contents as the column headings.  Otherwise, the first row's contents will be part of the data set.
        /// </summary>
        /// <remarks>The default value is false.</remarks>
        public bool ContainsHeaderRow { get; set; }

        /// <summary>
        ///     The delimiter that the file is split up by.
        /// </summary>
        /// <remarks>The default delimiter is a tab.</remarks>
        public string Delimiter { get; set; } = "\t";

        /// <summary>
        ///     The line terminator that is used to split records.  The default is carriage return and line feed (ascii 13 and 10).
        /// </summary>
        public string LineTerminator { get; set; } = Environment.NewLine;

        private IDataReader DataReader { get; set; }
        private DataTable DataTable { get; }

        /// <summary>
        ///     Closes the DataReader
        /// </summary>
        public void Close()
        {
            this.DataReader.Close();
        }

        public int Depth => this.DataReader.Depth;

        public DataTable GetSchemaTable()
        {
            return this.DataReader.GetSchemaTable();
        }

        public bool IsClosed => this.DataReader.IsClosed;

        public bool NextResult()
        {
            return this.DataReader.NextResult();
        }

        public bool Read()
        {
            return this.DataReader.Read();
        }

        public int RecordsAffected => this.DataReader.RecordsAffected;
        public int FieldCount => this.DataReader.FieldCount;

        public bool GetBoolean(int i)
        {
            return this.DataReader.GetBoolean(i);
        }

        public byte GetByte(int i)
        {
            return this.DataReader.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return this.DataReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return this.DataReader.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return this.DataReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            return this.DataReader.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            return this.DataReader.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            return this.DataReader.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            return this.DataReader.GetDouble(i);
        }

        public Type GetFieldType(int i)
        {
            return this.DataReader.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            return this.DataReader.GetFloat(i);
        }

        public Guid GetGuid(int i)
        {
            return this.DataReader.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            return this.DataReader.GetInt16(i);
        }

        public int GetInt32(int i)
        {
            return this.DataReader.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            return this.DataReader.GetInt64(i);
        }

        public string GetName(int i)
        {
            return this.DataReader.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return this.DataReader.GetOrdinal(name);
        }

        public string GetString(int i)
        {
            return this.DataReader.GetString(i);
        }

        public object GetValue(int i)
        {
            return this.DataReader.GetValue(i);
        }

        public int GetValues(object[] values)
        {
            return this.DataReader.GetValues(values);
        }

        public bool IsDBNull(int i)
        {
            return this.DataReader.IsDBNull(i);
        }

        public object this[int i] => this.DataReader[i];
        public object this[string name] => this.DataReader[name];

        #region " IDisposable Support "

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        ///     Returns a DataTable.  If the file has a header row and the ContainerHeaderRow is set to true then the column names
        ///     that will be called from the reader will by those names.  Otherwise you'll have to reference those value through Items
        ///     and input their index.  The data table can then be bound to other objects such as a DataGridView.
        /// </summary>
        public DataTable CreateDataTable()
        {
            var dt = new DataTable();
            var lines = this.Text.Split(this.LineTerminator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            bool columnsCreated = false;

            foreach (string line in lines)
            {
                int fieldCount = 0;
                var fields = line.Split(this.Delimiter.ToCharArray());
                fieldCount = fields.GetUpperBound(0);
                // field count has to be consistant

                // Create the columns in the datatable if they don't exist
                if (columnsCreated == false)
                {
                    for (int counter = 0; counter <= fieldCount; counter++)
                    {
                        if (this.ContainsHeaderRow == false)
                        {
                            var newColumn = new DataColumn("Column" + counter);
                            dt.Columns.Add(newColumn);
                        }
                        else
                        {
                            dt.Columns.Add(new DataColumn(fields[counter]));
                        }
                    }

                    // If it doesn't contain a header row, we'll now need to show the info
                    if (this.ContainsHeaderRow == false)
                    {
                        dt.Rows.Add(fields);
                    }

                    columnsCreated = true;
                }
                else
                {
                    dt.Rows.Add(fields);
                }
            }

            return dt;
        }

        /// <summary>
        ///     Returns a DataTableReader.  If the file has a header row and the ContainerHeaderRow is set to true then the column names
        ///     that will be called from the reader will by those names.  Otherwise you'll have to reference those value through Items
        ///     and input their index.
        /// </summary>
        public DataTableReader ExecuteReader()
        {
            return this.CreateDataTable().CreateDataReader();
        }

        /// <summary>
        ///     Resets the underlying DataReader and creates a new one that is at the first position.
        /// </summary>
        public void MoveFirst()
        {
            if (this.DataReader != null)
            {
                this.DataReader.Close();
                this.DataReader = null;
            }

            if (this.DataTable != null)
            {
                this.DataReader = this.DataTable.CreateDataReader();
            }
        }

        /// <summary>
        ///     The number of records in the DataReader.
        /// </summary>
        public int RowCount()
        {
            if (this.DataTable == null)
            {
                return 0;
            }

            return this.DataTable.Rows.Count;
        }

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: free other state (managed objects).
                    this.DataReader?.Close();
                    this.DataReader?.Dispose();
                    this.DataTable?.Dispose();
                }

                // TODO: free your own state (unmanaged objects).
                // TODO: set large fields to null.
            }

            _disposedValue = true;
        }
    }
}