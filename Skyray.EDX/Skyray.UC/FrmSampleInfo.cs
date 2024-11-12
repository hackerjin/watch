using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Skyray.EDX.Common;
using Skyray.EDXRFLibrary.Define;
using Skyray.Language;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    /// <summary>
    /// 样品信息类
    /// </summary>
    public partial class FrmSampleInfo : Skyray.Language.MultipleForm
    {
        /// <summary>
        /// 样品信息
        /// </summary>
        public SampleInfo SampleInfo { set; get; }

        public List<CompanyOthersInfo> CompanyInfoList = null;

        public SpecType SpecType;

        private string _SampleName;

        public string SampleName
        {
            get { return _SampleName; }
            set {
                if (!value.IsNullOrEmpty())
                {
                    _SampleName = value;
                    if (SpecType == SpecType.PureSpec)
                        this.comboBoxElementNameChild.Text = value;
                    else
                    this.txtSampleName.Text = value;
                }
            }
        }
        //形状
        private string _SampleShape;

        public string SampleShape
        {
            get { return _SampleShape; }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    _SampleShape = value;
                    this.comboBoxWShape.Text = value;
                }
            }
        }

        //供应商
        private string _SampleSupplier;

        public string SampleSupplier
        {
            get { return _SampleSupplier; }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    _SampleSupplier = value;
                    this.comboBoxWSupplier.Text = value;
                }
            }
        }

        //重量
        private string _Weight;

        public string Weight
        {
            get { return _Weight; }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    _Weight = value;
                    this.txtWeight.Text = value;
                }
            }
        }

        //烧失量
        private string _Loss;

        public string Loss
        {
            get { return _Loss; }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    _Loss = value;
                    this.txtLoss.Text = value;
                }
            }
        }

        //描述
        private string _SpecSummary;

        public string SpecSummary
        {
            get { return _SpecSummary; }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    _SpecSummary = value;
                    this.txtDescription.Text = value;
                }
            }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmSampleInfo(SpecType type)
        {
            InitializeComponent();
            this.SpecType = type;
            if (SpecType == SpecType.PureSpec)
            {
                this.txtSampleName.Visible = false;
                this.comboBoxElementNameChild.Visible = true;
                this.comboBoxElementNameChild.Items.Clear();
                var listElementName = from item in Atoms.AtomList select item.AtomName;
                this.comboBoxElementNameChild.DataSource = listElementName.ToList();
                this.comboBoxElementNameChild.AutoCompleteCustomSource.AddRange(listElementName.ToArray());
                this.comboBoxElementNameChild.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.comboBoxElementNameChild.AutoCompleteSource = AutoCompleteSource.ListItems;
                if (this.comboBoxElementNameChild.Items.Count > 0)
                    this.comboBoxElementNameChild.SelectedIndex = 0;
            }
            else
            {
                this.txtSampleName.Visible = true;
                this.comboBoxElementNameChild.Visible = false;
            }
            foreach (Shape shape in Shape.FindAll())
                this.comboBoxWShape.Items.Add(shape.Name);
            foreach (Supplier ccd in Supplier.FindAll())
                this.comboBoxWSupplier.Items.Add(ccd.Name);

            if (DifferenceDevice.IsXRF)
            {
                pnlLoss.Visible = true;
                //判断编辑元素中是否进行归一，如果不归一则，烧失量变灰。
                if (WorkCurveHelper.WorkCurveCurrent.ElementList!=null && !WorkCurveHelper.WorkCurveCurrent.ElementList.IsUnitary) this.txtLoss.Enabled = false;
            }
            else pnlLoss.Visible = false;
           

        }

        /// <summary>
        /// 确定按钮处理，验证所填的字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWOk_Click(object sender, EventArgs e)
        {
            string sampleName = string.Empty;
            if (this.SpecType != SpecType.PureSpec && this.txtSampleName.Text != null)
                sampleName = this.txtSampleName.Text.ToString();
            else
                sampleName = this.comboBoxElementNameChild.Text;

            string shape = string.Empty;
            if (this.comboBoxWShape.SelectedItem != null)
                shape = this.comboBoxWShape.SelectedItem.ToString();

            string supplier = string.Empty;
            if (this.comboBoxWSupplier.SelectedItem != null)
                supplier = this.comboBoxWSupplier.SelectedItem.ToString();
            string s = this.comboBoxWSupplier.Text.ToString();

            double weight = 0.00;
            if (this.txtWeight.Text != null && this.txtWeight.Text != "")
                weight = double.Parse(this.txtWeight.Text.ToString());

            string specSummary = string.Empty;
            if (this.txtDescription.Text != null)
                specSummary = this.txtDescription.Text.ToString();


            double loss = 0.00;
            if (DifferenceDevice.IsXRF && double.Parse((txtLoss.Text == "") ? "0" : txtLoss.Text) > 100)
            {
                Msg.Show(Info.strErrorLoss);
                return;
            }
            if (this.txtLoss.Text != null && this.txtLoss.Text != "")
                loss = double.Parse(this.txtLoss.Text.ToString());

            if (this.SpecType != SpecType.PureSpec && txtSampleName.Text.IsNullOrEmpty())
            {
                Msg.Show(Info.TestSampleNameValidate, Info.SelectSampleType, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtSampleName.Focus();
                return;
            }
            else if (this.SpecType == SpecType.PureSpec && this.comboBoxElementNameChild.Visible)
            {
                var list = from atom in Atoms.AtomList where String.Compare(this.comboBoxElementNameChild.Text, atom.AtomName, true) == 0 select atom;
                if (list.Count() == 0)
                {
                    Msg.Show(Info.SelectElementNotTable);
                    if (this.comboBoxElementNameChild.Items.Count > 0)
                    {
                        this.comboBoxElementNameChild.SelectedIndex = 0;
                        this.comboBoxElementNameChild.Text = this.comboBoxElementNameChild.Text;
                    }
                    return;
                }
                else
                {
                    string txtStr = this.comboBoxElementNameChild.Text;
                    if (txtStr.Length > 0 && !Char.IsUpper(txtStr, 0))
                    {
                        txtStr = txtStr.Substring(0, 1).ToUpper() + txtStr.Substring(1);

                    }
                    if (txtStr.Length > 0)
                    {
                        this.comboBoxElementNameChild.Text = txtStr;
                        this.comboBoxElementNameChild.Text = this.comboBoxElementNameChild.Text;
                    }
                }
            }
            try
            {
                if (this.txtWeight.Text.IsNullOrEmpty())
                    weight = 0.00;
                else
                    weight = double.Parse(this.txtWeight.Text);
            }
            catch
            {
                Msg.Show(Info.PleaseCorrectWeight);
                this.txtWeight.Focus();
                return;
            }

            this.SampleInfo = new SampleInfo(sampleName, shape, supplier, weight, specSummary, loss);
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消按钮处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > (char)47 && e.KeyChar < (char)58 || e.KeyChar == (char)8 || e.KeyChar == (char)46 || e.KeyChar == (char)3 || e.KeyChar == (char)22)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtLoss_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > (char)47 && e.KeyChar < (char)58 || e.KeyChar == (char)8 || e.KeyChar == (char)46 || e.KeyChar == (char)3 || e.KeyChar == (char)22)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }


        private void LayoutControls()
        {
            if (CompanyInfoList == null || CompanyInfoList.Count <= 0) return;
            int intBeginX = 21;
            int intBeginY = 70;
            List<Control> listControls = new List<Control>();
            int i = 0;
            foreach (var name in CompanyInfoList)
            {
                Label label = new Label();
                label.Name = "Label" + name.Id;
                label.Text = name.Name;
                label.AutoSize = true;
                label.BackColor = System.Drawing.Color.Transparent;
                label.Location = new System.Drawing.Point(intBeginX + (i % 2) * 244, intBeginY + (i/2) * 28);
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
                    combox.Location = new System.Drawing.Point(intBeginX + (i % 2) * 244 + 56, intBeginY + (i / 2) * 28);
                    combox.Name = "comBox_" + name.Id;
                    combox.Size = new System.Drawing.Size(121, 18);
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
                    combox.TextChanged += new EventHandler(combox_Click);
                    listControls.Add(combox);
                }
                else if (name.ControlType == 0)
                {
                    Skyray.Controls.TextBoxW textbox = new Skyray.Controls.TextBoxW();
                    textbox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
                    textbox.Location = new System.Drawing.Point(intBeginX + (i % 2) * 244 + 56, intBeginY + (i/2) * 28);
                    textbox.Name = "textbox_" + name.Id;
                    textbox.Size = new System.Drawing.Size(121, 18);
                    BindHelper.BindTextToCtrl(textbox, name, "DefaultValue", true);
                    listControls.Add(textbox);
                }
                else if (name.ControlType == 2)
                {
                    DateTimePicker dateTimePicker = new DateTimePicker();
                    dateTimePicker.Location = new System.Drawing.Point(intBeginX + (i % 2) * 244 + 56, intBeginY + (i / 2) * 28);
                    dateTimePicker.Name = "dateTimePicker_" + name.Id;
                    dateTimePicker.Size = new System.Drawing.Size(121, 18);
                    dateTimePicker.Format = DateTimePickerFormat.Custom;
                    dateTimePicker.CustomFormat = "yyyy-MM-dd";
                    BindHelper.BindTextToCtrl(dateTimePicker, name, "DefaultValue", true);
                    listControls.Add(dateTimePicker);
                }
                i++;
            }
            foreach (var ctrl in listControls)
            {
                panel3.Controls.Add(ctrl);
            }

        }


        private void combox_Click(object sender, EventArgs e)
        {
            if (CompanyInfoList == null || CompanyInfoList.Count <= 0) return;
            string ListInfo = ((Skyray.Controls.ComboBoxW)sender).Text;
            string CompanyOthersInfo_Id = (((Skyray.Controls.ComboBoxW)sender).Name.IndexOf("_") != -1) ? ((Skyray.Controls.ComboBoxW)sender).Name.Split('_')[1] : "";
            if (CompanyOthersInfo_Id == "") return;
            foreach (var name in CompanyInfoList)
            {
                if (name.Id.ToString() == CompanyOthersInfo_Id)
                {
                    name.DefaultValue = ListInfo;
                    break;
                }
            }
        }

        private void FrmSampleInfo_Load(object sender, EventArgs e)
        {
            LayoutControls();
        }
    }

}
