using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.Language;
using System.Windows.Forms;
using Skyray.Dog;

namespace Skyray.EDX.Common.Component
{
    public class MotorInstance
    {
        public static XMotor s_singleInstanceX;
        public static YMotor s_singleInstanceY;
        public static ZMotor s_singleInstanceZ;
        public static DeviceInterface interfaceDevice;

        public static string UpdateKeyFile
        {
            get { return Application.StartupPath + "\\KeyUpgrade\\KeyUpgrade.exe"; }
        }

        public void CreateInstance(MoterType moterType)
        {
            switch (moterType)
            {
                case MoterType.XMotor:
                    if (s_singleInstanceX == null)
                    {
                        s_singleInstanceX = new XMotor(WorkCurveHelper.deviceMeasure);
                    }
                    else
                    {
                        s_singleInstanceX.InitMotor();
                        s_singleInstanceX.InitXMotor(WorkCurveHelper.deviceMeasure);
                    }
                    break;
                case MoterType.YMotor:
                    if (s_singleInstanceY == null)
                    {
                        s_singleInstanceY = new YMotor(WorkCurveHelper.deviceMeasure);
                    }
                    else
                    {
                        s_singleInstanceY.InitMotor();
                        s_singleInstanceY.InitYMotor(WorkCurveHelper.deviceMeasure);
                    }
                    break;
                case MoterType.ZMotor:
                    if (s_singleInstanceZ == null)
                    {
                        s_singleInstanceZ = new ZMotor(WorkCurveHelper.deviceMeasure);
                    }
                    else
                    {
                        s_singleInstanceZ.InitMotor();
                        s_singleInstanceZ.InitZMotor(WorkCurveHelper.deviceMeasure);
                    }
                    break;
                default:
                    break;
            }
        }

        public static DeviceInterface CreateStoneInstance(InterfaceType type)
        {
            switch (type)
            {
                case InterfaceType.Usb:
                    interfaceDevice = new UsbDeviceInplement();
                    break;
                case InterfaceType.NetWork:
                    GetNetWork();
                    break;
                case InterfaceType.Dp5:
                    interfaceDevice = new Dp5DeviceInplement();
                    break;
                case InterfaceType.BlueTeeth:
                    interfaceDevice = new BlueTeethImplement();
                    break;
                case InterfaceType.Parllel:
                    interfaceDevice = new ParallelDeviceImplement();
                    break;
                case InterfaceType.Demo:
                    interfaceDevice = new DemoImplement();
                    return interfaceDevice;
                default:
                    break;
            }
            return interfaceDevice;
        }

        public static void GetNetWork()
        {
            if (interfaceDevice != null && interfaceDevice.port != null)
            {
                interfaceDevice.port.Dispose();
            }
            //else
            //{
                 interfaceDevice= new NetPortImplement();
                 //((NetPortImplement)interfaceDevice).NetPortType = WorkCurveHelper.IsNewDp5NetUse ? NetPortType.DPP100 : NetPortType.DMCA;
                 ((NetPortImplement)interfaceDevice).NetPortType = WorkCurveHelper.DeviceCurrent.IsDP5 ? NetPortType.DPP100 : NetPortType.DMCA;
            //}
        }

        public static string prePath;
        public static Dogy dog;

        //Add By WZW
        public static void LoadDLL(string keyPath,Device device)
        {
            string fileName = "TRUSBDev" + "." + device.PortType.ToString() +
                                            "." + device.UsbVersion.ToString() +
                                            "." + (device.IsPassward ? 0 : 1) + ".dll";

            LoadSpecifiedDll(AppDomain.CurrentDomain.BaseDirectory + fileName, keyPath, device);
        }

