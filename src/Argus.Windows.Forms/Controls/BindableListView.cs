using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Data;
using Argus.Extensions;

namespace Argus.Windows.Forms.Controls
{

    /// <summary>
    /// A ListView which supports binding to DataTable's, anything that implements IDataReader, Generic Lists, Arrays, single strings and primivites.
    /// </summary>
    /// <remarks>
    /// Note, this class does not support image binding, the string values are written out from the various DataSource types.
    /// </remarks>
    public class BindableListView : ListView
    {
        //*********************************************************************************************************************
        //
        //             Class:  BindableListView
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  06/02/2008
        //      Last Updated:  03/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// This ListView will set a default view of details here.
        /// </remarks>
        public BindableListView()
        {
            this.View = View.Details;
            this.FullRowSelect = true;
            this.DoubleBuffered = true;

            this.ColumnClick += BindableListView_ColumnClick;
        }

        /// <summary>
        /// Executes when the column is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// This sub has added custom sorting via our ListViewColumnSorter that will also properly handle
        /// dates and times.
        /// </remarks>
        private void BindableListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (this.DefaultSortingOnColumnClick == true)
            {
                this.BeginUpdate();

                var sorter = new ListViewColumnSorter
                {
                    SortColumn = e.Column
                };

                this.ListViewItemSorter = sorter;

                if (this.Sorting == SortOrder.Ascending)
                {
                    sorter.Order = SortOrder.Descending;
                    this.Sorting = SortOrder.Descending;
                }
                else
                {
                    sorter.Order = SortOrder.Ascending;
                    this.Sorting = SortOrder.Ascending;
                }

                this.Sort();
                ApplyItemColors();

                this.EndUpdate();
            }
        }

        /// <summary>
        /// This will refresh the ListView with the data that is current in the DataSource.  This will work for DataTable's, arrays and
        /// lists but WILL NOT work for DataReaders as they can only be read through forward once.
        /// </summary>
        /// <remarks>
        /// Do not use ReBind with a DataReader.  Since a DataReader is forward only, it will only be bindable once therefore the second
        /// attempt at binding it will produce an empty ListView.  All other supported DataSource's however will ReBind with their current
        /// contents.
        /// </remarks>
        public void ReBind()
        {
            this.DataSource = _dataSource;
        }

        /// <summary>
        /// Formats the column as dollars.  The data must already exist in the column or bound when this is run.
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <remarks></remarks>
        public void FormatColumnAsDollars(int columnIndex)
        {
            if (this.Columns.Count < columnIndex | columnIndex < 0)
            {
                return;
            }

            this.Columns[columnIndex].TextAlign = HorizontalAlignment.Right;

            foreach (ListViewItem lvi in this.Items)
            {
                lvi.SubItems[columnIndex].Text = string.Format("${0}", lvi.SubItems[columnIndex].Text.FormatIfNumber(2));
            }

        }

        /// <summary>
        /// Formats the column as numeric with user specified decimal places.  The data must already exist in the column or bound when this is run.
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <remarks></remarks>
        public void FormatColumnAsNumeric(int columnIndex, int decimalPlaces)
        {
            if (this.Columns.Count < columnIndex | columnIndex < 0)
            {
                return;
            }

            this.Columns[columnIndex].TextAlign = HorizontalAlignment.Right;

            foreach (ListViewItem lvi in this.Items)
            {
                lvi.SubItems[columnIndex].Text = string.Format("{0}", lvi.SubItems[columnIndex].Text.FormatIfNumber(decimalPlaces));
            }

        }

        /// <summary>
        /// Applies the item colors to the items in the ListView with the colors in the RowItemColor and RowAlternatingItemColor properties.
        /// </summary>
        /// <remarks></remarks>
        public void ApplyItemColors()
        {
            ApplyItemColors(RowItemColor, RowAlternatingItemColor);
        }

