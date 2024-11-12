using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Skyray.Print;
using Skyray.Language;
using System.Collections.Generic;
using Skyray.Controls;
using Microsoft.Win32;
using Skyray.EDX.Common.ReportHelper;
using System.ComponentModel;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.Component;
using Skyray.API;
using System.Threading;
using Lephone.Data.Common;
using System.Configuration;
namespace Skyray.UC
{
    public class ExceptClientGrobal
    {

        /// <summary>
        /// 打印模板
        /// </summary>
        public event EventDelegate.PrintTemplateSource OnPrintTemplateSource;


        public event EventDelegate.ContinuousData OnPassingData;

        //public event EventDelegate.HistoryTemplateSave OnHistoryTemplateSave;

        /// <summary>
        /// 保存配置路径位置
        /// </summary>
        public static string fileName;

        private int modelTool;


        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public ExceptClientGrobal()
        {
        }

        /// <summary>
        /// 找到父菜单OnExcuteCaculate
        /// </summary>
        /// <param name="naviName">菜单栏及工具栏等</param>
        /// <returns></returns>
        private ToolStripControls GetPreMenuStrip(string naviName, int position)
        {
            if (MenuLoadHelper.MenuStripCollection.Count == 0)
                return null;
            var queryNavi = from vast in MenuLoadHelper.MenuStripCollection
                            where vast.Postion == position && vast.CurrentNaviItem.Name == naviName
                            select vast;
            foreach (ToolStripControls returnResult in queryNavi)
                return returnResult;
            return null;
        }


        private ToolStripControls GetPreMenuStrip(string naviName)
        {
            if (MenuLoadHelper.MenuStripCollection.Count == 0)
                return null;
            var queryNavi = from vast in MenuLoadHelper.MenuStripCollection
                            where vast.CurrentNaviItem.Name == naviName
                            select vast;
            foreach (ToolStripControls returnResult in queryNavi)
                return returnResult;
            return null;
        }

        /// <summary>
        /// 从图标中获取Image对象
        /// </summary>
        /// <param name="fileName">指定的文件名称</param>
        /// <returns></returns>
        private Image GetIcoImage(string fileName)
        {
            return GetIcoImage(fileName, ".png");
        }


        /// <summary>
        /// 获取图标
        /// </summary>
        /// <param name="fileName">指定的文件名称</param>
        /// <param name="suffix">文件的后缀</param>
        /// <returns></returns>
        private Image GetIcoImage(string fileName, string suffix)
        {
            string fullName = Application.StartupPath + "\\ICO\\" + fileName + suffix;
            if (File.Exists(fullName))
            {
                return Image.FromFile(fullName);
            }
            return Properties.Resources.Default;
        }

        #region 加载工具栏，菜单栏等
        /// <summary>
        /// 加载菜单栏，工具栏对象
        /// </summary>
        public void LoadControls(bool flag)
        {
            NaviItem naviItem = null;
            List<NaviItem> listNavi = new List<NaviItem>();
            ToolStripControls container = null;
            ToolStripControls preContainer = null;
            ToolStripControls parentContainer = null;
            //修改：何晓明 20110713 工具栏增加菜单栏
            ToolStripControls parentOtherContainer = null;
            ToolStripControls parentCamerContainer = null;
            //
            ToolStripControls parentCContainer = null;
            WorkCurveHelper.NaviItems = new List<NaviItem>();
            string[] names = new string[] { Info.Settings, Info.Condition,Info.Workgion, Info.Curve, Info.Spec, Info.Quality, 
                                            Info.Report,Info.UserManage ,Info.Tools, Info.Language,Info.Camera, Info.Helper,Info.Others,Info.Exit,Info.ChangeUser };
            string[] mainName = new string[] { "Settings", "Condition", "Workgion", "Curve", "Spec", "Quality", "Report", "UserManage", "Tools", "Language", "Camera", "Helper", "Others", "Exit", "ChangeUser"};
            for (int i = 0; i < names.Length; i++)
            {
                naviItem = new NaviItem();
                naviItem.Name = mainName[i];
                naviItem.Text = names[i];
                naviItem.Enabled = true;
                if (naviItem.Name == "UserManage")
                {
                    naviItem.ShowInToolBar = false;
                    naviItem.TT = CreateUserManager;
                    naviItem.MenuStripItem.Tag = naviItem;
                }
                else if (naviItem.Name == "Quality")
                    listNavi.Add(naviItem);   //主要为改变模式定性分析菜单栏等项的变化
                //add by chuyaqin 2011-04-22 evening
                else if (naviItem.Name == "Curve")
                {
                    listNavi.Add(naviItem);
                }
                else if (naviItem.Name == "Exit")
                {
                    naviItem.ShowInToolBar = false;
                    naviItem.MenuStripItem.Tag = naviItem;
                    naviItem.excuteRequire = Exit;
                }         
                else if (naviItem.Name == "ChangeUser")
                {
                    naviItem.ShowInToolBar = false;
                    naviItem.MenuStripItem.Tag = naviItem;
                    naviItem.excuteRequire = ExcuteChangeUser;
                }
                WorkCurveHelper.NaviItems.Add(naviItem);
                container = new ToolStripControls();
                container.Postion = i;
                container.CurrentNaviItem = naviItem;
                container.isRoot = true;
                container.preToolStripMeauItem = null;
                container.parentStripMeauItem = container;
                MenuLoadHelper.MenuStripCollection.Add(container);
            }

            parentOtherContainer = GetPreMenuStrip("Others");
            //主界面
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.MainPage;
            naviItem.Name = "MainPage";
            naviItem.Image = GetIcoImage("MainPage");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.FlagType = 0;
            WorkCurveHelper.NaviItems.Add(naviItem);
            //修改：何晓明 20110713 工具栏增加菜单栏
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("Others");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);



            //parentCamerContainer = GetPreMenuStrip("Camera");
            ////修改：何晓明 20110713 工具栏增加菜单栏
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            //preContainer = GetPreMenuStrip("Camera");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentCamerContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = CreatePreHeat;
            naviItem.Text = Info.PreHeat;
            naviItem.Name = "PreHeat";
            naviItem.Image = GetIcoImage("PreHeat");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.FunctionType = 2;
            WorkCurveHelper.NaviItems.Add(naviItem);
            //修改：何晓明 20110713 工具栏增加菜单栏
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("MainPage");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            //

            // naviItem = new NaviItem();
            // naviItem.TT = CreatePureAuCalc;
            // naviItem.Text = Info.PureAu;
            // naviItem.Name = "PureAu";
            // naviItem.Image = GetIcoImage("PureAu");
            // naviItem.BtnDropDown.Tag = naviItem;
            // naviItem.FunctionType = 2;
            //// naviItem.excuteRequire = ExcutePureAuCalc;
            // WorkCurveHelper.NaviItems.Add(naviItem);
            // //修改：何晓明 20110713 工具栏增加菜单栏
            // container = new ToolStripControls();
            // container.Version = new string[] { "EDXRF" };
            // preContainer = GetPreMenuStrip("PreHeat");
            // container.Postion = preContainer.Postion;
            // container.CurrentNaviItem = naviItem;
            // container.preToolStripMeauItem = preContainer;
            // container.parentStripMeauItem = parentOtherContainer;
            // naviItem.MenuStripItem.Tag = naviItem;
            // MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = ResolveCaculate;
            naviItem.Text = Info.ResolveCaculate;
            naviItem.Name = "ResolveCaculate";
            naviItem.Image = GetIcoImage("ResolveCaculate");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.FunctionType = 2;
            //naviItem.FunctionType = 0;
            WorkCurveHelper.NaviItems.Add(naviItem);


            //修改：何晓明 20110713 工具栏增加菜单栏
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS" };
            preContainer = GetPreMenuStrip("PreHeat");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            //

            //历史记录
            naviItem = new NaviItem();
            naviItem.TT = CreateHistoryRecord;
            naviItem.Text = Info.HistoryRecord;
            naviItem.Name = "HistoryRecord";
            //修改：何晓明 2011-07-11 去除最大化窗口
            naviItem.IsMaxnium = true;
            //
            naviItem.Image = GetIcoImage("HistoryRecord");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.FlagType = 0;
            WorkCurveHelper.NaviItems.Add(naviItem);
            //修改：何晓明 20110713 工具栏增加菜单栏
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("ResolveCaculate");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            //

            //工作条件
            naviItem = new NaviItem();
            naviItem.TT = CreateUCCondition;
            naviItem.Text = Info.OpenCondition;
            naviItem.Name = "OpenCondition";
            naviItem.Image = GetIcoImage("OpenCondition");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.FlagType = 0;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("Condition");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = preContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //设置测量时间
            naviItem = new NaviItem();
            naviItem.TT = CreateSettingMeasureTime;
            naviItem.Text = Info.MeasureTime;
            naviItem.Name = "SettingTime";
            naviItem.Image = GetIcoImage("OpenCondition");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.FlagType = 0;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("OpenCondition");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            #region 屏蔽 211109

