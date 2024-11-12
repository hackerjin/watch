using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


/***************************************************************************************
 *  Interface : IHighPerformanceListViewProvider  
 *  Author: Curt Cearley
 *  Email : harpyeaglecp@aol.com
 *  
 *  This software is released under the Code Project Open License (CPOL)
 *  See official license description at: http://www.codeproject.com/info/cpol10.aspx
 *  
 *  This software is provided AS-IS without any warranty of any kind.
 *  
 *  Modification History:
 *  06/15/2009  curtcearley     Creation of class for www.codeproject.com example.
 * 
 * 
 ***************************************************************************************/

namespace Skyray.Controls
{
    /// <summary>
    /// Interface which defines a helper object which manages a list of data 
    /// items that are displayed in a HighPerformanceListView control.       
    /// </summary>
    public interface IHighPerformanceListViewProvider
    {
        /// <summary>
        /// Returns a list of column headers to use for the list view.
        /// </summary>
        /// <value>The column headers.</value>
        List<ColumnHeader> ColumnHeaders { get; }


        /// <summary>
        /// Sort the data list. Usually in response to clicking a column on the list view.
        /// </summary>
        /// <param name="sortColumnNumber">The column number to sort..</param>
        /// <param name="sortOrder">Order in which to perform the sort (Ascending or Descending).</param>
        void SortDataList(int sortColumnNumber, SortOrder sortOrder);


        /// <summary>
        /// Creates a ListViewItem that represents a single item of data from the data list.
        /// Used by the HighPerformanceListView dynamically create items to display in the view.
        /// </summary>
        /// <param name="dataIndex">Index of the data.</param>
        /// <returns>ListViewItem representing the data item to be displayed in the a ListView</returns>
        ListViewItem ConvertDataItemToListViewItem(int dataIndex);


        /// <summary>
        /// Returns the number of data items in the data list.
        /// </summary>
        /// <value>The data count.</value>
        int DataCount { get; set; }

        int DataTotal { get; }

        /// <summary>
        /// Gets or sets the controlling list view.
        /// Used by the HighPerformanceListView control to supply a refernece to itself
        /// to the provider it is using.
        /// </summary>
        /// <value>The controlling list view.</value>
        HighPerformanceListView ControllingListView { get; set; }


        //void ItemSelectedChange(ListViewItemSelectionChangedEventArgs ee);

        //void DoubleClick();
        

    }
}
