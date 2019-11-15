﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Argus.Data
{

    /// <summary>
    /// Class with shared functions for generating test data.
    /// </summary>
    /// <remarks></remarks>
    public class TestData
    {
        //*********************************************************************************************************************
        //
        //             Class:  TestData
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  06/30/2009
        //      Last Updated:  09/18/2017
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Returns a specified number of records as a DataTable.  The DataTable's fields are 'Guid' of System.String, 'Random Number' of
        /// System.Int32 and 'Date' of System.DataTime.
        /// </summary>
        /// <param name="numberOfRecords"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTable GetTestDataTable(int numberOfRecords)
        {
            Random rnd = new Random();
            DataTable dt = new DataTable("Test");
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
        /// <returns></returns>
        /// <remarks></remarks>
        public static IDataReader GetTestDataReader(int numberOfRecords)
        {
            DataTable dt = GetTestDataTable(numberOfRecords);
            return dt.CreateDataReader();
        }

        /// <summary>
        /// Returns a Generic List of strings with the specified number of records.  The strings are randomly generated guid's.
        /// </summary>
        /// <param name="numberOfRecords"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> GetGenericList(int numberOfRecords)
        {
            List<string>lst = new List<string>();

            for (int x = 1; x <= numberOfRecords; x++)
            {
                lst.Add(Guid.NewGuid().ToString());
            }

            return lst;
        }

        /// <summary>
        /// Returns a specified number of records as a string.  The strings fields are 'Guid' of System.String, 'Random Number' of
        /// System.Int32 and 'Date' of System.DataTime.  The string will be tab delimited.
        /// </summary>
        /// <param name="numberOfRecords"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetFlatFile(int numberOfRecords)
        {
            Random rnd = new Random();
            StringBuilder sb = new StringBuilder();

            for (int x = 1; x <= numberOfRecords; x++)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\r\n", Guid.NewGuid().ToString(), rnd.Next(10000), DateTime.Now);
            }

            return sb.ToString();
        }

    }

}