/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-12-13
 * @last updated      : 2019-03-13
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Data;
using System.Windows.Forms;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DataGridView" />.
    /// </summary>
    public static class DataGridViewExtensions
    {
        /// <summary>
        /// Converts a data grid view that's not databound (and one that is databound) into a data table.  If it's already data bound, there maybe an more efficient way to get a data table structure (e.g., if it's already bound to a data table).
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="exportHeader"></param>
        /// <param name="tablename"></param>
        public static DataTable DataGridViewToDataTable(this DataGridView dgv, bool exportHeader, string tablename)
        {
            var dt = new DataTable(tablename);

            if (exportHeader)
            {
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    dt.Columns.Add(column.Name, Type.GetType("System.String"));
                }
            }
            else
            {
                if (dgv.Columns.Count > 1)
                {
                    int columnCount = 1;

                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        dt.Columns.Add("Field" + columnCount, Type.GetType("System.String"));
                        columnCount += 1;
                    }
                }
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {
                int columnCount = 0;
                dynamic newRow = dt.NewRow();

                foreach (DataGridViewCell Cell in row.Cells)
                {
                    newRow.Item[columnCount] = Cell.Value;
                    columnCount = columnCount + 1;
                }

                dt.Rows.Add(newRow);
            }

            return dt;
        }
    }
}