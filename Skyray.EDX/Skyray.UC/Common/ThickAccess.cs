using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    public class ThickAccess : IThickAccess
    {
        private Thick _logClass;

        public ThickAccess(Thick logicClass)
        {
            _logClass = logicClass;
        }
        #region IThickAccess 成员

        public void SelectCountStyle(bool flag)
        {
            _logClass.SelectCountStyle(flag);
        }

        public void CountBits(int thickBits, int contentBits)
        {
            _logClass.CountBits(thickBits, contentBits);
        }

        public void ExcuteCurveCalibrate(NaviItem item,int cnt,OptMode optMode)
        {
            _logClass.ExcuteCurveCalibrate(item, cnt, optMode);
        }

        //add by Sunjian 2012/10/15
        public void AppSave()
        {
            _logClass.AppSave();
        }

        #endregion
    }
}
