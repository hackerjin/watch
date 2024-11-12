using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using Skyray.Controls;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using Skyray.Language;
using System.Xml;
using System.Data.SQLite;
using System.Data;
using System.Configuration;
using Skyray.Print;
using System.IO;
using Skyray.EDX.Common.Component;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Globalization;
using Lephone.Data;
using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.Win32;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.App;


namespace Skyray.UC
{

    public class SelectPointInfo
    {
        public int Number { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

    }
    /// <summary>
    /// 线程对象
    /// </summary>
    public class ThreadInfo
    {
        /// <summary>
        /// 线程的名称
        /// </summary>
        public string ThreadName { get; set; }

        /// <summary>
        /// 主界面中的标题
        /// </summary>
        public string MainFormTitle { get; set; }

        /// <summary>
        /// 主界面的功能
        /// </summary>
        public FuncType Function { get; set; }

        public AppStyle StyleInfo { get; set; }

        /// <summary>
        /// 主界面的图标
        /// </summary>
        public Icon IconMain { get; set; }

        /// <summary>
        /// 是否显示闪屏
        /// </summary>
        public bool ShowSplash { get; set; }

        /// <summary>
        /// 闪屏背景图片
        /// </summary>
        public Image SplasherBackGroupImage { get; set; }

        /// <summary>
        /// 是否启用日志
        /// </summary>
        public bool EnableLog { get; set; }

        public ThreadInfo()
        {
            ThreadName = "EDXRF";
        }

    }

    /// <summary>
    /// 程序执行功能
    /// </summary>
    public class EDXRFHelper
    {
        /// <summary>
        /// 加密狗 
        /// </summary>
        public delegate bool LoadDataSource();
        public static event LoadDataSource OnLoadDataSource;

        //public static void StartMainThread(ThreadInfo info)
        //{

        //    Application.ThreadException += new ThreadExceptionEventHandler(CustomExceptionHandler.OnThreadException);
        //    Lang.Model = new LanguageModel();//
        //    Skyray.Language.Lang.Model.InfoTypes.AddRange(new Type[]
        //    {
        //        typeof(Skyray.EDX.Common.Info),
        //        typeof(Skyray.EDXRFLibrary.LibraryInfo),
        //        typeof(Skyray.Print.PrintInfo),
        //        typeof(Skyray.Controls.CommonsInfo),
        //        typeof(Skyray.Controls.Extension.XRFInfo),
        //        typeof(Skyray.Camera.CameraInfo)
        //    });
        //    if (!Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang)
        //    {
        //        Skyray.Language.Lang.Model.ChangeLanguage();
        //    }

        //    if (info.EnableLog) Log.InitLog();//日志记录

        //    bool createdNew = false;//记录用户是否已经打开了应用程序       
        //    Ranges ranges = new Ranges();//界面输入控件边界信息
        //    Mutex m = new Mutex(true, info.ThreadName, out createdNew);//创建互斥变量
        //    WorkCurveHelper.deviceMeasure = new DeviceMeasure();
        //    WorkCurveHelper.DeviceCurrent = Device.FindOne(w => w.IsDefaultDevice == true);
        //    if (WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.ComType != ComType.BlueTooth && WorkCurveHelper.DeviceCurrent.ComType != ComType.FPGA)
        //    {
        //        WorkCurveHelper.deviceMeasure.CreateInitalize();
        //        MotorInstance.LoadDLL(MotorInstance.UpdateKeyFile, WorkCurveHelper.DeviceCurrent);
        //    }
        //    if (createdNew)
        //    {
        //        if (new FrmLogon().ShowDialog() == DialogResult.OK)
        //        {
        //            //MainForm from = new MainForm(info.StyleInfo);
        //            //Skyray.EDX.Common.Component.MotorInstance.LoadDLL();//加载动态库,Edit by WZW

        //            //if (WorkCurveHelper.DeviceCurrent.IsPassward) AppStartCheckDog();//仅提示用户

        //            //from.Icon = info.IconMain;
        //            //Application.Run(from);
        //            //m.ReleaseMutex();
        //        }
        //    }
        //    else
        //    {
        //        Msg.Show(Info.OnlySingleInstance);//提示用户只能打开一次软件 
        //        Environment.Exit(1);
        //    }
        //}

        private static void AppStartCheckDog()
        {
            Port port = WorkCurveHelper.deviceMeasure.interfacce.port;
            if (port is UsbPort)
            {
                var dog = Skyray.EDX.Common.Component.MotorInstance.dog;
                if (dog == null)
                {
                    dog = new Dogy();
                    dog.Start();
                }
                if (!dog.IsFoundPasswordDog())
                {
                    //string sql = "select value from languagedata where language_id="
                    //  + Lang.Model.CurrentLangId + " and key='Info.NotFindDogy'";
                    //Info.NotFindDogy = Lephone.Data.DbEntry.GetContext("Lang").ExecuteScalar(sql).ToString();
                    Msg.Show(Info.NotFindDogy);//提示未找到密码狗 
                    System.Environment.Exit(0);
                }
                //任意加密的动态库都可以读取加密狗的信息
                //Console.WriteLine(dog.LeftDays + "Day " + dog.LeftHours + "Hour " + dog.LeftSeconds + "Second");
            }
        }

       
          private static OpenFileDialog fileDialog = new OpenFileDialog();

