/*
 * @author            : Blake Pell
 * @initial date      : 2008-05-07
 * @last updated      : 2021-02-06
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Argus.Extensions;

namespace Argus.Data
{
    /// <summary>
    /// Various database utilities and helper subs/functions.
    /// </summary>
    public static class DatabaseUtils
    {
        /// <summary>
        /// Converts delimited text into a data table.
        /// </summary>
        /// <param name="buf">The delimited text you want to convert.</param>
        /// <param name="delimiter">The delimiter the text is split up by, typically a tab.</param>
        /// <param name="firstRowContainsHeader">Whether or not the first row contains column headers.</param>
        /// <param name="tableName">The name of the new <see cref="DataTable" />.</param>
        public static DataTable DelimitedTextToDataTable(string buf, string delimiter, bool firstRowContainsHeader, string tableName)
        {
            int rowCount = 0;

            // Get rid of carriage return and line feed combo's and make them just line feeds.
            buf = buf.Replace("\r\n", "\n");

            var records = buf.Split("\n".ToCharArray());
            var dt = new DataTable(tableName);

            foreach (string record in records)
            {
                var fields = record.Split(delimiter.ToCharArray());

                if (firstRowContainsHeader & (rowCount == 0))
                {
                    // This is the header row, add the columns then move on
                    foreach (string field in fields)
                    {
                        dt.Columns.Add(field, Type.GetType("System.String"));
                    }
                }
                else if ((firstRowContainsHeader == false) & (rowCount == 0))
                {
                    int fieldCounter = 0;

                    foreach (string field in fields)
                    {
                        fieldCounter += 1;
                        dt.Columns.Add("Field" + fieldCounter, Type.GetType("System.String"));
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
        /// TODO:  Add a parameter that will automatically convert the table to be all strings.  This will require creating a new table
        /// and swapping data between the two.
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
                    else if (Convert.IsDBNull(row[i]))
                    {
                        row[i] = 0;
                    }

                    if (row[i].ToString().IsNumeric())
                    {
                        row.BeginEdit();
                        row[i] = $"{preCharacter}{row[i].ToString().FormatIfNumber(precision)}{postCharacter}";
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
        /// be determined by the variable that you insert into the data table.  The string type is desirable in cases where you
        /// want to make modifications to formatting of numbers and store them as a string.
        /// </param>
        /// <remarks>
        /// This function assumes that the first column is a descriptive column of some sort.  An example would
        /// be an account number, unit code, person name, etc.
        /// </remarks>
        public static DataTable DataTableToCrossTab(DataTable sourceDataTable, bool removeFirstColumn, bool forceStringType)
        {
            var newDataTable = new DataTable();

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
                var dc = new DataColumn(row[0].ToString());
                dc.ReadOnly = false;

                if (forceStringType)
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
                if ((removeFirstColumn & (c > 0)) | (removeFirstColumn == false))
                {
                    var newRow = newDataTable.Rows.Add();

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
        /// <param name="handleBlankAsNull"></param>
        public static string GetValueCoalesce(DataRowView drv, string[] keyList, bool handleBlankAsNull)
        {
            foreach (string key in keyList)
            {
                if (drv.DataView.Table.Columns.Contains(key))
                {
                    if (handleBlankAsNull & string.IsNullOrEmpty(drv[key].ToString()))
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
        public static string GetValueCoalesce(DataRow row, string[] keyList)
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
        public static DataTable SchemaTable(DbConnection conn, string selectSql)
        {
            return SchemaTable(conn, selectSql, "");
        }

        /// <summary>
        /// Returns a DataTable containing the schema for the results of the executed SQL statement.
        /// </summary>
        /// <param name="conn">An open database connection.</param>
        /// <param name="selectSql">A select command, e.g. Select Top 1 * From users or Select * From Users Limit 1</param>
        /// <param name="orderByClause">E.g. ColumnOrdinal ASC</param>
        public static DataTable SchemaTable(DbConnection conn, string selectSql, string orderByClause)
        {
            if (conn == null)
            {
                throw new Exception("Database connection was null.");
            }

            if (conn.State != ConnectionState.Open)
            {
                throw new Exception("Database connection must already be open.");
            }

            var cmd = conn.CreateCommand();
            cmd.CommandText = selectSql;
            var dr = cmd.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);
            var dt = dr.GetSchemaTable();

            if (string.IsNullOrEmpty(orderByClause) == false)
            {
                dt.DefaultView.Sort = orderByClause;
            }

            cmd.Dispose();
            dr.Close();

            return dt;
        }
    }
}