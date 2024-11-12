using System;
using System.Windows.Forms;
using Skyray.UC;
using Skyray.EDX.Common;
using System.Threading;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Lephone.Data.Common;
using Skyray.EDXRFLibrary;
using Skyray.Dog;
namespace Skyray.Thick
{
    /// <summary>
    /// 程序入口类
    /// </summary>
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            DifferenceDevice.IsThick = true;
            InitSysParams.InitLang();
            //sw.Stop();
            //string showtime = sw.Elapsed.TotalMilliseconds.ToString();
            //Console.WriteLine("启动占用时间：{0}ms", sw.Elapsed.TotalMilliseconds);
            //sw.Reset();
            //sw.Start();
            //393ms
            bool createdNew = false;//记录用户是否已经打开了应用程序      
            Mutex m = new Mutex(true, "Thick", out createdNew);//创建互斥变量
            if (!createdNew)
            {
                Msg.Show(Info.OnlySingleInstance);
                return;
            }
            bool resultFlag = false;
            try
            {
                MainExtracted(1);
                InitSysParams.InitLog();
                if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["DemoInstance"]))
                {
                    bool.TryParse(ConfigurationSettings.AppSettings["DemoInstance"], out WorkCurveHelper.DemoInstance);
                }

                if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["ThickLimit"]))
                {
                    int.TryParse(ConfigurationSettings.AppSettings["ThickLimit"], out WorkCurveHelper.isThickLimit);
                }
                //sw.Stop();
                //string showtime1 = sw.Elapsed.TotalMilliseconds.ToString();
                //Console.WriteLine("启动占用时间1：{0}ms", sw.Elapsed.TotalMilliseconds);
                resultFlag = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialize Exception: " + ex.ToString());
                resultFlag = false;
                return;
            }
            finally
            {
                MainExtracted(0);
                if (!resultFlag)
                {
                    m.ReleaseMutex();
                }
            }

            bool ChangeUser = bool.Parse(ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/ChangeUser"));
            if (!ChangeUser)
            {


                if (new FrmLogon().ShowDialog() == DialogResult.OK)
                {

                    if (GP.CurrentUser.Role.RoleType.ToString() == "2")
                    {

                        WorkCurveHelper.curFrmThick = new FrmThickNew();
                        WorkCurveHelper.curFrmThickType = typeof(FrmThickNew);
                        Application.Run(WorkCurveHelper.curFrmThick);


                    }
                    else
                    {

                        WorkCurveHelper.curFrmThick = new FrmThick();
                        WorkCurveHelper.curFrmThickType = typeof(FrmThick);
                        Application.Run(WorkCurveHelper.curFrmThick);
                    }
                }
                m.ReleaseMutex();
            }
            else
            {

                FrmLogon.LoadP();
                FrmLogon.RegistrySoftWarePath();

                bool isInSevenDays = false;
                string surPlus = string.Empty;
                int type = (int)HardwareDog.SNConfirm(WorkCurveHelper.snFilePath, out isInSevenDays, out surPlus);
                HardwareDog.Deadline = surPlus;   //deadline赋值
                if (type == 1)
                {
                    FrmHardwareDogCA fhd = new FrmHardwareDogCA(type);
                    fhd.ShowDialog();
                }

                string CurUser = ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/CurUser");


                if (CurUser == "Admin")
                {


                    GP.UserName = FrmLogon.userName = "用户";

                }
                else
                {

                    GP.UserName = FrmLogon.userName = "Admin";
                }

                ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/CurUser", GP.UserName);
                string sql = "select * from User order by Id desc";
                DbObjectList<User> lstUser = User.FindBySql(sql);
                User user = lstUser.Find(w => w.Name == FrmLogon.userName);
                if (user != null)
                {
                    GP.CurrentUser = user;
                }

                if (GP.CurrentUser.Role.RoleType.ToString() == "2")
                {

                    WorkCurveHelper.curFrmThick = new FrmThickNew();
                    Application.Run(WorkCurveHelper.curFrmThick);


                }
                else
                {

                    WorkCurveHelper.curFrmThick = new FrmThick();
                    Application.Run(WorkCurveHelper.curFrmThick);
                }

                m.ReleaseMutex();




            }


        }

        public static void RegisterDll()
        {
            try
            {
                string arguments = "/user:Administrator \"cmd /K " + " regsvr32 /s xrfnet.dll\"";
                ProcessStartInfo myProcessStartInfo = new ProcessStartInfo("cmd.exe", arguments);
                myProcessStartInfo.UseShellExecute = false;
                myProcessStartInfo.CreateNoWindow = true;
                myProcessStartInfo.RedirectStandardOutput = true;
                myProcessStartInfo.RedirectStandardInput = true;
                Process myProcess = Process.Start(myProcessStartInfo);
                myProcess.StandardInput.WriteLine("Exit");
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// 显示和隐藏加载中
        /// </summary>
        /// <param name="type">0表示隐藏, 1表示显示</param>
        public static void MainExtracted(uint type)
        {
            //if (!IsProcessStarted("Skyray.NewProcessBar"))
            //{
            //    ProcessStartInfo startInfo = new ProcessStartInfo();
            //    startInfo.FileName = "Skyray.NewProcessBar.exe";
            //    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //    System.Diagnostics.Process.Start(startInfo);
            //}
            //IntPtr findPtr = FindWindow(null, "ProcessBox");
            //ShowWindow(findPtr, type);
            //findPtr = IntPtr.Zero; //释放句柄
        }

        public static bool IsProcessStarted(string processName)
        {
            Process[] temp = Process.GetProcessesByName(processName);
            return (temp != null && temp.Length > 0);

        }

        #region
        //public static void MainExtracted()
        //{
        //    if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.IsProcessStarted("Skyray.NewProcessBar")) return;
        //    ProcessStartInfo startInfo = new ProcessStartInfo();
        //    startInfo.FileName = "Skyray.NewProcessBar.exe";
        //    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //    System.Diagnostics.Process.Start(startInfo);
        //    IntPtr findPtr = FindWindow(null, "ProcessBox");
        //    ShowWindow(findPtr, 0);

        //}

        //public static void RegisterDll()
        //{
        //    try
        //    {
        //        string arguments = "/user:Administrator \"cmd /K " + " regsvr32 -s xrfnet.dll \"";
        //        Process myProcess = new Process();
        //        ProcessStartInfo myProcessStartInfo = new ProcessStartInfo("cmd.exe", arguments);
        //        myProcessStartInfo.UseShellExecute = false;
        //        myProcessStartInfo.CreateNoWindow = true;
        //        myProcessStartInfo.RedirectStandardOutput = true;
        //        myProcess.StartInfo = myProcessStartInfo;
        //        myProcess.Start();
        //        myProcess.Close();
        //        //Msg.Show("Dll Registered");
        //    }
        //    catch (Exception ex)
        //    {
        //        //Msg.Show("Register dll exception: " + ex.ToString());
        //    }
        //}
        #endregion

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
    }
}
