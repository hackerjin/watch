using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common.Component
{
    public class Super2400Port : UsbPort
    {
        public override void OpenXRayTubHV()
        {
            bXRayTubeSel(1);
            //先设管压
            int i = 0;
            while (i < 20 && i + 5 < 20)
            {
                i += 5;
                base.SetParam(i, 0, 13, 8200);
                System.Threading.Thread.Sleep(100);//管压设置梯度为100
            }
            base.SetParam(20, 0, 13, 8200);
            //再设管流
            i = 0;
            while (i < 2000 && i + 500 < 2000)
            {
                i += 500;
                base.SetParam(20, (int)Math.Round(i / 8.0), 13, 8200);
                System.Threading.Thread.Sleep(200);//管流设置梯度为200
            }
            base.SetParam(20, (int)Math.Round(2000 / 8.0), 13, 8200);
        }

        public override void CloseXRayTubHV()
        {
            //先降管流
            int i = 2000;
            while (i - 500 > 0)
            {
                i -= 500;
                base.SetParam(20, (int)Math.Round(i / 8.0), 13, 8200);
                System.Threading.Thread.Sleep(200);//管流设置梯度为200
            }
            base.SetParam(20, 0, 13, 8200);
            //再降管压
            i = 20;
            while (i - 5 > 0)
            {
                i -= 5;
                base.SetParam(i, 0, 13, 8200);
                System.Threading.Thread.Sleep(100);//管压设置梯度为100
            }
            base.SetParam(0, 0, 13, 8200);
            base.bXRayTubeSel(0);
        }
    }
}
