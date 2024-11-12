/********************************************************************
	created:	2009/11/06
	created:	6:11:2009   9:36
	filename: 	D:\work\xfP2.0\xFP2.0\XRF\���\VacuumPump.cs
	file path:	D:\work\xfP2.0\xFP2.0\XRF\���
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
    /// ʱ�����
    /// </summary>
    public class PumpCloseEvent : EventArgs
    {

    }

    /// <summary>
    /// ��ձÿ�����
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
        /// ���캯��
        /// </summary>
        /// <param name="portType">�ӿ�����</param>
        public VacuumPump(Port port)
        {
            this.port = port;
        }

        public event EventHandler<PumpCloseEvent> OnClose;

        /// <summary>
        /// �����״̬����ʾ�Ƿ��
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
        }

        /// <summary>
        /// ����ձ�
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
        /// ����ձ�
        /// </summary>
        /// <param name="time">�����ʱ�� ��λ��s��</param>
        public void Open(int time)
        {
            Open();
        }

        ///// <summary>
        ///// ��ʱ��ʱ�䵽
        ///// </summary>
        ///// <param name="state"></param>
        //private void OnTime(object state)
        //{
        //    timer.Dispose();
        //    Close();
        //}

        /// <summary>
        /// �ر���ձ�
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
        /// ����ձ�
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
        /// �ر���ձ�
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