        public static void LoadSpecifiedDll(string strPath,string keyPath,Device device)
        {
            //if (prePath == null )//|| !prePath.Equals(strPath)
            {
                if (device.IsPassward && WorkCurveHelper.type != InterfaceType.NetWork && WorkCurveHelper.type != InterfaceType.BlueTeeth && !WorkCurveHelper.DemoInstance)
                {

                    bool isInSevenDays = false;
                    string surPlus = string.Empty;
                    int type = (int)HardwareDog.SNConfirm(WorkCurveHelper.snFilePath, out isInSevenDays, out surPlus);
                    if (type == -3 || type == -2)
                    {
                        CheckDog(keyPath);//加密仪器才检查密码狗

                    }
                }
                prePath = strPath;
            }
            if (WorkCurveHelper.deviceMeasure.interfacce.port != null)
            {
                WorkCurveHelper.deviceMeasure.interfacce.port.StrDllPath = strPath;
                WorkCurveHelper.deviceMeasure.interfacce.port.DllVersion = device.UsbVersion;
                WorkCurveHelper.deviceMeasure.interfacce.GetReturnParams();
                WorkCurveHelper.Volumngreen = WorkCurveHelper.deviceMeasure.interfacce.ReturnVacuum;
            }
        }

        public static void CheckDog(ref string str)
        {
            if (dog != null && !dog.IsUnLocked) //Dog
            {
                if (dog.LeftDays == 1)
                {
                    //提示信息按小时
                    str += "-- " + string.Format(Info.LeftHour,dog.LeftHours);
                }
                //如果在1到30天
                else if ((dog.LeftDays > 1) && (dog.LeftDays <= 30))
                {
                    //提示信息按天
                    str+="-- " + string.Format(Info.LeftDay,dog.LeftDays);
                }
            }
        }

        public static void CheckDog(string keyPath)
        {
            try
            {
                if (dog == null)
                {
                    dog = new Dogy();
                    dog.Start();
                }
                if (!dog.IsFoundPasswordDog()) //没有找到加密狗
                {
                    Lang.Model.ChangeLanguage();
                    Msg.Show(Info.NotFindDogy);
                    System.Environment.Exit(0);
                }
                if (!dog.IsUnLocked) //Dog
                {
                    if (dog.LeftSeconds == 0)//密码狗到期
                    {
                        Msg.Show(Info.PasswardDogyExpire);
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = keyPath;
                        process.Start();
                        System.Environment.Exit(0);
                    }
                    else if (dog.LeftSeconds < 0)
                    {

                    }
                    else if (dog.LeftDays == 1) //剩余一天提示
                    {
                        Msg.Show(Info.SuplusDogyTime + dog.LeftHours);
                    }
                    else if ((dog.LeftDays <= 7) && (dog.LeftDays > 1)) //剩余一周提示
                    {
                        Msg.Show(Info.SuplusDayTime + dog.LeftDays);
                    }
                  
                }
            }
            catch (System.Exception )
            {
                //Msg.Show(ex.Message);
                Environment.Exit(0); //异常退出
            }
        }

        public static bool CheckDog()
        {
            if (dog == null)
            {
                dog = new Dogy();
                dog.Start();
            }
            if (!dog.IsFoundPasswordDog()) //没有找到加密狗
            {
                Lang.Model.ChangeLanguage();
                Msg.Show(Info.NotFindDogy);
                return false;
            }
            if (!dog.IsUnLocked) //Dog
            {
                if (dog.LeftSeconds == 0)//密码狗到期
                {
                    Msg.Show(Info.PasswardDogyExpire);
                    return false;
                }
                //else if (dog.LeftSeconds < 0)
                //{

                //}
                //else if (dog.LeftDays == 1) //剩余一天提示
                //{
                //    Msg.Show(Info.SuplusDogyTime + dog.LeftHours);
                //}
                //else if ((dog.LeftDays <= 7) && (dog.LeftDays > 1)) //剩余一周提示
                //{
                //    Msg.Show(Info.SuplusDayTime + dog.LeftDays);
                //}
            }
            
            return true;
        }
    }

    public enum MoterType
    {
        None,
        XMotor,
        YMotor,
        ZMotor
    }

    public enum PortType
    {
        Usb,
        NetWork
    }

    public struct MessageShow
    {
        public int dwData;
        public int cbData;
        public IntPtr lpData;

    }
}

