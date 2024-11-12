using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using System.Runtime.InteropServices;

namespace Skyray.EDX.Common
{
    public class UsbPort : Port
    {
        public static bool UsbIdel = true;

        public UsbPort()
        {
        }


        private int iGetDevVersion()
        {

            if (Dll == DllType.DLL4)
            {
                UsbPortDelegate.iGetDevVersion iGetDevVersion4 = (UsbPortDelegate.iGetDevVersion)DLLWrapper.GetFunctionAddress(hModule, "iGetDevVersion", typeof(UsbPortDelegate.iGetDevVersion));
                if (iGetDevVersion4 == null)
                {
                    DLLWrapper.FreeLibrary(hModule);
                    throw new Exception(Info.LoadLibrayFailure);
                }
                else
                    try
                    {
                        return iGetDevVersion4();
                    }
                    catch
                    {
                        Environment.Exit(0);
                    }
            }
            else if (Dll == DllType.DLL3)
            {
                UsbPortDelegate.iGetDevVersion3 iGetDevVersion3 = (UsbPortDelegate.iGetDevVersion3)DLLWrapper.GetFunctionAddress(hModule, "iGetDevVersion3", typeof(UsbPortDelegate.iGetDevVersion3));
                if (iGetDevVersion3 == null)
                {
                    DLLWrapper.FreeLibrary(hModule);
                    throw new Exception(Info.LoadLibrayFailure);
                }
                else
                {
                    try
                    {
                        return iGetDevVersion3();
                    }
                    catch
                    {
                        Environment.Exit(0);
                    }
                }
            }
            return 0;
            //return 1;
        }

        public int bGetState(ref int iVoltage, ref int iCurrent, ref int iTemp, ref int iVacuum, ref int iCover)
        {
            int ret = 1;
            try
            {
                if (Dll == DllType.DLL4)
                {
                    UsbPortDelegate.bGetState4 bGetStatets = (UsbPortDelegate.bGetState4)DLLWrapper.GetFunctionAddress(hModule, "bGetState", typeof(UsbPortDelegate.bGetState4));
                    if (bGetStatets == null)
                    {
                        DLLWrapper.FreeLibrary(hModule);
                        throw new Exception(Info.LoadLibrayFailure);
                    }
                    else
                    {
                        try
                        {
                            ret = bGetStatets(ref iVoltage, ref iCurrent, ref iTemp, ref iVacuum, ref iCover);
                        }
                        catch {
                            Environment.Exit(0);
                        }
                    }
                }

                else if (Dll == DllType.DLL3)
                {
                    UsbPortDelegate.bGetState3 bGetStatets = (UsbPortDelegate.bGetState3)DLLWrapper.GetFunctionAddress(hModule, "bGetState", typeof(UsbPortDelegate.bGetState3));
                    if (bGetStatets == null)
                    {
                        DLLWrapper.FreeLibrary(hModule);
                        throw new Exception(Info.LoadLibrayFailure);
                    }
                    else
                    {
                        iCover = 0;
                        try
                        {
                            ret = bGetStatets(ref iCurrent, ref iVoltage, ref iTemp, ref iVacuum);
                        }
                        catch
                        {
                            Environment.Exit(0);
                        }
                    }
                }
            }
            catch (System.Exception)
            {

            }
            return ret;
        }

