using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    public abstract class SpectrumData : DbObjectModel<SpectrumData>
    {
        /// <summary>
        /// 谱名称
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// 谱测量的高度
        /// </summary>
        public abstract double Height { get; set; }

        /// <summary>
        /// 谱计算高度
        /// </summary>
        public abstract double CalcAngleHeight { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public abstract string DeviceName { get; set; }

        /// <summary>
        /// 工作曲线名称
        /// </summary>
        [AllowNull]
        public abstract string WorkCurveName { get; set; }

        /// <summary>
        /// 谱类型，0非规则，1为规则
        /// </summary>
        public abstract int NameType { get; set; }

        /// <summary>
        /// 谱标识
        /// </summary>
        public abstract SpecType SpecTypeValue { get; set; }

        /// <summary>
        /// 样品名称
        /// </summary>
        public abstract string SampleName { get; set; }

        public abstract DateTime? SpecDate { get; set; }

        /// <summary>
        /// 保存的二进制流
        /// </summary>
        public abstract Byte[] Data { get; set; }

        public abstract SpectrumData Init
           (string Name, double Height,double CalcAngleHeight, string DeviceName, string WorkCurveName,
           int NameType, SpecType SpecTypeValue, string SampleName, Byte[] Data, DateTime? SpecDate);
   
    }
}
