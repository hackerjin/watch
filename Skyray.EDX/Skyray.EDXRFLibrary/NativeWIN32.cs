using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.EDXRFLibrary
{
    /// <summary>
    /// sealed不允许类被继承
    /// </summary>
    public sealed class NativeWIN32
    {

        #region 常量

        /// <summary>
        /// 系统命令
        /// </summary>
        public const int WM_SYSCOMMAND = 0x0112;

        /// <summary>
        /// 窗口关闭
        /// </summary>
        public const int SC_CLOSE = 0xF060;

        #endregion


        #region 构造器

        // 私有构造函数不允许实例化
        private NativeWIN32()
        { }

        #endregion


        #region 委托

        public delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);

        #endregion


        #region 字符串缓冲

        /// <summary>
        /// used for an output LPCTSTR parameter on a method call
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STRINGBUFFER
        {
            /// <summary>
            /// 用于存储字串
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szText;
        }

        #endregion


        #region 外部方法(win32)

        /// <summary>
        /// 多线程窗体
        /// </summary>
        /// <param name="threadId">线程ID</param>
        /// <param name="pfnEnum">多线程处理对象</param>
        /// <param name="lParam">lParam参数</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EnumThreadWindows(int threadId, EnumThreadProc pfnEnum, IntPtr lParam);

        /// <summary>
        /// 取得窗口标题
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="ClassName">字符串缓冲对象</param>
        /// <param name="nMaxCount">最大计数</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, out STRINGBUFFER ClassName, int nMaxCount);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        #endregion
    }
}
