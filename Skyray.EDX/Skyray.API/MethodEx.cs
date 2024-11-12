using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Skyray.API
{
    public class MethodEx
    {
        public static int GetLastError()
        {
            return System.Runtime.InteropServices.Marshal.GetLastWin32Error();
        }

        /// <summary>
        /// 根据提示框图标不同播放不同声音
        /// </summary>
        /// <param name="icon"></param>
        public static void PlaySoundByIcon(MessageBoxIcon icon)
        {
            if (icon == MessageBoxIcon.Question)
                WinMethod.PlaySound("SystemQuestion", IntPtr.Zero, PlaySoundFlags.SND_ASYNC);
            else if (icon == MessageBoxIcon.Asterisk)
                WinMethod.PlaySound("SystemAsterisk", IntPtr.Zero, PlaySoundFlags.SND_ASYNC);
            else if (icon == MessageBoxIcon.Exclamation)
                WinMethod.PlaySound("SystemExclamation", IntPtr.Zero, PlaySoundFlags.SND_ASYNC);
            else if (icon == MessageBoxIcon.Hand || icon == MessageBoxIcon.Error || icon == MessageBoxIcon.Stop)
                WinMethod.PlaySound("SystemHand", IntPtr.Zero, PlaySoundFlags.SND_ASYNC);
            else
                WinMethod.PlaySound("SystemNotification", IntPtr.Zero, PlaySoundFlags.SND_ASYNC);
        }


        /// <summary>
        /// 窗体前置
        /// </summary>
        /// <param name="windowPtr">窗体句柄</param>
        /// <returns></returns>
        public static bool BringWindowToFront(IntPtr windowPtr)
        {
            WinMethod.ShowWindowAsync(windowPtr, (int)ShowWindowStyles.SW_SHOWNORMAL); //确保窗口没有被最小化或最大化           
            return WinMethod.SetForegroundWindow(windowPtr);//窗体前置
        }

        /// <summary>
        /// 获取窗体lpClassName
        /// </summary>
        /// <param name="windowPtr">窗体句柄</param>
        /// <returns></returns>
        public static string GetlpClassName(IntPtr windowPtr)
        {
            StringBuilder ClassName = new StringBuilder(256);
            int ret = WinMethod.GetClassName(windowPtr, ClassName, ClassName.Capacity);
            return ClassName.ToString();
        }

        /// <summary>
        /// 将窗体置于屏幕中间,调用此函数之前请确保窗体没有最小化
        /// </summary>
        /// <param name="windowPtr">窗体句柄</param>
        public static void SetFormCenterScreen(IntPtr windowPtr)
        {
            RECT rc;
            WinMethod.GetWindowRect(windowPtr, out rc);//获取窗体大小，当窗体最小化时，获取的宽高为窗体标题栏的宽高

            int ScreenWidth = SystemInformation.VirtualScreen.Width;//屏幕宽
            int ScreenHeight = SystemInformation.VirtualScreen.Height;//屏幕高
            int WindowWidth = rc.Right - rc.Left;//窗体宽
            int WindowHeight = rc.Bottom - rc.Top;//窗体高

            WinMethod.MoveWindow(windowPtr//窗体句柄
                , (ScreenWidth - WindowWidth) / 2  //窗体位置X
                , (ScreenHeight - WindowHeight) / 2 //窗体位置Y
                , WindowWidth, WindowHeight //窗体宽高
                , true//重绘窗体
                );
        }

        /// <summary>
        /// 利用Api函数抓图
        /// </summary>
        /// <param name="control"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static bool CaptureWindow(System.Windows.Forms.Control control,
                        ref System.Drawing.Bitmap bitmap)
        {
            Graphics g2 = Graphics.FromImage(bitmap);

            //PRF_CHILDREN // PRF_NONCLIENT
            int meint = (int)(KnownParam.PRF_CLIENT | KnownParam.PRF_ERASEBKGND);
            //| PRF_OWNED ); //  );
            System.IntPtr meptr = new System.IntPtr(meint);
            System.IntPtr hdc = g2.GetHdc();
            WinMethod.SendMessage(control.Handle, (int)WinMsgs.WM_PRINT, hdc, meptr);
            g2.ReleaseHdc(hdc);
            g2.Dispose();
            return true;
        }

        public static void DrawDragRect(Graphics gs, Rectangle rect, Rectangle rectLast)
        {
            IntPtr hdc = new IntPtr();
            IntPtr hObject = new IntPtr();
            IntPtr ptr3 = new IntPtr();
            POINT lpPoint = new POINT();
            hdc = gs.GetHdc();
            drawingMode fnDrawMode = WinMethod.SetROP2(hdc, drawingMode.R2_NOTXORPEN);
            hObject = WinMethod.CreatePen(0, 1, 0);
            ptr3 = WinMethod.SelectObject(hdc, hObject);
            if (!rectLast.IsEmpty)
            {
                WinMethod.MoveToEx(hdc, rectLast.Left, rectLast.Top, ref lpPoint);
                WinMethod.LineTo(hdc, rectLast.Right, rectLast.Top);
                WinMethod.LineTo(hdc, rectLast.Right, rectLast.Bottom);
                WinMethod.LineTo(hdc, rectLast.Left, rectLast.Bottom);
                WinMethod.LineTo(hdc, rectLast.Left, rectLast.Top);
            }
            WinMethod.MoveToEx(hdc, rect.Left, rect.Top, ref lpPoint);
            WinMethod.LineTo(hdc, rect.Right, rect.Top);
            WinMethod.LineTo(hdc, rect.Right, rect.Bottom);
            WinMethod.LineTo(hdc, rect.Left, rect.Bottom);
            WinMethod.LineTo(hdc, rect.Left, rect.Top);
            WinMethod.SelectObject(hdc, ptr3);
            WinMethod.DeleteObject(hObject);
            WinMethod.SetROP2(hdc, fnDrawMode);
            gs.ReleaseHdc(hdc);
        }

        //public static void DrawDragRect(Graphics gs, Point pt, Point ptLast)
        //{
        //    IntPtr hdc = new IntPtr();
        //    IntPtr hObject = new IntPtr();
        //    IntPtr ptr3 = new IntPtr();
        //    POINT lpPoint = new POINT();
        //    hdc = gs.GetHdc();
        //    drawingMode fnDrawMode = WinMethod.SetROP2(hdc, drawingMode.R2_NOTXORPEN);
        //    hObject = WinMethod.CreatePen(0, 1, 0);
        //    ptr3 = WinMethod.SelectObject(hdc, hObject);
        //    if (!ptLast.IsEmpty)
        //    {
        //        WinMethod.MoveToEx(hdc, rectLast.Left, rectLast.Top, ref lpPoint);
        //        WinMethod.LineTo(hdc, rectLast.Right, rectLast.Top);
        //        WinMethod.LineTo(hdc, rectLast.Right, rectLast.Bottom);
        //        WinMethod.LineTo(hdc, rectLast.Left, rectLast.Bottom);
        //        WinMethod.LineTo(hdc, rectLast.Left, rectLast.Top);
        //    }
        //    WinMethod.MoveToEx(hdc, rect.Left, rect.Top, ref lpPoint);
        //    WinMethod.LineTo(hdc, rect.Right, rect.Top);
        //    WinMethod.LineTo(hdc, rect.Right, rect.Bottom);
        //    WinMethod.LineTo(hdc, rect.Left, rect.Bottom);
        //    WinMethod.LineTo(hdc, rect.Left, rect.Top);
        //    WinMethod.SelectObject(hdc, ptr3);
        //    WinMethod.DeleteObject(hObject);
        //    WinMethod.SetROP2(hdc, fnDrawMode);
        //    gs.ReleaseHdc(hdc);
        //}


        public static int GET_X_LPARAM(int lParam)
        {
            return (lParam & 0xffff);
        }


        public static int GET_Y_LPARAM(int lParam)
        {
            return (lParam >> 16);
        }

        public static Point GetPointFromLPARAM(int lParam)
        {
            return new Point(GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam));
        }

        public static int LOW_ORDER(int param)
        {
            return (param & 0xffff);
        }

        public static int HIGH_ORDER(int param)
        {
            return (param >> 16);
        }
    }
}
