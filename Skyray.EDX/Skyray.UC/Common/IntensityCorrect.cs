//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Skyray.EDX.Common;
//using System.Windows.Forms;
//using Skyray.EDXRFLibrary.Spectrum;
//using Skyray.EDXRFLibrary;

//namespace Skyray.UC.Common
//{
//public class IntensityCorrect
//{
//     private bool _bContinue = false;
//     private FrmStatus frmStatus = null;
//     List<SpecListEntity> listScanPureSpec = new List<SpecListEntity>();
//     List<SpecListEntity> listOldPureSpec = new List<SpecListEntity>();
//     SpecListEntity SpecListConlibrate = null;
//     List<string> listOldElems = new List<string>();
//     List<string> listScanElems = new List<string>();
//     DialogResult[] elemResults = null;
//     private int _currentElemIndex = -1;
//     private System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker();
//     private int _calibrateType = 2;//2，控样 1，纯元素
//     private int _InitialOrCalibrate = 1;//1,Initial 2,Calibrate
//     public int CurrentElemIndex
//     {
//         get { return _currentElemIndex; }
//     }
//     public bool BContinue
//     {
//         get { return _bContinue; }
//         set { _bContinue = value; }
//     }

//     public bool StartInitial()
//     {
//         if (Msg.Show(Info.MessageBoxTextProgrammableMeasureIsSureAdjust, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//         {
//             _bContinue = false;
//             return false;
//         }
//         _bContinue = true;
//         listScanPureSpec.Clear();
//         listOldPureSpec.Clear();
//         listScanElems.Clear();
//         listOldElems.Clear();
//         elemResults=new DialogResult[WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count];
//        DifferenceDevice.interClassMain.TestInitalize(true,false);
//        return true;
//     }
//     public void EndInitial(bool succeed)
//     {
//          if (!succeed)
//        {
//            _bContinue = false;
//            return;
//        }
//        FrmCheckInCal frmtemp = new FrmCheckInCal(WorkCurveHelper.WorkCurveCurrent.InCalType);
//        if (frmtemp.ShowDialog() == DialogResult.OK)
//        {
//            _calibrateType = frmtemp.CalibrateType;
//            _InitialOrCalibrate = frmtemp.InitialOrCalibrate;
//            if (_calibrateType == 1)
//            {
//                StartScanPureSpec(0);
//            }
//            else
//            {
//                _bContinue = StartScanConSpec();
//            }

//        }
//        else _bContinue = false;
        
//     }
//     public bool StartScanConSpec()
//     {
//         string ConSampleName = WorkCurveHelper.WorkCurveCurrent.InCalSampName;
//         if (_InitialOrCalibrate==1)
//         {
//             UcInCalibrate uc = new UcInCalibrate(2, ConSampleName);
//             //WorkCurveHelper.OpenUC(uc, false, Info.SelectElement,true, false);
//             WorkCurveHelper.OpenUC(uc, true, Info.SelectElement,true);
//             if (uc.dialogResult == DialogResult.Cancel) return false;
//             ConSampleName = uc.ConSampleName;
//         }
//         if(WorkCurveHelper.WorkCurveCurrent.IntensityCalibration.Count<=0) return false;
//         FrmScanPure fsp = new FrmScanPure();
//         fsp.CalibrateType = 2;
//         fsp.TestInfo = Info.PleaseCurveCalibrationSample + ConSampleName;
//         DialogResult dsTemp= fsp.ShowDialog();
//         if (dsTemp == DialogResult.Ignore)
//         {
//             List<SpecListEntity> listspec = EDXRFHelper.GetReturnSpectrum(false, false);
//             if (listspec != null && listspec.Count > 0)
//             {
//                 DifferenceDevice.interClassMain.specList = listspec[0];
//                 EndScanSpec();
//                 return false;
//             }
//         }
//         else if (dsTemp != DialogResult.Yes)
//             return false;
//         List<WordCureTest> listtestCurves = new List<WordCureTest>();
//         InterfaceClass.isMulitTest = false;
//         InterfaceClass.selePrintObjectL.Clear();
//         WorkCurveHelper.MainSpecList = null;
//         string DropTime = ReportTemplateHelper.LoadSpecifiedValueNoWait("TestParams", "DropTime");
//         MeasureParams MeasureParams = new MeasureParams(1, int.Parse(DropTime != string.Empty ? DropTime : "0"), false);
//         string specName = ConSampleName+"_" + (_InitialOrCalibrate == 1 ? WorkCurveHelper.WorkCurveCurrent.ConditionName + "_Orgin" : WorkCurveHelper.WorkCurveCurrent.ConditionName + "_Calibrate");
//         Lephone.Data.DbEntry.Context.ExecuteNonQuery("delete from historycompanyotherinfo where history_id in (select id from historyrecord where speclistName in ('" + specName + "'));" +
//                       "delete from historyelement where historyrecord_id in (select id from historyrecord where speclistName in ('" + specName + "'));" +
//                       "delete from historyrecord where speclistName in ('" + specName + "')");
//         WorkCurveHelper.DataAccess.DeleteRecord(specName);
//         WordCureTest testTemp = new WordCureTest("1", new SpecListEntity(specName, specName, "", 0, "", "", DateTime.Now, "", SpecType.PureSpec, DifferenceDevice.DefaultSpecColor.ToArgb(), DifferenceDevice.DefaultSpecColor.ToArgb()), WorkCurveHelper.WorkCurveCurrent.Name, WorkCurveHelper.WorkCurveCurrent.Id.ToString());
//         listtestCurves.Add(testTemp);
//         TestDevicePassedParams tempParam = new TestDevicePassedParams(false, MeasureParams, listtestCurves, WorkCurveHelper.DeviceCurrent.IsAllowOpenCover, SpecType.PureSpec, specName, false, false);
//         DifferenceDevice.IsConnect = true;
//         DifferenceDevice.interClassMain.StartTestProcess(tempParam);
//         return true;

