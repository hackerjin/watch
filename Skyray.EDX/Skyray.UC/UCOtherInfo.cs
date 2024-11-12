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
using System.Xml;
using System.IO;
using Skyray.EDX.Common.ReportHelper;
using Skyray.Language;

namespace Skyray.UC
{
    public partial class UCOtherInfo : Skyray.Language.UCMultiple
    {
        Lephone.Data.Common.DbObjectList<CompanyOthersInfo> companyOthersInfo_List;
        Lephone.Data.Common.DbObjectList<CompanyOthersInfo> companyOthersInfo_List_Del;

        public UCOtherInfo()
        {
            InitializeComponent();
        }

        private void UCOtherInfo_Load(object sender, EventArgs e)
        {
            this.comRoportControlType.Items.Clear();
            this.comRoportControlType.Items.AddRange(new object[] { Info.strTextBox, Info.strComboBox, Info.strDateTimePicker });
            this.comControlType.Items.Clear();
            this.comControlType.Items.AddRange(new object[] { Info.strTextBox, Info.strComboBox, Info.strDateTimePicker});



            comControlType.SelectedIndex = 0;
            comRoportControlType.SelectedIndex = 0;
            this.dgvwCompanyOtherInfo.Rows.Clear();
            companyOthersInfo_List_Del = new DbObjectList<CompanyOthersInfo>();
            companyOthersInfo_List = CompanyOthersInfo.FindBySql("select * from companyothersinfo where ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "'");//查询所有公司其它信息
            //companyOthersInfo_List = CompanyOthersInfo.FindBySql("select * from companyothersinfo where ExcelModeType='" + ExcelTemplateParams.iTemplateType.ToString() + "' and WorkCurveId='" + WorkCurveHelper.WorkCurveCurrent.Id.ToString() + "'");//查询所有公司其它信息
            GetReportInfo();
            InstanceOtherInfo();
            InstanceOtherListInfo();

            
        }

        #region 获取报表相关需要信息
        private void butAddReportName_Click(object sender, EventArgs e)
        {
            if (combReportInfo.Items.Count == 0) return;

            string sTarget = string.Empty;
            GetTarget(combReportInfo.Text, ref sTarget);            

            if (companyOthersInfo_List.Find(l => l.Name == combReportInfo.Text) == null)
            {
                int comControlType = 0;
                if (comRoportControlType.Text == Info.strTextBox) comControlType = 0;
                else if (comRoportControlType.Text == Info.strComboBox) comControlType = 1;
                else if (comRoportControlType.Text == Info.strDateTimePicker) comControlType = 2;
                var v = CompanyOthersInfo.New.Init(combReportInfo.Text, false, true, comControlType, ReportTemplateHelper.ExcelModeType.ToString(), sTarget, "");
                //var v = CompanyOthersInfo.New.Init(combReportInfo.Text, false, WorkCurveHelper.WorkCurveCurrent.Id, true, comControlType, ExcelTemplateParams.iTemplateType.ToString(), "");
                companyOthersInfo_List.Add(v);
                InstanceOtherInfo();
            }
        }

        private XmlNodeList xmlnodelist;
        private void GetReportInfo()
        {
            string sReportPath = AppDomain.CurrentDomain.BaseDirectory + "//printxml//CompanyInfo.xml";
            XmlDocument xmlDocReport = new XmlDocument();
            xmlDocReport.Load(sReportPath);

            string strWhere="";
            if (ReportTemplateHelper.ExcelModeType != 2)
                strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.ExcelModeType + "']";
            else
                strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.LoadReportName() + "']";

            
            xmlnodelist = xmlDocReport.SelectNodes(strWhere);
            if (xmlnodelist == null || xmlnodelist.Count == 0) return;
            foreach (XmlNode xmlnode in xmlnodelist)
            { 
                //获取支节点信息
                foreach (XmlNode childxmlnode in xmlnode.ChildNodes)
                {
                    string sd = childxmlnode.Attributes[WorkCurveHelper.LanguageShortName].Value;
                    combReportInfo.Items.Add(sd);
                    
                }
            }
            if(combReportInfo.Items.Count>0)
            combReportInfo.SelectedIndex = 0;
        }

        #endregion

        

        #region 公司其它信息界面操作

        /// <summary>
        /// 初始化公司其它信息
        /// </summary>
        private void InstanceOtherInfo()
        {
            BindingSource bs = new BindingSource();
            foreach (CompanyOthersInfo otherinfo in companyOthersInfo_List)
            {
                if (otherinfo.IsReport && otherinfo.ExcelModeTarget == null)
                {
                    string sTarget=string.Empty;
                    GetTarget(otherinfo.Name,ref sTarget);
                    otherinfo.ExcelModeTarget = sTarget;
                }

                if (otherinfo.ExcelModeTarget != "" && otherinfo.IsReport)
                {
                    string sOtherInfo=string.Empty;
                    GetCombReportInfoByTarget(otherinfo.ExcelModeTarget, ref sOtherInfo);
                    otherinfo.Name = sOtherInfo;
                }

                bs.Add(otherinfo);
            }
            dgvwCompanyOtherInfo.AutoGenerateColumns = false;
            dgvwCompanyOtherInfo.DataSource = bs;//绑定数据源
        }

        private void butAddName_Click(object sender, EventArgs e)
        {
            if (ValidateHelper.IllegalCheck(txtName))
            {
                if (companyOthersInfo_List.Find(l => l.Name == txtName.Text) != null)
                {
                    Msg.Show(Info.ExistName);//命名重复
                }
                else if (combReportInfo.Items.Contains(txtName.Text))
                {
                    Msg.Show(Info.strOtherInfo);//报表信息列，不能在此添加
                    return;
                }
                else
                {
                    int icomControlType = 0;
                    if (comControlType.Text == Info.strTextBox) icomControlType = 0;
                    else if (comControlType.Text == Info.strComboBox) icomControlType = 1;
                    else if (comControlType.Text == Info.strDateTimePicker) icomControlType = 2;
                    var v = CompanyOthersInfo.New.Init(txtName.Text, false,  false, icomControlType, ReportTemplateHelper.ExcelModeType.ToString(),"", "");
                    //var v = CompanyOthersInfo.New.Init(txtName.Text, false,WorkCurveHelper.WorkCurveCurrent.Id, false, icomControlType, ExcelTemplateParams.iTemplateType.ToString(), "");
                    companyOthersInfo_List.Add(v);
                    InstanceOtherInfo();
                }
            }
            txtName.Text = "";
        }

        private void butDeleteName_Click(object sender, EventArgs e)
        {
            if (dgvwCompanyOtherInfo.CurrentRow ==null)
            {
                Msg.Show(Info.NoSelect);
            }
            else
            {
                string name = this.dgvwCompanyOtherInfo.CurrentRow.Cells[ColName.Name].Value.ToString();
                string sTarget = string.Empty;
                GetTarget(name,ref sTarget);

                CompanyOthersInfo deleinfo = companyOthersInfo_List.Find(delegate(CompanyOthersInfo v) { return v.ExcelModeTarget == sTarget; });
                if (deleinfo!=null)
                {
                    companyOthersInfo_List_Del.Add(deleinfo);

                }
                companyOthersInfo_List.Remove(deleinfo);
                InstanceOtherInfo();
                
            }
        }

        private void dgvwCompanyOtherInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;

            InstanceOtherListInfo();
        }

