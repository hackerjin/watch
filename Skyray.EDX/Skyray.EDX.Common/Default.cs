using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary.Spectrum;
using Lephone.Data.Definition;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 缺省的一些对象实例
    /// </summary>
    public class Default
    {
        /// <summary>
        /// 缺省的设备参数对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static DeviceParameter GetDeviceParameter(SpecLength specLength,int index)
        {
            int endChannel = 2000;
            endChannel = (int)specLength - 50;
            return DeviceParameter.New.Init("C" + index + DateTime.Now.Millisecond, 100, 100, 40, 1, 1, 1, false, 0, false, 0, false, 1000, 4000, 50, endChannel, false, false, 0, 0, 0, 0, 15,TargetMode.OneTarget,1);
        }

        public static ElementList GetElementList()
        {
            ElementList elementList = ElementList.New.Init(false, 0, 0, false, 0, 0.99999, false);
            elementList.MatchSpecListIdStr = "";
            elementList.RefSpecListIdStr = "";
            elementList.MainElementToCalcKarat = "Au";
            elementList.LayerElemsInAnalyzer = "Rh";
            return elementList;
        }

        /// <summary>
        /// 缺省的初始化条件对象
        /// </summary>
        /// <returns></returns>
        public static InitParameter GetInitParameter(SpecLength specLength)
        {
            switch (specLength)
            {
                case SpecLength.Min:
                    return InitParameter.New.Init(40, 100, 60, 120, 60, 120, 470, 0, "Ag", 1, 1, 1, TargetMode.OneTarget, 1,"x",1);//加载默认值
                case SpecLength.Normal:
                    return InitParameter.New.Init(40, 100, 60, 120, 60, 120, 1105, 0, "Ag", 1, 1, 1, TargetMode.OneTarget, 1, "x",1);//加载默认值
                case SpecLength.Max:
                    return InitParameter.New.Init(40, 100, 60, 120, 60, 120, 2210, 0, "Ag", 1, 1, 1, TargetMode.OneTarget, 1, "x",1);//加载默认值
            }
            return InitParameter.New.Init(40, 100, 60, 120, 60, 120, 1105, 0, "Ag", 1, 1, 1, TargetMode.OneTarget, 1, "x",1);//加载默认值
        }

        /// <summary>
        /// 缺省的能量刻度Ag处理函数
        /// </summary>
        /// <returns></returns>
        public static DemarcateEnergy GetDemarcateEnergyAg(SpecLength specLength)
        {
            switch (specLength)
            {
                case SpecLength.Min:
                    return DemarcateEnergy.New.Init("Ag", XLine.Ka, 22.1, 470); ;//加载默认值
                case SpecLength.Normal:
                    return DemarcateEnergy.New.Init("Ag", XLine.Ka, 22.1, 1105); ;//加载默认值
                case SpecLength.Max:
                    return DemarcateEnergy.New.Init("Ag", XLine.Ka, 22.1, 2210); ;//加载默认值
            }
            return DemarcateEnergy.New.Init("Ag", XLine.Ka, 22.1, 1105);
        }

         public static List<DemarcateEnergy> ConvertFromNewOld(List<DemarcateEnergyEntity> newDemar,SpecLength speclength)
        {
            List<DemarcateEnergy> returnDemar = new List<DemarcateEnergy>();
            if (newDemar == null || newDemar.Count == 0)
            {
                returnDemar.Add(GetDemarcateEnergyAg(speclength));
                returnDemar.Add(GetDemarcateEnergyCu(speclength));
            }
            newDemar.ForEach(w => returnDemar.Add(w.ConvertFrom()));
            return returnDemar;
        }

         public static DemarcateEnergyEntity[] ConvertFormOldToNew(HasMany<DemarcateEnergy> oldDema, SpecLength speclength)
        {
            List<DemarcateEnergyEntity> tt = new List<DemarcateEnergyEntity>();
            if (oldDema == null || oldDema.Count == 0)
            {
                tt.Add(GetDemarcateEnergyAg(speclength).ConvertToDemEntity());
                tt.Add(GetDemarcateEnergyCu(speclength).ConvertToDemEntity());
            }
            oldDema.ToList().ForEach(w=>tt.Add(w.ConvertToDemEntity()));
             return tt.ToArray();
        }

         public static DemarcateEnergyEntity[] ConvertFormOldToNew(List<DemarcateEnergy> oldDema, SpecLength speclength)
         {
             List<DemarcateEnergyEntity> tt = new List<DemarcateEnergyEntity>();
             if (oldDema == null || oldDema.Count == 0)
             {
                 tt.Add(GetDemarcateEnergyAg(speclength).ConvertToDemEntity());
                 tt.Add(GetDemarcateEnergyCu(speclength).ConvertToDemEntity());
             }
             oldDema.ToList().ForEach(w => tt.Add(w.ConvertToDemEntity()));
             return tt.ToArray();
         }


        /// <summary>
        ///  缺省的能量刻度Cu处理函数
        /// </summary>
        /// <returns></returns>
         public static DemarcateEnergy GetDemarcateEnergyCu(SpecLength specLength)
        {
            switch (specLength)
            {
                case SpecLength.Min:
                    return DemarcateEnergy.New.Init("Cu", XLine.Ka, 8.041, 140); ;//加载默认值
                case SpecLength.Normal:
                    return DemarcateEnergy.New.Init("Cu", XLine.Ka, 8.041, 412); ;//加载默认值
                case SpecLength.Max:
                    return DemarcateEnergy.New.Init("Cu", XLine.Ka, 8.041,804); ;//加载默认值
            }
            return DemarcateEnergy.New.Init("Cu", XLine.Ka, 8.041, 412);
        }

        /// <summary>
        /// 缺省的谱列表构造
        /// </summary>
        /// <returns></returns>
        //public static SpecList GetSpecList()
        //{
        //    return SpecList.New.Init("", "", "", double.Epsilon, "", "", DateTime.Now, "",SpecType.UnKownSpec);
        //}

        //public static Spec GetSpec()
        //{
        //    return Spec.New.Init("", "", double.Epsilon, 0, 0, 0, "");
        //}
        /// <summary>
        /// 缺省的条件对象构造
        /// </summary>
        /// <returns></returns>
        public static Condition GetCondition()
        {
            return Condition.New.Init("");
        }

        /// <summary>
        /// 缺省的FPGA参数构造
        /// </summary>
        /// <returns></returns>
        public static FPGAParams GetFPGAParams()
        {
            return FPGAParams.New.Init(OFFON.ON, OFFON.OFF, OFFON.ON, 1, 2.4, 0.2, 0, 0, 0,"192.168.3.7", 100000,1);
        }

    }
}
