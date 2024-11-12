using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common;
using Lephone.Data;
using System.Data.SQLite;
using Skyray.Controls;
using Skyray.EDXRFLibrary;
using System.IO;
using Lephone.Data.SqlEntry;
using System.Threading;

namespace Skyray.UC
{
    public partial class UCThinDatabase : Skyray.Language.UCMultiple
    {
        private string _beginTime = string.Empty;
        private string _endTime = string.Empty;
        private string _beforeTime = string.Empty;
        private TimeType _timeType = TimeType.Before;
        private string _tipStr = string.Empty;
        private BackgroundWorker _worker = new BackgroundWorker();
        private Action _action;
        private List<int> _lstNumbers = new List<int>();

        public UCThinDatabase()
        {
            InitializeComponent();
            InitializeParams();
        }

        public UCThinDatabase(int type) : this()
        {
            btnStartToClear.Visible = false;
            grpSelectTimePeriod.Visible = false;
            pnlInfo.Location = new Point((int)((this.Width - pnlInfo.Width) / 2f), (int)((this.Height - pnlInfo.Height) / 2f));
        }

        private void InitializeParams()
        {
            this.Load += (s, e) =>
            {
                if (this.Parent != null && this.Parent is Form)
                {
                    Form frm = this.Parent as Form;
                    frm.FormClosing -= UCThinDatabase_FormClosing;
                    frm.FormClosing += UCThinDatabase_FormClosing;
                }
            };
            
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += new DoWorkEventHandler(_worker_DoWork);
            _worker.ProgressChanged += new ProgressChangedEventHandler(_worker_ProgressChanged);
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_worker_RunWorkerCompleted);
        }

