using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Skyray.EDX.Common;
using Skyray.Language;
using Skyray.EDX.Common.ReportHelper;
using Skyray.EDXRFLibrary.Define;

namespace Skyray.UC
{
    public partial class UCNameSetting : Skyray.Language.UCMultiple
    {
        private int SelectRowIndex_DGV;//标记DatagridView的选中行
        Dictionary<string, string> boolTostring;
        Dictionary<string, string> stringTobool;
        Dictionary<string, string> EditAfterOrderStr;//保存需要修改名字与对应的排序字符串

        public UCNameSetting()
        {
            InitializeComponent();
            SelectRowIndex_DGV = 0;
            InitCommonNameFields();            
            stringTobool = new Dictionary<string, string>();
            ReverseDictionary();
            EditAfterOrderStr = new Dictionary<string, string>();
        }

        private void ReverseDictionary()
        {
            stringTobool.Clear();
            foreach (var v in boolTostring)
            {
                if(!stringTobool.Keys.Contains(v.Value))
                    stringTobool.Add(v.Value, v.Key);
            }
        }

        private void InitCommonNameFields()
        {
            boolTostring = new Dictionary<string, string>();//直接从listBoxs里面读
            boolTostring.Add("bSpecListName", Info.SampleName);
            boolTostring.Add("bSpecType", Info.SpecType);
            boolTostring.Add("bInitConditionName", Info.Initialization + Info.Condition);
            boolTostring.Add("bCurrentWorkCurve", Info.CurveName);
            boolTostring.Add("bOptMode", Info.OptModeStr);
            boolTostring.Add("bTestDate", Info.SpecDate);
            boolTostring.Add("bTestTime", Info.MeasureTime);
            boolTostring.Add("bWorks", Info.Workgion);
            boolTostring.Add("bCollimator", Info.Collimator);
        }

        private string[] GetItems(string ItemsStr)
        {
            if (ItemsStr == String.Empty)
            {
                return null;
            }
            return ItemsStr.Split(("_").ToCharArray());
        }

        private void SaveNameSetting(string path)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlNode xmlNames = xmlDoc.SelectSingleNode("application/NameSetting");
                XmlNode xmlName = xmlNames.FirstChild;
                for (int i = 0; i < DGV_NameList.Rows.Count; i++)
                {
                    xmlName.InnerText = EditAfterOrderStr[xmlName.Attributes["Flag"].Value];
                    xmlName = xmlName.NextSibling;
                }
                xmlDoc.Save(path);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ExampleShow()
        {
            TBW_ShowText.Text = "";
            for (int i = 0; i < LB_ShowOrder.Items.Count; i++)
            {
                TBW_ShowText.Text += LB_ShowOrder.Items[i];
                if (i != LB_ShowOrder.Items.Count - 1) TBW_ShowText.Text += "_";
            }
        }

        private void UpBtn_Click(object sender, EventArgs e)
        {
            if (LB_ShowOrder.SelectedIndex >= 0)
            {
                if (LB_ShowOrder.SelectedIndex == 0)
                {
                    return;
                }
                else
                {
                    string temp = LB_ShowOrder.Items[LB_ShowOrder.SelectedIndex - 1].ToString();
                    LB_ShowOrder.Items[LB_ShowOrder.SelectedIndex - 1] = LB_ShowOrder.Items[LB_ShowOrder.SelectedIndex];
                    LB_ShowOrder.Items[LB_ShowOrder.SelectedIndex] = temp;
                    LB_ShowOrder.SelectedIndex--;
                }
            }
            ExampleShow();
            ApplyBtn.Enabled = true;
        }

        private void DownBtn_Click(object sender, EventArgs e)
        {
            if (LB_ShowOrder.SelectedIndex >= 0)
            {
                if (LB_ShowOrder.SelectedIndex == LB_ShowOrder.Items.Count - 1)
                {
                    return;
                }
                else
                {
                    string temp = LB_ShowOrder.Items[LB_ShowOrder.SelectedIndex + 1].ToString();
                    LB_ShowOrder.Items[LB_ShowOrder.SelectedIndex + 1] = LB_ShowOrder.Items[LB_ShowOrder.SelectedIndex];
                    LB_ShowOrder.Items[LB_ShowOrder.SelectedIndex] = temp;
                    LB_ShowOrder.SelectedIndex++;
                }
            }
            ExampleShow();
            ApplyBtn.Enabled = true;
        }

