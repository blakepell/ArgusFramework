/*
 * @author            : Blake Pell
 * @initial date      : 2009-06-30
 * @last updated      : 2021-02-06
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

namespace Argus.Data
{
    /// <summary>
    /// Class with shared functions for generating test data.
    /// </summary>
    public static class TestData
    {
        /// <summary>
        /// Returns a specified number of records as a DataTable.  The DataTable's fields are 'Guid' of System.String, 'Random Number' of
        /// System.Int32 and 'Date' of System.DataTime.
        /// </summary>
        /// <param name="numberOfRecords"></param>
        public static DataTable GetTestDataTable(int numberOfRecords)
        {
            var rnd = new Random();
            var dt = new DataTable("Test");
            dt.Columns.Add("Guid", Type.GetType("System.String", true, true));
            dt.Columns.Add("Random Number", Type.GetType("System.Int32", true, true));
            dt.Columns.Add("Date", Type.GetType("System.DateTime", true, true));

            for (int x = 1; x <= numberOfRecords; x++)
            {
                dt.Rows.Add(Guid.NewGuid().ToString(), rnd.Next(99999), DateTime.Now);
            }

            return dt;
        }

        /// <summary>
        /// Returns a specified number of records as a IDataReader.  The IDataReader's fields are 'Guid' of System.String, 'Random Number' of
        /// System.Int32 and 'Date' of System.DataTime.
        /// </summary>
        /// <param name="numberOfRecords"></param>
        public static IDataReader GetTestDataReader(int numberOfRecords)
        {
            var dt = GetTestDataTable(numberOfRecords);
            return dt.CreateDataReader();
        }

        /// <summary>
        /// Returns a Generic List of strings with the specified number of records.  The strings are randomly generated guid's.
        /// </summary>
        /// <param name="numberOfRecords"></param>
        public static List<string> GetGenericList(int numberOfRecords)
        {
            var lst = new List<string>();

            for (int x = 1; x <= numberOfRecords; x++)
            {
                lst.Add(Guid.NewGuid().ToString());
            }

            return lst;
        }

        /// <summary>
        /// Returns a specified number of records as a string.  The strings fields are 'Guid' of System.String, 'Random Number' of
        /// System.Int32 and 'Date' of System.DataTime.  The default delimiter is a tab but can be overriden.
        /// </summary>
        /// <param name="numberOfRecords"></param>
        /// <param name="delimiter"></param>
        public static string GetFlatFile(int numberOfRecords, char delimiter = '\t')
        {
            var rnd = new Random();
            var sb = new StringBuilder();

            for (int x = 1; x <= numberOfRecords; x++)
            {
                sb.AppendFormat("{1}{0}{2}{0}{3}\r\n", delimiter, Guid.NewGuid().ToString(), rnd.Next(10000), DateTime.Now);
            }

            return sb.ToString();
        }
    }
}