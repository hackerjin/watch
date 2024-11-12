using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;


/***************************************************************************************
 *  Class : ListViewUtils
 *  Type  : Utility class - Interfacing with Win32 ListView control.
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
 ***************************************************************************************/

namespace Skyray.Controls.Win32Util
{
    public class ListViewUtils
    {

        // Win32 List View Messages
        private const int LVM_FIRST = 0x1000;      
        private const int LVM_GETCOUNTPERPAGE = (LVM_FIRST + 40);
        private const int LVM_SETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 54);
        private const int LVM_GETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 55);

        // ListView Win32 Styles
        public const int LVS_EX_BORDERSELECT = 0x00008000;
        public const int LVS_EX_DOUBLEBUFFER = 0x00010000;


        // Win32 Scroll Bar Constants
        private const int SB_VERT = 1;

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessageA(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        [DllImport("user32.dll")]
        public extern static int SendMessageA(IntPtr hwnd, uint msg, uint wParam, uint lParam);

        /// <summary>
        /// Returns the number of rows that are visible on the listview
        /// </summary>
        /// <param name="listview">The listview.</param>
        /// <returns></returns>
        public static int GetVisibleRows(ListView listview)
        {
            int result = 0;
            if(listview!=null)
                result=SendMessageA(listview.Handle, LVM_GETCOUNTPERPAGE, 0, 0);
            return result;
        }


        /// <summary>
        /// Adds the supplied styles to existing styles of the listview
        /// </summary>
        /// <param name="listview">The listview.</param>
        /// <param name="stylesToAdd">The styles to add to list view styles (WinAPI bitmap integer styles).</param>
        public static void AddStylesToExistingStyles(ListView listview, int stylesToAdd)
        {
            // Get the styles currently set from the listview control
            int styles = (int)SendMessageA(listview.Handle, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);                      
            styles |= stylesToAdd;
            SendMessageA(listview.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, 0, styles);
        }

        /// <summary>
        /// Shows the vertical scroll bar.
        /// </summary>
        /// <param name="listview">The listview.</param>
        /// <param name="showBar">if true, show vertical scroll bar, false, hide vertical scroll bar</param>
        public static void ShowVerticalScrollBar(ListView listview, bool showBar)
        {
            ShowScrollBar(listview.Handle, SB_VERT, showBar);
        }

    }
}