//     }
//     //开始纯元素扫谱
//     public bool StartScanPureSpec(int i)
//     {
//         _currentElemIndex = i;
//         int elementCount = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count;
//         List<WordCureTest> listtestCurves = new List<WordCureTest>();
//         string ElemName = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Caption;
//         string specPureElem = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].ElementSpecName == null || WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].ElementSpecName == string.Empty ? ElemName : WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].ElementSpecName;
//         SpecListEntity specOld = DataBaseHelper.QueryByEdition(specPureElem, string.Empty, TotalEditionType.Default);
//         if (specOld != null && !listOldElems.Contains(ElemName))
//         {
//             listOldPureSpec.Add(specOld);
//             listOldElems.Add(ElemName);
//         }
//         FrmScanPure fsp = new FrmScanPure();
//         fsp.TestInfo = Info.PleaseCurveCalibrationSample + ElemName;
//         elemResults[i] = fsp.ShowDialog();
//         List<SpecListEntity> listspec=null;
//         //while((listspec==null||listspec.Count<=0)&&elemResults[i]==DialogResult.Ignore)
//         {
//            switch (elemResults[i])
//            {
//                //跳过
//                case DialogResult.Cancel:
//                    if(i<elementCount-1)
//                     return StartScanPureSpec(i+1);
//                    break;
//                // 打开谱  
//                case DialogResult.Ignore:
//                    listspec = EDXRFHelper.GetReturnSpectrum(false, false);
//                    if (listspec == null || listspec.Count <= 0)
//                    {
//                        return StartScanPureSpec(i);
//                    }
//                    listScanPureSpec.Add(listspec[0]);
//                    listScanElems.Add(ElemName);
//                    if (i<elementCount-1)
//                    {
//                        return StartScanPureSpec(i+1);
//                    }
//                    break;
//                //扫谱
//                case DialogResult.Yes:
//                    InterfaceClass.isMulitTest = false;
//                    InterfaceClass.selePrintObjectL.Clear();
//                    WorkCurveHelper.MainSpecList = null;
//                    string DropTime = ReportTemplateHelper.LoadSpecifiedValueNoWait("TestParams", "DropTime");
//                    MeasureParams MeasureParams = new MeasureParams(1, int.Parse(DropTime != string.Empty ? DropTime : "0"), false);
//                    string specName = specPureElem + "_Correct";
//                    Lephone.Data.DbEntry.Context.ExecuteNonQuery("delete from historycompanyotherinfo where history_id in (select id from historyrecord where speclistName in ('" + specName + "'));" +
//                                  "delete from historyelement where historyrecord_id in (select id from historyrecord where speclistName in ('" + specName + "'));" +
//                                  "delete from historyrecord where speclistName in ('" + specName + "')");
//                    WorkCurveHelper.DataAccess.DeleteRecord(specName);
//                    WordCureTest testTemp = new WordCureTest("1", new SpecListEntity(specName, specName, "", 0, "", "", DateTime.Now, "", SpecType.PureSpec, DifferenceDevice.DefaultSpecColor.ToArgb(), DifferenceDevice.DefaultSpecColor.ToArgb()), WorkCurveHelper.WorkCurveCurrent.Name, WorkCurveHelper.WorkCurveCurrent.Id.ToString());
//                    listtestCurves.Add(testTemp);
//                    TestDevicePassedParams tempParam = new TestDevicePassedParams(false, MeasureParams, listtestCurves, WorkCurveHelper.DeviceCurrent.IsAllowOpenCover, SpecType.PureSpec, specName, false, false);
//                    DifferenceDevice.IsConnect = true;
//                    DifferenceDevice.interClassMain.StartTestProcess(tempParam);
//                    return true;
//                //退出
//                case DialogResult.Abort:
//                    _bContinue = false;
//                    return false;
//            }