        #endregion

        /// <summary>
        /// 根据信息获取代替信息
        /// </summary>
        /// <param name="sCombReportInfo"></param>
        /// <param name="sTarget"></param>
        private void GetTarget(string sCombReportInfo, ref string sTarget)
        {
            if (xmlnodelist != null || xmlnodelist.Count > 0)
            {
                string strWhere = "lable[@" + WorkCurveHelper.LanguageShortName + " = '" + sCombReportInfo + "']";
                XmlNodeList childxmlnodelist = xmlnodelist[0].SelectNodes(strWhere);
                if (childxmlnodelist == null || childxmlnodelist.Count == 0) return;

                sTarget = childxmlnodelist[0].Attributes["Target"].Value;
            }
        }

        private void GetCombReportInfoByTarget(string sTarget, ref string sCombReportInfo)
        {
            if (xmlnodelist != null || xmlnodelist.Count > 0)
            {
                string strWhere = "lable[@Target = '" + sTarget + "']";
                XmlNodeList childxmlnodelist = xmlnodelist[0].SelectNodes(strWhere);
                if (childxmlnodelist == null || childxmlnodelist.Count == 0) return;

                sCombReportInfo = childxmlnodelist[0].Attributes[WorkCurveHelper.LanguageShortName].Value;
            }
        }

        

       

        #region 公司其它信息明细界面操作

