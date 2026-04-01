/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System.Data;
using Xunit;

namespace Argus.UnitTests
{
    public class DataTableTests
    {
        [Fact]
        public void ReadOnly_SetsAllColumnsReadOnly()
        {
            var dt = new DataTable();
            dt.Columns.Add("Col1", typeof(string));
            dt.Columns.Add("Col2", typeof(int));

            dt.ReadOnly(true);

            Assert.True(dt.Columns["Col1"]!.ReadOnly);
            Assert.True(dt.Columns["Col2"]!.ReadOnly);
        }

        [Fact]
        public void ReadOnly_UnsetsAllColumnsReadOnly()
        {
            var dt = new DataTable();
            dt.Columns.Add("Col1", typeof(string));
            dt.Columns.Add("Col2", typeof(int));

            dt.ReadOnly(true);
            dt.ReadOnly(false);

            Assert.False(dt.Columns["Col1"]!.ReadOnly);
            Assert.False(dt.Columns["Col2"]!.ReadOnly);
        }

        [Fact]
        public void SelectDuplicates_ReturnsDuplicateValues_Distinct()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Rows.Add(1);
            dt.Rows.Add(2);
            dt.Rows.Add(1);
            dt.Rows.Add(3);
            dt.Rows.Add(2);

            var duplicates = dt.SelectDuplicates("Id", true);

            Assert.Equal(2, duplicates.Count);
            Assert.Contains(1, duplicates);
            Assert.Contains(2, duplicates);
        }

        [Fact]
        public void SelectDuplicates_ReturnsDuplicateValues_All()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Rows.Add(1);
            dt.Rows.Add(2);
            dt.Rows.Add(1);
            dt.Rows.Add(3);
            dt.Rows.Add(2);

            var duplicates = dt.SelectDuplicates("Id", false);

            // 1 appears twice, 2 appears twice = 4 total entries
            Assert.Equal(4, duplicates.Count);
        }

        [Fact]
        public void SelectDuplicates_NoDuplicates_ReturnsEmpty()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Rows.Add(1);
            dt.Rows.Add(2);
            dt.Rows.Add(3);

            var duplicates = dt.SelectDuplicates("Id", true);

            Assert.Empty(duplicates);
        }
    }
}