            ////编辑工作区
            //naviItem = new NaviItem();
            //naviItem.TT = CreateUCEditWorkgion;
            //naviItem.Text = Info.EditWorkgion;
            //naviItem.Name = "EditWorkgion";
            //naviItem.Image = GetIcoImage("Workgion");
            //naviItem.BtnDropDown.Tag = naviItem;
            //naviItem.FlagType = 0;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS" };
            //preContainer = GetPreMenuStrip("Workgion");
            //parentContainer = preContainer;
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);
            #endregion
            //工作曲线
            naviItem = new NaviItem();
            naviItem.TT = CreateCurve;
            naviItem.Text = Info.OpenCurve;
            naviItem.Name = "OpenCurve";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("circle 1");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("Curve");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //元素列表
            naviItem = new NaviItem();
            naviItem.TT = CreateElement;
            naviItem.Text = Info.EditElement;
            naviItem.Name = "EditElement";
            naviItem.Image = GetIcoImage("EditElement");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.FlagType = 0;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("OpenCurve");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //影响元素
            naviItem = new NaviItem();
            naviItem.TT = CreateElementRef;
            naviItem.Text = Info.ElementRef;
            naviItem.Name = "ElementRef";
            naviItem.Image = GetIcoImage("ElementRef");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("EditElement");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            //优化因子
            naviItem = new NaviItem();
            naviItem.TT = CreateOptimization;
            naviItem.Text = Info.DataOptimization;
            naviItem.Name = "DataOptimization";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("DataOptimization");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("ElementRef");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //编辑数据
            naviItem = new NaviItem();
            naviItem.TT = CreateEditData;
            naviItem.Text = Info.EditData;
            naviItem.Name = "EditData";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("EditData");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("DataOptimization");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = CreateMatchSpec;
            naviItem.Text = Info.MatchSpec;
            naviItem.Name = "EditMatch";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("EditMatch");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "XRF", "EDXRF" };
            preContainer = GetPreMenuStrip("EditData");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);



            //自定义
            naviItem = new NaviItem();
            naviItem.TT = CreateCustomFiled;
            naviItem.Text = Info.CustomFiled;
            naviItem.Name = "CustomFiled";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("CustomFiled");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "XRF" };
            preContainer = GetPreMenuStrip("EditMatch");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.CreateIntRegion;
            naviItem.excuteRequire = CreateIntRegion;
            naviItem.Name = "CreateIntRegion";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("CreateIntRegion");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("CustomFiled");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.CaculateIntRegion;
            naviItem.excuteRequire = CaculateIntRegion;
            naviItem.Name = "CaculateIntRegion";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("CaculateIntRegion");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("CreateIntRegion");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            ////报规格
            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.ReportSpecification;
            //naviItem.Name = "ReportSpecification";
            //naviItem.excuteRequire = ExcuteReportSpecification;
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("ReportSpecification");
            //naviItem.BtnDropDown.Tag = naviItem;
            //naviItem.EnabledControl = false;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "XRF" };
            //preContainer = GetPreMenuStrip("CaculateIntRegion");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);


            ////陶瓷数据库
            //naviItem = new NaviItem();
            //naviItem.TT = CreateChinawareDataBase;
            //naviItem.Text = Info.ChinawareDataBase;
            //naviItem.excuteRequire = null;
            //naviItem.Name = "ChinawareDataBase";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("ChinawareDataBase");
            //naviItem.BtnDropDown.Tag = naviItem;
            //naviItem.EnabledControl = false;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "XRF" };
            //preContainer = GetPreMenuStrip("ReportSpecification");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //fp校正参数设置
            naviItem = new NaviItem();
            naviItem.TT = FpSpecCalibrate;
            naviItem.Text = Info.FpSpecCalibrate;
            naviItem.Name = "FpSpecCalibrate";
            naviItem.FlagType = 0;
            naviItem.EnabledControl = false;
            naviItem.Image = GetIcoImage("FpSpecCalibrate");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            // preContainer = GetPreMenuStrip("ChinawareDataBase");CaculateIntRegion
            preContainer = GetPreMenuStrip("CaculateIntRegion");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //强度偏移校正
            naviItem = new NaviItem();
            naviItem.TT = RExcusionCalibrate;
            //naviItem.TT = null;
            naviItem.Text = Info.RExcusionCalibrate;
            //naviItem.excuteRequire = null; ;
            //naviItem.excuteRequire = ExcuteAutoCorrect;
            naviItem.Name = "RExcusionCalibrate";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("RExcusionCalibrate");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("FpSpecCalibrate");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //消去值
            naviItem = new NaviItem();
            naviItem.TT = CreateExpunction;
            naviItem.Text = Info.strExpunction;
            naviItem.excuteRequire = null; ;
            naviItem.Name = "Expunction";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("Expunction");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("RExcusionCalibrate");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            //谱特殊处理
            naviItem = new NaviItem();
            naviItem.TT = CreateUCSpecialSpec;
            naviItem.Text = Info.strExpunction + "2";
            naviItem.excuteRequire = null; ;
            naviItem.Name = "Expunction2";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("Expunction");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("Expunction");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = SettingVirtualSpec;
            naviItem.Text = Info.VirtualSpec;
            naviItem.Name = "SettingVirtual";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("SettingVirtual");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "XRF", "EDXRF", "RoHS", "Thick" };
            preContainer = GetPreMenuStrip("Expunction2");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //显示峰标识
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.DisplayPeakFlag;
            naviItem.Name = "DisplayPeakFlag";
            naviItem.excuteRequire = ExcuteDisplayPeak;
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("DisplayPeakFlag");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("Quality");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //显示元素
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.DisplayElement;
            naviItem.Name = "DisplayElement";
            naviItem.excuteRequire = ExcuteDisplayElement;
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("DisplayElement");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("DisplayPeakFlag");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            //自动分析
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.AutoAnalysis;
            naviItem.Name = "AutoAnalysis";
            naviItem.excuteRequire = ExcuteAutoAnalysis;
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("AutoAnalysis");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("DisplayElement");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            //元素周期表
            naviItem = new NaviItem();
            naviItem.TT = CreateElementTable;
            naviItem.Text = Info.ManualAnalysis;
            naviItem.Name = "ManualAnalysis";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("ManualAnalysis");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("AutoAnalysis");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //分析参数
            naviItem = new NaviItem();
            naviItem.TT = CreateAnalyParam;
            naviItem.Text = Info.AnalysisParam;
            naviItem.Name = "AnalysisParam";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("AnalysisParam");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("ManualAnalysis");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //元素谱
            naviItem = new NaviItem();
            naviItem.TT = CreateElementSpectrum;
            naviItem.Text = Info.PureElementSpecData;
            naviItem.Name = "PureElementSpecData";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("PureElementSpecData");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            listNavi.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "XRF", "EDXRF" };
            preContainer = GetPreMenuStrip("AnalysisParam");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //智能模式fp校正参数设置
            naviItem = new NaviItem();
            naviItem.TT = FpSpecCalibrate;
            naviItem.Text = Info.FpSpecCalibrate;
            naviItem.Name = "FpFpSpecCalibrate";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("FpFpSpecCalibrate");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "XRF", "EDXRF" };
            preContainer = GetPreMenuStrip("PureElementSpecData");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            listNavi.Add(naviItem);

            naviItem = new NaviItem();
            naviItem.TT = AnalysisReport;
            naviItem.Text = Info.AnalysisReport;
            naviItem.Name = "AnalysisReport";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("AnalysisReport");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("PureElementSpecData");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //打开谱
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.excuteRequire = OpenSpec;
            naviItem.Text = Info.OpenSpec;
            naviItem.Name = "OpenSpec";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("OpenSpec");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("Spec");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //加对比谱
            naviItem = new NaviItem();
            naviItem.TT = CreateVirtualSpecs;
            naviItem.Text = Info.AddVirtualSpec;
            naviItem.Name = "AddVirtualSpec";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("AddVirtualSpec");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("OpenSpec");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.OpenRohs3;
            //naviItem.Name = "OpenRohs3";
            //naviItem.FlagType = 0;
            //naviItem.excuteRequire = OpenRohs3;
            //naviItem.Image = GetIcoImage("OpenRohs3");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS" };
            //preContainer = GetPreMenuStrip("AddVirtualSpec");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);


            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.OpenRohs4;
            //naviItem.Name = "OpenRohs4";
            //naviItem.FlagType = 0;
            //naviItem.excuteRequire = OpenRohs4;
            //naviItem.Image = GetIcoImage("OpenRohs4");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS" };
            //preContainer = GetPreMenuStrip("OpenRohs3");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //消本底
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.DisappearBk;
            naviItem.Name = "DisappearBk";
            naviItem.excuteRequire = DisappearBk;
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("DisappearBk");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("AddVirtualSpec");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = ComputeSampleIntensity;
            naviItem.Text = Info.ComputeSampleIntensity;
            naviItem.Name = "ComputeSampleIntensity";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("ComputeSampleIntensity");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("DisappearBk");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //计算谱的分辨率
            naviItem = new NaviItem();
            naviItem.TT = ComputeSpecResolve;
            naviItem.Text = Info.ComputeResolve;
            naviItem.Name = "ComputeSpecResolve";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("ComputeSpecResolve");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("ComputeSampleIntensity");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = ComputeMatching;
            naviItem.Text = Info.ComputeMatching;
            naviItem.Name = "ComputeMatching";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("ComputeMatching");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "XRF", "EDXRF" };
            preContainer = GetPreMenuStrip("ComputeSpecResolve");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.OpenOldSpec;
            naviItem.Name = "OpenOldSpec";
            naviItem.excuteRequire = CreateSpecType;
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("CreateSpecType");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "XRF" };
            preContainer = GetPreMenuStrip("ComputeMatching");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.LogSpectrum;
            naviItem.Name = "LogSpectrum";
            naviItem.excuteRequire = CheckLogSpectrum;
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("LogSpectrum");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("OpenOldSpec");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            //纯元素谱库
            naviItem = new NaviItem();
            naviItem.TT = CreatePureSpecLib;
            naviItem.Text = Info.PureSpecLib;
            naviItem.Name = "PureSpecLib";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("PureSpecLib");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("LogSpectrum");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //校正参数 
            naviItem = new NaviItem();
            naviItem.TT = CreateAdjustPure;
            naviItem.Text = Info.AdjustPureSpecLib;
            naviItem.Name = "AdjustPureSpecLib";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("AdjustPureSpecLib");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("PureSpecLib");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);



            //改变语言
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.ChangeLanguage;
            naviItem.Name = "ChangeLanguage";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = null;
            naviItem.Image = GetIcoImage("ChangeLanguage");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("Language");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            if (flag)
                LangHelper.InitLangMenu(naviItem.MenuStripItem);
            Lang.LangItem = naviItem.MenuStripItem;


            //编辑语言
            naviItem = new NaviItem();
            naviItem.TT = EditLanguage;
            naviItem.Text = Info.EditLanguage;
            naviItem.Name = "EditLanguage";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("EditLanguage");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("ChangeLanguage");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //关于
            naviItem = new NaviItem();
            naviItem.TT = CreateLineParam;
            naviItem.Text = Info.About;
            naviItem.Name = "About";
            naviItem.FlagType = 0;
            naviItem.ShowInMain = false;
            naviItem.Image = GetIcoImage("About");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("Helper");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            container.parentStripMeauItem = parentContainer;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //使用说明书
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.SoftManual;
            naviItem.Name = "SoftManual";
            naviItem.excuteRequire = GetSoftManual;
            naviItem.FlagType = 0;
            naviItem.ShowInMain = false;
            naviItem.Image = GetIcoImage("Help");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("About");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            container.parentStripMeauItem = parentContainer;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //使用说明书
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.MachineManual;
            naviItem.Name = "MachineManual";
            naviItem.excuteRequire = GetMachineManual;
            naviItem.FlagType = 0;
            naviItem.ShowInMain = false;
            naviItem.Image = GetIcoImage("Help");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("SoftManual");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            container.parentStripMeauItem = parentContainer;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //生成授权请求
            naviItem = new NaviItem();
            naviItem.TT = CreateAuthorization;
            naviItem.Text = Info.Authorization;
            naviItem.Name = "Authorization";
            naviItem.FlagType = 0;
            naviItem.ShowInMain = false;
            naviItem.Image = GetIcoImage("Help");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("MachineManual");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            container.parentStripMeauItem = parentContainer;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = CreateCountBit;
            naviItem.Text = Info.CountBits;
            naviItem.Name = "CountBits";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("CountBits");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("Tools");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.isExistsChild = true;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.CountRate;
            naviItem.Name = "CountRate";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("CountRate");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("CountBits");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.isExistsChild = true;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //风格
            naviItem = new NaviItem();
            naviItem.TT = CreateStyle;
            naviItem.Text = Info.Style;
            naviItem.Name = "Style";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = null;
            naviItem.Image = GetIcoImage("Style");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("CountRate");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = CreateTitle;
            naviItem.Text = Info.CreateTitle;
            naviItem.Name = "CreateTitle";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("CreateTitle");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("Style");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = CreateToolsAppConfig;
            naviItem.Text = Info.FunctionConfig;
            naviItem.Name = "CreateToolsAppConfig";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("CreateToolsAppConfig");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("CreateTitle");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //公司其他信息
            naviItem = new NaviItem();
            naviItem.TT = CompanyOtherInformation;
            naviItem.Text = Info.CompanyOtherInformation;
            naviItem.Name = "CompanyOtherInformation";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("CompanyOtherInformation");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS" };
            preContainer = GetPreMenuStrip("CreateToolsAppConfig");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            //

            naviItem = new NaviItem();
            naviItem.TT = CreateUIConfig;
            naviItem.Text = Info.UIConfig;
            naviItem.Name = "UIConfig";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("UIConfig");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("CompanyOtherInformation");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //20110615 何晓明 备份还原
            naviItem = new NaviItem();
            naviItem.TT = CreateBackUpAndRestore;
            naviItem.Text = Info.BackUpAndRestore;
            naviItem.Name = "BackUpAndRestore";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("BackUpAndRestore");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("UIConfig");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //数据库清理
            naviItem = new NaviItem();
            naviItem.TT = CreateThinDatabase;
            naviItem.Text = Info.ThinDatabase;
            naviItem.Name = "ThinDatabase";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("ThinDatabase");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("BackUpAndRestore");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.AverageMode;
            naviItem.Name = "AverageMode";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("AverageMode");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.excuteRequire = AverageMode;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("ThinDatabase");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = CreateParamsConfig;
            naviItem.NoneStyle = false;
            naviItem.Text = Info.ParamsConfig;
            naviItem.Name = "ParamsConfig";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("ParamsConfig");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.excuteRequire = AverageMode;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("AverageMode");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = CreateWarningSettings;
            naviItem.NoneStyle = false;
            naviItem.Text = Info.strWarningSettings;
            naviItem.Name = "WarningSettings";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("WarningSettings");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.excuteRequire = AverageMode;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("ParamsConfig");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.ConcluteMode;
            naviItem.Name = "ConcluteMode";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("ConcluteMode");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.excuteRequire = ConcluteMode;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("WarningSettings");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = CreateJapanStandard;
            naviItem.Text = Info.JudgeStandard;
            naviItem.Name = "JudgeStandard";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("JudgeStandard");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("ConcluteMode");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = CreateParamsSet;
            naviItem.Text = Info.CalcParams;
            naviItem.Name = "CaculateParams";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("CaculateParams");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("JudgeStandard");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.excuteRequire = programMode;
            naviItem.Text = Info.programMode;
            naviItem.Name = "programMode";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("programMode");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("CaculateParams");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);



            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.CurrentCountRate;
            naviItem.Name = "CurrentCountRate";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("CurrentCountRate");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.excuteRequire = CurrentCountRate;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("CountRate");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.isExistsChild = true;
            container.preToolStripMeauItem = null;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);



            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.AverageCountRate;
            naviItem.Name = "AverageCountRate";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("AverageCountRate");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.excuteRequire = AverageCountRate;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("CurrentCountRate");
            //parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.isExistsChild = true;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            naviItem.MenuStripItem.Checked = true;
            preItem = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = CreateReportSetting;
            naviItem.Text = Info.ReportSetting;
            naviItem.Name = "ReportSetting";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("ReportSetting");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("Report");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.isExistsChild = false;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            #region 取消打印设置


            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.PrintSetting;
            naviItem.Name = "PrintSetting";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("PrintSetting");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            //container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("ReportSetting");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.isExistsChild = true;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            parentCContainer = container;
            MenuLoadHelper.MenuStripCollection.Add(container);


            //打印
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.OldTemplate;
            naviItem.Name = "OldReport";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("OldReport");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            //container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("PrintSetting");
            //parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = null;
            container.parentStripMeauItem = parentCContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = CreateUCPrint;
            naviItem.Text = Info.DefiniteTemplate;
            naviItem.Name = "DefiniteTemplate";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("DefiniteTemplate");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            //container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("OldReport");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentCContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            #endregion

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.StorePrint;
            naviItem.Name = "StorePrint";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExcutePrint;
            naviItem.Image = GetIcoImage("StorePrint");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("PrintSetting");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.Print;
            naviItem.Name = "Print";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExcuteDirectPrint;
            naviItem.Image = GetIcoImage("Print");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("StorePrint");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);







            //增加模版类型选项 Strong 2012/10/15
            #region

            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.TemplateModel;
            //naviItem.Name = "TemplateModel";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("TemplateModel");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "Thick" };
            //preContainer = GetPreMenuStrip("Print");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.isExistsChild = true;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //parentCContainer = container;
            //MenuLoadHelper.MenuStripCollection.Add(container);


            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.NormalMode;
            //naviItem.Name = "NormalMode";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("NormalMode");
            //naviItem.BtnDropDown.Tag = naviItem;
            //naviItem.excuteRequire = TempleteModeParams;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "Thick" };
            //preContainer = GetPreMenuStrip("TemplateModel");
            //parentContainer = preContainer;
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.isExistsChild = true;
            //container.preToolStripMeauItem = null;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //naviItem.MenuStripItem.Checked = ReportTemplateHelper.ExcelModeType == 12 ? true : false;
            //MenuLoadHelper.MenuStripCollection.Add(container);


            ////naviItem = new NaviItem();
            ////naviItem.TT = null;
            ////naviItem.Text = Info.DefiniteTemplate;
            ////naviItem.Name = "CustomTemplate";
            ////naviItem.FlagType = 0;
            ////naviItem.Image = GetIcoImage("CustomTemplate");
            ////naviItem.BtnDropDown.Tag = naviItem;
            ////naviItem.excuteRequire = TempleteModeParams;//选择自定义模版，改变模版参数
            ////WorkCurveHelper.NaviItems.Add(naviItem);
            ////container = new ToolStripControls();
            ////container.Version = new string[] { "Thick" };
            ////preContainer = GetPreMenuStrip("NormalMode");
            //////parentContainer = preContainer;
            ////container.Postion = preContainer.Postion;
            ////container.CurrentNaviItem = naviItem;
            ////container.isExistsChild = true;
            ////container.preToolStripMeauItem = preContainer;
            ////container.parentStripMeauItem = parentContainer;
            ////naviItem.MenuStripItem.Tag = naviItem;
            ////naviItem.MenuStripItem.Checked = ReportTemplateHelper.ExcelModeType == 2 ? true : false;
            ////preItem = naviItem;
            ////MenuLoadHelper.MenuStripCollection.Add(container);
            #endregion
            #region 取消自定义模板
            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.TemplateModel;
            //naviItem.Name = "TemplateModel";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("TemplateModel");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "Thick" };
            //preContainer = GetPreMenuStrip("Print");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.isExistsChild = true;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //parentCContainer = container;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.NormalTemplate;
            //naviItem.Name = "NormalMode";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("NormalMode");
            //naviItem.BtnDropDown.Tag = naviItem;
            //naviItem.excuteRequire = TempleteModeNormal;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "Thick" };
            //preContainer = GetPreMenuStrip("TemplateModel");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = null;
            //container.parentStripMeauItem = parentCContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //naviItem.MenuStripItem.Checked = ReportTemplateHelper.ExcelModeType == 12 ? true : false;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.DefiniteTemplate;
            //naviItem.Name = "CustomTemplate";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("CustomTemplate");
            //naviItem.BtnDropDown.Tag = naviItem;
            //naviItem.excuteRequire = TempleteModeCustom;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "Thick" };
            //preContainer = GetPreMenuStrip("NormalMode");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentCContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //naviItem.MenuStripItem.Checked = ReportTemplateHelper.ExcelModeType == 2 ? true : false;
            //MenuLoadHelper.MenuStripCollection.Add(container);


            ////增加蓝牙
            //naviItem = new NaviItem();
            //naviItem.TT = CreateBlueSet;
            //naviItem.Text = Info.BlueSetting;
            //naviItem.Name = "BlueSetting";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("bluesetting");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            //preContainer = GetPreMenuStrip("TemplateModel");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //parentCContainer = container;
            //MenuLoadHelper.MenuStripCollection.Add(container);


            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.BluePrint;
            //naviItem.Name = "BluePrint";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("blueprint");
            //naviItem.BtnDropDown.Tag = naviItem;
            //naviItem.excuteRequire = ExcuteBlueToothPrint;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "XRF" };
            //preContainer = GetPreMenuStrip("BlueSetting");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //parentCContainer = container;
            //MenuLoadHelper.MenuStripCollection.Add(container);



            #endregion
            //

            //设备
            naviItem = new NaviItem();
            naviItem.TT = CreateDevice;
            naviItem.Text = Info.Device;
            naviItem.Name = "Device";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("Device");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("Settings");
            parentContainer = preContainer;
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            ////选择模式
            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.strTargetModelSelect;
            //naviItem.Name = "SuperModel";
            //naviItem.excuteRequire = CreateSuperModel;
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("SuperModel");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            //preContainer = GetPreMenuStrip("Device");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);



            ////形状
            //naviItem = new NaviItem();
            //naviItem.TT = CreateShape;
            //naviItem.Text = Info.Shape;
            //naviItem.Name = "Shape";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("Shape");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            //preContainer = GetPreMenuStrip("SuperModel");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);
            //供应商
            naviItem = new NaviItem();
            naviItem.TT = CreateSupplier;
            naviItem.Text = Info.Supplier;
            naviItem.Name = "Supplier";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("Supplier");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            //preContainer = GetPreMenuStrip("Shape");Device
            preContainer = GetPreMenuStrip("Device");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //系统配置
            naviItem = new NaviItem();
            naviItem.TT = CreateSysConfig;
            naviItem.Text = Info.SysConfig;
            naviItem.Name = "SysConfig";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("SysConfig");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("Supplier");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //标准
            naviItem = new NaviItem();
            naviItem.TT = CreateStandard;
            naviItem.Text = Info.StandardStone;
            naviItem.Name = "Standard";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("Standard");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS" };
            preContainer = GetPreMenuStrip("SysConfig");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            ////标样模板
            //naviItem = new NaviItem();
            //naviItem.TT = CreateStandardTemplate;
            //naviItem.Text = Info.StandSampleLibrary;
            //naviItem.Name = "StandardTemplate";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("Standard");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS" };
            //preContainer = GetPreMenuStrip("Standard");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);


            //naviItem = new NaviItem();
            //naviItem.TT = CreateSpecifications;
            //naviItem.Text = Info.Specifications;
            //naviItem.Name = "Specifications";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("Specifications");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "XRF" };
            //preContainer = GetPreMenuStrip("StandardTemplate");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //修改网口
            naviItem = new NaviItem();
            naviItem.TT = CreateIPSettings;
            naviItem.Text = Info.IPSettings;
            naviItem.Name = "IPSettings";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("IPSettings");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            // preContainer = GetPreMenuStrip("Specifications");
            preContainer = GetPreMenuStrip("Standard");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = CreateSurfaceSource;
            naviItem.Text = Info.SurfaceSourceSettings;
            naviItem.Name = "SurfaceSourceSettings";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("SurfaceSourceSettings");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("IPSettings");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.Spin;
            //naviItem.Name = "Spin";
            //naviItem.FlagType = 0;
            //naviItem.excuteRequire = Spin;
            //naviItem.Image = GetIcoImage("Spin");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            //preContainer = GetPreMenuStrip("SurfaceSourceSettings");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //naviItem.MenuStripItem.Checked = false;
            //MenuLoadHelper.MenuStripCollection.Add(container);


            //naviItem = new NaviItem();
            //naviItem.TT = CreateMatch;
            //naviItem.Text = Info.MatchLevelSetting;
            //naviItem.excuteRequire = null;
            //naviItem.Name = "MatchLevelSetting";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("MatchLevelSetting");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            //preContainer = GetPreMenuStrip("Spin");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //naviItem = new NaviItem();
            //naviItem.TT = CreateAlloyGrade;
            //naviItem.Text = Info.NitonAlloyGrade;
            //naviItem.excuteRequire = null; ;
            //naviItem.Name = "NitonAlloyGrade";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("NitonAlloyGrade");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "XRF" };
            //preContainer = GetPreMenuStrip("MatchLevelSetting");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //名称设置
            naviItem = new NaviItem();
            naviItem.TT = SettingName;
            naviItem.Text = Info.NameSetting;
            naviItem.Name = "NameSetting";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("SysConfig");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            // preContainer = GetPreMenuStrip("NitonAlloyGrade");SurfaceSourceSettings
            preContainer = GetPreMenuStrip("SurfaceSourceSettings");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.ShowDialog;
            //naviItem.excuteRequire = null;
            //naviItem.Name = "ShowDialog";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("ShowDialog");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "EDXRF" };
            //preContainer = GetPreMenuStrip("NameSetting");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.isExistsChild = true;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //parentCContainer = container;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //naviItem = new NaviItem();
            //naviItem.TT = PWDLock;
            //naviItem.Text = Info.PWDLock;
            //naviItem.Name = "PWDLock";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("Lock");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "EDXRF" };
            //preContainer = GetPreMenuStrip("ShowDialog");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.ShowResultInMain;
            //naviItem.Name = "ShowResultInMain";
            //naviItem.FlagType = 0;
            //naviItem.excuteRequire = ExcuteShowDialog;
            //naviItem.Image = GetIcoImage("Auto");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "EDXRF" };
            //preContainer = GetPreMenuStrip("PWDLock");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = null;
            //container.parentStripMeauItem = parentCContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.Auto;
            //naviItem.Name = "Auto";
            //naviItem.FlagType = 0;
            //naviItem.excuteRequire = AutoShowResult;
            //naviItem.Image = GetIcoImage("Auto");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "EDXRF" };
            //preContainer = GetPreMenuStrip("ShowResultInMain");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentCContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //showResultItem = naviItem;
            //naviItem.MenuStripItem.Checked = true;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.Manual;
            //naviItem.Name = "Manual";
            //naviItem.FlagType = 0;
            //naviItem.excuteRequire = ManualShowResult;
            //naviItem.Image = GetIcoImage("Manual");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "EDXRF" };
            //preContainer = GetPreMenuStrip("Auto");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentCContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            ////关盖测试
            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.TestOnCoverClosed;
            //naviItem.excuteRequire = null;
            //naviItem.Name = "TestOnCoverClosed";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("TestOnCoverClosed");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "EDXRF" };
            //preContainer = GetPreMenuStrip("NameSetting");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.isExistsChild = true;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //parentCContainer = container;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            ////是否开启关盖测试
            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.TestOnCoverClosedEnabled;
            //naviItem.Name = "TestOnCoverClosedEnabled";
            //naviItem.FlagType = 0;
            //naviItem.excuteRequire = TestOnCoverClosedEnableChanged;
            //naviItem.Image = GetIcoImage("TestOnCoverClosedEnabled");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "EDXRF" };
            //preContainer = GetPreMenuStrip("TestOnCoverClosed");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = null;
            //container.parentStripMeauItem = parentCContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //一键测试
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.TestOnButtonPressed;
            naviItem.excuteRequire = null;
            naviItem.Name = "TestOnButtonPressed";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("TestOnButtonPressed");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("NameSetting");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.isExistsChild = true;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            parentCContainer = container;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.TestOnButtonPressedEnabled;
            naviItem.Name = "TestOnButtonPressedEnabled";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = TestOnButtonPressedEnabledChanged;
            naviItem.Image = WorkCurveHelper.TestOnButtonPressedEnabled ? GetIcoImage("EnableTestOnButtonPressed") : GetIcoImage("TestOnButtonPressedEnabled");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "EDXRF" };
            preContainer = GetPreMenuStrip("TestOnButtonPressed");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = null;
            container.parentStripMeauItem = parentCContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            if (WorkCurveHelper.bSetHeight)
            {
                naviItem = new NaviItem();
                naviItem.TT = CreateEncoderValue;
                naviItem.Text = Info.EncoderValue;
                naviItem.excuteRequire = null; ;
                naviItem.Name = "EncoderValue";
                naviItem.FlagType = 0;
                naviItem.Image = GetIcoImage("NitonAlloyGrade");
                naviItem.BtnDropDown.Tag = naviItem;
                WorkCurveHelper.NaviItems.Add(naviItem);
                container = new ToolStripControls();
                container.Version = new string[] { "Thick" };
                preContainer = GetPreMenuStrip("TestOnButtonPressed");
                container.Postion = preContainer.Postion;
                container.CurrentNaviItem = naviItem;
                container.preToolStripMeauItem = preContainer;
                container.parentStripMeauItem = parentContainer;
                naviItem.MenuStripItem.Tag = naviItem;
                MenuLoadHelper.MenuStripCollection.Add(container);

            }

            naviItem = new NaviItem();
            naviItem.TT = CreateDefinePureElement;
            naviItem.Text = Info.DefinePureElem;
            naviItem.excuteRequire = null; ;
            naviItem.Name = "DefinePureElem";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("DefinePureElem");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "Thick" };
            if (WorkCurveHelper.bSetHeight)
                preContainer = GetPreMenuStrip("EncoderValue");
            else
                preContainer = GetPreMenuStrip("TestOnButtonPressed");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = CreateTabParams;
            naviItem.Text = Info.TabParams;
            naviItem.excuteRequire = null; ;
            naviItem.Name = "TabParams";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("TabParams");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("DefinePureElem");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            //添加系统设置菜单项
            naviItem = new NaviItem();
            naviItem.TT = CreateSysSettings;
            naviItem.Text = Info.SysSettings;
            naviItem.excuteRequire = null; ;
            naviItem.Name = "SysSettings";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("SysSettings");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("TabParams");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //添加数据库设置菜单项
            naviItem = new NaviItem();
            naviItem.TT = CreateDBSetting;
            naviItem.Text = Info.DBSetting;
            naviItem.excuteRequire = null;
            naviItem.Name = "DBSetting";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("DBSetting");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("SysSettings");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            //添加串口参数设置菜单项
            naviItem = new NaviItem();
            naviItem.TT =  CreateSerialSetting;
            naviItem.Text = Info.SerialSetting;
            naviItem.excuteRequire = null;
            naviItem.Name = "SerialSetting";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("SerialSetting");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("DBSetting");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //添加多点检测参数设置菜单项
            naviItem = new NaviItem();
            naviItem.TT = CreateDetectPointsSetting;
            naviItem.Text = Info.DetectPointsSetting;
            naviItem.excuteRequire = null;
            naviItem.Name = "DetectPointsSetting";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("DetectPointsSetting");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "Thick" };
            preContainer = GetPreMenuStrip("SerialSetting");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //新谱
            naviItem = new NaviItem();
            naviItem.TT = CreateNewSpec;
            naviItem.FunctionType = 2;
            naviItem.Text = Info.Start;
            naviItem.IsModel = true;
            naviItem.Name = "TestSetting";
            naviItem.FlagType = 0;
            naviItem.Image = Properties.Resources.StartTest;
            naviItem.BtnDropDown.Tag = naviItem;

            naviItem.EnabledControl = false;

            WorkCurveHelper.NaviItems.Add(naviItem);
            //修改：何晓明 20110713 工具栏增加菜单栏
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("HistoryRecord");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            EventInfo _event = naviItem.MenuStripItem.GetType().GetEvent("Click");
            Type handlerType = _event.EventHandlerType;
            MethodInfo miHandler = naviItem.GetType().GetMethod("ProcessCommon", BindingFlags.NonPublic | BindingFlags.Instance);
            EventHandler d = (EventHandler)EventHandler.CreateDelegate(handlerType, naviItem, miHandler);
            naviItem.MenuStripItem.Click -= d;
            naviItem.MenuStripItem.Click += new EventHandler(MenuStripItem_Click);
            naviItem.MenuStripItem.Click += d;

            //MethodInfo miClick = _event.GetAddMethod();
            //Delegate dClick = Delegate.CreateDelegate(handlerType, naviItem.MenuStripItem, miHandler);
            //Delegate[] _list = GetObjectEventList(naviItem.MenuStripItem, "Click", typeof(ToolStripMenuItem));

            //Delegate _Delegate = Delegate.CreateDelegate(typeof(EventHandler),_MethodInfos);
            //

            ////批量测量
            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.FunctionType = 2;
            //naviItem.excuteRequire = CreateBatchTest;
            //naviItem.Text = Info.BatchTest;
            //naviItem.Name = "BatchTest";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("BatchTest");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS" };
            //preContainer = GetPreMenuStrip("TestSetting");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //停止
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.StopTest;
            naviItem.Name = "StopTest";
            naviItem.FunctionType = 2;
            naviItem.excuteRequire = ExcuteStopOperation;
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("StopTest");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            //修改：何晓明 20110713 工具栏增加菜单栏
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("TestSetting");//BatchTest
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            //

            //初始化
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.Initialization;
            naviItem.Name = "Initialization";
            naviItem.FunctionType = 2;
            naviItem.excuteRequire = ExcuteInitializationOperation;
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("Initialization");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            //修改：何晓明 20110713 工具栏增加菜单栏
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("StopTest");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            //


            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.AutoDemarcateEnergy;
            //naviItem.Name = "AutoDemarcateEnergy";
            //naviItem.FunctionType = 2;
            //naviItem.FlagType = 0;
            //naviItem.excuteRequire = ExcuteAutoDemarcateEnergy;
            //naviItem.Image = GetIcoImage("DemarcateEnergy");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            ////修改：何晓明 20110713 工具栏增加菜单栏
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            //preContainer = GetPreMenuStrip("Initialization");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            ////追加标准线性算截距的功能2013-10-14
            ////修改为标样校正 20200701
            //naviItem = new NaviItem();
            //naviItem.TT = CreateConSampleCal;
            //naviItem.Text = Info.StandLine;
            //naviItem.Name = "ExcuteAutoFPGAIntercept";
            //naviItem.FunctionType = 2;
            //naviItem.FlagType = 0;
            //naviItem.excuteRequire = null;//ExcuteAutoFPGAIntercept;
            //naviItem.Image = GetIcoImage("Standard");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            ////修改：何晓明 20110713 工具栏增加菜单栏
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS" };//, "XRF", "Thick", "EDXRF"
            //preContainer = GetPreMenuStrip("AutoDemarcateEnergy");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);
            ////

            //naviItem = new NaviItem();
            //naviItem.TT = ExcuteMoveWorkStation;
            ////naviItem.FunctionType = 0;
            //naviItem.Text = Info.MoveWorkStation;
            //naviItem.Name = "MoveWorkStation";
            //naviItem.FlagType = 0;
            //naviItem.FunctionType = 2;
            //naviItem.Image = GetIcoImage("power");
            //naviItem.BtnDropDown.Tag = naviItem;
            ////修改：何晓明 20110711 移动平台创建单体实例
            //naviItem.IsModel = false;
            ////naviItem.IsModel = true;
            ////
            //worksStation = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            ////修改：何晓明 20110713 工具栏增加菜单栏
            //container = new ToolStripControls();
            //preContainer = GetPreMenuStrip("AutoDemarcateEnergy");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);
            //

            // naviItem = new NaviItem();
            // naviItem.TT = ExcuteChamberMove;
            // //naviItem.FunctionType = 0;
            // naviItem.Text = Info.ChamberMove;
            // naviItem.Name = "ChamberMove";
            // naviItem.FlagType = 0;
            // naviItem.FunctionType = 2;
            // //修改：何晓明 20110711 样品腔前端显示
            //// naviItem.IsModel = false;
            // naviItem.IsModel = true;
            // naviItem.EnabledControl = false;
            // //
            // naviItem.Image = GetIcoImage("Chamber");
            // naviItem.BtnDropDown.Tag = naviItem;
            // worksStation = naviItem;
            // WorkCurveHelper.NaviItems.Add(naviItem);
            // //修改：何晓明 20110713 工具栏增加菜单栏
            // container = new ToolStripControls();
            // container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            // preContainer = GetPreMenuStrip("ExcuteAutoFPGAIntercept");
            // container.Postion = preContainer.Postion;
            // container.CurrentNaviItem = naviItem;
            // container.preToolStripMeauItem = preContainer;
            // container.parentStripMeauItem = parentOtherContainer;
            // naviItem.MenuStripItem.Tag = naviItem;
            // MenuLoadHelper.MenuStripCollection.Add(container);
            //


            //计算
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.Caculate;
            naviItem.Name = "Caculate";
            naviItem.FunctionType = 2;
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExcuteCaculateOperation;
            naviItem.Image = GetIcoImage("Caculate");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            //修改：何晓明 20110713 工具栏增加菜单栏
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("Initialization");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            ////自检
            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.strDetection;
            //naviItem.Name = "Detection";
            //naviItem.FunctionType = 2;
            //naviItem.FlagType = 0;
            //naviItem.excuteRequire = ExcuteDetection;
            //naviItem.Image = GetIcoImage("Check");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS" };
            //preContainer = GetPreMenuStrip("Caculate");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);
            //

            //匹配边界校正
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.SetMatchborder;
            naviItem.Name = "SetMatchborder";
            naviItem.FunctionType = 2;
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExcuteMatchParams;
            naviItem.Image = GetIcoImage("SetMatchborder");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            //修改：何晓明 20110713 工具栏增加菜单栏
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS" };
            preContainer = GetPreMenuStrip("Caculate");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            //

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.ConnectDevice;
            naviItem.Name = "ConnectDevice";
            naviItem.FunctionType = 2;
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExcuteConnectDevice;
            naviItem.Image = GetIcoImage("ConnectDevice");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            //修改：何晓明 20110713 工具栏增加菜单栏
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("SetMatchborder");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);
            //

            ////调节放大倍数
            //naviItem = new NaviItem();
            //naviItem.TT = GainSetFOR3000;
            //naviItem.FunctionType = 2;
            //naviItem.Text = Info.strGainSet;
            //naviItem.Name = "GainSet";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("OpenCondition");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            ////修改：何晓明 20110713 工具栏增加菜单栏
            //container = new ToolStripControls();
            //container.Version = new string[] { "EDXRF" };
            //preContainer = GetPreMenuStrip("ConnectDevice");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //加载联测功能
            naviItem = new NaviItem();
            naviItem.TT = CreateHistoryRecordContinuous;
            naviItem.Text = Info.strHistoryRecordContinuoust;
            naviItem.Name = "HistoryRecordContinuous";
            naviItem.FlagType = 0;
            //修改：何晓明 2011-07-11 去除最大化窗口
            naviItem.IsMaxnium = true;
            //
            naviItem.Image = GetIcoImage("HistoryRecord");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            //修改：何晓明 20110713 工具栏增加菜单栏
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS" };
            preContainer = GetPreMenuStrip("ConnectDevice");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.CurveCalibrate;
            naviItem.Name = "CurveCalibrate";
            naviItem.FunctionType = 2;
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExcuteCurveCalibrate;
            naviItem.Image = GetIcoImage("CurveCalibrate");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "Thick" };
            preContainer = GetPreMenuStrip("HistoryRecordContinuous");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);




            ////卤素

            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = "RoHS";
            //naviItem.Name = "Lushu";
            //naviItem.excuteRequire = ExcuteChangeMode;
            //naviItem.FunctionType = 2;
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("Lushu");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS" };
            //preContainer = GetPreMenuStrip("CurveCalibrate");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            ////container.parentStripMeauItem = parentContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = CreateMoveStation;
            naviItem.Text = Info.MoveWorkStation;
            naviItem.Name = "MoveWorkStation";
            naviItem.FunctionType = 2;
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("power");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("CurveCalibrate");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.AdjustInitial;
            naviItem.Name = "InitialAdjust";
            naviItem.FunctionType = 2;
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExcuteInitialAdjust;
            naviItem.Image = GetIcoImage("InitialAdjust");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "Thick" };
            preContainer = GetPreMenuStrip("MoveWorkStation");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);




            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.BaseAdjust;
            naviItem.Name = "BaseAdjust";
            naviItem.FunctionType = 2;
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExcuteBaseAdjust;
            naviItem.Image = GetIcoImage("BaseAdjust");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "Thick" };
            preContainer = GetPreMenuStrip("InitialAdjust");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);




            naviItem = new NaviItem();
            naviItem.TT = CreateHeatMap;
            naviItem.Text = Info.HeatMap;
            naviItem.Name = "HeatMap";
            naviItem.FunctionType = 2;
            naviItem.FlagType = 0;
            naviItem.excuteRequire = null;
            naviItem.Image = GetIcoImage("HeatMap");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "Thick" };
            preContainer = GetPreMenuStrip("BaseAdjust");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = "电机自检";
            naviItem.Name = "SelfCheck";
            naviItem.FunctionType = 2;
            naviItem.FlagType = 0;
            naviItem.excuteRequire = selfCheck;
            naviItem.Image = GetIcoImage("SelfCheck");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "Thick" };
            preContainer = GetPreMenuStrip("HeatMap");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Text = Info.AutoCheck;
            //naviItem.Name = "AutoCheck";
            //naviItem.FunctionType = 2;
            //naviItem.FlagType = 0;
            //naviItem.excuteRequire = ExcuteAutoCheck;
            //naviItem.Image = GetIcoImage("AutoCheck");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS", "Thick" };
            //preContainer = GetPreMenuStrip("BaseAdjust");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);





            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.FunctionType = 2;
            //naviItem.Text = Info.AutoMatch;
            //naviItem.excuteRequire = ExcuteAutoMatch;
            //naviItem.Name = "AutoMatch";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("AutoMatch");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            ////修改：何晓明 20110713 工具栏增加菜单栏
            //container = new ToolStripControls();
            //container.Version = new string[] { "XRF", "EDXRF" };
            //preContainer = GetPreMenuStrip("MoveWorkStation");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);




            //naviItem = new NaviItem();
            //naviItem.TT = null;
            //naviItem.Name = "cboMode";
            //naviItem.Text = Info.Pattern;
            //naviItem.FlagType = 2;
            //if (naviItem.ComboStrip != null)
            //{
            //    naviItem.ComboStrip.Tag = listNavi;
            //    naviItem.ComboStrip.SelectedIndexChanged += new EventHandler(ComboStrip_SelectedIndexChanged1);
            //    naviItem.ComboStrip.Items.AddRange(new string[] { Info.Normal, Info.Intelligent });
            //    if (naviItem.ComboStrip.Items.Count > 0)
            //        naviItem.ComboStrip.SelectedIndex = 0;
            //}
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "XRF", "EDXRF" };
            //preContainer = GetPreMenuStrip("AutoMatch");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //naviItem = new NaviItem();
            //naviItem.TT = CreateLaserMode;
            //naviItem.Text = Info.Laser + Info.Shell;
            //naviItem.Name = "LaserMode";
            //naviItem.FunctionType = 2;
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("Laser");
            //naviItem.BtnDropDown.Tag = naviItem;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] {"EDXRF" };
            //preContainer = GetPreMenuStrip("cboMode");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);

            //naviItem = new NaviItem();
            //naviItem.TT = CreateChangeSampleInfo;
            //naviItem.Text = Info.ChangeSampleInfo;
            //naviItem.Name = "ChangeSampleInfo";
            //naviItem.FlagType = 0;
            ////naviItem.excuteRequire = PrintResults;
            //naviItem.Image = GetIcoImage("ElementRef");
            //naviItem.BtnDropDown.Tag = naviItem;
            //naviItem.FunctionType = 2;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            //preContainer = GetPreMenuStrip("LaserMode");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.SaveScreenshots;
            naviItem.Name = "SaveScreenshots";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = SaveScreenshots;
            naviItem.Image = GetIcoImage("Expunction");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.FunctionType = 2;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("MainPage");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.PrintScreenshots;
            naviItem.Name = "PrintScreenshots";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = PrintScreenshots;
            naviItem.Image = GetIcoImage("ElementRef");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.FunctionType = 2;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("MainPage");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.SaveResults;
            naviItem.Name = "SaveResults";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = SaveResults;
            naviItem.Image = GetIcoImage("Expunction");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.FunctionType = 2;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("MainPage");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.PrintResults;
            naviItem.Name = "PrintResults";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = PrintResults;
            naviItem.Image = GetIcoImage("ElementRef");
            naviItem.BtnDropDown.Tag = naviItem;
            naviItem.FunctionType = 2;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("MainPage");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentOtherContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);




            //naviItem = new NaviItem();
            //naviItem.TT = CreateStock;
            //naviItem.Text = Info.Stock;
            //naviItem.Name = "Stock";
            //naviItem.FlagType = 0;
            //naviItem.Image = GetIcoImage("Drawer");
            //naviItem.BtnDropDown.Tag = naviItem;
            //naviItem.FunctionType = 2;
            //naviItem.IsMaxnium = true;
            //WorkCurveHelper.NaviItems.Add(naviItem);
            //container = new ToolStripControls();
            //container.Version = new string[] { "EDXRF" };
            //preContainer = GetPreMenuStrip("MainPage");
            //container.Postion = preContainer.Postion;
            //container.CurrentNaviItem = naviItem;
            //container.preToolStripMeauItem = preContainer;
            //container.parentStripMeauItem = parentOtherContainer;
            //naviItem.MenuStripItem.Tag = naviItem;
            //MenuLoadHelper.MenuStripCollection.Add(container);


            // //影像
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.FocusArea;
            naviItem.excuteRequire = null;
            naviItem.Name = "FocusArea";
            naviItem.FlagType = 0;
            naviItem.Image = GetIcoImage("FocusArea");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("Camera");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.isExistsChild = true;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            parentCContainer = container;
            MenuLoadHelper.MenuStripCollection.Add(container);

            //影像
            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.Max;
            naviItem.Name = "FocusMax";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExecuteFocusType;
            naviItem.Image = GetIcoImage("FocusMax");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("FocusArea");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = null;
            container.parentStripMeauItem = parentCContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);



            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.Middle;
            naviItem.Name = "FocusMiddle";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExecuteFocusType;
            naviItem.Image = GetIcoImage("FocusMiddle");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("FocusMax");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentCContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.Min;
            naviItem.Name = "FocusMin";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExecuteFocusType;
            naviItem.Image = GetIcoImage("FocusMin");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("FocusMiddle");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentCContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.CameraZoomIn;
            naviItem.Name = "CameraZoomIn";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExecuteZoomIn;
            naviItem.Image = GetIcoImage("ZoomIn");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("FocusArea");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentCContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.CameraZoomOut;
            naviItem.Name = "CameraZoomOut";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExecuteZoomOut;
            naviItem.Image = GetIcoImage("ZoomOut");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("CameraZoomIn");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentCContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);


            naviItem = new NaviItem();
            naviItem.TT = null;
            naviItem.Text = Info.CameraZoom;
            naviItem.Name = "CameraZoom";
            naviItem.FlagType = 0;
            naviItem.excuteRequire = ExecuteZoom;
            naviItem.Image = GetIcoImage("Zoom");
            naviItem.BtnDropDown.Tag = naviItem;
            WorkCurveHelper.NaviItems.Add(naviItem);
            container = new ToolStripControls();
            container.Version = new string[] { "RoHS", "XRF", "Thick", "EDXRF" };
            preContainer = GetPreMenuStrip("CameraZoomOut");
            container.Postion = preContainer.Postion;
            container.CurrentNaviItem = naviItem;
            container.preToolStripMeauItem = preContainer;
            container.parentStripMeauItem = parentCContainer;
            naviItem.MenuStripItem.Tag = naviItem;
            MenuLoadHelper.MenuStripCollection.Add(container);

        }

        void MenuStripItem_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            WorkCurveHelper.DirectRun.IsKeyCall = true;
        }

        public static Delegate[] GetObjectEventList(object p_Object, string p_EventName, Type p_EventType)
        {
            PropertyInfo _PropertyInfo = p_Object.GetType().GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
            if (_PropertyInfo != null)
            {
                object _EventList = _PropertyInfo.GetValue(p_Object, null);
                if (_EventList != null && _EventList is EventHandlerList)
                {
                    EventHandlerList _List = (EventHandlerList)_EventList;
                    FieldInfo _FieldInfo = p_EventType.GetField(p_EventName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                    if (_FieldInfo == null) return null;
                    Delegate _ObjectDelegate = _List[_FieldInfo.GetValue(p_Object)];
                    //EventInfo _EventInfo = p_EventType.GetEvent(p_EventName);
                    //Delegate _ObjectDelegate = _List[_EventInfo.Name];
                    if (_ObjectDelegate == null) return null;
                    return _ObjectDelegate.GetInvocationList();
                    //EventInfo _EventInfo = p_EventType.GetEvent(p_EventName);
                    //if (_EventInfo == null) return null;
                    //MethodInfo[] _MehtodInfo = _EventInfo.GetAddMethod();
                    //return _MehtodInfo;
                }
            }
            return null;
        }

        #endregion
        private List<UCMoveWorksStation> moveWorksStation = new List<UCMoveWorksStation>();
        private NaviItem worksStation;


        public UCMultiple CreateLaserMode()
        {
            return new FrmLazer();
        }

        public UCMultiple CreateMoveStation()
        {
            return UCComponentMotor.CreateSingleInstance();
        }
        public UCMultiple CreateChangeSampleInfo()
        {
            return new FrmChangeSpInfo();
        }
        public void ExcuteCurveCalibrate(NaviItem item)
        {
            if (DifferenceDevice.irohs != null)
                DifferenceDevice.irohs.ExcuteCurveCalibrate(item);
            else if (DifferenceDevice.ithick != null)
            {

                DifferenceDevice.ithick.ExcuteCurveCalibrate(item, 0, OptMode.CurveCalibrate);
            }

        }


        public void ExcuteBaseAdjust(NaviItem item)
        {
            if (DifferenceDevice.ithick != null)
            {
                DifferenceDevice.ithick.ExcuteCurveCalibrate(item, 0, OptMode.BaseCalibrate);
            }

        }

        public UCMultiple CreateHeatMap()
        {
            if (WorkCurveHelper.testedRows > 0)
                return new UCHeatMapList();
            else
                return null;
        }

        public void selfCheck(NaviItem item)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(DifferenceDevice.CurCameraControl.selfCheck)).Start();

        }
     
        public void ExcuteChangeUser(NaviItem item)
        {
            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/CurUser", GP.UserName);
            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/ChangeUser", true.ToString());
            IRefreshFillInfo temp = (IRefreshFillInfo)WorkCurveHelper.curFrmThick;
            
            temp.LogOut();



        }


       

      

        public void ExcuteInitialAdjust(NaviItem item)
        {
            DifferenceDevice.interClassMain.bAdjustInitCount = true;
            DifferenceDevice.MediumAccess.TestInitialization();
        }

        public void ExcuteAutoMatch(NaviItem item)
        {
            NaviItem tempItem = WorkCurveHelper.NaviItems.Find(w => w.Name == "TestSetting");
            if (tempItem == null)
                return;

            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "SpecType", ((int)SpecType.UnKownSpec).ToString());
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "IsMatch", true.ToString());
            tempItem.MenuStripItem.PerformClick();
        }

        public void ExcuteChangeMode(NaviItem item)
        {
            int intLushu = 0;
            if (item.Text == Info.Lushu)
            {
                intLushu = 1;
                item.Text = "RoHS";
            }
            else
            {
                intLushu = 0;
                item.Text = Info.Lushu;
            }
            //Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedValue("OpenHistoryRecordType", "HistoryRecordContinuousLushuType", intLushu.ToString());
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "Parameter.xml", "Parameter/System/PlasticContinuous", intLushu.ToString());
        }

        #region
        #region
        //naviItem = new NaviItem();
        //naviItem.TT = null;
        //naviItem.Text = Info.MoveStationUp;
        //naviItem.Name = "MoveStationUp";
        //naviItem.FunctionType = 2;
        //naviItem.FlagType = 0;
        //naviItem.excuteRequire = ExcuteMoveUp;
        //naviItem.Image = GetIcoImage("MoveStationUp");
        //naviItem.BtnDropDown.Tag = naviItem;
        //WorkCurveHelper.NaviItems.Add(naviItem);
        //container = new ToolStripControls();
        //preContainer = GetPreMenuStrip("CurveCalibrate");
        //container.Postion = preContainer.Postion;
        //container.CurrentNaviItem = naviItem;
        //container.preToolStripMeauItem = preContainer;
        //container.parentStripMeauItem = parentOtherContainer;
        //naviItem.MenuStripItem.Tag = naviItem;
        //MenuLoadHelper.MenuStripCollection.Add(container);

        //naviItem = new NaviItem();
        //naviItem.TT = null;
        //naviItem.Text = Info.MoveStationDown;
        //naviItem.Name = "MoveStationDown";
        //naviItem.FunctionType = 2;
        //naviItem.FlagType = 0;
        //naviItem.excuteRequire = ExcuteMoveDown;
        //naviItem.Image = GetIcoImage("MoveStationDown");
        //naviItem.BtnDropDown.Tag = naviItem;
        //WorkCurveHelper.NaviItems.Add(naviItem);
        //container = new ToolStripControls();
        //preContainer = GetPreMenuStrip("MoveStationUp");
        //container.Postion = preContainer.Postion;
        //container.CurrentNaviItem = naviItem;
        //container.preToolStripMeauItem = preContainer;
        //container.parentStripMeauItem = parentOtherContainer;
        //naviItem.MenuStripItem.Tag = naviItem;
        //MenuLoadHelper.MenuStripCollection.Add(container);


        //naviItem = new NaviItem();
        //naviItem.TT = null;
        //naviItem.Text = Info.MoveStationStop;
        //naviItem.Name = "MoveStationStop";
        //naviItem.FunctionType = 2;
        //naviItem.FlagType = 0;
        //naviItem.excuteRequire = ExcuteMoveStop;
        //naviItem.Image = GetIcoImage("MoveStationStop");
        //naviItem.BtnDropDown.Tag = naviItem;
        //WorkCurveHelper.NaviItems.Add(naviItem);
        //container = new ToolStripControls();
        //preContainer = GetPreMenuStrip("MoveStationDown");
        //container.Postion = preContainer.Postion;
        //container.CurrentNaviItem = naviItem;
        //container.preToolStripMeauItem = preContainer;
        //container.parentStripMeauItem = parentOtherContainer;
        //naviItem.MenuStripItem.Tag = naviItem;
        //MenuLoadHelper.MenuStripCollection.Add(container);


        //naviItem = new NaviItem();
        //naviItem.TT = CreateXaxis;
        //naviItem.Text = Info.XaisMove;
        //naviItem.Name = "MoveStationX";
        //naviItem.FunctionType = 2;
        //naviItem.FlagType = 0;
        //naviItem.Image = GetIcoImage("MoveStationStop");
        //naviItem.BtnDropDown.Tag = naviItem;
        //WorkCurveHelper.NaviItems.Add(naviItem);
        //container = new ToolStripControls();
        //preContainer = GetPreMenuStrip("CurveCalibrate");
        //container.Postion = preContainer.Postion;
        //container.CurrentNaviItem = naviItem;
        //container.preToolStripMeauItem = preContainer;
        //container.parentStripMeauItem = parentOtherContainer;
        //naviItem.MenuStripItem.Tag = naviItem;
        //MenuLoadHelper.MenuStripCollection.Add(container);

        //naviItem = new NaviItem();
        //naviItem.TT = CreateYaxis;
        //naviItem.Text = Info.YaisMove;
        //naviItem.Name = "MoveStationY";
        //naviItem.FunctionType = 2;
        //naviItem.FlagType = 0;
        //naviItem.Image = GetIcoImage("MoveStationStop");
        //naviItem.BtnDropDown.Tag = naviItem;
        //WorkCurveHelper.NaviItems.Add(naviItem);
        //container = new ToolStripControls();
        //preContainer = GetPreMenuStrip("MoveStationX");
        //container.Postion = preContainer.Postion;
        //container.CurrentNaviItem = naviItem;
        //container.preToolStripMeauItem = preContainer;
        //container.parentStripMeauItem = parentOtherContainer;
        //naviItem.MenuStripItem.Tag = naviItem;
        //MenuLoadHelper.MenuStripCollection.Add(container);

        //naviItem = new NaviItem();
        //naviItem.TT = CreateZaxis;
        //naviItem.Text = Info.ZaisMove;
        //naviItem.Name = "MoveStationZ";
        //naviItem.FunctionType = 2;
        //naviItem.FlagType = 0;
        //naviItem.Image = GetIcoImage("MoveStationStop");
        //naviItem.BtnDropDown.Tag = naviItem;
        //WorkCurveHelper.NaviItems.Add(naviItem);
        //container = new ToolStripControls();
        //preContainer = GetPreMenuStrip("MoveStationY");
        //container.Postion = preContainer.Postion;
        //container.CurrentNaviItem = naviItem;
        //container.preToolStripMeauItem = preContainer;
        //container.parentStripMeauItem = parentOtherContainer;
        //naviItem.MenuStripItem.Tag = naviItem;
        //MenuLoadHelper.MenuStripCollection.Add(container);
        #endregion
        //public UCMultiple CreateXaxis()
        //{
        //   UCXYZMotor.CreateSingleInstance(Skyray.EDX.Common.Component.MoterType.XMotor);
        //   UCXYZMotor motor = UCXYZMotor.xMotor;
        //    motor.GrpText = Info.XaisMove;
        //    motor.EnableDisable = true;
        //    motor.LeftImage = Properties.Resources.back;
        //    motor.RightImage = Properties.Resources.forward;
        //    motor.MoterType = Skyray.EDX.Common.Component.MoterType.XMotor;
        //    motor.IsShowScroll = false;
        //    return motor;
        //}

        //public UCMultiple CreateYaxis()
        //{
        //    UCXYZMotor.CreateSingleInstance(Skyray.EDX.Common.Component.MoterType.YMotor);
        //    UCXYZMotor motor = UCXYZMotor.yMotor;
        //    motor.GrpText = Info.YaisMove;
        //    motor.EnableDisable = true;
        //    motor.LeftImage = Properties.Resources.go_up;
        //    motor.RightImage = Properties.Resources.down;
        //    motor.IsShowScroll = false;
        //    motor.MoterType = Skyray.EDX.Common.Component.MoterType.YMotor;
        //    return motor;
        //}

        //public UCMultiple CreateZaxis()
        //{
        //    UCXYZMotor.CreateSingleInstance(Skyray.EDX.Common.Component.MoterType.ZMotor);
        //    UCXYZMotor motor = UCXYZMotor.zMotor;
        //    motor.EnableDisable = true;
        //    motor.GrpText = Info.ZaisMove;
        //    motor.LeftImage = Properties.Resources.Top;
        //    motor.RightImage = Properties.Resources.Bottom;
        //    motor.IsShowScroll = false;
        //    motor.MoterType = Skyray.EDX.Common.Component.MoterType.ZMotor;
        //    return motor;
        //}
        //public void ExcuteMoveUp(NaviItem item)
        //{
        //    if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
        //      && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
        //        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, WorkCurveHelper.DeviceCurrent.MotorZDirect, 9000000, true, WorkCurveHelper.DeviceCurrent.MotorZSpeed);
        //}

        //public void ExcuteMoveDown(NaviItem item)
        //{
        //    if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
        //        && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
        //    {
        //        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1), 9000000, true, WorkCurveHelper.DeviceCurrent.MotorZSpeed);
        //    }
        //}

        //public void ExcuteMoveStop(NaviItem item)
        //{
        //    if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
        //       && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
        //        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, WorkCurveHelper.DeviceCurrent.MotorZDirect, 0, true, WorkCurveHelper.DeviceCurrent.MotorZSpeed);
        //}
        #endregion

        private UCMultiple ExcuteChamberMove()
        {
            return UCChamberMove.CreateSingleInstance();
        }

        private UCMultiple CreateReportSetting()
        {
            UCReportPath ucReport = new UCReportPath();
            return ucReport;
        }

        /// <summary>
        /// 设备
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateDevice()
        {
            FrmDevice FrmDevice = new FrmDevice();
            return FrmDevice;
        }

        /// <summary>
        /// 执行显示峰值事件通知主界面
        /// </summary>
        /// <param name="item"></param>
        private void ExcuteDisplayPeak(NaviItem item)
        {
            DifferenceDevice.MediumAccess.DisplayPeak(item);
        }

        private void ExcuteReportSpecification(NaviItem item)
        {
            DifferenceDevice.MediumAccess.ReportSpecification(item);
        }

        private void CheckLogSpectrum(NaviItem item)
        {
            item.MenuStripItem.Checked = !item.MenuStripItem.Checked;
            DifferenceDevice.interClassMain.CheckLogSpectrum(item.MenuStripItem.Checked);
        }


        /// <summary>
        /// 
        /// 打开纯元素谱库
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreatePureSpecLib()
        {

            //SelectSample us = new SelectSample(WorkCurveHelper.VirtualSpecList);
            // string inMsg = InputBox.ShowInputBox("请输入管理员(admin)的密码", string.Empty);
            //if (inMsg.Trim() != string.Empty)
            //{
            //    MessageBox.Show(inMsg);
            //}
            try
            {
                UCInputPWD pwd = new UCInputPWD();

                if (pwd.ShowDialog() == DialogResult.Yes)
                {
                    UCPureSpecLib pureSpec = new UCPureSpecLib();
                    return pureSpec;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public UCMultiple CreateAdjustPure()
        {
            UCAdjustPureSpecLib ucAjustPure = new UCAdjustPureSpecLib();
            return ucAjustPure;
        }

        /// <summary>
        /// 执行显示元素在谱图中
        /// </summary>
        /// <param name="item"></param>
        private void ExcuteDisplayElement(NaviItem item)
        {
            DifferenceDevice.MediumAccess.DisplayElement(item);
        }


        /// <summary>
        /// 执行自动分析
        /// </summary>
        /// <param name="item"></param>
        private void ExcuteAutoAnalysis(NaviItem item)
        {
            DifferenceDevice.MediumAccess.AutoAnalysis();
        }

        /// <summary>
        /// 执行自动校准
        /// </summary>
        /// <param name="item"></param>
        private void ExcuteAutoCorrect(NaviItem item)
        {
            //if (!DifferenceDevice.IsAnalyser)
            //{
            //    var us = new UcInCalibrate();
            //    WorkCurveHelper.OpenUC(us, true);
            //}
            ////DifferenceDevice.interClassMain.AutoCorrection(null); 
            ////开始初始化
            //else 
            //if (DifferenceDevice.interClassMain != null)
            //{
            //    DifferenceDevice.IntensityCorrect.StartInitial();
            //}
            //var us = new UcInCalibrate();
            //return us;
        }
        /// <summary>
        /// 界面中combox模式改变通知事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboStrip_SelectedIndexChanged1(object sender, EventArgs e)
        {
            ToolStripComboBox combox = sender as ToolStripComboBox;
            this.modelTool = combox.SelectedIndex;
            List<NaviItem> objectNavi = combox.Tag as List<NaviItem>;
            if (this.modelTool == 1 && objectNavi != null && objectNavi.Count > 0)
            {
                foreach (NaviItem item in objectNavi)
                {
                    if (item.Name == "Curve")//add by chuyaqin 2011-05-26 智能模式下没有曲线选择
                    {
                        item.Enabled = false;
                        continue;
                    }
                    if (item.Name == "FpFpSepcCalibrate")
                    {
                        item.Enabled = true;
                        continue;
                    }
                    if (item.Name == "Quality")
                    {
                        item.Text = Info.ExploreAnalysis;
                    }
                    else
                        item.Enabled = true;
                }
            }
            else
            {
                foreach (NaviItem item in objectNavi)
                {
                    User current = User.FindOne(w => w.Name == FrmLogon.userName);
                    if (item.Name == "Curve" && current != null && current.Role.RoleType != 2)//add by chuyaqin 2011-05-26 正常模式下有曲线选择
                    {
                        item.Enabled = true;
                        continue;
                    }
                    if (item.Name == "FpFpSepcCalibrate")
                    {
                        item.Enabled = false;
                        continue;
                    }
                    if (item.Name == "Quality")
                    {
                        item.Text = Info.Quality;
                    }
                    else
                        item.Enabled = false;
                }
            }
            DifferenceDevice.MediumAccess.SelectMode(combox.SelectedIndex);
        }

        /// <summary>
        /// 标准样品模板
        /// </summary>
        /// <returns></returns>
        private UCMultiple CreateStandardTemplate()
        {
            if (CustomStandard.FindAll().Count <= 1)
            {
                FrmSampleTemplate standard = new FrmSampleTemplate(CustomStandard.FindAll()[0], WorkCurveHelper.WorkCurveCurrent.ElementList);
                return standard;
            }
            else
            {
                FrmSelectStandard fss = new FrmSelectStandard();
                if (fss.ShowDialog() == DialogResult.OK)
                {
                    FrmSampleTemplate standard = new FrmSampleTemplate(fss.CurrentStandard, WorkCurveHelper.WorkCurveCurrent.ElementList);
                    return standard;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// 构造自定义标准
        /// </summary>
        /// <returns></returns>
        private UCMultiple CreateStandard()
        {
            UCCustomStandard standard = new UCCustomStandard();
            return standard;
        }

        private UCMultiple CreateJapanStandard()
        {
            FrmResultStandard frs = new FrmResultStandard();
            return frs;
        }

        private UCMultiple CreateParamsSet()
        {
            UCCaculateParamSet ucp = new UCCaculateParamSet();
            return ucp;
        }


        public void  programMode( NaviItem item)
        {
            DifferenceDevice.CurCameraControl.programMode();
            
        }
        private UCMultiple CreateSysConfig()
        {
            UCSysConfig uc = new UCSysConfig();
            return uc;
        }

        private UCMultiple CreateIPSettings()
        {
            UCIPSetting ipSetting = new UCIPSetting();
            return ipSetting;
        }

        private UCMultiple CreateSurfaceSource()
        {
            UCSurfaceSource UC = new UCSurfaceSource();
            return UC;
        }

        private UCMultiple CreateSpecifications()
        {
            UCSpecifications usSpecification = new UCSpecifications();
            return usSpecification;
        }

        private UCMultiple CreateStyle()
        {
            UCStyle style = new UCStyle();
            return style;
        }

        private UCMultiple SettingName()
        {
            UCNameSetting nameset = new UCNameSetting();
            return nameset;
        }

        private UCMultiple PWDLock()
        {
            UCPWDLock uc = new UCPWDLock(false);
            return uc;
        }

        /// <summary>
        /// 构造历史记录
        /// </summary>
        /// <returns></returns>
        private UCMultiple CreateHistoryRecord()
        {
            WorkCurveHelper.IsCreateHistory = true;
            if (!ReportTemplateHelper.IsSplitScreen)
            {

                UCHistoryRecord historyRecord = new UCHistoryRecord();
                Control[] controls = historyRecord.Controls.Find("dgvHistoryRecord", true);
                var dgv = (controls[0] as DataGridViewW);
                Skyray.Language.Lang.Model.SaveHeaderTextProperty(dgv.Columns);
                Skyray.Language.Lang.Model.RegistDGVColAddedEvent(dgv);

                //if(OnHistoryTemplateSave!=null)
                //    historyRecord.OnHistoryTemplateSave += new EventDelegate.HistoryTemplateSave(OnHistoryTemplateSave);

                return historyRecord;
            }
            else
            {

                //  UCHistoryRecordSplitScreen historyRecord = new UCHistoryRecordSplitScreen();
                foreach (Form child in Application.OpenForms)
                {
                    var control = child.Controls[0];
                    //  if (control is UCHistoryRecordSplitScreen)
                    if (control is UCHistoryRecord)
                    {
                        return null;
                    }
                }
                UCHistoryRecord historyRecord = new UCHistoryRecord(false);
                WorkCurveHelper.OpenUC(historyRecord, true, Info.HistoryRecord, false);

                return null;
            }
        }

        //private UCMultiple CreateStock()
        //{
        //    UCStock Stock = new UCStock();
        //    Control[] controls = Stock.Controls.Find("dgvHistoryRecord", true);
        //    var dgv = (controls[0] as DataGridViewW);
        //    Skyray.Language.Lang.Model.SaveHeaderTextProperty(dgv.Columns);
        //    Skyray.Language.Lang.Model.RegistDGVColAddedEvent(dgv);
        //    return Stock;
        //}

        private UCMultiple ResolveCaculate()
        {
            UCResolveParams uc = new UCResolveParams();
            return uc;
        }


        public UCMultiple CreatePreHeat()
        {
            var uc = new UCPreHeatParams();
            return uc;
        }




        private NaviItem preItem;

        public void CurrentCountRate(NaviItem item)
        {
            if (item.MenuStripItem.Checked)
                item.MenuStripItem.Checked = false;
            else
            {

                if (preItem != null)
                    preItem.MenuStripItem.Checked = false;
                item.MenuStripItem.Checked = true;
            }
            preItem = item;
            if (DifferenceDevice.ithick != null)
                DifferenceDevice.ithick.SelectCountStyle(true);
        }

        public void AverageMode(NaviItem item)
        {
            item.MenuStripItem.Checked = !item.MenuStripItem.Checked;
            if (DifferenceDevice.iEdxrf != null)
                DifferenceDevice.iEdxrf.ShowResultType(item.MenuStripItem.Checked);
        }

        public void ConcluteMode(NaviItem item)
        {
            item.MenuStripItem.Checked = !item.MenuStripItem.Checked;
            if (DifferenceDevice.iEdxrf != null)
                DifferenceDevice.iEdxrf.SetConcluteMode(item.MenuStripItem.Checked);
        }

        public void CreateSpecType(NaviItem item)
        {
            item.MenuStripItem.Checked = !item.MenuStripItem.Checked;
            WorkCurveHelper.SelectShowType = item.MenuStripItem.Checked ? 1 : 0;
        }

        public void TempleteModeNormal(NaviItem item)
        {
            if (item.MenuStripItem.Checked)
            {
                return;
            }
            else
            {
                item.MenuStripItem.Checked = true;
                NaviItem ni = WorkCurveHelper.NaviItems.Find(a => a.Name.Equals("CustomTemplate"));
                if (ni != null)
                    ni.MenuStripItem.Checked = false;
                ReportTemplateHelper.ExcelModeType = 12;
                if (DifferenceDevice.ithick != null)
                    DifferenceDevice.ithick.AppSave();
            }

            //if (item.Text.Equals(Info.DefiniteTemplate) && item.MenuStripItem.Checked)
            //{
            //    ReportTemplateHelper.ExcelModeType = 2;
            //}
            //else
            //{
            //    ReportTemplateHelper.ExcelModeType = 12;
            //}
            //preItem = item;
            //if (DifferenceDevice.ithick != null)
            //    DifferenceDevice.ithick.AppSave();

        }

        public void TempleteModeCustom(NaviItem item)
        {
            if (item.MenuStripItem.Checked)
            {
                return;
            }
            else
            {
                item.MenuStripItem.Checked = true;
                NaviItem ni = WorkCurveHelper.NaviItems.Find(a => a.Name.Equals("NormalMode"));
                if (ni != null)
                    ni.MenuStripItem.Checked = false;
                ReportTemplateHelper.ExcelModeType = 2;
                if (DifferenceDevice.ithick != null)
                    DifferenceDevice.ithick.AppSave();
            }
            //if (item.Text.Equals(Info.DefiniteTemplate) && item.MenuStripItem.Checked)
            //{
            //    ReportTemplateHelper.ExcelModeType = 2;
            //}
            //else
            //{
            //    ReportTemplateHelper.ExcelModeType = 12;
            //}
            //preItem = item;
            //if (DifferenceDevice.ithick != null)
            //    DifferenceDevice.ithick.AppSave();

        }

        public void AverageCountRate(NaviItem item)
        {
            if (item.MenuStripItem.Checked)
                item.MenuStripItem.Checked = false;
            else
            {

                if (preItem != null)
                    preItem.MenuStripItem.Checked = false;
                item.MenuStripItem.Checked = true;
            }
            preItem = item;
            if (DifferenceDevice.ithick != null)
                DifferenceDevice.ithick.SelectCountStyle(false);
        }

        public UCMultiple CreateCountBit()
        {
            UCCaculateBits bits = new UCCaculateBits();
            return bits;
        }

        public UCMultiple CreateMatch()
        {
            UCParamsSetting setting = new UCParamsSetting();
            return setting;
        }




        public UCMultiple CreateEncoderValue()
        {
            UCEncoderValue ucvalue = new UCEncoderValue();
            return ucvalue;
        }
        /// <summary>
        /// 构造纯元素谱
        /// </summary>
        /// <returns></returns>
        private UCMultiple CreateElementSpectrum()
        {
            UCPureElementsTable table = new UCPureElementsTable();
            return table;
        }

        /// <summary>
        /// 开始扫描通知主界面
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateNewSpec()
        {
            NewWorkSpec returnNew = NewWorkSpec.GetInstance(this.modelTool);
            return returnNew;
        }

        /// <summary>
        /// 批量测试
        /// </summary>
        /// <returns></returns>
        public void CreateBatchTest(NaviItem nav)
        {
            if (DifferenceDevice.interClassMain.MainForm != null && DifferenceDevice.interClassMain.MainForm is IBatchTest)
            {
                var mainForm = DifferenceDevice.interClassMain.MainForm;
                var frm = new FrmBatchTest(mainForm as IBatchTest);
                frm.Owner = mainForm;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(
                    mainForm.Location.X + (mainForm.Width - frm.Width) / 2,
                    mainForm.Location.Y + (mainForm.Height - frm.Height) / 2
                    );
                frm.Show();
                nav.EnabledControl = false;
            }
        }

        /// <summary>
        /// 高纯金测试
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreatePureAuCalc()
        {
            NewWorkSpec returnNew = null;
            string sql = "select * from WorkCurve as a join Condition as b on a.Condition_Id = b.Id join Device as d on d.Id=b.Device_Id where d.Id =" + WorkCurveHelper.DeviceCurrent.Id + " and a.Name='PureAu'";//+ WorkCurveHelper.DeviceCurrent.Id ;

            WorkCurve current = WorkCurve.FindBySql(sql).Count == 0 ? null : WorkCurve.FindBySql(sql)[0];
            if (current != null)
            {
                WorkCurveHelper.IsPureAuTest = true;
                WorkCurveHelper.BeforePureAuCurveCurrent = WorkCurveHelper.WorkCurveCurrent;
                WorkCurveHelper.WorkCurveCurrent = current;
                DifferenceDevice.MediumAccess.OpenCurveSubmit();
                returnNew = NewWorkSpec.GetInstance(this.modelTool);
            }
            else
            {
                Msg.Show(Info.WorkCurveDelete, Info.TestWarning, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //  if (!returnNew)
            //     WorkCurveHelper.WorkCurveCurrent = WorkCurveHelper.BeforePureAuCurveCurrent;
            return returnNew;
        }

        public UCMultiple GainSetFOR3000()
        {
            UCGainSetFor3000 uc = new UCGainSetFor3000();
            return uc;
        }

        /// <summary>
        /// 打开工作曲线
        /// </summary>
        /// <returns></returns>
        private UCMultiple CreateCurve()
        {
            UCCurve returnControl = new UCCurve();
            return returnControl;
        }

        private void CopyCurrentWorkCurve(NaviItem item)
        {
            DifferenceDevice.MediumAccess.CopyCurrentWorkCurve();
        }

        /// <summary>
        /// 添加感兴趣元素
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateElement()
        {
            //UserControl uc = null;
            if (WorkCurveHelper.WorkCurveCurrent == null)
                return null;
            UCElement element = new UCElement();
            return element;
        }

        public UCMultiple CreateLineParam()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Contains("India"))
                return new FrmAboutIndia();
            else
                return new FrmAbout();

        }

        public UCMultiple CreateAuthorization()
        {
            return new FrmAuthorization();

        }
        private void GetSoftManual(NaviItem item)
        {
            string strSoftMannualName = string.Empty;
            if (DifferenceDevice.IsAnalyser) strSoftMannualName = ".\\SoftGuide_ForEDX3000.chm";
            else if (DifferenceDevice.IsXRF) strSoftMannualName = ".\\SoftGuide_ForXRF.chm";
            else if (DifferenceDevice.IsRohs) strSoftMannualName = ".\\SoftGuide_ForRohs.chm";
            else if (DifferenceDevice.IsThick) strSoftMannualName = ".\\SoftGuide_ForThick.chm";
            if (File.Exists(Application.StartupPath + strSoftMannualName))
                System.Diagnostics.Process.Start(Application.StartupPath + strSoftMannualName);
            else Msg.Show(Info.FileNotExist);
        }
        private void GetMachineManual(NaviItem item)
        {
            if (File.Exists(Application.StartupPath + ".\\HardGuide.chm"))
                System.Diagnostics.Process.Start(Application.StartupPath + ".\\HardGuide.chm");
            else Msg.Show(Info.FileNotExist);
        }
        /// <summary>
        /// 扫描停止
        /// </summary>
        /// <param name="item"></param>
        public void ExcuteStopOperation(NaviItem item)
        {
            DifferenceDevice.MediumAccess.TestStop();
        }

        public void ExcuteShowDialog(NaviItem item)
        {
            DifferenceDevice.interClassMain.ShowMeasureDialog(item);
        }

        public void ExcuteInitializationOperation_Modbus()
        {
            if (WorkCurveHelper.initDoing > 1)
            {

                Msg.Show("正在通过modbusTcp初始化，不可重复初始化");
                return;

            }
            DifferenceDevice.MediumAccess.TestInitialization();
        }

        NaviItem showResultItem;
        public void AutoShowResult(NaviItem item)
        {
            if (item.MenuStripItem.Checked)
            {
                item.MenuStripItem.Checked = false;
                if (showResultItem != null)
                    showResultItem.MenuStripItem.Checked = true;
            }
            else
            {

                if (showResultItem != null)
                    showResultItem.MenuStripItem.Checked = false;
                item.MenuStripItem.Checked = true;
            }
            showResultItem = item;
            if (DifferenceDevice.iEdxrf != null)
                DifferenceDevice.iEdxrf.SetAutoManualType(true);
        }

        public void ManualShowResult(NaviItem item)
        {
            if (item.MenuStripItem.Checked)
            {
                item.MenuStripItem.Checked = false;
                if (showResultItem != null)
                    showResultItem.MenuStripItem.Checked = true;
            }
            else
            {

                if (showResultItem != null)
                    showResultItem.MenuStripItem.Checked = false;
                item.MenuStripItem.Checked = true;
            }
            showResultItem = item;
            if (DifferenceDevice.iEdxrf != null)
                DifferenceDevice.iEdxrf.SetAutoManualType(false);
        }

        public void Spin(NaviItem item)
        {
            item.MenuStripItem.Checked = item.MenuStripItem.Checked ? false : true;
            DifferenceDevice.Spin = item.MenuStripItem.Checked;
        }

        /// <summary>
        /// 扫描初始化
        /// </summary>
        /// <param name="item"></param>
        public void ExcuteInitializationOperation(NaviItem item)
        {
            DifferenceDevice.MediumAccess.TestInitialization();
        }


        /// <summary>
        /// 扫描执行计算
        /// </summary>
        /// <param name="item"></param>
        public void ExcuteCaculateOperation(NaviItem item)
        {
            DifferenceDevice.MediumAccess.ExcuteCaculate();

        }


        public void ExcuteMatchParams(NaviItem item)
        {
            if (DifferenceDevice.irohs != null)
                DifferenceDevice.irohs.ExcuteMatchParamsInput(item);
        }

        public void ExcuteDetection(NaviItem item)
        {
            DifferenceDevice.interClassMain.AutoDetection();
        }

        public void ExcuteConnectDevice(NaviItem item)
        {
            DifferenceDevice.MediumAccess.ConnectDevice();
        }

        public void ExcuteAutoDemarcateEnergy(NaviItem item)
        {
            DifferenceDevice.MediumAccess.ExcuteAutoDemarcateEnergy();
        }
        ////取消 
        //public void ExcuteAutoFPGAIntercept(NaviItem item)
        //{
        //    DifferenceDevice.MediumAccess.ExcuteAutoFPGAIntercept();
        //}
        public UCMultiple CreateConSampleCal()
        {
            UCSampleCal UCSampleCalForm = new UCSampleCal();
            return UCSampleCalForm;
        }
        /// <summary>
        /// 分析参数
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateAnalyParam()
        {
            UCAnalysisParam UCAnalysisParam = new UCAnalysisParam();
            return UCAnalysisParam;

        }

        /// <summary>
        /// 添加用户管理
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateUserManager()
        {
            FrmUser userForm = new FrmUser();
            return userForm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateCoeeParam()
        {
            UCCoeeParam UCCoeeParam = new UCCoeeParam();
            return UCCoeeParam;
        }

        /// <summary>
        /// 编辑语种
        /// </summary>
        /// <returns></returns>
        public UCMultiple EditLanguage()
        {
            UCLanguage UCLanguage = new UCLanguage();
            return UCLanguage;

        }

        /// <summary>
        /// 自定义元素
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateCustomFiled()
        {
            UCCustomField UCCustomField = new UCCustomField();
            return UCCustomField;
        }




        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateEditData()
        {
            UCMultiple uc;
            if (WorkCurveHelper.WorkCurveCurrent == null) return null;
            UCEditData element = new UCEditData();
            uc = element;
            return uc;
        }

        public UCMultiple CreateMatchSpec()
        {
            UCMultiple uc;
            if (WorkCurveHelper.WorkCurveCurrent == null) return null;
            UCWorkCurveMatch element = new UCWorkCurveMatch();
            element.FormType = 0;
            uc = element;
            return uc;
        }

        public UCMultiple SettingVirtualSpec()
        {
            UCMultiple uc;
            if (WorkCurveHelper.WorkCurveCurrent == null) return null;
            UCWorkCurveMatch element = new UCWorkCurveMatch();
            element.FormType = 1;
            uc = element;
            return uc;
        }
        /// <summary>
        /// 数据优化
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateOptimization()
        {
            UCOptimization UCOptimization = new UCOptimization();
            return UCOptimization;
        }

        /// <summary>
        /// 影响元素
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateElementRef()
        {
            UCRef UCRef = new UCRef();
            return UCRef;
        }

        /// <summary>
        /// 测量条件
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateUCCondition()
        {
            FrmCondition FrmCondition = new FrmCondition();
            return FrmCondition;

        }

        public UCMultiple CreateSettingMeasureTime()
        {
            FrmMeasureSetting setting = new FrmMeasureSetting();
            return setting;
        }

        public UCMultiple CreateUCEditWorkgion()
        {
            UCWorkRegionSetting wrSetting = new UCWorkRegionSetting();
            return wrSetting;
        }




        /// <summary>
        /// 打开工作谱
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateSpec()
        {

            SelectSample us = new SelectSample(AddSpectrumType.OpenSpectrum);
            return us;
        }

        /// <summary>
        /// 打开对比谱
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateVirtualSpecs()
        {
            //SelectSample us = new SelectSample(WorkCurveHelper.VirtualSpecList);
            UCVirtualSelect selectVirtual = new UCVirtualSelect();
            return selectVirtual;
        }

        public UCMultiple CreateTitle()
        {
            UCCreateTitle uc = new UCCreateTitle();
            return uc;
        }

        public UCMultiple CreateToolsAppConfig()
        {
            ToolsNaviForm ucForm = new ToolsNaviForm();
            return ucForm;
        }

        //公司其他信息
        public UCMultiple CompanyOtherInformation()
        {
            UCOtherInfo ucForm = new UCOtherInfo();
            //UCCompanyOthers ucForm = new UCCompanyOthers();
            return ucForm;
        }

        public UCMultiple CreateUIConfig()
        {
            UIConfig uiConfig = new UIConfig();
            return uiConfig;
        }

        //20110615 何晓明 备份还原 
        public UCMultiple CreateBackUpAndRestore()
        {
            UCBackUpAndRestore ucBackupAndRestore = new UCBackUpAndRestore();
            return ucBackupAndRestore;
        }

        //数据库清理
        public UCMultiple CreateThinDatabase()
        {
            UCThinDatabase ucThinDatabase = new UCThinDatabase();
            return ucThinDatabase;
        }

        public UCMultiple CreateParamsConfig()
        {
            UCUserConfigParams fig = new UCUserConfigParams();
            return fig;
        }

        public UCMultiple CreateWarningSettings()
        {
            UCWarningSettings uc = new UCWarningSettings();
            return uc;
        }


        public UCMultiple CreateBlueSet()
        {
            FrmBlueToothCfg fb = new FrmBlueToothCfg();
            return fb;
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateUCPrint()
        {
            //DifferenceDevice.IsExcutePrint = true;
            //DifferenceDevice.MediumAccess.ExcuteCaculate();
            //DifferenceDevice.IsExcutePrint = false;


            var ucPrint = new UCPrint();
            if (ReportTemplateHelper.ExcelModeType == 0 || ReportTemplateHelper.ExcelModeType == 1)
            {
                if (OnPrintTemplateSource != null)
                    if (!OnPrintTemplateSource())
                    {
                        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
            }
            else if (ReportTemplateHelper.ExcelModeType == 2)
            {
                if (!InterfaceClass.SetPrintTemplate(null, null))
                {
                    Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }


            }
            //string Edition = "";
            //if (DifferenceDevice.IsAnalyser) Edition = "EDXRF";
            //else if (DifferenceDevice.IsRohs) Edition = "Rohs";
            //else if (DifferenceDevice.IsThick) Edition = "FPThick";
            //else if (DifferenceDevice.IsXRF) Edition = "XRF";

            ucPrint.curreEdition = ReportTemplateHelper.Edition;
            ucPrint.DataSource = DirectPrintLibcs.lst;
            ucPrint.Load += new EventHandler(PageLoad);
            ucPrint.SaveTemplateEvent += new EventHandler(ucPrint_SaveTemplateEvent);
            return ucPrint;

            #region 备份

            //var ucPrint = new UCPrint();
            //if (OnPrintTemplateSource != null)
            //    if (!OnPrintTemplateSource())
            //    {
            //        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return null;
            //    }
            //ucPrint.DataSource = DirectPrintLibcs.lst;
            //ucPrint.Load += new EventHandler(PageLoad);
            //ucPrint.SaveTemplateEvent += new EventHandler(ucPrint_SaveTemplateEvent);
            //return ucPrint;

            #endregion
        }

        public List<TreeNodeInfo> list = new List<TreeNodeInfo>();

        public void ExcutePrint(NaviItem item)
        {
            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE, 0, 0);

            list.Clear();
            DifferenceDevice.MediumAccess.DirectPrint(list);
            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE_HIDE, 0, 0);

        }

        public void ExcuteDirectPrint(NaviItem item)
        {
            if (WorkCurveHelper.PrinterType == 1)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(DifferenceDevice.interClassMain.PrinterBlueThread), null);
                return;
            }
            else
            {
                DifferenceDevice.interClassMain.PrintExcel(list);
            }

        }

        public void ExcuteBlueToothPrint(NaviItem item)
        {
            DifferenceDevice.interClassMain.PrintBlueExcel();
        }

        public void DisappearBk(NaviItem item)
        {
            DifferenceDevice.MediumAccess.DisappearBk();
        }

        public void OpenRohs3(NaviItem item)
        {
            if (DifferenceDevice.irohs != null)
                DifferenceDevice.irohs.OpenRohs3(null);
        }

        public void OpenSpec(NaviItem item)
        {
            EDXRFHelper.GetReturnSpectrum(true, false);
        }

        public void OpenRohs4(NaviItem item)
        {
            if (DifferenceDevice.irohs != null)
                DifferenceDevice.irohs.OpenRohs4(null);
        }

        //public void OpenOldSpec(NaviItem item)
        //{
        //    DifferenceDevice.MediumAccess.OpenOldSpec();
        //}

        void ucPrint_SaveTemplateEvent(object sender, EventArgs e)
        {
            DifferenceDevice.MediumAccess.SaveTemplateUpdateEvent();
        }


        public void CreateIntRegion(NaviItem item)
        {
            DifferenceDevice.MediumAccess.CreateIntRegion();
        }

        public void CaculateIntRegion(NaviItem item)
        {
            DifferenceDevice.MediumAccess.CaculateIntRegion();
        }

        public void Exit(NaviItem item)
        {
            DifferenceDevice.interClassMain.Exist();
        }

        public void LogOut(NaviItem item)
        {
            DifferenceDevice.interClassMain.LogOut();
        }

        public UCMultiple ComputeSampleIntensity()
        {
            string sampleName = string.Empty;
            bool IsExplore = false;
            ElementList list = DifferenceDevice.MediumAccess.CaculateIntensityReport(out sampleName, out IsExplore);
            if (list == null || sampleName.IsNullOrEmpty())
                return null;
            var us = new UCComputeIntensity();
            us.elementList = list;
            us.sampleName = sampleName;
            us.IsExplore = IsExplore;
            return us;
        }
        //add by chuyaqin begin
        public UCMultiple ComputeSpecResolve()
        {
            if (WorkCurveHelper.MainSpecList != null)
            {
                var us = new UCCalcResolution(WorkCurveHelper.MainSpecList, WorkCurveHelper.DeviceCurrent, WorkCurveHelper.ResolveFactor, WorkCurveHelper.intResolve);
                return us;
            }
            return null;
        }

        public UCMultiple ComputeMatching()
        {
            UCCaculateMatching ucMatching = new UCCaculateMatching();
            return ucMatching;
        }

        public UCMultiple FpSpecCalibrate()
        {
            var us = new UCCoeeParamIntel();
            return us;
        }
        public UCMultiple CreateExpunction()
        {
            var us = new UCExpunction();
            return us;
        }
        public UCMultiple CreateUCSpecialSpec()
        {
            var us = new UCSpecialSpec();
            return us;
        }
        public UCMultiple RExcusionCalibrate()
        {
            //string sampleName = string.Empty;
            //var us = new UcInCalibrate();
            //us.BindingContext(Info.RExcusionCalibrate);
            //if (Msg.Show("确定要校正强度吗？","", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            //    return null;
            var us = new UcInCalibrate();
            return us;
        }
        //add by chuyaqin end 
        public UCMultiple AnalysisReport()
        {
            int[] lines = new int[1];
            string[] elementName = DifferenceDevice.MediumAccess.QualityResult(out lines);
            if (elementName != null && lines != null)
            {
                var uc = new UCQualityResult(elementName, lines);
                return uc;
            }
            return null;
        }

        public void DisplaySamplelog(NaviItem item)
        {
            DifferenceDevice.MediumAccess.DisplayLogData();
        }

        public void PageLoad(object sender, EventArgs e)
        {
            if (Skyray.Language.Lang.Model.CheckToSaveText())
            {
                Skyray.Language.Lang.Model.SaveFormText(true, (sender as UCPrint).PropertyPanel);
                Skyray.Language.Lang.Model.SaveFormText(true, sender);
            }
            else
            {
                Skyray.Language.Lang.Model.SetFormText(true, (sender as UCPrint).PropertyPanel);
                Skyray.Language.Lang.Model.SetFormText(true, sender);
            }

            Skyray.Language.Lang.Model.LanguageChanged += (s, ex) =>
            {
                Skyray.Language.Lang.Model.SetFormText(true, (sender as UCPrint).PropertyPanel);
                Skyray.Language.Lang.Model.SetFormText(true, sender);
            };

            Form fom = (sender as UCPrint).ParentForm;
            if (fom != null)
            {
                fom.MaximizeBox = true;
                fom.MinimizeBox = false;
                fom.WindowState = FormWindowState.Maximized;
            }
        }

        public void CreateSuperModel(NaviItem item)
        {
            FrmSuperModel uc = new FrmSuperModel();
            if (uc.ShowDialog() == DialogResult.OK)
            {
                //DifferenceDevice.interClassMain.SelectTargetModel();
            }
        }

        /// <summary>
        /// 形状
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateShape()
        {
            UCShape UCShape = new UCShape();
            return UCShape;
        }

        /// <summary>
        /// 供应商
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateSupplier()
        {
            UCSupplier UCSupplier = new UCSupplier();
            return UCSupplier;
        }

        public UCMultiple CreateDefinePureElement()
        {
            UCDefinePureElem UCDefinePure = new UCDefinePureElem();
            return UCDefinePure;
        }

        public UCMultiple CreateTabParams()
        {

            UcTabParams UCTabParams = new UcTabParams();
            return UCTabParams;
        }

        public UCMultiple CreateSysSettings()
        {

            UCSysSettings ucSysSettings = new UCSysSettings();
            return ucSysSettings;
        }

        /// <summary>
        /// 元素周期表
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateElementTable()
        {
            FrmElementTable frmElementTable = new FrmElementTable(WorkCurveHelper.Atoms, WorkCurveHelper.Lines, null, true, false);
            frmElementTable.ShowKL = true;
            return frmElementTable;
        }

        public UCMultiple CreateDBSetting()
        {

            UCDBSetting ucDBSetting = new UCDBSetting();
            return ucDBSetting;
        }

        public UCMultiple CreateSerialSetting()
        {

            FrmComSel frmcom = new FrmComSel();
            return frmcom;
        }

        public UCMultiple CreateDetectPointsSetting()
        {

            UCDetectPointsSetting ucDetectPointsSetting = new UCDetectPointsSetting();
            return ucDetectPointsSetting;
        }


        /// <summary>
        /// 联测历史记录
        /// </summary>
        /// <returns></returns>
        public UCMultiple CreateHistoryRecordContinuous()
        {
            UCHistoryRecordContinuous uc = new UCHistoryRecordContinuous();
            uc.OnContinuousData += new EventDelegate.ContinuousData(uc_OnContinuousData);
            return uc;
        }

        void uc_OnContinuousData(List<System.Data.DataTable> dt, List<object> obj)
        {
            if (OnPassingData != null)
                OnPassingData(dt, obj);
        }


        public void SaveScreenshots(NaviItem item)
        {
            DifferenceDevice.interClassMain.Screenshots(true);
        }

        public void PrintScreenshots(NaviItem item)
        {
            DifferenceDevice.interClassMain.Screenshots(false);
        }

        public void SaveResults(NaviItem item)
        {
            DifferenceDevice.interClassMain.SeleTemplatePrint(false);
        }

        public void PrintResults(NaviItem item)
        {
            DifferenceDevice.interClassMain.SeleTemplatePrint(true);
        }

        public void TestOnCoverClosedEnableChanged(NaviItem item)
        {
            if (!item.MenuStripItem.Checked)
            {
                if (WorkCurveHelper.WorkCurveCurrent == null
                    || WorkCurveHelper.WorkCurveCurrent.ElementList == null
                    || WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count <= 0)
                {
                    Msg.Show(Info.WarningTestContext);
                    return;
                }
            }
            WorkCurveHelper.TestOnCoverClosedEnabled = item.MenuStripItem.Checked = !item.MenuStripItem.Checked;
            if (item.MenuStripItem.Checked)
            {
                item.Image = GetIcoImage("EnableTestOnCoverClosed");
                NaviItem item2 = WorkCurveHelper.NaviItems.Find(w => w.Name == "TestOnButtonPressedEnabled");
                if (item2 != null)
                {
                    WorkCurveHelper.TestOnButtonPressedEnabled = item2.MenuStripItem.Checked = false;
                    item2.Image = GetIcoImage("TestOnButtonPressedEnabled");
                }
            }
            else
            {
                item.Image = GetIcoImage("TestOnCoverClosedEnabled");
            }
            //ReportTemplateHelper.SaveSpecifiedValue(Application.StartupPath + "\\AppParams.xml", "TestParams", "TestOnCoverClosedEnabled", DifferenceDevice.interClassMain.TestOnCoverClosedEnabled.ToString());
        }

        public void TestOnButtonPressedEnabledChanged(NaviItem item)
        {
            if (!item.MenuStripItem.Checked)
            {
                if (WorkCurveHelper.WorkCurveCurrent == null
                    || WorkCurveHelper.WorkCurveCurrent.ElementList == null
                    || WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count <= 0)
                {
                    Msg.Show(Info.WarningTestContext);
                    return;
                }
            }
            WorkCurveHelper.TestOnButtonPressedEnabled = item.MenuStripItem.Checked = !item.MenuStripItem.Checked;
            if (item.MenuStripItem.Checked)
            {
                item.Image = GetIcoImage("EnableTestOnButtonPressed");
                NaviItem item2 = WorkCurveHelper.NaviItems.Find(w => w.Name == "TestOnCoverClosedEnabled");
                if (item2 != null)
                {
                    WorkCurveHelper.TestOnCoverClosedEnabled = item2.MenuStripItem.Checked = false;
                    item2.Image = GetIcoImage("TestOnCoverClosedEnabled");
                }
            }
            else
            {
                item.Image = GetIcoImage("TestOnButtonPressedEnabled");
            }
            ReportTemplateHelper.SaveSpecifiedValue(Application.StartupPath + "\\AppParams.xml", "TestParams", "TestOnButtonPressedEnabled", WorkCurveHelper.TestOnButtonPressedEnabled.ToString());
        }



        public void ExecuteFocusType(NaviItem item)
        {
            if (item.MenuStripItem.Checked == true) return;
            item.MenuStripItem.Checked = !item.MenuStripItem.Checked;
            bool smin = false;
            bool smiddle = false;
            bool smax = false;
            if (item.Name == "FocusMax")
            {
                WorkCurveHelper.FocusArea = 1;
                smax = true;
                smiddle = false;
                smin = false;
            }
            else if (item.Name == "FocusMiddle")
            {
                WorkCurveHelper.FocusArea = 2;

                smax = false;
                smiddle = true;
                smin = false;
            }
            else
            {
                WorkCurveHelper.FocusArea = 3;

                smax = false;
                smiddle = false;
                smin = true;
            }

            NaviItem fMax = WorkCurveHelper.NaviItems.Find(w => w.Name == "FocusMax");
            if (fMax != null)
            {
                fMax.MenuStripItem.Checked = smax;
            }

            NaviItem fMiddle = WorkCurveHelper.NaviItems.Find(w => w.Name == "FocusMiddle");
            if (fMiddle != null)
            {
                fMiddle.MenuStripItem.Checked = smiddle;
            }

            NaviItem fMin = WorkCurveHelper.NaviItems.Find(w => w.Name == "FocusMin");
            if (fMin != null)
            {
                fMin.MenuStripItem.Checked = smin;
            }

            ReportTemplateHelper.SaveSpecifiedValueandCreate("Camera", "FocusArea", WorkCurveHelper.FocusArea.ToString());


        }



        public void ExecuteZoomIn(NaviItem item)
        {
            DifferenceDevice.interClassMain.SetCameraCoeff(1);
        }


        public void ExecuteZoomOut(NaviItem item)
        {
            DifferenceDevice.interClassMain.SetCameraCoeff(-1);
        }

        public void ExecuteZoom(NaviItem item)
        {
            DifferenceDevice.interClassMain.SetCameraCoeff(0);
        }

    }
}