        /// <summary>
        /// Applies the item colors to the items in the ListView.
        /// </summary>
        /// <param name="itemColor"></param>
        /// <param name="alternatingItemColor"></param>
        /// <remarks></remarks>
        public void ApplyItemColors(Color itemColor, Color alternatingItemColor)
        {
            this.BeginUpdate();
            bool @switch = true;

            foreach (ListViewItem lvi in this.Items)
            {
                if (@switch == true)
                {
                    lvi.BackColor = itemColor;
                }
                else
                {
                    lvi.BackColor = alternatingItemColor;
                }

                @switch = !@switch;
            }

            this.EndUpdate();
        }

        /// <summary>
        /// Applies the filter.  If the filter is blanked all items in the ListView will be set to visible.
        /// </summary>
        /// <remarks>
        /// This filter is run locally against the data currently avaiable and not against the actual DataSource.
        /// </remarks>
        private void ApplyFilter()
        {

            // Use begin update to lock the UI update until we're done setting the items.
            this.BeginUpdate();

            // Re-add any from the filtered items that are no longer valid.
            for (int i = _filteredItems.Count - 1; i >= 0; i += -1)
            {
                if (_filteredItems[i].Text.Contains(this.Filter) == true)
                {
                    this.Items.Add(_filteredItems[i]);
                    _filteredItems.Remove(_filteredItems[i]);
                }
            }

            // Loop through the items backwards and remove anything that has now been filtered out.
            for (int i = this.Items.Count - 1; i >= 0; i += -1)
            {
                switch (this.FilterType)
                {
                    case FilterTypes.Contains:
                        {
                            if (this.Items[i].Text.Contains(this.Filter) == false)
                            {
                                _filteredItems.Add(this.Items[i]);
                                this.Items.RemoveAt(i);
                            }

                            break;
                        }

                    case FilterTypes.StartsWith:
                        {
                            if (this.Items[i].Text.StartsWith(this.Filter) == false)
                            {
                                _filteredItems.Add(this.Items[i]);
                                this.Items.RemoveAt(i);
                            }

                            break;
                        }

                    case FilterTypes.EndsWith:
                        {
                            if (this.Items[i].Text.EndsWith(this.Filter) == false)
                            {
                                _filteredItems.Add(this.Items[i]);
                                this.Items.RemoveAt(i);
                            }

                            break;
                        }
                }
            }

            // Reset the switch colors
            ApplyItemColors();

            // Unlock the ListView's UI
            this.EndUpdate();
        }

        /// <summary>
        /// Used to save the filtered list view items so that we can add them back later.
        /// </summary>
        /// <remarks>
        /// This is used to compensate for the fact that there is now Visisble property available on the ListViewItem.
        /// </remarks>
        private List<ListViewItem> _filteredItems = new List<ListViewItem>();

