using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.Controls.Extension;
using System.IO;
using System.Windows.Forms;

namespace Skyray.EDX.Common.Component
{
    public class SpecMessage
    {
        private object theWholeObj = new object();
        public Thread excuteThead = null;
        private XRFChart xrfChart = null;
        public List<MessageFormat> localMesage = new List<MessageFormat>();
        //private AutoResetEvent doWork = new AutoResetEvent(false);
        public bool cancle = false;
       
        public SpecMessage()
        { }

        public void StartThread(XRFChart chart)
        {
            System.Diagnostics.Trace.WriteLine("Enter Thread" + DateTime.Now.ToString());
           
            lock (theWholeObj)
            {
                this.xrfChart = chart;
                excuteThead = new Thread(new ThreadStart(ExcuteProcess));
                excuteThead.IsBackground = true;
                excuteThead.Name = "Spectrum Message";
                excuteThead.Start();
            }
        }

        public void ExcuteProcess()
        {
            if (this.xrfChart == null)
                return;
            while (!cancle && this.xrfChart != null)
            {
                //this.doWork.WaitOne(TimeSpan.FromMilliseconds(100), false);
                Thread.Sleep(100);
                //this.doWork.WaitOne();
                MessageFormat reportObject = null;
                lock (this.theWholeObj)
                {
                    if (localMesage.Count > 0)
                    {
                        reportObject = localMesage[0];
                        localMesage.Remove(reportObject);
                    }
                }
                if (reportObject == null)
                    continue;
                if (this.xrfChart != null && this.xrfChart.IsHandleCreated && this.xrfChart.InvokeRequired)
                {
                    this.xrfChart.Invoke(new WriteInfo(WirteSpecMessage), reportObject);
                }
            }
        }

        public void WirteSpecMessage(MessageFormat message)
        {
            if (this.xrfChart == null)
                return;
            if (message.Type == 0)
                this.xrfChart.WriteInformation(message.messageContext);
            else
            {
                if (message.messageContext != null)
                    this.xrfChart.WriteInformation(message.messageContext);
            }
        }
    }

    public delegate void WriteInfo(MessageFormat message);

    public class MessageFormat
    {
        //消息内容
        public string messageContext { get; set; }

        //消息类型
        public int Type { get; set; }

        public MessageFormat()
        { }

        public MessageFormat(string messageContext, int type)
        {
            this.messageContext = messageContext;
            this.Type = type;
        }
    }
}
