using System;
using System.Collections.Generic;
using System.Text;
using Skyray.EDXRFLibrary;
using System.Xml;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDX.Common.Component
{
    public class Resolve
    {
        private double dblEnergyKa;///<���ڼ���ֱ��ʵ�Ԫ�ص�Ka�������
        private double dblEnergyKb;///<���ڼ���ֱ��ʵ�Ԫ�ص�Kb�������
        private long atomId;
        public DeviceParameter Deviceparam;
        public SpecEntity Spec;

        /// <summary>
        /// ���캯��
        /// </summary>
        public Resolve()
        {
            dblEnergyKa = 5895;
            dblEnergyKb = 6492;
        }

        /// <summary>
        /// �������ڼ���ֱ��ʵ�Ԫ��Ka��Kb�ߵ�����
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadElemEnergy(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode xmlTemp = doc.SelectSingleNode("Parameter/System/EnergyKa");
            dblEnergyKa = Convert.ToDouble(xmlTemp.InnerText);
            xmlTemp = doc.SelectSingleNode("Parameter/System/EnergyKb");
            dblEnergyKb = Convert.ToDouble(xmlTemp.InnerText);
        }

        public long AtomId
        {
            set
            {
                atomId = value;
                Atom atom = Atom.FindById(value);
                if (atom != null)
                {
                    dblEnergyKa = atom.Ka * 1000;
                    dblEnergyKb = atom.Kb * 1000;
                }
            }
        }

        /// <summary>
        /// ����ֱ���
        /// </summary>
        /// <returns></returns>
        public double CalculateResolve()
        {
            if (this.Spec == null)
            {
                return 0d;
            }
            int low = 0;
            int high = 0;
            try
            {
                int[] spec = this.Spec.SpecDatas;
                double ch = SpecHelper.FitChannOfMaxValue(0, spec.Length - 1, spec, ref low, ref high);
                double halfValue = spec[(int)ch] * 1.0 / 2;
                double slope = DemarcateEnergyHelp.k1 * 1000;
                ////���߿�ľ�ȷ�߽�
                if ((low - 1) < 0 || low < 0 || (high - 1) < 0 || high < 0) return 0d;
                double L = low + (halfValue - spec[low]) / (spec[low + 1] - spec[low]);
                double H = high - (halfValue - spec[high]) / (spec[high - 1] - spec[high]);

                //double scope0=(this.Spec.SpecDatas[low + 1] - this.Spec.SpecDatas[low]) / (DemarcateEnergyHelp.GetEnergy(low + 1)- DemarcateEnergyHelp.GetEnergy(low));
                //double tempDistance0 = this.Spec.SpecDatas[low + 1]-scope0 * DemarcateEnergyHelp.GetEnergy(low + 1);
                //double lenergy = (halfValue - tempDistance0) * 1 / scope0;

                //double scope1 = (this.Spec.SpecDatas[high- 1] - this.Spec.SpecDatas[high]) / (DemarcateEnergyHelp.GetEnergy(high - 1) - DemarcateEnergyHelp.GetEnergy(high));
                //double tempDistance1 = this.Spec.SpecDatas[high-1] - scope1 * DemarcateEnergyHelp.GetEnergy(high-1);
                //double renergy = (halfValue - tempDistance1) * 1 / scope1;
                //double energy0 = DemarcateEnergyHelp.GetEnergy((int)L);
                //double energy1 = DemarcateEnergyHelp.GetEnergy((int)H);
                //return ((H - L) * slope);
                return SpecHelper.GuassValueOfMaxValue(0, spec.Length - 1, spec, ref low, ref high)*slope;
            }
            catch  { return 0; }
            //int intSubProduct = 0;
            //int intSumCount = 0;
            //int intLowerHalf1 = 0;
            //int intLowerHalf2 = 0;
            //int intUpperHalf1 = 0;
            //int intUpperHalf2 = 0;
            //double dblFWHM = 0;
            //int[] bakData =this.Spec.SpecDatas;
            //int maxChannel = SpecHelper.GetHighSpecChannel(this.Deviceparam.BeginChann,this.Deviceparam.EndChann, bakData);
            //try
            //{
            //    int maxValue = bakData[maxChannel];
            //    double dblFirstHalf = maxValue * 0.5;
            //    for (int i = maxChannel; i > maxChannel - 50; i--)
            //    {
            //        if ((dblFirstHalf < bakData[i]) && (dblFirstHalf >= bakData[i - 1]))//�ҵ���߷壨Ka����߸պô��ڰ�ߵĵ�
            //        {
            //            intLowerHalf1 = i;
            //            break;
            //        }
            //    }
            //    for (int i = maxChannel; i < maxChannel + 50; i++)
            //    {
            //        if ((dblFirstHalf < bakData[i]) && (dblFirstHalf >= bakData[i + 1]))//�ҵ���߷壨Ka���ұ߸պô��ڰ�ߵĵ�
            //        {
            //            intUpperHalf1 = i;
            //            break;
            //        }
            //    }

            //    for (int i = intLowerHalf1; i <= intUpperHalf1; i++)
            //    {
            //        intSubProduct += i * bakData[i];
            //        intSumCount += bakData[i];
            //    }
            //    double dblCalChKa = intSubProduct / (intSumCount * 1.0); //Ka���Ӧ��ͨ��

            //    intSubProduct = 0;
            //    intSumCount = 0;
            //    int minChannel = SpecHelper.GetMinValue(maxValue, maxChannel, bakData);
            //    double dblSecondHalf = bakData[minChannel] * 0.5;
            //    for (int i = minChannel; i > minChannel - 50; i--)
            //    {
            //        if ((dblSecondHalf < bakData[i]) && (dblSecondHalf >= bakData[i - 1]))//�ҵ��ڶ��߷壨Kb����߸պô��ڰ�ߵĵ�
            //        {
            //            intLowerHalf2 = i;
            //            break;
            //        }
            //    }
            //    for (int i = minChannel; i < minChannel + 50; i++)
            //    {
            //        if ((dblSecondHalf < bakData[i]) && (dblSecondHalf >= bakData[i + 1]))//�ҵ��ڶ��߷壨Kb���ұ߸պô��ڰ�ߵĵ�
            //        {
            //            intUpperHalf2 = i;
            //            break;
            //        }
            //    }

            //    for (int i = intLowerHalf2; i <= intUpperHalf2; i++)
            //    {
            //        intSubProduct += i * bakData[i];
            //        intSumCount += bakData[i];
            //    }
            //    double dblCalChKb = intSubProduct / (intSumCount * 1.0);//Kb���Ӧ��ͨ��

            //    double dblFWHM1 = -(maxValue / 2.0 - bakData[intLowerHalf1]) / (bakData[intLowerHalf1 - 1] - bakData[intLowerHalf1]) + intLowerHalf1;
            //    double dblFWHM2 = intUpperHalf1 + (maxValue / 2.0 - bakData[intUpperHalf1]) / (bakData[intUpperHalf1 + 1] - bakData[intUpperHalf1]);

            //    double dblCalSlope = (dblEnergyKa - dblEnergyKb) / (dblCalChKa - dblCalChKb);

            //    dblFWHM = (dblFWHM2 - dblFWHM1) * dblCalSlope; //�ֱ���
            //}
            //catch (System.Exception )
            //{
            //    return 0;
            //}
            //return dblFWHM;
        }
    }
}
