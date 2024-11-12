/********************************************************************
	created:	2009/11/06
	created:	6:11:2009   9:36
	filename: 	D:\work\xfP2.0\xFP2.0\XRF\组件\VacuumPump.cs
	file path:	D:\work\xfP2.0\xFP2.0\XRF\组件
	file base:	VacuumPump
	file ext:	cs
	author:		
	
	purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 时间参数
    /// </summary>
    public class PumpCloseEvent : EventArgs
    {

    }

    /// <summary>
    /// 真空泵控制类
    /// </summary>
    public class VacuumPump
    {
        //private Timer timer;
        //private NetControl.NetControl port;
        Port port = null;
           
        private bool isOpen;
        private bool isTOpen;
        public bool Exist;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="portType">接口类型</param>
        public VacuumPump(Port port)
        {
            this.port = port;
        }

        public event EventHandler<PumpCloseEvent> OnClose;

        /// <summary>
        /// 电机的状态，显示是否打开
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
        }

        /// <summary>
        /// 打开真空泵
        /// </summary>
        public void Open()
        {
            try
            {
                isOpen = false; //port.OpenPump();

            }
            catch (System.Exception )
            {
                isOpen = false;
                return;
            }
        }

        /// <summary>
        /// 打开真空泵
        /// </summary>
        /// <param name="time">抽真空时间 单位（s）</param>
        public void Open(int time)
        {
            Open();
        }

        ///// <summary>
        ///// 定时器时间到
        ///// </summary>
        ///// <param name="state"></param>
        //private void OnTime(object state)
        //{
        //    timer.Dispose();
        //    Close();
        //}

        /// <summary>
        /// 关闭真空泵
        /// </summary>
        public void Close()
        {
            ////if (timer != null)
            ////{
            ////    timer.Dispose();
            ////}
            //try
            //{
            //    port.ClosePump();
            //}
            //catch (System.Exception )
            //{
            //    return;
            //}
            //isOpen = false;
            //if (OnClose != null)
            //{
            //    PumpCloseEvent e = new PumpCloseEvent();
            //    OnClose(this, e);
            //}
        }

        /// <summary>
        /// 打开真空泵
        /// </summary>
        public void TOpen()
        {
            try
            {
                lock (ob)
                {
                    isTOpen = port.OpenPump();
                }

            }
            catch (System.Exception)
            {
                isTOpen = false;
                return;
            }
        }

        object ob = new object();
        /// <summary>
        /// 关闭真空泵
        /// </summary>
        public void TClose()
        {
            //if (timer != null)
            //{
            //    timer.Dispose();
            //}
            try
            {
                lock (ob)
                {
                    port.ClosePump();
                }
            }
            catch (System.Exception)
            {
                return;
            }
            isTOpen = false;
            if (OnClose != null)
            {
                PumpCloseEvent e = new PumpCloseEvent();
                OnClose(this, e);
            }
        }
    }
}
