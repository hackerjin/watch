using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.EDX.Common.Component
{
    public class ParallelPort : Port
    {
        private short STATUS_PORT = 0x378 + 1; ///状态寄存器地址
        private short CONTROL_PORT = 0x378 + 2;///控制寄存器地址
        private short DATA_PORT = 0x378;       ///数据寄存器地址

        /// <summary>
        /// 连接设备
        /// </summary>
        /// <returns>联通返回true</returns>
        public override bool Connect()
        {
            WriteControlPort(0x07);
            ResetTime();
            //short value;
            //bool result;
            //int retry = 20;
            return IsReady();

            //WriteControlPort(0x08);
            //do
            //{
            //    value = ReadStatusPort();
            //    result = ((value & 0x20) == 0);
            //    retry--;
            //} while (!result && retry > 0);
            //return result;           
        }         
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Disconnect()
        {
            return true;
        }

    

        /// <summary>
        /// 打开高压
        /// </summary>
        /// <returns></returns>
        public override bool OpenVoltage()
        {
            WriteControlPort(0x07);

            WriteDataPort(0xBA);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x68);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x60);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0xFF);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);
            return true;
        }
        /// <summary>
        /// 关闭高压
        /// </summary>
        /// <returns></returns>
        public override bool CloseVoltage()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool OpenVoltageLamp()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CloseVoltageLamp()
        {
            return true;
        }
        /// <summary>
        /// 打开真空泵开始抽真空
        /// </summary>
        /// <returns></returns>
        public override bool OpenPump()
        {
            return true;
            //return (bOpenPump() == 1);
        }
        /// <summary>
        /// 关闭真空泵
        /// </summary>
        /// <returns></returns>
        public override bool ClosePump()
        {
            return true;
            //return (bClosePump() == 1);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>        
        public override bool GetData(int[] data)
        {
            bool result = false;
            int channel = 0;
            OpenVoltage();
            // WriteControlPort(0x07);
            ResetTime();
            DateTime dtBegin = DateTime.Now;
            do
            {
                if (IsReady())
                {
                    //读高4位
                    WriteControlPort(0x00);
                    ushort state = Convert.ToUInt16(ReadStatusPort());
                    channel = (ushort)((state & 0x78) << 5);
                    //读中4位
                    WriteControlPort(0x02);
                    state = Convert.ToUInt16(ReadStatusPort());
                    channel += ((state & 0x78) << 1);
                    //读低4位
                    WriteControlPort(0x06);
                    state = Convert.ToUInt16(ReadStatusPort());
                    channel = channel + ((state & 0x78) >> 3);
                    //选通RESET_AD
                    WriteControlPort(0x0A);
                    channel = Convert.ToUInt16(channel / 2);
                    if (channel >= 0 && channel < data.Length)
                    {
                        data[channel] += 1;
                    }
                    result = true;
                }
            } while (!TimeIsOut());
            DateTime dtOver = DateTime.Now;
            TimeSpan ts = dtOver - dtBegin;
            Console.WriteLine(ts.TotalSeconds.ToString());
            return result;
        }

        public override bool MotorControl(int index, int dir, int step, bool swtStop, int speedGear)
        {
            int stopFlag = 0;
            if (swtStop)
                stopFlag = 1;
            return (bMotoControl(index, dir, step, stopFlag) == 1);
        }
        /// <summary>
        ///电机是否空闲
        /// </summary>
        /// <param name="index" >电机编号</param>
        /// <returns></returns>
        /// 
        public override bool MotorIsIdel(int index)
        {
            int info = 0;
            bGetMotoInfo(ref info);
            int i = index * 2;
            return ((info & (0x01 << i)) == 0);
        }

        public override bool MotorIsIdelAll(int X,int Y)
        {
            return true;
        }

        public override bool GetMotorInfo(ref int info)
        {
            return true;
        }

        public override bool GetParams(ref int Voltage, ref int Current, ref int Temperature, ref int Vacuum, ref bool IsOpen)
        {
            return true;
        }

        public override void setFPGAParam(byte bBaseResume, byte bBaseLimit, byte bHeapUP, byte bRate, byte bCoarse, uint uFine, uint uTime, byte bUPTime, byte bWidthTime, byte bSlowLimit, double dIntercept)
        {
            //return true;
        }

        public override void setParam(double tubVoltage, double tubCurrent)
        {
            //return true;
        }

        public override void AllowUncover(bool allowUncover)
        {
            //return true;
        }

        public override int GetKeyInfo(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods)
        {
            return 1;
        }

        public override void OpenElectromagnet()
        {
            //return true;
        }

        public override void CloseElectromagnet()
        {
            //return true;
        }

        public override void OpenMagnet()
        {
            //return true;
        }

        public override void CloseMagnet()
        {
            //return true;
        }

        public override void ConsolePrint(string str)
        {
            //return true;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="adress">地址</param>      
        /// <param name="value">数据</param>      
        [DllImport("inpout32.dll", EntryPoint = "Out32")]
        private static extern void Output(int adress, int value);
        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="adress">地址</param>
        /// <returns>数据</returns>
        [DllImport("inpout32.dll", EntryPoint = "Inp32")]
        private static extern int Input(int adress);
        /// <summary>
        /// 开启真空泵
        /// </summary>
        /// <returns></returns>
        [DllImport("TRUSBDev2.dll")]
        private static extern int bOpenPump();
        /// <summary>
        /// 关闭真空泵
        /// </summary>
        /// <returns></returns>
        [DllImport("TRUSBDev2.dll")]
        private static extern int bClosePump();
        /// <summary>
        /// 步进电机控制
        /// </summary>
        /// <param name="iIndex">电机号</param>
        /// <param name="iDir">方向</param>
        /// <param name="iCycle">步长</param>
        /// <param name="iSwtStop">响应接近开关标志</param>
        /// <returns></returns>
        [DllImport("TRUSBDev2.dll")]
        private static extern int bMotoControl(int iIndex, int iDir, int iCycle, int iSwtStop);
        /// <summary>
        /// 获取电机及接近开关状态
        /// </summary>
        /// <param name="iMotorInfo"></param>
        /// <returns></returns>
        [DllImport("TRUSBDev2.dll")]
        private static extern int bGetMotoInfo(ref int iMotorInfo);
        /// <summary>
        /// 写数据寄存器
        /// </summary>
        /// <param name="value"></param>
        private void WriteDataPort(Byte value)
        {
            //OutPortB(DATA_PORT, value);
            Output(DATA_PORT, value);
        }
        /// <summary>
        /// 写控制寄存器
        /// </summary>        
        private void WriteControlPort(Byte value)
        {
            value &= 0x0F;
            value ^= 0x0B;
            //  CONTROL_PORT = 0x37A;
            //OutPortB(CONTROL_PORT, value);
            Output(CONTROL_PORT, value);
        }
        /// <summary>
        /// 读状态寄存器
        /// </summary>
        /// <returns>状态值</returns>
        private byte ReadStatusPort()
        {
            //return (short)(InPortB(STATUS_PORT) & 0x78);
            byte value = Convert.ToByte(Input(STATUS_PORT));
            return (byte)(value & 0x78);
        }
        /// <summary>
        /// 设置粗调码
        /// </summary>
        /// <param name="value">粗调码</param>
        private void SetGain(byte value)
        {
            WriteControlPort(0x07);
            value = Convert.ToByte(255 - value);
            //锁存DA数据
            WriteDataPort(value);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);//选通C2
            WriteControlPort(0x0E);//取消选通C2

            WriteDataPort(0x61);//选通DATA_CS 
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            //选择DA放大倍数粗调通道
            WriteDataPort(2);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x62);//选通DA_CHANNEL
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            //启动DA
            WriteDataPort(0xAA);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x6D);//选通DA_WRITE
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x60);//取消选通DA_WRITE
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0xFF);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);
        }
        /// <summary>
        /// 设置细调码
        /// </summary>      
        private void SetFineGain(byte value)
        {
            WriteControlPort(0x07);
            value = Convert.ToByte(255 - value);
            //锁存DA数据
            WriteDataPort(value);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);//选通C2
            WriteControlPort(0x0E);//取消选通C2

            WriteDataPort(0x61);//选通DATA_CS 
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            //选择DA放大倍数细调通道
            WriteDataPort(3);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x62);//选通DA_CHANNEL
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            //启动DA
            WriteDataPort(0xAA);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x6D);//选通DA_WRITE
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x60);//取消选通DA_WRITE
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0xFF);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);
        }
        /// <summary>
        /// 设置管压
        /// </summary>
        /// <param name="value">管压值</param>
        private void SetHVoltage(byte value)
        {
            WriteControlPort(0x07);
            value = Convert.ToByte(1.0 * value / 50 * 255);
            //锁存DA数据
            WriteDataPort(value);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);//选通C2
            WriteControlPort(0x0E);//取消选通C2

            WriteDataPort(0x61);//选通DATA_CS 
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            //选择DA电压通道
            WriteDataPort(1);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x62);//选通DA_CHANNEL
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            //启动DA
            WriteDataPort(0xAA);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x6D);//选通DA_WRITE
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x60);//取消选通DA_WRITE
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0xFF);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);
        }
        /// <summary>
        /// 设置管流
        /// </summary>
        /// <param name="value">管流值</param>
        public void SetCurrent(byte value)
        {
            WriteControlPort(0x07);
            value = Convert.ToByte(1.0 * value / 1000 * 255);
            //锁存DA数据
            WriteDataPort(value);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);//选通C2
            WriteControlPort(0x0E);//取消选通C2

            WriteDataPort(0x61);//选通DATA_CS 
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            //选择DA电流通道
            WriteDataPort(0);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x62);//选通DA_CHANNEL
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            //启动DA
            WriteDataPort(0xAA);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x6D);//选通DA_WRITE
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0x60);//取消选通DA_WRITE
            WriteControlPort(0x04);
            WriteControlPort(0x0E);

            WriteDataPort(0xFF);
            WriteControlPort(0x0C);
            WriteControlPort(0x0A);
            WriteControlPort(0x0E);

            WriteDataPort(0x6E);
            WriteControlPort(0x04);
            WriteControlPort(0x0E);
        }
        /// <summary>
        /// AD是否已经准备好
        /// </summary>
        /// <returns></returns>
        private bool IsReady()
        {
            short value;
            bool result;
            int retry = 20;
            WriteControlPort(0x08);
            do
            {
                value = ReadStatusPort();
                result = ((value & 0x20) == 0);
                retry--;
            } while (!result && retry > 0);
            return result;
        }
        /// <summary>
        /// 时间清零
        /// </summary>
        private void ResetTime()
        {
            // WriteControlPort(0x07);

            WriteDataPort(0x6A);//选通ADDRESS_TIME_RESET
            WriteControlPort(0x04);//选通C2
            WriteDataPort(0x00);//取消选通ADDRESS_TIME_RESET
            WriteControlPort(0x0E);//取消选通C2
        }
        /// <summary>
        /// 1秒钟时间到
        /// </summary>
        /// <returns></returns>
        private bool TimeIsOut()
        {
            WriteControlPort(0x08);
            byte value = ReadStatusPort();
            bool result = (value & 0x40) == 0;
            return result;
        }
        
        /// <summary>
        /// 是否在接近开关点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool MotorAtTouch(int index)
        {
            int info = 0;
            bGetMotoInfo(ref info);
            int i = index * 2 + 1;
            return ((info & (0x01 << i)) != 0);
        }
        /// <summary>
        /// 设置管压，管流，主放，粗放
        /// </summary>
        /// <returns></returns>
        public override bool SetParam(int tubVoltage, int tubCurrent, int gain, int fineGain)
        {
            SetHVoltage(Convert.ToByte(tubVoltage));
            SetCurrent(Convert.ToByte(tubCurrent));
            SetGain(Convert.ToByte(gain));
            SetFineGain(Convert.ToByte(fineGain));

            return true;
        }

        public override bool IPSettings(string IP, string SubNet, string GateWay, string DNS)
        {
            return true;
        }

        public override void getParam(out double uVoltage, out double uCurrent, out int iUncover)
        {
            throw new NotImplementedException();
        }

        public override void OpenXRayTubHV()
        {
            //WriteDataPort(0xBA);
            //WriteControlPort(0x0C);
            //WriteControlPort(0x0A);
            //WriteControlPort(0x0E);

            //WriteDataPort(0x6E);
            //WriteControlPort(0x04);
            //WriteControlPort(0x0E);

            //WriteDataPort(0x68); 
            //WriteControlPort(0x04); 
            //WriteControlPort(0x0E); 

            //WriteDataPort(0x60);  
            //WriteControlPort(0x04); 
            //WriteControlPort(0x0E); 

            //WriteDataPort(0xFF);
            //WriteControlPort(0x0C);
            //WriteControlPort(0x0A);
            //WriteControlPort(0x0E);

            //WriteDataPort(0x6E);
            //WriteControlPort(0x04);
            //WriteControlPort(0x0E); 
        }

        public override void CloseXRayTubHV()
        {
        }

        public override bool SetSurfaceSource(ushort firstLight, ushort secondLight, ushort thirdLight, ushort fourthLight)
        {
            return true;
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
            return 0;
        }

        public override bool OpenDevice()
        {
            return true;
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

        public override void GetChamberStatus(ref int steps, ref int chambeIndex, ref bool istruestep, ref bool initialed, ref bool isbusy)
        {

        }


        public override int ResetChamber(int ID, int DirectionFlag, int DefSpeed)
        {
            return -1;
        }
        public override bool LockHV(bool bLock)
        {
            return false;
        }
        public override void OpenHeightLaser(bool bOpen)
        {
        }
        public override void MoveZAutoMotor(int iSpeed)
        {
        }
        public override void GetSwitch()
        {
        }
    }
}
