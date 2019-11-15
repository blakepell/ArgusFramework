using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Data.Common;
using Argus.Extensions;

namespace Argus.Data
{

    /// <summary>
    /// Various database utilities and helper subs/functions.
    /// </summary>
    /// <remarks></remarks>
    public class DatabaseUtils
    {
        //*********************************************************************************************************************
        //
        //             Class:  DatabaseUtils
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  05/07/2008
        //      Last Updated:  02/09/2018
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Converts delimited text into a data table.
        /// </summary>
        /// <param name="buf">The delimited text you want to convert.</param>
        /// <param name="delimiter">The delimiter the text is split up by, typically a tab.</param>
        /// <param name="firstRowContainsHeader">Whether or not the first row contains column headers.</param>
        /// <param name="tableName">The name of the new datatable.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTable DelimitedTextToDataTable(string buf, string delimiter, bool firstRowContainsHeader, string tableName)
        {
            int rowCount = 0;

            // Get rid of carriage return and line feed combo's and make them just line feeds.
            buf = buf.Replace("\r\n", "\n");

            string[] records = buf.Split("\n".ToCharArray());
            DataTable dt = new DataTable(tableName);

            foreach (string record in records)
            {
                string[] fields = record.Split(delimiter.ToCharArray());

                if (firstRowContainsHeader == true & rowCount == 0)
                {
                    // This is the header row, add the columns then move on
                    foreach (string field in fields)
                    {
                        dt.Columns.Add(field, System.Type.GetType("System.String"));
                    }
                }
                else if (firstRowContainsHeader == false & rowCount == 0)
                {
                    int fieldCounter = 0;
                    foreach (string field in fields)
                    {
                        fieldCounter += 1;
                        dt.Columns.Add("Field" + fieldCounter.ToString(), System.Type.GetType("System.String"));
                    }

                    // Since it wasn't a header row, it needs to be added in.
                    dt.Rows.Add(fields);
                }
                else
                {
                    // Just a row, add it.
                    dt.Rows.Add(fields);
                }

                rowCount += 1;
            }

            return dt;

        }

        /// <summary>
        /// This will try to open a database connection.  If the connection fails because of a timeout it will attempt to connect
        /// 2 more times.  
        /// </summary>
        /// <param name="conn"></param>
        /// <remarks>
        /// An exception will be thrown only if the connection passed is is null or the connection fails to connect 3 times.
        /// </remarks>
        public static void OpenDbConnection(IDbConnection conn)
        {
            if (conn == null)
            {
                throw new Exception("The connection object is null.");
            }

            switch (conn.State)
            {
                case ConnectionState.Open:
                    return;
                case ConnectionState.Connecting:
                case ConnectionState.Broken:
                case ConnectionState.Fetching:
                case ConnectionState.Executing:
                    conn.Close();
                    break;
            }

            int tries = 0;

            while (tries <= 3)
            {
                tries += 1;

                try
                {
                    conn.Open();
                    return;
                }
                catch (Exception ex)
                {
                    // Throw the exception on the third try
                    if (tries >= 3)
                    {
                        throw ex;
                    }
                }
            }

        }

        /// <summary>
        /// Returns the specified value from a data reader and performs IsNull checks and additional checks to make sure
        /// that the data reader is valid.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="field">The database field to return.</param>
        /// <returns>String representation of the data type.  
        /// </returns>
        /// <remarks></remarks>
        public static string GetValue(ref IDataReader dr, string field)
        {

            if (dr == null | dr.IsClosed == true)
            {
                return "";
            }

            try
            {
                if (Convert.IsDBNull(dr[field]) == true)
                {
                    return "";
                }
                else {
                    try
                    {
                        if (dr[field].ToString().ToLower() == "(null)")
                        {
                            return "";
                        }
                        else {
                            return dr[field].ToString();
                        }
                    }
                    catch 
                    {
                        return "";
                    }
                }
            }
            catch
            {
                return "(Invalid Field: " + field + ")";
            }

        }

