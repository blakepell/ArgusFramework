/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2010-01-26
 * @last updated      : 2021-02-07
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Data;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for all classes that implement the IDataReader interface.
    /// </summary>
    public static class DataReaderExtensions
    {

        /// <summary>
        /// Returns the specified value from a data reader and performs IsNull checks and additional checks to make sure
        /// that the data reader is valid.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="field">The database field to return.</param>
        /// <param name="defaultValue">A default value to return if the data value is null (or blank if specified)</param>
        /// <param name="defaultIncludesBlanks">Whether or not to return the default value specified if the data field is blank.</param>
        public static string GetValue(this IDataReader dr, string field, string defaultValue = "", bool defaultIncludesBlanks = true)
        {
            if (dr == null || dr.IsClosed)
            {
                return defaultValue;
            }

            try
            {
                if (dr[field] == null || dr[field] == DBNull.Value)
                {
                    return defaultValue;
                }

                try
                {
                    if (dr[field].ToString().Equals("(null)", StringComparison.OrdinalIgnoreCase))
                    {
                        return defaultValue;
                    }

                    if (string.IsNullOrEmpty(dr[field].ToString()) && defaultIncludesBlanks)
                    {
                        return defaultValue;
                    }

                    return dr[field].ToString();
                }
                catch
                {
                    return defaultValue;
                }
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns a delimited string with the contents created from the IDataReader.  This means that the DataReader will be
        /// unusable after because it will have already been read through (DataReader's being forward only).
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldDelimiter"></param>
        public static string ToString(this IDataReader dr, string fieldDelimiter)
        {
            var cdf = new CreateDelimitedFile(false, fieldDelimiter);
            return cdf.ToString(dr);
        }

        /// <summary>
        /// Returns a character formatted string for display purposes from the contents of the IDataReader.  This means that the DataReader will be
        /// unusable after because it will have already been read through (DataReader's being forward only).
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="formatted">
        /// Whether or not the string should be formatted and lined up with spaces.  Setting this value as false will
        /// return just a delimited file.
        /// </param>
        /// <param name="fieldDelimiter"></param>
        /// <param name="exportHeader"></param>
        public static string ToString(this IDataReader dr, bool formatted, string fieldDelimiter, bool exportHeader)
        {
            var cdf = new CreateDelimitedFile(exportHeader, fieldDelimiter);
            string buf = cdf.ToString(dr);

            return formatted ? StringTransforms.FormatDelimitedString(buf, "\t") : buf;
        }

        /// <summary>
        /// Returns a <see cref="DataTable"/> with the contents of the <see cref="IDataReader"/>.  The source <see cref="IDataReader"/> will be closed after the
        /// DataTable is loaded.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="tableName">The name of the <see cref="DataTable" />.</param>
        public static DataTable ToDataTable(this IDataReader dr, string tableName = "")
        {
            var dt = new DataTable();

            if (!string.IsNullOrWhiteSpace(tableName))
            {
                dt.TableName = tableName;
            }

            dt.Load(dr);
            dr.Close();

            return dt;
        }

        /// <summary>
        /// Returns an HTML table with the contents of the IDataReader.  The header rows will be contained in TH tags.  If a css class is
        /// left blank it will not be included in the specified tag.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="tableCssClass">The CSS class for the 'table' tag.</param>
        /// <param name="tableHeaderCssClass">The CSS class for the 'th' tag.</param>
        /// <param name="tableRowCssClass">The CSS class for the 'tr' tag.</param>
        /// <param name="tableDataCssClass">The CSS class for the 'td' tag.</param>
        public static string ToHtmlTable(this IDataReader dr, string tableCssClass, string tableHeaderCssClass, string tableRowCssClass, string tableDataCssClass)
        {
            var sb = new StringBuilder();

            string tableClass = string.IsNullOrWhiteSpace(tableCssClass) ? "" : $"class=\"{tableCssClass}\"";
            string tableHeaderClass = string.IsNullOrWhiteSpace(tableHeaderCssClass) ? "" : $"class=\"{tableHeaderCssClass}\"";
            string tableRowClass = string.IsNullOrWhiteSpace(tableRowCssClass) ? "" : $"class=\"{tableRowCssClass}\"";
            string tableDataClass = string.IsNullOrWhiteSpace(tableDataCssClass) ? "" : $"class=\"{tableDataCssClass}\"";

            sb.AppendFormat("<table border=\"0\" {0}>", tableClass);
            sb.AppendFormat("<tr {0}>", tableRowClass);

            for (int x = 0; x <= dr.FieldCount - 1; x++)
            {
                sb.AppendFormat("<th {0}>{1}</th>", tableHeaderClass, dr.GetName(x));
            }

            sb.Append("</tr>");

            while (dr.Read())
            {
                sb.AppendFormat("<tr {0}>", tableRowClass);

                for (int x = 0; x <= dr.FieldCount - 1; x++)
                {
                    if (dr[x] != null && dr[x] == DBNull.Value)
                    {
                        sb.AppendFormat("<td {0}>{1}</td>", tableDataClass, dr[x]);
                    }
                    else
                    {
                        sb.AppendFormat("<td {0}></td>", tableDataClass);
                    }
                }

                sb.Append("</tr>");
            }

            sb.Append("</table>");

            // Cleanup tags if nothing was provided for the CSS classes for them.
            sb.Replace("<table border=\"0\" >", "<table border=\"0\">");
            sb.Replace("<tr >", "<tr>");
            sb.Replace("<th >", "<th>");
            sb.Replace("<td >", "<td>");

            // Get rid of the empty CSS classes
            return sb.ToString().Replace("class=\"\"", "");
        }

        /// <summary>
        /// This extension takes the current row the IDataReader is on and outputs a string array of it's values.  This expects that the DataReader is open
        /// and on the row needing to be outputted.
        /// </summary>
        /// <param name="dr"></param>
        public static string[] ToStringArray(this IDataReader dr)
        {
            var values = new string[dr.FieldCount - 1];

            for (int i = 0; i < dr.FieldCount; i++)
            {
                values[i] = dr[i].ToString();
            }

            return values;
        }

        /// <summary>
        /// Extension method to write the contents of a IDataReader out to a comma separated value file.  This extension
        /// will write the file line by line to keep the memory footprint low.
        /// </summary>
        /// <param name="dr">IDataReader that has been executed by not iterated through yet.</param>
        /// <param name="fileName">The full fill path that the CSV should be written to.</param>
        /// <param name="includeHeaderRow">Whether or not to include the header row with column names.</param>
        /// <param name="includeQuotes">Whether or not to wrap all values in double quotes.</param>
        public static void ToCsvFile(this IDataReader dr, string fileName, bool includeHeaderRow, bool includeQuotes)
        {
            ToCsvFile(dr, fileName, includeHeaderRow, includeQuotes, false);
        }

        /// <summary>
        /// Extension method to write the contents of a IDataReader out to a comma separated value file.  This extension
        /// will write the file line by line to keep the memory footprint low.
        /// </summary>
        /// <param name="dr">IDataReader that has been executed by not iterated through yet.</param>
        /// <param name="fileName">The full fill path that the CSV should be written to.</param>
        /// <param name="includeHeaderRow">Whether or not to include the header row with column names.</param>
        /// <param name="includeQuotes">Whether or not to wrap all values in double quotes.</param>
        /// <param name="convertDateTimeToShortDate">Whether or not to convert all DateTime fields to a short date time without the timestamp.</param>
        public static void ToCsvFile(this IDataReader dr, string fileName, bool includeHeaderRow, bool includeQuotes, bool convertDateTimeToShortDate)
        {
            using (var writer = new StreamWriter(fileName, false))
            {
                // Write the header row if requested
                if (includeHeaderRow)
                {
                    var headerRow = new string[dr.FieldCount];

                    for (int i = 0; i <= dr.FieldCount - 1; i++)
                    {
                        if (includeQuotes)
                        {
                            headerRow[i] = $"\"{dr.GetName(i)}\"";
                        }
                        else
                        {
                            headerRow[i] = dr.GetName(i);
                        }
                    }

                    writer.WriteLine(string.Join(",", headerRow));
                    writer.Flush();
                }

                // The rest of the rows
                while (dr.Read())
                {
                    var row = new string[dr.FieldCount];

                    for (int i = 0; i <= dr.FieldCount - 1; i++)
                    {
                        string value;

                        try
                        {
                            // If the caller wants to convert all the date times to a short date then check the type and
                            // convert if it's a DateTime, otherwise just dump the string.
                            if (convertDateTimeToShortDate && dr[i] is DateTime)
                            {
                                value = DateTime.Parse(dr[i].ToString()).ToShortDateString();
                            }
                            else
                            {
                                value = dr[i].ToString();
                            }
                        }
                        catch
                        {
                            // Eat the the hierarchyid error
                            value = string.Empty;
                        }

                        // Escape tabs and new line characters if they exist, we will unescape them on the other side.
                        if (includeQuotes)
                        {
                            row[i] = $"\"{value.Replace("\t", "\\t").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\"\"")}\"";
                        }
                        else
                        {
                            row[i] = value.Replace("\t", "\\t").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\"\"");
                        }
                    }

                    writer.WriteLine(string.Join(",", row));
                    writer.Flush();
                }
            }
        }

        /// <summary>
        /// Extension method to write the contents of a IDataReader out to a comma separated value string.
        /// </summary>
        /// <param name="dr">IDataReader that has been executed by not iterated through yet.</param>
        /// <param name="includeHeaderRow">Whether or not to include the header row with column names.</param>
        /// <param name="includeQuotes">Whether or not to wrap all values in double quotes.</param>
        public static string ToCsvString(this IDataReader dr, bool includeHeaderRow, bool includeQuotes)
        {
            return ToCsvString(dr, includeHeaderRow, includeQuotes, false);
        }

        /// <summary>
        /// Extension method to write the contents of a IDataReader out to a comma separated value string.
        /// </summary>
        /// <param name="dr">IDataReader that has been executed by not iterated through yet.</param>
        /// <param name="includeHeaderRow">Whether or not to include the header row with column names.</param>
        /// <param name="includeQuotes">Whether or not to wrap all values in double quotes.</param>
        /// <param name="convertDateTimeToShortDate">Whether or not to convert all DateTime values to a short date time.</param>
        public static string ToCsvString(this IDataReader dr, bool includeHeaderRow, bool includeQuotes, bool convertDateTimeToShortDate)
        {
            var sb = new StringBuilder();

            // Write the header row if requested
            if (includeHeaderRow)
            {
                var headerRow = new string[dr.FieldCount];

                for (int i = 0; i <= dr.FieldCount - 1; i++)
                {
                    if (includeQuotes)
                    {
                        headerRow[i] = $"\"{dr.GetName(i)}\"";
                    }
                    else
                    {
                        headerRow[i] = dr.GetName(i);
                    }
                }

                sb.Append(string.Join(",", headerRow));
                sb.AppendLine();
            }

            // The rest of the rows
            while (dr.Read())
            {
                var row = new string[dr.FieldCount];

                for (int i = 0; i <= dr.FieldCount - 1; i++)
                {
                    string value;

                    try
                    {
                        // If the caller wants to convert all the date times to a short date then check the type and
                        // convert if it's a DateTime, otherwise just dump the string.
                        if (convertDateTimeToShortDate && dr[i] is DateTime)
                        {
                            value = DateTime.Parse(dr[i].ToString()).ToShortDateString();
                        }
                        else
                        {
                            value = dr[i].ToString();
                        }
                    }
                    catch
                    {
                        // Eat the the hierarchyid error
                        value = "";
                    }

                    // Escape tabs and new line characters if they exist, we will unescape them on the other side.
                    if (includeQuotes)
                    {
                        row[i] = $"\"{value.Replace("\t", "\\t").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\"\"")}\"";
                    }
                    else
                    {
                        row[i] = value.Replace("\t", "\\t").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\"\"");
                    }
                }

                sb.Append(string.Join(",", row));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Extension method to write the contents of a IDataReader out to a tab separated value file.  This extension
        /// will write the file line by line to keep the memory footprint low.
        /// </summary>
        /// <param name="dr">IDataReader that has been executed by not iterated through yet.</param>
        /// <param name="fileName">The full fill path that the CSV should be written to.</param>
        /// <param name="includeHeaderRow">Whether or not to include the header row with column names.</param>
        public static void ToTabFile(this IDataReader dr, string fileName, bool includeHeaderRow)
        {
            ToTabFile(dr, fileName, includeHeaderRow, false);
        }

        /// <summary>
        /// Extension method to write the contents of a IDataReader out to a tab separated value file.  This extension
        /// will write the file line by line to keep the memory footprint low.
        /// </summary>
        /// <param name="dr">IDataReader that has been executed by not iterated through yet.</param>
        /// <param name="fileName">The full fill path that the CSV should be written to.</param>
        /// <param name="includeHeaderRow">Whether or not to include the header row with column names.</param>
        /// <param name="convertDateTimeToShortDate">Whether or not to convert all DateTime values to a short date time.</param>
        public static void ToTabFile(this IDataReader dr, string fileName, bool includeHeaderRow, bool convertDateTimeToShortDate)
        {
            using (var writer = new StreamWriter(fileName, false))
            {
                // Write the header row if requested
                if (includeHeaderRow)
                {
                    var headerRow = new string[dr.FieldCount];

                    for (int i = 0; i <= dr.FieldCount - 1; i++)
                    {
                        headerRow[i] = dr.GetName(i);
                    }

                    writer.WriteLine(string.Join("\t", headerRow));
                    writer.Flush();
                }

                // The rest of the rows
                while (dr.Read())
                {
                    var row = new string[dr.FieldCount];

                    for (int i = 0; i <= dr.FieldCount - 1; i++)
                    {
                        string value;

                        try
                        {
                            // If the caller wants to convert all the date times to a short date then check the type and
                            // convert if it's a DateTime, otherwise just dump the string.
                            if (convertDateTimeToShortDate && dr[i] is DateTime)
                            {
                                value = DateTime.Parse(dr[i].ToString()).ToShortDateString();
                            }
                            else
                            {
                                value = dr[i].ToString();
                            }
                        }
                        catch
                        {
                            // Eat the the hierarchyid error
                            value = "";
                        }

                        // Escape tabs and new line characters if they exist, we will unescape them on the other side.
                        row[i] = value.Replace("\t", "\\t").Replace("\n", "\\n").Replace("\r", "\\r");
                    }

                    writer.WriteLine(string.Join("\t", row));
                    writer.Flush();
                }
            }
        }

        /// <summary>
        /// Extension method to write the contents of a IDataReader out to a tab separated value string.
        /// </summary>
        /// <param name="dr">IDataReader that has been executed by not iterated through yet.</param>
        /// <param name="includeHeaderRow">Whether or not to include the header row with column names.</param>
        public static string ToTabString(this IDataReader dr, bool includeHeaderRow)
        {
            return ToTabString(dr, includeHeaderRow, false);
        }

        /// <summary>
        /// Extension method to write the contents of a IDataReader out to a tab separated value string.
        /// </summary>
        /// <param name="dr">IDataReader that has been executed by not iterated through yet.</param>
        /// <param name="includeHeaderRow">Whether or not to include the header row with column names.</param>
        /// <param name="convertDateTimeToShortDate">Whether or not to convert all DateTime values to a short date time.</param>
        public static string ToTabString(this IDataReader dr, bool includeHeaderRow, bool convertDateTimeToShortDate)
        {
            var sb = new StringBuilder();

            // Write the header row if requested
            if (includeHeaderRow)
            {
                var headerRow = new string[dr.FieldCount];

                for (int i = 0; i <= dr.FieldCount - 1; i++)
                {
                    headerRow[i] = dr.GetName(i);
                }

                sb.Append(string.Join("\t", headerRow));
                sb.AppendLine();
            }

            // The rest of the rows
            while (dr.Read())
            {
                var row = new string[dr.FieldCount];

                for (int i = 0; i <= dr.FieldCount - 1; i++)
                {
                    string value;

                    try
                    {
                        // If the caller wants to convert all the date times to a short date then check the type and
                        // convert if it's a DateTime, otherwise just dump the string.
                        if (convertDateTimeToShortDate && dr[i] is DateTime)
                        {
                            value = DateTime.Parse(dr[i].ToString()).ToShortDateString();
                        }
                        else
                        {
                            value = dr[i].ToString();
                        }
                    }
                    catch
                    {
                        // Eat the the hierarchyid error
                        value = "";
                    }

                    row[i] = value.Replace("\t", "\\t").Replace("\n", "\\n").Replace("\r", "\\r");
                }

                sb.Append(string.Join("\t", row));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Return the current row in the reader as an object
        /// </summary>
        /// <param name="dr">The IDataReader, open and on the row that should be mapped into a model.</param>
        /// <param name="model">The model the current data row should be mapped into.</param>
        /// <returns>Object</returns>
        public static T ToModel<T>(this IDataReader dr, T model)
        {
            // Create a new instance of T so the values are all the default.
            model = (T) Activator.CreateInstance(model.GetType());

            // Get all the properties in our Object
            var props = model.GetType().GetProperties();

            // For each property get the data from the reader to the object
            for (int i = 0; i < props.Length; i++)
            {
                if (ColumnExists(dr, props[i].Name) && dr[props[i].Name] != DBNull.Value)
                {
                    model.GetType().InvokeMember(props[i].Name, BindingFlags.SetProperty, null, model, new[] {dr[props[i].Name]});
                }
            }

            return model;
        }

        /// <summary>
        /// Return the current row in the reader as an object
        /// </summary>
        /// <param name="dr">The IDataReader, open and on the row that should be mapped into a model.</param>
        /// <returns>Object</returns>
        public static T ToModel<T>(this IDataReader dr)
        {
            // Create a new instance of T so the values are all the default.
            var model = (T) Activator.CreateInstance(typeof(T));

            // Get all the properties in our Object
            var props = model.GetType().GetProperties();

            // For each property get the data from the reader to the object
            for (int i = 0; i < props.Length; i++)
            {
                if (ColumnExists(dr, props[i].Name) && dr[props[i].Name] != DBNull.Value)
                {
                    model.GetType().InvokeMember(props[i].Name, BindingFlags.SetProperty, null, model, new[] {dr[props[i].Name]});
                }
            }

            return model;
        }

        /// <summary>
        /// Check if an SqlDataReader contains a field
        /// </summary>
        /// <param name="dr">The IDataReader</param>
        /// <param name="columnName">The column name that should be checked for existence.</param>
        public static bool ColumnExists(this IDataReader dr, string columnName)
        {
            if (dr == null)
            {
                return false;
            }

            dr.GetSchemaTable().DefaultView.RowFilter = $"ColumnName= '{columnName}'";

            return dr.GetSchemaTable().DefaultView.Count > 0;
        }
    }
}