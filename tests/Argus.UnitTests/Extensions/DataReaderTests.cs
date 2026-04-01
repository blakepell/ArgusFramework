/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System.Data;
using Xunit;

namespace Argus.UnitTests
{
    public class DataReaderTests
    {
        [Fact]
        public void ToDataTable_ConvertsDataReaderToDataTable()
        {
            var dt = new DataTable("Source");
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Rows.Add(1, "Alice");
            dt.Rows.Add(2, "Bob");

            using var reader = dt.CreateDataReader();
            var result = reader.ToDataTable("Result");

            Assert.Equal("Result", result.TableName);
            Assert.Equal(2, result.Rows.Count);
            Assert.Equal(2, result.Columns.Count);
        }

        [Fact]
        public void ToDataTable_EmptyTableName_NoTableNameSet()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));

            using var reader = dt.CreateDataReader();
            var result = reader.ToDataTable();

            Assert.Equal(0, result.Rows.Count);
        }

        [Fact]
        public void GetValue_NullReader_ReturnsDefault()
        {
            IDataReader dr = null;

            var result = dr.GetValue("field", "default");

            Assert.Equal("default", result);
        }
    }
}
