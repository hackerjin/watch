using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Skyray.Controls.Win32Util;



/***************************************************************************************
 *  Class : HighPerformanceListView
 *  Type  : User Control
 *  Author: Curt Cearley
 *  Email : harpyeaglecp@aol.com
 *  
 *  This software is released under the Code Project Open License (CPOL)
 *  See official license description at: http://www.codeproject.com/info/cpol10.aspx
 *  
 * This software is provided AS-IS without any warranty of any kind.
 *  
 *  Modification History:
 *  06/15/2009  curtcearley     Creation of class for www.codeproject.com example.
 * 
 * 
 ***************************************************************************************/

namespace Skyray.Controls
{
    /// <summary>
    /// 
    /// This control was written to demonstrate the technique of displaying
    /// a large quantity of data to a user, with very little performance 
    /// degredation.  A 'ListView' display is selected for this example.
    /// 
    /// </summary>
    /// 
    public partial class HighPerformanceListView : UserControl
    {
        #region PRIVATE_VARIABLES

        // Reference to the object that holds the data being displayed
        // in the list view.
        private IHighPerformanceListViewProvider listViewProvider;

        // Index into the data list that represents the first item 
        // to be displayed in the list view.
        // This represents the starting position of our 
        // sliding window moved over the data list.
        private int displayStartIdx = 0;

        // Number of rows from the data list that are being displayed
        private int rowsDisplayed = 0;

        // Maximum number of items (rows) that can be displayed in the list view
        // (Depends on size of control on screen)
        private int maxDisplayLines = 0;

        // Number of the column of the list view that is currently being sort
        private int sortColumnNumber = -1;

        // Specifies the sort order of the current sort column (ascending, descending)
        private SortOrder sortOrder = SortOrder.None;


        public delegate void ItemSelectedChange(ListView.SelectedListViewItemCollection e);
        public delegate void NewDoubleClick();
        public delegate void RefeshData();

        public delegate void ScrollLoadData();

        public int hScrollValue
        {
            get { return hScrollBar.Value; }
            set 
            { 
                hScrollBar.Value = value;
                this.hScrollBar_ValueChanged(null, null);
            }
        }

        public int DisplayStartIdx
        {
            get { return displayStartIdx; }
            set { displayStartIdx = value; }
        }

        public int MaxDisplayLines
        {
            get { return maxDisplayLines; }
        }

        public ItemSelectedChange OnItemSelectedChange;
        public NewDoubleClick OnNewDoubleClick;
        public RefeshData OnRefeshData;
        public ScrollLoadData OnScrollLoadData;
        #endregion

        #region ConstructorsAndInitialization

        /// <summary>
        /// Initializes a new instance of the <see cref="HighPerformanceListViewControl"/> class.
        /// </summary>
        public HighPerformanceListView()
        {
            InitializeComponent();

            // Hide verticle scroll bar until it is needed
            hScrollBar.Visible = false;

            // Add double buffering and borderselect to the list view styles:
            // Double buffering will help prevent flicker when drawing of the control
            ListViewUtils.AddStylesToExistingStyles(listView, ListViewUtils.LVS_EX_DOUBLEBUFFER | ListViewUtils.LVS_EX_BORDERSELECT);
            //listView.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(listView_ItemSelectionChanged);
            listView.DoubleClick += new EventHandler(listView_DoubleClick);
            // Setup listview for manually handling displayed items, and vertical scroll bar
            listView.FullRowSelect = true;
            listView.OwnerDraw = false;

        }

        void listView_DoubleClick(object sender, EventArgs e)
        {
            OnNewDoubleClick();
        }

        void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
           
        }

        /// <summary>
        /// Clears all items and columns from list view, then adds the columns supplied by the provider.
        /// </summary>
        private void SetupListViewColumns()
        {
            if (listViewProvider != null)
            {
                List<ColumnHeader> columnHeaderList = listViewProvider.ColumnHeaders;
                // Add Columns (ColumnHeader objects) to list view
                foreach (ColumnHeader listViewColumnHeader in columnHeaderList)
                    listView.Columns.Add(listViewColumnHeader);
            }
        }


        /// <summary>
        /// Displays the supplied data item list.
        /// </summary>
        public void DisplayDataItemList()
        {
            // Clear out any old data
            ClearItems();

            // Make sure before displaying the current data set, that we have
            // the correct maximum number of lines to display in the list view.
            // Quirk: GetListViewVisibleRows() has returned a lesser number when
            // called from the constructor. 
            // Therefore make sure we have the correct value before displaying data.
            maxDisplayLines = GetListViewVisisbleRows();

            // Refresh the view items on screen
            RefreshListViewItems();
        }
        #endregion

