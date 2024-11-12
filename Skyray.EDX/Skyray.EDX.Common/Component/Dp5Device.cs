using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common.Component
{
    public class Dp5Device : Dp5Interface
    {
        private IntPtr _objptr;
        private _DPP_STATUS _status;

        public _DPP_STATUS Dp5Status
        {
            get
            {
                return _status;
            }
        }
        private _DPP_CONFIG_SETTINGS _cfgSet;
        private int _numDev = 0;
        private byte _dppDevice = 0x4;

        public Dp5Device()
        {
            Dp5Versions = Dp5Version.Dp5_CommonUsb;
            _status = new _DPP_STATUS();
            _cfgSet = new _DPP_CONFIG_SETTINGS();
            _cfgSet.SCA = new int[8];
        }

        /// <summary>
        /// 打开设备
        /// </summary>
        public override void OpenDevice()
        {
            _objptr = DppApi.OpenDppApi();
            _numDev = DppApi.OpenUSBDevice(_objptr);
        }

        public override void CloseDevice()
        {
            DppApi.CloseUSBDevice(_objptr);
            DppApi.CloseDppApi(_objptr);
            _objptr = IntPtr.Zero;
        }

        public override bool ConnectDevice(string Ip, string Port)
        {
            return  true;
        }
        public override bool DisConnectDevice()
        {
            return true;
        }
        public override bool IsConnected()
        {
            return _objptr!=null?DppApi.MonitorUSBDppDevices(_objptr)>0:false;
        }

        public override bool LoadDP5CfgOther(ref int intPurOn, ref int intFastThreshold, ref string strPeaktime, ref string strFlattop, ref int hv)
        {
            int intPeaktime = 0;
            int intFlattop = 0;
            int intDecimation = 0;
            int inthv = 0;
            string path=GetCfgFilePath();
            if(path==string.Empty) return false;
            LoadDP5CfgOther(path, ref intPurOn, ref intFastThreshold, ref intPeaktime, ref intFlattop, ref intDecimation, ref inthv);
            List<float> PeakingTimes = GetPeakingTimes();
            switch (intDecimation)
            { 
                case 1:
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    intPeaktime += 8 + (intDecimation - 3) * 4;
                    break;
            }
            strPeaktime=string.Format("{0:0.0}", PeakingTimes[intPeaktime-1]);
            List<float> flatTops=GetFlatTops(PeakingTimes[intPeaktime-1]);
            strFlattop = string.Format("{0:0.00}", flatTops[intFlattop]);
            hv = inthv;
            return true;
        }


        public override bool SaveDP5CfgOther(int intPurOn, int intFastThreshold, string strPeaktime, string strFlattop, int intVoltage)
        {
            string path = GetCfgFilePath();
            if (path == string.Empty) return false;
            List<float> PeakingTimes = GetPeakingTimes();
            int peaktime = 0;
            for (int i = 0; i < PeakingTimes.Count; i++)
            {
                if (strPeaktime.Equals(string.Format("{0:0.0}", PeakingTimes[i])))
                {
                    peaktime = i+1;//从1开始排序
                    break;
                }
            }
            int flattop=0;
            List<float> flattops=GetFlatTops(PeakingTimes[peaktime-1]);
            for (int i = 0; i < flattops.Count; i++)
            {
                if (strFlattop.Equals(string.Format("{0:0.00}", flattops[i])))
                {
                    flattop = i;//从0开始排序
                    break;
                }
            }
            int decimation =0 ;
            if (peaktime <= 8)
            {
                decimation = 1;
            }
            else if(peaktime<=12)
            {
                peaktime -= 4;//从5开始排序
                decimation = 2;
            }
            else if (peaktime <= 16)
            {
                peaktime -= 8;//从5开始排序
                decimation = 3;
            }
            else if (peaktime <= 20)
            {
                peaktime -= 12;//从5开始排序
                decimation = 4;
            }
            else if (peaktime <= 24)
            {
                peaktime -= 16;//从5开始排序
                decimation = 5;
            }
            return SaveDP5CfgOther(path,intPurOn,intFastThreshold,peaktime,flattop,decimation,intVoltage);
        }


        public override bool LoadDP5Cfg()
        {
            string filepath = GetCfgFilePath();
            return LoadDP5Cfg(filepath);
        }


        public override bool ChangeCfgFineGain(string FineGain, int SpecLength, int PreTime)
        {
            int fineGain=(int)Math.Abs(Convert.ToSingle(FineGain));
            SetGainAndTime(fineGain, PreTime, SpecLength);
            return true;
        }

        public override bool GetData(int[] data, ref bool IsSDDEnabled, ref double UsedTime, ref double FastCount, ref double SlowCount)
        {
            int numDev = DppApi.MonitorUSBDppDevices(_objptr);
            if (numDev <= 0)
            {
                IsSDDEnabled = false;
                return false;
            }
            DppApi.GetStatusStruct(_objptr, true, ref _status);
            if (Dp5Status.SerialNumber < 1 || !Convert.ToBoolean(Dp5Status.SwConfigRcvd))
                return IsSDDEnabled = false;
            if (!Convert.ToBoolean(Dp5Status.StatMcaEnabled)) 
            {
                PauseDppDate(false);
                return IsSDDEnabled = false;
            }
            FastCount = Dp5Status.FastCount;
            SlowCount = Dp5Status.SlowCount;
            DppApi.GetDppData(_objptr, data);
            int[] statu = new int[32];

            DppApi.GetStatusBuffer(_objptr, true, statu);
            UsedTime = (double)statu[11] / (double)1000;
            return true;
        }

         public override bool PauseDppDate(bool IsPause)
        {
            DppApi.PauseDppData(_objptr, IsPause);
            return true;
        }

         public override bool ClearDppData()
        {
            DppApi.ClearDppData(_objptr, true);
            return true;
        }

        /// <summary>
        /// 加载DP5参数
        /// </summary>
         private bool LoadDP5Cfg(string strCfgFilename)
        {
            if (_numDev > 0)
            {
                DppApi.GetStatusStruct(_objptr, true, ref _status);
                if (_status.SerialNumber < 1)
                {
                    return false;
                }
                else
                {
                    _dppDevice = _status.StatDevInd;
                    int indDPP5 = 0;
                    if (_status.Firmware >= 5.0)
                    {
                        indDPP5 = 1;
                    }
                    _dppDevice = (byte)((indDPP5 * 2) + _status.StatDevInd * 1 + 1);
                    bool Dpp80MHzMode = Convert.ToBoolean(DppApi.Get80MHzMode(_objptr));
                    DppApi.SetFPGAClockDefault(_objptr, Dpp80MHzMode, _dppDevice);
                    DppApi.GetConfigFromFile(_objptr, strCfgFilename, strCfgFilename.Length, _dppDevice);
                    DppApi.SendConfigToDpp(_objptr);
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 加载DP5参数
        /// </summary>
        private bool LoadDP5CfgOther(string strCfgFilename, ref int intPurOn, ref int intFastThreshold, ref int intPeaktime, ref int intFlattop, ref int intDecimation, ref int hv)
        {
            if (System.IO.File.Exists(strCfgFilename))
            {
                _objptr = DppApi.OpenDppApi();
                _numDev = DppApi.OpenUSBDevice(_objptr);
                DppApi.GetStatusStruct(_objptr, true, ref _status);
                //if (_status.SerialNumber < 1)
                //{
                //    return false;
                //}
                //else
                //{
                _dppDevice = _status.StatDevInd;
                int indDPP5 = 0;
                if (_status.Firmware >= 5.0)
                {
                    indDPP5 = 1;
                }
                _dppDevice = (byte)((indDPP5 * 2) + _status.StatDevInd * 1 + 1);
                bool Dpp80MHzMode = Convert.ToBoolean(DppApi.Get80MHzMode(_objptr));
                DppApi.SetFPGAClockDefault(_objptr, Dpp80MHzMode, _dppDevice);
                DppApi.GetConfigFromFile(_objptr, strCfgFilename, strCfgFilename.Length, _dppDevice);
                //DppApi.SendConfigToDpp(_objptr);
                //DppApi.GetConfigFromDpp(_objptr);
                DppApi.GetTempConfigSettings(_objptr, ref _cfgSet, true);
                intPurOn = _cfgSet.PUREnable;
                intFastThreshold = _cfgSet.FastThreshold;
                intPeaktime = _cfgSet.TimeToPeak;
                intFlattop = _cfgSet.FlatTop;
                intDecimation = _cfgSet.Decimation;
                hv = _cfgSet.HV;
                DppApi.CloseUSBDevice(_objptr);
                DppApi.CloseDppApi(_objptr);
                _objptr = IntPtr.Zero;
                return true;

                //}
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 加载DP5参数
        /// </summary>
        private bool SaveDP5CfgOther(string strCfgFilename, int intPurOn, int intFastThreshold, int intPeaktime, int intFlattop, int intDecimation, int intVoltage)
        {
            if (System.IO.File.Exists(strCfgFilename))
            {
                //_objptr = DppApi.OpenDppApi();
                //_numDev = DppApi.OpenUSBDevice(_objptr);
                DppApi.GetStatusStruct(_objptr, true, ref _status);
                //if (_status.SerialNumber < 1)
                //{
                //    return false;
                //}
                //else
                //{
                _dppDevice = _status.StatDevInd;
                int indDPP5 = 0;
                if (_status.Firmware >= 5.0)
                {
                    indDPP5 = 1;
                }
                _dppDevice = (byte)((indDPP5 * 2) + _status.StatDevInd * 1 + 1);
                bool Dpp80MHzMode = Convert.ToBoolean(DppApi.Get80MHzMode(_objptr));
                DppApi.SetFPGAClockDefault(_objptr, Dpp80MHzMode, _dppDevice);
                DppApi.GetConfigFromFile(_objptr, strCfgFilename, strCfgFilename.Length, _dppDevice);
                //DppApi.SendConfigToDpp(_objptr);
                //DppApi.GetConfigFromDpp(_objptr);
                if (_cfgSet.SCA[1] != 0)
                { Console.WriteLine("1"); }
                DppApi.GetTempConfigSettings(_objptr, ref _cfgSet, true);
                if (_cfgSet.SCA[1] != 0)
                { Console.WriteLine("2"); }
                _cfgSet.PUREnable = (byte)intPurOn;
                _cfgSet.FastThreshold = (byte)intFastThreshold;
                _cfgSet.TimeToPeak = (byte)intPeaktime;
                _cfgSet.FlatTop = (byte)intFlattop;
                _cfgSet.Decimation = (byte)intDecimation;
                _cfgSet.HV = intVoltage;
                DppApi.SetTempConfigSettings(_objptr, ref _cfgSet, true);
                if (_cfgSet.SCA[1] != 0)
                { Console.WriteLine("3"); }
                //DppApi.SendConfigToDpp(_objptr);
                //string ss = "E:\\dp5.cfg";
                //DppApi.SaveConfigToFile(_objptr, ss, ss.Length);
                DppApi.SaveConfigToFile(_objptr, strCfgFilename, strCfgFilename.Length);
                DppApi.CloseUSBDevice(_objptr);
                DppApi.CloseDppApi(_objptr);
                _objptr = IntPtr.Zero;
                return true;
                //}
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 设置精调和测量时间
        /// </summary>
        /// <param name="fineGain"></param>
        /// <param name="preTime"></param>
        private void SetGainAndTime(int fineGain, int preTime,int SpecLength)
        {
            DppApi.GetTempConfigSettings(_objptr, ref _cfgSet, true);
            DppApi.SetFPGAClockDefault(_objptr, true, _dppDevice);
            _cfgSet.CoarseGain = 13;
            _cfgSet.FineGain = fineGain;
            _cfgSet.PresetTime = preTime * 10;
            switch (SpecLength)
            {
                case 512:
                    _cfgSet.MCAChannels = 1;
                    break;
                case 1024:
                    _cfgSet.MCAChannels = 2;
                    break;
                case 4096:
                    _cfgSet.MCAChannels = 4;
                    break;
                case 2048:
                    _cfgSet.MCAChannels = 3;
                    break;
            }
            DppApi.SetTempConfigSettings(_objptr, ref _cfgSet, true);
            DppApi.SendConfigToDpp(_objptr);//发送参数至设备
        }

        /// <summary>
        /// 测量前操作，必须先执行IsDPRead为true
        /// </summary>
        /// <param name="fineGain"></param>
        /// <param name="preTime"></param>
        //public void SetBeforeTesting(int fineGain, int preTime,int SpecLength)
        //{
        //    DppApi.PauseDppData(_objptr, true);
        //    DppApi.ClearDppData(_objptr,true);
        //    DppApi.GetConfigFromDpp(_objptr);
        //    SetGainAndTime(fineGain, preTime,SpecLength);
        //}

        //public void GetStatusBuffer(int[] stats)
        //{
        //    DppApi.GetStatusBuffer(_objptr, true, stats);
        //}

        public int MonitorDevice()
        {
            return DppApi.MonitorUSBDppDevices(_objptr);
        }

        //public void GetCurrentStatus()
        //{
        //    DppApi.GetStatusStruct(_objptr, true, ref _status);
        //}

        //public void GetDatas(int[] dataBuffers)
        //{
        //    DppApi.GetDppData(_objptr, dataBuffers);
        //}

        //public void PauseData(bool b)
        //{
        //    DppApi.PauseDppData(_objptr, b);
        //}

    }
}