        /// <summary>
        /// 初始化公司其它明细信息
        /// </summary>
        private void InstanceOtherListInfo()
        {
            string CompanyOthersInfo_Name = "";
            if (dgvwCompanyOtherInfo.CurrentRow == null) return;
            CompanyOthersInfo_Name = this.dgvwCompanyOtherInfo.CurrentRow.Cells[ColName.Name].Value.ToString();

            CompanyOthersInfo seleCompanyOthersInfo = companyOthersInfo_List.Find(delegate(CompanyOthersInfo v) { return v.Name == CompanyOthersInfo_Name; });

            if (seleCompanyOthersInfo.ControlType == 1)
            {
                butAddInfo.Enabled = true;
                butAddInfo.BackColor = Color.GhostWhite;
                butDeleteInfo.Enabled = true;
                butDeleteInfo.BackColor = Color.GhostWhite;

                BindingSource bs = new BindingSource();
                foreach (CompanyOthersListInfo otherinfo in seleCompanyOthersInfo.CompanyOthersListInfo)
                {
                    bs.Add(otherinfo);
                }
                dgvwOthersListInfo.AutoGenerateColumns = false;
                dgvwOthersListInfo.DataSource = bs;//绑定数据源
            }
            else
            {
                dgvwOthersListInfo.DataSource = null;
                butAddInfo.Enabled = false;
                butDeleteInfo.Enabled = false;
            }
        }
        private void butAddInfo_Click(object sender, EventArgs e)
        {
            if (ValidateHelper.IllegalCheck(txtInfo))
            {
                string CompanyOthersInfo_Name = "";
                if (dgvwCompanyOtherInfo.CurrentRow == null) return;
                CompanyOthersInfo_Name = this.dgvwCompanyOtherInfo.CurrentRow.Cells[ColName.Name].Value.ToString();
                CompanyOthersInfo seleCompanyOthersInfo = companyOthersInfo_List.Find(delegate(CompanyOthersInfo v) { return v.Name == CompanyOthersInfo_Name; });

                if (seleCompanyOthersInfo.CompanyOthersListInfo.ToList().Find(l => l.ListInfo == txtInfo.Text) != null)
                {
                    Msg.Show(Info.ExistName);//命名重复
                }
                else
                {

                    var v = CompanyOthersListInfo.New.Init(seleCompanyOthersInfo, txtInfo.Text,false);
                    seleCompanyOthersInfo.CompanyOthersListInfo.Add(v);
                    //companyOthersListInfo_List.Add(v);
                    InstanceOtherListInfo();
                }
            }
            txtInfo.Text = "";
        }

        private void butDeleteInfo_Click(object sender, EventArgs e)
        {
            if (dgvwOthersListInfo.CurrentRow == null)
            {
                Msg.Show(Info.NoSelect);
            }
            else
            {
                string CompanyOthersInfo_Name = "";
                if (dgvwCompanyOtherInfo.CurrentRow == null) return;
                CompanyOthersInfo_Name = this.dgvwCompanyOtherInfo.CurrentRow.Cells[ColName.Name].Value.ToString();
                CompanyOthersInfo seleCompanyOthersInfo = companyOthersInfo_List.Find(delegate(CompanyOthersInfo v) { return v.Name == CompanyOthersInfo_Name; });

                string name = this.dgvwOthersListInfo.CurrentRow.Cells[ColData.Name].Value.ToString();
                CompanyOthersListInfo deleinfo = seleCompanyOthersInfo.CompanyOthersListInfo.ToList().Find(delegate(CompanyOthersListInfo v) { return v.ListInfo == name; });
                if (deleinfo != null)
                {
                    seleCompanyOthersInfo.CompanyOthersListInfo.Remove(deleinfo);
                }
                InstanceOtherListInfo();

            }
        }

        /// <summary>
        /// 默认列选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwOthersListInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (dgvwOthersListInfo.Columns[e.ColumnIndex].Name.Equals("ColIsAcquiescence"))//默认
            {
                if (dgvwOthersListInfo[e.ColumnIndex, e.RowIndex].Value.Equals(false))
                {
                    dgvwOthersListInfo[e.ColumnIndex, e.RowIndex].Value = true;
                    for (int i = 0; i < dgvwOthersListInfo.Rows.Count; i++)
                    {
                        if (i != e.RowIndex)
                        {
                            dgvwOthersListInfo["ColIsAcquiescence", i].Value = false;
                        }
                    }
                }
                else
                {
                    dgvwOthersListInfo[e.ColumnIndex, e.RowIndex].Value = true;
                }
            }
        }

        #endregion


        private void btnApplication_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            foreach (var v in companyOthersInfo_List)
            {
                v.Save();
            }
            foreach (var v in companyOthersInfo_List_Del)
            {
                Lephone.Data.DbEntry.Context.ExecuteNonQuery("delete from historycompanyotherinfo where workcurveid='" + WorkCurveHelper.WorkCurveCurrent.Id + "' and companyothersinfo_id='" + v.Id + "'");
                v.Delete();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Save();
            EDXRFHelper.GotoMainPage(this);//转到主界面
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//转到主界面
        }

        

        

        

      
    }
}
