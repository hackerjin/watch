using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;


namespace Skyray.EDXRFLibrary
{
    /// <summary>
    /// 定性分析中纯元素的谱数据
    /// </summary>
    [Serializable]
    public abstract partial class ElementSpectrum : DbObjectModel<ElementSpectrum>
    {
        public abstract int ElementID { get; set; }      //元素的名称   
    
        /// <summary>
        /// Ka半高宽
        /// </summary>
        public abstract double KaFwhm { get; set; }
        /// <summary>
        /// Kb半高宽
        /// </summary>
        public abstract double kbFwhm { get; set; }
        /// <summary>
        /// La半高宽
        /// </summary>
        public abstract double LaFwhm { get; set; }//半高宽
        /// <summary>
        /// Lb半高宽
        /// </summary>
        public abstract double LbFwhm { get; set; }

        /// <summary>
        /// Ka峰高
        /// </summary>
        public abstract double KaHight { get; set; }//峰高
        /// <summary>
        /// Kb峰高
        /// </summary>
        public abstract double KbHight { get; set; }
        /// <summary>
        /// La峰高
        /// </summary>
        public abstract double LaHight { get; set; }
        /// <summary>
        /// Lb峰高
        /// </summary>
        public abstract double LbHight { get; set; }


        public abstract double KaCoeff { get; set; }//线强度的校正系数默认为1
        public abstract double kbCoeff { get; set; }
        public abstract double LaCoeff { get; set; }
        public abstract double LbCoeff { get; set; }


        /// <summary>
        /// Ka强度
        /// </summary>
        public abstract double KaIntensity { get; set; }
        /// <summary>
        /// Kb强度
        /// </summary>
        public abstract double KbIntensity { get; set; }
        /// <summary>
        /// La强度
        /// </summary>
        public abstract double LaIntensity { get; set; }
        /// <summary>
        /// Lb强度
        /// </summary>
        public abstract double LbIntensity { get; set; }

        public double KaArea
        {
            get { return XLineArea(XLine.Ka); }
        }

        public double KbArea
        {
            get { return XLineArea(XLine.Kb); }
        }

        public double LaArea
        {
            get { return XLineArea(XLine.La); }
        }

        public double LbArea
        {
            get { return XLineArea(XLine.Lb); }
        }

        public double LrArea
        {
            get { return XLineArea(XLine.Lr); }
        }

        public double LeArea
        {
            get { return XLineArea(XLine.Le); }
        }
         
        public abstract ElementSpectrum Init(int ElementID,
            double KaFwhm,
            double KbFwhm,
            double LaFwhm,
            double LbFwhm,
            double KaHight,
            double KbHight,
            double LaHight,
            double LbHight,
            double KaCoeff,
            double KbCoeff,
            double LaCoeff,
            double LbCoeff,
            double KaIntensity,
            double KbIntensity,
            double LaIntensity,
            double LbIntensity
            );
    }
}