        /// <summary>
        /// Returns the specified value from a data reader and performs IsNull checks and additional checks to make sure
        /// that the data reader is valid.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="field">The database field to return.</param>
        /// <param name="defaultValue">A default value to return if the data value is null (or blank if specified)</param>
        /// <param name="defaultIncludesBlanks">Whether or not to return the default value specified if the data field is blank.</param>
        /// <returns>String representation of the data type.  
        /// </returns>
        /// <remarks></remarks>
        public static string GetValue(ref IDataReader dr, string field, string defaultValue, bool defaultIncludesBlanks)
        {

            if (dr == null | dr.IsClosed == true)
            {
                return defaultValue;
            }

            try
            {
                if (Convert.IsDBNull(dr[field]) == true)
                {
                    return defaultValue;
                }
                else
                {
                    try
                    {
                        if (dr[field].ToString().ToLower() == "(null)")
                        {
                            return defaultValue;
                        }
                        else {
                            if (string.IsNullOrEmpty(dr[field].ToString()) == true & defaultIncludesBlanks == true)
                            {
                                return defaultValue;
                            }
                            else {
                                return dr[field].ToString();
                            }
                        }
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }
            catch 
            {
                return "(Invalid Field: " + field + ")";
            }

        }


        /// <summary>
        /// Returns the value of an item from a object type in a rows item property from a data table.
        /// </summary>
        /// <param name="dbItem"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetValue(object dbItem)
        {
            try
            {
                if (Convert.IsDBNull(dbItem) == true)
                {
                    return "";
                }
                else
                {
                    return dbItem.ToString();
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the specified value from a data reader and performs IsNull checks and additional checks to make sure
        /// that the data reader is valid.  Though this returns a string checks will be done to be sure that it is be a 
        /// number within the string.  You can do the conversion then to the number type that you need specifically
        /// need (e.g. Double, Integer, Int16, Int32, Int64, Long, etc.).
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="field"></param>
        /// <returns>String value with a number in it.</returns>
        /// <remarks></remarks>
        public static string GetNumberValue(ref IDataReader dr, string field)
        {

            if (dr.IsClosed == true)
            {
                return "0";
            }

            try
            {
                if (Convert.IsDBNull(dr[field]) == true)
                {
                    return "0";
                }
                else
                {
                    try
                    {
                        if (dr[field].ToString().IsNumeric() == true)
                        {
                            return dr[field].ToString();
                        }
                        else {
                            return "0";
                        }
                    }
                    catch 
                    {
                        return "0";
                    }
                }
            }
            catch 
            {
                return "(Invalid Field: " + field + ")";
            }

        }

        /// <summary>
        /// Cleans up and closes / disposes of any database resources.  If you don't need a specific object cleaned up pass it in as
        /// null (e.g. nothing).
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="dr"></param>
        /// <param name="command"></param>
        /// <remarks>This sub should close, dispose and cleanup all passed in objects which are used as references.</remarks>
        public static void CleanupDbResources(IDbConnection conn, IDataReader dr, IDbCommand command)
        {
            // Cleanup the data reader
            if (dr != null)
            {
                if (dr.IsClosed == false)
                {
                    dr.Close();
                }

                dr.Dispose();
                dr = null;
            }

            // Cleanup the database command object.
            if (command != null)
            {
                command.Dispose();
                command = null;
            }

            // Cleanup the connection.
            if (conn != null)
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }

                conn.Dispose();
                conn = null;
            }

        }

        /// <summary>
        /// Cleans up and closes / disposes of any database resources.  If you don't need a specific object cleaned up pass it in as
        /// null (e.g. nothing).
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="dr"></param>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <param name="dataTable"></param>
        /// <remarks>This sub should close, dispose and cleanup all passed in objects which are used as references.</remarks>

        public static void CleanupDbResources(IDbConnection conn, IDataReader dr, IDbCommand command, IDbTransaction transaction, DataTable dataTable)
        {
            // Cleanup the data reader
            if (dr != null)
            {
                if (dr.IsClosed == false)
                {
                    dr.Close();
                }

                dr.Dispose();
                dr = null;
            }

            // Cleanup the database transaction
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }

            if (dataTable != null)
            {
                dataTable.Clear();
                dataTable.Dispose();
                dataTable = null;
            }

            // Cleanup the database command object.
            if (command != null)
            {
                command.Dispose();
                command = null;
            }

            // Cleanup the connection.
            if (conn != null)
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }

                conn.Dispose();
                conn = null;
            }

        }

        /// <summary>
        /// Cleans up and closes / disposes of any database resources.  If you don't need a specific object cleaned up pass it in as
        /// null (e.g. nothing).
        /// </summary>
        /// <param name="dataTable"></param>
        /// <remarks>This sub should close, dispose and cleanup all passed in objects which are used as references.</remarks>
        public static void CleanupDbResources(DataTable dataTable)
        {
            if (dataTable != null)
            {
                dataTable.Clear();
                dataTable.Dispose();
                dataTable = null;
            }
        }