          public static List<SpecListEntity> GetReturnSpectrum(bool isCaculte,bool isVisual)
          {
            Skyray.EDX.Common.IApplication.IFactory factory = null;
            List<SpecListEntity> entityes = new List<SpecListEntity>();
            if (WorkCurveHelper.SelectShowType == 1)
            {
                UCSelectSpecType selectType = new UCSelectSpecType();
                WorkCurveHelper.OpenUC(selectType, false);
                if (selectType.dialogResult != DialogResult.OK)//未选择标样
                    return null;
                fileDialog.Filter = "Spe files (*.Spe)|*.Spe";
                if (WorkCurveHelper.EditionType == TotalEditionType.EDXRF)
                {
                    factory = new EDXRFImplement();
                    fileDialog.Filter = "XML files (*.xml)|*.xml";
                }
                else if (WorkCurveHelper.EditionType == TotalEditionType.ROHS4)
                    factory = new ROHSImplement();
                else if (WorkCurveHelper.EditionType == TotalEditionType.ROHS3)
                    factory = new RoHS3Implement();
                else if (WorkCurveHelper.EditionType == TotalEditionType.FPThick)
                    factory = new FPThickImplement();
                else if (WorkCurveHelper.EditionType == TotalEditionType.Thick800A)
                    factory = new Thick800AImplement();

                else if (WorkCurveHelper.EditionType == TotalEditionType.XFP2)
                    factory = new XRFImplement();
                else if (WorkCurveHelper.EditionType == TotalEditionType.XRF)
                    factory = new XRFDelphiImp();
                fileDialog.Multiselect = true;
                if (factory != null && fileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var tempfile in fileDialog.FileNames)
                    {
                        //WorkCurveHelper.SelectSpectrumPath = fileDialog.FileName;
                        // SpecListEntity temp = factory.LoadSpecFactory(fileDialog.FileName);
                        WorkCurveHelper.SelectSpectrumPath = tempfile;
                        SpecListEntity temp = factory.LoadSpecFactory(tempfile);
                        if (temp == null)
                            return null;
                        entityes.Add(temp);
                         if (selectType.IsExport)
                        {
                           ExportSpecType2(1, tempfile + "_export", temp);
                        }
                    }
                }
            }
            else
            {
                if (WorkCurveHelper.SaveType == 0)
                {
                    if (!isVisual)
                    {
                        SelectSample sample = new SelectSample(AddSpectrumType.OpenStandardSpec);//标样谱
                        sample.IsCaculate = false;
                        WorkCurveHelper.OpenUC(sample, false);
                        if (sample.DialogResult != DialogResult.OK)//未选择标样
                            return null;
                        entityes = sample.SelectedSpecList;
                    }
                    else
                    {
                        SelectSample sample = new SelectSample(new List<SpecListEntity>());
                        WorkCurveHelper.OpenUC(sample, false, Info.OpenVirtualSpec,true);
                        if (DialogResult.OK != sample.dialogResult) return null;
                        entityes = sample.SelectedSpecList;
                    }
                }
                else
                {
                    fileDialog.Filter = "Spec files (*.Spec)|*.Spec";
                     if (fileDialog.ShowDialog() == DialogResult.OK)
                     {
                         //SpecListEntity returnTT = WorkCurveHelper.DataAccess.Query(fileDialog.SafeFileName.Replace(".Spec", ""));
                         SpecListEntity returnTT = WorkCurveHelper.DataAccess.Query(fileDialog.FileName);
                         if (returnTT == null)
                             return null;
                         entityes.Add(returnTT);
                     }
                }
            }
            if (isCaculte)
            {
                DifferenceDevice.interClassMain.OpenWorkSpec(entityes, isCaculte);
            }
            return entityes;
        }


          private static void ExportSpecType1(int count, string path, SpecListEntity speclist)
          {
              ISpectrumDAL dal = WorkCurveHelper.GetExportType(path);
              SpecListEntity back = speclist;
              back.DeviceName = WorkCurveHelper.DeviceCurrent.Name;
              dal.Save(back);
          }

          private static void ExportSpecType2(int count, string path, SpecListEntity speclist)
          {
              List<SpecListEntity> lstDel = new List<SpecListEntity>();
              SpecListEntity back = speclist;
              int conditionCount = 0;
              foreach (var s in back.Specs)
              {
                  string fileName = path + back.Name + "_" + conditionCount.ToString() + ".txt";
                  using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                  {
                      using (StreamWriter sw = new StreamWriter(fileStream))
                      {
                          sw.WriteLine("TestTime=" + s.UsedTime);
                          sw.WriteLine("TubCurrent=" + s.TubCurrent);
                          sw.WriteLine("TubVoltage=" + s.TubVoltage);
                          sw.WriteLine("Filter=1");
                          sw.WriteLine("Collimator=1");
                          sw.WriteLine("Vacuumed=false");
                          sw.WriteLine("AdjustRate=false");
                          sw.WriteLine("Channel=" + s.SpecDatas.Length);
                          sw.WriteLine("Data=");
                          StringBuilder data = new StringBuilder();
                          int[] intSpec = Helper.ToInts(s.SpecData);
                          //int[] intSpec = s.SpecDatas;
                          foreach (var m in intSpec)
                          {
                              data.Append(m + "\r\n");
                          }
                          sw.WriteLine(data.ToString());
                      }
                  }
                  conditionCount++;
              }
          }



        public static bool LoadLoadSourceEvent()
        {
            if (OnLoadDataSource != null)
              return  OnLoadDataSource();
            return false;
        }
        public static void DirectPrintHelper()
        {
            string reportName = ReportTemplateHelper.LoadReportName();
            if (reportName.IsNullOrEmpty())
            {
                Msg.Show(Info.NoSelectTemplate);
                return;
            }
            if (WorkCurveHelper.ExcelPath.IsNullOrEmpty())
            {
                ReportTemplateHelper.SaveSpecifiedValueandCreate("ExcelPath", "Path", Application.StartupPath + "\\Report");
                WorkCurveHelper.ExcelPath = Application.StartupPath + "\\Report";
            }
            string templatePath = Application.StartupPath + "\\PrintTemplate";
            if (Directory.Exists(templatePath))
            {
                //修改：何晓明 20111129 报告命名设置 赋值设置名称
                //TreeNodeInfo info = DirectPrintLibcs.lst.Find(w => w.Name == "SampleName");
                //string nowTime = DateTime.Now.ToString("yyyyMMddhhmmss");
                //if (info != null)                    
                //    DirectPrintLibcs.LoadTemplate(templatePath + "\\" + reportName, WorkCurveHelper.ExcelPath, nowTime + "_" + info.TextValue);                   
                    
                //else
                //    DirectPrintLibcs.LoadTemplate(templatePath + "\\" + reportName, WorkCurveHelper.ExcelPath);
                //赋值设置的名称
                DirectPrintLibcs.LoadTemplate(templatePath + "\\" + reportName, WorkCurveHelper.ExcelPath, ReportName);
                //
            }
        }

