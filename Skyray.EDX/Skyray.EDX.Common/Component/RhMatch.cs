using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Skyray.EDXRFLibrary;
using System.Xml;

namespace Skyray.EDX.Common.Component
{
    public struct PeakRang
    {
        public int Left;
        public int Right;
        public double Area;
    }

    public struct ElemPeak
    {
        public string Name;
        public int Left;
        public int Right;
        public int PeakChannel;
        public int Caculate;
        public int CurveType;
    }
    /// <summary>
    /// 识别材质（属于哪条工作曲线）
    /// </summary>
    public class Matter
    {
        private PeakRang basePeak;
        private PeakRang mainPeak;
        private ElemPeak[] elemPeak;
        private int[] data;
        private double ratio;///<主峰面积/背峰面积
        private XmlDocument xmlDoc=new XmlDocument();
        //private bool matchType;
        public PeakRang BasePeak { get { return basePeak; } }
        public PeakRang MainPeak { get { return mainPeak; } }
        public double Ratio { get {return ratio;}}

        public Matter(string filePath)
        {
            basePeak = new PeakRang();
            mainPeak = new PeakRang();
            data = new int[(int)WorkCurveHelper.DeviceCurrent.SpecLength];
            ratio = 5.5;
            xmlDoc.Load(filePath);
            LoadParameter();
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="filePath"></param>
        private void LoadParameter()
        {
            XmlNodeList xmlList = xmlDoc.SelectNodes("Parameter/Element");
            elemPeak = new ElemPeak[xmlList.Count];
            int i=0;
            foreach (XmlNode mode in xmlList)
            {
                elemPeak[i].Name = mode.SelectSingleNode("ElementName").InnerText;
                elemPeak[i].Left = int.Parse(mode.SelectSingleNode("Left").InnerText);
                elemPeak[i].Right = int.Parse(mode.SelectSingleNode("Right").InnerText);
                if (mode.SelectSingleNode("CurType") != null)
                    elemPeak[i].CurveType = int.Parse(mode.SelectSingleNode("CurType").InnerText);
                if (mode.SelectSingleNode("Caculate") != null)
                    elemPeak[i].Caculate = int.Parse(mode.SelectSingleNode("Caculate").InnerText);
                elemPeak[i].PeakChannel = (int)(elemPeak[i].Left + elemPeak[i].Right) / 2;
                i++;
            }
            ratio = double.Parse(xmlDoc.SelectSingleNode("Parameter").Attributes["TotalRatio"].InnerText);

            left = basePeak.Left = int.Parse(xmlDoc.SelectSingleNode("Parameter").Attributes["bLeft"].InnerText);
            right = basePeak.Right = int.Parse(xmlDoc.SelectSingleNode("Parameter").Attributes["bRight"].InnerText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void WriteParameter(string filePath)
        {
            XmlNodeList xmlList = xmlDoc.SelectNodes("Parameter/Element");

            foreach (XmlNode mode in xmlList)
            {
                int left = 0;
                int right = 0;
                if (WorkCurveHelper.SetDefaultPeak(mode.SelectSingleNode("ElementName").InnerText, out left, out right))
                {
                    mode.SelectSingleNode("Left").InnerText = left.ToString();
                    mode.SelectSingleNode("Right").InnerText = right.ToString();
                    //ElemPeak peak = elemPeak.ToList().Find(w => w.Name.ToLower() == mode.SelectSingleNode("ElementName").InnerText.ToLower());
                    //peak.Left = left;
                    //peak.Right = right;
                    //peak.PeakChannel = (int)(peak.Left + peak.Right) / 2;                  


                }
            }
            xmlDoc.Save(filePath);

            LoadParameter();
        }
        /// <summary>
        /// 计算单个峰的全面积
        /// </summary>
        /// <param name="left">左边界</param>
        /// <param name="right">右边界</param>
        /// <returns></returns>
        private double TotalArea(int left, int right)
        {
            double area = 0;
            if (left < 0 || right < 0)
                return 0;
            for (int i = left; i <= right; i++)
            {
                area += data[i];
            }
            return area;
        }

        /// <summary>
        /// 计算峰比例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="type">元素类型:1:Cr,Cl;2:Sn</param>
        /// <returns></returns>
        private double PeakRatio(int left, int right,int type)
        {
            double ratio = 0;
            double area = TotalArea(left, right);
            if (area != 0)
            {
                switch (type)
                {
                    case 1:
                        double netArea = area - (data[left] + data[right]) * (right - left + 1) * 0.5;
                        ratio = netArea / area;
                        break;
                    case 2:
                        ratio = area / mainPeak.Area;
                        break;
                    default: break;
                }
            }
            return ratio;
        }

        /// <summary>
        ///寻找（设置）主峰的左右边界
        /// </summary>
        private void SetMainPeakRang()
        {
            int max = 0;
            for (int i = basePeak.Left - 1; i >= 0; i--)
            {
                if (data[i] > data[max])
                    max = i;
            }
            for (int i = basePeak.Right + 1; i < data.Length; i++)
            {
                if (data[i] > data[max])
                    max = i;
            }
            double minPeak = data[max] / 3;
            for (int i = max; i >= 0; i--)
            {
                if (minPeak > data[i])
                {
                    mainPeak.Left = i;
                    break;
                }
            }
            for (int i = max; i < data.Length; i++)
            {
                if (minPeak > data[i])
                {
                    mainPeak.Right = i;
                    break;
                }
            }
        }

        private int left;
        private int right;

        /// <summary>
        /// 判断属于哪条工作曲线
        /// </summary>
        /// <param name="curveType">曲线类型</param>
        /// <param name="flag">是否匹配</param>
        /// <returns>工作曲线名</returns>
        public string Verdict(int cureType, out int matchRegion,int bLeft,int bRight,bool flag)
        {
            string matterName = string.Empty;
            if (!flag && cureType == 0)
            {
                basePeak.Left = bLeft;
                basePeak.Right = bRight;
            }
            else
            {
                basePeak.Left = left;
                basePeak.Right = right;
            }
            matchRegion = 0;
            double ratioTemp = CaculateRatio(cureType);
            if (ratioTemp > ratio)//金属
            {
                matterName = MetalPartVerdict(cureType, out matchRegion);
            }
            if ((matterName == "Br") || matterName=="Cl"|| (ratioTemp <= ratio))
            {
                matterName = NoMetalPartVerdict(cureType, out matchRegion, matterName, ratioTemp);
            }
            return matterName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cureType"></param>
        /// <returns></returns>
        private double CaculateRatio(int cureType)
        {
            string matterName = string.Empty;
            basePeak.Area = TotalArea(basePeak.Left, basePeak.Right);
            SetMainPeakRang();
            mainPeak.Area = TotalArea(mainPeak.Left, mainPeak.Right);
            int peakChannel = (int)(mainPeak.Left + mainPeak.Right) / 2;
            mainPeak.Left -= 20;
            mainPeak.Right += 20;
            double tempRatio = 0;
            if (basePeak.Area != 0)
            {
                tempRatio = mainPeak.Area / basePeak.Area;
            }
            else
            {
                tempRatio = ratio + 1;
            }
            return tempRatio;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curveType"></param>
        /// <param name="matchRegion"></param>
        /// <returns></returns>
        private string MetalPartVerdict(int curveType,out int matchRegion)
        {
            matchRegion = 0;
            string matterName = string.Empty;
            for (int i = 0; i < elemPeak.Length; i++)
            {
                if ((elemPeak[i].PeakChannel > mainPeak.Left) && (elemPeak[i].PeakChannel < mainPeak.Right))
                {
                    matterName = elemPeak[i].Name;
                    XmlNodeList xmlNodeList = xmlDoc.SelectNodes("Parameter/match/Metal/Node");
                    bool changeRegion = false;
                    double ironMax = 0;
                    foreach (XmlNode xTemp in xmlNodeList)
                    {
                        ElemPeak ep = elemPeak.ToList().Find(wde => wde.Name.Equals(xTemp.Attributes["elementName"].InnerText) == true); ;
                        double ratioTemp = 0;
                        double.TryParse(xTemp.Attributes["ratio"].InnerText, out ratioTemp);
                        int calculateTemp = 0;
                        int.TryParse(xTemp.Attributes["caculate"].InnerText, out calculateTemp);
                        string conditionStr = null;
                        int type = int.Parse(xTemp.Attributes["cureType"].InnerText);
                        if (xTemp.Attributes["condition"] != null)
                        {
                            conditionStr = xTemp.Attributes["condition"].InnerText;
                        }
                        string tempName = xTemp.Attributes["matterName"].InnerText;
                        int direction = int.Parse(xTemp.Attributes["direction"].InnerText);
                        if ((conditionStr != null && matterName.Equals(conditionStr) && PeakRatio(ep.Left, ep.Right, calculateTemp) > ratioTemp && direction == 1)
                            || (PeakRatio(ep.Left, ep.Right, calculateTemp) > ratioTemp && direction == 1))
                        {
                            double temp = PeakRatio(ep.Left, ep.Right, calculateTemp);
                            if (ironMax < temp)
                            {
                                matterName = tempName;
                                matchRegion = type;
                                changeRegion = true;
                            }
                        }
                    }
                    if (!changeRegion)
                        matchRegion = elemPeak[i].CurveType;
                    break;
                }
            }
            return matterName;
        }

        public string peakRatio;
        //public string caculateRatio;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curveType"></param>
        /// <param name="matchRegion"></param>
        /// <param name="matterName"></param>
        /// <returns></returns>
        private string NoMetalPartVerdict(int curveType, out int matchRegion, string matterName,double tempRatio)
        {
            string outMatterName = string.Empty;
            matchRegion = 0;
            bool flag = false;
            peakRatio="";
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("Parameter/match/NoMetal/Node");
            double areaMax = double.MinValue;
            foreach (XmlNode xTemp in xmlNodeList)
            {
                ElemPeak ep = elemPeak.ToList().Find(wde => wde.Name.Equals(xTemp.Attributes["elementName"].InnerText) == true);
                var curtypeObj = xTemp.Attributes["cureType"];
                if (curtypeObj == null)
                    continue;
                int calculateTemp = 0;
                double ratioTemp = 0;
                double.TryParse(xTemp.Attributes["ratio"].InnerText, out ratioTemp);
                int.TryParse(xTemp.Attributes["caculate"].InnerText, out calculateTemp);
                int type = int.Parse(xTemp.Attributes["cureType"].InnerText);
                int direction = int.Parse(xTemp.Attributes["direction"].InnerText);
                string tempName = xTemp.Attributes["matterName"].InnerText;
                string conditionStr = string.Empty;
                if (xTemp.Attributes["OrignalRatio"] != null)
                {
                    conditionStr = xTemp.Attributes["OrignalRatio"].InnerText;
                }
                if (type == curveType)
                {
                    flag = true;
                    peakRatio += tempName + "," + PeakRatio(ep.Left, ep.Right, calculateTemp).ToString() + "," + ratioTemp + "," + direction + "|";
                    if (
                        (!string.IsNullOrEmpty(conditionStr) && tempRatio>double.Parse(conditionStr)) 
                        || (string.IsNullOrEmpty(conditionStr) && PeakRatio(ep.Left, ep.Right, calculateTemp) > ratioTemp && direction == 1)
                        || (string.IsNullOrEmpty(conditionStr) && PeakRatio(ep.Left, ep.Right, calculateTemp) <= ratioTemp && direction == 0))
                    {
                        double temp = PeakRatio(ep.Left, ep.Right, calculateTemp);
                        if (areaMax < temp)
                        {
                            areaMax = temp;
                            matterName = tempName;
                            matchRegion = type;
                        }
                    }
                }
            }
            if (!flag)
            {
                XmlNodeList NodeListChild = xmlDoc.SelectNodes("Parameter/match/NoMetal/Default");
                foreach (XmlNode tempNode in NodeListChild)
                {
                    ElemPeak ep = elemPeak.ToList().Find(wde => wde.Name.Equals(tempNode.Attributes["elementName"].InnerText) == true);
                    int calculateTemp = 0;
                    int.TryParse(tempNode.Attributes["caculate"].InnerText, out calculateTemp);
                    double ratioTemp = 0;
                    double.TryParse(tempNode.Attributes["ratio"].InnerText, out ratioTemp);
                    string tempName = tempNode.Attributes["matterName"].InnerText;
                    int direction = int.Parse(tempNode.Attributes["direction"].InnerText);
                    int type = int.Parse(tempNode.Attributes["cureType"].InnerText);
                    peakRatio += tempName + "," + PeakRatio(ep.Left, ep.Right, calculateTemp).ToString() + "," + ratioTemp + "," + direction + "|";
                    if ((PeakRatio(ep.Left, ep.Right, calculateTemp) > ratioTemp && direction == 1)
                        || (PeakRatio(ep.Left, ep.Right, calculateTemp) <= ratioTemp && direction == 0))
                    {
                        matterName = tempName;
                        matchRegion = type;
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(peakRatio))
                peakRatio = peakRatio.Substring(0,peakRatio.Length-1);
            outMatterName = matterName;
            return outMatterName;
        }

        public void SetData(int[] data)
        {
            data.CopyTo(this.data, 0);
        }
    }
}