        /// <summary>
        /// Cleans up and closes / disposes of any database resources.  If you don't need a specific object cleaned up pass it in as
        /// null (e.g. nothing).
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="command"></param>
        /// <remarks>This sub should close, dispose and cleanup all passed in objects which are used as references.</remarks>
        public static void CleanupDbResources(IDataReader dr, IDbCommand command)
        {
            CleanupDbResources(dr);
            CleanupDbResources(command);
        }

        /// <summary>
        /// Cleans up and closes / disposes of any database resources.  If you don't need a specific object cleaned up pass it in as
        /// null (e.g. nothing).
        /// </summary>
        /// <param name="dr"></param>
        /// <remarks>This sub should close, dispose and cleanup all passed in objects which are used as references.</remarks>

        public static void CleanupDbResources(IDataReader dr)
        {
            // Cleanup the data reader
            if (dr != null)
            {
                if (dr.IsClosed == false)
                {
                    dr.Close();
                }

                dr.Dispose();
                dr = null;
            }
        }

        /// <summary>
        /// Cleans up and closes / disposes of any database resources.  If you don't need a specific object cleaned up pass it in as
        /// null (e.g. nothing).
        /// </summary>
        /// <param name="conn"></param>
        /// <remarks>This sub should close, dispose and cleanup all passed in objects which are used as references.</remarks>
        public static void CleanupDbResources(IDbConnection conn)
        {
            // Cleanup the connection.
            if (conn != null)
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }

