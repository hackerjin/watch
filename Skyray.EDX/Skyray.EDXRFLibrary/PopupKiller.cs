using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using System.Security.Cryptography;
using System.IO;

namespace Skyray.EDXRFLibrary
{
    /// <summary>
    /// 关掉当前进程中的弹出窗口
    /// </summary>
    public class PopupKiller
    {

        #region 字段

        // 侦查员，每隔固定时间就出去瞧瞧目标来了没
        private Timer m_Timer; // explicit namespace (Timer also in System.Threading) 

        // 狙击手杀人时间间隔
        private const double m_nInterval = 30;

        private string orgCaption = "fppro";

        //private IntPtr handle;

        private string newCaption = String.Empty;

        private string newText = String.Empty;

        /// <summary>
        /// 弹出窗体委托
        /// </summary>
        public FoundPopupWindow PopupWindowProcessMethods;

        #endregion


        #region 委托和事件

        /// <summary>
        /// 成功查杀
        /// </summary>
        //public delegate void PopupSuccessKilledEventHandler();

        ///// <summary>
        ///// 显示新提示
        ///// </summary>
        //public delegate void ShowNewTipEventHandler();

        /// <summary>
        /// 成功查杀事件
        /// </summary>
        //public event PopupSuccessKilledEventHandler PopupSuccessKilled;

        public delegate void FoundPopupWindow();
        #endregion


        #region 构造器

        /// <summary>
        /// 构造狙击手
        /// </summary>
        public PopupKiller()
        {
            m_Timer = new Timer();
            m_Timer.Elapsed += new ElapsedEventHandler(OnTimerKillPopup);
            m_Timer.Interval = m_nInterval; // for instance 3000 milliseconds 
            m_Timer.Enabled = false; // start timer 
        }

        /// <summary>
        /// 带参构造器
        /// </summary>
        /// <param name="title">目标窗口标题</param>
        public PopupKiller(String title)
            : this()
        {
            this.orgCaption = title;
        }

        #endregion


        #region 方法

        // 侦查员的望远镜，用来发现目标
        private bool FindPopupToKill(Process p)
        {
            // traverse all threads and enum all windows attached to the thread 
            NativeWIN32.EnumThreadProc callbackProc =
                new NativeWIN32.EnumThreadProc(MyEnumThreadWindowsProc);
            foreach (ProcessThread t in p.Threads)
            {
                //int threadId = t.Id;
                //NativeWIN32.EnumThreadProc callbackProc =
                //    new NativeWIN32.EnumThreadProc(MyEnumThreadWindowsProc);
                //NativeWIN32.EnumThreadWindows(threadId, callbackProc, IntPtr.Zero);
                if (!NativeWIN32.EnumThreadWindows(t.Id, callbackProc, IntPtr.Zero))
                {
                    if (PopupWindowProcessMethods != null)
                    {
                        PopupWindowProcessMethods();
                    }
                    return true;
                }
            }
            return false;
        }

        // callback used to enumerate Windows attached to one of the threads        
        private bool MyEnumThreadWindowsProc(IntPtr hwnd, IntPtr lParam)
        {
            // get window caption 
            NativeWIN32.STRINGBUFFER sLimitedLengthWindowTitle;
            NativeWIN32.GetWindowText(hwnd, out sLimitedLengthWindowTitle, 256);
            String sWindowTitle = sLimitedLengthWindowTitle.szText;

            //if (sWindowTitle.Length == 0) return false;
            if (sWindowTitle.StartsWith(orgCaption))//"fppro"))
            {
                NativeWIN32.SendMessage(hwnd, NativeWIN32.WM_SYSCOMMAND, NativeWIN32.SC_CLOSE, (int)IntPtr.Zero); // try soft kill 
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 给侦查员指派侦查间隔
        /// </summary>
        /// <param name="interval">时间间隔</param>
        public void SetInterval(double interval)
        {
            if (m_Timer == null) return;
            m_Timer.Interval = interval;
        }

        /// <summary>
        /// 派出杀手
        /// </summary>
        public void StartKill()
        {
            StartKill("fppro");
            //if (m_Timer == null) return;
            //OnTimerKillPopup(null, null);
            //m_Timer.Start();
        }

        /// <summary>
        /// 派出杀手
        /// </summary>
        /// <param name="title">目标</param>
        public void StartKill(string orgCaption)
        {
            //if (m_Timer == null) return;
            //this.orgCaption = caption;
            //OnTimerKillPopup(null, null);
            //m_Timer.Start();
            StartKill(orgCaption, null, null);
        }

        public void StartKill(string orgCaption, string newCaption, string newText)
        {
            if (m_Timer == null) return;

            this.orgCaption = orgCaption;
            this.newCaption = newCaption;
            this.newText = newText;
            OnTimerKillPopup(null, null);
            m_Timer.Start();
        }

        /// <summary>
        /// 唤回杀手
        /// </summary>
        public void EndKill()
        {
            if (m_Timer == null) return;
            m_Timer.Stop();
            m_Timer.Dispose();
            m_Timer = null;
        }

        // 时间到就执行杀人过程
        private void OnTimerKillPopup(Object sender, ElapsedEventArgs e)
        {
            m_Timer.Enabled = false; // pause the timer 
            if (FindPopupToKill(Process.GetCurrentProcess()))
            {
                // 显示新提示
                OnPopupSuccessKilled(this.newCaption, this.newText);
                //if (PopupSuccessKilled != null)
                //{
                //    PopupSuccessKilled();
                //}
            }
            if (m_Timer != null)
                m_Timer.Enabled = true;
        }

        private void OnPopupSuccessKilled(string newCaption, string newText)
        {
            if (newCaption == null || newText == null || newCaption == string.Empty || newText == string.Empty)
            {
                return;
            }
            else
            {
                //if (handle == null)
                //{
                System.Windows.Forms.MessageBox.Show(newText,
                    newCaption,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
                //}
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show(handle,
                //        newText, 
                //        newCaption, 
                //        System.Windows.Forms.MessageBoxButtons.OK, 
                //        System.Windows.Forms.MessageBoxIcon.Information);
                //}
            }
        }
        #endregion
    }
}
