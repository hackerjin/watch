using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Skyray.Language;
using Skyray.EDXRFLibrary.Define;
using Skyray.EDX.Common;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Collections.Specialized;
using System.Collections;
using System.Data.OleDb;
using System.IO;
using Aspose.Cells;
using Skyray.Controls;

namespace Skyray.UC
{
    public partial class FrmBatchTest : MultipleForm
    {
        private IBatchTest iBatchTest;
        private UCOtherInfoSet otherInfoSetter;
        private RecordInfo riBase;
        private List<RecordInfo> infoList;
        private const string ColNameId = "Id";
        private const string ColNameState = "State";
        private const string ColNameSampleName = "SampleName";
        private const string ColNameSupplier = "Supplier";
        private List<string> excludeColumnNameList;
        private Thread thd;
        private volatile static bool isBusy = false;
        private AutoResetEvent evt = new AutoResetEvent(false);
        private volatile static bool thdExit = true;
        private System.Drawing.Font font;
        public FrmBatchTest(IBatchTest bt)
        {
            InitializeComponent();
            iBatchTest = bt;
            iBatchTest.ActionAfterTestFinished = SetEvent;
            font = new System.Drawing.Font(this.Font.FontFamily, 12);
            SetMsgBoxLang();
        }

        private void SetMsgBoxLang()
        {
            MsgBox.Yes = Info.CustomMsgBoxYes;
            MsgBox.No = Info.CustomMsgBoxNo;
            MsgBox.Cancel = Info.CustomMsgBoxCancel;
            MsgBox.OK = Info.CustomMsgBoxOK;
            MsgBox.Ignore = Info.CustomMsgBoxIgnore;
            MsgBox.Abort = Info.CustomMsgBoxAbort;
            MsgBox.Retry = Info.CustomMsgBoxRetry;
        }

        private void ShowError(string msg)
        {
            Toast.ShowError(this, msg, font);
        }

        private void ShowSuccess(string msg)
        {
            Toast.ShowSuccess(this, msg, font);
        }

        private void ShowSuccessStay(string msg)
        {
            Toast.ShowSuccess(this, msg, null, null, font);
        }

        private void ShowWarning(string msg)
        {
            Toast.ShowWarning(this, msg, font);
        }

        private DialogResult Show(string msg)
        {
            return MsgBox.Show(this, msg, Color.DarkOrange, Color.LightSlateGray, 10, 230, new Size(100, 80), new Size(500, 400), new Padding(20, 25, 20, 20), font, MessageBoxButtons.YesNo, new Size(100, 30), 15, 20, false);
        }

        private void SetEvent()
        {
            if (evt != null && !thdExit)
            {
                evt.Set();
            }
        }

        private void FrmBatchTest_Load(object sender, EventArgs e)
        {
            otherInfoSetter = new UCOtherInfoSet(new Rectangle(new Point(tbxSampleName.Location.X - 11, tbxSampleName.Location.Y), tbxSampleName.Size), 7, 10, 25);
            pnlOtherInfos.Controls.Add(otherInfoSetter);
            otherInfoSetter.Dock = DockStyle.Fill;
            excludeColumnNameList = new List<string>{ ColNameSampleName, ColNameSupplier, ColNameId, ColNameState};
            PreLoadRecordInfo();
            this.ClearText(pnlOtherInfos, true);
            btnBTBrowser.Select();
        }

        private void PreLoadRecordInfo()
        {
            riBase = new RecordInfo()
            {
                SampleName = string.Empty,
                Supplier = string.Empty
            };
            var list = otherInfoSetter.GetOtherInfoList();
            if (list == null || list.Count <= 0) return;
            foreach (var item in list)
            {
                if (riBase.OtherInfoDictionary == null)
                {
                    riBase.OtherInfoDictionary = new HybridDictionary();
                }
                riBase.OtherInfoDictionary.Add(item.Name, item.DefaultValue);
            }
            ContructDgv(dgvContent, riBase);
        }

