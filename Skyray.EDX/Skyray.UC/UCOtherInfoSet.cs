using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary.Define;
using Lephone.Data.Common;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common.ReportHelper;

namespace Skyray.UC
{
    public partial class UCOtherInfoSet : Skyray.Language.UCMultiple
    {
        private List<CompanyOthersInfo> listOtherInfo;
        private Rectangle defaultControlRect = new Rectangle(108, 0, 121, 18);
        private int startX = 7;
        private int startY = 40;
        private int verticalDistance = 25;

        public UCOtherInfoSet()
        {
            listOtherInfo = CompanyOthersInfo.FindBySql("select * from companyothersinfo where 1=1 and Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "' ");
            InitializeComponent();          
        }

        public UCOtherInfoSet(List<CompanyOthersInfo> listOtherInfo)
        {
            this.listOtherInfo = listOtherInfo;
            InitializeComponent();
        }

        public UCOtherInfoSet(Rectangle rect, int startx, int starty, int distance):this()
        {
            lab_InfoSele.Visible = false;
            buttonWOK.Visible = false;
            btnClose.Visible = false;
            panel1.Dock = DockStyle.Fill;
            panel1.BorderStyle = BorderStyle.None;
            defaultControlRect = rect;
            startX = startx;
            startY = starty;
            verticalDistance = distance;
        }

        private void UCOtherInfoSet_Load(object sender, EventArgs e)
        {
            List<Control> listControls = new List<Control>();
            int i = 0;
            foreach (var name in listOtherInfo)
            {
                Label label = new Label();
                label.Name = "Label" + name.Id;
                label.Text = name.Name;
                label.AutoSize = true;
                label.BackColor = System.Drawing.Color.Transparent;
                label.Location = new System.Drawing.Point(startX, startY + 5 + i * verticalDistance);
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
                    combox.Location = new System.Drawing.Point(defaultControlRect.X, startY + i * verticalDistance);
                    combox.Name = "comBox_" + name.Id;
                    combox.Size = defaultControlRect.Size;
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
                    textbox.Location = new System.Drawing.Point(defaultControlRect.X, startY + i * verticalDistance);
                    textbox.Name = "textbox_" + name.Id;
                    textbox.Size = defaultControlRect.Size;
                    BindHelper.BindTextToCtrl(textbox, name, "DefaultValue", true);
                    listControls.Add(textbox);
                }
                else if (name.ControlType == 2)
                {
                    DateTimePicker dateTimePicker = new DateTimePicker();
                    //dateTimePicker.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
                    dateTimePicker.Location = new System.Drawing.Point(defaultControlRect.X, startY + i * verticalDistance);
                    dateTimePicker.Name = "dateTimePicker_" + name.Id;
                    dateTimePicker.Size = defaultControlRect.Size;
                    dateTimePicker.Format = DateTimePickerFormat.Custom;
                    dateTimePicker.CustomFormat = "yyyy-MM-dd";
                    BindHelper.BindTextToCtrl(dateTimePicker, name, "DefaultValue", true);
                    listControls.Add(dateTimePicker);
                }
                i++;
            }
            foreach (var ctrl in listControls)
            {
                panel1.Controls.Add(ctrl);
            }
        }

        private void combox_Click(object sender, EventArgs e)
        {
            string ListInfo=((Skyray.Controls.ComboBoxW)sender).Text;
            string CompanyOthersInfo_Id = (((Skyray.Controls.ComboBoxW)sender).Name.IndexOf("_")!=-1)?((Skyray.Controls.ComboBoxW)sender).Name.Split('_')[1]:"";
            if (CompanyOthersInfo_Id == "") return;
            Lephone.Data.DbEntry.Context.ExecuteNonQuery("update  companyothersListinfo set display=0 where companyothersinfo_id='" + CompanyOthersInfo_Id + "'; update  companyothersListinfo set display=1 where companyothersinfo_id='" + CompanyOthersInfo_Id + "' and listinfo='" + ListInfo + "'");
        }

        
        private void GetSetCompanyOthersInfo()
        {
            WorkCurveHelper.SeleCompanyOthersInfo.Clear();
            foreach (Control c in panel1.Controls)
            {
                if (c is Skyray.Controls.ComboBoxW || c is Skyray.Controls.TextBoxW || c is DateTimePicker)
                {
                    if (c.Name.IndexOf("_") != -1)
                    {
                        WorkCurveHelper.SeleCompanyOthersInfo.Add(c.Name.Split('_')[1], c.Text);
                    }
                }
            } 
 
        }

        private void buttonWOK_Click(object sender, EventArgs e)
        {
            SaveOtherInfo();
            EDXRFHelper.GotoMainPage(this);
        }

        public void SaveOtherInfo()
        {
            GetSetCompanyOthersInfo();
            foreach (var name in listOtherInfo)
            {
                name.Save();
            }
        }

        public List<CompanyOthersInfo> GetOtherInfoList()
        {
            return this.listOtherInfo;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//转到主界面
        }
    }
}
