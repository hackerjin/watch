using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.EDX.Common.Component
{
    public class NewNetPortStatus
    {
        public byte[] RAW = new byte[32];
        public double AccumulationTime;
        public double RealTime;
        public double FastCount;	
        public double SlowCount;
        public double FastCountRate;	
        public double SlowCountRate;
        public double LiveTimeClockPercent;
        public int BLRDAC;
        public int DBLRVAL;
        public int PreRstRate;

        public bool Process_Status()
        {
            bool bDppk100Error = false;
            bDppk100Error = bDppk100Error ||((RAW[1] == 0xFF) && (RAW[0] == 0xFF) && (RAW[3] == 0xFF));
            RealTime = ((double)RAW[1] + ((double)RAW[0] * 256.0) + ((double)(RAW[3]&0x0F) * 65536.0));
            RealTime *= 0.1;
            bDppk100Error = bDppk100Error || ((RAW[2] == 0xFF) && (RAW[5] == 0xFF) && (RAW[4] == 0xFF));
            AccumulationTime = ((double)RAW[2] + ((double)RAW[5] * 256.0) + ((double)(RAW[4] & 0x0F) * 65536.0));
            AccumulationTime *= 0.1;

            bDppk100Error = bDppk100Error || ((RAW[6] == 0xFF) && (RAW[7] == 0xFF) && (RAW[8] == 0xFF) && (RAW[9] == 0xFF));
            SlowCount = (double)RAW[7] + (double)RAW[6] * 256 + (double)RAW[9] * 65536 + (double)RAW[8] * 16777216;
            FastCount = (double)RAW[21] + (double)RAW[20] * 256 + (double)RAW[23] * 65536 + (double)RAW[22] * 16777216;

            FastCountRate = (double)RAW[11] + (double)RAW[10] * 256 + (double)(RAW[13] & 0x03) * 65536;
            FastCountRate /=0.1;
            SlowCountRate  = (RAW[17] + RAW[16] * 256) * 2;
            SlowCountRate /= 0.1;
            PreRstRate = RAW[19] + (RAW[18] & 0x03) * 256;
            LiveTimeClockPercent = (RAW[27]  + RAW[26]  * 256) * 2;
            LiveTimeClockPercent /= 100000;
            BLRDAC = RAW[25] + RAW[24] * 256;
            DBLRVAL = RAW[29] + (RAW[28]&0x3F) * 256;
            if (bDppk100Error)
            {
                Log.Error("Dpp100 Error:1; Dpp100 spi Error!(Please check the spi line.)");
            }
            return true;
        }
    } ;
    public class NewNetPortProtocol
    {
        [DllImport("kernel32.dll")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(string section, string key, string def,StringBuilder refVal,int size, string filepath);
        //public const int MAX_BUFFER_DATA = 8192;
        //byte[] PacketIn; // largest possible IN packet
        public long[] SPECTRUM;
        private int iSpecLength;
        public int SpecLength
        {
            get { return iSpecLength; }
        }

        private int Dpp100Version = 0xA1;
        NewNetPortStatus Dp5Status = new NewNetPortStatus();
        private bool BLR_ON = false;

        //Packet_In PIN;
        public string strDp5ConfigFile;
        public string strDp5ConfigValue;
        public NewNetPortProtocol(int specLength)
        {
            SPECTRUM = new long[specLength];
            iSpecLength = specLength;
        }
        public byte[] recDatas = new byte[16390];
        byte[] FPGA_CTRL0 = new byte[2];

        public byte[] RecB = new byte[2];
        public int PresetTestTime = 100;//预设测试时间

        public double FineGain = 1.0f;

        public int DppVersion
        {
            get { return Dpp100Version; }
        }

        public string Dpp100HID;

        private byte[] GetCommandByProtocol(int iType, string strCommand)
        {
            char[] charsCommand = strCommand.ToCharArray();
            byte[] byteTemp = new byte[charsCommand.Length + 9+2];//前9字节，2字节fpga命令，后面是数据
            byteTemp[0] = Convert.ToByte('A');
            byteTemp[4] = 5;
            byteTemp[7] = (byte)((charsCommand.Length + 2) >> 8);
            byteTemp[8] = (byte)(charsCommand.Length+2);
            switch (iType)
            {
                case 0://启动Mca
                    byteTemp[9] = 0x80;
                    byteTemp[10] = 0x00;
                    break;
                case 1://暂停Mca
                    byteTemp[9] = 0x80;
                    byteTemp[10] = 0x00;
                    break;
                case 2://获取谱数据
                    byteTemp[9] = 0x80;
                    byteTemp[10] = 0x00;
                    break;
                case 3://清除谱数据和状态
                    byteTemp[9] = 0x80;
                    byteTemp[10] = 0x00;
                    break;
                case 4://设置寄存器（所有）
                    byteTemp[9] = 0x70;
                    byteTemp[10] = 0x00;
                    break;
                case 5://读取寄存器
                    byteTemp[9] = 0xF0;
                    byteTemp[10] = 0x00;
                    break;
                case 6://读取Status
                    byteTemp[9] = 0xD0;
                    byteTemp[10] = 0x00;
                    break;
                case 7://读取硬件ID
                    byteTemp[9] = 0xE0;
                    byteTemp[10] = 0x00;
                    break;

            }
            for (int i = 0; i < charsCommand.Length;i++ )
            {
                byteTemp[i + 11] = (byte)charsCommand[i];
            }
            return byteTemp;
        }

        /// <summary>
        /// 取Status的Byte数组
        /// </summary>
        public byte[] GetStatusCommand
        {
            get
            {
                //byte[] byteTemp = new byte[11];//前9字节，2字节fpga命令，后面是数据
                //byteTemp[0] = Convert.ToByte('A');
                //byteTemp[4] = 5;
                //byteTemp[7] = 0;
                //byteTemp[8] = 2;
                //byteTemp[9] = 0xD0;
                //byteTemp[10] = 0x00;
                byte[] byteTemp = new byte[4];//前9字节，2字节fpga命令，后面是数据
                byteTemp[0] = 0x70;
                byteTemp[1] = 0xCC;
                byteTemp[2] = 0xD0;
                byteTemp[3] = 0x00;
                return byteTemp;
            }
        }
        /// <summary>
        /// 启动MCA命令的Byte数组
        /// </summary>
        public byte[] EnableMCACommand
        {
            get
            {
                //byte[] byteTemp = new byte[9+8];//前9字节，2字节fpga命令，后面是数据(两个写命令)
                //byteTemp[0] = Convert.ToByte('A'); 
                //byteTemp[4] = 5;
                //byteTemp[7] = 0;
                //byteTemp[8] = 8;
                //byteTemp[9] = 0xF0;
                //byteTemp[10] = 0x00;
                //byteTemp[11] = FPGA_CTRL0[0];
                //byteTemp[12] = (byte)(FPGA_CTRL0[1]|0x02);
                //byteTemp[13] = 0xF0;
                //byteTemp[14] = 0x00;
                //byteTemp[15] = FPGA_CTRL0[0];
                //byteTemp[16] = (byte)(FPGA_CTRL0[1] & 0xFD);
                byte[] byteTemp = new byte[8];//前9字节，2字节fpga命令，后面是数据
                byteTemp[0] = 0x70;
                byteTemp[1] = 0x00;
                byteTemp[2] = FPGA_CTRL0[0];
                byteTemp[3] = (byte)(FPGA_CTRL0[1] | 0x02);
                byteTemp[4] = 0x70;
                byteTemp[5] = 0x00;
                byteTemp[6] = FPGA_CTRL0[0];
                byteTemp[7] = (byte)(FPGA_CTRL0[1] & 0xFD);
                return byteTemp;
            }
        }

        public byte[] SetStopValue
        {
            get
            {
                int acquisitionstopValue = PresetTestTime * 10;//-----------------------------------------------------------------------预设时间为0.1s单位
                byte[] byteTemp = new byte[8];//前9字节，2字节fpga命令，后面是数据
                byteTemp[0] = 0x70;
                byteTemp[1] = 0x33;
                //byteTemp[2] = (byte)(acquisitionstopValue >> 24 & 0xFF);
                //byteTemp[3] = (byte)(acquisitionstopValue >> 16 & 0xFF);
                byteTemp[2] = (byte)(acquisitionstopValue >> 8 & 0xFF);
                byteTemp[3] = (byte)(acquisitionstopValue & 0xFF);
                byteTemp[4] = 0x70;
                byteTemp[5] = 0x3C;
                //byteTemp[6] = (byte)(acquisitionstopValue >> 8 & 0xFF);
                //byteTemp[7] = (byte)(acquisitionstopValue & 0xFF);
                byteTemp[6] = (byte)(acquisitionstopValue >> 24 & 0xFF);
                byteTemp[7] = (byte)(acquisitionstopValue >> 16 & 0xFF);
                //byteTemp[7] = (byte)(acquisitionstopValue >> 8 & 0xFF);
                //byteTemp[6] = (byte)(acquisitionstopValue & 0xFF);
                return byteTemp;
            }
        }

        public byte[] SetFineGain
        {
             get
            {
                string strAdcGain = string.Empty;
                string[] dp5configs = strDp5ConfigFile.Split(new char[] { ';' });
                string strElem = dp5configs.First(w => w.Contains("ADGAIN=")) != null ? dp5configs.First(w => w.Contains("ADGAIN=")) : "ADGAIN=GAIN1";
                strElem = strElem.Replace("ADGAIN=", "");
                strAdcGain = strElem.Replace(";", "").ToUpper();
                byte[] byteTemp = new byte[4];
                byteTemp[0] = 0x70;
                byteTemp[1] = 0xC3;
                int AdcGain = 1;
                if (strAdcGain == "GAIN1") AdcGain = 1;
                else if (strAdcGain == "GAIN2") AdcGain = 2;
                else if (strAdcGain == "GAIN3") AdcGain = 4;
                else if (strAdcGain == "GAIN4") AdcGain = 8;
                byteTemp[2] = (byte)(AdcGain << 4 & 0xF0);
                //int iDigitalGain = 2048;
                int iDigitalGain =(Dpp100Version == 0xA1 ? 2048 : 2047);
                //iDigitalGain += (int)((FineGain - 1.00) * 2048);
                 iDigitalGain += (int)((FineGain - 1.00) * (Dpp100Version == 0xA1 ?2048: 8192));
                //Console.WriteLine((Dpp100Version > 0xA1 ? 8192 : 2048));
                byteTemp[2] |= (byte)(iDigitalGain >> 8 & 0x0F);
                byteTemp[3] |= (byte)(iDigitalGain);
                return byteTemp;
            }                
        }
        /// <summary>
        /// 停止MCA命令的Byte数组
        /// </summary>
        public byte[] DisabelMCACommand
        {
            get
            {
                //byte[] byteTemp = new byte[9 + 8];//前9字节，2字节fpga命令，后面是数据(两个写命令)
                //byteTemp[0] = Convert.ToByte('A');
                //byteTemp[4] = 5;
                //byteTemp[7] = 0;
                //byteTemp[8] = 8;
                //byteTemp[9] = 0xF0;
                //byteTemp[10] = 0x00;
                //byteTemp[11] = FPGA_CTRL0[0];
                //byteTemp[12] = (byte)(FPGA_CTRL0[1] | 0x01);
                //byteTemp[13] = 0xF0;
                //byteTemp[14] = 0x00;
                //byteTemp[15] = FPGA_CTRL0[0];
                //byteTemp[16] = (byte)(FPGA_CTRL0[1] & 0xFE);
               
                byte[] byteTemp = new byte[8];//前9字节，2字节fpga命令，后面是数据
                byteTemp[0] = 0x70;
                byteTemp[1] = 0x00;
                byteTemp[2] = FPGA_CTRL0[0];
                byteTemp[3] = (byte)(FPGA_CTRL0[1] | 0x01);
                byteTemp[4] = 0x70;
                byteTemp[5] = 0x00;
                byteTemp[6] = FPGA_CTRL0[0];
                byteTemp[7] = (byte)(FPGA_CTRL0[1] & 0xFE);
               
                return byteTemp;
            }
        }
        /// <summary>
        /// 获取谱和状态命令的Byte数组
        /// </summary>
        public byte[] GetSpectrumCommand
        {
            get
            {
                //byte[] byteTemp = new byte[11];//前9字节，2字节fpga命令，后面是数据
                //byteTemp[0] = Convert.ToByte('A');
                //byteTemp[4] = 5;
                //byteTemp[7] = 0;
                //byteTemp[8] = 2;
                //byteTemp[9] = 0x80;
                //byteTemp[10] = 0x00;
                byte[] byteTemp = new byte[4];//前9字节，2字节fpga命令，后面是数据
                byteTemp[0] = 0x70;
                byteTemp[1] = 0xCC;
                byteTemp[2] = 0x80;
                byteTemp[3] = 0x00;
                return byteTemp;
            }
        }
        /// <summary>
        /// 清空谱和状态的命令的Byte数组
        /// </summary>
        public byte[] ClearSpectrumCommand
        {
            get
            {
                //byte[] byteTemp = new byte[9 + 8];//前9字节，2字节fpga命令，后面是数据(两个写命令)
                //byteTemp[0] = Convert.ToByte('A');
                //byteTemp[4] = 5;
                //byteTemp[7] = 0;
                //byteTemp[8] = 8;
                //byteTemp[9] = 0xF0;
                //byteTemp[10] = 0x00;
                //byteTemp[11] = FPGA_CTRL0[0];
                //byteTemp[12] = (byte)(FPGA_CTRL0[1] | 0x04);
                //byteTemp[13] = 0xF0;
                //byteTemp[14] = 0x00;
                //byteTemp[15] = FPGA_CTRL0[0];
                //byteTemp[16] = (byte)(FPGA_CTRL0[1] & 0xFB);
                byte[] byteTemp = new byte[8];//前9字节，2字节fpga命令，后面是数据
                byteTemp[0] = 0x70;
                byteTemp[1] = 0x00;
                byteTemp[2] = FPGA_CTRL0[0];
                byteTemp[3] = (byte)(FPGA_CTRL0[1] | 0x04);
                byteTemp[4] = 0x70;
                byteTemp[5] = 0x00;
                byteTemp[6] = FPGA_CTRL0[0];
                byteTemp[7] = (byte)(FPGA_CTRL0[1] & 0xFB);
                return byteTemp;
            }
        }

        /// <summary>
        /// 设置Config命令的Byte数组
        /// </summary>
        public byte[] SetConfigCommand
        {
            get
            {
                string[] dp5configs = strDp5ConfigFile.Split(new char[] { ';' });
                //int ipeakSearchLen = 0;

                //修改peakSearchLen-〉PreAmpResetLockout2014-05-21
                int preAMPResetLockout = 0;
                string strElem = dp5configs.First(w => w.Contains("PREAMPLOCKOUT=")) != null ? dp5configs.First(w => w.Contains("PREAMPLOCKOUT=")) : "PREAMPLOCKOUT=1";
                strElem = strElem.Replace("PREAMPLOCKOUT=", "");
                strElem = strElem.Replace(";", "");
                preAMPResetLockout = int.Parse(strElem);
                //新增顶宽时间2014-05-21
                int FlatTop = 0;
                strElem = dp5configs.First(w => w.Contains("FLATTOPWITH=")) != null ? dp5configs.First(w => w.Contains("FLATTOPWITH=")) : "FLATTOPWITH=0";
                strElem = strElem.Replace("FLATTOPWITH=", "");
                strElem = strElem.Replace(";", "");
                FlatTop = int.Parse(strElem);


                bool bplieUpReject = false;
                strElem = dp5configs.First(w => w.Contains("PILEUPREJECT=")) != null ? dp5configs.First(w => w.Contains("PILEUPREJECT=")) : "PILEUPREJECT=ON";
                strElem = strElem.Replace("PILEUPREJECT=", "");
                strElem = strElem.Replace(";", "");
                bplieUpReject = strElem.ToUpper()=="ON"?true:false;


                int iadcValue = 0;
                strElem = dp5configs.First(w => w.Contains("ADCTHRESHOLD=")) != null ? dp5configs.First(w => w.Contains("ADCTHRESHOLD=")) : "ADCTHRESHOLD=120";
                strElem = strElem.Replace("ADCTHRESHOLD=", "");
                strElem = strElem.Replace(";", "");
                iadcValue = int.Parse(strElem);


                //bool bTcSelect = false;
                //strElem = dp5configs.First(w => w.Contains("TCSELECT=")) != null ? dp5configs.First(w => w.Contains("TCSELECT=")) : "TCSELECT=ON";
                //strElem = strElem.Replace("TCSELECT=", "");
                //strElem = strElem.Replace(";", "");
                //bTcSelect = strElem.ToUpper() == "ON" ? true : false;
                //修改TCSELECT-〉firBLRSpeed2014-05-21
                int ifirBLRSpeed = 0;
                strElem = dp5configs.First(w => w.Contains("FIRBLRSPEED=")) != null ? dp5configs.First(w => w.Contains("FIRBLRSPEED=")) : "FIRBLRSPEED=1";
                strElem = strElem.Replace("FIRBLRSPEED=", "");
                strElem = strElem.Replace(";", "");
                ifirBLRSpeed = int.Parse(strElem);

                //新增ZeroOffsetValue2014-05-21
                int iZeroOffsetValue = 0;
                strElem = dp5configs.First(w => w.Contains("ZEROOFFSETVALUE=")) != null ? dp5configs.First(w => w.Contains("ZEROOFFSETVALUE=")) : "ZEROOFFSETVALUE=1";
                strElem = strElem.Replace("ZEROOFFSETVALUE=", "");
                strElem = strElem.Replace(";", "");
                iZeroOffsetValue = int.Parse(strElem);



                int iTcOffset = 0;
                strElem = dp5configs.First(w => w.Contains("TC_OFFSET_CFG=")) != null ? dp5configs.First(w => w.Contains("TC_OFFSET_CFG=")) : "TC_OFFSET_CFG=3277";
                strElem = strElem.Replace("TC_OFFSET_CFG=", "");
                strElem = strElem.Replace(";", "");
                iTcOffset = int.Parse(strElem);


                string strAdcGain = string.Empty;
                strElem = dp5configs.First(w => w.Contains("ADGAIN=")) != null ? dp5configs.First(w => w.Contains("ADGAIN=")) : "ADGAIN=GAIN1";
                strElem = strElem.Replace("ADGAIN=", "");
                strAdcGain = strElem.Replace(";", "").ToUpper();


                double dDigitalGain = FineGain;
                //strElem = dp5configs.First(w => w.Contains("DIGITALGAIN=")) != null ? dp5configs.First(w => w.Contains("DIGITALGAIN=")) : "DIGITALGAIN=1.0000";
                //strElem = strElem.Replace("DIGITALGAIN=", "");
                //strElem = strElem.Replace(";", "");
                //dDigitalGain = double.Parse(strElem);

                int iFirLen = 16;
                strElem = dp5configs.First(w => w.Contains("FIRLEN=")) != null ? dp5configs.First(w => w.Contains("FIRLEN=")) : "FIRLEN=256";
                strElem = strElem.Replace("FIRLEN=", "");
                strElem = strElem.Replace(";", "");
                iFirLen = int.Parse(strElem);

                int iFirThresh = 0;
                strElem = dp5configs.First(w => w.Contains("FIRTHRESHOLD=")) != null ? dp5configs.First(w => w.Contains("FIRTHRESHOLD=")) : "FIRTHRESHOLD=200";
                strElem = strElem.Replace("FIRTHRESHOLD=", "");
                strElem = strElem.Replace(";", "");
                iFirThresh = int.Parse(strElem);

                int iDigitalThresh = 0;
                strElem = dp5configs.First(w => w.Contains("DIGITALTHRESHOLD=")) != null ? dp5configs.First(w => w.Contains("DIGITALTHRESHOLD=")) : "DIGITALTHRESHOLD=35";
                strElem = strElem.Replace("DIGITALTHRESHOLD=", "");
                strElem = strElem.Replace(";", "");
                iDigitalThresh = int.Parse(strElem);


                strElem = dp5configs.First(w => w.Contains("BLR=")) != null ? dp5configs.First(w => w.Contains("BLR=")) : "BLR=OFF";
                strElem = strElem.Replace("BLR=", "");
                strElem = strElem.Replace(";", "");
                if (strElem.Trim().ToUpper() == "OFF")
                    BLR_ON = false;
                else BLR_ON = true;
                byte[] byteCmd = new byte[68];

                byte[] cmdTemp = new byte[4];

                #region 顺序
                //cmdTemp[0] = 0x70;
                //cmdTemp[1] = 0x03;
                //cmdTemp[2] = 0;
                //cmdTemp[3] = 0;

                //string strElem = dp5configs.First(w => w.Contains("TCSELECT=")) != null ? dp5configs.First(w => w.Contains("TCSELECT=")) : "TCSELECT=ON";
                //strElem = strElem.Replace("TCSELECT=", "");
                //strElem = strElem.Replace(";", "");
                //if (strElem.Trim().ToUpper() == "ON")
                //    cmdTemp[3] |= 0x02;
                //Array.Copy(cmdTemp, 0, byteCmd, 0, 4);//内部寄存器1  设置Tcselect enable

                //cmdTemp[0] = 0x73;
                //cmdTemp[1] = 0x03;
                //cmdTemp[2] = 0;
                //cmdTemp[3] = 0;
                //strElem = dp5configs.First(w => w.Contains("TC_OFFSET_CFG=")) != null ? dp5configs.First(w => w.Contains("TC_OFFSET_CFG=")) : "TC_OFFSET_CFG=3277";
                //strElem = strElem.Replace("TC_OFFSET_CFG=", "");
                //strElem = strElem.Replace(";", "");
                //int offset = Convert.ToInt32(strElem.Trim());
                //cmdTemp[2] = (byte)((int)offset / 256);
                //cmdTemp[3] = (byte)((int)offset % 256);
                //Array.Copy(cmdTemp, 0, byteCmd,4, 4);//外部寄存器 TC_OFFSET_CFG


                //cmdTemp[0] = 0x70;
                //cmdTemp[1] = 0xC3;
                //cmdTemp[2] = 0;
                //cmdTemp[3] = 0;
                //strElem = dp5configs.First(w => w.Contains("ADGAIN=")) != null ? dp5configs.First(w => w.Contains("ADGAIN=")) : "ADGAIN=GAIN1";
                //strElem = strElem.Replace("ADGAIN=", "");
                //strElem = strElem.Replace(";", "");
                //int AdcGain = 1;
                //if (strElem.Trim().ToUpper() == "GAIN1") AdcGain = 1;
                //else if (strElem.Trim().ToUpper() == "GAIN2") AdcGain = 2;
                //else if (strElem.Trim().ToUpper() == "GAIN3") AdcGain = 4;
                //else if (strElem.Trim().ToUpper() == "GAIN4") AdcGain = 8;
                //cmdTemp[2] = (byte)(AdcGain << 4 & 0xF0);
                //strElem = dp5configs.First(w => w.Contains("DIGITALGAIN=")) != null ? dp5configs.First(w => w.Contains("DIGITALGAIN=")) : "DIGITALGAIN=1.0000";
                //strElem = strElem.Replace("DIGITALGAIN=", "");
                //strElem = strElem.Replace(";", "");
                //double digitalGain = double.Parse(strElem.Trim());
                //int iDigitalGain = 2048;
                //if (digitalGain > 1.00) iDigitalGain += (int)((digitalGain - 1.00) * 2048);
                //if (digitalGain > 1.00) iDigitalGain += (int)((1.0 - digitalGain) * 2048);
                //cmdTemp[2] |= (byte)(iDigitalGain >> 8 & 0x0F);
                //cmdTemp[3] |= (byte)(iDigitalGain);
                //Array.Copy(cmdTemp, 0, byteCmd, 8, 4);//内部寄存器7 


                //cmdTemp[0] = 0x70;
                //cmdTemp[1] = 0x03;
                //cmdTemp[2] = 0;
                //cmdTemp[3] = 0;
                //strElem = dp5configs.First(w => w.Contains("PEAKSEARCHLEN=")) != null ? dp5configs.First(w => w.Contains("PEAKSEARCHLEN=")) : "FIRLEN=1";
                //strElem = strElem.Replace("PEAKSEARCHLEN=", "");
                //strElem = strElem.Replace(";", "");
                //if (strElem == "1")
                //    cmdTemp[2] = 0x20;
                //else if (strElem == "2")
                //    cmdTemp[2] = 0x40;

                //strElem = dp5configs.First(w => w.Contains("PILEUPREJECT=")) != null ? dp5configs.First(w => w.Contains("PILEUPREJECT=")) : "PILEUPREJECT=ON";
                //strElem = strElem.Replace("PILEUPREJECT=", "");
                //strElem = strElem.Replace(";", "");
                //if (strElem.Trim().ToUpper() == "ON")
                //    cmdTemp[2] |= 0x10;

                //strElem = dp5configs.First(w => w.Contains("ADCTHRESHOLD=")) != null ? dp5configs.First(w => w.Contains("ADCTHRESHOLD=")) : "ADCTHRESHOLD=120";
                //strElem = strElem.Replace("ADCTHRESHOLD=", "");
                //strElem = strElem.Replace(";", "");
                //int adcValue = Convert.ToInt32(strElem);
                //cmdTemp[2] |= (byte)((adcValue >> 6) & 0x0F);
                //cmdTemp[3] |= (byte)((adcValue << 2) & 0xFC);

                //strElem = dp5configs.First(w => w.Contains("TCSELECT=")) != null ? dp5configs.First(w => w.Contains("TCSELECT=")) : "TCSELECT=ON";
                //strElem = strElem.Replace("TCSELECT=", "");
                //strElem = strElem.Replace(";", "");
                //if (strElem.Trim().ToUpper() == "ON")
                //    cmdTemp[3] |= 0x02;
                //Array.Copy(cmdTemp, 0, byteCmd, 12, 4);//内部寄存器1

                //cmdTemp[0] = 0x70;
                //cmdTemp[1] = 0;
                //cmdTemp[2] = 0;
                //cmdTemp[3] = 0;
                //strElem = dp5configs.First(w => w.Contains("BLR=")) != null ? dp5configs.First(w => w.Contains("BLR=")) : "BLR=ON";
                //strElem = strElem.Replace("BLR=", "");
                //strElem = strElem.Replace(";", "");
                //if (strElem.Trim().ToUpper() == "ON")
                //    cmdTemp[2] = 0x20;
                //else cmdTemp[2] = 0x00;
                //cmdTemp[2] |= 0x40;
                //Array.Copy(cmdTemp, 0, byteCmd, 16, 4);//内部寄存器0

                //cmdTemp[0] = 0x70;
                //cmdTemp[1] = 0;
                //cmdTemp[2] = 0;
                //cmdTemp[3] = 0;
                //strElem = dp5configs.First(w => w.Contains("BLR=")) != null ? dp5configs.First(w => w.Contains("BLR=")) : "BLR=ON";
                //strElem = strElem.Replace("BLR=", "");
                //strElem = strElem.Replace(";", "");
                //if (strElem.Trim().ToUpper() == "ON")
                //    cmdTemp[2] = 0x20;
                //else cmdTemp[2] = 0x00;
                //Array.Copy(cmdTemp, 0, byteCmd, 20, 4);//内部寄存器 0  Reset BLR   



                //cmdTemp = new byte[4];
                //cmdTemp[0] = 0x70;
                //cmdTemp[1] = 0;
                //strElem = dp5configs.First(w => w.Contains("BLR=")) != null ? dp5configs.First(w => w.Contains("BLR=")) : "BLR=ON";
                //strElem = strElem.Replace("BLR=", "");
                //strElem = strElem.Replace(";", "");
                //if (strElem.Trim().ToUpper() == "ON")
                //    cmdTemp[2] = 0x20;
                //else cmdTemp[2] = 0x00;
                //strElem = dp5configs.First(w => w.Contains("FIRLEN=")) != null ? dp5configs.First(w => w.Contains("FIRLEN=")) : "FIRLEN=256";
                //strElem = strElem.Replace("FIRLEN=", "");
                //strElem = strElem.Replace(";", "");
                //int firType = Convert.ToInt32(strElem);
                //cmdTemp[2] |= (byte)(((firType - 1)&0xF8>>3)& 0x1F);
                //cmdTemp[3] |= 0xE8;
                //Array.Copy(cmdTemp, 0, byteCmd, 24,4);//内部寄存器0

                //cmdTemp[0] = 0x70;
                //cmdTemp[1] = 0x30;
                //cmdTemp[2] = 0;
                //cmdTemp[3] = 0;
                //strElem = dp5configs.First(w => w.Contains("FIRTHRESHOLD=")) != null ? dp5configs.First(w => w.Contains("FIRTHRESHOLD=")) : "FIRTHRESHOLD=200";
                //strElem = strElem.Replace("FIRTHRESHOLD=", "");
                //strElem = strElem.Replace(";", "");
                //int firthresholdvalue = Convert.ToInt16(strElem.Trim());
                //cmdTemp[2] = (byte)(firthresholdvalue >> 8 & 0x03);
                //cmdTemp[3] = (byte)firthresholdvalue;
                //Array.Copy(cmdTemp, 0, byteCmd, 28, 4);//内部寄存器3

                //cmdTemp[0] = 0x70;
                //cmdTemp[1] = 0x0C;
                //cmdTemp[2] = 0;
                //cmdTemp[3] = 0;
                //strElem = dp5configs.First(w => w.Contains("INSPECTLEN=")) != null ? dp5configs.First(w => w.Contains("INSPECTLEN=")) : "INSPECTLEN=70";
                //strElem = strElem.Replace("INSPECTLEN=", "");
                //strElem = strElem.Replace(";", "");
                //int spectLen = Convert.ToInt16(strElem.Trim());
                //cmdTemp[2] = (byte)spectLen;

                //strElem = dp5configs.First(w => w.Contains("DIGITALTHRESHOLD=")) != null ? dp5configs.First(w => w.Contains("DIGITALTHRESHOLD=")) : "DIGITALTHRESHOLD=35";
                //strElem = strElem.Replace("DIGITALTHRESHOLD=", "");
                //strElem = strElem.Replace(";", "");
                //int digitalLevel = Convert.ToInt16(strElem.Trim());
                //cmdTemp[3] = (byte)digitalLevel;
                //Array.Copy(cmdTemp, 0, byteCmd, 32, 4);//内部寄存器2


               
                                                         

               
                //cmdTemp[0] = 0x70;
                //cmdTemp[1] = 0x33;
                //cmdTemp[2] = 0;
                //cmdTemp[3] = 0;
                //strElem = dp5configs.First(w => w.Contains("STOPVALUE=")) != null ? dp5configs.First(w => w.Contains("STOPVALUE=")) : "STOPVALUE=100000";
                //strElem = strElem.Replace("STOPVALUE=", "");
                //strElem = strElem.Replace(";", "");
                //int acquisitionstopValue = Convert.ToInt16(strElem.Trim());
                //cmdTemp[2] = (byte)(acquisitionstopValue >> 24 & 0xFF);
                //cmdTemp[3] = (byte)(acquisitionstopValue >> 16 & 0xFF);
                //Array.Copy(cmdTemp, 0, byteCmd,36, 4);//内部寄存器4


                //cmdTemp[0] = 0x70;
                //cmdTemp[1] = 0x3C;
                //cmdTemp[2] = 0;
                //cmdTemp[3] = 0;
                //cmdTemp[2] = (byte)(acquisitionstopValue >> 8 & 0xFF);
                //cmdTemp[3] = (byte)(acquisitionstopValue & 0xFF);
                //Array.Copy(cmdTemp, 0, byteCmd, 40, 4);//内部寄存器5



                //cmdTemp[0] = 0x70;
                //cmdTemp[1] = 0xC0;
                //cmdTemp[2] = 0;
                //cmdTemp[3] = 0;
                //strElem = dp5configs.First(w => w.Contains("STOPMODE=")) != null ? dp5configs.First(w => w.Contains("STOPMODE=")) : "STOPMODE=2";
                //strElem = strElem.Replace("STOPMODE=", "");
                //strElem = strElem.Replace(";", "");
                //int stopmodeType = Convert.ToInt16(strElem.Trim());
                //cmdTemp[2] |= (byte)(stopmodeType << 5 & 0xE0);
                //Array.Copy(cmdTemp, 0, byteCmd, 44, 4);//内部寄存器6


                //cmdTemp[0] = 0x70;
                //cmdTemp[1] = 0x00;
                //cmdTemp[2] = 0;
                //cmdTemp[3] = 0;
                //strElem = dp5configs.First(w => w.Contains("BLR=")) != null ? dp5configs.First(w => w.Contains("BLR=")) : "BLR=ON";
                //strElem = strElem.Replace("BLR=", "");
                //strElem = strElem.Replace(";", "");
                //if (strElem.Trim().ToUpper() == "ON")
                //    cmdTemp[2] = 0x20;
                //else cmdTemp[2] = 0x00;
                //strElem = dp5configs.First(w => w.Contains("FIRLEN=")) != null ? dp5configs.First(w => w.Contains("FIRLEN=")) : "FIRLEN=256";
                //strElem = strElem.Replace("FIRLEN=", "");
                //strElem = strElem.Replace(";", "");
                //firType = Convert.ToInt32(strElem);
                //cmdTemp[2] |= (byte)(((firType - 1) & 0xF8 >> 3) & 0x1F); ;
                //cmdTemp[3] |= 0xE0;
                //FPGA_CTRL0[0] = cmdTemp[2];
                //FPGA_CTRL0[1] = cmdTemp[3];
                //Array.Copy(cmdTemp, 0, byteCmd, 48, 4);//内部寄存器0  使能FIR

                //strElem = dp5configs.First(w => w.Contains("KEYSERIALNUMBER=")) != null ? dp5configs.First(w => w.Contains("KEYSERIALNUMBER=")) : "KEYSERIALNUMBER=FFFFFFFF";
                //strElem = strElem.Replace("KEYSERIALNUMBER=", "");
                //strElem = strElem.Replace(";", "");
                //char[] charsSeriral = strElem.Trim().ToCharArray();
                //byte[] serieralNumber = new byte[8];
                //for (int i = 0; i < serieralNumber.Length; i++)
                //{
                //    if (2 * i < charsSeriral.Length && 2 * i + 1 < charsSeriral.Length) serieralNumber[i] = Convert.ToByte(HexCharToInt(charsSeriral[2 * i]) << 4 | (HexCharToInt(charsSeriral[2 * i + 1]) & 0x0F));
                //}
                //for (int i = 1; i < 5; i++)
                //{
                //    cmdTemp[0] = 0x73;
                //    switch (i)
                //    {
                //        case 1:
                //            cmdTemp[1] = 0x3C;
                //            break;
                //        case 2:
                //            cmdTemp[1] = 0xC0;
                //            break;
                //        case 3:
                //            cmdTemp[1] = 0xC3;
                //            break;
                //        case 4:
                //            cmdTemp[1] = 0xCC;
                //            break;
                            
                //    }
                //    cmdTemp[2] = serieralNumber[(i-1)*2];
                //    cmdTemp[3] = serieralNumber[(i - 1) * 2+1];
                //    Array.Copy(cmdTemp, 0, byteCmd, 48 + i * 4, 4);//serieralnumber寄存器
                //}
                #endregion

                #region
                cmdTemp[0] = 0x70;
                cmdTemp[1] = 0x03;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                //if (ipeakSearchLen == 1)
                //{
                //    cmdTemp[2] = 0x20;
                //}
                //else if (ipeakSearchLen == 2)
                //{
                //    cmdTemp[2] = 0x40;
                //}
                cmdTemp[2] = (byte)((FlatTop << 7) & 0x80);
                cmdTemp[2] |= (byte)((preAMPResetLockout << 5) & 0x60);//复位
                if(bplieUpReject)
                    cmdTemp[2] |= 0x10;

                cmdTemp[2] |= (byte)((iadcValue >> 6) & 0x0F);
                cmdTemp[3] |= (byte)((iadcValue << 2) & 0xFC);
                //if (bTcSelect)
                //    cmdTemp[3] |= 0x03;
                cmdTemp[3] |= (byte)(ifirBLRSpeed & 0x03);
                Array.Copy(cmdTemp, 0, byteCmd, 0, 4);//内部寄存器1(Tc_SECT)


                cmdTemp[0] = 0x73;
                cmdTemp[1] = 0x03;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                cmdTemp[2] = (byte)(iTcOffset / 256);
                cmdTemp[3] = (byte)(iTcOffset % 256);
                Array.Copy(cmdTemp, 0, byteCmd, 4, 4);//外部寄存器 TC_OFFSET_CFG


                cmdTemp[0] = 0x70;
                cmdTemp[1] = 0xC3;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                int AdcGain = 1;
                if (strAdcGain == "GAIN1") AdcGain = 1;
                else if (strAdcGain == "GAIN2") AdcGain = 2;
                else if (strAdcGain == "GAIN3") AdcGain = 4;
                else if (strAdcGain == "GAIN4") AdcGain = 8;
                cmdTemp[2] = (byte)(AdcGain << 4 & 0xF0);
                //int iDigitalGain = 2048;
                //iDigitalGain += (int)((dDigitalGain - 1.00) * 2048);
                //int iDigitalGain = 2048;
                int iDigitalGain = (Dpp100Version == 0xA1 ? 2048 : 2047);
                //iDigitalGain += (int)((dDigitalGain - 1.00) * 8192);
                iDigitalGain += (int)((dDigitalGain - 1.00) * (Dpp100Version == 0xA1 ? 2048 : 8192));
                
               // Console.WriteLine((Dpp100Version > 0xA1 ? 8192 : 2048));
                cmdTemp[2] |= (byte)(iDigitalGain >> 8 & 0x0F);
                cmdTemp[3] |= (byte)(iDigitalGain);
                Array.Copy(cmdTemp, 0, byteCmd, 8, 4);//内部寄存器7 (Anal_Gain)


                cmdTemp[0] = 0x70;
                cmdTemp[1] = 0x03;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                //if (ipeakSearchLen == 1)
                //{
                //    cmdTemp[2] = 0x20;
                //}
                //else if (ipeakSearchLen == 2)
                //{
                //    cmdTemp[2] = 0x40;
                //}
                cmdTemp[2] = (byte)((FlatTop << 7) & 0x80);
                cmdTemp[2] |= (byte)((preAMPResetLockout << 5) & 0x60);//复位
                if (bplieUpReject)
                    cmdTemp[2] |= 0x10;

                cmdTemp[2] |= (byte)((iadcValue >> 6) & 0x0F);
                cmdTemp[3] |= (byte)((iadcValue << 2) & 0xFC);
                //if (bTcSelect)
                //    cmdTemp[3] |= 0x03;
                cmdTemp[3] |= (byte)(ifirBLRSpeed & 0x03);
                Array.Copy(cmdTemp, 0, byteCmd, 12, 4);//内部寄存器1(ADC_Thresh)

                cmdTemp[0] = 0x70;
                cmdTemp[1] = 0;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                cmdTemp[2] = 0x20;//BLR ON
                cmdTemp[2] |= 0x40;//BLR reset 1
                cmdTemp[2] |= (byte)((((iFirLen - 1) & 0xF8) >> 3) & 0x1F);
                //cmdTemp[3] |= (byte)((((iFirLen - 1) & 0xF8) << 5) & 0xE0);
                cmdTemp[3] |= (byte)((((iFirLen - 1) & 0x07) << 5) & 0xE0);
                cmdTemp[3] |= 0x08;
                Array.Copy(cmdTemp, 0, byteCmd, 16, 4);//内部寄存器0 blrreset

                cmdTemp[0] = 0x70;
                cmdTemp[1] = 0;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                cmdTemp[2] = 0x20;//BLR ON, BLR reset 0
                cmdTemp[2] |= (byte)((((iFirLen - 1) & 0xF8) >> 3) & 0x1F);
                //cmdTemp[3] |= (byte)((((iFirLen - 1) & 0xF8) << 5) & 0xE0);
                cmdTemp[3] |= (byte)((((iFirLen - 1) & 0x07) << 5) & 0xE0);
                cmdTemp[3] |= 0x08;
                Array.Copy(cmdTemp, 0, byteCmd, 20, 4);//内部寄存器0blrreset



                cmdTemp[0] = 0x70;
                cmdTemp[1] = 0;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                cmdTemp[2] = 0x20;//BLR ON, BLR reset 0
                cmdTemp[2] |= (byte)((((iFirLen - 1) & 0xF8) >> 3) & 0x1F);
                //cmdTemp[3] |= (byte)((((iFirLen - 1) & 0xF8) << 5) & 0xE0);
                cmdTemp[3] |= (byte)((((iFirLen - 1) & 0x07) << 5) & 0xE0);
                cmdTemp[3] |= 0x08;
                Array.Copy(cmdTemp, 0, byteCmd, 24, 4);//内部寄存器0 Fir_Len

                cmdTemp[0] = 0x70;
                cmdTemp[1] = 0x30;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                cmdTemp[2] = (byte)(iZeroOffsetValue << 2 & 0xFC);//2014-05-21
                cmdTemp[2] |= (byte)(iFirThresh >> 8 & 0x03);
                cmdTemp[3] = (byte)iFirThresh;
                Array.Copy(cmdTemp, 0, byteCmd, 28, 4);//内部寄存器3 Fir_Thresh

                cmdTemp[0] = 0x70;
                cmdTemp[1] = 0x0C;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                //strElem = dp5configs.First(w => w.Contains("INSPECTLEN=")) != null ? dp5configs.First(w => w.Contains("INSPECTLEN=")) : "INSPECTLEN=70";
                //strElem = strElem.Replace("INSPECTLEN=", "");
                //strElem = strElem.Replace(";", "");
                //int spectLen = (iFirLen + 20) / 4;
                int spectLen = iFirLen/2+2 ;
                cmdTemp[2] = (byte)spectLen;
                cmdTemp[3] = (byte)iDigitalThresh;
                Array.Copy(cmdTemp, 0, byteCmd, 32, 4);//内部寄存器2 Fir_Len  Fast_FPGA_Thresh






                cmdTemp[0] = 0x70;
                cmdTemp[1] = 0x33;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                //strElem = dp5configs.First(w => w.Contains("STOPVALUE=")) != null ? dp5configs.First(w => w.Contains("STOPVALUE=")) : "STOPVALUE=100000";
                //strElem = strElem.Replace("STOPVALUE=", "");
                //strElem = strElem.Replace(";", "");
                //int acquisitionstopValue = Convert.ToInt16(strElem.Trim());
                int acquisitionstopValue = PresetTestTime * 10;//-----------------------------------------------------------------------预设时间为0.1s单位

                cmdTemp[2] = (byte)(acquisitionstopValue >> 8 & 0xFF);
                cmdTemp[3] = (byte)(acquisitionstopValue & 0xFF);
                //cmdTemp[2] = (byte)(acquisitionstopValue >> 24 & 0xFF);
                //cmdTemp[3] = (byte)(acquisitionstopValue >> 16 & 0xFF);
                Array.Copy(cmdTemp, 0, byteCmd, 36, 4);//内部寄存器4  


                cmdTemp[0] = 0x70;
                cmdTemp[1] = 0x3C;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                //cmdTemp[2] = (byte)(acquisitionstopValue >> 8 & 0xFF);
                //cmdTemp[3] = (byte)(acquisitionstopValue & 0xFF);
                cmdTemp[2] = (byte)(acquisitionstopValue >> 24 & 0xFF);
                cmdTemp[3] = (byte)(acquisitionstopValue >> 16 & 0xFF);
                Array.Copy(cmdTemp, 0, byteCmd, 40, 4);//内部寄存器5



                cmdTemp[0] = 0x70;
                cmdTemp[1] = 0xC0;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                strElem = dp5configs.First(w => w.Contains("STOPMODE=")) != null ? dp5configs.First(w => w.Contains("STOPMODE=")) : "STOPMODE=2";
                strElem = strElem.Replace("STOPMODE=", "");
                strElem = strElem.Replace(";", "");
                int stopmodeType = Convert.ToInt16(strElem.Trim());
                cmdTemp[2] |= (byte)(stopmodeType << 5 & 0xE0);
                if (Dpp100Version != 0xA1)
                {
                    strElem = dp5configs.First(w => w.Contains("QCPROTECT=")) != null ? dp5configs.First(w => w.Contains("QCPROTECT=")) : "QCPROTECT=4";
                    strElem = strElem.Replace("QCPROTECT=", "");
                    strElem = strElem.Replace(";", "");
                    int iQCProtect = Convert.ToInt16(strElem.Trim());
                    cmdTemp[3] |= (byte)(iQCProtect & 0x0F);
                }
                    //strElem = dp5configs.First(w => w.Contains("QCPROTECT=")) != null ? dp5configs.First(w => w.Contains("QCPROTECT=")) : "QCPROTECT=4";
                    //strElem = strElem.Replace("QCPROTECT=", "");
                    //strElem = strElem.Replace(";", "");
                    //int iQCProtect = Convert.ToInt16(strElem.Trim());
                    //cmdTemp[2] |= (byte)((iQCProtect/256) & 0x0F);
                    //cmdTemp[3] = (byte)(iQCProtect % 256);
                    

                Array.Copy(cmdTemp, 0, byteCmd, 44, 4);//内部寄存器6


                cmdTemp[0] = 0x70;
                cmdTemp[1] = 0x00;
                cmdTemp[2] = 0;
                cmdTemp[3] = 0;
                cmdTemp[2] = 0x20;//BLR ON, BLR reset 0
                cmdTemp[2] |= (byte)((((iFirLen - 1) & 0xF8) >> 3) & 0x1F);
                //cmdTemp[3] |= (byte)((((iFirLen - 1) & 0xF8) << 5) & 0xE0);
                cmdTemp[3] |= (byte)((((iFirLen - 1) & 0x07) << 5) & 0xE0);
                cmdTemp[3] |= 0x00;//FIR reset 0
                //FPGA_CTRL0[0] = (byte)(cmdTemp[2]&0xDF);//(保存寄存器1的值其中关闭BLR OFF)
                if (!BLR_ON)
                    FPGA_CTRL0[0] = (byte)(cmdTemp[2] & 0xDF);
                else FPGA_CTRL0[0] = (byte)cmdTemp[2];
                //if (!BLR_ON)
                //    cmdTemp[2] = (byte)(cmdTemp[2] & 0xDF);
                //else cmdTemp[2] = (byte)cmdTemp[2];
                //FPGA_CTRL0[0] = cmdTemp[2];
                FPGA_CTRL0[1] = cmdTemp[3];
                Array.Copy(cmdTemp, 0, byteCmd, 48, 4);//内部寄存器0  使能FIR_Reset

                strElem = dp5configs.First(w => w.Contains("KEYSERIALNUMBER=")) != null ? dp5configs.First(w => w.Contains("KEYSERIALNUMBER=")) : "KEYSERIALNUMBER=FFFFFFFF";
                strElem = strElem.Replace("KEYSERIALNUMBER=", "");
                strElem = strElem.Replace(";", "");
                char[] charsSeriral = strElem.Trim().ToCharArray();
                byte[] serieralNumber = new byte[8];
                for (int i = 0; i < serieralNumber.Length; i++)
                {
                    if (2 * i < charsSeriral.Length && 2 * i + 1 < charsSeriral.Length) serieralNumber[i] = Convert.ToByte(HexCharToInt(charsSeriral[2 * i]) << 4 | (HexCharToInt(charsSeriral[2 * i + 1]) & 0x0F));
                }
                for (int i = 1; i < 5; i++)
                {
                    cmdTemp[0] = 0x73;
                    switch (i)
                    {
                        case 1:
                            cmdTemp[1] = 0x3C;
                            break;
                        case 2:
                            cmdTemp[1] = 0xC0;
                            break;
                        case 3:
                            cmdTemp[1] = 0xC3;
                            break;
                        case 4:
                            cmdTemp[1] = 0xCC;
                            break;

                    }
                    cmdTemp[2] = serieralNumber[(i - 1) * 2];
                    cmdTemp[3] = serieralNumber[(i - 1) * 2 + 1];
                    Array.Copy(cmdTemp, 0, byteCmd, 48 + i * 4, 4);//serieralnumber寄存器
                }

                #endregion
                #region



                //string[] dp5configs= strDp5ConfigFile.Split(new char[] { ';' });
                //byte[] byteCmd = new byte[9+128];
                //byteCmd[0] = Convert.ToByte('A');
                //byteCmd[4] = 5;
                //byteCmd[8] = 128;
                //for (int j = 0; j < 2; j++)
                //{
                //    byte[] SetFpGA = new byte[64];

                //    byte[] cmdTemp = new byte[2];
                //    cmdTemp[0] = 0xF0;
                //    cmdTemp[1] = 0;
                //    Array.Copy(cmdTemp, 0, SetFpGA, 0, 2);//命令


                //    cmdTemp[0] = 0;
                //    cmdTemp[1] = 0;
                //    string strElem = dp5configs.First(w => w.Contains("BLR=")) != null ? dp5configs.First(w => w.Contains("BLR=")) : "BLR=ON";
                //    strElem = strElem.Replace("BLR=", "");
                //    strElem = strElem.Replace(";", "");
                //    if (strElem.Trim().ToUpper() == "ON")
                //        cmdTemp[0] = 0x20;
                //    else cmdTemp[0] = 0x00;
                //    if (j == 0)
                //    {
                //        cmdTemp[0] |= 0x40;
                //    }
                //    strElem = dp5configs.First(w => w.Contains("FIRLEN=")) != null ? dp5configs.First(w => w.Contains("FIRLEN=")) : "FIRLEN=256";
                //    strElem = strElem.Replace("FIRLEN=", "");
                //    strElem = strElem.Replace(";", "");
                //    int firType = Convert.ToInt32(strElem);
                //    if (j == 0)
                //    {
                //        cmdTemp[0] |= (byte)(((firType / 256 - 1) << 1) | 0x01);
                //        cmdTemp[1] |= 0xE8;
                //    }
                //    else
                //    {
                //        cmdTemp[0] |= (byte)(((firType / 256 - 1) << 1) | 0x01);
                //        cmdTemp[1] |= 0xE0;
                //    }
                //    Array.Copy(cmdTemp, 0, SetFpGA, 2, 2);//内部寄存器0



                //    cmdTemp[0] = 0;
                //    cmdTemp[1] = 0;
                //    strElem = dp5configs.First(w => w.Contains("PEAKSEARCHLEN=")) != null ? dp5configs.First(w => w.Contains("PEAKSEARCHLEN=")) : "FIRLEN=1";
                //    strElem = strElem.Replace("PEAKSEARCHLEN=", "");
                //    strElem = strElem.Replace(";", "");
                //    if (strElem == "1")
                //        cmdTemp[0] = 0x20;
                //    else if (strElem == "2")
                //        cmdTemp[0] = 0x40;

                //    strElem = dp5configs.First(w => w.Contains("PILEUPREJECT=")) != null ? dp5configs.First(w => w.Contains("PILEUPREJECT=")) : "PILEUPREJECT=ON";
                //    strElem = strElem.Replace("PILEUPREJECT=", "");
                //    strElem = strElem.Replace(";", "");
                //    if (strElem.Trim().ToUpper() == "ON")
                //        cmdTemp[0] |= 0x10;

                //    strElem = dp5configs.First(w => w.Contains("ADCTHRESHOLD=")) != null ? dp5configs.First(w => w.Contains("ADCTHRESHOLD=")) : "ADCTHRESHOLD=120";
                //    strElem = strElem.Replace("ADCTHRESHOLD=", "");
                //    strElem = strElem.Replace(";", "");
                //    int adcValue = Convert.ToInt32(strElem);
                //    cmdTemp[0] |= (byte)((adcValue >> 6) & 0x0F);
                //    cmdTemp[1] |= (byte)((adcValue << 2) & 0xFC);

                //    strElem = dp5configs.First(w => w.Contains("TCSELECT=")) != null ? dp5configs.First(w => w.Contains("TCSELECT=")) : "TCSELECT=ON";
                //    strElem = strElem.Replace("TCSELECT=", "");
                //    strElem = strElem.Replace(";", "");
                //    if (strElem.Trim().ToUpper() == "ON")
                //        cmdTemp[1] |= 0x02;
                //    Array.Copy(cmdTemp, 0, SetFpGA, 4, 2);//内部寄存器1


                //    cmdTemp[0] = 0;
                //    cmdTemp[1] = 0;
                //    strElem = dp5configs.First(w => w.Contains("INSPECTLEN=")) != null ? dp5configs.First(w => w.Contains("INSPECTLEN=")) : "INSPECTLEN=70";
                //    strElem = strElem.Replace("INSPECTLEN=", "");
                //    strElem = strElem.Replace(";", "");
                //    int spectLen = Convert.ToInt16(strElem.Trim());
                //    cmdTemp[0] = (byte)spectLen;

                //    strElem = dp5configs.First(w => w.Contains("DIGITALTHRESHOLD=")) != null ? dp5configs.First(w => w.Contains("DIGITALTHRESHOLD=")) : "DIGITALTHRESHOLD=35";
                //    strElem = strElem.Replace("DIGITALTHRESHOLD=", "");
                //    strElem = strElem.Replace(";", "");
                //    int digitalLevel = Convert.ToInt16(strElem.Trim());
                //    cmdTemp[1] = (byte)digitalLevel;
                //    Array.Copy(cmdTemp, 0, SetFpGA, 6, 2);//内部寄存器2


                //    cmdTemp[0] = 0;
                //    cmdTemp[1] = 0;
                //    strElem = dp5configs.First(w => w.Contains("FIRTHRESHOLD=")) != null ? dp5configs.First(w => w.Contains("FIRTHRESHOLD=")) : "FIRTHRESHOLD=200";
                //    strElem = strElem.Replace("FIRTHRESHOLD=", "");
                //    strElem = strElem.Replace(";", "");
                //    int firthresholdvalue = Convert.ToInt16(strElem.Trim());
                //    cmdTemp[0] = (byte)(firthresholdvalue >> 8 & 0x03);
                //    cmdTemp[1] = (byte)firthresholdvalue;
                //    Array.Copy(cmdTemp, 0, SetFpGA, 8, 2);//内部寄存器3


                //    cmdTemp[0] = 0;
                //    cmdTemp[1] = 0;
                //    strElem = dp5configs.First(w => w.Contains("STOPVALUE=")) != null ? dp5configs.First(w => w.Contains("STOPVALUE=")) : "STOPVALUE=100000";
                //    strElem = strElem.Replace("STOPVALUE=", "");
                //    strElem = strElem.Replace(";", "");
                //    int acquisitionstopValue = Convert.ToInt16(strElem.Trim());
                //    cmdTemp[0] = (byte)(acquisitionstopValue >> 24 & 0xFF);
                //    cmdTemp[1] = (byte)(acquisitionstopValue >> 16 & 0xFF);
                //    Array.Copy(cmdTemp, 0, SetFpGA, 10, 2);//内部寄存器4

                //    cmdTemp[0] = 0;
                //    cmdTemp[1] = 0;
                //    cmdTemp[0] = (byte)(acquisitionstopValue >> 8 & 0xFF);
                //    cmdTemp[1] = (byte)(acquisitionstopValue & 0xFF);
                //    Array.Copy(cmdTemp, 0, SetFpGA, 12, 2);//内部寄存器5


                //    cmdTemp[0] = 0;
                //    cmdTemp[1] = 0;

                //    strElem = dp5configs.First(w => w.Contains("STOPMODE=")) != null ? dp5configs.First(w => w.Contains("STOPMODE=")) : "STOPMODE=2";
                //    strElem = strElem.Replace("STOPMODE=", "");
                //    strElem = strElem.Replace(";", "");
                //    int stopmodeType = Convert.ToInt16(strElem.Trim());
                //    cmdTemp[0] |= (byte)(stopmodeType << 5 & 0xE0);
                //    Array.Copy(cmdTemp, 0, SetFpGA, 14, 2);//内部寄存器6

                //    cmdTemp[0] = 0;
                //    cmdTemp[1] = 0;
                //    strElem = dp5configs.First(w => w.Contains("ADGAIN=")) != null ? dp5configs.First(w => w.Contains("ADGAIN=")) : "ADGAIN=GAIN1";
                //    strElem = strElem.Replace("ADGAIN=", "");
                //    strElem = strElem.Replace(";", "");
                //    int AdcGain = 1;
                //    if (strElem.Trim().ToUpper() == "GAIN1") AdcGain = 1;
                //    else if (strElem.Trim().ToUpper() == "GAIN2") AdcGain = 2;
                //    else if (strElem.Trim().ToUpper() == "GAIN3") AdcGain = 3;
                //    else if (strElem.Trim().ToUpper() == "GAIN4") AdcGain = 4;
                //    cmdTemp[0] = (byte)(AdcGain << 4 & 0xF0);
                //    strElem = dp5configs.First(w => w.Contains("DIGITALGAIN=")) != null ? dp5configs.First(w => w.Contains("DIGITALGAIN=")) : "DIGITALGAIN=1.0000";
                //    strElem = strElem.Replace("DIGITALGAIN=", "");
                //    strElem = strElem.Replace(";", "");
                //    double digitalGain = double.Parse(strElem.Trim());
                //    int iDigitalGain = 2048;
                //    if (digitalGain > 1.00) iDigitalGain += (int)((digitalGain - 1.00) * 2048);
                //    if (digitalGain > 1.00) iDigitalGain += (int)((1.0 - digitalGain) * 2048);
                //    cmdTemp[0] |= (byte)(iDigitalGain >> 8 & 0x0F);
                //    cmdTemp[1] |= (byte)(iDigitalGain);
                //    Array.Copy(cmdTemp, 0, SetFpGA, 16, 2);//内部寄存器7 TC_OFFSET_CFG


                //    cmdTemp[0] = 0;
                //    cmdTemp[1] = 0;
                //    strElem = dp5configs.First(w => w.Contains("TC_OFFSET_CFG=")) != null ? dp5configs.First(w => w.Contains("TC_OFFSET_CFG=")) : "TC_OFFSET_CFG=3277";
                //    strElem = strElem.Replace("TC_OFFSET_CFG=", "");
                //    strElem = strElem.Replace(";", "");
                //    int offset = Convert.ToInt32(strElem.Trim());
                //    cmdTemp[0] = (byte)((int)offset / 256);
                //    cmdTemp[1] = (byte)((int)offset % 256);
                //    Array.Copy(cmdTemp, 0, SetFpGA, 18, 2);//外部寄存器

                //    strElem = dp5configs.First(w => w.Contains("KEYSERIALNUMBER=")) != null ? dp5configs.First(w => w.Contains("KEYSERIALNUMBER=")) : "KEYSERIALNUMBER=FFFFFFFF";
                //    strElem = strElem.Replace("KEYSERIALNUMBER=", "");
                //    strElem = strElem.Replace(";", "");
                //    char[] charsSeriral = strElem.Trim().ToCharArray();
                //    byte[] serieralNumber = new byte[8];
                //    for (int i = 0; i < serieralNumber.Length; i++)
                //    {
                //        if (2 * i < charsSeriral.Length && 2 * i + 1 < charsSeriral.Length) serieralNumber[i] = Convert.ToByte(charsSeriral[2 * i] << 4 | (charsSeriral[2 * i + 1] & 0x0F));
                //    }
                //    Array.Copy(cmdTemp, 0, SetFpGA, 20, 8);//serieralnumber寄存器

                //    Array.Copy(SetFpGA, 0, byteCmd, 9 + j * 64, 64);//seri
                //}
                #endregion
                return byteCmd;
            }
        }
        ///// <summary>
        ///// 获取Config命令的Byte数组
        ///// </summary>
        public byte[] GetConfigCommand
        {
            get
            {
                //byte[] byteTemp = new byte[11];//前9字节，2字节fpga命令，后面是数据
                //byteTemp[0] = Convert.ToByte('A');
                //byteTemp[4] = 5;
                //byteTemp[7] = 0;
                //byteTemp[8] = 2;
                //byteTemp[9] = 0xD0;
                //byteTemp[10] = 0x00;
                byte[] byteTemp = new byte[4];//前9字节，2字节fpga命令，后面是数据
                byteTemp[0] = 0x70;
                byteTemp[1] = 0xCC;
                byteTemp[2] = 0xD0;
                byteTemp[3] = 0x00;
                return byteTemp;
            }
        }

        public byte[] GetVoiceCommand
        {
            get
            {
                //byte[] byteTemp = new byte[11];//前9字节，2字节fpga命令，后面是数据
                //byteTemp[0] = Convert.ToByte('A');
                //byteTemp[4] = 5;
                //byteTemp[7] = 0;
                //byteTemp[8] = 2;
                //byteTemp[9] = 0xC0;
                //byteTemp[10] = 0x00;
                byte[] byteTemp = new byte[4];//前9字节，2字节fpga命令，后面是数据
                byteTemp[0] = 0x70;
                byteTemp[1] = 0xCC;
                byteTemp[2] = 0xC0;
                byteTemp[3] = 0x00;
                return byteTemp;
            }
        }
        public byte[] GetHardIDCommand
        {
            get
            {
                //byte[] byteTemp = new byte[11];//前9字节，2字节fpga命令，后面是数据
                //byteTemp[0] = Convert.ToByte('A');
                //byteTemp[4] = 5;
                //byteTemp[7] = 0;
                //byteTemp[8] = 2;
                //byteTemp[9] = 0xE0;
                //byteTemp[10] = 0x00;
                byte[] byteTemp = new byte[4];//前9字节，2字节fpga命令，后面是数据
                byteTemp[0] = 0x70;
                byteTemp[1] = 0xCC;
                byteTemp[2] = 0xE0;
                byteTemp[3] = 0x00;
                return byteTemp;
            }
        }
        /// <summary>
        /// 处理接收的byte数组
        /// </summary>
        /// <param name="RecData"></param>
        /// <returns></returns>
        public bool ProcessExData(byte[] cmd,byte[] RecData)
        {
            //if (RecData[0] != 'A')
            //{
            //    return false;
            //}
            //int dataLen = RecData[7] * 256 + RecData[8];
            //if (dataLen < 9+2) return false;
            //int PID1 = RecData[9];
            //int PID2 = RecData[10];
            int beginIndex = 1;
            int dataLen = RecData.Length;
            int PID1 = cmd[0];
            int PID2 = cmd[1];
            if ((PID1 & 0xF0) == 0x80)//谱解析
            {
                int DataLen = 8192*2;
                int specLen = 4096;
                
                if (dataLen < specLen*4+2)
                {
                    return false;
                }
                SPECTRUM = new long[specLen];
                for (int i = 0; i < specLen-1 ; i++)//取谱 最后一道数据不取（溢出所用2014-05-22）
                {
                    //SPECTRUM[i] = (long)(RecData[i * 4 + beginIndex] + RecData[i * 4 + 1 + beginIndex] * 256) + (long)RecData[i * 4 + 2 + beginIndex] * 65535 + (long)RecData[i * 4 + 3 + beginIndex] * 65535 * 256;
                    SPECTRUM[i] = (long)(RecData[i * 4 + beginIndex] * 256 + RecData[i * 4 + 1 + beginIndex]) + (long)(RecData[i * 4 + 2 + beginIndex] * 256 + (long)RecData[i * 4 + 3 + beginIndex]) * 65536;
                }
            }
            else if ((PID1 & 0xF0) == 0xD0)//接受到状态
            {
                for (int j = 0; j < Dp5Status.RAW.Length; j++)
                {
                    Dp5Status.RAW[j] = RecData[j + beginIndex];
                }
                Dp5Status.Process_Status();
            }
            else if ((PID1 & 0xF0) == 0xE0)//硬件ID
            {
                Dpp100HID = string.Empty;
                byte[] hard = new byte[32];
                byte[] hardId = new byte[7];
                for (int i = 0; i < hard.Length; i++)
                {
                    Dp5Status.RAW[i] = RecData[i + beginIndex];
                }
                hardId[0] = Dp5Status.RAW[7];
                hardId[1] = Dp5Status.RAW[6];
                hardId[2] = Dp5Status.RAW[5];
                hardId[3] = Dp5Status.RAW[4];
                hardId[4] = Dp5Status.RAW[3];
                hardId[5] = Dp5Status.RAW[2];
                hardId[6] = Dp5Status.RAW[1];
                Console.WriteLine("DppVersion:" + Dp5Status.RAW[0] + " " + Dp5Status.RAW[1]);
                Dpp100HID = BytesToHexChar(hardId);
                Dpp100Version = Dp5Status.RAW[0];
                bool bDppk100Error = false;
                if (Dp5Status.RAW[0] == 0xFF || Dp5Status.RAW[0] == 0x00) bDppk100Error = true;
                if (bDppk100Error)
                {
                    Log.Error("Dpp100 Error:1; Dpp100 Version Error!" + ((Dp5Status.RAW[0] == 0xFF) ? "0xFF" : "0x00"));
                }
            }
            else   //未知的PID
            {
                RecB[0] = RecData[3];
                RecB[1] = RecData[4];
                //return false;
            }
            return true;
        }

        /// <summary>
        /// 从配置文件读取配置信息
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string ReadCfgFromFile(string filename)
        {

            if (!System.IO.File.Exists(filename))
            { return strDp5ConfigFile = "BLR=OFF;FLATTOPWITH=0;FIRBLRSPEED=1;ZEROOFFSETVALUE=0;TC_OFFSET_CFG=160;FIRLEN=256;INSPECTLEN=77;PREAMPLOCKOUT=1;ADCTHRESHOLD=200;DIGITALTHRESHOLD=35;FIRTHRESHOLD=100;PILEUPREJECT=ON;ADGAIN=GAIN1;DIGITALGAIN=1.0000;STOPMODE=1;STOPVALUE=1000000;QCPROTECT=4;KEYSERIALNUMBER=FFFFFFFF;"; }
            string strStatus = string.Empty;
            string section = "DP5 Configuration File";
            int size = 255;
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, "BLR", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim()!=string.Empty)
            {
                strStatus += "BLR=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "BLR=ON;";

            GetPrivateProfileString(section, "FLATTOPWITH", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "FLATTOPWITH=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "FLATTOPWITH=0;";

            GetPrivateProfileString(section, "FIRBLRSPEED", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "FIRBLRSPEED=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "FIRBLRSPEED=1;";

            GetPrivateProfileString(section, "ZEROOFFSETVALUE", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "ZEROOFFSETVALUE=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "ZEROOFFSETVALUE=1;";

            GetPrivateProfileString(section, "TC_OFFSET_CFG", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "TC_OFFSET_CFG=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "TC_OFFSET_CFG=160;";

            GetPrivateProfileString(section, "FIRLEN", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "FIRLEN=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "FIRLEN=256;";


            GetPrivateProfileString(section, "INSPECTLEN", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "INSPECTLEN=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "INSPECTLEN=70;";

            GetPrivateProfileString(section, "PREAMPLOCKOUT", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "PREAMPLOCKOUT=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "PREAMPLOCKOUT=1;";

            GetPrivateProfileString(section, "ADCTHRESHOLD", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "ADCTHRESHOLD=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "ADCTHRESHOLD=200;";


            GetPrivateProfileString(section, "DIGITALTHRESHOLD", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "DIGITALTHRESHOLD=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "DIGITALTHRESHOLD=35;";

            GetPrivateProfileString(section, "FIRTHRESHOLD", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "FIRTHRESHOLD=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "FIRTHRESHOLD=100;";


            GetPrivateProfileString(section, "PILEUPREJECT", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "PILEUPREJECT=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "PILEUPREJECT=ON;";


            GetPrivateProfileString(section, "ADGAIN", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "ADGAIN=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "ADGAIN=GAIN1;";


            GetPrivateProfileString(section, "DIGITALGAIN", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "DIGITALGAIN=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "DIGITALGAIN=1.0000;";

            GetPrivateProfileString(section, "STOPMODE", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "STOPMODE=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "STOPMODE=2;";

            GetPrivateProfileString(section, "STOPVALUE", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "STOPVALUE=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "STOPVALUE=1000000;";

            GetPrivateProfileString(section, "QCPROTECT", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "QCPROTECT=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "QCPROTECT= 4;";

            GetPrivateProfileString(section, "KEYSERIALNUMBER", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "KEYSERIALNUMBER=" + temp.ToString().Split(';')[0] + ";";
            }
            else strStatus += "KEYSERIALNUMBER=FFFFFFFF;";
            return strDp5ConfigFile = strStatus;
        }

        public bool ReadCfgsFromFile(ref string strPeakingTime, ref string strGain, ref string strFastLimit, ref string strSlowLimit, ref string strPileUp,
            ref string strFlatTop,ref string strPreAmpLockOut,ref int iFIRBLRSpeed,ref int iZeroOffset,ref int iQCProtect )
        {

            string filePath = System.Windows.Forms.Application.StartupPath + "\\Dpp100.cfg";
            try
            {
                List<string> strcfg = ReadCfgFromFile(filePath).Split(';').ToList();
                string strPeaking = strcfg.Find(w => w.Contains("FIRLEN")).Split('=')[1];
                //strPeakingTime = ((int.Parse(strPeaking)) / 16 * 0.2).ToString("F1") + "uS";
                //修改成型时间2014-05-21
                strPeakingTime = ((int.Parse(strPeaking)) * 0.1).ToString("F1") + "uS";
                strGain = strcfg.Find(w => w.Contains("ADGAIN")).Split('=')[1];
                strFastLimit = strcfg.Find(w => w.Contains("DIGITALTHRESHOLD")).Split('=')[1];
                strSlowLimit = strcfg.Find(w => w.Contains("FIRTHRESHOLD")).Split('=')[1];
                strPileUp = strcfg.Find(w => w.Contains("PILEUPREJECT")).Split('=')[1];
                //新增顶宽时间,复位时间,基线复位速度,ZeroOffsetValue2014-05-21
                strPeaking = strcfg.Find(w => w.Contains("FLATTOPWITH")).Split('=')[1];
                strFlatTop = ((int.Parse(strPeaking) + 1) * 0.2).ToString("F1") + "uS";
                strPeaking = strcfg.Find(w => w.Contains("PREAMPLOCKOUT")).Split('=')[1];
                strPreAmpLockOut = ((int)Math.Pow(2,int.Parse(strPeaking)) * 25.6).ToString("F1") + "uS";
                iFIRBLRSpeed = int.Parse(strcfg.Find(w => w.Contains("FIRBLRSPEED")).Split('=')[1]);
                iZeroOffset = int.Parse(strcfg.Find(w => w.Contains("ZEROOFFSETVALUE")).Split('=')[1]);
                iQCProtect = int.Parse(strcfg.Find(w => w.Contains("QCPROTECT")).Split('=')[1]);

            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool SaveCfgsToFile(string strPeakingTime, string strGain, string strFastLimit, string strSlowLimit, string strPileUp,
             string strFlatTop,string strPreAmpLockOut,int iFIRBLRSpeed,int iZeroOffset,int iQCProtect)
        {

            string filePath = System.Windows.Forms.Application.StartupPath + "\\Dpp100.cfg";
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    ReadCfgFromFile(filePath);
                    SaveCfgToFile(filePath);
                }
                string section = "DP5 Configuration File";
                string str = strPeakingTime.Replace("uS", "");
                //WritePrivateProfileString(section, "FIRLEN", ((int)(double.Parse(str) / 0.2) * 16).ToString() + ";", filePath);
                //修改成型时间2014-05-21
                WritePrivateProfileString(section, "FIRLEN", ((int)(double.Parse(str) / 0.1)).ToString() + ";", filePath);
                WritePrivateProfileString(section, "ADGAIN", strGain+";", filePath); 
                WritePrivateProfileString(section, "DIGITALTHRESHOLD", strFastLimit + ";", filePath);
                WritePrivateProfileString(section, "FIRTHRESHOLD", strSlowLimit + ";", filePath);
                WritePrivateProfileString(section, "PILEUPREJECT", strPileUp + ";", filePath);
                //新增顶宽时间,复位时间,基线复位速度,ZeroOffsetValue2014-05-21
                str = strFlatTop.Replace("uS", "");
                WritePrivateProfileString(section, "FLATTOPWITH", ((int)(double.Parse(str) / 0.2)-1).ToString() + ";", filePath);
                str = strPreAmpLockOut.Replace("uS", "");
                WritePrivateProfileString(section, "PREAMPLOCKOUT", (int)Math.Log((double.Parse(str) / 25.6),2) + ";", filePath);
                WritePrivateProfileString(section, "FIRBLRSPEED", iFIRBLRSpeed + ";", filePath);
                WritePrivateProfileString(section, "ZEROOFFSETVALUE", iZeroOffset + ";", filePath);
                WritePrivateProfileString(section, "QCPROTECT", iQCProtect + ";", filePath);

            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 向配置文件写配置信息
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool SaveCfgToFile(string filename)
        {
            if (strDp5ConfigFile.Trim()==string.Empty)
            {
                return false;
            }
            string[] strElems = strDp5ConfigFile.Split(new char[] { ';' });
            string section = "DP5 Configuration File";
            foreach (string temp in strElems)
            {
                if (temp.Trim() == string.Empty || temp.Trim() == "=") continue;
                string strkey = temp.Split('=')[0];
                string temp1=temp.Replace(strkey + "=", "");
                temp1 = temp1.Replace(";", "");
                string value = temp1+";";
                WritePrivateProfileString(section, strkey, value, filename);
            }
            return true;
        }

        public string GetSpectrumString()
        {

           
            string strSpec = string.Empty;
            for (int i = 0; i < SPECTRUM.Length;i++ )
            {
                strSpec += SPECTRUM[i].ToString()+",";
            }
            //double AccTime = 0;
            //double LocalTime = 0;
            //double SlowCount = 0;
            //double SlowCountRate = 0;
            //StatusStr = GetStatusString(ref AccTime, ref LocalTime, ref SlowCount, ref SlowCountRate);
            return strSpec;
        }

        public string GetStatusString(ref double AccumulationTime, ref double LocalTime,ref double SlowCount,ref double SlowCountRate)
        {
            string StatusStr = string.Empty;
	        StatusStr += "Status:\r\n";
            StatusStr += "AccumulationTime:"+Dp5Status.AccumulationTime+"\r\n";
            StatusStr += "RealTime:" + Dp5Status.RealTime + "\r\n";
            StatusStr += "SlowCountRate:" + Dp5Status.SlowCountRate + "\r\n";
            StatusStr += "FaseCountRate:" + Dp5Status.FastCountRate + "\r\n";
            StatusStr += "FastCount:" + Dp5Status.FastCount + "\r\n";
            StatusStr += "SlowCount:" + Dp5Status.SlowCount + "\r\n";
            StatusStr += "Live Time Clock Percent:" + Dp5Status.LiveTimeClockPercent + "\r\n";
            AccumulationTime = Dp5Status.AccumulationTime;
            LocalTime = Dp5Status.RealTime;
            SlowCount = Dp5Status.SlowCount;
            SlowCountRate = Dp5Status.SlowCountRate;
            return StatusStr;
        }

        public int HasHeader(byte[] RecData,int RecLen)
        {
            int i = 0;
            while (i < RecLen - 1)
            {
                if (RecData[i] == 'A')
                {
                    return i;
                }
            }
            return -1;
        }

        public  int HexCharToInt(char a) 
        {
            if (a >= '0' & a <= '9') return Convert.ToInt16(a)-48;
            switch (a)
            { 
                case 'a':
                case 'A':
                    return 10;
                case 'b':
                case 'B':
                    return 11;
                case 'c':
                case 'C':
                    return 12;
                case 'd':
                case 'D':
                    return 13;
                case 'e':
                case 'E':
                    return 14;
                case 'f':
                case 'F':
                    return 15;
                default:
                    return 0;
            }
        }


        public string BytesToHexChar(byte[] a)
        {
            string str = string.Empty;
            for (int i = 0; i < a.Length; i++)
            {
                int temp=(a[i]>>4)&0x0F;
                str += temp <= 9 ? temp.ToString() : (Convert.ToChar((temp - 10) + 'a')).ToString();
                temp = a[i]& 0x0F;
                str += temp <= 9 ? temp.ToString() : (Convert.ToChar((temp - 10) + 'a')).ToString();
                str += "  ";
            }
            return str;
        }
    }
}
