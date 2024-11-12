using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common.Component;
using System.Windows.Forms;
using XRFNetLib;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 抽象类：与底层的通讯接口函数集
    /// </summary>
    public abstract class Port
    {
        public bool NetReadOK = false;
        public DllType Dll;
        public UsbVersion DllVersion = UsbVersion.Usb2;

        private bool _connectState;

        public bool ConnectState
        {
            get
            {
                return this is NetPort ? _connectState : true;
            }
            set
            {
                if (!value)
                    WorkCurveHelper.dataStore.InputDiscretes[1] = false;
                else
                    WorkCurveHelper.dataStore.InputDiscretes[1] = true;
                if (!value && onDisConnect != null)
                    onDisConnect();
                _connectState = value;
            }
        }

        private string strDllPath;

        public string StrDllPath
        {
            get { return strDllPath; }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    strDllPath = value;
                    if (hModule != 0)
                        DLLWrapper.FreeLibrary(hModule);
                    try
                    {
                        hModule = DLLWrapper.LoadLibrary(value);
                    }
                    catch (Exception e)
                    {
                        Msg.Show(e.Message);
                    }
                }
            }
        }
        public void FreeLibrary()
        {
            if (hModule != 0)
                DLLWrapper.FreeLibrary(hModule);
        }
        public string strIp = "";
        public int hModule;
        public ushort uPort = 0;
        public byte[] byteIp;
        public uint uQuickCount = 0;
        public uint uSlowCount = 0;
        public uint uSystemTime = 0;
        public uint uLoaclTime = 0;
        public int dataLength;
        public int[] specData;
        public delegate void DisConnect();
        public event DisConnect onDisConnect;
        public static Action ActionAfterDeviceInfoNotGot;
        public static Action ActionAfterDeviceInfoChecked;
        public static Action ActionReconnect;


        #region 抽象方法

        public abstract void OpenXRayTubHV();

        public abstract void CloseXRayTubHV();

        /// <summary>
        /// 设置网口IP
        /// </summary>
        /// <returns></returns>
        public abstract bool IPSettings(string IP, string SubNet, string GateWay, string DNS);
        public abstract bool SetSurfaceSource(ushort firstLight, ushort secondLight, ushort thirdLight, ushort fourthLight);
        /// <summary>
        /// 连接设备
        /// </summary>
        /// <returns></returns>
        public abstract bool Connect();

        /// <summary>
        /// 断开与设备的连接
        /// </summary>
        /// <returns></returns>
        public abstract bool Disconnect();

        /// <summary>
        /// 打开高压
        /// </summary>
        /// <returns></returns>
        public abstract bool OpenVoltage();

        /// <summary>
        /// 关闭高压
        /// </summary>
        /// <returns></returns>
        public abstract bool CloseVoltage();

        /// <summary>
        /// 打开真空泵开始抽真空
        /// </summary>
        /// <returns></returns>
        public abstract bool OpenPump();

        public abstract bool GetVacuum(uint uType, out uint uVacuum);

        /// <summary>
        /// 关闭真空泵
        /// </summary>
        /// <returns></returns>
        public abstract bool ClosePump();

        /// <summary>
        /// 开高压指示灯
        /// </summary>
        /// <returns></returns>
        public abstract bool OpenVoltageLamp();

        /// <summary>
        /// 关闭高压指示灯
        /// </summary>
        /// <returns></returns>
        public abstract bool CloseVoltageLamp();

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public abstract bool GetData(int[] data);

        /// <summary>
        /// 控制电机
        /// </summary>
        /// <param name="index">电机号</param>
        /// <param name="dir">电机方向（0，1）</param>
        /// <param name="step">移动步长</param>
        /// <param name="swtStop">遇到接近开关时是否停止。true＝停止</param>
        /// <param name="speedGear">速度控制</param>
        /// <returns></returns>
        public abstract bool MotorControl(int index, int dir, int step, bool swtStop, int speedGear);

        public abstract bool MotorIsIdelAll(int X, int Y);
        /// <summary>
        /// 电机是否空闲
        /// </summary>
        /// <param name="index" >电机号 </param>
        /// <returns></returns>
        public abstract bool MotorIsIdel(int index);

        public abstract bool GetMotorInfo(ref int info);

        public abstract bool GetParams(ref int Voltage, ref int Current, ref int Temperature, ref int Vacuum, ref bool IsOpen);

        public abstract void setFPGAParam(byte bBaseResume, byte bBaseLimit, byte bHeapUP, byte bRate, byte bCoarse, uint uFine, uint uTime, byte bUPTime, byte bWidthTime, byte bSlowLimit, double dIntercept);

        /// <summary>
        /// 设置管压，管流，主放，粗放
        /// </summary>
        /// <returns></returns>
        public abstract bool SetParam(int tubVoltage, int tubCurrent, int gain, int fineGain);

        public abstract void setParam(double tubVoltage, double tubCurrent);

        public abstract void getParam(out double uVoltage, out double uCurrent, out int iUncover);

        public abstract void AllowUncover(bool allowUncover);

        public virtual bool coverClosed { get; private set; }

        public abstract int GetKeyInfo(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods);

        public abstract void OpenElectromagnet();//3200L

        public abstract void CloseElectromagnet();//3200L

        public abstract void OpenMagnet();

        public abstract void CloseMagnet();

        public abstract void ConsolePrint(string str);

        public abstract void Dispose();

        public abstract int GetDeviceKeyInfo();

        public abstract bool OpenDevice();

        public abstract bool GetDoubleVacuum(uint uType, out uint pUpVacuum, out uint pDownVacuum);

        public abstract LightStatus GetLightShutState(int index);

        public abstract void GetChamberStatus(ref int steps, ref int chambeIndex, ref bool isturestep, ref bool initialed, ref bool isbusy);
        public abstract int ResetChamber(int ID, int DirectionFlag, int DefSpeed);
        public abstract bool LockHV(bool bLock);

        public abstract void OpenHeightLaser(bool bOpen);
        public abstract void MoveZAutoMotor(int iSpeed);
        public abstract void GetSwitch();


        #endregion
    }
    public enum LightStatus
    {
        Open,
        Close,
        Middle,
        Fail
    }
}
