using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;
using Skyray.Controls;
using Skyray.EDXRFLibrary.Define;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    public partial class FrmChangeSpInfo : Skyray.Language.UCMultiple
    {
        private List<CompanyOthersInfo> listOtherInfo;
        public FrmChangeSpInfo()
        {
            listOtherInfo = CompanyOthersInfo.FindBySql("select * from companyothersinfo where 1=1 and Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "' ");
            InitializeComponent();
        }

        private void FrmChangeSpInfo_Load(object sender, EventArgs e)
        {
            List<Control> listControls = new List<Control>();
            //增加样品名和供应商以及备注
            int i = 0;
            Label labelTemp = new Label();
            labelTemp.Name = "LabelSpName";
            labelTemp.Text = Info.SampleName;
            labelTemp.AutoSize = true;
            labelTemp.BackColor = System.Drawing.Color.Transparent;
            labelTemp.Location = new System.Drawing.Point(7, 8 + i * 26);
            labelTemp.Size = new System.Drawing.Size(47, 18);
            listControls.Add(labelTemp);
            Skyray.Controls.TextBoxW textboxTemp = new Skyray.Controls.TextBoxW();
            textboxTemp.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            textboxTemp.Location = new System.Drawing.Point(108, 3 + i * 26);
            textboxTemp.Name = "textbox_SpName";
            textboxTemp.Size = new System.Drawing.Size(221, 18);
            textboxTemp.Text = DifferenceDevice.interClassMain.specList.SampleName;
            listControls.Add(textboxTemp);
            i++;
            labelTemp = new Label();
            labelTemp.Name = "LabelSupplier";
            labelTemp.Text = Info.Supplier;
            labelTemp.AutoSize = true;
            labelTemp.BackColor = System.Drawing.Color.Transparent;
            labelTemp.Location = new System.Drawing.Point(7, 8 + i * 26);
            labelTemp.Size = new System.Drawing.Size(47, 18);
            listControls.Add(labelTemp);

            System.Windows.Forms.ComboBox cboTemp = new System.Windows.Forms.ComboBox();
            cboTemp.Location = new System.Drawing.Point(108, 3 + i * 26);
            cboTemp.Name = "cbo_Supplier";
            cboTemp.Size = new System.Drawing.Size(221, 18);
            cboTemp.Text = DifferenceDevice.interClassMain.specList.Supplier;
            cboTemp.AutoCompleteMode = AutoCompleteMode.Suggest;
            cboTemp.AutoCompleteSource = AutoCompleteSource.ListItems;
            foreach (Supplier ccd in Supplier.FindAll())
                cboTemp.Items.Add(ccd.Name);
           // cboTemp.DropDownStyle = ComboBoxStyle.Simple;
            listControls.Add(cboTemp);
            i++;

            labelTemp = new Label();
            labelTemp.Name = "LabelShape";
            labelTemp.Text = Info.Shape;
            labelTemp.AutoSize = true;
            labelTemp.BackColor = System.Drawing.Color.Transparent;
            labelTemp.Location = new System.Drawing.Point(7, 8 + i * 26);
            labelTemp.Size = new System.Drawing.Size(47, 18);
            listControls.Add(labelTemp);

            cboTemp = new System.Windows.Forms.ComboBox();
            cboTemp.Location = new System.Drawing.Point(108, 3 + i * 26);
            cboTemp.Name = "cbo_Shape";
            cboTemp.Size = new System.Drawing.Size(221, 18);
            cboTemp.Text = DifferenceDevice.interClassMain.specList.Shape;
            cboTemp.AutoCompleteMode = AutoCompleteMode.Suggest;
            cboTemp.AutoCompleteSource = AutoCompleteSource.ListItems;
            //cboTemp.DropDownStyle = ComboBoxStyle.Simple;
             foreach (Shape shape in Shape.FindAll())
                 cboTemp.Items.Add(shape.Name);

           
            listControls.Add(cboTemp);
            i++;
            labelTemp = new Label();
            labelTemp.Name = "LabelWeight";
            labelTemp.Text = Info.Weight;
            labelTemp.AutoSize = true;
            labelTemp.BackColor = System.Drawing.Color.Transparent;
            labelTemp.Location = new System.Drawing.Point(7, 8 + i * 26);
            labelTemp.Size = new System.Drawing.Size(47, 18);
            listControls.Add(labelTemp);
            textboxTemp = new Skyray.Controls.TextBoxW();
            textboxTemp.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            textboxTemp.Location = new System.Drawing.Point(108, 3 + i * 26);
            textboxTemp.Name = "textbox_Weight";
            textboxTemp.Size = new System.Drawing.Size(221, 18);
            textboxTemp.Text = DifferenceDevice.interClassMain.specList.Weight.ToString();
            listControls.Add(textboxTemp);
            i++;


            foreach (var name in listOtherInfo)
            {
                Label label = new Label();
                label.Name = "Label" + name.Id;
                label.Text = name.Name;
                label.AutoSize = true;
                label.BackColor = System.Drawing.Color.Transparent;
                label.Location = new System.Drawing.Point(7, 8 + i * 26);
                label.Size = new System.Drawing.Size(47, 18);
                listControls.Add(label);
                if (name.ControlType == 1)
                {
                    List<CompanyOthersListInfo> listOtherInfoData = CompanyOthersListInfo.FindBySql("select * from companyothersListinfo where 1=1 and companyothersinfo_id =" + name.Id);
                    Skyray.Controls.ComboBoxW combox = new Skyray.Controls.ComboBoxW();
                    combox.AutoComplete = false;
                    combox.AutoDropdown = false;
                    combox.BackColorEven = System.Drawing.Color.White;
                    combox.BackColorOdd = System.Drawing.Color.White;
                    combox.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
                    combox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
                    combox.ColumnNames = "";
                    combox.ColumnWidthDefault = 75;
                    combox.ColumnWidths = "";
                    combox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
                    combox.FormattingEnabled = true;
                    combox.LinkedTextBox = null;
                    combox.Location = new System.Drawing.Point(108, 3 + i * 26);
                    combox.Name = "comBox_" + name.Id;
                    combox.Size = new System.Drawing.Size(221, 18);
                    foreach (var data in listOtherInfoData)
                    {
                        combox.Items.Add(data.ListInfo);
                    }
                    if (combox.Items.Count > 0)
                    {
                        CompanyOthersListInfo DisplayListInfo = listOtherInfoData.Find(l => l.Display == true);
                        if (DisplayListInfo != null)
                            combox.Text = DisplayListInfo.ListInfo;
                        else combox.SelectedIndex = 0;
                    }
                    BindHelper.BindTextToCtrl(combox, name, "DefaultValue", true);
                    //combox.TextChanged += new EventHandler(combox_Click);
                    listControls.Add(combox);
                }
                else if (name.ControlType == 0)
                {
                    Skyray.Controls.TextBoxW textbox = new Skyray.Controls.TextBoxW();
                    textbox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
                    textbox.Location = new System.Drawing.Point(108, 3 + i * 26);
                    textbox.Name = "textbox_" + name.Id;
                    textbox.Size = new System.Drawing.Size(221, 18);
                    BindHelper.BindTextToCtrl(textbox, name, "DefaultValue", true);
                    listControls.Add(textbox);
                }
                else if (name.ControlType == 2)
                {
                    DateTimePicker dateTimePicker = new DateTimePicker();
                    //dateTimePicker.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
                    dateTimePicker.Location = new System.Drawing.Point(108, 3 + i * 26);
                    dateTimePicker.Name = "dateTimePicker_" + name.Id;
                    dateTimePicker.Size = new System.Drawing.Size(221, 18);
                    dateTimePicker.Format = DateTimePickerFormat.Custom;
                    dateTimePicker.CustomFormat = "yyyy-MM-dd";
                    BindHelper.BindTextToCtrl(dateTimePicker, name, "DefaultValue", true);
                    listControls.Add(dateTimePicker);
                }
                i++;
            }
            labelTemp = new Label();
            labelTemp.Name = "LabelSummary";
            labelTemp.Text = Info.Description;
            labelTemp.AutoSize = true;
            labelTemp.BackColor = System.Drawing.Color.Transparent;
            labelTemp.Location = new System.Drawing.Point(7, 8 + i * 26);
            labelTemp.Size = new System.Drawing.Size(47, 18);
            listControls.Add(labelTemp);
            i++;
            textboxTemp = new Skyray.Controls.TextBoxW();
            textboxTemp.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            textboxTemp.Location = new System.Drawing.Point(7, 3 + i * 26);
            textboxTemp.Multiline = true;
            textboxTemp.Name = "textbox_Summary";
            textboxTemp.Size = new System.Drawing.Size(329, 60);
            textboxTemp.Text = DifferenceDevice.interClassMain.specList.SpecSummary;
            listControls.Add(textboxTemp);
            foreach (var ctrl in listControls)
            {
              panel1.Controls.Add(ctrl);
            }
        }

        private void buttonWSubmit_Click(object sender, EventArgs e)
        {
            string strSpName = panel1.Controls["textbox_SpName"].Text.Trim();
            string constStr = DifferenceDevice.interClassMain.GetDefineSpectrumName(strSpName, DifferenceDevice.interClassMain.testDevicePassedParams.IsRuleName);
            int specType = 0;
            bool exist = WorkCurveHelper.DataAccess.ExistsRecord(constStr, out specType);
            if (exist)
            {
                if (specType != (int)SpecType.UnKownSpec)
                {
                    Msg.Show(Info.UnKownSpecOtherSpecSameName);
                    return ;
                }
                var result = WorkCurveHelper.DirectRun.IsDirectRun ? DialogResult.Yes : Msg.Show(Info.strCoverSpecName, Info.Suggestion, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    DifferenceDevice.interClassMain.IsIncrement = true;
                }
                else
                {
                    return;
                }
            }
            foreach (Control ctrl in panel1.Controls)
            {
                if(ctrl  is Label) continue;
                else 
                {
                    string strctrlName = ctrl.Name;
                    int indexS = strctrlName.IndexOf('_');
                    if (indexS <= 0) continue;
                    string porpertyName = strctrlName.Substring(indexS+1);
                    switch (porpertyName)
                    {
                        case "SpName":
                            
                            DifferenceDevice.interClassMain.specList.SampleName = ctrl.Text;
                            break;
                        case "Supplier":
                            DifferenceDevice.interClassMain.specList.Supplier = ctrl.Text;
                            break;
                        case "Shape":
                            DifferenceDevice.interClassMain.specList.Shape = ctrl.Text;
                            break;
                        case "Summary":
                            DifferenceDevice.interClassMain.specList.SpecSummary = ctrl.Text;
                            break;
                        case "Weight":
                            double d=0;
                            try {
                                d=double.Parse(ctrl.Text);
                            }
                            catch{}
                            DifferenceDevice.interClassMain.specList.Weight =d ;
                            break;
                        default:
                            int coiId=int.Parse(porpertyName);
                            foreach (CompanyOthersInfo coi in listOtherInfo)
                            {
                                if (coi.Id == coiId)
                                {
                                    coi.DefaultValue = ctrl.Text;
                                    coi.Save();
                                    if (WorkCurveHelper.SeleCompanyOthersInfo.ContainsKey(coiId.ToString()))
                                    {
                                        WorkCurveHelper.SeleCompanyOthersInfo[coiId.ToString()] = ctrl.Text;
                                    }
                                }
                            }

                            break;
                    }

                }

            }
            DifferenceDevice.interClassMain.refreshFillinof.RefreshSpec(DifferenceDevice.interClassMain.specList, DifferenceDevice.interClassMain.spec);
            if (DifferenceDevice.interClassMain.testDevicePassedParams != null && DifferenceDevice.interClassMain.testDevicePassedParams.Spec != null)
            {
                DifferenceDevice.interClassMain.testDevicePassedParams.Spec.SampleName = DifferenceDevice.interClassMain.specList.SampleName;
                DifferenceDevice.interClassMain.testDevicePassedParams.Spec.SampleName = DifferenceDevice.interClassMain.specList.SampleName;
                DifferenceDevice.interClassMain.testDevicePassedParams.Spec.SpecSummary = DifferenceDevice.interClassMain.specList.SpecSummary;
                DifferenceDevice.interClassMain.testDevicePassedParams.Spec.Shape = DifferenceDevice.interClassMain.specList.Shape;
                DifferenceDevice.interClassMain.testDevicePassedParams.Spec.Supplier = DifferenceDevice.interClassMain.specList.Supplier;
                DifferenceDevice.interClassMain.testDevicePassedParams.Spec.Weight = (double)DifferenceDevice.interClassMain.specList.Weight;
            }
            if (DifferenceDevice.interClassMain.CurveTest.sampleInfo != null)
            {
                DifferenceDevice.interClassMain.CurveTest.sampleInfo.SampleName = DifferenceDevice.interClassMain.specList.SampleName;
                DifferenceDevice.interClassMain.CurveTest.sampleInfo.SpecSummary = DifferenceDevice.interClassMain.specList.SpecSummary;
                DifferenceDevice.interClassMain.CurveTest.sampleInfo.Shape = DifferenceDevice.interClassMain.specList.Shape;
                DifferenceDevice.interClassMain.CurveTest.sampleInfo.Supplier = DifferenceDevice.interClassMain.specList.Supplier;
                DifferenceDevice.interClassMain.CurveTest.sampleInfo.Weight = (double)DifferenceDevice.interClassMain.specList.Weight;

            }
            if (DifferenceDevice.interClassMain.CurveTest.Spec != null)
            {
                DifferenceDevice.interClassMain.CurveTest.Spec.SampleName = DifferenceDevice.interClassMain.specList.SampleName;
                DifferenceDevice.interClassMain.CurveTest.Spec.SpecSummary = DifferenceDevice.interClassMain.specList.SpecSummary;
                DifferenceDevice.interClassMain.CurveTest.Spec.Shape = DifferenceDevice.interClassMain.specList.Shape;
                DifferenceDevice.interClassMain.CurveTest.Spec.Supplier = DifferenceDevice.interClassMain.specList.Supplier;
                DifferenceDevice.interClassMain.CurveTest.Spec.Weight = (double)DifferenceDevice.interClassMain.specList.Weight;
            }
            //foreach (WordCureTest wd in DifferenceDevice.interClassMain.testDevicePassedParams.WordCureTestList)
            //{
            //    if (DifferenceDevice.interClassMain.CurveTest.sampleInfo == null || wd.SerialNumber != DifferenceDevice.interClassMain.CurveTest.SerialNumber) continue;
            //    wd.sampleInfo.SampleName = DifferenceDevice.interClassMain.specList.SampleName;
            //    wd.sampleInfo.SpecSummary = DifferenceDevice.interClassMain.specList.SpecSummary;
            //    wd.sampleInfo.Shape = DifferenceDevice.interClassMain.specList.Shape;
            //    wd.sampleInfo.Supplier = DifferenceDevice.interClassMain.specList.Supplier;
            //    wd.sampleInfo.Weight = (double)DifferenceDevice.interClassMain.specList.Weight;
                
            //}


                            
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }



    }
}
