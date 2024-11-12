using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.Language;
using Skyray.EDX.Common;
using System.Windows.Forms;
using Skyray.Controls;

namespace Skyray.UC
{
    ///// <summary>
    ///// 多语言实例对象
    ///// </summary>
    //public class Lang
    //{
    //    /// <summary>
    //    /// 记录当前语言信息
    //    /// </summary>
    //    public static LanguageModel Model;

    //    public static ToolStripMenuItem LangItem;
    //}
    /// <summary>
    /// 语言帮助类
    /// </summary>
    public class LangHelper
    {
        //public static void InitLang(params object[] objs)
        //{
        //    Skyray.Language.Lang.Model.LangObj.Add(new LangInfo
        //    {
        //        IsForm = false,
        //        PropertyNames = new string[] { "Text" },
        //        ObjToChangeLang = new object[] 
        //        { 
        //            WorkCurveHelper.NaviItems ,
        //            MessageInterface.AutoDic,
        //            UCHistoryRecord.AutoDic
        //        }
        //    });
        //    Skyray.Language.Lang.Model.LangObj.Add(new LangInfo
        //    {
        //        IsForm = true,
        //        PropertyNames = Skyray.Language.Param.PropertyNames,
        //        ObjToChangeLang = objs
        //    });

        //    Skyray.Language.Lang.Model.LangObj.Add(new LangInfo
        //    {
        //        IsForm = false,
        //        PropertyNames = Skyray.Language.Param.PropertyNames,
        //        ObjToChangeLang = ControlSerializable.DyObjs
        //    });

        //    LangHelper.SaveAllText();

        //    LangHelper.RegistDGVColAddedEvent();
        //}

        ///// <summary>
        ///// 保存Datagridview HeaderText
        ///// </summary>
        //private static void SaveDyDGVHeaderText()
        //{
        //    foreach (var dgv in ControlSerializable.DGVs)
        //    {
        //        foreach (DataGridViewColumn col in dgv.Columns)
        //        {
        //            SaveColHeaderText(col);
        //        }
        //    }
        //}
        //public static void RegistDGVColAddedEvent()
        //{
        //    foreach (var dgv in ControlSerializable.DGVs)
        //    {
        //        dgv.ColumnAdded += (sender, e) => SetColHeaderText(e.Column);
        //        if (!LanguageModel.LangDBOK && Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang)
        //        {
        //            dgv.ColumnAdded += (sender, e) => SaveColHeaderText(e.Column);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 设置Datagridview HeaderText
        ///// </summary>
        //public static void SetDyDGVHeaderText()
        //{
        //    foreach (var dgv in ControlSerializable.DGVs)
        //    {
        //        foreach (DataGridViewColumn col in dgv.Columns)
        //        {
        //            SetColHeaderText(col);
        //        }
        //    }
        //}

        //private static void SaveColHeaderText(DataGridViewColumn col)
        //{
        //    string str = col.HeaderText;
        //    string key = col.Name + ".HeaderText";

        //    if (Skyray.Language.Lang.Model.CheckString(ref str) && !Skyray.Language.Lang.Model.CheckData(key))
        //    {
        //        Skyray.Language.Lang.Model.CurrentLang.LanguageDatas.Add
        //            (LanguageData.New.Init(key, str));
        //    }
        //    Skyray.Language.Lang.Model.CurrentLang.Save();
        //}
        //private static void SetColHeaderText(DataGridViewColumn col)
        //{
        //    string key = col.Name + ".HeaderText";

        //    if (Skyray.Language.Lang.Model.LangData.ContainsKey(key))
        //    {
        //        col.HeaderText = Skyray.Language.Lang.Model.LangData[key];
        //    }
        //}

        ///// <summary>
        ///// 保存文本
        ///// </summary>
        //private static void SaveAllText()
        //{
        //    if (!LanguageModel.LangDBOK)
        //    {
        //        SaveDyDGVHeaderText();

        //        Skyray.Language.Lang.Model.SaveText(false, null, new string[] { "Text" }, MessageInterface.AutoDic,
        //            UCHistoryRecord.AutoDic, WorkCurveHelper.NaviItems);

        //        Skyray.Language.Lang.Model.SaveInfoText(Skyray.Language.Lang.Model.InfoTypes.ToArray());
        //        Skyray.Language.Lang.Model.SaveText(false, ControlSerializable.DyObjs.ToArray());
        //        foreach (var langInfo in Skyray.Language.Lang.Model.LangObj)
        //        {
        //            Skyray.Language.Lang.Model.SaveText(langInfo.IsForm, langInfo.PropertyNames, langInfo.ObjToChangeLang);
        //        }
        //    }
        //}

        /// <summary>
        /// 更改界面DatagridView语言
        /// </summary>
        /// <param name="dgvs"></param>
        //public static void ChangeDgvLang(params DataGridView[] dgvs)
        //{
        //    var dics = Skyray.EDX.Common.MessageInterface.AutoDic;//字典

