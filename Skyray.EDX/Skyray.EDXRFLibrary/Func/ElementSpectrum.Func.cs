using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDXRFLibrary
{
    public partial class ElementSpectrum
    {
        public object Clone()
        {
            ElementSpectrum elementSpectrum = this.MemberwiseClone() as ElementSpectrum;
            return elementSpectrum;
        }

        /// <summary>
        /// 计算高斯面积；面积计算公式为：fwhm×hight×sqrt（PI/Ln(2)）/2;
        /// 这里只计算fwhm×Hight
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public double XLineArea(XLine line)
        {
            double coef = Math.Sqrt(Math.PI / Math.Log(2)) / 2;
            switch (line)
            {
                case XLine.Ka:
                    return KaFwhm * KaHight * coef;
                case XLine.Kb:
                    return kbFwhm * kbFwhm * coef;
                case XLine.La:
                    return LaFwhm * LaHight * coef;
                case XLine.Lb:
                    return LbHight * LbFwhm * coef;
                //case XLine.Lr:
                //    return LrFwhm * LrHight;
                //    break;
                //case XLine.Le:
                //    return LeHight * LeFwhm;
                //    break;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 返回校正系数
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public Double XLineCoeff(XLine line)
        {
            switch (line)
            {
                case XLine.Ka:
                    return KaCoeff;
                case XLine.Kb:
                    return kbCoeff;
                case XLine.La:
                    return LaCoeff;
                case XLine.Lb:
                    return LbCoeff;
                default:
                    return 0;
            }
        }
        public Double XLineIntensity(XLine line)
        {
            switch (line)
            {
                case XLine.Ka:
                    return KaIntensity;
                case XLine.Kb:
                    return KbIntensity;
                case XLine.La:
                    return LaIntensity;
                case XLine.Lb:
                    return LbIntensity;
                default:
                    return 0;
            }
        }
    }
}
