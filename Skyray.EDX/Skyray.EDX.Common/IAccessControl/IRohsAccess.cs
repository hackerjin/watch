using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDX.Common
{
    public interface IRohsAccess
    {
        /// <summary>
        /// 增加工作区
        /// </summary>
        /// <param name="spaceName"></param>
        void ReloadWorkSpace(string spaceName);

        /// <summary>
        /// 峰标识创建完成，重写Parameter.ini文件
        /// </summary>
        /// <param name="item"></param>
        void ExcuteMatchParamsInput(NaviItem item);

        /// <summary>
        /// 设备分辨率计算
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        void ExcuteResolveCaculate(Condition item,long id,int selectIndex);

        /// <summary>
        /// 打开rohs3谱数据
        /// </summary>
        /// <param name="item"></param>
        void OpenRohs3(NaviItem item);

        /// <summary>
        /// 打开rohs4谱数据
        /// </summary>
        /// <param name="item"></param>
        void OpenRohs4(NaviItem item);

        /// <summary>
        /// 曲线校正
        /// </summary>
        /// <param name="item"></param>
        void ExcuteCurveCalibrate(NaviItem item);


        void MatchWorkCurve(ref string strMatter, ref int Id,int count);

        void PassingContinousWorkCurve(SpecListEntity currentList, WorkCurve workCurve);
    }
}