        private void UCNameSetting_Load(object sender, EventArgs e)
        {
            InitCBL_AllItems();
            //加载xml文件，读取排序顺序
            string otherpath = Application.StartupPath + "\\AppParams.xml";
            XElement xele = XElement.Load(otherpath);
            if (xele == null) return;
            var names = xele.Elements("NameSetting").Elements("Name").ToList();
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i].Attribute(XName.Get("Flag")).Value.ToString() == "SampleName")
                {
                    DGV_NameList.Rows.Add(Info.SampleName);
                }
                else if (names[i].Attribute(XName.Get("Flag")).Value.ToString() == "SpectrumName")
                {
                    DGV_NameList.Rows.Add(Info.SpecName);
                }
                else if (names[i].Attribute(XName.Get("Flag")).Value.ToString()=="ReportName")
                {
                    DGV_NameList.Rows.Add(Info.ReportName);
                }
                //DGV_NameList.Rows.Add(names[i].Attribute(XName.Get("Flag")).Value.ToString());
                EditAfterOrderStr.Add(names[i].Attribute(XName.Get("Flag")).Value.ToString(), names[i].Value);
                //得到排序的项
                string[] orderItems = GetItems(names[i].Value);
                if (orderItems == null) continue;
                foreach (var v in orderItems)
                {
                    for (int j = 0; j < CBL_AllItems.Items.Count; j++)
                    {
                        if (stringTobool[CBL_AllItems.Items[j].ToString()] == v)
                        {
                            if (SelectRowIndex_DGV == i)
                            {
                                LB_ShowOrder.Items.Add(boolTostring[v]);
                                CBL_AllItems.SetItemChecked(j, true);
                            }
                            break;
                        }
                    }
                }
            }
            ApplyBtn.Enabled = false;
        }

        private void InitCBL_AllItems()
        {
            CBL_AllItems.Items.Clear();
            //首先加载所有可能的组件到ListBox中去
            for (int i = 0; i < boolTostring.Count(); i++)
            {
                CBL_AllItems.Items.Add(boolTostring.ElementAt(i).Value);
                CBL_AllItems.SetItemChecked(i, false);
            }
            CBL_AllItems.CheckOnClick = true;
        }

        private void DGV_NameList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                string orderStr = string.Empty;
                if (DGV_NameList.Rows[e.RowIndex].Cells[0].Value.ToString() == Info.SampleName)
                {
                    InitCommonNameFields();
                    orderStr = EditAfterOrderStr["SampleName"];                    
                }
                else if (DGV_NameList.Rows[e.RowIndex].Cells[0].Value.ToString() == Info.SpecName)
                {
                    InitCommonNameFields();
                    orderStr = EditAfterOrderStr["SpectrumName"];                    
                }
                else if (DGV_NameList.Rows[e.RowIndex].Cells[0].Value.ToString()==Info.ReportName)
                {
                    //UnionDictionaryStringString(boolTostring, GetHistoryRecordItems(), GetCompanyOthersInfo());
                    boolTostring.AddRangs(GetHistoryRecordItems(), GetCompanyOthersInfo());
                    orderStr = EditAfterOrderStr["ReportName"];                    
                }
                InitCBL_AllItems();
                ReverseDictionary();
                string[] orderItems = GetItems(orderStr);
                LB_ShowOrder.Items.Clear();
                for (int i = 0; i < CBL_AllItems.Items.Count; i++ )
                {
                    CBL_AllItems.SetItemChecked(i, false);
                }
                if (orderItems != null)
                {
                    foreach (var v in orderItems)
                    {
                        for (int j = 0; j < CBL_AllItems.Items.Count; j++)
                        {
                            //CBL_AllItems.SetItemChecked(j, false);
                            if (stringTobool[CBL_AllItems.Items[j].ToString()] == v)
                            {
                                LB_ShowOrder.Items.Add(boolTostring[v]);
                                CBL_AllItems.SetItemChecked(j, true);
                                break;
                            }
                        }
                    }
                }
                SelectRowIndex_DGV = e.RowIndex;
                ApplyBtn.Enabled = false;
            }
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            ReverseDictionary();
            string filename = Application.StartupPath + "\\AppParams.xml";
            if (LB_ShowOrder.Items.Count <= 0)
            {
                Msg.Show(Info.NameIsNull);
                return;
            }
            for (int i = 0; i < DGV_NameList.Rows.Count; i++)
            {
                string Innertext = "";
                for (int j = 0; j < LB_ShowOrder.Items.Count; j++)
                {
                    Innertext += stringTobool[LB_ShowOrder.Items[j].ToString()];
                    if (j != LB_ShowOrder.Items.Count - 1)
                    {
                        Innertext += "_";
                    }
                }
                if (DGV_NameList.Rows[SelectRowIndex_DGV].Cells[0].Value.ToString() == Info.SampleName)
                {
                    EditAfterOrderStr["SampleName"] = Innertext;
                }
                else if (DGV_NameList.Rows[SelectRowIndex_DGV].Cells[0].Value.ToString() == Info.SpecName)
                {
                    EditAfterOrderStr["SpectrumName"] = Innertext;
                }
                else if (DGV_NameList.Rows[SelectRowIndex_DGV].Cells[0].Value.ToString() == Info.ReportName)
                {
                    EditAfterOrderStr["ReportName"] = Innertext;
                }
            }
            SaveNameSetting(filename);
            ApplyBtn.Enabled = false;
        }

        private void CBL_AllItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //选中
            if (e.CurrentValue == CheckState.Unchecked)
            {
                //判断是否以加载
                if (!LB_ShowOrder.Items.Contains(CBL_AllItems.Items[e.Index].ToString()))
                {
                    LB_ShowOrder.Items.Add(CBL_AllItems.Items[e.Index].ToString());
                }

            }
            else //未选中
            {
                for (int i = 0; i < LB_ShowOrder.Items.Count; i++)
                {
                    if (LB_ShowOrder.Items.Contains(CBL_AllItems.Items[e.Index].ToString()))
                    {
                        LB_ShowOrder.Items.Remove(CBL_AllItems.Items[e.Index].ToString());
                    }
                }
            }
            ExampleShow();
            ApplyBtn.Enabled = true;
        }

        private Dictionary<string,string> GetHistoryRecordItems()
        {
            Dictionary<string, string> dicHisRecord = new Dictionary<string,string>();
            Array.ForEach<System.Reflection.PropertyInfo>(typeof(Skyray.EDXRFLibrary.HistoryRecord).GetProperties(), p =>
                {
                    Skyray.EDXRFLibrary.Auto auto = p.GetCustomAttributes(typeof(Skyray.EDXRFLibrary.Auto), true).FirstOrDefault() as Skyray.EDXRFLibrary.Auto;
                    if (auto != null )
                    {
                        string sType = "Skyray.EDX.Common.Info,Skyray.EDX.Common";
                        Type type = Type.GetType(sType);
                        System.Reflection.FieldInfo fi = type.GetField(auto.Text.Substring(auto.Text.LastIndexOf('.')+1));
                        if (fi != null) auto.Text = fi.GetValue(null)==null?auto.Text:fi.GetValue(null).ToString();//反射翻译 
                        if(!dicHisRecord.Values.Contains(auto.Text) && !dicHisRecord.Keys.Contains(p.Name))
                            dicHisRecord.Add(p.Name, auto.Text);
                    }
                });
            return dicHisRecord;
        }

        private Dictionary<string, string> GetCompanyOthersInfo()
        {
            Dictionary<string, string> dicCompanyOthers = new Dictionary<string, string>();
            XmlNodeList xmlNodeList = ExcelTemplateParams.GetCompanyOtherInfoTitles();
            List<CompanyOthersInfo> companyOthersInfo = CompanyOthersInfo.Find(w => w.Display == true && w.ExcelModeType == ReportTemplateHelper.ExcelModeType.ToString()).ToList();
            Dictionary<string, string> dicDBCompanyOthers = new Dictionary<string, string>();
            string translation = string.Empty;
            foreach (var com in companyOthersInfo)
            {
                translation= com.Name;//获取公司其他信息名称
                foreach(XmlNode list in xmlNodeList)
                {
                    foreach(XmlNode child in list.ChildNodes)
                    {
                        foreach(XmlAttribute attribute in child.Attributes)
                        {
                            if (attribute.Value == com.Name && child.Attributes[WorkCurveHelper.LanguageShortName]!=null)//如果有一种语言相符 并且有需求语言
                                translation = string.IsNullOrEmpty(child.Attributes[WorkCurveHelper.LanguageShortName].Value) ? translation :
                                    child.Attributes[WorkCurveHelper.LanguageShortName].Value;//从配置文件中获取翻译内容
                        }
                    }
                }
                dicDBCompanyOthers.Add(com.Name, translation);
            }
            return dicDBCompanyOthers;
        }

        //private Dictionary<string, string> UnionDictionaryStringString(Dictionary<string, string> dicBase, params Dictionary<string, string>[] dicOthers)
        //{
        //    Dictionary<string, string> dicBack = new Dictionary<string, string>();
        //    if(dicBase!=null)
        //        dicBack = dicBase;
        //    foreach (Dictionary<string, string> dicOther in dicOthers)
        //    {
        //        if (dicOther == null) continue;
        //        foreach (var key in dicOther.Keys)
        //        {
        //            //if (!dicBack.Contains(new KeyValuePair<string, string>(key, dicOther[key])))
        //            if(!dicBack.Keys.Contains(key)&&!dicBack.Values.Contains(dicOther[key]))
        //                dicBack.Add(key, dicOther[key]);
        //        }
        //    }
        //    return dicBack;
        //}

    }
}