        #region ListViewProviderMethods
        /// <summary>
        /// Gets or sets the list view provider.
        /// </summary>
        /// <value>The list view provider.</value>
        public IHighPerformanceListViewProvider ListViewProvider
        {
            get
            {
                return this.listViewProvider;
            }
            set
            {
                listView.Clear();
                listViewProvider = value;
                if (listViewProvider != null)
                {
                    listViewProvider.ControllingListView = this;
                    SetupListViewColumns();
                }
            }
        }

        #endregion

        #region ListViewManiplulation

        /// <summary>
        /// Returns the number of rows that can be displayed in the ListView
        /// </summary>
        /// <returns>Number of visible rows</returns>
        public int GetListViewVisisbleRows()
        {

            // Get the maximum number of visible rows in listbox
            // (Note: requires sending win32 message to listview)
            int result = ListViewUtils.GetVisibleRows(listView);
            return result;
        }

        /// <summary>
        /// Hide native vertical scroll bar, since we are handling vertical scrolling with our own scroll bar.
        /// This method ensures that the scroll bar does not accidentally who up.
        /// </summary>
        private void HideNativeVerticalScrollBar()
        {
            ListViewUtils.ShowVerticalScrollBar(listView, false);
        }



        /// <summary>
        /// Append the data item at supplied index, 'dataIndex', to the list view.
        /// </summary>
        /// <param name="dataIndex">Index of the data item to display.</param>
        /// <returns>'false' if the item was NOT displayed, 'true' if the item was displayed.</returns>
        private bool AddDataItemToListView(int dataIndex)
        {
            bool result = false;

            if (rowsDisplayed < maxDisplayLines)
            {
                // There is room for at least one more row to be displayed
                ListViewItem lvi = listViewProvider.ConvertDataItemToListViewItem(dataIndex);
                listView.Items.Add(lvi);
                rowsDisplayed++;
                result = true;
            }
            return result;
        }

       
        /// <summary>
        /// Refreshes items displayed in the list view, starting with the starting item to be displayed,
        /// and ending with as many items that can be displayed on the ListView.
        /// </summary>
        private void RefreshListViewItems()
        {
            // Clear the current items in the list view
            listView.BeginUpdate();
            listView.Items.Clear();
            rowsDisplayed = 0;
           
            if (listViewProvider != null)
                //for (int x = displayStartIdx*maxDisplayLines; (rowsDisplayed < maxDisplayLines) && (x < listViewProvider.DataCount); x++)
                //    AddDataItemToListView(x);
            for (int x = rowsDisplayed; (rowsDisplayed < maxDisplayLines) && (x < listViewProvider.DataCount); x++)
                AddDataItemToListView(x);
            if (OnRefeshData != null)
                OnRefeshData();
            listView.EndUpdate();
            UpdateVerticalScrollBar();
        }

        /// <summary>
        /// Clears the items in the list view, and resets the starting display index to zero.
        /// </summary>
        public void ClearItems()
        {
            // Clear displayed items
            listView.Items.Clear();

            rowsDisplayed = 0;
            displayStartIdx = 0;

            hScrollBar.Value = 0;

            ClearSorting();
        }
        #endregion


