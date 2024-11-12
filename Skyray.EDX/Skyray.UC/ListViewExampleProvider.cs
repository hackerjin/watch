using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Skyray.Controls;
using System.Windows.Forms;

namespace Skyray.UC
{
    public class ListViewExampleProvider : ListViewProviderBase
    {
        // List that holds a reference to the application specific data to be displayed.
        // In real life - this could be other data sources, such as rows in a very large
        // SQL query, etc.
        private List<DataRow> dataList;

        /// <summary>
        /// Gets or sets the application specific data list.        
        /// </summary>
        /// <value>The data list.</value>
        public List<DataRow> DataList
        {
            get
            {
                return dataList;
            }

            set
            {
                // Save reference to new data list
                dataList = value;

                if (listView == null)
                    throw new Exception("ListViewExampleProvider Error - the internal HighPerformanceListView has not been set, cannot display data");


                // Tell the list view to display a new list.
                listView.DisplayDataItemList();
            }
        }


        #region IHighPerformanceListViewProvider Members

        /////////////////////////////////////////////////////////////////////////////////
        // The interface methods are used to interact with the HighPerformanceListView //
        /////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Returns a list of column headers to use for the list view.
        /// </summary>
        /// <value>The column headers.</value>
        public override List<ColumnHeader> ColumnHeaders
        {
            get
            {
                ColumnHeader hdr;
                List<ColumnHeader> result = new List<ColumnHeader>();

                // First Column: Title 
                hdr = new ColumnHeader();
                hdr.Text = "Column One";
                hdr.Width = 200;
                result.Add(hdr);

                //// Second Column: 
                //hdr = new ColumnHeader();
                //hdr.Text = "Column Two";
                //hdr.Width = 130;
                //result.Add(hdr);

                return result;
            }
        }

        /// <summary>
        /// Sort the data list. Usually in response to clicking a column on the list view.
        /// </summary>
        /// <param name="sortColumnNumber">The column number to sort.</param>
        /// <param name="sortOrder">Order in which to perform the sort (Ascending or Descending).</param>
        public override void SortDataList(int sortColumnNumber, System.Windows.Forms.SortOrder sortOrder)
        {
            if (dataList != null)
            {
                dataList.Sort(delegate(DataRow firstItem, DataRow secondItem)
                {
                    int result = 0;

                    if (sortColumnNumber == 0)
                    {
                        // Sort the data by first name
                        result = firstItem["name"].ToString().CompareTo(secondItem["name"].ToString());
                    }
                    //else if (sortColumnNumber == 1)
                    //{
                    //    // Sort the data by last name
                    //    result = firstItem.LastName.CompareTo(secondItem.LastName);
                    //}

                    if (sortOrder == SortOrder.Descending)
                        result = result * (-1); // Reverse the result for descending order

                    return result;
                });

            }
        }

        /// <summary>
        /// Creates a ListViewItem that represents a single item of data from the data list.
        /// Used by the HighPerformanceListView dynamically create items to display in the view.
        /// </summary>
        /// <param name="dataIndex">Index of the data item to display in ListViewItem.</param>
        /// <returns>ListViewItem representing the data item at the supplied index.</returns>
        public override ListViewItem ConvertDataItemToListViewItem(int dataIndex)
        {
            ListViewItem result;
            if (dataList != null && dataIndex < dataList.Count)
            {
                result = new ListViewItem(this.dataList[dataIndex]["name"].ToString());
                result.Name = this.dataList[dataIndex]["id"].ToString();
                result.SubItems.Add(dataList[dataIndex]["name"].ToString());
            }
            else
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Returns the number of data items in the data list.
        /// </summary>
        /// <value>The data count.</value>
        public override int DataCount
        {
            get
            {
                int result;
                if (dataList != null)
                    result = dataList.Count;
                else
                    result = 0;
                return result;
            }
            set
            {
                dataCount = value;
            }
        }

        //public int DataTotal
        //{
        //    get { return this.dataCount; }
        //}
        #endregion
    }
}
