using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Skyray.EDX.Common.Component
{
    public class COMThreadManager
    {
        private static Thread thd;
        private static Action act;
        private static AutoResetEvent evt;
        private static AutoResetEvent evt2;
        private volatile static bool thdExit;
        private static readonly object locker = new object();

        public static void SafeCall(Action action)
        {
            SafeCall(action, null);
        }

        public static void SafeCall(Action action, int? timeout)
        {
            lock (locker)
            {
                if (thd == null)
                {
                    thdExit = false;
                    evt = new AutoResetEvent(false);
                    evt2 = new AutoResetEvent(false);
                    thd = new Thread(() =>
                    {
                        while (!thdExit)
                        {
                            evt.WaitOne();
                            if ((act != null) && (!thdExit))
                            {
                                try
                                {
                                    act();
                                }
                                catch (Exception)
                                {
                                }
                                finally
                                {
                                    evt2.Set();
                                }
                            }
                        }

                    }) { IsBackground = true };
                    thd.SetApartmentState(ApartmentState.STA);
                    thd.Start();
                    Thread.Sleep(0);
                }
                act = action;
                evt.Set();
                if(timeout.HasValue)
                    evt2.WaitOne(timeout.Value);
                else
                {
                    evt2.WaitOne();
                }
            }
        }

        public static void ExitCOMThread()
        {
            try
            {
                thdExit = true;
                if (evt != null)
                    evt.Set();
            }
            catch (Exception)
            {

            }

        }
    }
}
