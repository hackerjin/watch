using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.Controls;
using System.Data.SQLite;
using Lephone.Data;

namespace Skyray.UC
{
    public partial class FrmHistoryColumnManage : Form
    {
        public FrmHistoryColumnManage()
        {
            InitializeComponent();
        }

        public DataTable dtColmunManage;

        private void GetSetColmun()
        {
            string[] strcolmun = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "AddColumn").Split(',');
            foreach (string strc in strcolmun)
            {
                if (strc.IndexOf("~") != -1)
                {
                    DataRow row = dtColmunManage.NewRow();
                    row["ColmunTableName"] = strc.Split('~')[0];
                    row["ColmunName"] = strc.Split('~')[1];
                    dtColmunManage.Rows.Add(row);
                }

            }
            this.dgvColumnManage.DataSource = dtColmunManage;
            this.dgvColumnManage.Columns["ColmunTableName"].Visible = false;
            this.dgvColumnManage.Columns["ColmunName"].HeaderText = Info.strUserAddColumn;
            this.dgvColumnManage.Columns["ColmunName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void FrmHistoryColumnManage_Load(object sender, EventArgs e)
        {
            DataGridViewCheckBoxColumn colCB01 = new DataGridViewCheckBoxColumn();
            colCB01.Name = "aa";
            colCB01.Width = 20;
            colCB01.HeaderText = "";
            this.dgvColumnManage.Columns.Add(colCB01);

            dtColmunManage = new DataTable("Words");
            DataColumn dc0 = dtColmunManage.Columns.Add("ColmunTableName", Type.GetType("System.String"));
            DataColumn dc1 = dtColmunManage.Columns.Add("ColmunName", Type.GetType("System.String"));

            GetSetColmun();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtAddColmunName.Text == "") return;

            DataRow[] foundRows = dtColmunManage.Select("ColmunName='" + txtAddColmunName.Text + "' ");
            if (foundRows.Length > 0)
            {
                SkyrayMsgBox.Show(Info.ExistName);//命名重复
                return;
            }

            int max = 0;
            foreach (DataRow rows in dtColmunManage.Rows)
            {
                if (int.Parse(rows["ColmunTableName"].ToString().Replace("column", "")) > max)
                    max = int.Parse(rows["ColmunTableName"].ToString().Replace("column", ""));
            }


            DataRow row = dtColmunManage.NewRow();
            row["ColmunTableName"] = "column" + (max + 1).ToString("D2");
            row["ColmunName"] = txtAddColmunName.Text;
            dtColmunManage.Rows.Add(row);

            txtAddColmunName.Text = "";
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            List<string> deleList = new List<string>();
            foreach (DataGridViewRow row in dgvColumnManage.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == "True")
                {
                    deleList.Add(row.Cells["ColmunName"].Value.ToString());
                }
            }
            if (deleList == null || deleList.Count == 0) return;

            foreach (string s in deleList)
            {
                DataRow[] foundRows = dtColmunManage.Select("ColmunName='" + s + "' ");
                if (foundRows.Length > 0)
                {
                    dtColmunManage.Rows.Remove(foundRows[0]);
                }
            }

        }

        private void SaveColmun()
        {
            string strcolmun = "";

            string strsql = "";

            DataTable dt = GetData("select sql from sqlite_master where tbl_name='HistoryRecord' and type='table'");
            foreach (DataRow row in dtColmunManage.Rows)
            {
                strcolmun += row["ColmunTableName"] + "~" + row["ColmunName"] + ",";
                if (dt.Rows[0][0].ToString().IndexOf(row["ColmunTableName"].ToString()) == -1) strsql += "alter table historyrecord add " + row["ColmunTableName"] + " NTEXT ;  ";
            }
            if (strcolmun != "") strcolmun = strcolmun.Substring(0, strcolmun.Length - 1);

            if (strsql != "") Lephone.Data.DbEntry.Context.ExecuteNonQuery(strsql);
            ReportTemplateHelper.SaveSpecifiedValue("HistoryItem", "AddColumn", strcolmun);

            //删除字段处理
            bool isdelecolumn = false;
            string stroldcolumn = "";
            string stroldcolumn1 = "";
            dt = GetData("select sql from sqlite_master where tbl_name='HistoryRecord' and type='table'");
            string sinfo = dt.Rows[0][0].ToString();
            string sinfo1 = dt.Rows[0][0].ToString();
            int ibracket = sinfo.IndexOf("(");
            sinfo = sinfo.Substring(ibracket + 1, sinfo.Length - (ibracket + 1));

            //string sinfo = dt.Rows[0][0].ToString().Replace("CREATE TABLE [HistoryRecord] (","");

            List<string> deleColmun = new List<string>();
            string[] colmun = sinfo.Split(',');

            foreach (string s in colmun)
            {
                string cil = "";
                if (s.Split(' ')[0] == "") cil = s.Split(' ')[1]; else cil = s.Split(' ')[0];
                if (cil.IndexOf('[') != -1) cil = cil.Split('[')[1];
                if (cil.IndexOf(']') != -1) cil = cil.Split(']')[0];

                DataRow[] foundRows = dtColmunManage.Select("ColmunTableName='" + cil + "' ");
                if (cil.Contains("column") && foundRows.Length == 0) isdelecolumn = true;

                if (cil.Contains("column") && foundRows.Length > 0)
                {
                    stroldcolumn1 += "," + cil.Replace("(", "").Replace(")", "");
                    stroldcolumn += "," + s.Replace("(", "").Replace(")", "");
                }
                else if (!cil.Contains("column"))
                {
                    stroldcolumn1 += "," + cil;
                    stroldcolumn += "," + s;
                }
                else
                    deleColmun.Add(cil);

            }

            if (isdelecolumn)
            {
                string strSetting = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "Setting");
                foreach (string s in deleColmun)
                    if (strSetting.Contains(s)) strSetting = strSetting.Replace(s, "");
                ReportTemplateHelper.SaveSpecifiedValue("HistoryItem", "Setting", strSetting);

                stroldcolumn1 = stroldcolumn1.Substring(1, stroldcolumn1.Length - 1);
                stroldcolumn = stroldcolumn.Substring(1, stroldcolumn.Length - 1);
                string delecolmun = @"BEGIN TRANSACTION; " +
                " CREATE TEMPORARY TABLE HistoryRecord_backup(" + stroldcolumn + ");" +
                " INSERT INTO HistoryRecord_backup SELECT " + stroldcolumn1 + " FROM HistoryRecord;" +
                " DROP TABLE HistoryRecord;" +
                " CREATE TABLE HistoryRecord(" + stroldcolumn + ");" +
                " INSERT INTO HistoryRecord SELECT " + stroldcolumn1 + " FROM HistoryRecord_backup;" +
                " DROP TABLE HistoryRecord_backup;" +
                " COMMIT;";

                Lephone.Data.DbEntry.Context.ExecuteNonQuery(delecolmun);
            }






        }

        private DataTable GetData(string strSql)
        {
            DataTable dt = new DataTable();
            string connectionString = DbEntry.Context.Driver.ConnectionString;
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveColmun();
            this.DialogResult = DialogResult.OK;
            this.Hide();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        private void toolTipW1_Popup(object sender, PopupEventArgs e)
        {

        }


    }
}
