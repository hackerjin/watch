using System;
using System.Xml;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Linq;

using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;

/// <summary>
/// ���ڷ�����
/// </summary>
/// 
namespace Skyray.EDX.Common
{
    /// <summary>
    /// ���Է���
    /// </summary>
    public class QualeElementOperation
    {
        /// <summary>
        /// �ж��Ƿ�Ϊ����ģʽ
        /// </summary>
        public Boolean IsIntelligence { get; set; }
        private SpecEntity spec;
        private QualeElement Element;
        private int[] peakPositions;    //���з��λ��
        private Double[] data;           //ԭʼ�׳��Բ���ʱ����������
        //private ElementSpectrumDataBase db; //��Ԫ�������ݿ�
        public List<int> listInt = new List<int>();

        public delegate int EnergyToChannel(double energy); //����ת��Ϊ��
        public delegate double ChannelToEnergy(int channel); //��ת��Ϊ����

        public EnergyToChannel ToChannel;
        public ChannelToEnergy ToEnergy;

        //׷����Ԫƥ��
        public delegate int EnergyToChannel2(double energy, List<DemarcateEnergy> demarcateEnergy); //����ת��Ϊ��
        public EnergyToChannel2 ToChannel2;

        private ElementSpectrumDataBase ElementSpectrumDataBase;

        public QualeElementOperation()
        {
            Element = QualeElement.New;
            Element.AvoidElem = "Tc,Dy,Eu,Tm,Po,At,Rn,Fr,Ra";
            Element.ChannFWHM = 30;
            Element.WindowWidth = 15;
            Element.Trh1 = 1.5;
            Element.ValleyDistance = 1.5;
            Element.AreaLimt = 1;
            ElementSpectrumDataBase = new ElementSpectrumDataBase();
        }

        /// <summary>
        /// ����߷�λ��
        /// </summary>
        /// <returns></returns>
        private int StorelyPoisition()
        {
            int storelyPosition = -1;
            for (int i = 0; i < peakPositions.Length; i++)
            {
                if (data[peakPositions[i]] <= 0)
                {
                    continue;
                }
                if (storelyPosition < 0)
                {
                    storelyPosition = peakPositions[i];
                    continue;
                }
                if (data[storelyPosition] < data[peakPositions[i]])
                {
                    storelyPosition = peakPositions[i];
                }
            }
            return storelyPosition;
        }

        /// <summary>
        /// ��position��������߷�
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private int StorelyPoisition(int position)
        {
            int storelyPosition = -1;
            int errorChann = Element.ChannFWHM; //channFWHM / 2;
            int mindispeak = 0;//��ž�����̵ķ�
            for (int i = 0; i < peakPositions.Length; i++)
            {
                //if (data[peakPositions[i]] <= 0)
                //{
                //    continue;
                //}
                //else if (peakPositions[i] >= position - errorChann && peakPositions[i] <= position + errorChann)
                //{
                //    if (storelyPosition < 0)
                //    {
                //        storelyPosition = peakPositions[i];
                //    }
                //    else if (data[storelyPosition] < peakPositions[i])
                //    {
                //        storelyPosition = peakPositions[i];
                //    }
                //}
                if (Math.Abs(peakPositions[i] - position) < Math.Abs(mindispeak - position) && (data[peakPositions[i]] > 0))
                {
                    mindispeak = peakPositions[i];//����С����ķ�
                }
            }
            if (Math.Abs(mindispeak - position) <= errorChann && (data[mindispeak] > 0))
            {
                storelyPosition = mindispeak;
            }
            return storelyPosition;
        }

        /// <summary>
        /// ɾ��������ָ����ֵ
        /// </summary>
        /// <param name="a"></param>
        /// <param name="item"></param>
        /// <param name="n"></param>
        private void DeleteInem(string[] a, string item, int n)
        {
            {
                int i, j;
                for (i = 0; i < n; i++)
                {
                    if (a[i] == item)
                    {
                        for (j = i; j < n - 1; j++)
                            a[j] = a[j + 1];
                        n--;
                    }
                }
            }
        }

