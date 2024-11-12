using System;
using Skyray.Language;
using System.Linq;
using System.Windows.Forms;
using Skyray.Controls;
using Skyray.EDX.Common;
using Lephone.Data.Common;
using Lephone.Data;
using System.Collections.Generic;
using Lephone.Data.Definition;
using System.Data.SQLite;
using System.Data;
using System.Collections;
using System.Data.OleDb;


namespace Skyray.UC
{
    /// <summary>
    /// 编辑语言类
    /// </summary>
    public partial class UCLanguage : Skyray.Language.UCMultiple
    {

        #region Fields

        /// <summary>
        /// 语言数据列表
        /// </summary>
        private DbObjectList<Languages> lstLang;
        /// <summary>
        /// 新语言
        /// </summary>
        private Languages langNew;

        /// <summary>
        /// 默认语言
        /// </summary>
        private Languages defaultLang;

        #endregion

        #region Init
        /// <summary>
        /// 构造
        /// </summary>
        /// 
        public UCLanguage()
        {
            InitializeComponent();
        }

        public override void SaveText()
        {
            Skyray.Language.Lang.Model.SaveHeaderTextProperty(this.dgvLang.Columns[0]);
        }
        public override void SetText()
        {
            if (this.dgvLang.Columns.Count > 0)
                Skyray.Language.Lang.Model.SetHeaderTextProperty(this.dgvLang.Columns[0]);
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCLanguage_Load(object sender, EventArgs e)
        {
#if DEBUG
            lblTranslate.Visible = true;
            btnTranslateSelectRow.Visible = true;
            btnImportData.Visible = true;
            Skyray.Language.Lang.Model.OnTranslate += new EventHandler<LanguageModel.TranslateEventArgs>(Model_OnTranslate);
#endif

            lstLang = Languages.FindAll();//加载所有语言

            if (lstLang.Count == 0) return;//无数据返回

            DataGridViewTextBoxColumn dgvAttribute = new DataGridViewTextBoxColumn();
            dgvAttribute.Name = "colFieldname";
            //dgvAttribute.HeaderText = Info.AttributeName;
            dgvAttribute.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvAttribute.Width = 110;
            dgvLang.Columns.Add(dgvAttribute);

            foreach (var lang in lstLang)
            {
                if (lang.IsDefaultLang) defaultLang = lang;

                DataGridViewTextBoxColumn dgvc = new DataGridViewTextBoxColumn();
                dgvc.Name = lang.ShortName;
                dgvc.HeaderText = lang.FullName;
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvLang.Columns.Add(dgvc);
                CheckBox checkBox = new CheckBox();
                checkBox.Name = lang.ShortName;
            }

            int rowCount = lstLang[0].LanguageDatas.Count;
            int colCount = lstLang.Count;
            string key;
            string value;
            LanguageData data;
            for (int i = 0; i < rowCount; i++)
            {
                var objs = new object[colCount + 1];
                key = lstLang[0].LanguageDatas[i].Key;
                objs[0] = key;
                for (int j = 0; j < colCount; j++)
                {
                    data = lstLang[j].LanguageDatas.FirstOrDefault(x => x.Key == key);
                    value = data == null ? string.Empty : data.Value;
                    objs[j + 1] = value;
                }
                dgvLang.Rows.Add(objs);
            }
        }


        #endregion

        #region Events

        /// <summary>
        /// 添加新语言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddLang_Click(object sender, EventArgs e)
        {
            if (txtLangName.Text.Equals(""))
            {
                SkyrayMsgBox.Show(Info.NameIsNull);
            }
            else if (lstLang.Find(l => l.FullName == txtLangName.Text) != null)
            {
                SkyrayMsgBox.Show(Info.NameRepeat);//名称重复
            }
            else
            {
                DataGridViewTextBoxColumn dgvc = new DataGridViewTextBoxColumn();
                dgvc.Name = txtLangName.Text;
                dgvc.HeaderText = txtLangName.Text;
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvLang.Columns.Add(dgvc);
                langNew = Languages.New.Init(txtLangName.Text, txtLangName.Text, false, false);
                lstLang.Add(langNew);//添加新语言
            }
        }
        /// <summary>
        /// 保存语言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveLang_Click(object sender, EventArgs e)
        {
            for (int k = 0; k < lstLang.Count; k++)
            {
                List<LanguageData> lstData = new List<LanguageData>();
                if (lstLang[k].LanguageDatas.Count > 0)//修改
                {
                    for (int i = 0; i < dgvLang.Rows.Count; i++)
                    {
                        string key = Convert.ToString(dgvLang[0, i].Value);
                        string value = Convert.ToString(dgvLang[k + 1, i].Value);
                        if (string.IsNullOrEmpty(key)) continue;
                        var data = lstLang[k].LanguageDatas.FirstOrDefault(l => l.Key == key);
                        if (data != null)
                        {
                            data.Value = value;
                        }
                        else
                        {
                            lstLang[k].LanguageDatas.Add(LanguageData.New.Init(key, value));
                        }
                    }
                    lstLang[k].Save();

                }
                else if (lstLang[k].LanguageDatas.Count == 0)//保存
                {
                    for (int i = 0; i < dgvLang.Rows.Count; i++)
                    {
                        string key = Convert.ToString(dgvLang[0, i].Value);
                        string value = Convert.ToString(dgvLang[k + 1, i].Value);
                        if (string.IsNullOrEmpty(key)) continue;

                        var data = LanguageData.New.Init(key, value);
                        //data.MultipleLanguage = lstLang[k];
                        lstData.Add(data);
                    }
                    lstLang[k].Save();
                    DbEntry.GetContext("Lang").FastSaveList(lstData, lstLang[k].Id);
                }
            }
            LangHelper.InitLangMenu(Lang.LangItem);
        }
        /// <summary>
        /// 删除语言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelLang_Click(object sender, EventArgs e)
        {
            int count = dgvLang.SelectedColumns.Count;
            if (count <= 0)
            {
                SkyrayMsgBox.Show(Info.NoSelect);
                return;
            }
            int index = dgvLang.SelectedColumns[0].Index;
            if (index <= 2)
            {
                SkyrayMsgBox.Show(Info.CanotDel);
                return;
            }
            //if (lstLang[index - 1].)
            if (lstLang[index - 1].IsCurrentLang)
            {
                SkyrayMsgBox.Show(Info.CanotDel);
                return;
            }
            if (DialogResult.Yes == SkyrayMsgBox.Show(Info.ConfirmDel, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                // Lephone.Data.DbEntry.Context.ExecuteNonQuery("delete from LanguageData where Language_Id= " + lstLang[index - 1].Id);

                Lephone.Data.DbEntry.GetContext("Lang").FastDeleteList(lstLang[index - 1].LanguageDatas);
                var id = lstLang[index - 1].Id;
                Languages.DeleteAll(x => x.Id == id);

                //lstLang[index - 1].Delete();//删除数据

                lstLang.RemoveAt(index - 1);
                dgvLang.Columns.RemoveAt(index);//删除缓存
                LangHelper.InitLangMenu(Lang.LangItem);
            }
        }

        #endregion

        private void Model_OnTranslate(object sender, Skyray.Language.LanguageModel.TranslateEventArgs e)
        {
            dgvLang[transColIndex, e.Line].Value = e.Result;
        }

        int transColIndex = -1;
        private void lblTranslate_Click(object sender, EventArgs e)
        {
            if (dgvLang.SelectedColumns.Count > 0)
            {
                transColIndex = dgvLang.SelectedColumns[0].Index;
                if (transColIndex > 1)
                {
                    var targetId = lstLang[transColIndex - 1].Id;

                    Skyray.Language.Lang.Model.Translate(defaultLang.LanguageDatas, targetId);
                }
            }
        }

        private void btnTranslateSelectRow_Click(object sender, EventArgs e)
        {
            List<int> lstRowIndex = new List<int>();
            List<int> lstColIndex = new List<int>();

            foreach (DataGridViewCell cell in dgvLang.SelectedCells)
            {
                if (!lstRowIndex.Contains(cell.RowIndex))
                    lstRowIndex.Add(cell.RowIndex);

                if (!lstColIndex.Contains(cell.ColumnIndex))
                    lstColIndex.Add(cell.ColumnIndex);
            }
            if (lstColIndex.Count > 0 && lstRowIndex.Count > 0)
            {
                if (lstColIndex.Count > 1)
                {
                    MessageBox.Show("只能选中一列!");
                }
                else
                {
                    transColIndex = lstColIndex[0];
                    Skyray.Language.Lang.Model.Translate(defaultLang.LanguageDatas,
                        lstLang[1].Id, lstRowIndex.ToArray());
                }
            }
        }

        private void btnImportData_Click(object sender, EventArgs e)
        {
            if (dgvLang.SelectedColumns.Count > 0)
            {
                transColIndex = dgvLang.SelectedColumns[0].Index;
                if (transColIndex > 1)
                {
                    var targetId = lstLang[transColIndex - 1].Id;
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.Filter = "PDF File (*.sdb)|*.sdb";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + dlg.FileName))
                        {
                            conn.Open();
                            SQLiteCommand cmd = new SQLiteCommand
                                ("select * from LanguageData where Language_Id=" + targetId, conn);
                            var dr = cmd.ExecuteReader();
                            int i = 0;
                            while (dr.Read())
                            {
                                string key = dr[1].ToString();
                                string value = dr[2].ToString();
                                foreach (DataGridViewRow drow in dgvLang.Rows)
                                {
                                    if (drow.Cells[0].Value.ToString() == key)
                                    {
                                        drow.Cells[transColIndex].Value = value;
                                        i++;
                                        Console.Write("Update " + i);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btWSearch_Click(object sender, EventArgs e)
        {
            if (this.txtBoxContext.Text.IsNullOrEmpty())
            {
                int rowCount = lstLang[0].LanguageDatas.Count;
                int colCount = lstLang.Count;
                string key;
                string value;
                LanguageData data;
                for (int i = 0; i < rowCount; i++)
                {
                    var objs = new object[colCount + 1];
                    key = lstLang[0].LanguageDatas[i].Key;
                    objs[0] = key;
                    for (int j = 0; j < colCount; j++)
                    {
                        data = lstLang[j].LanguageDatas.FirstOrDefault(x => x.Key == key);
                        value = data == null ? string.Empty : data.Value;
                        objs[j + 1] = value;
                    }
                    dgvLang.Rows.Add(objs);
                }
            }
            if (dgvLang.SelectedColumns.Count > 0)
            {
                transColIndex = dgvLang.SelectedColumns[0].Index;
                if (transColIndex > 0)
                {
                    var targetId = lstLang[transColIndex - 1].Id;
                    DbEntry.GetContext("Lang").UsingConnection(delegate
                    {
                        string sql = "select * from LanguageData where Language_Id=" + targetId + " and Value like '%" + this.txtBoxContext.Text + "%'";
                        using (SQLiteConnection conn = new SQLiteConnection("Data Source=Language.sdb"))
                        {
                            conn.Open();
                            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                            var dr = cmd.ExecuteReader();
                            this.dgvLang.Rows.Clear();
                            string key = string.Empty;
                            while (dr.Read())
                            {
                                var objs = new object[lstLang.Count + 1];
                                key = dr[1].ToString();
                                string value;
                                LanguageData data;
                                objs[0] = key;
                                for (int j = 0; j < lstLang.Count; j++)
                                {
                                    data = lstLang[j].LanguageDatas.FirstOrDefault(x => x.Key == key);
                                    value = data == null ? string.Empty : data.Value;
                                    objs[j + 1] = value;
                                }
                                dgvLang.Rows.Add(objs);
                            }
                        }
                    }
                    );
                }
            }
        }

        private void btwExcel_Click(object sender, EventArgs e)
        {
            dgvLang.ExportExcel_Public();
        }

        private void btWInputExcel_Click(object sender, EventArgs e)
        {
            DataSet ds = ExcelToDS();
            int RowsCount = ds.Tables[0].Rows.Count;
            Lephone.Data.DbEntry.GetContext("Lang").ExecuteNonQuery("delete from LanguageData");
            int columnCount = ds.Tables[0].Columns.Count;
            for (int j = 1; j < columnCount; j++)
            {
                List<LanguageData> lstData = new List<LanguageData>();
                Languages newLang = Languages.FindOne(w => w.FullName == ds.Tables[0].Columns[j].ColumnName);
                if (newLang == null)
                {
                    newLang = Languages.New.Init(ds.Tables[0].Columns[j].ColumnName, ds.Tables[0].Columns[j].ColumnName, false, false);
                    newLang.Save();
                }
                for (int i = 0; i < RowsCount; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    string key = dr[0].ToString();
                    string value = Convert.ToString(dr[j]).TrimEnd().TrimStart();
                    if (string.IsNullOrEmpty(key)) continue;
                    var data = LanguageData.New.Init(key, value);
                    lstData.Add(data);
                }
                DbEntry.GetContext("Lang").FastSaveList(lstData, newLang.Id);
            }
            this.dgvLang.Rows.Clear();
            this.dgvLang.Columns.Clear(); ;
            UCLanguage_Load(null, null);
        }

        public DataSet ExcelToDS()
        {
             DataSet ds = null;
             if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
             {
                 string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + this.openFileDialog1.FileName + ";" + "Extended Properties=Excel 8.0";
                 OleDbConnection conn = new OleDbConnection(strConn);
                 conn.Open();
                 string strExcel = "";
                 OleDbDataAdapter myCommand = null;
                 strExcel = string.Format("select * from [Sheet1$]");
                 myCommand = new OleDbDataAdapter(strExcel, strConn);
                 ds = new DataSet();
                 myCommand.Fill(ds);
                 conn.Close();
             }
            return ds;
        }

    }
}