//         }
//         if (i == elementCount - 1)
//         {
           
//             bw.DoWork += new System.ComponentModel.DoWorkEventHandler(DoCorrect);
//             bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(CloseFrmStatus);
//             //System.Threading.Thread test = new System.Threading.Thread(new System.Threading.ThreadStart(newFrmStatus));
//             //test.Priority = System.Threading.ThreadPriority.Normal;
//             //test.Start();
//             bw.WorkerSupportsCancellation = true;
//             bw.RunWorkerAsync(this);
//             newFrmStatus();
//            // DoCorrect();
//         }
//        return true;
//     }

//     public void EndScanSpec()
//     {
//         if (_calibrateType == 1)//纯元素校准
//         {
//             if (DifferenceDevice.interClassMain.specList != null && !listScanElems.Contains(WorkCurveHelper.WorkCurveCurrent.ElementList.Items[_currentElemIndex].Caption))
//             {
//                 listScanPureSpec.Add(DifferenceDevice.interClassMain.specList);
//                 listScanElems.Add(WorkCurveHelper.WorkCurveCurrent.ElementList.Items[_currentElemIndex].Caption);
//             }
//             if (_currentElemIndex < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count - 1)
//             {
//                 StartScanPureSpec(_currentElemIndex + 1);
//             }
//             else
//             {

//                 bw.DoWork += new System.ComponentModel.DoWorkEventHandler(DoCorrect);
//                 bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(CloseFrmStatus);
//                 bw.WorkerSupportsCancellation = true;
//                 //System.Threading.Thread test = new System.Threading.Thread(new System.Threading.ThreadStart(newFrmStatus));
//                 //test.Priority = System.Threading.ThreadPriority.Normal;
//                 //test.Start();
//                 bw.RunWorkerAsync(this);
//                 newFrmStatus();
//                 //DoCorrect();

//             }
//         }
//         else //控样校准
//         {
//             _bContinue = false;
//             if (DifferenceDevice.interClassMain.specList != null)
//             {
//                 SpecListConlibrate = DifferenceDevice.interClassMain.specList;
//             }
//             else return;
//             //根据选择的峰求面积

//             foreach (var icem in WorkCurveHelper.WorkCurveCurrent.IntensityCalibration)
//             {
//                 string element = icem.Element;
//                 int specID = -1;
//                 foreach (var spec in SpecListConlibrate.Specs)
//                 {
//                     specID++;
//                     if (icem.PeakLeft >= spec.DeviceParameter.BeginChann && icem.PeakRight <= spec.DeviceParameter.EndChann)
//                     {
//                         break;
//                     }
//                 }
                