        private int bOpenDevice()
        {
            UsbPortDelegate.bOpenDevice bOpenDevices = (UsbPortDelegate.bOpenDevice)DLLWrapper.GetFunctionAddress(hModule, "bOpenDevice", typeof(UsbPortDelegate.bOpenDevice));
            if (bOpenDevices != null)
            {
                try
                {
                    return bOpenDevices();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            return 0;
        }

        private int bCloseDevice()
        {
            UsbPortDelegate.bCloseDevice bCloseDevices = (UsbPortDelegate.bCloseDevice)DLLWrapper.GetFunctionAddress(hModule, "bCloseDevice", typeof(UsbPortDelegate.bCloseDevice));
            if (bCloseDevices == null)
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            else
            {
                try
                {
                    return bCloseDevices();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            return 0;
        }

        //public abstract int iGetDevVersion();

        private int bSetDA(int iVoltage, int iCurrent, int iGain, int iFineGain)
        {
            UsbPortDelegate.bSetDA bSetDAs = (UsbPortDelegate.bSetDA)DLLWrapper.GetFunctionAddress(hModule, "bSetDA", typeof(UsbPortDelegate.bSetDA));
            if (bSetDAs == null)
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            else
            {
                try
                {
                    return bSetDAs(iVoltage, iCurrent, iGain, iFineGain);
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            return 0;
        }

        private int bEnableCoverSwitch(bool allowUncover)
        {
            if (Dll == DllType.DLL4)
            {
                UsbPortDelegate.bEnableCoverSwitch bEnableCoverSwitchs = (UsbPortDelegate.bEnableCoverSwitch)DLLWrapper.GetFunctionAddress(hModule, "bEnableCoverSwitch", typeof(UsbPortDelegate.bEnableCoverSwitch));
                if (bEnableCoverSwitchs == null)
                {
                    DLLWrapper.FreeLibrary(hModule);
                    throw new Exception(Info.LoadLibrayFailure);
                }
                else
                {
                    try
                    {
                        return bEnableCoverSwitchs(allowUncover);
                    }
                    catch
                    {
                        Environment.Exit(0);
                    }
                }
            }
            return 0;
        }

        private int bOpenHV()
        {
            UsbPortDelegate.bOpenHV bOpenHVs = (UsbPortDelegate.bOpenHV)DLLWrapper.GetFunctionAddress(hModule, "bOpenHV", typeof(UsbPortDelegate.bOpenHV));
            if (bOpenHVs == null)
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            else
            {
                try
                {
                    return bOpenHVs();
                }
                catch {
                    Environment.Exit(0);
                }
            }
            return 0;
        }
        private int bCloseHV()
        {
            UsbPortDelegate.bCloseHV bCloseHVs = (UsbPortDelegate.bCloseHV)DLLWrapper.GetFunctionAddress(hModule, "bCloseHV", typeof(UsbPortDelegate.bCloseHV));
            if (bCloseHVs == null)
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            else
            {
                try
                {
                    return bCloseHVs();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            return 0;
        }
        private int bOpenPump()
        {
            UsbPortDelegate.bOenPump bOpenPumps = (UsbPortDelegate.bOenPump)DLLWrapper.GetFunctionAddress(hModule, "bOpenPump", typeof(UsbPortDelegate.bOenPump));
            if (bOpenPumps == null)
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            else
            {
                try
                {
                return bOpenPumps();
              }
                catch
                {
                    Environment.Exit(0);
                }
            }
            return 0;
        }

        public int bXRayTubeSel(int index)
        {
            UsbPortDelegate.bXRayTubeSel bXRayTubeSels = (UsbPortDelegate.bXRayTubeSel)DLLWrapper.GetFunctionAddress(hModule, "bXRayTubeSel", typeof(UsbPortDelegate.bXRayTubeSel));
            if (bXRayTubeSels == null)
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            else
            {
                try
                {
                    return bXRayTubeSels(index);
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            return 0;
        }

        private int bClosePump()
        {
            UsbPortDelegate.bClosePump bClosePumps = (UsbPortDelegate.bClosePump)DLLWrapper.GetFunctionAddress(hModule, "bClosePump", typeof(UsbPortDelegate.bClosePump));
            if (bClosePumps == null)
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            else
            {
                try
                {
                    return bClosePumps();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            return 0;
        }

        private int bOpenHVLamp()
        {
            UsbPortDelegate.bOpenHVLamp bOpenHVLamps = (UsbPortDelegate.bOpenHVLamp)DLLWrapper.GetFunctionAddress(hModule, "bOpenHVLamp", typeof(UsbPortDelegate.bOpenHVLamp));
            if (bOpenHVLamps == null)
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            else
            {
                try
                {
                    return bOpenHVLamps();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            return 0;
        }
        private int bCloseHVLamp()
        {
            UsbPortDelegate.bCloseHVLamp bCloseHVLamps = (UsbPortDelegate.bCloseHVLamp)DLLWrapper.GetFunctionAddress(hModule, "bCloseHVLamp", typeof(UsbPortDelegate.bCloseHVLamp));
            if (bCloseHVLamps == null)
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            else
            {
                try
                {
                    return bCloseHVLamps();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            return 0;
        }
        private int bReStart()
        {
            UsbPortDelegate.bReStart bReStarts = (UsbPortDelegate.bReStart)DLLWrapper.GetFunctionAddress(hModule, "bReStart", typeof(UsbPortDelegate.bReStart));
            if (bReStarts == null)
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            else
            {
                try
                {
                    return bReStarts();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            return 0;
        }
        private unsafe Byte* GetMCAData()
        {
            UsbPortDelegate.GetMCAData GetMCADatas = (UsbPortDelegate.GetMCAData)DLLWrapper.GetFunctionAddress(hModule, "GetMCAData", typeof(UsbPortDelegate.GetMCAData));
            if (GetMCADatas == null)
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            else
            {
                try
                {
                    return GetMCADatas();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            return null;
        }

        private int bMotoControl(int iIndex, int iDir, int iCycle, int iSwtStop, int spwspeed)
        {
            if (Dll == DllType.DLL4)
            {
                UsbPortDelegate.bMotoControl bMotoControl4 = (UsbPortDelegate.bMotoControl)DLLWrapper.GetFunctionAddress(hModule, "bMotoControl", typeof(UsbPortDelegate.bMotoControl));
                if (bMotoControl4 == null)
                {
                    DLLWrapper.FreeLibrary(hModule);
                    throw new Exception(Info.LoadLibrayFailure);
                }
                else
                {
                    try
                    {
                        return bMotoControl4(iIndex, iDir, iCycle, iSwtStop, spwspeed);
                    }
                    catch
                    {
                        Environment.Exit(0);
                    }
                }
            }
            else if (Dll == DllType.DLL3)
            {
                UsbPortDelegate.bMotoControl3 bMotoControl3 = (UsbPortDelegate.bMotoControl3)DLLWrapper.GetFunctionAddress(hModule, "bMotoControl", typeof(UsbPortDelegate.bMotoControl3));
                if (bMotoControl3 == null)
                {
                    DLLWrapper.FreeLibrary(hModule);
                    throw new Exception(Info.LoadLibrayFailure);
                }
                else
                {
                    try
                    {
                        return bMotoControl3(iIndex, iDir, iCycle, iSwtStop);
                    }
                    catch
                    {
                        Environment.Exit(0);
                    }
                }
            }
            return 0;
        }
        private int bGetMotoInfo(ref int iMotorInfo)
        {
            UsbPortDelegate.bGetMotoInfo bGetMotoInfos = (UsbPortDelegate.bGetMotoInfo)DLLWrapper.GetFunctionAddress(hModule, "bGetMotoInfo", typeof(UsbPortDelegate.bGetMotoInfo));
            if (bGetMotoInfos == null)
            {
                DLLWrapper.FreeLibrary(hModule);
                throw new Exception(Info.LoadLibrayFailure);
            }
            else
            {
                try
                {
                    return bGetMotoInfos(ref iMotorInfo);
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            return 0;
        }

        #region 动态加载获得加密狗信息,By ZQH
        //public override int GetKeyInfo(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods)
        //{
        //    UsbPortDelegate.GetKeyInfo bGetKeyInfo = (UsbPortDelegate.GetKeyInfo)DLLWrapper.GetFunctionAddress(hModule, "GetKeyInfo", typeof(UsbPortDelegate.GetKeyInfo));
        //    if (bGetKeyInfo == null)
        //    {
        //        DLLWrapper.FreeLibrary(hModule);
        //        throw new Exception(Info.LoadLibrayFailure);
        //    }
        //    else
        //        return bGetKeyInfo(company, mode, serialNum, ref LeftSencods);
        //}
        #endregion

        #region 直接导入函数获取加密狗信息,避免部分动态库内部弹出提示,By WZW

        [DllImport("TRUSBDev.Dll4.Usb2.0.dll", EntryPoint = "GetKeyInfo", CharSet = CharSet.Ansi)]
        private static extern int GetKeyInfo4(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods);

        public override int GetKeyInfo(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods)
        {
            return GetKeyInfo4(company, mode, serialNum, ref LeftSencods);
        }

        #endregion
        /// <summary>
        /// 连接设备
        /// </summary>
        /// <returns></returns>
        public override bool Connect()
        {
            bReStart();
            return (bOpenDevice() == 1);
        }


        public override bool OpenDevice()
        {
            return (bOpenDevice() == 1);
        }

        /// <summary>
        /// 断开与设备的连接
        /// </summary>
        /// <returns></returns>
        public override bool Disconnect()
        {
            return (bCloseDevice() == 1);
        }

        /// <summary>
        /// 打开高压
        /// </summary>
        /// <returns></returns>
        public override bool OpenVoltage()
        {
            return (bOpenHV() == 1);
        }

        /// <summary>
        /// 关闭高压
        /// </summary>
        /// <returns></returns>
        public override bool CloseVoltage()
        {
            return (bCloseHV() == 1);
        }

        /// <summary>
        /// 打开真空泵开始抽真空
        /// </summary>
        /// <returns></returns>
        public override bool OpenPump()
        {
            return (bOpenPump() == 1);
        }

        /// <summary>
        /// 关闭真空泵
        /// </summary>
        /// <returns></returns>
        public override bool ClosePump()
        {
            return (bClosePump() == 1);
        }

        /// <summary>
        /// 开高压指示灯
        /// </summary>
        /// <returns></returns>
        public override bool OpenVoltageLamp()
        {
            return (bOpenHVLamp() == 1);
        }

        /// <summary>
        /// 关闭高压指示灯
        /// </summary>
        /// <returns></returns>
        public override bool CloseVoltageLamp()
        {
            return (bCloseHVLamp() == 1);
        }

        /// <summary>
        /// 获取一秒数据
        /// </summary>
        /// <returns></returns>
        public override unsafe bool GetData(int[] data)
        {
            OpenVoltage();
            byte* bData = null;
            bData = GetMCAData();
            if (bData != null)
            {
                bReStart();
                for (int i = 0; i < data.Length; i++)
                    data[i] = data[i] + (int)bData[i * 2] * 256 + (int)bData[2 * i + 1];
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 控制电机
        /// </summary>
        /// <param name="index">电机号</param>
        /// <param name="dir">电机方向（0，1）</param>
        /// <param name="step">移动步长</param>
        /// <param name="swtStop">遇到接近开关时是否停止。true＝停止</param>
        /// <param name="speedGear">移动速率</param>
        /// <returns></returns>
        public override bool MotorControl(int index, int dir, int step, bool swtStop, int speedGear)
        {
            do
            {
            } while (!UsbIdel);
            UsbIdel = false;
            if (index == 4)
            {
                //int i = 0;
            }
            int stopFlag = 0;
            if (swtStop)
                stopFlag = 1;
            bool result = (bMotoControl(index, dir, step, stopFlag, speedGear) == 1);
            System.Threading.Thread.Sleep(50);
            UsbIdel = true;
            return result;
        }


        public override bool MotorIsIdelAll(int X, int Y)
        {
            return true;
        }
        /// <summary>
        ///电机是否空闲
        /// </summary>
        /// <param name="index" > </param>
        /// <returns></returns>
        /// 
        public override bool MotorIsIdel(int index)
        {
            do
            {
            } while (!UsbIdel);
            UsbIdel = false;
            int info = 0;
            int i = 0;
            bGetMotoInfo(ref info);
            switch (index)
            {
                case 2:
                    i = 8;
                    break;
                case 3:
                    i = 11;
                    break;
                case 4:
                    i = 4;
                    break;
                default:
                    i = index * 2;
                    break;
            }
            // 是否空闲只比较一位
            bool result = (info & (0x01 << i)) == 0;
            System.Threading.Thread.Sleep(50);
            UsbIdel = true;
            return result;
        }

        /// <summary>
        /// 获取电机状态位
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool GetMotorInfo(ref Int32 info)
        {
            return bGetMotoInfo(ref info) == 1;
        }

        public override bool GetParams(ref int Voltage, ref int Current, ref int Temperature, ref int Vacuum, ref bool IsOpen)
        {
            int isOpen = 0;
            int result = bGetState(ref Voltage, ref Current, ref Temperature, ref Vacuum, ref isOpen);
            IsOpen = (isOpen == 1) ? true : false;
            return result == 1;
        }

        /// <summary>
        /// 是否在接近开关点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool MotorAtTouch(int index)
        {
            //return MotorIsIdel(index);
            int info = 0;
            int i = 0;
            bGetMotoInfo(ref info);
            switch (index)
            {
                case 2:
                    i = 9;
                    break;
                case 3:
                    i = 12;
                    break;
                case 4:
                    i = 5;
                    break;
                default:
                    i = index * 2 + 1;
                    return (info & (0x01 << i)) != 0;   // 是否接近，其他电机暂时只比较两位
            }
            // 是否接近，xyz轴电机需比较两位
            return (info & (0x01 << i)) != 0 || (info & (0x01 << i + 1)) != 0;
        }

        /// <summary>
        /// 是否在聚焦点上
        /// </summary>
        /// <returns></returns>
        public bool MotorAtFocus()
        {
            int info = 0;
            bGetMotoInfo(ref info);
            //return (info & (0x01 << 14)) != (info & (0x01 << 15));//((info & (0x01 << 14)) == 0 && (info & (0x01 << 15)) == 1) || ((info & (0x01 << 14)) == 1 && (info & (0x01 << 15)) == 0);
            info = info >> 14;
            return (info == 1 || info == 2);
        }

        public bool MotorHyperFocus()
        {
            int info = 0;
            bGetMotoInfo(ref info);
            // return (info & (0x01 << 14)) == 1 && (info & (0x01 << 15)) == 1;
            info = info >> 14;
            return (info == 3);
        }
        public bool MotorUnderFocus()
        {
            int info = 0;
            bGetMotoInfo(ref info);
            //return (info & (0x01 << 14)) == 0 && (info & (0x01 << 15)) == 0;
            info = info >> 14;
            return (info == 0);
        }

        /// <summary>
        /// 设置管压，管流，主放，粗放
        /// </summary>
        /// <returns></returns>
        public override bool SetParam(int tubVoltage, int tubCurrent, int gain, int fineGain)
        {
            int hv = tubVoltage * 5;
            return (bSetDA(hv, tubCurrent, gain, fineGain) == 1);
        }

        public override void setFPGAParam(byte bBaseResume, byte bBaseLimit, byte bHeapUP, byte bRate, byte bCoarse, uint uFine, uint uTime, byte bUPTime, byte bWidthTime, byte bSlowLimit, double dIntercept)
        {
            throw new NotImplementedException();
        }

        public override void setParam(double tubVoltage, double tubCurrent)
        {
            throw new NotImplementedException();
        }

        public override void getParam(out double uVoltage, out double uCurrent, out int iUncover)
        {
            throw new NotImplementedException();
        }

        public override void AllowUncover(bool allowUncover)
        {
            bEnableCoverSwitch(allowUncover);
        }

        public override void OpenElectromagnet()
        {
            throw new NotImplementedException();
        }

        public override void CloseElectromagnet()
        {
            throw new NotImplementedException();
        }

        public override void OpenMagnet()
        {
            throw new NotImplementedException();
        }

        public override void CloseMagnet()
        {
            throw new NotImplementedException();
        }

        public override void ConsolePrint(string str)
        {
            throw new NotImplementedException();
        }

        public override bool IPSettings(string IP, string SubNet, string GateWay, string DNS)
        {
            throw new NotImplementedException();
        }

        public override void OpenXRayTubHV()
        {
            bXRayTubeSel(1);
        }

        public override void CloseXRayTubHV()
        {
            bXRayTubeSel(0);
        }

        public override bool SetSurfaceSource(ushort firstLight, ushort secondLight, ushort thirdLight, ushort fourthLight)
        {
            throw new NotImplementedException();
        }

        public override bool GetVacuum(uint uType, out uint uVacuum)
        {
            throw new NotImplementedException();
        }

        //public override event EventHandler GetConnect;

        #region IDisposable 成员

        public override void Dispose()
        {
            
        }

        #endregion

        public override int GetDeviceKeyInfo()
        {
            int iVersion = this.iGetDevVersion();
            return (iVersion&0xFFFF)==257?1:0;
        }

        public override bool GetDoubleVacuum(uint uType, out uint pUpVacuum, out uint pDownVacuum)
        {
            pUpVacuum = 0;
            pDownVacuum = 0;
            return true;
        }

        public override LightStatus GetLightShutState(int index)
        {
            return LightStatus.Fail;
        }


        private bool bChamberStatus(ref int Steps, ref int ChamberIndex, ref bool IsTrueStep, ref bool IsInitialed, ref bool IsBusyed)
        {
            if (Dll == DllType.DLL4&&DllVersion==UsbVersion.Usb2)
            {
                UsbPortDelegate.bGetChamberInfos bGetChamberInfos = (UsbPortDelegate.bGetChamberInfos)DLLWrapper.GetFunctionAddress(hModule, "bGetChamberStatus", typeof(UsbPortDelegate.bGetChamberInfos));
                if (bGetChamberInfos != null)
                {
                    try
                    {
                        byte[] status = new byte[5];
                        for (int i = 0; i < status.Length; i++)
                        {
                            status[i] = 0;
                        }
                        int statusLen = 5;
                        bool result = false;
                        result = bGetChamberInfos(status, ref statusLen);
                   

                        Steps = (int)status[3] * 256 * 256 * 256 + (int)status[2] * 256 * 256 + (int)status[1] * 256 + (int)status[0];
                        IsBusyed = (status[4] & 0x80) == 0x80;
                        IsInitialed = (status[4] & 0x40) == 0x40;
                        IsTrueStep = (status[4] & 0x20) == 0x20;
                        ChamberIndex = result==true?(status[4] & 0x07):-1;
                        return result;
                    }
                    catch
                    {
                        DLLWrapper.FreeLibrary(hModule);
                        Environment.Exit(0);
                    }
                }
                else
                {
                    DLLWrapper.FreeLibrary(hModule);
                    throw new Exception(Info.LoadLibrayFailure);
                }
            }
            return true;
        }
        //6000B特殊修改------------begin
        private  bool bResetChamber()
        {
            if (Dll == DllType.DLL4 && DllVersion == UsbVersion.Usb2)
            {
                UsbPortDelegate.bResetChamber bResetChamber = (UsbPortDelegate.bResetChamber)DLLWrapper.GetFunctionAddress(hModule, "bResetChamber", typeof(UsbPortDelegate.bResetChamber));
                if (bResetChamber != null)
                    try
                    {
                        return bResetChamber();
                    }
                    catch
                    {
                        Environment.Exit(0);
                    }
                else
                {
                    DLLWrapper.FreeLibrary(hModule);
                    throw new Exception(Info.LoadLibrayFailure);
                }
                return false;
            }
            return true;
        }

        public override void GetChamberStatus(ref int steps, ref int chambeIndex, ref bool istruestep, ref bool initialed, ref bool isbusy)
        {
            steps = 0;
            chambeIndex = -1;
            istruestep = false;
            initialed = false;
            isbusy = false;
            bChamberStatus(ref steps, ref chambeIndex, ref istruestep, ref initialed, ref isbusy);
        }


        public override int ResetChamber(int ID, int DirectionFlag, int DefSpeed)
        {
            MotorControl(ID, DirectionFlag, 1, true, DefSpeed);//先走一步，给复位设置速度
            bResetChamber();
            return 1;
        }
        //6000B特殊修改------------end

        //Dp5加密仪器修改---------------------begin
        private bool bLockHV(int iLock)
        {
            if (Dll == DllType.DLL4 && DllVersion == UsbVersion.Usb2)
            {
                UsbPortDelegate.bLockHV bLock = (UsbPortDelegate.bLockHV)DLLWrapper.GetFunctionAddress(hModule, "bLockHV", typeof(UsbPortDelegate.bLockHV));
                if (bLock != null)
                    try
                    {
                        return bLock(iLock);
                    }
                    catch
                    {
                        Environment.Exit(0);
                    }
                else
                {
                    DLLWrapper.FreeLibrary(hModule);
                    throw new Exception(Info.LoadLibrayFailure);
                }
                return false;
            }
            return true;
        }
        public override bool LockHV(bool bLock)
        {
            return bLockHV(Convert.ToInt32(bLock));
        }
        //Dp5加密仪器修改----------------------end
        public override void OpenHeightLaser(bool bOpen)
        {
            if (!bOpen) OpenPump();
            else ClosePump();
        }
        public override void MoveZAutoMotor(int iSpeed)
        {
        }
        public override void GetSwitch()
        {
        }
    }
}
