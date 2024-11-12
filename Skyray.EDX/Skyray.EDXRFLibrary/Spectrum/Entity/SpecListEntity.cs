using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary.Define;
using System.Xml.Serialization;

namespace Skyray.EDXRFLibrary.Spectrum
{
    [Serializable]
    public class SpecListEntity
    {
        public string Name { get; set; }
        public string SampleName { get; set; }
        public double Height { get; set; }
        public double CalcAngleHeight { get; set; }
        /// <summary>;
        /// 工作曲线的ID
        /// </summary>
        public string WorkCurveName { set; get; }

        public string DeviceName { get; set; }
        public string Supplier { get; set; }
        public double? Weight { get; set; }
        public string Shape { get; set; }
        public string Operater { get; set; }
        public DateTime? SpecDate { get; set; }
        public string SpecSummary { get; set; }
        public SpecType SpecType;
        public int SpecTypeValue { get { return (int)this.SpecType; } }
        public double ActualVoltage { get; set; }//实际管压
        public double ActualCurrent { get; set; }//实际管流
        public double CountRate { get; set; }//计数率
        public double PeakChannel { get; set; }//峰通道
        public double Resole { get; set; }//分辨率
        //public int TotalCount { get; set; }//总计数
        public long TotalCount { get; set; }//总计数
        public int NameType { get; set; }//0非规则，1为规则
        public bool ImageShow { get; set; }
        public int Color { get; set; }
        public int VirtualColor { get; set; }
        public double Loss { get; set; }

        public InitParamsEntity InitParam { get; set; }
        /// <summary>
        /// 能量刻度
        /// </summary>
        public DemarcateEnergyEntity[] DemarcateEnergys { get; set; }

        /// <summary>
        /// 包含子谱数据
        /// </summary>
        public SpecEntity[] Specs { get; set; }

        [XmlIgnore]
        public List<CompanyOthersInfo> CompanyInfoList { get; set; }  //将样品其他信息与谱关联
       

        public SpecListEntity() {

            //Specs = new SpecEntity[] { };
            //DemarcateEnergys = new List<DemarcateEnergyEntity>();
        }
        public SpecListEntity
            (string Name,
            string SampleName,
             double Height,
            double CalcAngleHeight,
            string Supplier,
            double? Weight,
            string Shape,
            string Operater,
            DateTime? SpecDate,
            string SpecSummary,
            SpecType SpecType,
            int Color,int  VirtualColor):this()
        {
            this.Name = Name;
            this.SampleName = SampleName;
            this.Height = Height;
            this.CalcAngleHeight = CalcAngleHeight;
            this.Supplier = Supplier;
            this.Weight = Weight;
            this.Shape = Shape;
            this.SpecDate = SpecDate;
            this.Operater = Operater;
            this.SpecSummary = SpecSummary;
            this.SpecType = SpecType;
            this.Color = Color;
            this.VirtualColor = VirtualColor;
        }

      
    }
}