//                 specID = specID >= SpecListConlibrate.Specs.Length ? 0 : specID;
//                 var Elem = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => w.ConditionID == specID&&w.PeakDivBase);
//                 if (_InitialOrCalibrate == 1)
//                 {
//                     int[] specDatas = SpecListConlibrate.Specs[specID].SpecDatac;
//                     icem.CalibrateIn = SpecHelper.TotalArea(icem.PeakLeft, icem.PeakRight, specDatas) / SpecListConlibrate.Specs[specID].UsedTime;
//                     icem.OriginalIn = icem.CalibrateIn;
//                     if (Elem != null)
//                     {
//                         icem.CalibrateBaseIn = SpecHelper.TotalArea(Elem.BaseLow, Elem.BaseHigh, specDatas) / SpecListConlibrate.Specs[specID].UsedTime;
//                         icem.OriginalBaseIn = icem.CalibrateBaseIn;
//                     }
//                     else
//                     {
//                         icem.OriginalBaseIn = icem.CalibrateBaseIn=0f;
//                     }
//                 }
//                 else if (_InitialOrCalibrate == 2)
//                 {
//                     int[] specDatas = SpecListConlibrate.Specs[specID].SpecDatac;
//                     icem.OriginalIn = SpecHelper.TotalArea(icem.PeakLeft, icem.PeakRight, specDatas) / SpecListConlibrate.Specs[specID].UsedTime;
//                     if (Elem != null)
//                         icem.OriginalBaseIn = SpecHelper.TotalArea(Elem.BaseLow, Elem.BaseHigh, specDatas) / SpecListConlibrate.Specs[specID].UsedTime;
//                     else icem.OriginalBaseIn = 0;
//                 }
//             }
//             WorkCurveHelper.WorkCurveCurrent.InCalType = _calibrateType;
//             WorkCurveHelper.WorkCurveCurrent.Save();
//             Msg.Show(Info.CorrectCompleted);

//         }
//     }

//     private void newFrmStatus()
//     {
//         FrmStatus fs = new FrmStatus();
//         fs.Name = "correctstatus";
//         frmStatus = fs;
//         frmStatus.TopMost = true;
//         frmStatus.ShowDialog();
//     }

//     private void CloseFrmStatus(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
//     {
//         if (frmStatus != null)
//         {
//             frmStatus.Close();
//             frmStatus.Dispose();
//             frmStatus = null;
//             //var us = new UcInCalibrate();
//            // WorkCurveHelper.OpenUC(us, true);
//             //提示完成
//             Msg.Show(Info.CorrectCompleted);
//             _bContinue = false;
//         }
         
//     }
//     //放弃
//     public void GiveUpCorrect()
//     {

//     }

//     public void DoCorrect(object sender, System.ComponentModel.DoWorkEventArgs e)
//     {
//         try
//         {
//             int elementCount = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count;
//             //替换曲线中纯元素谱，求得强度
//             for (int i = 0; i < elementCount; i++)
//             {
//                 if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].SSpectrumData != null
//                     && WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].SSpectrumData != string.Empty
//                     && listOldElems.Contains(WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Caption))
//                 {
//                     int specIndex = listOldElems.FindIndex(w => w == WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Caption);
//                     SpecEntity specTemp = listOldPureSpec[specIndex].Specs[WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].ConditionID];
//                     WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].SSpectrumData = Helper.TransforToDivTime(Helper.ToStrs(specTemp.SpecDatac), specTemp.UsedTime);
//                     //WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].ElementSpecName = listOldPureSpec[specIndex].Name;//不修改纯元素谱名，以供多次校正
//                 }
//             }
//             List<IntensityCalibration> lst = WorkCurveHelper.WorkCurveCurrent.IntensityCalibration.ToList();
//            // WorkCurveHelper.WorkCurveCurrent.IntensityCalibration.Clear();
//             for (int i = 0; i < elementCount; i++)
//             {
//                 string ElemName = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Caption;
//                 int oldIndex = listOldElems.FindIndex(w => w == ElemName);
//                // IntensityCalibration ic = WorkCurveHelper.WorkCurveCurrent.IntensityCalibration.First(w => w.Element.Equals(ElemName));
//                 IntensityCalibration ic = IntensityCalibration.New.Init(ElemName, 0, 0, WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].PeakLow, WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].PeakHigh,0,0);
//                 if (oldIndex < 0)
//                 {
//                     ic.CalibrateIn = ic.OriginalIn = 1;
//                 }
//                 else
//                 {
//                     SpecListEntity[] spelists = new SpecListEntity[1];
//                     spelists[0] = listOldPureSpec[oldIndex];
//                     if (!WorkCurveHelper.WorkCurveCurrent.CaculateIntensity(spelists))
//                     {