        void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                ShowText(Info.Canceled, true);
                Msg.Show(Info.Canceled);
            }
            else
            {
                ShowText(Info.Done, true);
                Msg.Show(Info.Done);
            }
        }

        void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_action != null)
            {
                _action.Invoke();
                if (_worker.CancellationPending)
                    e.Cancel = true;
            }
        }

        void UCThinDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_worker != null && _worker.IsBusy)
            {
                e.Cancel = true;
                Msg.Show(Info.UnderProgressPlsWait);
            }
        }

        public void Execute(Action action)
        {
            if (action == null)
                return;
            if (_worker.IsBusy)
            {
                Msg.Show(Info.UnderProgressPlsWait);
                return;
            }
            _action = action;
            _worker.RunWorkerAsync();
        }

        public BackgroundWorker GetClearWorker()
        {
            return _worker;
        }

        public void SetCount(decimal num, decimal total)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    lblCurrentAndTotal.Text = num.ToString() + "/" + total.ToString();
                }));
            }
            else
            {
                lblCurrentAndTotal.Text = num.ToString() + "/" + total.ToString();
            }
        }

        private void btnStartToClear_Click(object sender, EventArgs e)
        {
            if (_worker != null && _worker.IsBusy)
            {
                Msg.Show(Info.UnderProgressPlsWait);
                return;
            }
            DialogResult dr = Msg.Show(Info.ConfirmDel, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dr != DialogResult.OK) return;
            Execute(new Action(() =>
            {
                //获取用户输入并初始化变量
                GetUserInput();
                //显示正在处理
                ShowText(Info.UnderProgress, true);
                //清理历史记录
                ClearHistory();
                //清理谱
                SelectSample.ClearSpec(string.Empty, _beginTime, _endTime, string.Empty, string.Empty, 1000, _worker, SetCount);
                //压缩数据库
                ShowText(Info.UnderReleaseSpace, false);
                ReleaseSpace();
                //显示已完成
                //ShowText(_worker.CancellationPending? Info.Canceled : Info.Done, true);

            }));
        }

        /// <summary>
        /// 删除记录后压缩数据库空间，否则文件大小不变
        /// </summary>
        /// <param name="sql"></param>
        public static void ReleaseSpace()
        {
            string sql = "VACUUM";
            string connStr = Lephone.Data.DbEntry.Context.Driver.ConnectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ShowText(string prefix, bool showTip)
        {
            this.Invoke(new Action(() =>
            {
                lblClearTip.Text = prefix + (showTip? ("  " + _tipStr):string.Empty);
            }));

        }

        public static void ShowText(UCThinDatabase td, string content)
        {
            td.Invoke(new Action(() =>
            {
                td.lblClearTip.Text = content;
            }));

        }

        private void GetUserInput()
        {
            this.Invoke(new Action(() =>
            {
                _beginTime = _endTime = _beforeTime = _tipStr = string.Empty;
                _timeType = TimeType.Before;
                progressBar1.Value = 0;
                progressBar1.Visible = true;
                if (radBeforeOneMonth.Checked)
                {
                    _beforeTime = _endTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    _tipStr = radBeforeOneMonth.Text + " < " + _beforeTime;
                }
                else if (radBefore15Days.Checked)
                {
                    _beforeTime = _endTime = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
                    _tipStr = radBefore15Days.Text + " < " + _beforeTime;
                }
                else if (radBeforeCustomTime.Checked)
                {
                    _beforeTime = _endTime = dateTimePickerBefore.Value != null ? dateTimePickerBefore.Value.ToString("yyyy-MM-dd") : string.Empty;
                    _tipStr = radBeforeCustomTime.Text + " < " + _beforeTime;
                }
                else if (radCustomTimePeriod.Checked)
                {
                    _timeType = TimeType.Period;
                    _beginTime = dateTimePickerFrom.Value != null ? dateTimePickerFrom.Value.ToString("yyyy-MM-dd") : string.Empty;
                    _endTime = dateTimePickerTo.Value != null && dateTimePickerTo.Value >= dateTimePickerFrom.Value ? dateTimePickerTo.Value.ToString("yyyy-MM-dd") : _beginTime;
                    _tipStr = radCustomTimePeriod.Text + " >= " + _beginTime + ", <= " + _endTime;
                }
                else if (radAllTime.Checked)
                {
                    _timeType = TimeType.All;
                    _tipStr = radAllTime.Text;
                }
            }));

        }

        private void ClearHistory()
        {
            string timeSql = string.Empty;
            if (_timeType == TimeType.Before)
            {
                timeSql += " and datetime(specdate) < datetime('" + _beforeTime + " 00:00:00') ";
            }
            else if (_timeType == TimeType.Period)
            {
                timeSql += " and datetime(specdate) <=datetime('" + _endTime + " 23:59:59')" + " and datetime(specdate)>=datetime('" + _beginTime + " 00:00:00') ";
            }

            string strsql = " and workcurveid in (select id from workcurve where condition_id in ( " +
       " select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))";

            SqlStatement sqlstate = new SqlStatement("delete from historycompanyotherinfo where history_id in (select id from  historyrecord where 1=1 " + strsql + timeSql + ");  " +
            " delete from  historyElement where historyrecord_id in ( " +
                         " select id from  historyrecord where  1=1 " + strsql + timeSql + ");" +
            " delete from  historyrecord where  1=1 " + strsql + timeSql + "");
            Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlstate);
        }

        private void ClearSpec()
        {
            List<SpecListEntity> lstGetSpecList = new List<SpecListEntity>();
            if (_timeType == TimeType.Period)
            {
                lstGetSpecList = WorkCurveHelper.DataAccess.ResearchByConditions(string.Empty, _beginTime, _endTime, string.Empty, "asc");
            }
            else if (_timeType == TimeType.All)
            {
                lstGetSpecList = WorkCurveHelper.DataAccess.GetAllSpectrum();
            }
            else if (_timeType == TimeType.Before)
            {
                lstGetSpecList = WorkCurveHelper.DataAccess.ResearchByConditions(string.Empty, _beginTime, _beforeTime, string.Empty, "asc");
            }

            string mathStr = string.Empty;
            string SearchConditionsElemListSql = "Select* from ElementList a,WorkCurve b,Condition c,Device d Where a.WorkCurve_Id=b.Id And b.Condition_Id=c.Id And c.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id;
            List<ElementList> listElementList = ElementList.FindBySql(SearchConditionsElemListSql);
            foreach (var mathElementList in listElementList)
            {
                if (string.IsNullOrEmpty(mathElementList.MatchSpecListIdStr))
                    continue;
                string[] strs = mathElementList.MatchSpecListIdStr.Split(',');
                for (int i = 0; i < strs.Length; i++)
                    if (i % 2 == 0)
                        mathStr += strs[i] + ",";
            }
            string[] mathArr = mathStr.Split(',');

            foreach (var spd in lstGetSpecList)
            {
                if (spd.SpecType == SpecType.UnKownSpec || spd.SpecType == SpecType.UnSelected)
                {
                    if (!mathArr.Contains(spd.Name) && DifferenceDevice.interClassMain.selectSpeclist.Find(w => w.Name == spd.Name) == null)
                    {
                        if (WorkCurveHelper.VirtualSpecList != null && WorkCurveHelper.VirtualSpecList.Count > 0)
                        {
                            var tempList = WorkCurveHelper.VirtualSpecList.Find(s => s.Name == spd.Name);
                            if (tempList != null) WorkCurveHelper.VirtualSpecList.Remove(tempList);
                        }
                        FileInfo file = new FileInfo(WorkCurveHelper.SaveSamplePath + "\\" + spd.Name + ".jpg");
                        if (file.Exists)
                            file.Delete();
                        file = new FileInfo(WorkCurveHelper.SaveGraphicPath + "\\" + spd.Name + ".jpg");
                        if (file.Exists)
                            file.Delete();
                        WorkCurveHelper.DataAccess.DeleteRecord(spd.Name);
                    }
                }
            }
        }

        private void radBeforeCustomTime_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerBefore.Visible = radBeforeCustomTime.Checked;
        }

        private void radCustomTimePeriod_CheckedChanged(object sender, EventArgs e)
        {
            pnlPeriod.Visible = radCustomTimePeriod.Checked;
        }

        private void btnCancelClear_Click(object sender, EventArgs e)
        {
            if(_worker != null)
                _worker.CancelAsync();
            //lblClearTip.Text = Info.Canceled;
        }


    }

    public enum TimeType
    {
        Before,
        Period,
        All
    }

    public enum ResultType
    {
        Cancel = 0,
        Success,
        Error
    }
}