        //    //遍历控件
        //    foreach (var dgv in dgvs)
        //    {
        //        if (dgv == null)
        //            continue;
        //        string key = string.Empty;//定义变量记录
        //        for (int i = 0; i < dgv.Rows.Count; i++)
        //        {
        //            object tag = dgv.Rows[i].Tag;
        //            if (tag == null) continue;

        //            key = tag.ToString();//查找该属性对应的key
        //            var auto = dics.FirstOrDefault(x => x.Key == key);//查找属性值
        //            if (auto != null) dgv[0, i].Value = auto.Text;//存在即赋值
        //        }
        //    }
        //}

        public static ToolStripMenuItem LastLangItem;
        /// <summary>
        /// 初始化语言菜单
        /// </summary>
        /// <param name="item"></param>
        public static void InitLangMenu(ToolStripMenuItem item)
        {
            if (item != null)
            {
                item.DropDownItems.Clear();
                Languages.FindAll().ForEach(lang =>
                {
                    var newItem = new ToolStripMenuItem(lang.FullName, null,
                        (sender, e) =>
                        {
                            if (Skyray.Language.Lang.Model.CurrentLangId != lang.Id)
                            {
                                Skyray.Language.Lang.Model.CurrentLangId = lang.Id;
                                Skyray.Language.Lang.Model.ChangeLanguage();
                                ChangeCameraLang();
                                if (LastLangItem != null) LastLangItem.Checked = false;
                                LastLangItem = sender as ToolStripMenuItem;
                                LastLangItem.Checked = true;
                                ReportTemplateHelper.LoadDirctoryTemplate();
                            }

                        });
                    //newItem.CheckOnClick = true;
                    if (Lang.Model != null)
                    {
                        newItem.Checked = Skyray.Language.Lang.Model.CurrentLangId == lang.Id;
                        if (newItem.Checked) LastLangItem = newItem;
                        item.DropDownItems.Add(newItem);
                        ChangeCameraLang();
                    }
                });
            }
        }



