using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.Print;

namespace Skyray.UC
{
    public class MediumAccess:IMediumAccess
    {

        #region IMediumAccess Members

        public void OpenCurveSubmit()
        {
            bool isOpenSpec = false;
            //if (WorkCurveHelper.WorkCurveCurrent != null)
            //{
            //    if (WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr != null && WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr != string.Empty)
            //        WorkCurveName =WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr;
            //}
           
            DifferenceDevice.interClassMain.OpenWorkCurveLog(WorkCurveHelper.WorkCurveCurrent,1);
            if (isOpenSpec)
                DifferenceDevice.interClassMain.ReloadVirtualSpecByWorkCurve();
            //add by chuyaqin 2011-07-23光斑的修改
            if (DifferenceDevice.interClassMain.skyrayCamera != null)
            {
                DifferenceDevice.interClassMain.skyrayCamera.FociIndex = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].CollimatorIdx - 1;
            }
        }

       

        public void ExcuteTestStart(TestDevicePassedParams testParams)
        {
            DifferenceDevice.interClassMain.StartTestProcess(testParams);
        }

        public void TestInitialization()
        {
            int id = 0;
            DifferenceDevice.interClassMain.TestInitalize(true,false,true,id);
        }

        public void TestStop()
        {
            DifferenceDevice.interClassMain.TestStop();
        }

        public void ExcuteCaculate()
        {
            DifferenceDevice.interClassMain.CaculateExcute(false,true);
        }

        public void ExcuteAutoDemarcateEnergy()
        {
            DifferenceDevice.interClassMain.TestInitalize(false,false,false,0);
        }
        public void ExcuteAutoFPGAIntercept()
        {
            DifferenceDevice.interClassMain.TestInitalize(false,true,false,0);
        }
        public void DirectPrint(List<TreeNodeInfo> list)
        {
            DifferenceDevice.interClassMain.SaveExcel(list,0);
        }

        public void SaveTemplateUpdateEvent()
        {
            ReportTemplateHelper.LoadDirctoryTemplate();
        }

        public void AddIntrestedElements()
        {
            DifferenceDevice.interClassMain.AddInterestedElemenetUpdateProcess();
        }

        public void SelectMode(int index)
        {
            if (DifferenceDevice.interClassMain != null)
            {
                DifferenceDevice.interClassMain.currentSelectMode = index;
                DifferenceDevice.interClassMain.ExploreModeDec();
                if (index == 0 && WorkCurveHelper.WorkCurveCurrent != null)
                {
                    DifferenceDevice.interClassMain.refreshFillinof.ContructMeasureRefreshData(1, WorkCurveHelper.WorkCurveCurrent.ElementList);
                    DifferenceDevice.interClassMain.refreshFillinof.CreateContructStatis(WorkCurveHelper.WorkCurveCurrent.ElementList);
                }
            }
        }

        public void DisplayPeak(NaviItem item)
        {
            DifferenceDevice.interClassMain.DisplayPeak(item);
        }

        public void ReportSpecification(NaviItem item)
        {
            DifferenceDevice.interClassMain.ReportSpecification(item);
        }

        public void AutoAnalysis()
        {
            DifferenceDevice.interClassMain.AutoAnalysisProcess(null);
        }

        public void DisplayElement(NaviItem item)
        {
            DifferenceDevice.interClassMain.DisplayElement(item);
        }

        public void OpenWorkSpectrumSelect(List<SpecListEntity> spelist)
        {
            if (spelist != null && spelist.Count > 0)
                DifferenceDevice.interClassMain.OpenWorkSpec(spelist,true);
        }

        public void OpenVirtualWorkSpectrum(List<SpecListEntity> splist)
        {
            DifferenceDevice.interClassMain.AddVirutulSpec(splist);
        }

        public void SelectRatioVirtualSpec(List<SpecListEntity> splist)
        {
            DifferenceDevice.interClassMain.SelectRatioVirtualSpec(splist);
        }

