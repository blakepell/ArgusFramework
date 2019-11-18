using System;
using System.Collections;
using System.Windows.Forms;
using Argus.Extensions;

namespace Argus.Windows.Forms.Controls
{

    /// <summary>
    /// Utility comparer that allows a WinForms ListView control to be sorted by column.
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
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public ListViewColumnSorter()
        {

        }

        /// <summary>
        /// Compares to values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX = (ListViewItem)x;
            ListViewItem listviewY = (ListViewItem)y;

            // Compare the two items.
            if (listviewX.SubItems[SortColumn].Text.IsDateTime() && listviewY.SubItems[SortColumn].Text.IsDateTime())
            {
                compareResult = ObjectCompare.Compare(Convert.ToDateTime(listviewX.SubItems[SortColumn].Text), Convert.ToDateTime(listviewY.SubItems[SortColumn].Text));
            }
            else if (listviewX.SubItems[SortColumn].Text.IsNumeric() && listviewY.SubItems[SortColumn].Text.IsNumeric())
            {
                compareResult = ObjectCompare.Compare(Convert.ToDouble(listviewX.SubItems[SortColumn].Text), Convert.ToDouble(listviewY.SubItems[SortColumn].Text));
            }
            else
            {
                compareResult = ObjectCompare.Compare(listviewX.SubItems[SortColumn].Text, listviewY.SubItems[SortColumn].Text);
            }

            // Calculate the correct return value based on the object comparison.
            if (Order == SortOrder.Ascending)
            {
                // Ascending sort is selected, return typical result of compare operation.
                return compareResult;
            }
            else if (Order == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation.
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate that they are equal.
                return 0;
            }

        }

        /// <summary>
        /// Object Compare
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CaseInsensitiveComparer ObjectCompare { get; set; } = new CaseInsensitiveComparer();

        /// <summary>
        /// The sort column.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int SortColumn { get; set; } = 0;

        /// <summary>
        /// The sort order.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public SortOrder Order { get; set; } = SortOrder.None;

    }
}