        public static void ChangeCameraLang()
        {
            if (Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.Count <= 0) return;
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("Flip0") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("Flip0"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["Flip0"] = Skyray.Language.Lang.Model.LangData["Flip0"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("Flip1") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("Flip1"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["Flip1"] = Skyray.Language.Lang.Model.LangData["Flip1"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("Flip2") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("Flip2"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["Flip2"] = Skyray.Language.Lang.Model.LangData["Flip2"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("Flip3") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("Flip3"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["Flip3"] = Skyray.Language.Lang.Model.LangData["Flip3"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("Flip4") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("Flip4"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["Flip4"] = Skyray.Language.Lang.Model.LangData["Flip4"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("Flip5") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("Flip5"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["Flip5"] = Skyray.Language.Lang.Model.LangData["Flip5"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("Flip6") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("Flip6"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["Flip6"] = Skyray.Language.Lang.Model.LangData["Flip6"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("Flip7") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("Flip7"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["Flip7"] = Skyray.Language.Lang.Model.LangData["Flip7"];
            }

            if (Skyray.Language.Lang.Model.LangData.ContainsKey("Flip4") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("Flip4"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["Flip4"] = Skyray.Language.Lang.Model.LangData["Flip4"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("Flip5") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("Flip5"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["Flip5"] = Skyray.Language.Lang.Model.LangData["Flip5"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("Flip6") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("Flip6"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["Flip6"] = Skyray.Language.Lang.Model.LangData["Flip6"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("Flip7") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("Flip7"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["Flip7"] = Skyray.Language.Lang.Model.LangData["Flip7"];
            }

            if (Skyray.Language.Lang.Model.LangData.ContainsKey("CameraIsNotOpen.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("CameraIsNotOpen"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["CameraIsNotOpen"] = Skyray.Language.Lang.Model.LangData["CameraIsNotOpen.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("tsmiOpen.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiOpen"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["tsmiOpen"] = Skyray.Language.Lang.Model.LangData["tsmiOpen.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("tsmiCameraProperty.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiCameraProperty"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["tsmiCameraProperty"] = Skyray.Language.Lang.Model.LangData["tsmiCameraProperty.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("tsmiCameraFormat.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiCameraFormat"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["tsmiCameraFormat"] = Skyray.Language.Lang.Model.LangData["tsmiCameraFormat.Text"];
            }


            if (Skyray.Language.Lang.Model.LangData.ContainsKey("tsmiCameraParam.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiCameraParam"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["tsmiCameraParam"] = Skyray.Language.Lang.Model.LangData["tsmiCameraParam.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("AutoSaveSamplePicToolStripMenuItem.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("AutoSaveSamplePicToolStripMenuItem"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["AutoSaveSamplePicToolStripMenuItem"] = Skyray.Language.Lang.Model.LangData["AutoSaveSamplePicToolStripMenuItem.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("tsmiGraphy.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiGraphy"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["tsmiGraphy"] = Skyray.Language.Lang.Model.LangData["tsmiGraphy.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("tsmiClose.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiClose"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["tsmiClose"] = Skyray.Language.Lang.Model.LangData["tsmiClose.Text"];
            }

            if (Skyray.Language.Lang.Model.LangData.ContainsKey("tsmiDelCurrentFlag.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiDelCurrentFlag"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["tsmiDelCurrentFlag"] = Skyray.Language.Lang.Model.LangData["tsmiDelCurrentFlag.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("tsmiDelAllFlag.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiDelAllFlag"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["tsmiDelAllFlag"] = Skyray.Language.Lang.Model.LangData["tsmiDelAllFlag.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("tsmAdjustOrient.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmAdjustOrient"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["tsmAdjustOrient"] = Skyray.Language.Lang.Model.LangData["tsmAdjustOrient.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("gbxFocus.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("gbxFocus"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["gbxFocus"] = Skyray.Language.Lang.Model.LangData["gbxFocus.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("lblFocusX.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblFocusX"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["lblFocusX"] = Skyray.Language.Lang.Model.LangData["lblFocusX.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("lblFocusY.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblFocusY"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["lblFocusY"] = Skyray.Language.Lang.Model.LangData["lblFocusY.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("lblSpotShape.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblSpotShape"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["lblSpotShape"] = Skyray.Language.Lang.Model.LangData["lblSpotShape.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("rbtnSpotShapeEllipse.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("rbtnSpotShapeEllipse"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["rbtnSpotShapeEllipse"] = Skyray.Language.Lang.Model.LangData["rbtnSpotShapeEllipse.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("rbtnSpotShapeRectangle.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("rbtnSpotShapeRectangle"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["rbtnSpotShapeRectangle"] = Skyray.Language.Lang.Model.LangData["rbtnSpotShapeRectangle.Text"];
            }


            if (Skyray.Language.Lang.Model.LangData.ContainsKey("gbxView.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("gbxView"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["gbxView"] = Skyray.Language.Lang.Model.LangData["gbxView.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("lblViewWidth.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblViewWidth"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["lblViewWidth"] = Skyray.Language.Lang.Model.LangData["lblViewWidth.Text"];
            }

            if (Skyray.Language.Lang.Model.LangData.ContainsKey("lblViewHeight.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblViewHeight"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["lblViewHeight"] = Skyray.Language.Lang.Model.LangData["lblViewHeight.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("lblScaleUnit.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblScaleUnit"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["lblScaleUnit"] = Skyray.Language.Lang.Model.LangData["lblScaleUnit.Text"];
            }

            if (Skyray.Language.Lang.Model.LangData.ContainsKey("btnDefault.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnDefault"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["btnDefault"] = Skyray.Language.Lang.Model.LangData["btnDefault.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("btnAccept.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnAccept"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["btnAccept"] = Skyray.Language.Lang.Model.LangData["btnAccept.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("lblVideoDev.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblVideoDev"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["lblVideoDev"] = Skyray.Language.Lang.Model.LangData["lblVideoDev.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("lblCameraSize.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblCameraSize"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["lblCameraSize"] = Skyray.Language.Lang.Model.LangData["lblCameraSize.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("btnCancel.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnCancel"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["btnCancel"] = Skyray.Language.Lang.Model.LangData["btnCancel.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("tsmiShowAllTestPoint.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiShowAllTestPoint"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["tsmiShowAllTestPoint"] = Skyray.Language.Lang.Model.LangData["tsmiShowAllTestPoint.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("tsmiSaveMultiPoint.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiSaveMultiPoint"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["tsmiSaveMultiPoint"] = Skyray.Language.Lang.Model.LangData["tsmiSaveMultiPoint.Text"];
            }

            if (Skyray.Language.Lang.Model.LangData.ContainsKey("tsmiSaveOutMultiPoint.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiSaveOutMultiPoint"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["tsmiSaveOutMultiPoint"] = Skyray.Language.Lang.Model.LangData["tsmiSaveOutMultiPoint.Text"];
            }

            if (Skyray.Language.Lang.Model.LangData.ContainsKey("lblmultiName.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblmultiName"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["lblmultiName"] = Skyray.Language.Lang.Model.LangData["lblmultiName.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("btnDel.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnDel"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["btnDel"] = Skyray.Language.Lang.Model.LangData["btnDel.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("btnEdit.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnEdit"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["btnEdit"] = Skyray.Language.Lang.Model.LangData["btnEdit.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("btnDeletePoint.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnDeletePoint"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["btnDeletePoint"] = Skyray.Language.Lang.Model.LangData["btnDeletePoint.Text"];
            }
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("btnSave.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnSave"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["btnSave"] = Skyray.Language.Lang.Model.LangData["btnSave.Text"];
            }
         
            if (Skyray.Language.Lang.Model.LangData.ContainsKey("lblNewPointName.Text") && Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblNewPointName"))
            {
                Skyray.Camera.SkyrayCamera.SkyrayCameraLangDic["lblNewPointName"] = Skyray.Language.Lang.Model.LangData["lblNewPointName.Text"];
            }
           
            if (DifferenceDevice.interClassMain.skyrayCamera != null) DifferenceDevice.interClassMain.skyrayCamera.InitalFrmLanguage();

        }
    }
}
