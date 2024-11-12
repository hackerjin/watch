using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using Skyray.Controls;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDX.Common
{
    public interface IRefreshFillInfo
    {
        void RefreshQuality();

        void RefreshCurve(InitParameter InitParameter, DeviceParameter deviceParams);

        void RefreshDevice();

        void RefreshSpec(SpecListEntity specList,SpecEntity spec);

        void RefreshMeasureResult(int currentTimes,ElementList elementList);

        void RefreshChinawareResult(string strChiawareInfo, string Centainty, List<HistoryElement> elementlist);

        void RefreshStaticsInfo(int currentTimes,ElementList elementList);

        //void WorkCurveExistsElements(TestDevicePassedParams testParams);

        void UpdateWorkSpec(DeviceParameter deviceParams,SpecListEntity specList);

        void ContructMeasureRefreshData(int count,ElementList elementList);

        void CreateContructStatis(ElementList list);

        void SetStyle(Style style);

        void RefreshWorkRegion();

        void SaveExcel();

        void PrintExcel();

        void RefreshTarget();

        void Exist();

        void RefreshMeasureDialog();

        void LogOut();

        void RefreshHistory();

        void ChangeCheckedCurve();

        void RefreshStartStatus(bool visibled);

        void PrintBlueExcel();


       
    }
}
