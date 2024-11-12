using System;
using System.Drawing;

namespace Skyray.API
{
    public class AppBarInfo
    {
        private APPBARDATA m_data;

        // Appbar messages
        private const int ABM_NEW = 0x00000000;
        private const int ABM_REMOVE = 0x00000001;
        private const int ABM_QUERYPOS = 0x00000002;
        private const int ABM_SETPOS = 0x00000003;
        private const int ABM_GETSTATE = 0x00000004;
        private const int ABM_GETTASKBARPOS = 0x00000005;
        private const int ABM_ACTIVATE = 0x00000006;  // lParam == TRUE/FALSE means activate/deactivate
        private const int ABM_GETAUTOHIDEBAR = 0x00000007;
        private const int ABM_SETAUTOHIDEBAR = 0x00000008;

        // Appbar edge constants
        private const int ABE_LEFT = 0;
        private const int ABE_TOP = 1;
        private const int ABE_RIGHT = 2;
        private const int ABE_BOTTOM = 3;

        // SystemParametersInfo constants
        private const System.UInt32 SPI_GETWORKAREA = 0x0030;

        public enum ScreenEdge
        {
            Undefined = -1,
            Left = ABE_LEFT,
            Top = ABE_TOP,
            Right = ABE_RIGHT,
            Bottom = ABE_BOTTOM
        }

        public ScreenEdge Edge
        {
            get { return (ScreenEdge)m_data.uEdge; }
        }

        public Rectangle WorkArea
        {
            get
            {
                Int32 bResult = 0;
                RECT rc = new RECT();
                IntPtr rawRect = System.Runtime.InteropServices.Marshal.AllocHGlobal(System.Runtime.InteropServices.Marshal.SizeOf(rc));
                bResult = WinMethod.SystemParametersInfo(SPI_GETWORKAREA, 0, rawRect, 0);
                rc = (RECT)System.Runtime.InteropServices.Marshal.PtrToStructure(rawRect, rc.GetType());

                if (bResult == 1)
                {
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(rawRect);
                    return new Rectangle(rc.Left, rc.Top, rc.Right - rc.Left, rc.Bottom - rc.Top);
                }

                return new Rectangle(0, 0, 0, 0);
            }
        }

        public void GetPosition(string strClassName, string strWindowName)
        {
            m_data = new APPBARDATA();
            m_data.cbSize = (UInt32)System.Runtime.InteropServices.Marshal.SizeOf(m_data.GetType());

            IntPtr hWnd = WinMethod.FindWindow(strClassName, strWindowName);

            if (hWnd != IntPtr.Zero)
            {
                UInt32 uResult = WinMethod.SHAppBarMessage(ABM_GETTASKBARPOS, ref m_data);

                if (uResult == 1)
                {
                }
                else
                {
                    throw new Exception("Failed to communicate with the given AppBar");
                }
            }
            else
            {
                throw new Exception("Failed to find an AppBar that matched the given criteria");
            }
        }

        public void GetSystemTaskBarPosition()
        {
            GetPosition("Shell_TrayWnd", null);
        }
    }
}
