/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-06-17
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Argus.Data;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DataTable" />.
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// The custom field formatting types available through given extensions.
        /// </summary>
        public enum FormatFieldTypes
        {
            /// <summary>
            /// Removes any extra characters from a number that are not numeric.
            /// </summary>
            /// <remarks></remarks>
            CleanupNumber,

            /// <summary>
            /// Formats the date column as a short date (MM/DD/YYYY)
            /// </summary>
            /// <remarks></remarks>
            ShortDate,

            /// <summary>
            /// Formats as a dollar value with the $ sign and 2 decimal places
            /// </summary>
            /// <remarks></remarks>
            Dollars,

            /// <summary>
            /// Returns the email address as an HTML mailto link.
            /// </summary>
            /// <remarks></remarks>
            EmailLink,

            /// <summary>
            /// Returns the value as an HTML link.
            /// </summary>
            /// <remarks></remarks>
            Link,

            /// <summary>
            /// Formats the column as a number with commas but no decimal
            /// </summary>
            /// <remarks></remarks>
            NumberWithFormattingNoDecimal,

            /// <summary>
            /// Formats the column as a number with commas and two decimal places.
            /// </summary>
            /// <remarks></remarks>
            NumberWithFormattingTwoDecimals,

            /// <summary>
            /// Formats the column as a percent with no decimal places.
            /// </summary>
            /// <remarks></remarks>
            PercentWithNoDecimal,

            /// <summary>
            /// Formats the column as a percent with two decimal places.
            /// </summary>
            /// <remarks></remarks>
            PercentWithTwoDecimals,

            /// <summary>
            /// Formats the column as a phone number.
            /// </summary>
            /// <remarks></remarks>
            PhoneNumber,

            /// <summary>
            /// Formats the column as a zip code.
            /// </summary>
            /// <remarks></remarks>
            ZipCode
        }

        /// <summary>
        /// Sets the ReadOnly flag on all of the columns in the DataTable.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="readOnly"></param>
        public static void ReadOnly(this DataTable dt, bool readOnly)
        {
            foreach (DataColumn col in dt.Columns)
            {
                col.ReadOnly = readOnly;
            }
        }

        /// <summary>
        /// Removes duplicates from the data table.
        /// </summary>
        /// <param name="dt"></param>
        /// <remarks>
        /// This extension method will remove duplicate rows from the DataTable that it is called from.
        /// </remarks>
        public static void RemoveDuplicateRows(this DataTable dt)
        {
            var columnList = new string[dt.Columns.Count - 1];

            for (int counter = 0; counter <= dt.Columns.Count - 1; counter++)
            {
                columnList[counter] = dt.Columns[counter].ColumnName;
            }

            dt = dt.DefaultView.ToTable(true, columnList);
        }

        /// <summary>
        /// Returns duplicate values in a column.
        /// </summary>
        /// <param name="dt">The DataTable to search.</param>
        /// <param name="columnName">The column to look for duplicate values in.</param>
        /// <param name="distinctList">
        /// Whether or not all of the values should be returned.  E.g. If an ID of 1 is in the list 10 times, it can either
        /// be returned once indicating it's a duplicate or it can be returned all 10 times.
        /// </param>
        public static List<object> SelectDuplicates(this DataTable dt, string columnName, bool distinctList)
        {
            var duplicateList = new List<object>();

            for (int x = 0; x <= dt.Rows.Count - 1; x++)
            {
                if (dt.Select($"{columnName} = {dt.Rows[x][columnName]}").Length > 1)
                {
                    if (distinctList)
                    {
                        if (duplicateList.Contains(dt.Rows[x][columnName]) == false)
                        {
                            duplicateList.Add(dt.Rows[x][columnName]);
                        }
                    }
                    else
                    {
                        duplicateList.Add(dt.Rows[x][columnName]);
                    }
                }
            }

            return duplicateList;
        }

        /// <summary>
        /// Returns unique values in a column.
        /// </summary>
        /// <param name="dt">The DataTable to search.</param>
        /// <param name="columnName">The column to look for unique values in.</param>
        public static List<object> SelectUniques(this DataTable dt, string columnName)
        {
            var uniqueList = new List<object>();

            for (int x = 0; x <= dt.Rows.Count - 1; x++)
            {
                if (dt.Select($"{columnName} = {dt.Rows[x][columnName]}").Length == 1)
                {
                    // This is only going to exist one time so we do not need to check to see if exists
                    uniqueList.Add(dt.Rows[x][columnName]);
                }
            }

            return uniqueList;
        }

        /// <summary>
        /// Returns a DataTable with the selected criteria ordered by the requested field.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        public static DataTable SelectRows(this DataTable dt, string whereExpression, string orderByExpression)
        {
            dt.DefaultView.RowFilter = whereExpression;
            dt.DefaultView.Sort = orderByExpression;

            return dt.DefaultView.ToTable();
        }

        /// <summary>
        /// Sorts records by the supplied fields.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="orderBy"></param>
        /// <remarks>
        /// This extension will sort the records in the DataTable by the supplied orderBy string.  This is similar
        /// to SQL only the Order By clause is not needed.  This works by setting the Sort property on the
        /// DataTable.DefaultView object.  An exception will occur if the supplied string includes fields that
        /// do not exist in the DataTable's schema.
        /// <code>
        /// DataTable.OrderBy("last_name, first_name")
        /// </code>
        /// </remarks>
        public static void OrderBy(this DataTable dt, string orderBy)
        {
            dt.DefaultView.Sort = orderBy;
        }

        /// <summary>
        /// Formats a specified column in a DataTable with one of the pre-specified formats.  These will only work in string
        /// data type fields.
        /// Todo: Could the data.formatting functions be exposed as extensions on Object or String?
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnIndex"></param>
        /// <param name="formatType"></param>
        public static DataTable FormatColumn(this DataTable dt, int columnIndex, FormatFieldTypes formatType)
        {
            // The main rows/cells
            foreach (DataRow row in dt.Rows)
            {
                switch (formatType)
                {
                    case FormatFieldTypes.Dollars:
                        row[columnIndex] = Formatting.FormatDollars(row[columnIndex].ToString());

                        break;
                    case FormatFieldTypes.EmailLink:
                        row[columnIndex] = Formatting.ReturnMailLink(row[columnIndex].ToString());

                        break;
                    case FormatFieldTypes.Link:
                        row[columnIndex] = Formatting.ReturnLinkHtml(row[columnIndex].ToString(), true, true);

                        break;
                    case FormatFieldTypes.CleanupNumber:
                        row[columnIndex] = Formatting.CleanupNumber(row[columnIndex].ToString());

                        break;
                    case FormatFieldTypes.NumberWithFormattingNoDecimal:
                        row[columnIndex] = row[columnIndex].ToString().FormatIfNumber(0);

                        break;
                    case FormatFieldTypes.NumberWithFormattingTwoDecimals:
                        row[columnIndex] = row[columnIndex].ToString().FormatIfNumber(2);

                        break;
                    case FormatFieldTypes.PercentWithNoDecimal:
                        row[columnIndex] = Formatting.FormatPercent(row[columnIndex].ToString(), 0);

                        break;
                    case FormatFieldTypes.PercentWithTwoDecimals:
                        row[columnIndex] = Formatting.FormatPercent(row[columnIndex].ToString(), 2);

                        break;
                    case FormatFieldTypes.PhoneNumber:
                        row[columnIndex] = Formatting.FormatPhoneNumber(row[columnIndex].ToString());

                        break;
                    case FormatFieldTypes.ZipCode:
                        row[columnIndex] = Formatting.FormatZipCode(row[columnIndex].ToString());

                        break;
                    case FormatFieldTypes.ShortDate:
                        DateTime asDate;

                        if (DateTime.TryParse(row[columnIndex].ToString(), out asDate))
                        {
                            row[columnIndex] = asDate.ToString("MM/dd/yyyy");
                        }

                        break;
                }
            }

            return dt;
        }

        /// <summary>
        /// Formats a specified column in a DataTable with one of the pre-specified formats.  These will only work in string data
        /// type fields.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <param name="formatType"></param>
        public static DataTable FormatColumn(this DataTable dt, string columnName, FormatFieldTypes formatType)
        {
            int columnIndex = dt.Columns.IndexOf(columnName);

            return dt.FormatColumn(columnIndex, formatType);
        }

        /// <summary>
        /// Adds the contents of a DataReader to a DataTable, but only where the field names match up.  This requires that the field
        /// names that do match up have the same field names.  If there are columns in the IDataReader that do not match columns in the
        /// DataTable then an exception will be thrown.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dr"></param>
        /// <remarks>
        /// TODO:  Make it so it ignores fields in the data reader that don't exist instead of throwing an exception.
        /// </remarks>
        public static void PartialAdd(this DataTable dt, IDataReader dr)
        {
            while (dr.Read())
            {
                var row = dt.NewRow();

                for (int x = 0; x <= dr.FieldCount - 1; x++)
                {
                    string name = dr.GetName(x);
                    row[name] = dr[name];
                }

                dt.Rows.Add(row);
            }
        }

        /// <summary>
        /// Adds the contents of a DataReader to a DataTable, but only where the field names match up.  This requires that the field
        /// names that do match up have the same field names.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dr"></param>
        /// <param name="ignoreMismatchedFields">If True, will ignore fields that don't match up.  If False, an exception will be thrown if fields are found that don't match up.</param>
        public static void PartialAdd(this DataTable dt, IDataReader dr, bool ignoreMismatchedFields)
        {
            if (ignoreMismatchedFields == false)
            {
                PartialAdd(dt, dr);

                return;
            }

            while (dr.Read())
            {
                var row = dt.NewRow();

                for (int x = 0; x <= dr.FieldCount - 1; x++)
                {
                    string name = dr.GetName(x);

                    if (row.Table.Columns.Contains(name))
                    {
                        row[name] = dr[name];
                    }
                }

                dt.Rows.Add(row);
            }
        }

        /// <summary>
        /// Merges two XML files that were saved from a DataTable.  These XML files must have been saved
        /// from the same DataTable otherwise an exception will be thrown.  This DataTable must match
        /// the schema of the XML files for the merge to occur.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePathOne">The path to the first XML file.</param>
        /// <param name="filePathTwo">The path to the second XML file.</param>
        /// <param name="destinationPath"></param>
        public static void MergeXml(this DataTable dt, string filePathOne, string filePathTwo, string destinationPath)
        {
            var firstDt = new DataTable();
            var secondDt = new DataTable();

            // Set the name of the tables to this DataTable
            firstDt.TableName = dt.TableName;
            secondDt.TableName = dt.TableName;

            // Clone the structure of the DataTable
            firstDt = dt.Clone();
            secondDt = dt.Clone();

            // Read in the XML files, if the structure doesn't match, an exception will be thrown.
            firstDt.ReadXml(filePathOne);
            secondDt.ReadXml(filePathTwo);

            // Next the merge.
            firstDt.Merge(secondDt);

            // Write the file out.
            firstDt.WriteXml(destinationPath);

            firstDt.Clear();
            firstDt.Dispose();
            secondDt.Clear();
            secondDt.Dispose();
        }

        /// <summary>
        /// Returns the number of tables that exist in a DataSet.  This performs all of the appropriate null checks.  A table will be
        /// returned in the count whether it contains data or not.
        /// </summary>
        /// <param name="ds"></param>
        public static int GetTableCount(this DataSet ds)
        {
            if (ds?.Tables == null)
            {
                return 0;
            }

            return ds.Tables.Count;
        }

        /// <summary>
        /// Checks to see whether the DataSet is empty of not.  This will also look in the tables to see if data
        /// exists, not just at whether a table exists.  All appropriate null checks are performed.
        /// </summary>
        /// <param name="ds"></param>
        public static bool IsEmpty(this DataSet ds)
        {
            if (ds?.Tables == null)
            {
                return true;
            }

            if (ds.Tables.Count == 0)
            {
                return true;
            }

            foreach (DataTable dt in ds.Tables)
            {
                if (dt.Rows.Count > 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Replaces a value in a specified column with a new value.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnIndex"></param>
        /// <param name="findValue"></param>
        /// <param name="replaceValue"></param>
        public static void ReplaceInColumn(this DataTable dt, int columnIndex, string findValue, string replaceValue)
        {
            foreach (DataRow row in dt.Rows)
            {
                row[columnIndex] = row[columnIndex].ToString().Replace(findValue, replaceValue);
            }
        }

        /// <summary>
        /// Replaces a value in a specified column with a new value.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <param name="findValue"></param>
        /// <param name="replaceValue"></param>
        public static void ReplaceInColumn(this DataTable dt, string columnName, string findValue, string replaceValue)
        {
            int columnIndex = dt.Columns.IndexOf(columnName);
            ReplaceInColumn(dt, columnIndex, findValue, replaceValue);
        }

        /// <summary>
        /// Replaces a value in the entire table with a new value.  This will only attempt to replace values in types that map to System.String.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="findValue"></param>
        /// <param name="replaceValue"></param>
        public static void ReplaceInTable(this DataTable dt, string findValue, string replaceValue)
        {
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in row.ItemArray)
                {
                    if (col.DataType is string)
                    {
                        row[col.ColumnName] = row[col.ColumnName].ToString().Replace(findValue, replaceValue);
                    }
                }
            }
        }

        /// <summary>
        /// Exports the contents of the DataTable to a delimited string.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="exportHeader">Whether or not to export the name of the columns in the header row.</param>
        /// <param name="delimiter">The value to place in between the individual fields, typically something like a tab.</param>
        public static object ToString(this DataTable dt, bool exportHeader, string delimiter)
        {
            var ff = new CreateDelimitedFile(exportHeader, delimiter);

            return ff.ToString(dt);
        }

        /// <summary>
        /// Returns a value from the specified column.  If the column is a null a blank will be returned safely.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        public static object GetValue(this DataRow dr, string columnName)
        {
            int columnIndex = dr.Table.Columns.IndexOf(columnName);

            return dr.GetValue(columnIndex);
        }

        /// <summary>
        /// Returns a value from the specified column.  If the column is null a blank will be returned safely.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnIndex"></param>
        public static object GetValue(this DataRow dr, int columnIndex)
        {
            if (dr[columnIndex] == null || dr[columnIndex] == DBNull.Value)
            {
                return "";
            }

            return dr[columnIndex];
        }

        /// <summary>
        /// Returns a delimited string of the DataRow.  Leave the lineTerminator blank if you don't want it at the end of the record.
        /// The delimiter will be escaped in any field with a \d code.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="delimiter"></param>
        /// <param name="lineTerminator"></param>
        public static string ToString(this DataRow dr, string delimiter, string lineTerminator = "")
        {
            var sb = new StringBuilder();

            for (int x = 0; x <= dr.Table.Columns.Count; x++)
            {
                sb.AppendFormat("{0}{1}", dr[x].ToString().Replace(delimiter, "\\d"), delimiter);
            }

            if (string.IsNullOrEmpty(lineTerminator))
            {
                return sb.ToString().TrimEnd(delimiter.ToCharArray());
            }

            return sb.ToString().TrimEnd(delimiter.ToCharArray()) + lineTerminator;
        }

        /// <summary>
        /// Returns a tab delimited string of the DataRow.  A carriage return/line feed will be used as the
        /// line terminator.  Tabs are escaped to \t.
        /// </summary>
        /// <param name="dr"></param>
        public static string ToString(this DataRow dr)
        {
            var sb = new StringBuilder();

            for (int x = 0; x <= dr.Table.Columns.Count; x++)
            {
                sb.AppendFormat("{0}\t", dr[x].ToString().Replace("\t", "\\t"));
            }

            return sb.ToString().TrimEnd('\t') + Environment.NewLine;
        }
    }
}