using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common
{
    public class TargetDic
    {
        public static Dictionary<string, bool> TargetDictionary;

        public TargetDic()
        {
            if (TargetDictionary!=null)
                   TargetDictionary.Clear();
            TargetDictionary = new Dictionary<string, bool>();

            TargetDictionary.Add("Target", WorkCurveHelper.DeviceCurrent.HasTarget);//当前设备是否含有靶材
            TargetDictionary.Add("CollimatorIdx", WorkCurveHelper.DeviceCurrent.HasCollimator);//当前设备是否含有准直器
            TargetDictionary.Add("Collimator", WorkCurveHelper.DeviceCurrent.HasCollimator);//当前设备是否含有准直器
            TargetDictionary.Add("FilterIdx", WorkCurveHelper.DeviceCurrent.HasFilter);//当前设备是否含有滤光片
            TargetDictionary.Add("Filter", WorkCurveHelper.DeviceCurrent.HasFilter);//当前设备是否含有滤光片
            TargetDictionary.Add("TargetMode", WorkCurveHelper.DeviceCurrent.HasTarget);//当前设备是否分一次二次靶
            TargetDictionary.Add("CurrentRate", WorkCurveHelper.DeviceCurrent.HasTarget);//当前设备二次靶下得管流比例因子
        }

        public TargetDic(bool First)
        {
            if (TargetDictionary != null)
                 TargetDictionary.Clear();
            TargetDictionary = new Dictionary<string, bool>();

            TargetDictionary.Add("Target", First);//当前设备是否含有靶材
            TargetDictionary.Add("CollimatorIdx", First);//当前设备是否含有准直器
            TargetDictionary.Add("Collimator", First);//当前设备是否含有准直器
            TargetDictionary.Add("FilterIdx", First);//当前设备是否含有滤光片
            TargetDictionary.Add("Filter", First);//当前设备是否含有滤光片
            TargetDictionary.Add("TargetMode", First);//当前设备是否分一次二次靶
            TargetDictionary.Add("CurrentRate", First);//当前设备二次靶下得管流比例因子
        }
    }
}
