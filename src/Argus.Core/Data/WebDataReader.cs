﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Collections.Specialized;
using System.Text;

namespace Argus.Data
{

	/// <summary>
	/// A class that reads delimited files from a web address.
	/// </summary>
	/// <remarks>
	/// This class does not handle the delimiter being contained in the returned fields.  The data presented should make
	/// sure that the delimiter is escaped and does not appear in the fields returned.
	/// </remarks>
	public class WebDataReader : IDataReader
	{
		//*********************************************************************************************************************
		//
		//             Class:  WebDataReader
		//      Organization:  http://www.blakepell.com        
		//      Initial Date:  09/07/2011
		//      Last Updated:  09/07/2011
		//     Programmer(s):  Blake Pell, blakepell@hotmail.com
		//
		//*********************************************************************************************************************

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="url">The address of the web resource.</param>
		/// <remarks></remarks>
		public WebDataReader(string url) : this(url, "\t", true, new NameValueCollection(), new NameValueCollection(), null)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="url">The address of the web resource.</param>
		/// <param name="delimiter">The delimiter that sepeates fields.  The default seperator is a tab.</param>
		/// <remarks></remarks>
		public WebDataReader(string url, string delimiter) : this(url, delimiter, true, new NameValueCollection(), new NameValueCollection(), null)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="url">The address of the web resource.</param>
		/// <param name="delimiter">The delimiter that sepeates fields.  The default seperator is a tab.</param>
		/// <param name="firstRowContainsHeaders">Whether or not the first row contains column headers that can then be used to request items from in the DataReader.</param>
		/// <remarks></remarks>
		public WebDataReader(string url, string delimiter, bool firstRowContainsHeaders) : this(url, delimiter, firstRowContainsHeaders, new NameValueCollection(), new NameValueCollection(), null)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="url">The address of the web resource.</param>
		/// <param name="delimiter">The delimiter that sepeates fields.  The default seperator is a tab.</param>
		/// <param name="firstRowContainsHeaders">Whether or not the first row contains column headers that can then be used to request items from in the DataReader.</param>
		/// <param name="formData">Form field values to be posted to the web page.</param>
		/// <param name="queryStringData">QueryString values to be sent at the end of the Url.</param>
		/// <param name="cd">The credentials that should be used to authenticate to the web page.  A null set of credentials should be used for sites that don't need authentication.
		/// </param>
		/// <remarks></remarks>
		public WebDataReader(string url, string delimiter, bool firstRowContainsHeaders, NameValueCollection formData, NameValueCollection queryStringData, System.Net.ICredentials cd)
		{
			this.Url = url;
			this.Delimiter = delimiter;

			if (queryStringData == null)
			{
				queryStringData = new NameValueCollection();
			}

			if (formData == null)
			{
				formData = new NameValueCollection();
			}

			using (WebClient wc = new WebClient())
			{
				if (cd != null)
				{
					wc.Credentials = cd;
				}

				if (queryStringData.Count > 0)
				{
					wc.QueryString = queryStringData;
				}

				if (formData.Count > 0)
				{
					wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
					byte[] data = wc.UploadValues(url, formData);
					this.DataTable = Argus.Data.DatabaseUtils.DelimitedTextToDataTable(Encoding.ASCII.GetString(data), delimiter, firstRowContainsHeaders, "WebDataReader");
				}
				else
				{
					string data = wc.DownloadString(url);
					this.DataTable = Argus.Data.DatabaseUtils.DelimitedTextToDataTable(data, delimiter, firstRowContainsHeaders, "WebDataReader");
				}

				this.DataReader = this.DataTable.CreateDataReader();
			}

		}

		private string _delimiter = "\t";
		/// <summary>
		/// The delimiter that is used in the DataReader.  The default value is a tab.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		private string Delimiter
		{
			get { return _delimiter; }
			set { _delimiter = value; }
		}

		/// <summary>
		/// The underlaying DataTableReader that we are wrapping.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks>
		/// These methods are exposed via the implemented properties of this class and does not need
		/// to be exposed itself.
		/// </remarks>
		private DataTableReader DataReader { get; set; }

		/// <summary>
		/// The underlaying DataTable that is populated from the specified url.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks>
		/// This has been left a private variable so that the DataTable isn't tampered with while
		/// iterating over the DataReader.
		/// </remarks>
		private DataTable DataTable { get; set; }

		/// <summary>
		/// The url of the delimited text.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Url { get; set; } = "";

		/// <summary>
		/// Resets the underlaying DataReader and creates a new one that is at the first position.
		/// </summary>
		/// <remarks></remarks>
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
		/// Closes the DataReader
		/// </summary>
		/// <remarks></remarks>
		public void Close()
		{
			this.DataReader.Close();
		}

		/// <summary>
		/// The number of records in the DataReader.
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public int RowCount()
		{
			if (this.DataTable == null)
			{
				return 0;
			}

			return this.DataTable.Rows.Count;
		}

		public int Depth
		{
			get { return this.DataReader.Depth; }
		}

		public System.Data.DataTable GetSchemaTable()
		{
			return this.DataReader.GetSchemaTable();
		}

		public bool IsClosed
		{
			get { return this.DataReader.IsClosed; }
		}

		public bool NextResult()
		{
			return this.DataReader.NextResult();
		}

		public bool Read()
		{
			return this.DataReader.Read();
		}

		public int RecordsAffected
		{
			get { return this.DataReader.RecordsAffected; }
		}

		public int FieldCount
		{
			get { return this.DataReader.FieldCount; }
		}

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

		public System.Data.IDataReader GetData(int i)
		{
			throw new NotImplementedException();
		}

		public string GetDataTypeName(int i)
		{
			return this.DataReader.GetDataTypeName(i);
		}

		public System.DateTime GetDateTime(int i)
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

		public System.Type GetFieldType(int i)
		{
			return this.DataReader.GetFieldType(i);
		}

		public float GetFloat(int i)
		{
			return this.DataReader.GetFloat(i);
		}

		public System.Guid GetGuid(int i)
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

		public object this[int i]
		{
			get { return this.DataReader[i]; }
		}

		public object this[string name]
		{
			get { return this.DataReader[name]; }
		}

		// To detect redundant calls
		private bool _disposedValue = false;

		// IDisposable
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposedValue)
			{
				if (disposing)
				{
					Argus.Data.DatabaseUtils.CleanupDbResources(null, this.DataReader, null, null, this.DataTable);
					// TODO: free other state (managed objects).
				}

				// TODO: free your own state (unmanaged objects).
				// TODO: set large fields to null.
			}
			this._disposedValue = true;
		}

		#region " IDisposable Support "
		// This code added by Visual Basic to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

	}

}