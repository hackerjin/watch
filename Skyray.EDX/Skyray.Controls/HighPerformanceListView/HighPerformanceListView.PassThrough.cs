using System.ComponentModel;
using System.Windows.Forms;

/***************************************************************************************
 *  Class : HighPerformanceListView (Partial class file)
 *  Type  : User Control
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
    /// This is a partial class file for the HighPerformanceListView control.
    /// 
    /// This partial file contains the 'pass-through' methods and properties.
    /// 
    /// These methods,properties, and events pass information through to the internal 
    /// 'ListView' control that actually does the display.
    /// 
    /// These items are just a subset of the ListView.
    /// Add more items here in this file as needed.
    /// 
    /// 
    /// </summary>
	public partial class HighPerformanceListView
	{
        #region PassThroughEvents_EventProperties

        /// <summary>
        /// Occurs when the details view of a System.Windows.Forms.ListView is drawn
        /// and the System.Windows.Forms.ListView.OwnerDraw property is set to true.
        /// </summary>
        [
        Category("Appearance"),
        Description("Occurs when the details view of a ListView is drawn, and the OwnerDraw property is true.")
        ]
        public event DrawListViewSubItemEventHandler DrawSubItem
        {
            // Pass the DrawSubItem event through to the ListView.DrawSubItem
            add { listView.DrawSubItem += value; }
            remove { listView.DrawSubItem -= value; }
        }

        /// <summary>
        ///  Occurs when the details view of a System.Windows.Forms.ListView is drawn
        //   and the System.Windows.Forms.ListView.OwnerDraw property is set to true.
        /// </summary>
        [
        Category("Appearance"),
        Description("Occurs when the details view of a System.Windows.Forms.ListView is drawn")
        ]
        public event DrawListViewColumnHeaderEventHandler DrawColumnHeader
        {
            // Pass the DrawColumnHeader event through to the ListView.DrawColumnHeader
            add { listView.DrawColumnHeader += value; }
            remove { listView.DrawColumnHeader -= value; }
        }

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is released.
        /// </summary>
        [
        Category("Behavior"),
        Description(" Occurs when the mouse pointer is over the control and a mouse button is released.")
        ]
        public new event MouseEventHandler MouseUp
        {
            // Pass the MouseUp event through to the ListView.MouseUp
            add { listView.MouseUp += value; }
            remove { listView.MouseUp -= value; }
        }


        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is released.
        /// </summary>
        [
        Category("Behavior"),
        Description(" Occurs when the mouse pointer is over the control and a mouse button is released.")
        ]
        public new event MouseEventHandler MouseDown
        {
            // Pass the MouseDown event through to the ListView.MouseDown
            add    { listView.MouseDown += value; }
            remove { listView.MouseDown -= value; }
        }


        /// <summary>
        /// Occurs when the mouse pointer is moved over the control.
        /// </summary>
        [
        Category("Behavior"),
        Description(" Occurs when the mouse pointer is moved over the control.")
        ]
        public new event MouseEventHandler MouseMove
        {
            // Pass the MouseMove event through to the ListView.MouseMove
            add    { listView.MouseMove += value; }
            remove { listView.MouseMove -= value; }
        }

        #endregion



        #region PassThroughListViewMethods
        /// <summary>
        /// Begins a drag-and-drop operation (on the list view)
        /// </summary>
        /// <param name="data">The data to drag.</param>
        /// <param name="allowedEffects">One of the <see cref="T:System.Windows.Forms.DragDropEffects"/> values.</param>
        /// <returns>
        /// A value from the <see cref="T:System.Windows.Forms.DragDropEffects"/> enumeration that represents the final effect that was performed during the drag-and-drop operation.
        /// </returns>
        public new DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            // Pass through method to ListView.DoDragDrop
            return listView.DoDragDrop(data, allowedEffects);
        }
        #endregion

        #region PassThroughProperties
        /// <summary>
        /// Gets a collection contains all items displayed in the list view control.
        /// </summary>
        /// <value>The items.</value>
        public ListView.ListViewItemCollection Items
        {
            // Pass through 'Items' property to ListView.Items
            get { return listView.Items; }
        }

        /// <summary>
        /// Gets the indexes of the selected items in the list view control.
        /// </summary>
        /// <value>The selected indices.</value>
        public ListView.SelectedIndexCollection SelectedIndices
        {
            // Pass through 'SelectedIndices' to ListView.SelectedIndices
            get { return listView.SelectedIndices; }
        }
        #endregion


	}
}
