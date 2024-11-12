using System;
using System.Collections.Generic;
using System.Text;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 元素谱数据库
    /// </summary>
    public class ElementSpectrumDataBase
    {
        public static double coef = Math.Sqrt(Math.PI / Math.Log(2)) / 2;
        public ElementSpectrumDataBase()
        { }
        /// <summary>
        /// 获取一个大于ElementID号
        /// </summary>
        /// <param name="elementID">元素编号</param>
        /// <returns></returns>
        private ElementSpectrum QueryNext(int elementID)
        {
            IList<ElementSpectrum> result = ElementSpectrum.Find(w => w.ElementID > elementID);
            ElementSpectrum item = ElementSpectrum.New;
            if (result.Count > 0)
            {
                item = result[0];
            }
            for (int i = 1; i < result.Count; i++)
            {
                if (result[i].ElementID < item.ElementID)
                {
                    item = result[i];
                }
            }
            return item;
        }

        /// <summary>
        /// 获取一个小于ElementID号
        /// </summary>
        /// <param name="elementID">元素编号</param>
        /// <returns></returns>
        private ElementSpectrum QueryPrior(int elementID)
        {
            IList<ElementSpectrum> result = ElementSpectrum.Find(w=>w.ElementID <= elementID);
            ElementSpectrum item = ElementSpectrum.New;
            if (result.Count > 0)
            {
                item = result[0];
            }
            for (int i = 1; i < result.Count; i++)
            {
                if (result[i].ElementID > item.ElementID)
                {
                    item = result[i];
                }
            }
            return item;
        }

        /// <summary>
        /// 如果不存在，构造一个元素谱
        /// </summary>
        /// <param name="elementID"></param>
        /// <returns></returns>
        public ElementSpectrum FoctaryElementSpec(int elementID)
        {
            ElementSpectrum item = ElementSpectrum.FindOne(w=>w.ElementID==elementID);
            if (item != null && item.ElementID > 0)
            {
                return item;
            }
            else
            {
                item = ElementSpectrum.New;
                ElementSpectrum nextItem = QueryNext(elementID + 1);
                ElementSpectrum priorItem = QueryPrior(elementID - 1);
                ElementSpectrum tempItem = ElementSpectrum.New;
                //2011-06-17修改元素序号不为0 chuyaqin
                if (nextItem.ElementID <= 0 && priorItem.ElementID <= 0)
                {
                    return null;
                }
                if (nextItem.ElementID < 0)
                {
                    nextItem = QueryPrior(priorItem.ElementID - 1);
                    if (nextItem.ElementID < 0)
                    {
                        return item;
                    }
                }
                if (priorItem.ElementID < 0)
                {
                    priorItem = QueryNext(nextItem.ElementID + 1);
                    if (priorItem.ElementID < 0)
                    {
                        return item;
                    }
                }
                if ((priorItem.ElementID - elementID) * (nextItem.ElementID - elementID) < 0)
                {
                    if ((elementID - priorItem.ElementID) < (nextItem.ElementID - elementID))
                    {
                        tempItem = QueryPrior(priorItem.ElementID - 1);
                        if (tempItem.ElementID > 0)
                        {
                            if ((elementID - tempItem.ElementID) < (nextItem.ElementID - elementID))
                            {
                                nextItem = tempItem.Clone() as ElementSpectrum;
                            }
                        }
                    }
                    if ((elementID - priorItem.ElementID) > (nextItem.ElementID - elementID))
                    {
                        tempItem = QueryNext(nextItem.ElementID + 1);
                        if (tempItem.ElementID > 0)
                        {
                            if ((elementID - priorItem.ElementID) > (tempItem.ElementID - elementID))
                            {
                                priorItem = tempItem.Clone() as ElementSpectrum;
                            }
                        }
                    }
                }

                //开始构造
                item.ElementID = elementID;
                //item.IsCoeff = false;
                double temp = 1.0 * (item.ElementID - nextItem.ElementID) / (nextItem.ElementID - priorItem.ElementID);
                item.KaFwhm = nextItem.KaFwhm + (nextItem.KaFwhm - priorItem.KaFwhm) * temp;
                item.kbFwhm = nextItem.kbFwhm + (nextItem.kbFwhm - priorItem.kbFwhm) * temp;
                item.LaFwhm = nextItem.LaFwhm + (nextItem.LaFwhm - priorItem.LaFwhm) * temp;
                item.LbFwhm = nextItem.LbFwhm + (nextItem.LbFwhm - priorItem.LbFwhm) * temp;
                //item.LrFwhm = nextItem.LrFwhm + (nextItem.LrFwhm - priorItem.LrFwhm) * temp;
                //item.LeFwhm = nextItem.LeFwhm + (nextItem.LeFwhm - priorItem.LeFwhm) * temp;

                item.KaHight = nextItem.KaHight + (nextItem.KaHight - priorItem.KaHight) * temp;
                item.KbHight = nextItem.KbHight + (nextItem.KbHight - priorItem.KbHight) * temp;
                item.LaHight = nextItem.LaHight + (nextItem.LaHight - priorItem.LaHight) * temp;
                item.LbHight = nextItem.LbHight + (nextItem.LbHight - priorItem.LbHight) * temp;

                item.KaIntensity = item.KaArea;
                item.KbIntensity = item.KbArea;
                item.LbIntensity = item.LbArea;
                item.LaIntensity = item.LaArea;
                //item.LrHight = nextItem.LrHight + (nextItem.LrHight - priorItem.LrHight) * temp;
                //item.LeHight = nextItem.LeHight + (nextItem.LeHight - priorItem.LeHight) * temp;
            }
            return item;
        }

        /// <summary>
        /// 调整数据库中的coeff；
        /// </summary>
        public void CoeffAdjust()
        {
            List<ElementSpectrum> elemList = ElementSpectrum.FindAll();
            int[] indexs = KLineIndexFind(elemList);
            if (indexs.Length == 1)
            {
                for (int i = 0; i < elemList.Count; i++)
                {
                    elemList[i].KaCoeff = elemList[indexs[0]].KaCoeff;
                    elemList[i].kbCoeff = elemList[indexs[0]].kbCoeff;
                }
            }
            else if (indexs.Length > 1)
            {
                for (int i = 0; i < indexs.Length - 1; i++)
                {
                    int temp1 = i > 0 ? indexs[i] : 0;
                    int temp2 = i == (indexs.Length - 2) ? elemList.Count - 1 : indexs[i + 1];
                    Double aK = (elemList[indexs[i]].KaCoeff - elemList[indexs[i + 1]].KaCoeff)
                              / (elemList[indexs[i]].ElementID - elemList[indexs[i + 1]].ElementID);
                    Double aC = elemList[indexs[i]].KaCoeff - aK * elemList[indexs[i]].ElementID;
                    Double bK = (elemList[indexs[i]].kbCoeff - elemList[indexs[i + 1]].kbCoeff)
                              / (elemList[indexs[i]].ElementID - elemList[indexs[i + 1]].ElementID);
                    Double bC = elemList[indexs[i]].kbCoeff - bK * elemList[indexs[i]].ElementID; ;
                    for (int j = temp1; j < temp2; j++)
                    {
                        elemList[i].KaCoeff = aK * elemList[j].ElementID + aC;
                        elemList[i].kbCoeff = bK * elemList[j].ElementID + bC;
                    }
                }
            }
            // 调整L系的的系数
            indexs = LLineIndexFind(elemList);
            if (indexs.Length == 1)
            {
                for (int i = 0; i < elemList.Count; i++)
                {
                    elemList[i].LaCoeff = elemList[indexs[0]].LaCoeff;
                    elemList[i].LbCoeff = elemList[indexs[0]].LbCoeff;
                }
            }
            else if (indexs.Length > 1)
            {
                for (int i = 0; i < indexs.Length - 1; i++)
                {
                    int temp1 = i > 0 ? indexs[i] : 0;
                    int temp2 = i == (indexs.Length - 2) ? elemList.Count - 1 : indexs[i + 1];
                    Double aK = (elemList[indexs[i]].LaCoeff - elemList[indexs[i + 1]].LaCoeff)
                              / (elemList[indexs[i]].ElementID - elemList[indexs[i + 1]].ElementID);
                    Double aC = elemList[indexs[i]].LaCoeff - aK * elemList[indexs[i]].ElementID;
                    Double bK = (elemList[indexs[i]].LbCoeff - elemList[indexs[i + 1]].LbCoeff)
                              / (elemList[indexs[i]].ElementID - elemList[indexs[i + 1]].ElementID);
                    Double bC = elemList[indexs[i]].LbCoeff - bK * elemList[indexs[i]].ElementID; ;
                    for (int j = temp1; j < temp2; j++)
                    {
                        elemList[i].LaCoeff = aK * elemList[j].ElementID + aC;
                        elemList[i].LbCoeff = bK * elemList[j].ElementID + bC;
                    }
                }
            }
        }

       

        /// <summary>
        /// 调整ka，kb的系数
        /// </summary>
        private int[] KLineIndexFind(List<ElementSpectrum> elemList)
        {
            int[] indexs = new int[elemList.Count];
            int count = 0;
            for (int i = 0; i < elemList.Count; i++)
            {
                if (coef * elemList[i].KaFwhm * elemList[i].KaHight > 0 
                    && coef * elemList[i].kbFwhm * elemList[i].KbHight > 0)
                {
                    indexs[count] = i;
                    count++;
                }
            }
            if (count > 0)
            {
                int[] result = new int[count];
                indexs.CopyTo(result, count);
                return result;
            }
            else
            {
                return new int[0];
            }
        }

        /// <summary>
        /// 调整LaLb的系数
        /// </summary>
        private int[] LLineIndexFind(List<ElementSpectrum> elemList)
        {
            int[] indexs = new int[elemList.Count];
            int count = 0;
            for (int i = 0; i < elemList.Count; i++)
            {
                if (coef * elemList[i].LaFwhm * elemList[i].LaHight > 0 
                    && coef * elemList[i].LbHight * elemList[i].LbFwhm > 0)
                {
                    indexs[count] = i;
                    count++;
                }
            }
            if (count > 0)
            {
                int[] result = new int[count];
                indexs.CopyTo(result, count);
                return result;
            }
            else
            {
                return new int[0];
            }
        }


    }
}