//                         // Msg.Show("计算错误");
//                         ic.CalibrateIn = ic.OriginalIn = 1;
//                     }
//                     else ic.OriginalIn = ic.CalibrateIn = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Intensity;
//                 }
//                 WorkCurveHelper.WorkCurveCurrent.IntensityCalibration.Add(ic);

//             }
//             //WorkCurveHelper.WorkCurveCurrent.Save();//保存数据
//             WorkCurveHelper.WorkCurveCurrent.ElementList.Save();//先保存元素谱的更改，不会导致下面执行时事务错误
//             Lephone.Data.DbEntry.Context.FastSave(WorkCurveHelper.WorkCurveCurrent.Id,
//             new Lephone.Data.SqlEntry.DataProvider.LineInfo<IntensityCalibration>
//             {
//                 Objs = WorkCurveHelper.WorkCurveCurrent.IntensityCalibration
//             },
//              new Lephone.Data.SqlEntry.DataProvider.LineInfo<IntensityCalibration>
//              {
//                  IsToDelete = true,
//                  Objs = lst
//              });
//             WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
//             //替换曲线中纯元素谱，求得强度
//             for (int i = 0; i < elementCount; i++)
//             {
//                 if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].SSpectrumData != null
//                     && WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].SSpectrumData != string.Empty
//                     && listScanElems.Contains(WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Caption))
//                 {
//                     int specIndex = listScanElems.FindIndex(w => w == WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Caption);
//                     SpecEntity specTemp = listScanPureSpec[specIndex].Specs[WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].ConditionID];
//                     WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].SSpectrumData = Helper.TransforToDivTime(Helper.ToStrs(specTemp.SpecDatac), specTemp.UsedTime);
//                     //WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].ElementSpecName = listScanPureSpec[specIndex].Name;//不修改纯元素谱名，以供多次校正
//                 }
//             }
//             for (int i = 0; i < elementCount; i++)
//             {
//                 string ElemName = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Caption;
//                 //int oldIndex = listOldElems.FindIndex(w => w == ElemName);
//                 //IntensityCalibration ic = WorkCurveHelper.WorkCurveCurrent.IntensityCalibration.First(w => w.Element.Equals(ElemName));
//                 //if (oldIndex < 0)
//                 //{
//                 //    ic.CalibrateIn = ic.OriginalIn = 1;
//                 //    ic.Save();
//                 //    continue;
//                 //}
//                 //SpecListEntity[] spelists = new SpecListEntity[1];
//                 //spelists[0] = listOldPureSpec[oldIndex];
//                 //if (!WorkCurveHelper.WorkCurveCurrent.CaculateIntensity(spelists))
//                 //{

//                 //    // Msg.Show("计算错误");
//                 //    ic.CalibrateIn = ic.OriginalIn = 1;
//                 //    ic.Save();
//                 //    continue;
//                 //}
//                 IntensityCalibration ic = WorkCurveHelper.WorkCurveCurrent.IntensityCalibration.First(w => w.Element.Equals(ElemName));
//                 int scanIndex = listScanElems.FindIndex(w => w == ElemName);
//                 if (scanIndex < 0)
//                 {
//                     ic.OriginalIn = ic.CalibrateIn;
//                 }
//                 else
//                 {
//                     SpecListEntity[] spelists = new SpecListEntity[1];
//                     spelists[0] = listScanPureSpec[scanIndex];
//                     if (!WorkCurveHelper.WorkCurveCurrent.CaculateIntensity(spelists))
//                     {
//                         Msg.Show("计算错误");
//                         ic.OriginalIn = ic.CalibrateIn;
//                     }
//                     else
//                     {
//                         ic.OriginalIn = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Intensity;
//                     }
//                 }
//                 ic.Save();
//             }
//           WorkCurveHelper.WorkCurveCurrent.InCalType = _calibrateType;
//           WorkCurveHelper.WorkCurveCurrent.Save();
//         }
//         catch (Exception ex)
//         {
//             Msg.Show(ex.Message);
//         }
//         //if (frmStatus != null)
//         //{
//         //    //frmStatus.Close();
//         //    Application.OpenForms["correctstatus"].Close();
//         //    Application.OpenForms["correctstatus"].Dispose();
//         //    frmStatus = null;
//         //}
     
//     } 
//}
//}