        public static SpecListEntity FirstSpectList { get; set; }
        public static WorkCurve FirstWorkCurve { get; set; }
        public static long FirstHistoryRecordId { get; set; }
        public static string ReportName { get { return DifferenceDevice.interClassMain.GetDefineReportName(FirstSpectList, FirstWorkCurve,FirstHistoryRecordId); } }
        public static void NewPrintDirectPrintHelper(int RecordCount)
        {
            //string Edition = "";
            //if (DifferenceDevice.IsAnalyser) Edition = "EDXRF";
            //else if (DifferenceDevice.IsRohs) Edition = "Rohs";
            //else if (DifferenceDevice.IsThick) Edition = "FPThick";
            //else if (DifferenceDevice.IsXRF) Edition = "XRF";

            string reportName = ReportTemplateHelper.LoadReportName();
            string MulitName="";
            string path = Application.StartupPath + "\\PrintTemplate\\template.xml";
            try
            {
                XmlDocument doc = new XmlDocument();

                if (File.Exists(path))
                {
                    doc.Load(path);
                    XmlNodeList rootNodeList = doc.SelectNodes("application/ReportTemplate/" + ReportTemplateHelper.Edition + "/" + WorkCurveHelper.LanguageShortName + "/TemplateName");
                    if (rootNodeList != null)
                    {
                        foreach (XmlNode rootNode in rootNodeList)
                        {
                            if(rootNode.InnerText==reportName)
                            MulitName = rootNode.Attributes["Mulit"].Value;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Msg.Show(e.Message);

            }

            if (RecordCount > 1)
            {
                if (MulitName != "")
                reportName = MulitName;
            }

            if (reportName.IsNullOrEmpty())
            {
                Msg.Show(Info.NoSelectTemplate);
                return;
            }
            if (WorkCurveHelper.ExcelPath.IsNullOrEmpty())
            {
                ReportTemplateHelper.SaveSpecifiedValueandCreate("ExcelPath", "Path", Application.StartupPath + "\\Report");
                WorkCurveHelper.ExcelPath = Application.StartupPath + "\\Report";
            }
            string templatePath = Application.StartupPath + "\\PrintTemplate";
            if (Directory.Exists(templatePath))
            {
                //修改：何晓明 20111129 报告命名设置 赋值设置名称
                //TreeNodeInfo info = DirectPrintLibcs.lst.Find(w => w.Name == "SampleName");
                //string nowTime = DateTime.Now.ToString("yyyyMMddhhmmss");
                //if (info != null)
                //    DirectPrintLibcs.LoadTemplate(templatePath + "\\" + reportName, WorkCurveHelper.ExcelPath, nowTime + "_" + info.TextValue);
                //else
                //    DirectPrintLibcs.LoadTemplate(templatePath + "\\" + reportName, WorkCurveHelper.ExcelPath);
                DirectPrintLibcs.LoadTemplate(templatePath + "\\" + reportName, WorkCurveHelper.ExcelPath, ReportName);
                //
            }
        }

        

        public static void DirectPrintHelper(string templateName)
        {
            if (WorkCurveHelper.ExcelPath.IsNullOrEmpty())
            {
                ReportTemplateHelper.SaveSpecifiedValueandCreate("ExcelPath", "Path", Application.StartupPath + "\\Report");
                WorkCurveHelper.ExcelPath = Application.StartupPath + "\\Report";
            }
            string templatePath = Application.StartupPath + "\\PrintTemplate";
            string nowTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            if (Directory.Exists(templatePath))
            {
                //修改：何晓明 20111129 报告命名设置 赋值设置名称
                //TreeNodeInfo info = DirectPrintLibcs.lst.Find(w => w.Name == "SampleName");
                //if (info != null)
                //    DirectPrintLibcs.LoadTemplate(templatePath + "\\" + templateName, WorkCurveHelper.ExcelPath, nowTime + "_" + info.TextValue);
                //else
                //    DirectPrintLibcs.LoadTemplate(templatePath + "\\" + templateName, WorkCurveHelper.ExcelPath);
                DirectPrintLibcs.LoadTemplate(templatePath + "\\" + templateName, WorkCurveHelper.ExcelPath, ReportName);
                //
            }
        }

        public static void DirectPrint()
        {
            string reportName = ReportTemplateHelper.LoadReportName();
            if (reportName.IsNullOrEmpty())
            {
                Msg.Show(Info.NoSelectTemplate);
                return;
            }
            string templatePath = Application.StartupPath + "\\PrintTemplate";
            string excelPath = Application.StartupPath + "\\Report";
            if (Directory.Exists(templatePath) && Directory.Exists(excelPath))
            {
                //TreeNodeInfo info = DirectPrintLibcs.lst.Find(w => w.Name == "SampleName");
                //if (info != null)
                    DirectPrintLibcs.DirectPrint(templatePath + "\\" + reportName);
            }
        }


        public static void NewDirectPrint(int RecordCount)
        {
            string Edition = "";
            if (DifferenceDevice.IsAnalyser) Edition = "EDXRF";
            else if (DifferenceDevice.IsRohs) Edition = "Rohs";
            else if (DifferenceDevice.IsThick) Edition = "FPThick";
            else if (DifferenceDevice.IsXRF) Edition = "XRF";

            string reportName = ReportTemplateHelper.LoadReportName();
            string MulitName = "";
            string path = Application.StartupPath + "\\PrintTemplate\\template.xml";
            try
            {
                XmlDocument doc = new XmlDocument();

                if (File.Exists(path))
                {
                    doc.Load(path);
                    XmlNodeList rootNodeList = doc.SelectNodes("application/ReportTemplate/" + Edition + "/" + WorkCurveHelper.LanguageShortName + "/TemplateName");
                    if (rootNodeList != null)
                    {
                        foreach (XmlNode rootNode in rootNodeList)
                        {
                            if (rootNode.InnerText == reportName)
                                MulitName = rootNode.Attributes["Mulit"].Value;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Msg.Show(e.Message);

            }

            if (RecordCount > 1)
            {
                if (MulitName != "")
                    reportName = MulitName;
            }


            if (reportName.IsNullOrEmpty())
            {
                Msg.Show(Info.NoSelectTemplate);
                return;
            }
            string templatePath = Application.StartupPath + "\\PrintTemplate";
            string excelPath = Application.StartupPath + "\\Report";
            if (Directory.Exists(templatePath) && Directory.Exists(excelPath))
            {
                //TreeNodeInfo info = DirectPrintLibcs.lst.Find(w => w.Name == "SampleName");
                //if (info != null)
                DirectPrintLibcs.DirectPrint(templatePath + "\\" + reportName);
            }
        }


   

        /// <summary>
        /// 转换到主界面窗口中
        /// </summary>
        /// <param name="control"></param>
        public static void GotoMainPage(UCMultiple control)
        {
            if (control.ParentForm != null)
                control.ParentForm.Close();//关闭窗体
            else
            {
                if (control.TopLevelControl != null)
                {
                    Control[] controls = (control.TopLevelControl).Controls.Find("MainPage", true);
                    if (controls != null && controls.Length > 0)
                        controls[0].Visible = true;
                    control.ExcuteEndProcess(null);
                }
            }
        }

        public static List<TreeNodeInfo> PrintTreeNodeList = new List<TreeNodeInfo>();


     


        public static bool LoadTemplate(List<TreeNodeInfo> listInfo,WorkCurve workCurve,SpecListEntity specListSource)
        {
              if (listInfo== null || specListSource.Specs == null || specListSource.Specs.Length == 0 || workCurve == null)
                    return false;
                #region 标签
                TreeNodeInfo info = listInfo.Find(w => w.Name == "Label1");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    Type = CtrlType.Label,
                    Name = "Label1",
                    Text = Info.AddressContext,
                    Caption = Info.Address
                });

                info = listInfo.Find(w => w.Name == "Label2");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    Type = CtrlType.Label,
                    Name = "Label2",
                    Text = Info.TelephoneContext,
                    Caption = Info.Telephone
                });

                info = listInfo.Find(w => w.Name == "Label3");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    Type = CtrlType.Label,
                    Name = "Label3",
                    Text = Info.MailContext,
                    Caption = Info.Mail
                });

                info = listInfo.Find(w => w.Name == "Label4");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    Type = CtrlType.Label,
                    Name = "Label4",
                    Text = Info.NetWorkAddressContext,
                    Caption = Info.NetWorkAddress
                });

