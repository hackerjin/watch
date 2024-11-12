using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.UC
{
    public partial class UcInCalibrate : Skyray.Language.UCMultiple
    {
        DataTable dtblIntensity = new DataTable();
        //private int _CalibarateType = 2;
        //public int CalibrateType
        //{
        //    get { return _CalibarateType; }
        //}

        //private string _ConSampleName = string.Empty;
        //public string ConSampleName
        //{
        //    get { return _ConSampleName; }
        //}
        WorkCurve _WorkCurve = null;
        /// <summary>
        /// 无参构造函数中，初始化强度界面
        /// </summary>
        //public UcInCalibrate()
        //{
        //    InitializeComponent();


        //}

        public UcInCalibrate()
        {
            InitializeComponent();
            //_CalibarateType = calibratetype;
            //_ConSampleName = strSampleName;
            dtblIntensity.Columns.Add("Elements", typeof(string));
            dtblIntensity.Columns.Add("OrginalIn", typeof(double));
            dtblIntensity.Columns.Add("CalibrateIn", typeof(double));
            //dtblIntensity.Columns.Add("PeakLeft", typeof(int));
            //dtblIntensity.Columns.Add("PeakRight", typeof(int));
            dtblIntensity.Columns.Add("OriginalBaseIn", typeof(double));
            dtblIntensity.Columns.Add("CalibrateBaseIn", typeof(double));
            dgvIntensitys.DataSource = dtblIntensity;
            dgvIntensitys.Columns[0].HeaderText = Info.ElementName;
            //dgvIntensitys.Columns[0].ReadOnly = _CalibarateType == 1 ? true : false;
            dgvIntensitys.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvIntensitys.Columns[1].HeaderText = Info.OrignalIntensity;
            dgvIntensitys.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvIntensitys.Columns[2].HeaderText = Info.CalibrateIntensity;
            dgvIntensitys.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            //dgvIntensitys.Columns[3].HeaderText = Info.PeakLeft;
            //dgvIntensitys.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            //dgvIntensitys.Columns[4].HeaderText = Info.PeakRight;
            //dgvIntensitys.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvIntensitys.Columns[3].HeaderText = Info.OrignalIntensity;
            dgvIntensitys.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvIntensitys.Columns[4].HeaderText = Info.CalibrateIntensity;
            dgvIntensitys.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            _WorkCurve = null;
            if (WorkCurveHelper.WorkCurveCurrent == null || WorkCurveHelper.WorkCurveCurrent.ElementList == null)
            {
                return;
            }
            _WorkCurve = WorkCurveHelper.WorkCurveCurrent;
            //判断曲线中有无校正强度节点，有则显示
            if (_WorkCurve.ElementList.Items.Count > 0 && _WorkCurve.IntensityCalibration.Count > 0)
            {
                int Num = _WorkCurve.IntensityCalibration.Count;
                for (int i = 0; i < Num; i++)
                {
                    object[] row ={
                                       _WorkCurve.IntensityCalibration[i].Element,
                                       _WorkCurve.IntensityCalibration[i].OriginalIn,
                                       _WorkCurve.IntensityCalibration[i].CalibrateIn,
                                       _WorkCurve.IntensityCalibration[i].OriginalBaseIn,
                                       _WorkCurve.IntensityCalibration[i].CalibrateBaseIn
                                   };
                    dtblIntensity.Rows.Add(row);
                }

            }
            //判断曲线中有无校正强度节点，没有则有元素列表构造
            else if (_WorkCurve.ElementList.Items.Count > 0 && _WorkCurve.IntensityCalibration.Count <= 0)
            {
                int Num = _WorkCurve.ElementList.Items.Count;
                for (int i = 0; i < Num; i++)
                {
                    IntensityCalibration inc = IntensityCalibration.New;
                    inc.Element = _WorkCurve.ElementList.Items[i].Caption;
                    inc.PeakLeft = _WorkCurve.ElementList.Items[i].PeakLow;
                    inc.PeakRight = _WorkCurve.ElementList.Items[i].PeakHigh;
                    inc.CalibrateIn = 0;
                    inc.OriginalIn = 0;
                    inc.OriginalBaseIn = 0;
                    inc.CalibrateBaseIn = 0;
                    inc.InCalType = 1;
                    object[] row ={
                                       inc.Element,
                                       inc.OriginalIn,
                                       inc.OriginalBaseIn,
                                       inc.CalibrateBaseIn
                                   };
                    dtblIntensity.Rows.Add(row);
                    _WorkCurve.IntensityCalibration.Add(inc);
                }
                _WorkCurve.Save();
            }
            string strSample = _WorkCurve.InCalSampName;
            lblHighSample.Text = strSample == null || strSample.Trim() == string.Empty ? Info.HighStandSample : strSample;
            strSample = _WorkCurve.InCalSampNameL;
            lblLowSample.Text = strSample == null || strSample.Trim() == string.Empty ? Info.LowStandSample : strSample;
            this.txtCaliHighT.Leave += new System.EventHandler(this.txtCaliHighT_Leave);
            this.txtCaliLowT.Leave += new System.EventHandler(this.txtCaliHighT_Leave);
            this.txtMeasLowT.Leave += new System.EventHandler(this.txtCaliHighT_Leave);
            this.txtMesuHighT.Leave += new System.EventHandler(this.txtCaliHighT_Leave);
        }
        /// <summary>
        /// 获取样品高标原始强度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrignal_Click(object sender, EventArgs e)
        {
            SpecListEntity temp = null;
            List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false, false);
            if (returnResult == null || returnResult.Count == 0)
                return;
            temp = returnResult[0];
            if (temp != null)
            {
                if (temp.Specs.Length < WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count)
                {
                    Msg.Show(Info.WorkCurveMeasureCondition);
                    return;
                }
                _WorkCurve.CaculateIntensity(temp);
                for (int i = 0; i < dtblIntensity.Rows.Count; i++)
                {
                    string element = dtblIntensity.Rows[i]["Elements"].ToString();
                    double CalInt = Math.Round(_WorkCurve.ElementList.Items.ToList().Find(w => w.Caption == element).Intensity, WorkCurveHelper.SoftWareIntensityBit);
                    dtblIntensity.Rows[i]["OriginalBaseIn"] = CalInt;
                    IntensityCalibration ic = _WorkCurve.IntensityCalibration.ToList().Find(w => w.Element == element);
                    if (ic != null) ic.OriginalBaseIn = CalInt;
                }
            }
            dgvIntensitys_SelectionChanged(null, null);
        }
        /// <summary>
        /// 获取样品高标校正强度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            SpecListEntity temp = null;
            List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false, false);
            if (returnResult == null || returnResult.Count == 0)
                return;
            temp = returnResult[0];
            if (temp != null)
            {
                if (temp.Specs.Length < WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count)
                {
                    Msg.Show(Info.WorkCurveMeasureCondition);
                    return;
                }
                _WorkCurve.CaculateIntensity(temp);
                for (int i = 0; i < dtblIntensity.Rows.Count; i++)
                {
                    string element = dtblIntensity.Rows[i]["Elements"].ToString();
                    double CalInt = Math.Round(_WorkCurve.ElementList.Items.ToList().Find(w => w.Caption == element).Intensity, WorkCurveHelper.SoftWareIntensityBit);
                    dtblIntensity.Rows[i]["CalibrateBaseIn"] = CalInt;
                    IntensityCalibration ic = _WorkCurve.IntensityCalibration.ToList().Find(w => w.Element == element);
                    if (ic != null) ic.CalibrateBaseIn = CalInt;
                }

                lblLowSample.Text = temp.Name;
                _WorkCurve.InCalSampNameL = temp.Name;

            }
            dgvIntensitys_SelectionChanged(null, null);
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            int RowCount = dtblIntensity.Rows.Count;
            for (int i = 0; i < RowCount; i++)
            {
                dtblIntensity.Rows[i]["OrginalIn"] = "0";
                dtblIntensity.Rows[i]["CalibrateIn"] = "0";
            }
            foreach (var en in _WorkCurve.IntensityCalibration)
            {
                en.OriginalIn = 0;
                en.CalibrateIn = 0;
                if (en.InCalType == 2) en.InCalType = 1;
            }
            lblHighSample.Text = Info.HighStandSample;
            _WorkCurve.InCalSampName = string.Empty;
            dgvIntensitys_SelectionChanged(null, null);
        }
        /// <summary>
        /// 取消按钮，放弃操作，返回主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.dialogResult = DialogResult.Cancel;
            EDXRFHelper.GotoMainPage(this);//返回主界面
            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
        }
        /// <summary>
        /// 确定按钮提交数据到数据库，并返回到主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent == null
                || WorkCurveHelper.WorkCurveCurrent.ElementList == null
                || WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count <= 0
                )//||WorkCurveHelper.WorkCurveCurrent.IntensityCalibration==null
            {
                EDXRFHelper.GotoMainPage(this);//返回主界面
                this.dialogResult = DialogResult.Cancel;
                return;
            }
            _WorkCurve.Save();

            //int RowCount = dtblIntensity.Rows.Count;
            //try
            //{
            //    List<IntensityCalibration> lst = WorkCurveHelper.WorkCurveCurrent.IntensityCalibration.ToList();
            //   // WorkCurveHelper.WorkCurveCurrent.IntensityCalibration.Clear();

            //    for (int i = 0; i < RowCount; i++)
            //    {
            //        string ElemName = dtblIntensity.Rows[i]["Elements"].ToString().Trim();
            //        if (ElemName == string.Empty) continue;
            //        IntensityCalibration ic = WorkCurveHelper.WorkCurveCurrent.IntensityCalibration.ToList().Find(w => w.Element == dtblIntensity.Rows[i]["Elements"].ToString());
            //       ic.Element = dtblIntensity.Rows[i]["Elements"].ToString().Trim() ;
            //        ic.OriginalIn = Convert.ToDouble(dtblIntensity.Rows[i]["OrginalIn"]);
            //        ic.CalibrateIn = Convert.ToDouble(dtblIntensity.Rows[i]["CalibrateIn"]);
            //        ic.PeakLeft = 0;
            //        ic.PeakRight = 0;
            //        ic.OriginalBaseIn = Convert.ToDouble(dtblIntensity.Rows[i]["OriginalBaseIn"]);
            //        ic.CalibrateBaseIn = Convert.ToDouble(dtblIntensity.Rows[i]["CalibrateBaseIn"]);
            //        //WorkCurveHelper.WorkCurveCurrent.IntensityCalibration.Add(ic);
            //    }
            //    //Lephone.Data.DbEntry.Context.FastSave(WorkCurveHelper.WorkCurveCurrent.Id,
            //    //  new Lephone.Data.SqlEntry.DataProvider.LineInfo<IntensityCalibration>
            //    //  {
            //    //      Objs = WorkCurveHelper.WorkCurveCurrent.IntensityCalibration
            //    //  },
            //    //   new Lephone.Data.SqlEntry.DataProvider.LineInfo<IntensityCalibration>
            //    //   {
            //    //       IsToDelete = true,
            //    //       Objs = lst
            //    //   });
            //    //WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            //    WorkCurveHelper.WorkCurveCurrent.InCalSampName = lblHighSample.Text != Info.HighStandSample ? lblHighSample.Text : string.Empty;
            //    WorkCurveHelper.WorkCurveCurrent.InCalSampNameL = lblLowSample.Text != Info.LowStandSample ? lblLowSample.Text : string.Empty;
            //    WorkCurveHelper.WorkCurveCurrent.Save();
            //    WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            //    if (RowCount <= 0) this.dialogResult = DialogResult.Cancel;
            //    else this.dialogResult = DialogResult.OK;
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    return;
            //}
            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        private void dgvIntensitys_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvIntensitys.SelectedRows.Count <= 0) return;
            IntensityCalibration ic = _WorkCurve.IntensityCalibration.ToList().Find(w => w.Element == dgvIntensitys.SelectedRows[0].Cells["Elements"].Value.ToString());
            if (ic != null)
            {
                txtSpHightT.Text = _WorkCurve.InCalSampName;
                txtSpLowT.Text =ic.InCalType==2? _WorkCurve.InCalSampNameL:"";
                txtMesuHighT.Text = ic.OriginalIn.ToString();
                txtMeasLowT.Text =ic.InCalType==2? ic.OriginalBaseIn.ToString():"0";
                txtCaliHighT.Text = ic.CalibrateIn.ToString();
                txtCaliLowT.Text = ic.InCalType==2?ic.CalibrateBaseIn.ToString():"0";
                txtCaliLowT.Enabled = txtMeasLowT.Enabled = ic.InCalType == 2;
                if (ic.InCalType == 2) rdobtnA_B.Checked = true;
                else rdobtnA.Checked = true;
                double R=(ic.InCalType==2&&ic.OriginalBaseIn>0?(ic.CalibrateIn-ic.CalibrateBaseIn)/(ic.OriginalIn-ic.OriginalBaseIn):(ic.InCalType==1&&ic.CalibrateIn>0?ic.CalibrateIn/ic.OriginalIn:1));
                txtCoefR.Text = R.ToString("F4");
                txtCoefb.Text = (ic.InCalType == 2) ? (ic.CalibrateIn - ic.OriginalIn * R).ToString("F4") : "0";
       

            }

        }

        private void btnResetLow_Click(object sender, EventArgs e)
        {
            int RowCount = dtblIntensity.Rows.Count;
            for (int i = 0; i < RowCount; i++)
            {
                dtblIntensity.Rows[i]["OriginalBaseIn"] = "0";
                dtblIntensity.Rows[i]["CalibrateBaseIn"] = "0";
            }
            foreach (var en in _WorkCurve.IntensityCalibration)
            {
                en.OriginalBaseIn = 0;
                en.CalibrateBaseIn = 0;
                if (en.InCalType == 2) en.InCalType = 1;
            }
            lblLowSample.Text = Info.LowStandSample;
            _WorkCurve.InCalSampNameL = string.Empty;
            dgvIntensitys_SelectionChanged(null, null);
        }

        private void btnSpHCalibrate_Click(object sender, EventArgs e)
        {
            SpecListEntity temp = null;
            List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false, false);
            if (returnResult == null || returnResult.Count == 0)
                return;
            temp = returnResult[0];
            if (temp != null)
            {
                if (temp.Specs.Length < WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count)
                {
                    Msg.Show(Info.WorkCurveMeasureCondition);
                    return;
                }
                _WorkCurve.CaculateIntensity(temp);
                for (int i = 0; i < dtblIntensity.Rows.Count; i++)
                {
                    string element = dtblIntensity.Rows[i]["Elements"].ToString();
                    double CalInt = Math.Round(_WorkCurve.ElementList.Items.ToList().Find(w => w.Caption == element).Intensity, WorkCurveHelper.SoftWareIntensityBit);
                    dtblIntensity.Rows[i]["CalibrateIn"] = CalInt;
                    IntensityCalibration ic = _WorkCurve.IntensityCalibration.ToList().Find(w => w.Element == element);
                    if (ic != null) ic.CalibrateIn = CalInt;
                }
                lblHighSample.Text = temp.Name;
                _WorkCurve.InCalSampName = temp.Name;
            }
            dgvIntensitys_SelectionChanged(null, null);
        }

        private void btnSpHMeasure_Click(object sender, EventArgs e)
        {
            SpecListEntity temp = null;
            List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false, false);
            if (returnResult == null || returnResult.Count == 0)
                return;
            temp = returnResult[0];
            if (temp != null)
            {
                if (temp.Specs.Length < WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count)
                {
                    Msg.Show(Info.WorkCurveMeasureCondition);
                    return;
                }
                _WorkCurve.CaculateIntensity(temp);
                for (int i = 0; i < dtblIntensity.Rows.Count; i++)
                {
                    string element = dtblIntensity.Rows[i]["Elements"].ToString();
                    double CalInt = Math.Round(_WorkCurve.ElementList.Items.ToList().Find(w => w.Caption == element).Intensity, WorkCurveHelper.SoftWareIntensityBit);
                    dtblIntensity.Rows[i]["OrginalIn"] = CalInt;
                    IntensityCalibration ic = _WorkCurve.IntensityCalibration.ToList().Find(w => w.Element == element);
                    if (ic != null) ic.OriginalIn = CalInt;
                }
            }
             dgvIntensitys_SelectionChanged(null, null);
        }

        private void txtCaliHighT_Leave(object sender, EventArgs e)
        {
            IntensityCalibration ic = _WorkCurve.IntensityCalibration.ToList().Find(w => w.Element == dgvIntensitys.SelectedRows[0].Cells["Elements"].Value.ToString());
            if (ic != null)
            {
                if (((TextBox)sender).Name == txtCaliHighT.Name)
                {
                    ic.CalibrateIn = double.Parse(txtCaliHighT.Text);
                    dgvIntensitys.SelectedRows[0].Cells["CalibrateIn"].Value = ic.CalibrateIn;
                }
                else if (((TextBox)sender).Name == txtCaliLowT.Name)
                {
                    ic.CalibrateBaseIn = double.Parse(txtCaliLowT.Text);
                    dgvIntensitys.SelectedRows[0].Cells["CalibrateBaseIn"].Value = ic.CalibrateBaseIn;
                }
                else if (((TextBox)sender).Name == txtMesuHighT.Name)
                {
                    ic.OriginalIn = double.Parse(txtMesuHighT.Text);
                    dgvIntensitys.SelectedRows[0].Cells["OrginalIn"].Value = ic.OriginalIn;
                }
                else if (((TextBox)sender).Name == txtMeasLowT.Name)
                {
                    ic.OriginalBaseIn = double.Parse(txtMeasLowT.Text);
                    dgvIntensitys.SelectedRows[0].Cells["OriginalBaseIn"].Value = ic.OriginalBaseIn;
                }
            }
        }

        private void rdobtnA_Click(object sender, EventArgs e)
        {
            IntensityCalibration ic = _WorkCurve.IntensityCalibration.ToList().Find(w => w.Element == dgvIntensitys.SelectedRows[0].Cells["Elements"].Value.ToString());
            if (ic == null) return;
            if (rdobtnA.Checked)
            {
                ic.InCalType = 1;
            }
            dgvIntensitys_SelectionChanged(null, null);
        }

        private void rdobtnA_B_Click(object sender, EventArgs e)
        {
            IntensityCalibration ic = _WorkCurve.IntensityCalibration.ToList().Find(w => w.Element == dgvIntensitys.SelectedRows[0].Cells["Elements"].Value.ToString());
            if (ic == null) return;
            if (rdobtnA_B.Checked)
            {
                ic.InCalType = 2;
            }
            dgvIntensitys_SelectionChanged(null, null);
        }

    }
}