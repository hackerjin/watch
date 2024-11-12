using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace Skyray.EDX.Common.Component
{
    public class DP5_DP4_FORMAT_STATUS
    {
        public byte[] RAW = new byte[64];
        public ulong SerialNumber;
        public double FastCount;		// LONG is signed :(
        public double SlowCount;
        public byte FPGA;
        public byte Firmware;
        public byte Build;
        public double AccumulationTime;
        public double RealTime;
        public double HV;
        public double DET_TEMP;
        public double DP5_TEMP;
        public bool PX4;
        public bool AFAST_LOCKED;
        public bool MCA_EN;
        public bool PRECNT_REACHED;
        public bool PresetRtDone;
        public bool SUPPLIES_ON;
        public bool SCOPE_DR;
        public bool DP5_CONFIGURED;
        public double GP_COUNTER;
        public bool AOFFSET_LOCKED;
        public bool MCS_DONE;
        public bool RAM_TEST_RUN;
        public bool RAM_TEST_ERROR;
        public double DCAL;
        public byte PZCORR;				// or single?
        public byte UC_TEMP_OFFSET;
        public double AN_IN;
        public double VREF_IN;
        public ulong PC5_SN;
        public bool PC5_PRESENT;
        public bool PC5_HV_POL;
        public bool PC5_8_5V;
        public double LIVETIME;
        public byte BOOT_FLAG_LSB;
        public byte BOOT_FLAG_MSB;
        public double BOOT_HV;
        public double BOOT_TEC;
        public double BOOT_INPOFFSET;    // or long?
        public double ADC_GAIN_CAL;
        public byte ADC_OFFSET_CAL;
        public long SPECTRUM_OFFSET;     // or single?
        public bool b80MHzMode;
        public bool bFPGAAutoClock;
        public byte DEVICE_ID;
        public bool ReBootFlag;
        public byte PX5_OPTIONS;
        public bool Process_Status()
        {
            FastCount = (double)RAW[0] + (double)RAW[1] * 256 + (double)RAW[2] * 65536 + (double)RAW[3] * 16777216;
            SlowCount = (double)RAW[4] + (double)RAW[5] * 256 + (double)RAW[6] * 65536 + (double)RAW[7] * 16777216;
            GP_COUNTER = (double)RAW[8] + (double)RAW[9] * 256 + (double)RAW[10] * 65536 + (double)RAW[11] * 16777216;
            AccumulationTime = (float)RAW[12] * 0.001 + (float)(RAW[13] + (float)RAW[14] * 256.0 + (float)RAW[15] * 65536.0) * 0.1;
            RealTime = ((double)RAW[20] + ((double)RAW[21] * 256.0) + ((double)RAW[22] * 65536.0) + ((double)RAW[23] * 16777216.0)) * 0.001;

            Firmware = RAW[24];
            FPGA = RAW[25];

            if (Firmware > 0x65)
            {
                Build = (byte)(RAW[37] & 0xF);		//Build # added in FW6.06
            }
            else
            {
                Build = 0;
            }

            if (RAW[29] < 128)
            {
                SerialNumber = (ulong)RAW[26] + (ulong)RAW[27] * 256 + (ulong)RAW[28] * 65536 + (ulong)RAW[29] * 16777216;
            }
            else
            {
                SerialNumber = 0;
            }

            // m_DP5_Status->HV = (double)(m_DP5_Status->RAW[31] + (m_DP5_Status->RAW[30] & 15) * 256) * 0.5;					// 0.5V/count

            if (RAW[30] < 128)
            {    // not negative
                HV = ((double)RAW[31] + ((double)RAW[30] * 256.0)) * 0.5;  // 0.5V/count
            }
            else
            {
                HV = (((double)RAW[31] + ((double)RAW[30] * 256)) - 65536.0) * 0.5; // 0.5V/count
            }

            DET_TEMP = (double)((RAW[33]) + (RAW[32] & 15) * 256) * 0.1; // - 273.16;		// 0.1K/count
            DP5_TEMP = RAW[34] - ((RAW[34] & 128) * 2);

            PresetRtDone = ((RAW[35] & 128) == 128);
            AFAST_LOCKED = ((RAW[35] & 64) == 64);
            MCA_EN = ((RAW[35] & 32) == 32);
            PRECNT_REACHED = ((RAW[35] & 16) == 16);
            SCOPE_DR = ((RAW[35] & 4) == 4);
            DP5_CONFIGURED = ((RAW[35] & 2) == 2);

            AOFFSET_LOCKED = ((RAW[36] & 128) == 0);  // 0=locked, 1=searching
            MCS_DONE = ((RAW[36] & 64) == 64);

            b80MHzMode = ((RAW[36] & 2) == 2);
            bFPGAAutoClock = ((RAW[36] & 1) == 1);

            PC5_PRESENT = ((RAW[38] & 128) == 128);
            if (PC5_PRESENT)
            {
                PC5_HV_POL = ((RAW[43] & 64) == 64);
                PC5_8_5V = ((RAW[43] & 32) == 32);
            }
            else
            {
                PC5_HV_POL = false;
                PC5_8_5V = false;
            }

            DEVICE_ID = RAW[39];

            PX5_OPTIONS = 0;
            if (DEVICE_ID == 1)
            {		// test for px5 options
                // HPGe HVPS installed
                PX5_OPTIONS = Convert.ToByte((RAW[42] & 1) == 1);
            }

            if (Firmware >= 0x65)
            {		// reboot flag added FW6.05
                if ((RAW[36] & 32) == 32)
                {
                    ReBootFlag = true;
                }
                else
                {
                    ReBootFlag = false;
                }
            }
            else
            {
                ReBootFlag = false;
            }
            return true;
        }
    } ;
    class DP5Protocol
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
        DP5_DP4_FORMAT_STATUS Dp5Status = new DP5_DP4_FORMAT_STATUS();
        //Packet_In PIN;
        public string strDp5ConfigFile;
        public string strDp5ConfigValue;
        public DP5Protocol(int specLength)
        {
            SPECTRUM = new long[specLength];
            iSpecLength = specLength;
        }


        private byte[] GetCommandByProtocol(int iType, string strCommand)
        {
            char[] charsCommand = strCommand.ToCharArray();
            byte[] byteTemp = new byte[charsCommand.Length + 8];
            byteTemp[0] = 0xF5;
            byteTemp[1] = 0xFA;
            switch (iType)
            {
                case 0://启动Mca
                    byteTemp[2] = 0xF0;
                    byteTemp[3] = 0x02;
                    byteTemp[4] = 0;
                    byteTemp[5] = 0;
                    break;
                case 1://暂停Mca
                    byteTemp[2] = 0xF0;
                    byteTemp[3] = 0x03;
                    byteTemp[4] = 0;
                    byteTemp[5] = 0;
                    break;
                case 2://获取谱数据和状态
                    byteTemp[2] = 0x02;
                    byteTemp[3] = 0x03;
                    byteTemp[4] = 0;
                    byteTemp[5] = 0;
                    break;
                case 3://清除谱数据和状态
                    byteTemp[2] = 0x02;
                    byteTemp[3] = 0x04;
                    byteTemp[4] = 0;
                    byteTemp[5] = 0;
                    break;
                case 4://设置config
                    byteTemp[2] = 0x20;
                    byteTemp[3] = 0x02;
                    byteTemp[4] = (byte)((charsCommand.Length & 0xFF00) / 256);
                    byteTemp[5] = (byte)(charsCommand.Length & 0xFF);
                    //Array.Copy(charsCommand, 0, byteTemp, 6, charsCommand.Length);
                    break;
                case 5://读取config
                    byteTemp[2] = 0x20;
                    byteTemp[3] = 0x03;
                    byteTemp[4] = (byte)((charsCommand.Length & 0xFF00) / 256);
                    byteTemp[5] = (byte)(charsCommand.Length & 0xFF);
                    //Array.Copy(charsCommand, 0, byteTemp, 6, charsCommand.Length);
                    break;
                case 6://读取Status
                    byteTemp[2] = 0x01;
                    byteTemp[3] = 0x01;
                    byteTemp[4] = 0;
                    byteTemp[5] = 0;
                    break;

            }
            for (int i = 0; i < charsCommand.Length;i++ )
            {
                byteTemp[i + 6] = (byte)charsCommand[i];
            }
            int Csum = 0;
            for (int i = 0; i < byteTemp.Length - 2; i++)
            {
                Csum += byteTemp[i];
            }
            Csum = (Csum ^ 0xFFFF) + 1;
            byteTemp[byteTemp.Length - 2] = (byte)((Csum & 0xFF00) / 256);
            byteTemp[byteTemp.Length - 1] = (byte)(Csum & 0x00FF);
            return byteTemp;
        }

        /// <summary>
        /// 取Status的Byte数组
        /// </summary>
        public byte[] GetStatusCommand
        {
            get
            {
                return GetCommandByProtocol(6, string.Empty);
            }
        }
        /// <summary>
        /// 启动MCA命令的Byte数组
        /// </summary>
        public byte[] EnableMCACommand
        {
            get
            {
                return GetCommandByProtocol(0, string.Empty);
            }
        }
        /// <summary>
        /// 停止MCA命令的Byte数组
        /// </summary>
        public byte[] DisabelMCACommand
        {
            get
            {
                return GetCommandByProtocol(1, string.Empty);
            }
        }
        /// <summary>
        /// 获取谱和状态命令的Byte数组
        /// </summary>
        public byte[] GetSpectrumCommand
        {
            get
            {
                return GetCommandByProtocol(2, string.Empty);
            }
        }
        /// <summary>
        /// 清空谱和状态的命令的Byte数组
        /// </summary>
        public byte[] ClearSpectrumCommand
        {
            get
            {
                return GetCommandByProtocol(3, string.Empty);
            }
        }
        /// <summary>
        /// 设置Config命令的Byte数组
        /// </summary>
        public byte[] SetConfigCommand
        {
            get
            {
                return GetCommandByProtocol(4, strDp5ConfigFile);
            }
        }
        /// <summary>
        /// 获取Config命令的Byte数组
        /// </summary>
        public byte[] GetConfigCommand
        {
            get
            {
                string strTemp = "RESC=?;CLCK=?;TPEA=?;GAIF=?;GAIN=?;RESL=?;TFLA=?;TPFA=?;PURE=?;RTDE=?;MCAS=?;MCAC=?;SOFF=?;AINP=?;INOF=?;GAIA=?;CUSP=?;PDMD=?;THSL=?;TLLD=?;THFA=?;DACO=?;DACF=?;RTDS=?;RTDT=?;BLRM=?;BLRD=?;BLRU=?;GATE=?;AUO1=?;PRET=?;PRER=?;PREC=?;PRCL=?;PRCH=?;HVSE=?;TECS=?;PAPS=?;SCOE=?;SCOT=?;SCOG=?;MCSL=?;MCSH=?;MCST=?;AUO2=?;TPMO=?;GPED=?;GPIN=?;GPME=?;GPGA=?;GPMC=?;MCAE=?;BOOT=?;";
                return GetCommandByProtocol(5, strTemp);
            }
        }

        /// <summary>
        /// 处理接收的byte数组
        /// </summary>
        /// <param name="RecData"></param>
        /// <returns></returns>
        public bool ProcessExData(byte[] RecData)
        {
            if (!(RecData[0] == 0xF5) || !(RecData[1] == 0xFA))
            {
                return false;
            }
            int dataLen = RecData[4] * 256 + RecData[5];
            if (dataLen > RecData.Length-8)
            {
                return false;
            }
            int PID1 = RecData[2];
            int PID2 = RecData[3];
            int Csum = 0;
            for (int i = 0; i <dataLen + 6; i++)
            {
                Csum += RecData[i];
            }
            Csum += 256 * RecData[dataLen + 6] + RecData[dataLen + 7];
            if ((Csum & 0xFFFF) != 0)//和检验失败
            {
                return false;
            };
            if (PID1 == 0x81 && PID2>=0x1&& PID2<= 0x0C)//谱和状态解析
            {
                int DataLen = RecData[4] * 256 + RecData[5];
                int specLen = (short)(256 * Math.Pow(2.0, ((PID2 - 1) & 14) / 2));
                
                if (dataLen < 3 * specLen)
                {
                    return false;
                }
                SPECTRUM = new long[specLen];
                for (int i = 0; i < specLen ; i++)//取谱
                {
                    SPECTRUM[i] = (long)(RecData[i * 3 + 6] + RecData[i * 3 + 1 + 6] * 256) + (long)RecData[i * 3 + 2 + 6] * 65536;
                }
                if ((PID2 & 0x01) == 0)//取状态
                {
                    if (dataLen < 3 * specLen + Dp5Status.RAW.Length)
                    {
                        return false;
                    }
                    for (int j = 0; j < Dp5Status.RAW.Length; j++)
                    {
                        Dp5Status.RAW[j] = RecData[j + specLen * 3 + 6];
                    }
                    Dp5Status.Process_Status();
                }
            }
            else if (PID1==0x80&&PID2==0x01)//接受到状态
            {
                int DataLen = RecData[4] * 256 + RecData[5];
                for (int j = 0; j < Dp5Status.RAW.Length; j++)
                {
                    Dp5Status.RAW[j] = RecData[j + 6];
                }
                Dp5Status.Process_Status();
            }
            else if (PID1 == 0x82 && PID2 == 0x07)//Config解析
            {
                strDp5ConfigValue = string.Empty;
                int configLen = RecData[4] * 256 + RecData[5];
                string cstrCh;
                for (int i = 0; i < configLen; i++)
                {
                    cstrCh = Convert.ToString(Convert.ToChar(RecData[i + 6]));
                    strDp5ConfigValue += cstrCh;
                }
            }
            else if (PID1==0xFF)
            {
               string cstrPID2;
               bool IsCommandTrue = PID2ToString("ACK", PID2, out cstrPID2);
               if (!IsCommandTrue)
               {
                   //throw new Exception(cstrPID2);
                   Console.WriteLine(cstrPID2);
                   return false;
               }
            }
            //else if (PID2==2)  //未知的PID
            //{

            //    return false;
            //}
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
            { return strDp5ConfigFile = ""; }
            string strStatus = string.Empty;
            string section = "DP5 Configuration File";
            int size = 255;
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, "RESC", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim()!=string.Empty)
            {
                strStatus += "RESC=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "CLCK", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "CLCK=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "TPEA", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "TPEA=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "GAIF", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "GAIF=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "GAIN", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "GAIN=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "RESL", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "RESL=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "TFLA", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "TFLA=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "TPFA", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "TPFA=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "PURE", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "PURE=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "RTDE", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "RTDE=" + temp.ToString().Split(';')[0] + ";";
            }



            GetPrivateProfileString(section, "MCAS", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "MCAS=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "MCAC", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                iSpecLength = Convert.ToInt32(temp.ToString().Split(';')[0]);
                strStatus += "MCAC=" + iSpecLength .ToString()+ ";";
            }
            //strStatus += "MCAC=" + iSpecLength.ToString();//修改谱长度
            GetPrivateProfileString(section, "SOFF", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "SOFF=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "AINP", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "AINP=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "INOF", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "INOF=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "GAIA", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "GAIA=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "CUSP", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "CUSP=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "PDMD", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "PDMD=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "THSL", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "THSL=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "TLLD", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "TLLD=" + temp.ToString().Split(';')[0] + ";";
            }



            GetPrivateProfileString(section, "THFA", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "THFA=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "DACO", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "DACO=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "DACF", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "DACF=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "RTDS", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "RTDS=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "RTDT", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "RTDT=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "BLRM", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "BLRM=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "BLRD", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "BLRD=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "BLRU", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "BLRU=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "GATE", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "GATE=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "AUO1", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "AUO1=" + temp.ToString().Split(';')[0] + ";";
            }



            GetPrivateProfileString(section, "PRET", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "PRET=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "PRER", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "PRER=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "PREC", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "PREC=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "PRCL", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "PRCL=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "PRCH", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "PRCH=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "HVSE", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "HVSE=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "TECS", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "TECS=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "PAPS", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "PAPS=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "SCOE", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "SCOE=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "SCOT", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "SCOT=" + temp.ToString().Split(';')[0] + ";";
            }



            GetPrivateProfileString(section, "SCOG", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "SCOG=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "MCSL", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "MCSL=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "MCSH", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "MCSH=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "MCST", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "MCST=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "AUO2", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "AUO2=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "TPMO", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "TPMO=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "GPED", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "GPED=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "GPIN", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "GPIN=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "GPME", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "GPME=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "GPGA", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "GPGA=" + temp.ToString().Split(';')[0] + ";";
            }


            GetPrivateProfileString(section, "GPMC", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "GPMC=" + temp.ToString().Split(';')[0] + ";";
            }
            GetPrivateProfileString(section, "MCAE", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "MCAE=" + temp.ToString().Split(';')[0] + ";";
            }
            //非DP5
            //GetPrivateProfileString(section, "VOLU", "?", temp, size, filename);
            //if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            //{
            //    strStatus += "VOLU=" + temp.ToString().Split(';')[0] + ";";
            //}
            //GetPrivateProfileString(section, "CON1", "?", temp, size, filename);
            //if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            //{
            //    strStatus += "CON1=" + temp.ToString().Split(';')[0] + ";";
            //}
            //GetPrivateProfileString(section, "CON2", "?", temp, size, filename);
            //if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            //{
            //    strStatus += "CON2=" + temp.ToString().Split(';')[0] + ";";
            //}
            GetPrivateProfileString(section, "BOOT", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                strStatus += "BOOT=" + temp.ToString().Split(';')[0] + ";";
            }
            return strDp5ConfigFile = strStatus;
        }

        /// <summary>
        /// 加载DP5参数
        /// </summary>
        public void LoadDP5CfgOther(string filename, ref string intPurOn, ref int intFastThreshold, ref double intPeaktime, ref double intFlattop, ref string hv)
        {

            if (!System.IO.File.Exists(filename))
                return;
            string strStatus = string.Empty;
            string section = "DP5 Configuration File";
            int size = 255;
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, "TPEA", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                intPeaktime = double.Parse(temp.ToString().Split(';')[0]);
            }

            GetPrivateProfileString(section, "PURE", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                intPurOn =  temp.ToString().Split(';')[0];
            }

            GetPrivateProfileString(section, "TFLA", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                intFlattop = double.Parse(temp.ToString().Split(';')[0]);
            }
 

            GetPrivateProfileString(section, "THFA", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                intFastThreshold = (int)double.Parse(temp.ToString().Split(';')[0]);
            }


            GetPrivateProfileString(section, "HVSE", "?", temp, size, filename);
            if (!temp.ToString().Equals("?") && temp.ToString().Split(';')[0].Trim() != string.Empty)
            {
                hv = temp.ToString().Split(';')[0];
            }

        }

        public void SaveDP5CfgOther(string filename, string purOn, int fastThreshold, string peakingTime, string flatTop, string intVlotage)
        {
            string[] sections = new string[] { "DP5 Configuration File" };//, "DP5 Configuration Values"
            foreach (string section in sections)
            {
                WritePrivateProfileString(section, "PURE", purOn, filename);
                System.Threading.Thread.Sleep(10);
                WritePrivateProfileString(section, "THFA", fastThreshold.ToString(), filename);
                System.Threading.Thread.Sleep(10);
                WritePrivateProfileString(section, "TPEA", peakingTime, filename);
                System.Threading.Thread.Sleep(10);
                WritePrivateProfileString(section, "TFLA", flatTop, filename);
                System.Threading.Thread.Sleep(10);
                WritePrivateProfileString(section, "HVSE", intVlotage, filename);
                System.Threading.Thread.Sleep(10);
            }
            //SaveCfgToFile(filename);
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
            
                string strkey = temp.Split('=')[0];
                temp.Replace(strkey + "=", "");
                temp.Replace(";", "");
                string value = temp;
                WritePrivateProfileString(section, strkey, value, filename);
            }
            if (strDp5ConfigValue.Trim() == string.Empty)
            strElems = strDp5ConfigFile.Split(new char[] { ';' });
            section = "DP5 Configuration Value";
            foreach (string temp in strElems)
            {
            
                string strkey = temp.Split('=')[0];
                temp.Replace(strkey + "=", "");
                temp.Replace(";", "");
                string value = temp;
                WritePrivateProfileString(section, strkey, value, filename);
            }
            return true;
        }

        public bool PID2ToString(string strPacketSource, int PID2, out string cstrPID2)
        {
            cstrPID2=string.Empty;
            switch (PID2)
            {
                case 0:  
                    cstrPID2 = "";
                    return true;
                case 1:
                    cstrPID2 = strPacketSource + ": Sync Error\t";
                    break;
                case 2:
                    cstrPID2 = strPacketSource + ": PID Error\t";
                    break;
                case 3:
                    cstrPID2 = strPacketSource + ": Length Error\t";
                    break;
                case 4:
                    cstrPID2 = strPacketSource + ": Checksum Error\t";
                    break;
                case 5:	
                    cstrPID2 = strPacketSource + ": Bad Parameter\t";
                    break;
                case 6:
                    cstrPID2 = strPacketSource + ": Bad HEX Record\t";
                    break;
                case 7:
                    cstrPID2 = strPacketSource + ": Unrecognized Command\t";
                    break;
                case 8:
                    cstrPID2 = strPacketSource + ": FPGA not initialized\t";
                    break;
                case 9:
                    cstrPID2 = strPacketSource + ": CP2201 not found\t";
                    break;
                case 10:
                    cstrPID2 = strPacketSource + ": No scope data\t";
                    break;
                case 11:
                    cstrPID2 = strPacketSource + ": PC5 not present\t";
                    break;
                case 12:
                    cstrPID2 = strPacketSource + ": Ethernet sharing request\t";
                    break;
                case 13:
                    cstrPID2 = strPacketSource + ": Ethernet sharing request\t";
                    break;
                default:
                    cstrPID2 = strPacketSource + ": Unrecognized Error\t";
                    break;
            }
            return false;
        }

        public string GetSpectrumString(out string StatusStr)
        {

           
            string strSpec = string.Empty;
            for (int i = 0; i < SPECTRUM.Length;i++ )
            {
                strSpec += SPECTRUM[i].ToString();
            }
            double usedTime = 0;
            double realTime = 0;
            int serialNumber = 0;
            double fastCount = 0;
            double slowCount = 0;
            StatusStr = GetStatusString(ref usedTime, ref realTime, ref serialNumber, ref fastCount, ref slowCount);

            return strSpec;
        }
        
        public  string GetStatusString(ref double usedTime,ref double realTime,ref int serialNumber,ref double fastCount,ref double slowCount)
        {
            string StatusStr = string.Empty;
	        StatusStr += "Status:;";
            StatusStr += "AccumulationTime:"+Dp5Status.AccumulationTime+";";
            StatusStr += "RealTime:" + Dp5Status.RealTime + ";";
            StatusStr += "SerialNumber:" + Dp5Status.SerialNumber + ";";
            StatusStr += "FastCount:" + Dp5Status.FastCount + ";";
            StatusStr += "SlowCount:" + Dp5Status.SlowCount + ";";
            usedTime = Dp5Status.AccumulationTime;
            realTime = Dp5Status.RealTime;
            serialNumber = (int)Dp5Status.SerialNumber;
            fastCount = Dp5Status.FastCount;
            slowCount = Dp5Status.SlowCount;
            return StatusStr;
        } 
        public int HasHeader(byte[] RecData,int RecLen)
        {
            int i=0;
            while (i < RecLen - 1)
            {
                if ((RecData[i] == 0xF5) &&(RecData[i+1] == 0xFA))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
    }
}