                info = listInfo.Find(w => w.Name == "Label5");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    Type = CtrlType.Label,
                    Name = "Label5",
                    Text = Info.CompanyNameContext,
                    Caption = Info.CompanyName
                });
                #endregion
                #region 初始化参数
                if (workCurve.Condition == null || workCurve.Condition.InitParam == null)
                    return false;
                var initParam = workCurve.Condition.InitParam;
                info = listInfo.Find(w => w.Name == "InitParam.Channel");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    GroupName = Info.InitParam,
                    Type = CtrlType.Field,
                    Name = "InitParam.Channel",

                    Caption = Info.InitalChann,//初始化通道
                    Text = Info.InitalChann,//初始化通道
                    TextValue = initParam.Channel.ToString(),
                });

                info = listInfo.Find(w => w.Name == "InitParam.InitalElem");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    GroupName = Info.InitParam,
                    Type = CtrlType.Field,
                    Name = "InitParam.InitalElem",
                    Caption = Info.InitalElem,//初始化元素
                    Text = Info.InitalElem,//初始化元素
                    TextValue =initParam.ElemName ==null?"": initParam.ElemName.ToString()
                });
                info = listInfo.Find(w => w.Name == "InitParam.Filter");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    GroupName = Info.InitParam,
                    Type = CtrlType.Field,
                    Name = "InitParam.Filter",
                    Caption = Info.Filter,//管流
                    Text = Info.Filter,//管流
                    TextValue = initParam.Filter.ToString()
                });

                info = listInfo.Find(w => w.Name == "InitParam.Collimator");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    GroupName = Info.InitParam,
                    Type = CtrlType.Field,
                    Name = "InitParam.Collimator",
                    Caption = Info.Collimator,//管流
                    Text = Info.Collimator,//管流
                    TextValue =initParam.Collimator.ToString()
                });

                info = listInfo.Find(w => w.Name == "InitParam.ChannelError");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    GroupName = Info.InitParam,
                    Type = CtrlType.Field,
                    Name = "InitParam.ChannelError",
                    Caption = Info.ChannelError,//管流
                    Text = Info.ChannelError,//管流
                    TextValue =initParam.ChannelError.ToString()
                });

                info = listInfo.Find(w => w.Name == "InitParam.Gain");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    GroupName = Info.InitParam,
                    Type = CtrlType.Field,
                    Name = "InitParam.Gain",
                    Caption = Info.Gain,//管流
                    Text = Info.Gain,//管流
                    TextValue =initParam.Gain.ToString()
                });
                info = listInfo.Find(w => w.Name == "InitParam.FineGain");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    GroupName = Info.InitParam,
                    Type = CtrlType.Field,
                    Name = "InitParam.FineGain",
                    Caption = Info.FineGain,//管流
                    Text = Info.FineGain,//管流
                    TextValue = initParam.FineGain.ToString()
                });


                #endregion
                #region 工作曲线
                if (string.IsNullOrEmpty(workCurve.Name) || string.IsNullOrEmpty(workCurve.Condition.Name))
                    return false;
                var condition = workCurve.Condition;
                info = listInfo.Find(w => w.Name == "conditionName");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    GroupName = Info.Curve,
                    Type = CtrlType.Field,
                    Name = "conditionName",

                    Caption = Info.Condition,//测量条件名称
                    Text = Info.Condition,////测量条件名称
                    TextValue = condition.Name
                });

                info = listInfo.Find(w => w.Name == "curveName");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    GroupName = Info.Curve,
                    Type = CtrlType.Field,
                    Name = "curveName",

                    Caption = Info.CurveName,//工作曲线名称
                    Text = Info.CurveName,////工作曲线名称
                    TextValue = workCurve.Name
                });

                info = listInfo.Find(w => w.Name == "CurveVoltage");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    GroupName = Info.Curve,
                    Type = CtrlType.Field,
                    Name = "CurveVoltage",

                    Caption = Info.Voltage,//工作曲线名称
                    Text = Info.Voltage,////工作曲线名称
                    //TextValue = (DifferenceDevice.interClassMain.spec == null ? "" : (DifferenceDevice.interClassMain.spec.DeviceParameter == null ? "" : DifferenceDevice.interClassMain.spec.DeviceParameter.TubVoltage.ToString()))
                    TextValue = (specListSource == null ? "" : (specListSource.Specs.Length > 0? specListSource.Specs[0].TubVoltage.ToString() : "0"))
                });

                info = listInfo.Find(w => w.Name == "curveCurrent");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    GroupName = Info.Curve,
                    Type = CtrlType.Field,
                    Name = "curveCurrent",

                    Caption = Info.Current,//工作曲线名称
                    Text = Info.Current,////工作曲线名称
                    //TextValue = (DifferenceDevice.interClassMain.spec == null ? "" : (DifferenceDevice.interClassMain.spec.DeviceParameter == null ? "" : DifferenceDevice.interClassMain.spec.DeviceParameter.TubCurrent.ToString()))
                    TextValue = (specListSource == null ? "" : (specListSource.Specs.Length > 0? specListSource.Specs[0].TubCurrent.ToString() : "0"))
                });

                info = listInfo.Find(w => w.Name == "MeasureData");
                if (info != null)
                    listInfo.Remove(info);

                listInfo.Add(new TreeNodeInfo
                {
                    GroupName = Info.Curve,
                    Type = CtrlType.Field,
                    Name = "MeasureData",

                    Caption = Info.SpecDate,//工作曲线名称
                    Text = Info.SpecDate,////工作曲线名称
                    TextValue = (specListSource == null && specListSource.SpecDate!=null ? "" : specListSource.SpecDate.ToString())
                });

                var spec = specListSource.Specs[0];
               
                if (spec != null)
                {
                    //List<HistoryRecord> historyRecordList = HistoryRecord.FindBySql("select * from historyrecord  where  speclistname='" + specListSource.Name + "' and  WorkCurveId=" +WorkCurve.FindOne(w=>w.Name==specListSource.WorkCurveName.ToString()).Id);
                    //string samplename = (historyRecordList == null || historyRecordList.Count == 0) ? "" : historyRecordList[0].SampleName;
                    info = listInfo.Find(w => w.Name == "SampleName");
                    if (info != null)
                        listInfo.Remove(info);
                    listInfo.Add(new TreeNodeInfo
                    {
                        GroupName = Info.SampleInfo,
                        Type = CtrlType.Field,
                        Name = "SampleName",

                        Caption = Info.SampleName,//样品名称
                        Text = Info.SampleName,//样品名称
                        TextValue = specListSource.SampleName
                        //TextValue = string.IsNullOrEmpty(spec.Name) ? "" : spec.Name
                    });

                    info = listInfo.Find(w => w.Name == "Shape");
                    if (info != null)
                        listInfo.Remove(info);
                    listInfo.Add(new TreeNodeInfo
                    {
                        GroupName = Info.SampleInfo,
                        Type = CtrlType.Field,
                        Name = "Shape",

                        Caption = Info.Shape,//样品名称
                        Text = Info.Shape,//样品名称
                        TextValue = (specListSource == null || string.IsNullOrEmpty(specListSource.Shape)) ? "" : specListSource.Shape
                    });

                    //修改：何晓明 2011-05-24 增加重量信息
                    info = listInfo.Find(w => w.Name == "Weight");
                    if (info != null)
                        listInfo.Remove(info);
                    listInfo.Add(new TreeNodeInfo
                    {
                        GroupName = Info.SampleInfo,
                        Type = CtrlType.Field,
                        Name = "Weight",

                        Caption = Info.Weight,
                        Text = Info.Weight,
                        TextValue = (specListSource == null||specListSource.Weight==null || !specListSource.Weight.HasValue) ? "" : specListSource.Weight.Value.ToString("f"+WorkCurveHelper.SoftWareContentBit.ToString())
                    });
                    info = listInfo.Find(w => w.Name == "Supplier");
                    if (info != null)
                        listInfo.Remove(info);

                    listInfo.Add(new TreeNodeInfo
                    {
                        GroupName = Info.SampleInfo,
                        Type = CtrlType.Field,
                        Name = "Supplier",

                        Caption = Info.Supplier,//样品名称
                        Text = Info.Supplier,//样品名称
                        TextValue = (specListSource == null || string.IsNullOrEmpty(specListSource.Supplier)) ? "" : specListSource.Supplier
                    });

                    info = listInfo.Find(w => w.Name == "MeasureTimeSample");
                    if (info != null)
                        listInfo.Remove(info);

                    listInfo.Add(new TreeNodeInfo
                    {
                        GroupName = Info.SampleInfo,
                        Type = CtrlType.Field,
                        Name = "MeasureTimeSample",

                        Caption = Info.MeasureTime,//样品名称
                        Text = Info.MeasureTime,//样品名称
                        TextValue =spec.SpecTime.ToString()
                    });
                    info = listInfo.Find(w => w.Name == "SampleOperator");
                    if (info != null)
                        listInfo.Remove(info);
                    listInfo.Add(new TreeNodeInfo
                    {
                        GroupName = Info.SampleInfo,
                        Type = CtrlType.Field,
                        Name = "SampleOperator",

                        Caption = Info.Operator,//样品名称
                        Text = Info.Operator,//样品名称
                        TextValue = (specListSource == null || specListSource.Operater == null) ? "" : specListSource.Operater
                    });

                  
                }
                #endregion
                #region 图形
                var chat = DifferenceDevice.interClassMain.XrfChart;

                if (chat == null || chat.Width == 0 || chat.Height == 0)
                    return false;

                var bitmap = new Bitmap(chat.Width, chat.Height);

                bool b1 = chat.IsShowHScrollBar;
                bool b2 = chat.IsShowVScrollBar;
                chat.IsShowHScrollBar = false;
                chat.IsShowVScrollBar = false;
                chat.DrawToBitmap(bitmap, chat.Bounds);
                chat.IsShowHScrollBar = b1;
                chat.IsShowVScrollBar = b2;

                info = listInfo.Find(w => w.Name == "putu");
                if (info != null)
                    listInfo.Remove(info);
                listInfo.Add(new TreeNodeInfo
                {
                    Type = CtrlType.Image,
                    Name = "putu",
                    Caption = Info.SampleGraph,//工作曲线名称
                    Text = Info.SampleGraph,////工作曲线名称
                    Image = bitmap
                });

                //var specList = DifferenceDevice.interClassMain.specList;
                //if (specListSource != null && specListSource.Image != null && !specListSource.ImageShow && specListSource.Image.Length>0)
                //{
                //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(specListSource.Image))
                //    {
                //        Bitmap b = new Bitmap(ms);
                //        if (b != null)
                //        {
                //            info = listInfo.Find(w => w.Name == "SampleGraphic");
                //            if (info != null)
                //                listInfo.Remove(info);
                //            listInfo.Add(new TreeNodeInfo
                //            {
                //                Type = CtrlType.Image,
                //                Name = "SampleGraphic",
                //                Caption = Info.SampleImage,//工作曲线名称
                //                Text = Info.SampleImage,////工作曲线名称
                //                Image = b
                //            });
                //        }
                //    }
                //}

                if (specListSource != null && specListSource.ImageShow)
                {
                    string fileNameFull = WorkCurveHelper.SaveSamplePath + "\\" + specListSource.Name + ".jpg";
                    FileInfo infoIf = new FileInfo(fileNameFull);
                    if (infoIf.Exists)
                    {
                        Image iamge = Image.FromFile(fileNameFull);
                        info = listInfo.Find(w => w.Name == "SampleGraphic");
                        if (info != null)
                            listInfo.Remove(info);
                        listInfo.Add(new TreeNodeInfo
                        {
                            Type = CtrlType.Image,
                            Name = "SampleGraphic",
                            Caption = Info.SampleImage,//工作曲线名称
                            Text = Info.SampleImage,////工作曲线名称
                            Image = iamge
                        });
                    }
                }
                #endregion
                return true;
        }
       
        public static void ReloadCurve()
        {
            string sql = @"select * from WorkCurve a inner join Condition b on a.Condition_Id = b.Id 
            inner join Device c on b.Device_Id=c.Id where c.Id=" + WorkCurveHelper.DeviceCurrent.Id;
            sql += " and a.FuncType=" + (int)FuncType.Rohs + " and b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id;
            var listCuve = WorkCurve.FindBySql(sql);
            //Curve curveMessage = new Curve();
            //curveMessage.type = DataGridViewType.CurveList;
            //curveMessage.IsValidate = true;
            //MainForm.localTaskList.Add(curveMessage);

            //curveMessage = new Curve();
            //List<CurveItem> curveItemList = new List<CurveItem>();
            //foreach (WorkCurve curve in listCuve)
            //{
            //    CurveItem item = new CurveItem(curve.Name, curve.CalcType.ToString(), curve.ConditionName, curve.Id);
            //    curveItemList.Add(item);
            //}
            //curveMessage.rowNumber = curveItemList.Count;
            //curveMessage.curveResult.AddRange(curveItemList);
            //curveMessage.IsValidate = false;
            //curveMessage.ConditionType = 0;
            //curveMessage.type = DataGridViewType.CurveList;
            //MainForm.localTaskList.Add(curveMessage);
        }

        public static void DisplayWorkCurveControls()
        {
            for (int i = 0; i < WorkCurveHelper.NaviItems.Count; i++)
            {
                if (WorkCurveHelper.NaviItems[i].Name.Equals("SurfaceSourceSettings") && WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
                {
                    WorkCurveHelper.NaviItems[i].Enabled = true;
                    WorkCurveHelper.NaviItems[i].EnabledControl = true;
                    continue;
                }
                else if (WorkCurveHelper.NaviItems[i].Name.Equals("SurfaceSourceSettings") && WorkCurveHelper.DeviceCurrent.ComType != ComType.FPGA)
                {
                    WorkCurveHelper.NaviItems[i].Enabled = false;
                    WorkCurveHelper.NaviItems[i].EnabledControl = false;
                    continue;
                }
                if (WorkCurveHelper.NaviItems[i].Name.Equals("SuperModel") && WorkCurveHelper.DeviceCurrent.HasTarget)
                {
                    WorkCurveHelper.NaviItems[i].Enabled = false;
                    WorkCurveHelper.NaviItems[i].EnabledControl = false;
                    continue;
                }
                else if (WorkCurveHelper.NaviItems[i].Name.Equals("SuperModel") && !WorkCurveHelper.DeviceCurrent.HasTarget)
                {
                    WorkCurveHelper.NaviItems[i].Enabled = false;
                    WorkCurveHelper.NaviItems[i].EnabledControl = false;
                    continue;
                }
            }
            if (WorkCurveHelper.WorkCurveCurrent == null)
            {
                for (int i = 0; i < WorkCurveHelper.NaviItems.Count; i++)
                {
                    if ((WorkCurveHelper.NaviItems[i].Name != null))
                    {
                        if (WorkCurveHelper.NaviItems[i].Name.Equals("ElementRef") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("DataOptimization") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("EditData") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("CustomFiled") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("CoeeParamSet") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("EditElement")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("FpSpecCalibrate")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("Expunction")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("Expunction2")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("RExcusionCalibrate")
                             || WorkCurveHelper.NaviItems[i].Name.Equals("EditMatch")
                             || WorkCurveHelper.NaviItems[i].Name.Equals("SettingVirtual"))
                        {
                            WorkCurveHelper.NaviItems[i].EnabledControl = false;
                        }
                    }
                }
            }
            else if (WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
            {
                for (int i = 0; i < WorkCurveHelper.NaviItems.Count; i++)
                {
                    if ((WorkCurveHelper.NaviItems[i].Name != null))
                    {
                        if (WorkCurveHelper.NaviItems[i].Name.Equals("ElementRef") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("EditData") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("DataOptimization") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("CustomFiled") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("CoeeParamSet") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("EditElement") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("ChinawareDataBase") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("ReportSpecification")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("FpSpecCalibrate")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("Expunction")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("Expunction2")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("RExcusionCalibrate")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("SettingVirtual")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("EditMatch"))
                        {
                            WorkCurveHelper.NaviItems[i].EnabledControl = true;
                        }
                        if (WorkCurveHelper.NaviItems[i].Name == "RExcusionCalibrate" && DifferenceDevice.IsThick)
                        {
                            WorkCurveHelper.NaviItems[i].EnabledControl = false;
                        }
                        //if(WorkCurveHelper.NaviItems[i].Name.Equals("DataOptimization") && (WorkCurveHelper.DeviceFunctype == FuncType.Rohs))
                        //{
                        //    WorkCurveHelper.NaviItems[i].EnabledControl = false;
                        //}
                        //if (WorkCurveHelper.NaviItems[i].Name.Equals("CustomFiled") && (WorkCurveHelper.DeviceFunctype == FuncType.Rohs || WorkCurveHelper.DeviceFunctype == FuncType.Thick))
                        //{
                        //    WorkCurveHelper.NaviItems[i].EnabledControl = false;
                        //}
                        //if (WorkCurveHelper.NaviItems[i].Name.Equals("ElementRef") && (WorkCurveHelper.DeviceFunctype == FuncType.Thick))
                        //{
                        //    WorkCurveHelper.NaviItems[i].EnabledControl = false;
                        //}
                        //if (WorkCurveHelper.NaviItems[i].Name.Equals("ChinawareDataBase") && (WorkCurveHelper.DeviceFunctype == FuncType.Rohs || WorkCurveHelper.DeviceFunctype == FuncType.Thick))
                        //{
                        //    WorkCurveHelper.NaviItems[i].EnabledControl = false;
                        //}
                        //if (WorkCurveHelper.NaviItems[i].Name.Equals("ReportSpecification") &&WorkCurveHelper.DeviceFunctype == FuncType.Thick)
                        //{
                        //    WorkCurveHelper.NaviItems[i].EnabledControl = false;
                        //}
                        //if (WorkCurveHelper.NaviItems[i].Name.Equals("Expunction") && (WorkCurveHelper.DeviceFunctype == FuncType.Rohs || WorkCurveHelper.DeviceFunctype == FuncType.Thick || WorkCurveHelper.CalcType == CalcType.EC))
                        //{
                        //    WorkCurveHelper.NaviItems[i].EnabledControl = false;
                        //}
                    }
                }
                return;
            }
            else
            {
                for (int i = 0; i < WorkCurveHelper.NaviItems.Count; i++)
                {
                    if ((WorkCurveHelper.NaviItems[i].Name != null))
                    {
                        if (WorkCurveHelper.NaviItems[i].Name.Equals("ElementRef") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("DataOptimization") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("EditData") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("CustomFiled") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("CoeeParamSet") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("ChinawareDataBase") 
                            || WorkCurveHelper.NaviItems[i].Name.Equals("ReportSpecification")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("FpSpecCalibrate")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("Expunction")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("RExcusionCalibrate")
                            || WorkCurveHelper.NaviItems[i].Name.Equals("EditMatch"))
                        {
                            WorkCurveHelper.NaviItems[i].EnabledControl = false;
                        }
                        if (WorkCurveHelper.NaviItems[i].Name == "EditElement" || WorkCurveHelper.NaviItems[i].Name.Equals("SettingVirtual"))
                        {
                            WorkCurveHelper.NaviItems[i].EnabledControl = true;
                        }
                    }
                }
            }
        }


        public static string ReadConfigXml()
        {
            string currentPath = Application.StartupPath + "\\Language\\Types.xml";
            string currentLangName = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(currentPath);

            XmlNodeList list = xmlDoc.GetElementsByTagName("Skyray");
            bool result = false;

            foreach (XmlElement element in list)
            {
                if (element.ChildNodes.Count == 0)
                    continue;
                foreach (XmlNode node in element.ChildNodes)
                {
                    if (node.Name.Equals("FullName"))
                        currentLangName = node.InnerText;
                    if (node.Name.Equals("IsCurrent") && node.InnerText.Equals("Y"))
                    {
                        result = true;
                        break;
                    }
                }

                if (result)
                    break;
            }
            return currentLangName;
        }

        public static void RecurveNextUC(ToolStripControls controlStrip)
        {
            if (controlStrip == null)
                return;
            if (controlStrip.CurrentNaviItem != null && controlStrip.CurrentNaviItem.Enabled && controlStrip.CurrentNaviItem.EnabledControl)
            {
                if (controlStrip.CurrentNaviItem.TT != null)
                {
                    WorkCurveHelper.OpenUC(controlStrip.CurrentNaviItem.TT(), false, controlStrip.CurrentNaviItem.Text,true);
                }
            }
            else if (controlStrip.CurrentNaviItem != null)
            {
                ToolStripControls toolsControl = MenuLoadHelper.MenuStripCollection.Find(w => w != null && w.Postion == controlStrip.Postion && w.preToolStripMeauItem == controlStrip);
                if (toolsControl != null)
                    RecurveNextUC(toolsControl);
            }
        }

        public static void RecurseUpUC(ToolStripControls controlStrip)
        {
            if (controlStrip == null)
                return;
            if (controlStrip.CurrentNaviItem != null && controlStrip.CurrentNaviItem.Enabled&& controlStrip.CurrentNaviItem.EnabledControl)
            {
                WorkCurveHelper.OpenUC(controlStrip.CurrentNaviItem.TT(), false, controlStrip.CurrentNaviItem.Text,true);
            }
            else if (controlStrip.CurrentNaviItem != null)
            {
                ToolStripControls toolsControl = MenuLoadHelper.MenuStripCollection.Find(w => w.Postion == controlStrip.Postion && w == controlStrip.preToolStripMeauItem);
                if (toolsControl != null)
                    RecurseUpUC(toolsControl);
            }
        }

        public static DataTable GetData(string strSql)
        {
            DataTable dt = new DataTable();
            string connectionString = Lephone.Data.DbEntry.Context.Driver.ConnectionString;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(strSql, connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public static ImageCodecInfo GetCodecInfo(ImageFormat format)
        {

            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo ici in CodecInfo)
            {

                if (ici.FormatID == format.Guid) return ici;

            }
            return null;

        }  


        public static bool UpdateSetXml(string currentLangName, out long longId)
        {
            longId = 0L;
            if (currentLangName == null)
                return false; ;
            if (currentLangName == "中文")
            {
                Languages langs = Languages.FindOne(w => w.FullName == "中文");
                if (langs == null)
                    return false;
                langs.IsDefaultLang = false;
                langs.IsCurrentLang = true;
                langs.Save();

                Languages lang = Languages.FindOne(w => w.FullName == "English");
                if (lang == null)
                    return false; ;

                lang.IsDefaultLang = false;
                lang.IsCurrentLang = false;
                lang.Save();

                longId = langs.Id;
            }

            if (currentLangName == "English")
            {
                Languages langs = Languages.FindOne(w => w.FullName == "中文");
                if (langs == null)
                    return false;
                langs.IsDefaultLang = false;
                langs.IsCurrentLang = false;
                langs.Save();

                Languages lang = Languages.FindOne(w => w.FullName == "English");
                if (lang == null)
                    return false;
                lang.IsDefaultLang = false;
                lang.IsCurrentLang = true;
                lang.Save();

                longId = lang.Id;
            }
            return true;
        }

        /// <summary>
        /// 随机生成字母数字
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GenerateRandomLetter(int Length)
        {
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string result = "";
            int n = Pattern.Length;
            Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
    
    }
    [Flags]
    public enum MINIDUMPTYPE : uint
    { 
        MiniDumpNormal = 0x00000000, 
        MiniDumpWithDataSegs = 0x00000001, 
        MiniDumpWithFullMemory = 0x00000002, 
        MiniDumpWithHandleData = 0x00000004, 
        MiniDumpFilterMemory = 0x00000008, 
        MiniDumpScanMemory = 0x00000010, 
        MiniDumpWithUnloadedModules = 0x00000020,
        MiniDumpWithIndirectlyReferencedMemory = 0x00000040, 
        MiniDumpFilterModulePaths = 0x00000080, 
        MiniDumpWithProcessThreadData = 0x00000100,
        MiniDumpWithPrivateReadWriteMemory = 0x00000200,
        MiniDumpWithoutOptionalData = 0x00000400,
        MiniDumpWithFullMemoryInfo = 0x00000800, 
        MiniDumpWithThreadInfo = 0x00001000,
        MiniDumpWithCodeSegs = 0x00002000    
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]  
    public struct ExceptionHandle
    {
        public uint ThreadId;
        public IntPtr Exception;
        [MarshalAs(UnmanagedType.Bool)]
        public bool ClientPointers;
    }

    /// <summary>
    /// 未捕获异常处理
    /// </summary>
    public class CustomExceptionHandler
    {
       
        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, ref ExceptionHandle expParam, IntPtr userStreamParam, IntPtr callbackParam);

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        static extern uint GetCurrentThreadId();

        public static void OnThreadException(object sender, ThreadExceptionEventArgs t)
        {
            //DialogResult dialogResult = DialogResult.Cancel;
            try
            {
                //CreateMiniDump();
                //dialogResult = ShowThreadExceptionDialog(t.Exception);
                if (Log.Inited) Log.Error("Fatal Error", t.Exception);  //尝试记录日志
            }
            catch
            {
                try
                {
                    Msg.Show("Fatal Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
            //if (dialogResult == DialogResult.Abort)
            //{
            //    Application.Exit();
            //}
        }

        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                //string errorMsg = "An application error occurred. Please contact the adminstrator " +
                //    "with the following information:\n\n";

                //// Since we can't prevent the app from terminating, log this to the event log.
                //if (!EventLog.SourceExists("ThreadException"))
                //{
                //    EventLog.CreateEventSource("ThreadException", "Application");
                //}

                //// Create an EventLog instance and assign its source.
                //EventLog myLog = new EventLog();
                //myLog.Source = "ThreadException";
                //myLog.WriteEntry(errorMsg + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                if (Log.Inited) Log.Error("Fatal Error", ex);
            }
            catch (Exception exc)
            {
                try
                {
                    MessageBox.Show("Fatal Non-UI Error",
                        "Fatal Non-UI Error. Could not write the error to the event log. Reason: "
                        + exc.Message, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }


        public static bool CreateMiniDump() 
        {
            DateTime endTime = DateTime.Now;
            System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
            string dt = endTime.ToString("yyyy.MM.dd.HH.mm.ss", DateTimeFormatInfo.InvariantInfo);
            string dumpFileName = AppDomain.CurrentDomain.BaseDirectory + "\\"+(DifferenceDevice.IsXRF ? "EDXX" : (DifferenceDevice.IsRohs ? "EDXR" : (DifferenceDevice.IsThick ? "EDXT" : "EDX3000"))) +"_" + dt + ".dmp";
            using (FileStream fs = new FileStream(dumpFileName, FileMode.Create))
            {
                ExceptionHandle handle = new ExceptionHandle();
                handle.ThreadId = GetCurrentThreadId();
                handle.ClientPointers = false;
                uint currentProcessId = (uint)process.Id;
                handle.Exception = System.Runtime.InteropServices.Marshal.GetExceptionPointers();
                bool bRet = false;
                bRet = MiniDumpWriteDump(process.Handle, currentProcessId, fs.SafeFileHandle, (uint)MINIDUMPTYPE.MiniDumpWithFullMemory, ref handle, IntPtr.Zero, IntPtr.Zero);
                fs.Flush();
                fs.Close();
                return bRet;
            }
        }

        /// <summary>
        /// 弹出异常对话框
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static DialogResult ShowThreadExceptionDialog(Exception e)
        {
            return Msg.Show(e.Message+ @"\n\nStack Trace:\n" + e.StackTrace, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
        }
    }

    public class CalcFwhm
    {
        private static int low;
        private static int high;

        public static void Calc(int Channel, int[] SpecData)
        {
            int[] Data = SpecData;
            double ch = fitPeakChannel(Channel, Data);
            if (!double.IsNaN(ch))
            {
                double halfValue = Data[Convert.ToInt32(ch)] * 1.0 / 2;
                double slope = DemarcateEnergyHelp.k1 * 1000;
                //求半高宽的精确边界
                double L = low + (halfValue - Data[low]) / (Data[low +1] - Data[low]);
                double H = high - (halfValue - Data[high]) / (Data[high - 1] - Data[high]);
                //double FixGaussDelta = ((H - L) * slope) / Channel;
                double FixGaussDelta = ((H - L)) / Channel;
                if (!Double.IsNaN(FixGaussDelta)&&FixGaussDelta > 0)
                {
                    WorkCurveHelper.DeviceCurrent.Detector.FixGaussDelta = FixGaussDelta;
                    string sql = "update Detector set FixGaussDelta =" + FixGaussDelta + " where Id=" + WorkCurveHelper.DeviceCurrent.Detector.Id;
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                }
            }
        }


      

        /// <summary>
        /// 拟合锋的位置
        /// </summary>
        private static double fitPeakChannel(int ch, int[] Data)
        {
            int halfValue = Data[ch] / 2;
            double sumValueChannel = 0;
            double sumValue = 0;
            low = ch - 1;
            high = ch + 1;//半高宽的边界            
            for (int i = ch - 1; i >= 0; i--)
            {
                if (Data[i] < halfValue)
                {
                    low = i;
                    break;
                }
            }
            for (int i = ch + 1; i < Data.Length; i++)
            {
                if (Data[i] < halfValue)
                {
                    high = i;
                    break;
                }
            }
            if (low > -1 && high > -1)
            {
                for (int i = low; i <= high; i++)
                {
                    sumValueChannel += i * Data[i];
                    sumValue += Data[i];
                }
            }
            double result = sumValueChannel / sumValue;
            return result;
        }
    }

    public class ColumnObject
    {
        public string Name;
        public int Index;
        public string HeadText;
        public bool Visible;

        public ColumnObject(string name, int index, string headText, bool visible)
        {
            Name = name;
            Index = index;
            HeadText = headText;
            Visible = visible;
        }
    }
}