        public void ManAnalysis(int[] index, string[] atoms)
        {
            DifferenceDevice.interClassMain.ManAnalysis(index, atoms);
        }

        public void PrintTemplateSource(ref List<TreeNodeInfo> template)
        {
            //EDXRFHelper.LoadTemplate();
        }

        public void CustomFileld()
        {
            DifferenceDevice.interClassMain.OpenWorkCurveLog(WorkCurveHelper.WorkCurveCurrent,1);
        }

        public void DevicceChange()
        {
            if (DifferenceDevice.interClassMain != null)
            DifferenceDevice.interClassMain.DeviceChangeProcess();
        }

        public void SaveDevice(NaviItem item,ContainerObject panel)
        {
            TargetDic atar = new TargetDic();
            if (DifferenceDevice.interClassMain != null)
            {
                DifferenceDevice.interClassMain.RefrenshMoveStation(item, panel);
                //DifferenceDevice.interClassMain.TargetModel();
            }
        }

        public void ConnectDevice()
        {
            DifferenceDevice.interClassMain.ConnectDevice();
        }

        public void SaveChangeStandand()
        {
            DifferenceDevice.interClassMain.refreshFillinof.ContructMeasureRefreshData(1,WorkCurveHelper.WorkCurveCurrent.ElementList);
            DifferenceDevice.interClassMain.refreshFillinof.CreateContructStatis(WorkCurveHelper.WorkCurveCurrent.ElementList);
        }

        public void DisappearBk()
        {
            DifferenceDevice.interClassMain.DisappearBk();
        }

        public void CreateIntRegion()
        {
            DifferenceDevice.interClassMain.CreateIntRegion();
        }

        public void CaculateIntRegion()
        {
            DifferenceDevice.interClassMain.CaculateIntRegion();
        }

        public ElementList CaculateIntensityReport(out string sampleName,out bool IsExplore)
        {
            return DifferenceDevice.interClassMain.CaclIntensityReport(out sampleName, out IsExplore);
        }

        public void DisplayLogData()
        {
            DifferenceDevice.interClassMain.DisplayLogData();
        }

        public string[] QualityResult(out int[] lines)
        {
            return DifferenceDevice.interClassMain.QualityResult(out lines);
        }

        public void StartPreHeatProcess(PreHeatParams heatParams)
        {
            DifferenceDevice.interClassMain.PreHeatProcess(heatParams);
        }

        public void SelectSpecification()
        {
            DifferenceDevice.interClassMain.refreshFillinof.ContructMeasureRefreshData(1,WorkCurveHelper.WorkCurveCurrent.ElementList);
            DifferenceDevice.interClassMain.refreshFillinof.CreateContructStatis(WorkCurveHelper.WorkCurveCurrent.ElementList);
        }

        public bool IPSettings(string IP, string SubNet, string GateWay, string DNS)
        {
            return DifferenceDevice.interClassMain.IPSettings(IP,SubNet,GateWay,DNS);
        }

        public bool SetSurfaceSource(ushort firstLight, ushort secondLight, ushort thirdLight, ushort fourthLight)
        {
            return DifferenceDevice.interClassMain.SetSurfaceSource(firstLight, secondLight, thirdLight, fourthLight);
        }

        public void CopyCurrentWorkCurve()
        {
            DifferenceDevice.interClassMain.CopyCurrentWorkCurve();
        }


        public void UpdateTitleICO()
        {
            DifferenceDevice.interClassMain.UpdateTitleICO();
        }

        //public void OpenOldSpec()
        //{
        //    DifferenceDevice.interClassMain.OpenOldSpec();
        //}

        //public void SpectrumImport(string path)
        //{
        //    DifferenceDevice.interClassMain.SpectrumImport(path);
        //}

        //public void SpectrumExport(string path)
        //{
        //    DifferenceDevice.interClassMain.SpectrumExport(path);
        //}
        #endregion


        #region IMediumAccess 成员


        public void RefreshHistory()
        {
            DifferenceDevice.interClassMain.RefreshHistory();
        }


        #endregion
    }
}