        private string _filter = "";
        /// <summary>
        /// The filter that is applied to the list box.  Use the filter type field to specify how this filter behaves.  The filter will
        /// filter on the Text value of the ListViewItem, not the sub items.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>
        /// This filter is run locally against the data currently avaiable and not against the actual DataSource.  The filter can be
        /// slow on large lists, you may want to update your DataSource in that case for better performance.
        /// </remarks>
        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                ApplyFilter();
            }
        }

        /// <summary>
        /// The type of filters available.
        /// </summary>
        /// <remarks></remarks>
        public enum FilterTypes
        {
            /// <summary>
            /// True if the string matches the beginning.
            /// </summary>
            /// <remarks></remarks>
            StartsWith,
            /// <summary>
            /// True if the string is contained anywhere.
            /// </summary>
            /// <remarks></remarks>
            Contains,
            /// <summary>
            /// True if the string ends with.
            /// </summary>
            /// <remarks></remarks>
            EndsWith
        }

        /// <summary>
        /// The type of filter that will be applied if the Filter property is set.  The default is a contains match.
        /// </summary>
        /// <value></value>
        /// <returns>The filter can either be a begins with match, a contains match or a ends with match.</returns>
        /// <remarks></remarks>
        public FilterTypes FilterType { get; set; } = FilterTypes.Contains;

        /// <summary>
        /// Converts a CamelCase string to Title Case
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <remarks>This is used to format the column headings in cases where they are created from a .Net object's properties.</remarks>
        private string CamelCaseToTitleCase(string text)
        {
            text = text.Substring(0, 1).ToUpper() + text.Substring(1);
            return Regex.Replace(text, @"(\B[A-Z])", " $1");
        }

        private object _dataSource;
        /// <summary>
        /// The DataSource property supports binding to either a DataTable, any object that implements IDataReader, single Objects,
        /// Arrays or Generic Lists.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>
        /// Whenever the DataSource is set it automatically triggers the binding of the ListView to the specified data.
        /// </remarks>
        public object DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                // We want to clear out all of the items and the columns when we rebind.
                this.Items.Clear();
                this.Columns.Clear();

                if (value == null)
                {
                    return;
                }

                // This will greatly speed up the rendering of the ListView.. the EndUpdate must be called at the end of the
                // procedure though so the ListView can be drawn.
                this.BeginUpdate();

                if (value is DataTable)
                {
                    // *****************************************************************
                    // Binding to a DataTable
                    // *****************************************************************
                    var dt = value as DataTable;

                    foreach (DataColumn column in dt.Columns)
                    {
                        this.Columns.Add(column.ColumnName, column.ColumnName);
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        string[] items = new string[((DataTable)value).Columns.Count];
                        var loopTo = ((DataTable)value).Columns.Count - 1;

                        for (int counter = 0; counter <= loopTo; counter++)
                        {
                            if (!DBNull.Value.Equals(row[counter]))
                            {
                                items[counter] = row[counter].ToString();
                            }
                            else
                            {
                                items[counter] = "";
                            }
                        }

                        // New item, the -1 being that there is no ListView image associated with this.                        
                        var item = new ListViewItem(items, -1);
                        this.Items.Add(item);
                    }
                }
                else if (value is IDataReader)
                {
                    var loopTo1 = ((IDataReader)value).FieldCount - 1;
                    // *****************************************************************
                    // Binding to a IDataReader objects
                    // *****************************************************************

                    // TODO, determine type to know which way to align field.
                    for (int counter = 0; counter <= loopTo1; counter++)
                    {
                        this.Columns.Add(((IDataReader)value).GetName(counter), ((IDataReader)value).GetName(counter));
                    }

                    while (((IDataReader)value).Read())
                    {
                        string[] items = new string[((IDataReader)value).FieldCount + 1];
                        var loopTo2 = ((IDataReader)value).FieldCount - 1;

                        for (int counter = 0; counter <= loopTo2; counter++)
                        {
                            items[counter] = ((IDataReader)value)[counter].ToString();
                        }

                        // New item, the -1 being that there is no ListView image associated with this.
                        var item = new ListViewItem(items, -1);
                        this.Items.Add(item);
                    }
                }
                else if (value is Array)
                {
                    // *******************************************************************
                    // Binding to an Array that has a type that ToString() will manifest
                    // correctly
                    // *******************************************************************

                    this.Columns.Add("Value", "Value");

                    Array list = value as Array;

                    foreach (object o in list)
                    {
                        this.Items.Add(o.ToString());
                    }
                }
                else if (value.GetType().IsGenericType == true && value is IEnumerable)
                {
                    // *****************************************************************
                    // Binding to an IEnumerable of objects whose properties will make
                    // up the columns.
                    // *****************************************************************
                    int rowCounter = 0;
                    IEnumerable list = value as IEnumerable;

                    foreach (object o in list)
                    {
                        if (o == null)
                        {
                            continue;
                        }

                        // This case will get string lists and lists of primitives.
                        if (o is string || o.GetType().IsPrimitive)
                        {
                            if (rowCounter == 0)
                            {
                                this.Columns.Add("Value", "Value");
                            }

                            this.Items.Add(o.ToString());

                            rowCounter++;
                            continue;
                        }

                        // If we get here we should have IEnumerable of some class.  This will go through the properties
                        // of the class and pull them off to put into the each column.
                        var pi = o.GetType().GetProperties();
                        var items = new string[pi.Count() + 1];

                        // All the objects are going to be the same (or should be) and we only want to add the column
                        // headings one time.
                        if (rowCounter == 0)
                        {
                            foreach (var info in pi)
                            {
                                this.Columns.Add(info.Name, info.Name);
                            }
                        }

                        int counter = 0;
                        foreach (PropertyInfo info in pi)
                        {
                            if (info.CanRead == true)
                            {
                                items[counter] = info.GetValue(o, null).ToString();
                            }

                            counter += 1;
                        }

                        // New item, the -1 being that there is no ListView image associated with this.
                        var item = new ListViewItem(items, -1);

                        if (this.SaveObjectInTag == true)
                        {
                            item.Tag = o;
                        }

                        this.Items.Add(item);

                        rowCounter += 1;
                    }
                }
                else if (value is string || value.GetType().IsPrimitive)
                {
                    // *****************************************************************
                    // This is a single string or a primitive.
                    // *****************************************************************
                    this.Columns.Add("Value", "Value");
                    this.Items.Add(value.ToString());
                }
                else
                {
                    // *****************************************************************
                    // Binding to an individual object.
                    // *****************************************************************
                    var pi = value.GetType().GetProperties();

                    foreach (var info in pi)
                    {
                        this.Columns.Add(info.Name, info.Name);
                    }

                    var items = new string[pi.Count() + 1];

                    int counter = 0;
                    foreach (PropertyInfo info in pi)
                    {
                        if (info.CanRead == true)
                        {
                            items[counter] = info.GetValue(value, null).ToString();
                        }

                        counter += 1;
                    }

                    // New item, the -1 being that there is no ListView image associated with this.
                    var item = new ListViewItem(items, -1);

                    if (this.SaveObjectInTag == true)
                    {
                        item.Tag = value;
                    }

                    this.Items.Add(item);

                }


                _dataSource = value;

                // Cleanup the column headings of property is true.
                if (this.CleanupColumnHeadings == true)
                {
                    foreach (ColumnHeader c in this.Columns)
                    {
                        c.Text = CamelCaseToTitleCase(c.Text);
                        c.Name = CamelCaseToTitleCase(c.Name);
                    }
                }

                // Resize to the size of the column heading if property is true.
                if (this.ResizeOnDataSourceChange == true)
                {
                    this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }

                // Apply the item back colors
                this.ApplyItemColors();
                this.EndUpdate();


            }
        }

        /// <summary>
        /// The back color of the every odd row.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>The default is FromArgb(237,243,254)</remarks>
        public Color RowItemColor { get; set; } = Color.FromArgb(237, 243, 254);

        /// <summary>
        /// The back color of every even row.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>The default is white.</remarks>
        public Color RowAlternatingItemColor { get; set; } = Color.White;

        /// <summary>
        /// Whether or not to automatically resize the columns when the list view is bound.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ResizeOnDataSourceChange { get; set; } = true;

        /// <summary>
        /// This property defines whether spaces should be put in between words in a column heading when CamelCase is found or when
        /// an underscore exists.
        /// </summary>
        /// <value></value>
        /// <returns>'EmployeeFirstName' would become 'Employee First Name'</returns>
        /// <remarks></remarks>
        public bool CleanupColumnHeadings { get; set; } = true;

        /// <summary>
        /// Whether or not the default alpha numeric sorting is enabled when a column is clicked.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool DefaultSortingOnColumnClick { get; set; } = true;

        /// <summary>
        /// Puts the object that was used to create the ListViewItem into the Tag property of the ListViewItem for
        /// reference after the fact.  This will only work for IEnumerable object lists, it will not work with
        /// IDataReader or DataTable bindings.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool SaveObjectInTag { get; set; } = false;

    }
}