        /// <summary>
        /// �ҵ�ǿ������Ԫ��
        /// </summary>
        /// <returns></returns>
        private string StorelyElement(int position, out int index)
        {
            index = -1;
            int otherPosition = -1;
            string result = string.Empty;
            if (position < 0)        //�Ҳ����壬����
            {
                return result;
            }
            //�ж���ka��kb��la��lb   
            double positionEnergy = ToEnergy(position);
            string[] atomNames;
            string kaNames = Atoms.GetAtoms(positionEnergy, XLine.Ka);
            //if (kaNames == "")
            //    return null;
            if (kaNames.Length > 0)
            {
                atomNames = kaNames.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string atomName in atomNames)
                {

                    Atom atom = Atoms.AtomList.Find(s => s.AtomName == atomName);
                    if (atom.AtomID < 11)
                    {
                        DeleteInem(atomNames, atomName, atomNames.Length);
                        continue;
                    }
                    if (atom.AtomID > 56 && atom.AtomID < 72)
                    {
                        DeleteInem(atomNames, atomName, atomNames.Length);
                        continue;
                    }
                    if (atom.AtomID > 88)
                    {
                        DeleteInem(atomNames, atomName, atomNames.Length);
                        continue;
                    }
                    otherPosition = ToChannel(atom.Kb);
                    otherPosition = StorelyPoisition(otherPosition);
                    if (otherPosition > 0)
                    {
                        result = atom.AtomName;// +"," + XLine.Ka.ToString();
                        index = 0;
                        RemoveXLines(atom, XLine.Ka, position, otherPosition);
                        break;
                    }
                }
            }
            if (otherPosition < 0)
            {
                string kbNames = Atoms.GetAtoms(positionEnergy, XLine.Kb);
                atomNames = kbNames.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string atomName in atomNames)
                {
                    Atom atom = Atoms.AtomList.Find(s => s.AtomName == atomName);
                    if (atom.AtomID < 11)
                    {
                        DeleteInem(atomNames, atomName, atomNames.Length);
                        continue;
                    }
                    if (atom.AtomID > 56 && atom.AtomID < 72)
                    {
                        DeleteInem(atomNames, atomName, atomNames.Length);
                        continue;
                    }
                    if (atom.AtomID > 88)
                    {
                        DeleteInem(atomNames, atomName, atomNames.Length);
                        continue;
                    }
                    otherPosition = ToChannel(atom.Ka);
                    otherPosition = StorelyPoisition(otherPosition);
                    if (otherPosition > 0)
                    {
                        result = atom.AtomName;// +"," + XLine.Kb.ToString();
                        index = 0; ;
                        RemoveXLines(atom, XLine.Kb, otherPosition, position);
                        break;
                    }
                }
            }
            if (otherPosition < 0)
            {
                string LaNames = Atoms.GetAtoms(positionEnergy, XLine.La);
                atomNames = LaNames.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string atomName in atomNames)
                {
                    Atom atom = Atoms.AtomList.Find(s => s.AtomName == atomName);
                    if (atom.AtomID < 11)
                    {
                        DeleteInem(atomNames, atomName, atomNames.Length);
                        continue;
                    }
                    if (atom.AtomID > 56 && atom.AtomID < 72)
                    {
                        DeleteInem(atomNames, atomName, atomNames.Length);
                        continue;
                    }
                    if (atom.AtomID > 88)
                    {
                        DeleteInem(atomNames, atomName, atomNames.Length);
                        continue;
                    }
                    otherPosition = ToChannel(atom.Lb);
                    otherPosition = StorelyPoisition(otherPosition);
                    if (otherPosition > 0)
                    {
                        result = atom.AtomName;// +"," + XLine.La.ToString();
                        index = 1;
                        RemoveXLines(atom, XLine.La, position, otherPosition);
                        break;
                    }
                }
            }
            if (otherPosition < 0)
            {
                string LaNames = Atoms.GetAtoms(positionEnergy, XLine.Lb);
                atomNames = LaNames.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string atomName in atomNames)
                {
                    Atom atom = Atoms.AtomList.Find(s => s.AtomName == atomName);
                    if (atom.AtomID < 11)
                    {
                        DeleteInem(atomNames, atomName, atomNames.Length);
                        continue;
                    }
                    if (atom.AtomID > 56 && atom.AtomID < 72)
                    {
                        DeleteInem(atomNames, atomName, atomNames.Length);
                        continue;
                    }
                    if (atom.AtomID > 88)
                    {
                        DeleteInem(atomNames, atomName, atomNames.Length);
                        continue;
                    }
                    otherPosition = ToChannel(atom.La);
                    otherPosition = StorelyPoisition(otherPosition);
                    if (otherPosition > 0)
                    {
                        result = atom.AtomName;// +"," + XLine.Lb.ToString();
                        index = 1;
                        RemoveXLines(atom, XLine.Lb, otherPosition, position);
                        break;
                    }
                }
            }
            if (otherPosition > 0)
            {
                if (Element.AvoidElem.Contains(result))
                {
                    return string.Empty;
                }
                else
                {
                    return result;
                }
            }
            else//�����壬ɾ��
            {
                //data[position] = 0;
                data[position] /= 101;
                return string.Empty;
            }
        }

        /// <summary>
        /// ����Ԫ����������ı����۳�
        /// </summary>
        /// <param name="atom"></param>
        /// <param name="line1">����</param>       
        /// <param name="pa">a��λ��</param>
        /// <param name="pb">b��λ��</param>
        private void RemoveXLines(Atom atom, XLine line, int pa, int pb)
        {
            // return;
            //ElementSpectrum elemSpec = ElementSpectrumDataBase.FoctaryElementSpec(atom.AtomID);
            Double h1, h2;
            //if (IsIntelligence) //����ģʽ
            //{
            //    if (line == XLine.Ka || line == XLine.Kb)
            //    {
            //        h1 = elemSpec.KaHight;
            //        h2 = elemSpec.KbHight;
            //    }
            //    else
            //    {
            //        h1 = elemSpec.LaHight;
            //        h2 = elemSpec.LbHight;
            //    }
            //}
            //else //��������ģʽ
            //{
                if (line == XLine.Ka || line == XLine.Kb)
                {
                //    h1 = 15;
                //    h2 = 3;
                    h1 = 15;
                    h2 = 2;
                }
                else
                {
                    //h1 = 11;
                    //h2 = 8;
                    h1 = 10;
                    h2 = 8;
                }
            //}
            Double theoryRate = h2 / h1;
            if (data[pa] * theoryRate > data[pb])
            {
                data[pa] -= data[pb] / theoryRate;
                //data[pb] = 0;
                data[pb] /= 101;
            }
            else
            {
                data[pb] -= data[pa] * theoryRate;
               // data[pa] = 0;
                data[pa] /= 101;
            }
        }

        ///// <summary>
        ///// �Զ�����
        ///// </summary>
        public string[] Quale()
        {
            listInt.Clear();
            List<QualeElement> listQual =  QualeElement.FindAll();
            if (listQual == null || listQual.Count == 0)
                return null;
            Element = listQual[0];
            try
            {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml");
            XmlNode node = doc.SelectSingleNode("application/QualeParams/QualeAvoidElems");
            Element.AvoidElem = node != null ? node.InnerText : Element.AvoidElem;
            }
            catch
            {
            }
            //data = new double[spec.SpecDatas.Length];
            data = Helper.ToDoubles(spec.SpecDatas);
            peakPositions = Find(data, Element.ChannFWHM, Element.WindowWidth, Element.Trh1, Element.ValleyDistance, Element.AreaLimt);
            int temp = StorelyPoisition();
            if (temp < 0) return null;
            double maxPositionValue = data[temp];
            string findElem;
            int position = 0;
            string findElemList = string.Empty;
            do
            {
                position = StorelyPoisition();
                //������߷��1/50��Ͳ��ڷ�����
                if (data[position] < maxPositionValue * 1.0 / 80)
                {
                    break;
                }
                int index = -1;
                findElem = StorelyElement(position, out index);
                //if (findElem == null)
                //    continue;
                if (findElemList.IndexOf(findElem, 0, StringComparison.Ordinal) < 0)
                {
                    listInt.Add(index);
                    findElemList = findElemList + findElem + ";";
                }

            } while (position > 0);// while (position > 0);//(!findElem.Equals(string.Empty));
            string[] result = findElemList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            return result;
        }

        /// <summary>
        /// Ѱ���㷨
        /// </summary>
        /// <param name="SpecData">������</param>
        /// <param name="FWHM">��߿�</param>
        /// <param name="WindowWidth">����</param>
        /// <param name="TRH1">��������ֵ(normal 2-5)</param>
        /// <param name="PVDistance">��Ⱦ���ֵ</param>
        /// <param name="AreaLimt">�������ֵ��ƥ���˲��������ֵ�ľ����/ȫ�����</param>
        /// <returns></returns>
        /*public int[] Find(double[] SpecData, double FWHM, int WindowWidth, double TRH1, double PVDistance, double AreaLimt)
        {
            if (SpecData == null)
                return null;
            if (SpecData.Length < 2 * WindowWidth + 1)
                return null;
            double[] C = new double[2 * WindowWidth + 1];//ƥ���˲���ϵ��
            double sigma = FWHM / 2.355;
            double b = 0;
            for (int i = -WindowWidth; i <= WindowWidth; i++)
            {
                b += 1.0 / (2.0 * (double)WindowWidth + 1.0) * Math.Exp(-(double)i * (double)i / (2.0 * sigma * sigma));
            }
            for (int i = -WindowWidth; i <= WindowWidth; i++)
            {
                C[i + WindowWidth] = Math.Exp(-(double)i * (double)i / (2.0 * sigma * sigma)) - b;
            }
            double[] Y0 = new double[SpecData.Length];//��˹�����ƥ���˲����
            double[] dY0 = new double[SpecData.Length];
            double[] R = new double[SpecData.Length];
            double[] Rn = new double[SpecData.Length];//R 5��ƽ�����
            double[] dRn = new double[SpecData.Length - 1];//Rn һ�׵���
            double AreaAll = 0;//Rn��ȫ���
            for (int i = WindowWidth; i < SpecData.Length - WindowWidth; i++)
            {
                Y0[i] = 0;
                dY0[i] = 0;
                for (int j = -WindowWidth; j <= WindowWidth; j++)
                {
                    Y0[i] += C[j + WindowWidth] * SpecData[i + j];
                    dY0[i] += C[j + WindowWidth] * C[j + WindowWidth] * SpecData[i + j];
                }
                if (dY0[i] < 0)
                    dY0[i] = 0;
                dY0[i] = Math.Sqrt(dY0[i]);
                if (dY0[i] == 0)
                    R[i] = 0;
                else
                    R[i] = Y0[i] / dY0[i];
            }
            for (int i = 2; i < R.Length - 2; i++)
            {
                if (i >= 2 && i < R.Length - 2)
                    Rn[i] = (R[i - 2] + R[i - 1] + R[i] + R[i + 1] + R[i + 2]) / 5;
                else
                    Rn[i] = R[i];
            }
            for (int i = 0; i < Rn.Length; i++)
                AreaAll += Rn[i] > 0 ? Rn[i] : 0;
            for (int i = 0; i < dRn.Length - 1; i++)
                dRn[i] = Rn[i + 1] - Rn[i];
            int[] PeakPos = new int[SpecData.Length];
            int[] ValePos = new int[SpecData.Length];
            int PNum = 0, VNum = 0;
            for (int i = 0; i < dRn.Length - 1; i++)
            {
                if (dRn[i] > 0 && dRn[i + 1] <= 0)
                {
                    PeakPos[PNum] = i + 1;
                    PNum++;
                }
                if (dRn[i] < 0 && dRn[i + 1] >= 0)
                {
                    ValePos[VNum] = i + 1;
                    VNum++;
                }
            }
            double AreaN = 0;
            int[] nPeakPos = new int[PNum];
            int PeakNum = 0;
            for (int i = 0; i < PNum; i++)
            {
                if (Rn[PeakPos[i]] >= TRH1)
                {
                    nPeakPos[PeakNum] = PeakPos[i];
                    PeakNum++;
                    continue;
                }
                else
                {
                    if (PeakPos[0] > ValePos[0])
                    {
                        double PVDis = Rn[PeakPos[i]] - Rn[ValePos[i]];
                        if (i + 1 < VNum)
                        {
                            if (PVDis < Rn[PeakPos[i]] - Rn[ValePos[i + 1]])
                                PVDis = Rn[PeakPos[i]] - Rn[ValePos[i + 1]];
                        }
                        if (PVDis < PVDistance)
                            continue;
                        else
                        {
                            int startpos = ValePos[i];
                            int endpos = i + 1 < VNum ? ValePos[i + 1] : Rn.Length - 1;
                            for (int j = startpos; j <= endpos; j++)
                            {
                                if (Rn[j] > 0)
                                    AreaN += Rn[j];
                            }
                            if (AreaN >= AreaLimt * Math.Sqrt(AreaAll))
                            {
                                nPeakPos[PeakNum] = PeakPos[i];
                                PeakNum++;
                            }
                        }
                    }
                    else
                    {
                        double PVDis = Rn[PeakPos[i]] - Rn[ValePos[i]];
                        if (i - 1 >= 0)
                        {
                            if (PVDis < Rn[PeakPos[i]] - Rn[ValePos[i - 1]])
                                PVDis = Rn[PeakPos[i]] - Rn[ValePos[i - 1]];
                        }
                        if (PVDis < PVDistance)
                            continue;
                        else
                        {
                            int startpos = i - 1 >= 0 ? ValePos[i - 1] : 0;
                            int endpos = ValePos[i];
                            for (int j = startpos; j <= endpos; j++)
                            {
                                if (Rn[j] > 0)
                                    AreaN += Rn[j];
                            }
                            if (AreaN >= AreaLimt * Math.Sqrt(AreaAll))
                            {
                                nPeakPos[PeakNum] = PeakPos[i];
                                PeakNum++;
                            }
                        }
                    }
                }
            }
            int[] PeakPostions = new int[PeakNum];
            for (int i = 0; i < PeakPostions.Length; i++)
                PeakPostions[i] = nPeakPos[i];
            return PeakPostions;
        }*/
        /// <summary>
        /// Ѱ���㷨
        /// </summary>
        /// <param name="SpecData">������</param>
        /// <param name="FWHM">��߿�</param>
        /// <param name="WindowWidth">����</param>
        /// <param name="TRH1">��������ֵ(normal 2-5)</param>
        /// <param name="PVDistance">��Ⱦ���ֵ</param>
        /// <param name="AreaLimt">�������ֵ��ƥ���˲��������ֵ�ľ����/ȫ�����</param>
        /// <returns></returns>
        public static int[] Find(double[] SpecData, double FWHM, int WindowWidth, double TRH1, double PVDistance, double AreaLimt)
        {
            if (SpecData == null)
                return null;
            if (SpecData.Length < 2 * WindowWidth + 1)
                return null;
            double[] C = new double[2 * WindowWidth + 1];//ƥ���˲���ϵ��
            double sigma = FWHM / 2.355;
            double b = 0;
            for (int i = -WindowWidth; i <= WindowWidth; i++)
            {
                b += 1.0 / (2.0 * (double)WindowWidth + 1.0) * Math.Exp(-(double)i * (double)i / (2.0 * sigma * sigma));
            }
            for (int i = -WindowWidth; i <= WindowWidth; i++)
            {
                C[i + WindowWidth] = Math.Exp(-(double)i * (double)i / (2.0 * sigma * sigma)) - b;
            }
            //double[] Y0 = new double[SpecData.Length];//��˹�����ƥ���˲����
            //double[] dY0 = new double[SpecData.Length];
            double[] R = new double[SpecData.Length];
            double[] Rn = new double[SpecData.Length];//R 5��ƽ�����
            //double[] dRn = new double[SpecData.Length - 1];//Rn һ�׵���
            double AreaAll = 0;//Rn��ȫ���
            double y0 = 0.0;
            double dy0 = 0.0;
            for (int i = WindowWidth; i < SpecData.Length - WindowWidth; i++)
            {
                y0 = 0.0;
                dy0 = 0.0;
                for (int j = -WindowWidth; j <= WindowWidth; j++)
                {
                    y0 += C[j + WindowWidth] * SpecData[i + j];
                    dy0 += C[j + WindowWidth] * C[j + WindowWidth] * SpecData[i + j];
                }
                if (dy0 < 0)
                    dy0 = 0;
                dy0 = Math.Sqrt(dy0);
                if (dy0 == 0)
                    R[i] = 0;
                else
                    R[i] = y0 / dy0;
            }
            for (int i = 2; i < R.Length - 2; i++)
            {
                if (i >= 2 && i < R.Length - 2)
                    Rn[i] = (R[i - 2] + R[i - 1] + R[i] + R[i + 1] + R[i + 2]) / 5;
                else
                    Rn[i] = R[i];
            }
            //���涼û����R[],������R[]����dR[]
            for (int i = 0; i < Rn.Length; i++)
                AreaAll += Rn[i] > 0 ? Rn[i] : 0;
            for (int i = 0; i < R.Length - 2; i++)
                R[i] = Rn[i + 1] - Rn[i];//��R[]��Ϊһ�׵��洢��ֻ��ǰLength - 2������

            int[] PeakPos = new int[SpecData.Length];
            int[] ValePos = new int[SpecData.Length];
            int PNum = 0, VNum = 0;
            for (int i = 0; i < R.Length - 1; i++)
            {
                if (R[i] > 0 && R[i + 1] <= 0)
                {
                    PeakPos[PNum] = i + 1;
                    PNum++;
                }
                if (R[i] < 0 && R[i + 1] >= 0)
                {
                    ValePos[VNum] = i + 1;
                    VNum++;
                }
            }
            double AreaN = 0;
            int[] nPeakPos = new int[PNum];
            int PeakNum = 0;
            for (int i = 0; i < PNum; i++)
            {
                if (Rn[PeakPos[i]] >= TRH1)
                {
                    nPeakPos[PeakNum] = PeakPos[i];
                    PeakNum++;
                    continue;
                }
                else
                {
                    if (PeakPos[0] > ValePos[0])
                    {
                        double PVDis = Rn[PeakPos[i]] - Rn[ValePos[i]];
                        if (i + 1 < VNum)
                        {
                            if (PVDis < Rn[PeakPos[i]] - Rn[ValePos[i + 1]])
                                PVDis = Rn[PeakPos[i]] - Rn[ValePos[i + 1]];
                        }
                        if (PVDis < PVDistance)
                            continue;
                        else
                        {
                            int startpos = ValePos[i];
                            int endpos = i + 1 < VNum ? ValePos[i + 1] : Rn.Length - 1;
                            for (int j = startpos; j <= endpos; j++)
                            {
                                if (Rn[j] > 0)
                                    AreaN += Rn[j];
                            }
                            if (AreaN >= AreaLimt * Math.Sqrt(AreaAll))
                            {
                                nPeakPos[PeakNum] = PeakPos[i];
                                PeakNum++;
                            }
                        }
                    }
                    else
                    {
                        double PVDis = Rn[PeakPos[i]] - Rn[ValePos[i]];
                        if (i - 1 >= 0)
                        {
                            if (PVDis < Rn[PeakPos[i]] - Rn[ValePos[i - 1]])
                                PVDis = Rn[PeakPos[i]] - Rn[ValePos[i - 1]];
                        }
                        if (PVDis < PVDistance)
                            continue;
                        else
                        {
                            int startpos = i - 1 >= 0 ? ValePos[i - 1] : 0;
                            int endpos = ValePos[i];
                            for (int j = startpos; j <= endpos; j++)
                            {
                                if (Rn[j] > 0)
                                    AreaN += Rn[j];
                            }
                            if (AreaN >= AreaLimt * Math.Sqrt(AreaAll))
                            {
                                nPeakPos[PeakNum] = PeakPos[i];
                                PeakNum++;
                            }
                        }
                    }
                }
            }
            int[] PeakPostions = new int[PeakNum];
            for (int i = 0; i < PeakPostions.Length; i++)
                PeakPostions[i] = nPeakPos[i];
            return PeakPostions;
        }
        /// <summary>
        /// ��������
        /// </summary>
        public SpecEntity Spec
        {
            get
            {
                return spec;
            }

            set
            {
                spec = value;
            }

        }


        //2013-05-20 chuyaqin ׷����Ԫ��ƥ��
        /// <summary>
        /// ȡ�׵�ǰ����Ԫ���Լ����ǿ�ȸ߶�
        /// </summary>
        /// <param name="dblPeakHeights">���ر��ҵ�������Ԫ�ص����ǿ��</param>
        /// <param name="spec1">����������</param>
        /// <param name="demarcateEnergy">ɨ�׵�ʱ�������̶�</param>
        /// <param name="bIsHighSubstrate">�Ƿ��Ǹ߱���</param>
        /// <returns></returns>
        public List<string> GetSpectrumThreeElements(ref List<double> dblPeakHeights, SpecEntity spec1, List<DemarcateEnergy> demarcateEnergy, bool bIsHighSubstrate)
        {
            List<QualeElement> listQual = QualeElement.FindAll();
            if (listQual == null || listQual.Count == 0)
                return null;
            Element = listQual[0];
            int[] intSpec = Helper.ToInts(spec1.SpecData);
            intSpec = Helper.Smooth(intSpec, 3);
            double[] datasLater = new double[intSpec.Length];
            for (int i = 0; i < intSpec.Length; i++)
            {
                if (bIsHighSubstrate && (i < WorkCurveHelper.MatchHighSubstrateLeft || i > WorkCurveHelper.MatchHighSubstrateRight))
                {
                    datasLater[i] = 0;
                    continue;
                }
                datasLater[i] = intSpec[i];
            }
            //ȥ�ҷ�
            peakPositions = Find(datasLater, Element.ChannFWHM, Element.WindowWidth, Element.Trh1, Element.ValleyDistance, Element.AreaLimt);

            dblPeakHeights.Clear();
            List<string> strElements = new List<string>();

            //ȥ��Ԫ��
            int countElem = MatchElementPeakPos(ref dblPeakHeights, ref strElements, datasLater, peakPositions.ToList(), Element.ChannFWHM, demarcateEnergy, bIsHighSubstrate);

            //����
            for (int count = 0; count < dblPeakHeights.Count; count++)
            {
                double max = dblPeakHeights[count];
                int maxi = count;
                for (int count1 = count + 1; count1 < dblPeakHeights.Count; count1++)
                {
                    if (dblPeakHeights[count1] > max)
                    {
                        max = dblPeakHeights[count1];
                        maxi = count1;
                    }
                }
                string temp = strElements[maxi];
                dblPeakHeights[maxi] = dblPeakHeights[count];
                strElements[maxi] = strElements[count];
                dblPeakHeights[count] = max;
                strElements[count] = temp;
            }
            //��ҪԪ�ص�ɸѡ
            //double MinorContent = 0;
            //for (int count = 0; count < dblPeakHeights.Count; count++)
            //{
            //    dblPeakHeights[count] /= dblPeakHeights[0];
            //    if (count != 0)
            //    {
            //        MinorContent += dblPeakHeights[count];
            //    }
            //}

            //if (dblPeakHeights.Count > 1 && MinorContent / dblPeakHeights[0] < WorkCurveHelper.MatchMinorElemRatio && !bIsHighSubstrate)
            //{
            //    for (int count = dblPeakHeights.Count - 1; count > 0; count--)
            //    {
            //        dblPeakHeights.RemoveAt(count);
            //        strElements.RemoveAt(count);
            //    }
            //}
            return strElements;
        }

        /// <summary>
        /// �ж��Ƿ��Ǹ߱���
        /// </summary>
        /// <param name="demarcateEnergy">ɨ�׵�ʱ�������̶�</param>
        /// <param name="spec">����������</param>
        /// <returns></returns>
        public bool IsHaveBG(List<DemarcateEnergy> demarcateEnergy, SpecEntity spec)
        {

            int[] intSpec = Helper.ToInts(spec.SpecData);
            intSpec = Helper.Smooth(intSpec, 3);
            #region fp��̬��۱���
            float[] datasOrgin = new float[intSpec.Length];
            for (int i = 0; i < intSpec.Length; i++)
            {
                datasOrgin[i] = intSpec[i];
            }

            float[] datas = new float[spec.SpecDatas.Length];
            datasOrgin.CopyTo(datas, 0);
            SpecProcess.RemoveBase(datas, intSpec.Length, 1, 1, (float)DemarcateEnergyHelp.k1);
            float sumdata = 0;
            float sumtotal = 0;
            for (int i = 0; i < intSpec.Length; i++)
            {
                sumdata += datas[i];
                sumtotal += intSpec[i];
            }
            if (sumdata / sumtotal < (1 - WorkCurveHelper.MatchBaseRatio))
                return true;
            else return false;
            #endregion
        }


        /// <summary>
        ///  �ҵ���Ԫ��
        /// </summary>
        /// <param name="dblPeakHeights">���ر��ҵ�������Ԫ�ص����ǿ��</param>
        /// <param name="strElements"></param>
        /// <param name="data">��������������</param>
        /// <param name="peakPositions">�ҵ����׵����еķ�</param>
        /// <param name="channelFWHM"></param>
        /// <param name="demarcateEnergy"></param>
        /// <param name="bIsHighSubstrate">�Ƿ��Ǹ߱���</param>
        /// <returns></returns>
        private int MatchElementPeakPos(ref List<double> dblPeakHeights, ref List<string> strElements, double[] data, List<int> peakPositions, int channelFWHM, List<DemarcateEnergy> demarcateEnergy, bool bIsHighSubstrate)
        {
            //����Ԫ��
            double MaxPeakValue = 0;
            bool bFirstPeakFind = false;
            bool bSecondPeakFind = false;
            bool bThirdPeakFind = false;
            const int OFFSET_CHANNEL = 6;
            //int firstPeak = 0, secondPeak = 0;
           // double[] dblCoefs = { 0, 0, 0 };
            double[] dblCoefs = new double[100];
            for (int elementCount = 0; elementCount < 100; elementCount++)
            {
                bool bPeakFind = false;
                int peakCount = peakPositions.Count;
                if (peakCount <= 0)
                    break;
                int maxPaakChannel = 0;
                for (int i = 0; i < peakCount; i++)
                {
                    int peakTemp = peakPositions[i];
                    if (data[peakTemp] > data[maxPaakChannel])
                    {
                        maxPaakChannel = peakTemp;
                    }
                }
                if (elementCount == 0)
                    MaxPeakValue = data[maxPaakChannel];
                int PeakChannel = maxPaakChannel;
                dblCoefs[elementCount] = data[maxPaakChannel] / MaxPeakValue;
                for (int nCount = OFFSET_CHANNEL; nCount <= OFFSET_CHANNEL && !bPeakFind; nCount++)
                {
                    int AtomsCount = Atoms.AtomList.Count;
                    for (int j = 0; j < AtomsCount && !bPeakFind; j++)
                    {
                        int AtomID = Atoms.AtomList[j].AtomID;
                        if (AtomID < 11 || AtomID == 18 || AtomID == 36 || AtomID == 54 || AtomID == 86 || AtomID >= 89 || (AtomID >= 57 && AtomID <= 71))
                        {
                            continue;
                        }
                        //kϵ
                        int iKaChannel = ToChannel2(Atoms.AtomList[j].Ka, demarcateEnergy);
                        if (Math.Abs(iKaChannel - PeakChannel) <= nCount)
                        {
                            int iKbChannel = ToChannel2(Atoms.AtomList[j].Kb, demarcateEnergy);
                            double PeakChannelOffset2 = nCount * Atoms.AtomList[j].Kb / Atoms.AtomList[j].Ka;
                            int mindispeak = 0;
                            for (int k = 0; k < peakCount; k++)
                            {
                                if (Math.Abs(mindispeak - iKbChannel) > Math.Abs(iKbChannel - peakPositions[k]))
                                {
                                    mindispeak = peakPositions[k];//����С����ķ�
                                }
                            }
                            if (Math.Abs(iKbChannel - mindispeak) <= PeakChannelOffset2 + 15 && (data[mindispeak] != 0))
                            {
                                iKbChannel = mindispeak; //ͨ�������ka/kb=10;
                                if (data[PeakChannel] < 10 * data[iKbChannel])
                                {
                                    data[iKbChannel] -= data[PeakChannel] / 10;
                                    //data[PeakChannel] = 0;//��Ϊ��߷��1/31
                                    data[PeakChannel] /= 31;
                                    peakPositions.Remove(PeakChannel);
                                    peakCount--;
                                }
                                else
                                {
                                    data[PeakChannel] -= 10 * data[iKbChannel];
                                    data[iKbChannel] /= 31;
                                    peakPositions.Remove(iKbChannel);
                                    peakCount--;
                                }
                                bPeakFind = true;
                                if (strElements.Contains(Atoms.AtomList[j].AtomName))
                                {
                                    elementCount--;
                                }
                                else
                                {
                                    dblCoefs[elementCount] *= WorkCurveHelper.MatchBaseKRatio;
                                    strElements.Add(Atoms.AtomList[j].AtomName);
                                }
                            }
                        }
                        //if (bPeakFind)
                        //{
                        //    continue;
                        //}
                        ////Kbϵ
                        //iKaChannel = ToChannel2(Atoms.AtomList[j].Kb, demarcateEnergy);
                        //if (Math.Abs(iKaChannel - PeakChannel) <= nCount)//+10
                        //{
                        //    int iKbChannel = ToChannel2(Atoms.AtomList[j].Ka, demarcateEnergy);
                        //    double PeakChannelOffset2 = nCount / (Atoms.AtomList[j].Kb / Atoms.AtomList[j].Ka);
                        //    int mindispeak = 0;
                        //    for (int k = 0; k < peakCount; k++)
                        //    {
                        //        if (Math.Abs(mindispeak - iKbChannel) > Math.Abs(iKbChannel - peakPositions[k]))
                        //        {
                        //            mindispeak = peakPositions[k];//����С����ķ�
                        //        }
                        //    }
                        //    if (Math.Abs(iKbChannel - mindispeak) <= PeakChannelOffset2 && (data[mindispeak] != 0))
                        //    {
                        //        iKbChannel = mindispeak; //ͨ�������ka/kb=10;
                        //        if (data[PeakChannel] * 10 > data[iKbChannel])
                        //        {

                        //            data[PeakChannel] -= data[iKbChannel] / 10;
                        //            data[iKbChannel] /= 31;
                        //        }
                        //        else
                        //        {
                        //            data[iKaChannel] -= 10 * data[PeakChannel];
                        //            data[PeakChannel] /= 31;
                        //        }
                        //        bPeakFind = true;
                        //        if (strElements.Contains(Atoms.AtomList[j].AtomName))
                        //        {
                        //            elementCount--;
                        //            break; ;
                        //        }
                        //        strElements.Add(Atoms.AtomList[j].AtomName);
                        //        break;
                        //    }

                        //}
                        if (bPeakFind || j < 39 || bIsHighSubstrate)
                        {
                            continue;
                        }
                        //Lϵ
                        int iLaChannel = ToChannel2(Atoms.AtomList[j].La, demarcateEnergy);
                        if (Math.Abs(iLaChannel - PeakChannel) <= nCount)
                        {
                            int iLbChannel = ToChannel2(Atoms.AtomList[j].Lb, demarcateEnergy);
                            double PeakChannelOffset2 = nCount * Atoms.AtomList[j].Lb / Atoms.AtomList[j].La;
                            int mindispeak = 0;
                            for (int k = 0; k < peakCount; k++)
                            {
                                if (Math.Abs(mindispeak - iLbChannel) > Math.Abs(iLbChannel - peakPositions[k]))
                                {
                                    mindispeak = peakPositions[k];//����С����ķ�
                                }
                            }
                            if (Math.Abs(iLbChannel - mindispeak) <= PeakChannelOffset2 + 15
                                && (data[mindispeak] != 0)
                                && data[mindispeak] / data[PeakChannel] > 0.5)
                            {
                                iLbChannel = mindispeak; //ͨ�������La/Lb=1;
                                if (data[PeakChannel] < data[iLbChannel])
                                {
                                    data[iLbChannel] -= data[PeakChannel];
                                    data[PeakChannel] /= 31;
                                    peakPositions.Remove(PeakChannel);
                                    peakCount--;
                                }
                                else
                                {
                                    data[PeakChannel] -= data[iLbChannel];
                                    data[iLbChannel] /= 31;
                                    peakPositions.Remove(iLbChannel);
                                    peakCount--;
                                }
                                bPeakFind = true;
                                if (j < 52 || strElements.Contains(Atoms.AtomList[j].AtomName))
                                {
                                    elementCount--;
                                }
                                else
                                {
                                    strElements.Add(Atoms.AtomList[j].AtomName);
                                    dblCoefs[elementCount] *= WorkCurveHelper.MatchBaseLRatio;
                                }
                            }

                        }
                        if (bPeakFind)
                        {
                            continue;
                        }
                        iLaChannel = ToChannel2(Atoms.AtomList[j].Lb, demarcateEnergy);
                        if (Math.Abs(iLaChannel - PeakChannel) <= nCount)//+ 15
                        {
                            int iLbChannel = ToChannel2(Atoms.AtomList[j].La, demarcateEnergy);
                            double PeakChannelOffset2 = (nCount) / (Atoms.AtomList[j].Lb / Atoms.AtomList[j].La);
                            int mindispeak = 0;
                            for (int k = 0; k < peakCount; k++)
                            {
                                if (Math.Abs(mindispeak - iLbChannel) > Math.Abs(iLbChannel - peakPositions[k]))
                                {
                                    mindispeak = peakPositions[k];//����С����ķ�
                                }
                            }
                            if (Math.Abs(iLbChannel - mindispeak) <= PeakChannelOffset2
                                && (data[mindispeak] != 0)
                                && data[mindispeak] / data[PeakChannel] > 0.5)
                            {
                                iLbChannel = mindispeak; //ͨ�������La/Lb=1;
                                if (data[PeakChannel] < data[iLbChannel])
                                {
                                    data[iLbChannel] -= data[PeakChannel];
                                    data[PeakChannel] /= 31;
                                    peakPositions.Remove(PeakChannel);
                                    peakCount--;
                                }
                                else
                                {
                                    data[PeakChannel] -= data[iLbChannel];
                                    data[iLbChannel] /= 31;
                                    peakPositions.Remove(iLbChannel);
                                    peakCount--;
                                }
                                bPeakFind = true;
                                if (j < 52 || strElements.Contains(Atoms.AtomList[j].AtomName))
                                {
                                    elementCount--;
                                }
                                else
                                {
                                    dblCoefs[elementCount] *= WorkCurveHelper.MatchBaseLRatio;
                                    strElements.Add(Atoms.AtomList[j].AtomName);

                                }
                            }
                        }

                    }
                }
                switch (elementCount)
                {
                    case 0:
                        bFirstPeakFind = bPeakFind;
                        break;
                    case 1:
                        bSecondPeakFind = bPeakFind;
                        break; ;
                    case 2:
                        bThirdPeakFind = bPeakFind;
                        break;
                }
                if (!bPeakFind)
                {
                    //data[PeakChannel] = 0;
                    data[PeakChannel] /= 31;
                    peakPositions.Remove(PeakChannel);
                    peakCount--;
                    elementCount--;
                }
                bool bContinue = false;
                for (int peaki = 0; peaki < peakCount; peaki++)
                {
                    int peakTmp = peakPositions[peaki];
                    if (data[peakTmp] > MaxPeakValue / 30)
                    {
                        bContinue = true;
                        break;
                    }
                }
                if (!bContinue)
                {
                    break;
                }
            }
            for (int i = 0; i < strElements.Count; i++)
            {
                dblPeakHeights.Add(dblCoefs[i]);
            }
            return strElements.Count;

        }

        //public List<ElementSpectrum> ConstructElementSpectrum(List<CurveElement> CurveElements, List<ElementSpectrum> ElementSpectrums)
        //{
        //    List<ElementSpectrum> res = new List<ElementSpectrum>();

        //    //  �궨Ԫ�ؾ�����

        //    if (ElementSpectrums.Count == 2)
        //    {
        //        //  ��������б��
        //        double[] HFW_K = new double[4];
        //        HFW_K[0] = (ElementSpectrums.ElementAtOrDefault(0).KaFwhm - ElementSpectrums.ElementAtOrDefault(1).KaFwhm) /
        //            (ElementSpectrums.ElementAtOrDefault(0).Id - ElementSpectrums.ElementAtOrDefault(1).Id);
        //        HFW_K[1] = (ElementSpectrums.ElementAtOrDefault(0).KbFwhm - ElementSpectrums.ElementAtOrDefault(1).KbFwhm) /
        //            (ElementSpectrums.ElementAtOrDefault(0).Id - ElementSpectrums.ElementAtOrDefault(1).Id);
        //        HFW_K[2] = (ElementSpectrums.ElementAtOrDefault(0).LaFwhm - ElementSpectrums.ElementAtOrDefault(1).LaFwhm) /
        //            (ElementSpectrums.ElementAtOrDefault(0).Id - ElementSpectrums.ElementAtOrDefault(1).Id);
        //        HFW_K[3] = (ElementSpectrums.ElementAtOrDefault(0).LbFwhm - ElementSpectrums.ElementAtOrDefault(1).LbFwhm) /
        //            (ElementSpectrums.ElementAtOrDefault(0).Id - ElementSpectrums.ElementAtOrDefault(1).Id);


        //        //  �������б��
        //        double[] FH_K = new double[4];
        //        FH_K[0] = (ElementSpectrums.ElementAtOrDefault(0).KaHight - ElementSpectrums.ElementAtOrDefault(1).KaHight) /
        //            (ElementSpectrums.ElementAtOrDefault(0).Id - ElementSpectrums.ElementAtOrDefault(1).Id);
        //        FH_K[1] = (ElementSpectrums.ElementAtOrDefault(0).KbHight - ElementSpectrums.ElementAtOrDefault(1).KbHight) /
        //            (ElementSpectrums.ElementAtOrDefault(0).Id - ElementSpectrums.ElementAtOrDefault(1).Id);
        //        FH_K[2] = (ElementSpectrums.ElementAtOrDefault(0).LaHight - ElementSpectrums.ElementAtOrDefault(1).LaHight) /
        //            (ElementSpectrums.ElementAtOrDefault(0).Id - ElementSpectrums.ElementAtOrDefault(1).Id);
        //        FH_K[3] = (ElementSpectrums.ElementAtOrDefault(0).LbHight - ElementSpectrums.ElementAtOrDefault(1).LbHight) /
        //            (ElementSpectrums.ElementAtOrDefault(0).Id - ElementSpectrums.ElementAtOrDefault(1).Id);


        //        foreach (var item in ElementSpectrums)
        //        {
        //            ElementSpectrum temp = ElementSpectrum.New;
        //            temp.KaFwhm = ElementSpectrums.ElementAtOrDefault(0).KaFwhm +
        //                HFW_K[0] * (item.Id - ElementSpectrums.ElementAtOrDefault(0).Id);
        //            temp.KbFwhm = ElementSpectrums.ElementAtOrDefault(0).KbFwhm +
        //                HFW_K[1] * (item.Id - ElementSpectrums.ElementAtOrDefault(0).Id);
        //            temp.LaFwhm = ElementSpectrums.ElementAtOrDefault(0).LaFwhm +
        //                HFW_K[2] * (item.Id - ElementSpectrums.ElementAtOrDefault(0).Id);
        //            temp.LbFwhm = ElementSpectrums.ElementAtOrDefault(0).LbFwhm +
        //                HFW_K[3] * (item.Id - ElementSpectrums.ElementAtOrDefault(0).Id);

        //            temp.KaHight = ElementSpectrums.ElementAtOrDefault(0).KaFwhm +
        //                HFW_K[0] * (item.Id - ElementSpectrums.ElementAtOrDefault(0).Id);
        //            temp.KbHight = ElementSpectrums.ElementAtOrDefault(0).KbHight +
        //                HFW_K[1] * (item.Id - ElementSpectrums.ElementAtOrDefault(0).Id);
        //            temp.LaHight = ElementSpectrums.ElementAtOrDefault(0).LaHight +
        //                HFW_K[2] * (item.Id - ElementSpectrums.ElementAtOrDefault(0).Id);
        //            temp.LbHight = ElementSpectrums.ElementAtOrDefault(0).LbHight +
        //                HFW_K[3] * (item.Id - ElementSpectrums.ElementAtOrDefault(0).Id);
        //            temp.KaIntensity = temp.KaFwhm * temp.KaHight * 2.506638 / 2.35482;

        //            res.Add(temp);
        //        }
        //        return res;
        //    }

        //    if (ElementSpectrums.Count > 2)
        //    {
        //        List<int> CareAtomicNumber = new List<int>();
        //        foreach (var item in CurveElements)
        //        {
        //            var id = item.AtomicNumber;
        //            CareAtomicNumber.Add(id);
        //        }
        //        List<int> KnownAtomicNumber = new List<int>();
        //        foreach (var item in KnownAtomicNumber)
        //        {
        //            var id = item.AtomicNumber;
        //            KnownAtomicNumber.Add(id);
        //        }
        //        foreach (var ind in CareAtomicNumber)
        //        {
        //            var AtomicNumber_1 = ind;
        //            var AtomicNumber_2 = ind;

        //            //  ��֪��ԭ�ӵ��������

        //            for (int i = 0; i < KnownAtomicNumber.Count; ++i)
        //            {
        //                for (int j = i; j < KnownAtomicNumber.Count; ++j)
        //                {
        //                    if (KnownAtomicNumber.ElementAt(i) > KnownAtomicNumber.ElementAt(j))
        //                    {
        //                        var temp = KnownAtomicNumber.ElementAt(j);
        //                        KnownAtomicNumber.Insert(j, KnownAtomicNumber.ElementAt(i));
        //                        KnownAtomicNumber.RemoveAt(j + 1);
        //                        KnownAtomicNumber.Insert(i, temp);
        //                        KnownAtomicNumber.RemoveAt(i + 1);
        //                    }
        //                }
        //            }

        //            //  �ҳ����ԭ�����������ԭ�����

        //            for (int k = 1; k < KnownAtomicNumber.Count; ++k)
        //            {
        //                if (ind >= KnownAtomicNumber.ElementAt(k - 1) && ind < KnownAtomicNumber.ElementAt(k))
        //                {
        //                    AtomicNumber_1 = KnownAtomicNumber.ElementAt(k - 1);
        //                    AtomicNumber_2 = KnownAtomicNumber.ElementAt(k);
        //                    break;
        //                }
        //            }

        //            //  ��������б��

        //            double[] HFW_K = new double[4];
        //            HFW_K[0] = (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).KaFwhm - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).KaFwhm) /
        //                (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).Id);
        //            HFW_K[1] = (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).KbFwhm - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).KbFwhm) /
        //                (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).Id);
        //            HFW_K[2] = (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).LaFwhm - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).LaFwhm) /
        //                (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).Id);
        //            HFW_K[3] = (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).LbFwhm - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).LbFwhm) /
        //                (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).Id);


        //            //  �������б��

        //            double[] FH_K = new double[4];
        //            FH_K[0] = (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).KaHight - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).KaHight) /
        //                (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).Id);
        //            FH_K[1] = (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).KbHight - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).KbHight) /
        //                (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).Id);
        //            FH_K[2] = (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).LaHight - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).LaHight) /
        //                (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).Id);
        //            FH_K[3] = (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).LbHight - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).LbHight) /
        //                (ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_2).Id);


        //            foreach (var item in ElementSpectrums)
        //            {
        //                ElementSpectrum temp = ElementSpectrum.New;
        //                temp.KaFwhm = ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).KaFwhm +
        //                    HFW_K[0] * (item.Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id);
        //                temp.KbFwhm = ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).KbFwhm +
        //                    HFW_K[1] * (item.Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id);
        //                temp.LaFwhm = ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).LaFwhm +
        //                    HFW_K[2] * (item.Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id);
        //                temp.LbFwhm = ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).LbFwhm +
        //                    HFW_K[3] * (item.Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id);

        //                temp.KaHight = ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).KaFwhm +
        //                    HFW_K[0] * (item.Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id);
        //                temp.KbHight = ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).KbHight +
        //                    HFW_K[1] * (item.Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id);
        //                temp.LaHight = ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).LaHight +
        //                    HFW_K[2] * (item.Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id);
        //                temp.LbHight = ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).LbHight +
        //                    HFW_K[3] * (item.Id - ElementSpectrums.ElementAtOrDefault(AtomicNumber_1).Id);
        //                temp.KaIntensity = temp.KaFwhm * temp.KaHight * 2.506638 / 2.35482;

        //                res.Add(temp);
        //            }
        //        }
        //        return res;
        //    }
        //}
    }
}
