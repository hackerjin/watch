using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common
{
    public interface IThickAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        void SelectCountStyle(bool flag);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentBits"></param>
        /// <param name="thickBits"></param>
        void CountBits(int thickBits, int contentBits);

        void AppSave();

        /// <summary>
        /// 曲线校正
        /// </summary>
        /// <param name="item"></param>
        void ExcuteCurveCalibrate(NaviItem item,int cnt,OptMode optMode);
    }
}
