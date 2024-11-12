using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common.Component
{
    public abstract class Dp5Interface
    {
        public Dp5Version _Dp5Versions;
        /// <summary>
        /// Dp5版本
        /// </summary>
        public Dp5Version Dp5Versions
        {
            get { return _Dp5Versions; }
            set {
                _Dp5Versions = value;
            }
        }


        public List<string> GetDp5HV()
        {
            List<string> lstHV=new List<string>();
            switch (Dp5Versions)
            {

                case Dp5Version.Dp5_FastNet:
                case Dp5Version.Dp5_FastUsb:
                    lstHV.Add("-115");
                    lstHV.Add("-130");
                    break;
                case Dp5Version.Dp5_CommonUsb:
                default:
                    lstHV.Add("134");
                    lstHV.Add("157");
                    lstHV.Add("245");
                    lstHV.Add("354");
                    break;
            }
            return lstHV;
        }

        public  string GetCfgFilePath()
        {
            switch (Dp5Versions)
            { 
                case Dp5Version.Dp5_FastNet:
                case Dp5Version.Dp5_FastUsb:
                     return AppDomain.CurrentDomain.BaseDirectory + "\\dp5net.cfg";
                case Dp5Version.Dp5_CommonUsb:
                default:
                    return AppDomain.CurrentDomain.BaseDirectory + "\\dp5.cfg";
            }
        }

        protected List<float> GetPeakingTimes()//单位us
        {
            List<float> Peakings = new List<float>();
            float temp = 0.1f;
            if (Dp5Versions == Dp5Version.Dp5_FastNet || Dp5Versions == Dp5Version.Dp5_FastUsb)
            {
                Peakings.Add(0.05f);
                Peakings.Add(temp);
            }
            for (int i = 1; i < 9; i++)
            {
                temp = 0.2f * i;
                Peakings.Add(temp);
            }
            for (int i = 0; i < 4; i++)
            {
                temp += 0.4f;
                Peakings.Add(temp);
            }
            for (int i = 0; i < 4; i++)
            {
                temp += 0.8f;
                Peakings.Add(temp);
            }
            for (int i = 0; i < 4; i++)
            {
                temp += 1.6f;
                Peakings.Add(temp);
            }
            for (int i = 0; i < 4; i++)
            {
                temp += 3.2f;
                Peakings.Add(temp);
            }
            return Peakings;
        }

        public List<string> GetPeakingTimesStr()
        {
            List<string> peakings = new List<string>();
            List<float> dblPeakings = GetPeakingTimes();
            switch (Dp5Versions)
            {

                case Dp5Version.Dp5_FastNet:
                case Dp5Version.Dp5_FastUsb:
                    for (int i = 0; i < dblPeakings.Count; i++)
                    {
                        peakings.Add(string.Format("{0:0.00}", dblPeakings[i]));
                    }
                    break;
                case Dp5Version.Dp5_CommonUsb:
                default:
                    for (int i = 0; i < dblPeakings.Count; i++)
                    {
                        peakings.Add(string.Format("{0:0.0}", dblPeakings[i]));
                    }
                    break;
            }
            return peakings;
        }

        protected List<float> GetFlatTops(float PeakingTime)//单位us
        {
            List<float> FlatTops = new List<float>();
            switch (Dp5Versions)
            {

                case Dp5Version.Dp5_FastNet:
                case Dp5Version.Dp5_FastUsb:
                    float temp=0f;
                    if (PeakingTime <= 1.6005f)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            temp += 0.0125f;
                            FlatTops.Add(temp);
                        }
                        for (int i = 0; i < 13; i++)
                        {
                            temp += 0.05f;
                            FlatTops.Add(temp);
                        }
                        FlatTops.Add(0.787f);
                    }
                    else if (PeakingTime <= 3.2005f)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            temp += 0.025f;
                            FlatTops.Add(temp);
                        }
                        for (int i = 0; i < 13; i++)
                        {
                            temp += 0.1f;
                            FlatTops.Add(temp);
                        }
                        FlatTops.Add(1.575f);
                    }
                    else if (PeakingTime <= 6.4005f)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            temp += 0.05f;
                            FlatTops.Add(temp);
                        }
                        for (int i = 0; i < 13; i++)
                        {
                            temp += 0.2f;
                            FlatTops.Add(temp);
                        }
                        FlatTops.Add(3.150f);
                    }
                    else if (PeakingTime <= 12.8005f)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            temp += 0.1f;
                            FlatTops.Add(temp);
                        }
                        for (int i = 0; i < 13; i++)
                        {
                            temp += 0.4f;
                            FlatTops.Add(temp);
                        }
                        FlatTops.Add(6.3f);
                    }
                    else if (PeakingTime <= 25.6005f)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            temp += 0.2f;
                            FlatTops.Add(temp);
                        }
                        for (int i = 0; i < 13; i++)
                        {
                            temp += 0.8f;
                            FlatTops.Add(temp);
                        }
                        FlatTops.Add(12.6f);
                    }
                    break;
                case Dp5Version.Dp5_CommonUsb:
                default:
                    if (PeakingTime <= 1.6005f)
                    {
                        for (int i = 1; i < 17; i++)
                        {
                            FlatTops.Add(0.05f*i);
                        }
                    }
                    else if (PeakingTime <= 3.2005f)
                    {
                        for (int i = 1; i < 17; i++)
                        {
                            FlatTops.Add(0.1f * i);
                        }
                    }
                    else if (PeakingTime <= 6.4005f)
                    {
                        for (int i = 1; i < 17; i++)
                        {
                            FlatTops.Add(0.2f * i);
                        }
                    }
                    else if (PeakingTime <= 12.8005f)
                    {
                        for (int i = 1; i < 17; i++)
                        {
                            FlatTops.Add(0.4f * i);
                        }
                    }
                    else if (PeakingTime <= 25.6005f)
                    {
                        for (int i = 1; i < 17; i++)
                        {
                            FlatTops.Add(0.8f * i);
                        }
                    }
                    break;
            }
            return FlatTops;
        }

        public List<string> GetFlatTopsStr(float PeakingTime)
        {
            List<string> flattops = new List<string>();
            List<float> dblflattops = GetFlatTops(PeakingTime);
            switch (Dp5Versions)
            {

                case Dp5Version.Dp5_FastNet:
                case Dp5Version.Dp5_FastUsb:
                    for (int i = 0; i < dblflattops.Count; i++)
                    {
                        flattops.Add(string.Format("{0:0.000}", dblflattops[i]));
                    }
                    break;
                case Dp5Version.Dp5_CommonUsb:
                default:
                    for (int i = 0; i < dblflattops.Count; i++)
                    {
                        flattops.Add(string.Format("{0:0.00}", dblflattops[i]));
                    }
                    break;
            }
            return flattops;
        }

        public virtual bool SaveDP5CfgOther(int intPurOn, int intFastThreshold, string strPeaktime, string strFlattop, int intVoltage)
        {
            return true;
        }
        public virtual bool LoadDP5Cfg()//测试时
        {
            return true;
        }
        public virtual bool LoadDP5CfgOther(ref int intPurOn, ref int intFastThreshold, ref string strPeaktime, ref string strFlattop, ref int hv)
        {
            return true;
        }
        public virtual bool ChangeCfgFineGain(string FineGain, int SpecLength, int PreTime)
        {
            return true;
        }
        public virtual bool GetData(int[] data, ref bool IsSDDEnabled, ref double UsedTime, ref double FastCount, ref double SlowCount)
        {
            return true;
        }
        public virtual bool PauseDppDate(bool IsPause)
        {
            return true;
        }
        public virtual bool ClearDppData()
        {
            return true;
        }

        public virtual void OpenDevice()
        {
            return ;
        }
        public virtual void CloseDevice()
        {
            return ;
        }

        public virtual bool ConnectDevice(string Ip,string Port)
        {
            return true;
        }
        public virtual bool DisConnectDevice()
        {
            return true;
        }
        public virtual bool IsConnected()
        {
            return true;
        }
    }
   
}