                conn.Dispose();
                conn = null;
            }
        }

        /// <summary>
        /// Cleans up and closes / disposes of any database resources.  If you don't need a specific object cleaned up pass it in as
        /// null (e.g. nothing).
        /// </summary>
        /// <param name="command"></param>
        /// <remarks>This sub should close, dispose and cleanup all passed in objects which are used as references.</remarks>
        public static void CleanupDbResources(IDbCommand command)
        {
            if (command == null)
            {
                return;
            }

            command.Dispose();
            command = null;
        }


        /// <summary>
        /// Formats specified columns in a DataTable to a given number format
        /// </summary>
        /// <param name="dt">The DataTable, passed by reference.</param>
        /// <param name="columnIndexes">The index of the columns to format.</param>
        /// <param name="precision">The number of digits after the decimal.</param>
        /// <param name="preCharacter">A character or string that goes before the number, e.g. a $</param>
        /// <param name="postCharacter">A character or string that goes after the number, e.g. a %</param>
        /// <remarks>
        /// This procedure will force all columns ReadOnly property to false in order to make the changes.  Changing a number
        /// to a string will require that the DataType of the field be a character/string format otherwise an exception will be
        /// thrown.  A DataTable does not allow the DataType to be changed once data exists.  The other option is to specifically
        /// set all fields on the DataTable to String before it's loaded from a DataReader (this will force it to be cast when it's
        /// loading, otherwise the DataReader will pass it's type on to the DataTable).  Any values that are null will be converted
        /// to 0 for display purposes.
        /// 
        /// TODO:  Add a parameter that will automatically convert the table to be all strings.  This will require creating a new table
        /// and swapping data between the two.
        /// 
        /// </remarks>

        public static void FormatNumberColumns(ref DataTable dt, List<int> columnIndexes, int precision, string preCharacter, string postCharacter)
        {
            foreach (DataColumn c in dt.Columns)
            {
                c.ReadOnly = false;
            }

            foreach (DataRow row in dt.Rows)
            {

                foreach (int i in columnIndexes)
                {
                    if (row[i] == null)
                    {
                        row[i] = 0;
                    }
                    else if (Convert.IsDBNull(row[i]) == true)
                    {
                        row[i] = 0;
                    }

                    if (row[i].ToString().IsNumeric() == true)
                    {
                        row.BeginEdit();
                        row[i] = string.Format("{0}{1}{2}", preCharacter, row[i].ToString().FormatIfNumber(precision), postCharacter);
                        row.EndEdit();
                    }
                }
            }

        }

        /// <summary>
        /// Takes a data table and switches the columns and the rows creating a cross tab view.
        /// </summary>
        /// <param name="sourceDataTable"></param>
        /// <param name="removeFirstColumn">
        /// Whether or not to remove the first column.  The first column data becomes the name for the column headers
        /// and thus is repeated if not removed.  A case to remove it would be where you are binding to a DataGrid that
        /// will already display that information from the column headings.  A case to leave it would be if you were
        /// exporting the cross tab to a flat file or manually looping over it.
        /// </param>
        /// <param name="forceStringType">
        /// Whether or not to force all columns to be a string type.  Otherwise, the type will
        /// be determined by the variable that you insert into the data table.  The string type is desireable in cases where you
        /// want to make modifications to formatting of numbers and store them as a string.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// This function assumes that the first column is a descriptive column of some sort.  An example would
        /// be an account number, unit code, person name, etc.
        /// </remarks>
        public static DataTable DataTableToCrossTab(DataTable sourceDataTable, bool removeFirstColumn, bool forceStringType)
        {

            DataTable newDataTable = new DataTable();

            //************************************************************
            // Create the columns
            //************************************************************

            // This is the upper top left hand cell, it will be blank for the cross tab view.  We put one space in
            // because the column name cannot actually be blank, so this is our hack when the table is bound to a 
            // data grid.
            newDataTable.Columns.Add(" ");

            // Loop through the source rows and create the new columns from them
            foreach (DataRow row in sourceDataTable.Rows)
            {
                DataColumn dc = new DataColumn(row[0].ToString());
                dc.ReadOnly = false;

                if (forceStringType == true)
                {
                    dc.DataType = Type.GetType("System.String", true, true);
                }

                newDataTable.Columns.Add(dc);
            }

            //************************************************************
            // Create the rows
            //************************************************************
            for (int c = 0; c <= sourceDataTable.Columns.Count - 1; c++)
            {
                if ((removeFirstColumn == true & c > 0) | removeFirstColumn == false)
                {
                    DataRow newRow = newDataTable.Rows.Add();

                    newRow[0] = sourceDataTable.Columns[c].ColumnName;

                    for (int x = 0; x <= sourceDataTable.Rows.Count - 1; x++)
                    {
                        newRow[x + 1] = sourceDataTable.Rows[x][c];
                    }
                }
            }

            return newDataTable;

        }

        /// <summary>
        /// Returns the first non null value from the DataRowView as specified in the keyList.  If no non null value is found then a blank is returned.
        /// </summary>
        /// <param name="drv"></param>
        /// <param name="keyList"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetValueCoalesce(System.Data.DataRowView drv, string[] keyList, bool handleBlankAsNull)
        {
            foreach (string key in keyList)
            {
                if (drv.DataView.Table.Columns.Contains(key))
                {
                    if (handleBlankAsNull == true & string.IsNullOrEmpty(drv[key].ToString()))
                    {
                        continue;
                    }
                    return drv[key].ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// Returns the first non null value from the DataRow as specified in the keyList.  If no non null value is found then a blank is returned.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="keyList"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetValueCoalesce(System.Data.DataRow row, string[] keyList)
        {
            foreach (string key in keyList)
            {
                if (row.Table.Columns.Contains(key))
                {
                    return row[key].ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// Returns a DataTable containing the schema for the results of the executed SQL statement.
        /// </summary>
        /// <param name="conn">An open database connection.</param>
        /// <param name="selectSql">A select command, e.g. Select Top 1 * From users or Select * From Users Limit 1</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Data.DataTable SchemaTable(System.Data.Common.DbConnection conn, string selectSql)
        {
            return SchemaTable(conn, selectSql, "");
        }

        /// <summary>
        /// Returns a DataTable containing the schema for the results of the executed SQL statement.
        /// </summary>
        /// <param name="conn">An open database connection.</param>
        /// <param name="selectSql">A select command, e.g. Select Top 1 * From users or Select * From Users Limit 1</param>
        /// <param name="orderByClause">E.g. ColumnOrdinal ASC</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Data.DataTable SchemaTable(System.Data.Common.DbConnection conn, string selectSql, string orderByClause)
        {
            if (conn == null)
            {
                throw new Exception("Database connection was null.");
            }
            else if (conn.State != System.Data.ConnectionState.Open)
            {
                throw new Exception("Database connection must already be open.");
            }

            System.Data.Common.DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = selectSql;
            System.Data.Common.DbDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SchemaOnly | System.Data.CommandBehavior.KeyInfo);
            System.Data.DataTable dt = dr.GetSchemaTable();

            if (string.IsNullOrEmpty(orderByClause) == false)
            {
                dt.DefaultView.Sort = orderByClause;
            }

            cmd.Dispose();
            cmd = null;
            dr.Close();
            dr = null;
            return dt;
        }

    }

}