        private void btnConfirmSingle_Click(object sender, EventArgs e)
        {
            if (isBusy)
            {
                ShowWarning(Info.BTUnderBatchTest);
                return;
            }
            if (tbxSampleName.Text == null || tbxSampleName.Text.Trim().Length <= 0)
            {
                ShowWarning(Info.BTSampleNameCanNotBeEmpty);
                return;
            }
            var ri = new RecordInfo()
            {
                SampleName = tbxSampleName.Text.Trim(),
                Supplier = tbxSupplier.Text.Trim()
            };
            var list = otherInfoSetter.GetOtherInfoList();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (ri.OtherInfoDictionary == null)
                    {
                        ri.OtherInfoDictionary = new HybridDictionary();
                    }
                    ri.OtherInfoDictionary.Add(item.Name, item.DefaultValue);
                }
            }
            ContructDgv(dgvContent, ri);
            ContructRow(dgvContent, ri);
            ClearInput();
        }

        private void ClearInput()
        {
            ClearText(grpBTSingleInput, true);
        }

        private void ClearText(Control parent, bool containChildControls)
        {
            if (parent == null)
                return;
            foreach (Control con in parent.Controls)
            {
                if(containChildControls && con.HasChildren)
                    ClearText(con, containChildControls);
                if (con is TextBox)
                {
                    (con as TextBox).Clear();
                }
            }
        }

        private void ContructRow(DataGridView dgv, RecordInfo ri)
        {
            if (dgv == null || ri == null || dgv.Columns.Count <= 0)
                return;
            dgv.RowCount++;
            dgv[ColNameId, dgv.RowCount - 1].Value = dgv.RowCount;
            dgv[ColNameState, dgv.RowCount - 1].Value = "Waiting";
            dgv[ColNameSampleName, dgv.RowCount - 1].Value = ri.SampleName;
            dgv[ColNameSupplier, dgv.RowCount - 1].Value = ri.Supplier;
            if (ri.OtherInfoDictionary != null && ri.OtherInfoDictionary.Count > 0)
            {
                foreach (DictionaryEntry e in ri.OtherInfoDictionary)
                {
                    if (dgv.Columns.Contains(e.Key.ToString()))
                        dgv[e.Key.ToString(), dgv.RowCount - 1].Value = e.Value.ToString();
                }
            }
        }

        private void ContructDgv(DataGridView dgv, RecordInfo ri)
        {
            if (dgv == null || ri == null || dgv.Columns.Count > 0)
                return;
            var col = new DataGridViewTextBoxColumn()
            {
                Name = ColNameId,
                HeaderText = Info.BTOrderNumber,
                Width = 60,
                ReadOnly = true,
                ValueType = typeof(int),
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dgv.Columns.Add(col);
            col = new DataGridViewTextBoxColumn()
            {
                Name = ColNameState,
                HeaderText = Info.BTState,
                Width = 80,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dgv.Columns.Add(col);
            col = new DataGridViewTextBoxColumn()
            {
                Name = ColNameSampleName,
                HeaderText = Info.BTSampleName,
                Width = 80,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dgv.Columns.Add(col);
            col = new DataGridViewTextBoxColumn()
            {
                Name = ColNameSupplier,
                HeaderText = Info.BTSupplier,
                Width = 80,
                ReadOnly = true,
                ValueType = typeof(string),
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dgv.Columns.Add(col);
            if (ri.OtherInfoDictionary != null && ri.OtherInfoDictionary.Count > 0)
            {
                foreach (DictionaryEntry e in ri.OtherInfoDictionary)
                {
                    col = new DataGridViewTextBoxColumn()
                    {
                        Name = e.Key.ToString(),
                        HeaderText = e.Key.ToString(),
                        Width = 80,
                        ReadOnly = true,
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgv.Columns.Add(col);
                }
            }
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            if (isBusy)
            {
                ShowWarning(Info.BTUnderBatchTest);
                return;
            }
            OpenExcelFile();
        }

        private bool OpenExcelFile()
        {
            var fd = new OpenFileDialog
            {
                Title = "Open excel file",
                Filter = "Excel(*.xls) |*.xls",
                CheckFileExists = true
            };
            if (fd.ShowDialog() == DialogResult.OK)
            {
                var p = Path.GetFullPath(fd.FileName);
                var msg = string.Empty;
                var dt = this.InputExcel(p, ref msg);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Columns.Count < dgvContent.ColumnCount - 2)
                    {
                        ShowError(Info.BTColumnCountLessThanCommand);
                        return false;
                    }

                    if (!dt.Columns.Contains(ColNameSampleName) && !dt.Columns.Contains(dgvContent.Columns[ColNameSampleName].HeaderText))
                    {
                        ShowError(Info.BTColumnsNotIncludeInExcel + ":" + ColNameSampleName + " " + Info.BTOr + " " + dgvContent.Columns[ColNameSampleName].HeaderText);
                        return false;
                    }

                    if (!dt.Columns.Contains(ColNameSupplier) && !dt.Columns.Contains(dgvContent.Columns[ColNameSupplier].HeaderText))
                    {
                        ShowError(Info.BTColumnsNotIncludeInExcel + ":" + ColNameSupplier + " " + Info.BTOr + " " + dgvContent.Columns[ColNameSupplier].HeaderText);
                        return false;
                    }
                    if (riBase != null && riBase.OtherInfoDictionary != null && riBase.OtherInfoDictionary.Count > 0)
                    {
                        foreach (DictionaryEntry e in riBase.OtherInfoDictionary)
                        {
                            if (!dt.Columns.Contains(e.Key.ToString()))
                            {
                                ShowError(Info.BTColumnsNotIncludeInExcel + ": " + e.Key.ToString());
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (riBase == null) 
                        {
                            ShowError(Info.BTFailedToLoadRecordInfo);
                            return false;
                        }

                    }
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var ri = new RecordInfo()
                        {
                            SampleName = dt.Columns.Contains(ColNameSampleName) ? dt.Rows[i][ColNameSampleName].ToString() : dt.Rows[i][dgvContent.Columns[ColNameSampleName].HeaderText].ToString(),
                            Supplier = dt.Columns.Contains(ColNameSupplier) ? dt.Rows[i][ColNameSupplier].ToString() : dt.Rows[i][dgvContent.Columns[ColNameSupplier].HeaderText].ToString()
                        };
                        for (var j = 0; j < dt.Columns.Count; j++)
                        {
                            if (excludeColumnNameList.Contains(dt.Columns[j].ColumnName)) continue;
                            if (ri.OtherInfoDictionary == null)
                            {
                                ri.OtherInfoDictionary = new HybridDictionary();
                            }
                            ri.OtherInfoDictionary.Add(dt.Columns[j].ColumnName, dt.Rows[i][dt.Columns[j].ColumnName].ToString());
                        }
                        ContructDgv(dgvContent, ri);
                        ContructRow(dgvContent, ri);

                    }
                    ShowSuccess(Info.BTSucceedToInput);
                    return true;
                }
                else
                {
                    ShowError(Info.BTFailedToInput);
                }
            }
            return false;

        }

        public DataTable InputExcel(string path, ref string exceptionMsg)
        {
            DataTable dt = null;
            try
            {
                var strConn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", path);
                using (var conn = new OleDbConnection(strConn))
                {
                    conn.Open();
                    var sheetDt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    var sheet = new string[sheetDt.Rows.Count];
                    for (var i = 0; i < sheetDt.Rows.Count; i++)
                    {
                        sheet[i] = sheetDt.Rows[i]["TABLE_NAME"].ToString();
                    }
                    var strExcel = string.Format("select * from [{0}]", sheet[0]);
                    var myCommand = new OleDbDataAdapter(strExcel, strConn);
                    dt = new DataTable();
                    myCommand.Fill(dt);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                exceptionMsg = ex.Message;
            }
            return dt;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(isBusy)
            {
                ShowWarning(Info.BTUnderBatchTest);
                return;
            }
            if(iBatchTest == null)
            {
                ShowError("IBatchTest null");
                return;
            }
            if(dgvContent.RowCount <= 0)
            {
                ShowError(Info.BTPlsInputDataFirst);
                return;
            }

            if(thd == null)
            {
                if(!DgvDataToList(dgvContent, ref infoList))
                    return;
                ResetDgv(dgvContent);
                thd = new Thread(() =>
                                     {
                                         thdExit = false;
                                         isBusy = true;
                                         var msg = string.Empty;
                                         var flag = false;
                                         for (int i = 0; i < infoList.Count && !thdExit; i++)
                                         {
                                             this.Invoke(new Action(() =>
                                                                        {
                                                                            dgvContent.ClearSelection();
                                                                            dgvContent.FirstDisplayedScrollingRowIndex = i;
                                                                            dgvContent[ColNameState, i].Value = "Testing";
                                                                            dgvContent.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
                                                                            flag = iBatchTest.StartTest(infoList[i], ref msg);
                                                                        }));
                                             if(!flag)
                                             {
                                                 this.Invoke(new Action(() =>
                                                 {
                                                     dgvContent.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                                                     dgvContent[ColNameState, i].Value = "Fail";

                                                 }));

                                                 var r = this.SafeCall(() =>
                                                 {
                                                     return Show(Info.BTErrorOccurredContinueOrNot + ":" + msg) == DialogResult.Yes;

                                                 });
                                                 if (r)
                                                     continue;
                                                 thdExit = true;
                                                 thd = null;
                                                 isBusy = false;
                                                 return;
                                             }
                                             else
                                             {
                                                 evt.WaitOne();
                                                 if (!thdExit)
                                                 {
                                                     this.Invoke(new Action(() =>
                                                     {
                                                         dgvContent.Rows[i].DefaultCellStyle.BackColor = iBatchTest.IsTestNormal ? Color.Green : Color.Red;
                                                         dgvContent[ColNameState, i].Value = iBatchTest.IsTestNormal ? "Done" : "Fail";
                                                     }));
                                                 }
                                                 
                                                 if (!thdExit && (i < infoList.Count - 1))
                                                 {
                                                     var r = this.SafeCall(() =>
                                                     {
                                                         return Show(Info.BTCurrrentFinishedContinueOrNot) == DialogResult.Yes;

                                                     });
                                                     if (r)
                                                         continue;
                                                 }

                                                 if (!thdExit && (i == infoList.Count - 1))
                                                 {
                                                     this.SafeCall(() => { ShowSuccessStay(Info.BTAllFinished); });

                                                 }
                                                 thdExit = true;
                                                 thd = null;
                                                 isBusy = false;
                                                 return;
                                             }
                                         }
                                     });
                thd.IsBackground = true;
                thd.Start();
            }
            
        }

        private void ResetDgv(DataGridView dgv)
        {
            if (dgv == null || dgv.RowCount <= 0)
                return;
            for (int i = 0; i < dgvContent.RowCount; i++)
            {
                dgvContent.Rows[i].DefaultCellStyle.BackColor = Color.Empty;
                dgvContent[ColNameState, i].Value = "Waiting";
            }

        }

        private bool DgvDataToList(DataGridView dgv, ref List<RecordInfo> list)
        {
            if (dgv == null || dgv.RowCount <= 0)
                return false;
            if(list == null)
            {
                list = new List<RecordInfo>();
            }
            list.Clear();
            for (int i = 0; i < dgv.RowCount; i++)
            {
                var ri = new RecordInfo();
                ri.SampleName = dgv[ColNameSampleName, i].Value.ToString();
                ri.Supplier = dgv[ColNameSupplier, i].Value.ToString();
                if(ri.OtherInfoDictionary == null)
                {
                    ri.OtherInfoDictionary = new HybridDictionary();
                }
                for (int j = 0; j < dgv.ColumnCount; j++)
                {
                    if (excludeColumnNameList.Contains(dgv.Columns[j].Name))
                        continue;
                    ri.OtherInfoDictionary.Add(dgv.Columns[j].Name, dgv[dgv.Columns[j].Name, i].Value.ToString());
                }
                list.Add(ri);
            }
            return true;
        }

        private void FrmBatchTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (WorkCurveHelper.AskToClose && Show(Info.BTSureToExit) != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
            thdExit = true;
            if(evt!= null)
                evt.Set();
            iBatchTest.ActionAfterTestFinished = null;
            if (font != null)
                font.Dispose();
            var v = WorkCurveHelper.NaviItems.Find(nav => nav.Name == "BatchTest");
            if (v != null)
                v.EnabledControl = true;
        }

        public bool ExportToExcel(DataGridView dgv)
        {
            Workbook workbook = new Workbook();
            Cells cells = workbook.Worksheets[0].Cells;

            int k = 0;
            bool hasRecord = false;
            for (int j = k; j < dgv.ColumnCount; j++)
            {
                var col = dgv.Columns[j];
                if (col.Visible)
                {
                    var cell = cells[0, k];
                    cell.PutValue(col.HeaderText);
                    for (int i = 0; i < dgv.RowCount; i++)
                    {
                        cell = cells[i + 1, k];
                        var obj = dgv[j, i].Value;
                        if (obj != null)
                        {
                            var typ = obj.GetType();
                            if (typ == typeof(DateTime))
                            {
                                Aspose.Cells.Style styletemp = cell.GetDisplayStyle();
                                styletemp.Custom = "yyyy-mm-dd hh:mm:ss";
                                cell.SetStyle(styletemp);
                                cell.PutValue(obj.ToString());
                            }
                            else if (typ == typeof(string))
                            {
                                var str = obj.ToString();
                                if (str.IsNum())
                                    cell.PutValue(double.Parse(str));
                                else
                                    cell.PutValue(str);
                            }
                            else
                            {
                                cell.PutValue(obj);
                            }
                            hasRecord = true;
                        }
                    }
                    k++;
                }
            }
            if (hasRecord)
            {
                var sdlg = new SaveFileDialog();
                sdlg.Filter = "Excel File(*.xls)|*.xls";
                if (sdlg.ShowDialog() == DialogResult.OK)
                {
                    workbook.Save(sdlg.FileName);
                }
                return true;
            }
            return false;
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (dgvContent == null || dgvContent.RowCount <= 0)
            {
                ShowError(Info.BTNoDataInDgv);
                return;
            }
             if(ExportToExcel(dgvContent))
             {
                 ShowSuccess(Info.BTSucceedToOutput);
             }
             else
             {
                 ShowError(Info.BTFailedToOutput);
             }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (isBusy)
            {
                ShowWarning(Info.BTUnderBatchTest);
                return;
            }
            dgvContent.Rows.Clear();
        }
    }
}
