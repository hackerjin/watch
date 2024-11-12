using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace Skyray.EDX.Common
{
    public class Ranges
    {
        public static Dictionary<string, RangeInfo> RangeDictionary;

        public Ranges()
        {
            RangeDictionary = new Dictionary<string, RangeInfo>();
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Ranges.xml"))
            {
                RangeDictionary.Add("Energy", new RangeInfo { DecimalPlaces = 2, Increment = 0.01m, Max = 1000, Min = 0 });//能量
                RangeDictionary.Add("Fwhm", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//分辨率
                RangeDictionary.Add("AtomNum", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 92, Min = 0 });//原子序号
                RangeDictionary.Add("Angel", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 90, Min = 0 });//角度
                RangeDictionary.Add("Thickness", new RangeInfo { DecimalPlaces = 2, Increment = 0.01m, Max = 90, Min = 0 });//厚度
                RangeDictionary.Add("Incident", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 90, Min = 0 });//入射角
                RangeDictionary.Add("Exit", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 90, Min = 0 });//出射角
                RangeDictionary.Add("MaterialAtomNum", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 92, Min = 0 });//窗口原子序号
                RangeDictionary.Add("SpecLength", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 4096, Min = 0 });//谱长度
                RangeDictionary.Add("VoltageScaleFactor", new RangeInfo { DecimalPlaces = 2, Increment = 0.01m, Max = 100, Min = 0 });//管压比例因子
                RangeDictionary.Add("CurrentScaleFactor", new RangeInfo { DecimalPlaces = 2, Increment = 0.01m, Max = 100, Min = 0 });//管流比例因子
                RangeDictionary.Add("TubVoltage", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 50, Min = 0 });//管压
                RangeDictionary.Add("TubCurrent", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 800, Min = 0 });//管流
                RangeDictionary.Add("Gain", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 25500, Min = 0 });//粗调码
                RangeDictionary.Add("FineGain", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 25500, Min = 0 });//细调码
                RangeDictionary.Add("Channel", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 4096, Min = 0 });//初始化通道
                RangeDictionary.Add("PreHeatTime", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 9999, Min = 1 });//开始预热时间
                RangeDictionary.Add("FinalHeatTime", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 9999, Min = 1 });//扫谱预热时间
                RangeDictionary.Add("ChannelError", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 2048, Min = 0 });//误差道
                RangeDictionary.Add("Filter", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 1 });//滤光片
                RangeDictionary.Add("Collimator", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 1 });//准直器
                RangeDictionary.Add("Target", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 1 });//靶材
                RangeDictionary.Add("Chamber", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 1 });//样品腔
                RangeDictionary.Add("PrecTime", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 9999, Min = 1 });//测量时间
                RangeDictionary.Add("FilterIdx", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 1 });//滤光片
                RangeDictionary.Add("CollimatorIdx", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 1 });//准直器
                RangeDictionary.Add("ColTargetIdx", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 1 });//靶材
                RangeDictionary.Add("ColCurrentRate", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 1 });//二次靶管流因子
                RangeDictionary.Add("CurrentRate", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 1 });//二次靶管流因子
                RangeDictionary.Add("VacuumTime", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//抽真空时间
                RangeDictionary.Add("VacuumDegree", new RangeInfo { DecimalPlaces = 2, Increment = 0.01m, Max = 1000, Min = 0 });//真空度
                RangeDictionary.Add("MinRate", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100000, Min = 1 });//最小計數率
                RangeDictionary.Add("MaxRate", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100000, Min = 1 });//最大計數率
                RangeDictionary.Add("PeakLow", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 4096, Min = 0 });//峰左界
                RangeDictionary.Add("PeakHigh", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 4096, Min = 0 });//峰右界
                RangeDictionary.Add("BaseLow", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 4096, Min = 0 });//背左界
                RangeDictionary.Add("BaseHigh", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 4096, Min = 0 });//背右界
                RangeDictionary.Add("UnitaryValue", new RangeInfo { DecimalPlaces = 3, Increment = 0.001m, Max = 100, Min = 0 });//归一值
                RangeDictionary.Add("OptimiztionValue", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100, Min = 0 });//优化值
                RangeDictionary.Add("OptimiztionRange", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100, Min = 0 });//优化范围
                RangeDictionary.Add("OptimiztionFactor", new RangeInfo { DecimalPlaces = 2, Increment = 0.01m, Max = 1, Min = 0 });//优化因子
                RangeDictionary.Add("OptimiztionMax", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100, Min = 0 });//优化最大值
                RangeDictionary.Add("OptimiztionMin", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100, Min = 0 });//优化最小值
                RangeDictionary.Add("EscapeAngle", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//逃逸峰处理角度
                RangeDictionary.Add("EscapeFactor", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//逃逸峰处理因子
                RangeDictionary.Add("PulseResolution", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//脉冲对分辨率(ms)
                RangeDictionary.Add("SumFactor", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//因子 
                RangeDictionary.Add("RemoveFirstTimes", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//扣本底1次数 
                RangeDictionary.Add("RemoveFirstFactor", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//扣本底1因子
                RangeDictionary.Add("RemoveSecondTimes", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//扣本底2次数 
                RangeDictionary.Add("RemoveSecondFactor", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//扣本底2因子
                RangeDictionary.Add("BackGroundPoint", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 3, Min = 1 });//背景点
                RangeDictionary.Add("ChannFWHM", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100000, Min = 0 });//峰的半高宽
                RangeDictionary.Add("WindowWidth", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100000, Min = 0 });//窗口宽度
                RangeDictionary.Add("Trh1", new RangeInfo { DecimalPlaces = 1, Increment = 0.1m, Max = 100000, Min = 0 });//灵敏度阈值
                RangeDictionary.Add("ValleyDistance", new RangeInfo { DecimalPlaces = 1, Increment = 0.1m, Max = 100000, Min = 0 });//峰谷距阈值
                RangeDictionary.Add("AreaLimt", new RangeInfo { DecimalPlaces = 1, Increment = 0.1m, Max = 100000, Min = 0 });//面积比阈值
                RangeDictionary.Add("MeasuringCount", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 1 });//测量次数
                RangeDictionary.Add("LossPacketTime", new RangeInfo { DecimalPlaces = 0, Increment = 0.3m, Max = 100, Min = 0 });//丢包时间
                RangeDictionary.Add("MatchTime", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100, Min = 0 });//匹配时间 
                RangeDictionary.Add("ElectricalCode", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 0 });//电机编号
                RangeDictionary.Add("ElectricalDirect", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1, Min = 0 });//方向
                
                RangeDictionary.Add("MotorXCode", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 0 });//X轴电机编号
                RangeDictionary.Add("MotorXDirect", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1, Min = 0 });//X轴电机方向
                RangeDictionary.Add("MotorXMaxStep", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//X轴电机最大步长
                RangeDictionary.Add("MotorYCode", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 0 });//Y轴电机编号
                RangeDictionary.Add("MotorYDirect", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1, Min = 0 });//Y轴电机方向
                RangeDictionary.Add("MotorYMaxStep", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//Y轴电机最大步长
                RangeDictionary.Add("MotorZCode", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 0 });//Z轴电机编号
                RangeDictionary.Add("MotorZDirect", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1, Min = 0 });//Z轴电机方向
                RangeDictionary.Add("MotorZMaxStep", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//Z轴电机最大步长
                
                RangeDictionary.Add("MotorY1Code", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 0 });//Y1轴电机编号
                RangeDictionary.Add("MotorY1Direct", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1, Min = 0 });//Y1轴电机方向
                RangeDictionary.Add("MotorY1MaxStep", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000, Min = 0 });//Y1轴电机最大步长
                
                RangeDictionary.Add("StandardContent", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100000, Min = 0 });//标准含量
                RangeDictionary.Add("StartStandardContent", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100000, Min = 0 });//标准含量
                RangeDictionary.Add("RefConf", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 10000, Min = -10000 });//影响系数
                RangeDictionary.Add("CollimatorMaxNum", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 0 });//准直器最大编号
                RangeDictionary.Add("FilterMaxNum", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 0 });//滤光片最大编号
                RangeDictionary.Add("ChamberMaxNum", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 16, Min = 0 });//样品腔最大编号
                RangeDictionary.Add("TargetMaxNum", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 8, Min = 0 });//靶材最大编号
                RangeDictionary.Add("EditContent", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100, Min = 0 });//编辑数据含量
                RangeDictionary.Add("EditIntensity", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000000, Min = 0 });//编辑数据强度
                RangeDictionary.Add("ComparisonCoefficient", new RangeInfo { DecimalPlaces = 1, Increment = 0.1m, Max = 100, Min = 0 });//ROHS感兴趣元素阈值
                RangeDictionary.Add("ExampleMaxValue", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100, Min = 0 });//规格最大值
                RangeDictionary.Add("ExampleMinValue", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 100, Min = 0 });//规格最大值
                RangeDictionary.Add("StandardThick", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000000, Min = 0 });//规格最大值
                RangeDictionary.Add("StandardThickMax", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = 1000000, Min = 0 });//规格最大值

                XmlTextWriter writer = new XmlTextWriter(AppDomain.CurrentDomain.BaseDirectory + "Ranges.xml", Encoding.Unicode);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("Ranges");
                foreach (var a in RangeDictionary)
                {
                    writer.WriteStartElement("Range");
                    writer.WriteAttributeString("Name", a.Key);
                    writer.WriteAttributeString("DecimalPlaces", a.Value.DecimalPlaces.ToString());
                    writer.WriteAttributeString("Increment", a.Value.Increment.ToString());
                    writer.WriteAttributeString("Max", a.Value.Max.ToString());
                    writer.WriteAttributeString("Min", a.Value.Min.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            try
            {
                RangeDictionary.Clear();
                XmlDocument doc = new XmlDocument();
                doc.Load(AppDomain.CurrentDomain.BaseDirectory + "Ranges.xml");
                XmlNode node = doc.SelectSingleNode("Ranges");
                XmlNodeList nodes = node.SelectNodes("Range");
                foreach (XmlNode a in nodes)
                {
                    string name = a.Attributes["Name"].InnerText;
                    int decimalplace = int.Parse(a.Attributes["DecimalPlaces"].InnerText);
                    decimal increment = decimal.Parse(a.Attributes["Increment"].InnerText);
                    decimal max = decimal.Parse(a.Attributes["Max"].InnerText);
                    decimal min = decimal.Parse(a.Attributes["Min"].InnerText);
                    RangeDictionary.Add(a.Attributes["Name"].InnerText, new RangeInfo { DecimalPlaces = decimalplace, Increment = increment, Max = max, Min = min });
                }
            }
            catch (Exception ex)
            {
                string decimalSymbol = GetCurrentDecimalSymbol();//检查是否是系统小数点的问题
                if (decimalSymbol != ".")
                {
                    MessageBox.Show("Load Ranges Error.\r\nCurrent number decimal symbol \"" + decimalSymbol + "\" is not supported, please change it to \".\" through [Region-Number-DecimalSymbol]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //如果不是上述问题, 则直接报错, 并附上异常信息, 便于查找原因
                MessageBox.Show("Load Ranges Error.\r\nDetails: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetCurrentDecimalSymbol()
        {
            return System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        }
    }

    public class RangeInfo
    {
        public int DecimalPlaces { get; set; }
        public decimal Increment { get; set; }
        public decimal Max { get; set; }
        public decimal Min { get; set; }
    }
}
