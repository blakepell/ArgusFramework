/*
 * @author            : Blake Pell, Dave Demuelenaere
 * @website           : http://www.blakepell.com
 * @initial date      : 2016-04-27
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Argus.Extensions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Argus.Office.Excel
{
    /// <summary>
    /// Excel Spreadsheet creation from various sources including DataReader's, DataTable's and IEnumerable.
    /// </summary>
    /// <remarks>
    /// This class uses and requires Microsoft's OpenXml library which is included via a Nuget feed.
    /// </remarks>
    public class Spreadsheet : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="destinationPath">The location where the file will be stored.</param>
        public Spreadsheet(string destinationPath)
        {
            // Create the new spreadsheet
            this.ExcelDocument = SpreadsheetDocument.Create(destinationPath, SpreadsheetDocumentType.Workbook);

            // Create the Workbook for the spreadsheet
            this.ExcelDocument.AddWorkbookPart();

            // Create the writer that will handle the outer portion of the spreadsheet, it will need to have
            // these tags closed out when the spreadsheet is closed.
            this.Writer = OpenXmlWriter.Create(this.ExcelDocument.WorkbookPart);
            this.Writer.WriteStartElement(new Workbook());
            this.Writer.WriteStartElement(new Sheets());
        }

        /// <summary>
        /// Provides a key/pair lookup to swap out database column names for a friendly names.  The key should correspond
        /// to the field name the query returns and the value should be the name to swap in it's place.  E.g. a key of
        /// "log_time" might map to "Log Time".
        /// </summary>
        public Dictionary<string, string> ColumnHeaderMap { get; set; }

        private int SheetId { get; set; }

        /// <summary>
        /// The underlying spreadsheet document.
        /// </summary>
        public SpreadsheetDocument ExcelDocument { get; set; }

        /// <summary>
        /// This is the OpenXmlWriter that will handle the outer portion of the spreadsheet.
        /// </summary>
        private OpenXmlWriter Writer { get; }

        /// <summary>
        /// IDisposable implementation.
        /// </summary>
        public void Dispose()
        {
            if (this.Writer != null)
            {
                // this is for Sheets
                this.Writer.WriteEndElement();

                // this is for Workbook
                this.Writer.WriteEndElement();
                this.Writer.Close();
                this.Writer.Dispose();
            }

            this.ExcelDocument.Close();
            this.ExcelDocument.Dispose();
        }

        /// <summary>
        /// Adds a sheet to the current spreadsheet from an IDataReader (this writes via a stream and supports
        /// large spreadsheets).
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="workSheetName"></param>
        public void AddSheet(IDataReader dr, string workSheetName)
        {
            // Validate the sheet name to make sure it's not illegal
            this.ValidateWorksheetName(workSheetName);

            // Increment the sheet number this is
            this.SheetId += 1;

            // If they didn't provide a sheet name put in the default name with the incremented number behind it
            // like Excel would do.
            if (string.IsNullOrWhiteSpace(workSheetName))
            {
                workSheetName = $"Sheet{this.SheetId.ToString()}";
            }

            // Create the writer that we're going to use.. it will write data into the parts of the spreadsheet
            // which we will then write into the Spreadsheet.
            List<OpenXmlAttribute> oxa;
            OpenXmlWriter oxw;

            var wsp = this.ExcelDocument.WorkbookPart.AddNewPart<WorksheetPart>();

            oxw = OpenXmlWriter.Create(wsp);
            oxw.WriteStartElement(new Worksheet());
            oxw.WriteStartElement(new SheetData());

            // Header Row
            int index = 1;

            {
                oxa = new List<OpenXmlAttribute>();
                // this is the row index
                oxa.Add(new OpenXmlAttribute("r", null, index.ToString()));

                // This is for the row
                oxw.WriteStartElement(new Row(), oxa);

                for (int x = 0; x <= dr.FieldCount - 1; x++)
                {
                    oxa = new List<OpenXmlAttribute>();
                    // this is the data type ("t"), with CellValues.String ("str")
                    oxa.Add(new OpenXmlAttribute("t", null, "str"));

                    var cell = GetCell(typeof(string), this.ColumnHeaderLookup(dr.GetName(x)));
                    oxw.WriteElement(cell);
                }

                // This is for the row
                oxw.WriteEndElement();
            }

            // Add a row for each data item.
            while (dr.Read())
            {
                index += 1;

                oxa = new List<OpenXmlAttribute>();
                // this is the row index
                oxa.Add(new OpenXmlAttribute("r", null, index.ToString()));

                // This is for the row
                oxw.WriteStartElement(new Row(), oxa);

                // Add value for each field in the DataReader.
                for (int x = 0; x <= dr.FieldCount - 1; x++)
                {
                    oxa = new List<OpenXmlAttribute>();
                    // this is the data type ("t"), with CellValues.String ("str")
                    oxa.Add(new OpenXmlAttribute("t", null, "str"));

                    var cell = GetCell(dr[x].GetType(), dr[x].ToString());
                    oxw.WriteElement(cell);
                }

                // this is for Row
                oxw.WriteEndElement();
            }


            // this is for SheetData
            oxw.WriteEndElement();

            // this is for Worksheet
            oxw.WriteEndElement();
            oxw.Close();
            oxw.Dispose();

            // you can use object initializers like this only when the properties
            // are actual properties. SDK classes sometimes have property-like properties
            // but are actually classes. For example, the Cell class has the CellValue
            // "property" but is actually a child class internally.
            // If the properties correspond to actual XML attributes, then you're fine.

            // Writer this into the global Writer we have open.
            this.Writer.WriteElement(new Sheet
            {
                Name = $"{workSheetName}",
                SheetId = (uint) this.SheetId,
                Id = this.ExcelDocument.WorkbookPart.GetIdOfPart(wsp)
            });
        }

        ///// TODO - Implement this later, allows each row of a data reader to be transformed by a custom object.
        ///// <summary>
        ///// Add's a sheet to the current spreadsheet from an IDataReader.
        ///// </summary>
        ///// <param name="dr"></param>
        ///// <param name="workSheetName"></param>
        ///// <param name="transformModel">A model that contains custom setters used to transform returned data before writing to the cells of the spreadsheet.</param>
        //public void AddSheet(IDataReader dr, string workSheetName, object transformModel)
        //{
        //    ValidateWorksheetName(workSheetName);

        //    SheetData sheetData = new SheetData();

        //    // Add the fields into the header row from the names of the columsn in the IDataReader
        //    Row headerRow = new Row();

        //    // Add value for each property in the model
        //    foreach (PropertyInfo pi in transformModel.GetType().GetProperties())
        //    {
        //        if (pi.CanRead)
        //        {
        //            Cell cell = GetCell(pi.Name.GetType(), pi.Name);
        //            headerRow.AppendChild(cell);
        //        }
        //    }

        //    sheetData.AppendChild(headerRow);

        //    // Add a row for each data item.
        //    while (dr.Read())
        //    {
        //        Row newRow = new Row();

        //        transformModel = dr.ToModel(transformModel);

        //        // Add value for each property in the model
        //        foreach (PropertyInfo pi in transformModel.GetType().GetProperties())
        //        {
        //            if (pi.CanRead)
        //            {
        //                object value = pi.GetValue(transformModel, null);

        //                if (value != null)
        //                {
        //                    Cell cell = GetCell(value.GetType(), value.ToString());
        //                    newRow.AppendChild(cell);
        //                }
        //                else
        //                {
        //                    Cell cell = GetCell("".GetType(), "");
        //                    newRow.AppendChild(cell);
        //                }

        //            }

        //        }

        //        sheetData.AppendChild(newRow);
        //    }

        //    AddSheet(sheetData, workSheetName);
        //}

        /// <summary>
        /// Adds a sheet to the current spreadsheet from a DataTable.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="workSheetName"></param>
        public void AddSheet(DataTable dt, string workSheetName)
        {
            this.AddSheet(dt.CreateDataReader(), workSheetName);
        }

        /// <summary>
        /// Adds a sheet based off on an IEnumerable list.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="workSheetName"></param>
        public void AddSheet<T>(IEnumerable<T> data, string workSheetName) where T : class
        {
            this.ValidateWorksheetName(workSheetName);

            // Increment the sheet number this is
            this.SheetId += 1;

            // If they didn't provide a sheet name put in the default name with the incrimented number behind it
            // like Excel would do.
            if (string.IsNullOrWhiteSpace(workSheetName))
            {
                workSheetName = $"Sheet{this.SheetId.ToString()}";
            }

            var dataProperties = (data.FirstOrDefault().GetType() ?? typeof(T)).GetProperties();

            // Create the writer that we're going to use.. it will write data into the parts of the spreadsheet
            // which we will then write into the Spreadsheet.
            List<OpenXmlAttribute> oxa;
            OpenXmlWriter oxw;

            var wsp = this.ExcelDocument.WorkbookPart.AddNewPart<WorksheetPart>();

            oxw = OpenXmlWriter.Create(wsp);
            oxw.WriteStartElement(new Worksheet());
            oxw.WriteStartElement(new SheetData());

            // Header Row
            int index = 1;

            {
                oxa = new List<OpenXmlAttribute>();
                // this is the row index
                oxa.Add(new OpenXmlAttribute("r", null, index.ToString()));

                // This is for the row
                oxw.WriteStartElement(new Row(), oxa);

                foreach (var property in dataProperties)
                {
                    oxa = new List<OpenXmlAttribute>();
                    // this is the data type ("t"), with CellValues.String ("str")
                    oxa.Add(new OpenXmlAttribute("t", null, "str"));

                    var cell = GetCell(typeof(string), this.ColumnHeaderLookup(property.Name));
                    oxw.WriteElement(cell);
                }

                // This is for the row
                oxw.WriteEndElement();
            }

            foreach (var item in data)
            {
                index += 1;

                oxa = new List<OpenXmlAttribute>();
                // this is the row index
                oxa.Add(new OpenXmlAttribute("r", null, index.ToString()));

                // This is for the row
                oxw.WriteStartElement(new Row(), oxa);

                // Add value for each property.
                foreach (var property in dataProperties)
                {
                    oxa = new List<OpenXmlAttribute>();
                    // this is the data type ("t"), with CellValues.String ("str")
                    oxa.Add(new OpenXmlAttribute("t", null, "str"));

                    var cell = GetCell(property.PropertyType, property?.GetValue(item, null)?.ToString() ?? "");
                    oxw.WriteElement(cell);
                }

                // this is for Row
                oxw.WriteEndElement();
            }

            // this is for SheetData
            oxw.WriteEndElement();

            // this is for Worksheet
            oxw.WriteEndElement();
            oxw.Close();
            oxw.Dispose();

            // you can use object initializers like this only when the properties
            // are actual properties. SDK classes sometimes have property-like properties
            // but are actually classes. For example, the Cell class has the CellValue
            // "property" but is actually a child class internally.
            // If the properties correspond to actual XML attributes, then you're fine.

            // Writer this into the global Writer we have open.
            this.Writer.WriteElement(new Sheet
            {
                Name = $"{workSheetName}",
                SheetId = (uint) this.SheetId,
                Id = this.ExcelDocument.WorkbookPart.GetIdOfPart(wsp)
            });
        }

        /// <summary>
        /// Adds a sheet based off <see cref="SheetData" />.
        /// </summary>
        /// <param name="sheetData"></param>
        /// <param name="workSheetName"></param>
        /// <remarks>Since this SheetData is all in memory it will not support large spreadsheets.</remarks>
        public void AddSheet(SheetData sheetData, string workSheetName)
        {
            this.ValidateWorksheetName(workSheetName);

            // Incriment the sheet number this is
            this.SheetId += 1;

            // If they didn't provide a sheet name put in the default name with the incremented number behind it
            // like Excel would do.
            if (string.IsNullOrWhiteSpace(workSheetName))
            {
                workSheetName = $"Sheet{this.SheetId.ToString()}";
            }

            // Create the writer that we're going to use.. it will write data into the parts of the spreadsheet
            // which we will then write into the Spreadsheet.
            OpenXmlWriter oxw;

            var wsp = this.ExcelDocument.WorkbookPart.AddNewPart<WorksheetPart>();

            oxw = OpenXmlWriter.Create(wsp);
            oxw.WriteStartElement(new Worksheet());
            oxw.WriteElement(sheetData);

            // this is for Worksheet
            oxw.WriteEndElement();
            oxw.Close();
            oxw.Dispose();

            // you can use object initializers like this only when the properties
            // are actual properties. SDK classes sometimes have property-like properties
            // but are actually classes. For example, the Cell class has the CellValue
            // "property" but is actually a child class internally.
            // If the properties correspond to actual XML attributes, then you're fine.

            // Writer this into the global Writer we have open.
            this.Writer.WriteElement(new Sheet
            {
                Name = $"{workSheetName}",
                SheetId = (uint) this.SheetId,
                Id = this.ExcelDocument.WorkbookPart.GetIdOfPart(wsp)
            });
        }

        /// <summary>
        /// Adds a blank sheet with a specified name
        /// </summary>
        public void AddSheet(string workSheetName)
        {
            var sd = new SheetData();
            this.AddSheet(sd, workSheetName);
        }

        /// <summary>
        /// Adds a blank sheet in with the default name.
        /// </summary>
        public void AddSheet()
        {
            // Passing a blank through will make it default it's name to "Sheet" then the
            // sheet number that next.
            this.AddSheet("");
        }

        /// <summary>
        /// Returns a spreadsheet <see cref="Cell" /> with its type set according to the .NET type of the data.
        /// </summary>
        /// <param name="type">A .NET data type.</param>
        /// <param name="value">The CellValue for the returned <see cref="Cell" /></param>
        private static Cell GetCell(Type type, string value)
        {
            var cell = new Cell();

            // Return whatever the object is, the library will then code that cell appropriately System.RuntimeType
            // is an internal .Net class that sometimes can be returned and if it is needs to
            // be handled specifically because it will throw an exception if put directly into a cell.
            if (type.ToString() == "System.RuntimeType")
            {
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(value.SafeLeft(32767));

                return cell;
            }

            // The 'TypeCode' enum doesn't have 'Guid'.
            if (type.ToString() == "System.Guid")
            {
                Guid guidResult;
                Guid.TryParse(value, out guidResult);
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(guidResult.ToString());

                return cell;
            }

            // Make sure the value isn't null before putting it into the cell.
            // If it is null, put a blank in the cell.
            if (value == null || Convert.IsDBNull(value))
            {
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue("");

                return cell;
            }

            var typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.String:
                    // Most common case so we'll include it first
                    cell.DataType = CellValues.String;

                    // `ToValidXmlAsciiCharacters` will remove any invalid XML characters falling in the ascii code range of 0-32
                    cell.CellValue = new CellValue(value.SafeLeft(32767).ToValidXmlAsciiCharacters());

                    break;

                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    // Second most common cases
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(value);

                    break;

                case TypeCode.DateTime:
                    // It expects a string here, due to a bug in the OpenXml library we're going to create this as
                    // a string that's formatted for sorting (that can then be converted in Excel if need be).
                    var dt = Convert.ToDateTime(value).Date;
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue($"{dt.Year}/{dt.MonthTwoCharacters()}/{dt.DayTwoCharacters()}");

                    break;
                default:
                    // Everything else
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(value);

                    break;
            }

            return cell;
        }

        /// <summary>
        /// Checks whether a worksheet name is valid or not, if a worksheet name is invalid an Exception will
        /// be thrown with information as to why the name is invalid.  If the worksheet name is valid this method
        /// will silently succeed.
        /// </summary>
        /// <remarks>
        /// The underlying Excel library will fail with a non descript error message if any of these rules are
        /// violated, this attempts to isolate those and let the caller know which rule they broke.
        /// </remarks>
        /// <param name="worksheetName"></param>
        private void ValidateWorksheetName(string worksheetName)
        {
            if (worksheetName.Length > 31)
            {
                throw new Exception($"Maximum length of worksheet name exceeded:  A worksheet name must be 31 or less characters.  You attempted to create a worksheet name that was {worksheetName.Length.ToString()} characters long.");
            }

            if (worksheetName.IndexOfAny(new[] {'\\', '/', '*', '[', ']', ':', '?'}) != -1)
            {
                throw new Exception("Invalid worksheet name:  You attempted to create a worksheet name that contains an invalid character");
            }
        }

        /// <summary>
        /// Checks whether a worksheet name is valid or not.
        /// </summary>
        /// <param name="worksheetName"></param>
        public static bool WorksheetNameIsValid(string worksheetName)
        {
            if (worksheetName.Length > 31 || worksheetName.IndexOfAny(new[] {'\\', '/', '*', '[', ']', ':', '?'}) != -1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Private lookup for the column header map that checks for whether it's set (not null) and then
        /// returns either the found mapped value or the passed in key if it is not found.
        /// </summary>
        /// <param name="key"></param>
        private string ColumnHeaderLookup(string key)
        {
            if (this.ColumnHeaderMap != null)
            {
                if (this.ColumnHeaderMap.ContainsKey(key))
                {
                    return this.ColumnHeaderMap[key];
                }
            }

            return key;
        }

        /// <summary>
        /// Closes and disposes of all resources.  The object will need to be re-instantiated after this usage.
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }
    }
}