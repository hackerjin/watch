using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDX.Common.Component
{
    public class BackGroundHelper
    {
        /// <summary>
        /// 根据空白谱消除当前本体。
        /// </summary>
        /// <param name="specBlank">空白谱</param>
        /// <param name="currentSpec">当前谱</param>
        public static void BGDisappear(SpecListEntity specBlank, SpecListEntity currentSpec)
        {
            if (specBlank == null || currentSpec == null || specBlank.Specs == null || currentSpec.Specs == null)
                return;
            if (specBlank.Specs.Length != currentSpec.Specs.Length && specBlank.Specs.Length != 1)
                return;
            int[] bkData = Helper.ToInts(specBlank.Specs[0].SpecData);
            int[] spData = Helper.ToInts(currentSpec.Specs[0].SpecData);
            if (bkData == null || spData == null || bkData.Length != spData.Length)
                return;
            for (int i = 0; i < bkData.Length; i++)
            {
                double temp = Math.Round(bkData[i] * currentSpec.Specs[0].SpecTime / specBlank.Specs[0].SpecTime);
                if (spData[i] < temp)
                    spData[i] = 0;
                else
                    spData[i] = spData[i] - Convert.ToInt32(temp);
            }
            currentSpec.Specs[0].SpecData = Helper.ToStrs(spData);
        }

        /// <summary>
        /// 计算感兴趣元素面积
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="leftBorder"></param>
        /// <param name="rightBorder"></param>
        /// <returns></returns>
        public static double CaculateArea(SpecEntity spec,int leftBorder,int rightBorder)
        {
            if (spec == null || leftBorder > rightBorder)
                return 0d;
            double totalArea = 0d;
            int[] data = spec.SpecDatas;
            for (int i = leftBorder; i <= rightBorder; i++)
                totalArea += data[i];
            return totalArea;
        }

        /// <summary>
        ///  感兴趣元素区创建及计算显示
        /// </summary>
        /// <param name="flag"></param>
        public static void ChangeIntVisible(bool flag)
        {
            NaviItem item = WorkCurveHelper.NaviItems.Find(w => w.Name == "CaculateIntRegion");
            if (item != null)
                item.EnabledControl = flag;
            item = WorkCurveHelper.NaviItems.Find(w => w.Name == "CreateIntRegion");
            if (item != null)
                item.EnabledControl = flag;
        }
    }
}
