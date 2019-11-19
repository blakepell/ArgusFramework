using System;
using System.Collections;
using System.Windows.Forms;
using Argus.Extensions;

namespace Argus.Windows.Forms.Controls
{
    /// <summary>
    ///     Utility comparer that allows a WinForms ListView control to be sorted by column.
    /// </summary>
    /// <remarks></remarks>
    public class ListViewColumnSorter : IComparer
    {
        //*********************************************************************************************************************
        //
        //             Class:  ListViewColumnSorter
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  06/02/2008
        //      Last Updated:  03/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Object Compare
        /// </summary>
        public CaseInsensitiveComparer ObjectCompare { get; set; } = new CaseInsensitiveComparer();

        /// <summary>
        ///     The sort column.
        /// </summary>
        public int SortColumn { get; set; } = 0;

        /// <summary>
        ///     The sort order.
        /// </summary>
        public SortOrder Order { get; set; } = SortOrder.None;

        /// <summary>
        ///     Compares to values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public int Compare(object x, object y)
        {
            int compareResult;
            var listViewX = (ListViewItem) x;
            var listViewY = (ListViewItem) y;

            // Compare the two items.
            if (listViewX.SubItems[this.SortColumn].Text.IsDateTime() && listViewY.SubItems[this.SortColumn].Text.IsDateTime())
            {
                compareResult = this.ObjectCompare.Compare(Convert.ToDateTime(listViewX.SubItems[this.SortColumn].Text), Convert.ToDateTime(listViewY.SubItems[this.SortColumn].Text));
            }
            else if (listViewX.SubItems[this.SortColumn].Text.IsNumeric() && listViewY.SubItems[this.SortColumn].Text.IsNumeric())
            {
                compareResult = this.ObjectCompare.Compare(Convert.ToDouble(listViewX.SubItems[this.SortColumn].Text), Convert.ToDouble(listViewY.SubItems[this.SortColumn].Text));
            }
            else
            {
                compareResult = this.ObjectCompare.Compare(listViewX.SubItems[this.SortColumn].Text, listViewY.SubItems[this.SortColumn].Text);
            }

            // Calculate the correct return value based on the object comparison.
            if (this.Order == SortOrder.Ascending)
            {
                // Ascending sort is selected, return typical result of compare operation.
                return compareResult;
            }

            if (this.Order == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation.
                return -compareResult;
            }

            // Return '0' to indicate that they are equal.
            return 0;
        }
    }
}