        #region ScrollBarAndResizeMethods

        
        /// <summary>
        /// Handles the Scroll event of the vScrollBar control.
        /// Vertical Scroll bar value changed - manually adjust items visible in the list view
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ScrollEventArgs"/> instance containing the event data.</param>
        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.OldValue == e.NewValue)
                return; // Same value - do not do anything
            // Docs from Microsoft:
            // Note:  The value of a scroll bar cannot reach its maximum value through user interaction at run time. 
            // The maximum value that can be reached is equal to the Maximum property value minus the LargeChange property value plus one.
            // The maximum value can only be reached programmatically.


            int newStartIdx = e.NewValue; // new value for scroll - ListView should be the index of the top item of dataList to display in ListView                    
            int endDisplayIdx = displayStartIdx + maxDisplayLines - 1; // Ending index of the items currently being displayed


            // NOTE: It was discovered that the by doing the following, the best performance was
            // observed from the list box with less flicker, etc.
            // Instead of removing the portion of the list view items that are no longer visible,
            // and adding new listview items that have become visible, resulted in bad performance of the
            // list view control, with flicker, and the listview's native scroll bar showing up periodically.
            // This way the native scroll bar will never appear.
            displayStartIdx = newStartIdx;
            if (OnScrollLoadData != null)
                OnScrollLoadData();
            //if (OnRefeshData != null)
            //    OnRefeshData();
            //if (displayStartIdx % maxDisplayLines == 0 || (displayStartIdx / maxDisplayLines + 1) * maxDisplayLines > listViewProvider.DataCount)
                RefreshListViewItems();
        }


        /// <summary>
        /// sync the min/max values with the number of message items in the userViewableDataList
        /// </summary>
        private void UpdateVerticalScrollBar()
        {

            // Make sure that internal scroll bar never appears
            HideNativeVerticalScrollBar();

            //if (listViewProvider != null && listViewProvider.DataCount > maxDisplayLines)
            if (listViewProvider != null && listViewProvider.DataTotal > maxDisplayLines)
            {
                // There is more data to display than available viewable lines on the list view.

                hScrollBar.Visible = true;

                // Adjust maximum value of the scroll bar, such that the max value returned by the control
                // will be the index value (into userViewableDataList) that should be displayed as the starting index
                //hScrollBar.Maximum = listViewProvider.DataCount - maxDisplayLines + hScrollBar.LargeChange - 1;
                hScrollBar.Maximum = (listViewProvider.DataTotal - maxDisplayLines + hScrollBar.LargeChange - 1) / maxDisplayLines + 1;
                //hScrollBar.Maximum = (int)Math.Ceiling((listViewProvider.DataCount - maxDisplayLines + hScrollBar.LargeChange - 1)/(double)maxDisplayLines);
            }
            else
            {
                // No need to display vertical scroll bar (we can view all items in the existing listview window)
                hScrollBar.Visible = false;
            }
        }


        /// <summary>
        /// Handles the Resize event of the ListView control.
        /// Resize callback method - Add or remove viewable items based on the new
        /// ListView display count.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ListView_Resize(object sender, EventArgs e)
        {
            lock (listView)
            {
                // Update the number of lines we can see.
                maxDisplayLines = GetListViewVisisbleRows();
                if (maxDisplayLines > 0) 
                    RefreshListViewItems();
            }
        }
        #endregion

        #region ColumnSorting
        /// <summary>
        /// Clears the variables keeping track of data sorting.
        /// Also clears the image key for all columns so that the
        /// up/down arrow is not displayed.
        /// </summary>
        private void ClearSorting()
        {
            if (sortOrder != SortOrder.None)
            {
                // Clear column sorting data:
                sortOrder = SortOrder.None;
                this.sortColumnNumber = -1;

                // Set the direction arrows on the column headers
                for (int x = 0; x < listView.Columns.Count; x++)
                    listView.Columns[x].ImageKey = null;
            }
        }


        /// <summary>
        /// Handles the ColumnClick event of the ListView control.
        ///  Sort the current column being clicked
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ColumnClickEventArgs"/> instance containing the event data.</param>
        private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (listViewProvider != null)
            {
                if (sortColumnNumber == e.Column)
                    // Same column - toggle the sort order
                    sortOrder = (sortOrder == SortOrder.None ? SortOrder.Ascending : (sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending));
                else
                    // Different column - default to Ascending
                    sortOrder = SortOrder.Ascending;

                // Record the current sort column
                sortColumnNumber = e.Column;

                // Set the direction arrows on the column headers
                for (int x = 0; x < listView.Columns.Count; x++)
                {
                    if (x == e.Column)
                        listView.Columns[x].ImageKey = (sortOrder == SortOrder.Ascending ? "up.png" : "down.png");
                    else
                        listView.Columns[x].ImageKey = null;
                }

                //Sort the list
                listViewProvider.SortDataList(sortColumnNumber, sortOrder);

                // Reset the display to the first element in the list after the sort
                // In future, could keep track of currently selected item, if one exists,
                // and set the list view to view that item.
                displayStartIdx = 0;

                hScrollBar.Value = 0;

                RefreshListViewItems();
            }
        }
        #endregion 

        #region ListItemSelct
        public ListView.SelectedListViewItemCollection SelectedItems
                {

                    get { return this.listView.SelectedItems; }
                }
        #endregion


        private void listView_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = this.listView.SelectedItems;
            if (items != null && items.Count > 0)
            {
                OnItemSelectedChange(items);
            }
        }

        /// <summary>
        /// listView_Click在按住鼠标拖动选择后, 有几率提示未选中, 所以替换为mouseup事件
        /// </summary>
        void listView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            ListView.SelectedListViewItemCollection items = this.listView.SelectedItems;
            if (items != null && items.Count > 0)
            {
                OnItemSelectedChange(items);
            }
        }

        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            
        }
    }
}
