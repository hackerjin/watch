using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using Skyray.Language;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.UC
{
    public partial class UCWorkCurveMatch : UCMultiple
    {
        public int FormType = 0;//0,是匹配谱窗体 1，对比谱窗体
        public UCWorkCurveMatch()
        {
            InitializeComponent();
        }

        private void tlsNewMatch_Click(object sender, EventArgs e)
        {
            //if (WorkCurveHelper.SaveType == 0)
            //{
            //    SelectSample ss = new SelectSample(AddSpectrumType.OpenSpectrum);
            //    ss.IsCaculate = false;
            //    WorkCurveHelper.OpenUC(ss, false);
            //    if (ss.dialogResult != DialogResult.OK) return;
            //    ttList = ss.SelectedSpecList;
            //}
            //else
            //{
            //    var returnTT = DifferenceDevice.interClassMain.OpenFileDialog(false);
            //    if (returnTT == null)
            //        return;
            //    ttList.Add(returnTT);
            //}
            List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false,false);
            if (returnResult == null || returnResult.Count == 0)
                return;
            List<SpecListEntity> selectMathSpec = returnResult;
            if (selectMathSpec != null && selectMathSpec.Count > 0)
            {
                if (FormType == 0)
                {
                    foreach (SpecListEntity tt in selectMathSpec)
                    {
                        if (!IsExistsDgv(tt.Name))
                        {
                            this.dgvMatchData.Rows.Add(true, tt.Name);
                        }
                    }
                }
                else
                {
                    this.dgvMatchData.RowCount = 1;
                    this.dgvMatchData.Rows[0].Cells[0].Value = true;
                    this.dgvMatchData.Rows[0].Cells[1].Value = selectMathSpec[0].Name;
                }
            }
        }

        private bool IsExistsDgv(string listName)
        {
            if (this.dgvMatchData.Rows.Count == 0)
                return false;
            foreach (DataGridViewRow rows in this.dgvMatchData.Rows)
            {
                if (listName == rows.Cells["ColMatchName"].Value.ToString())
                    return true;
            }
            return false;
        }
        private string updateSampleId = string.Empty;
        private void tlsRemoveMatch_Click(object sender, EventArgs e)
        {
            if (this.dgvMatchData.SelectedRows != null)
            {
                foreach (DataGridViewRow tt in this.dgvMatchData.SelectedRows)
                {
                    //foreach(var item in WorkCurveHelper.WorkCurveCurrent.ElementList.Items)
                    //{
                    //    StandSample sample = item.Samples.ToList().Find(delegate(StandSample temp) { return temp.MatchSpecListId.ToString() == tt.Cells[ColSpecListId.Name].Value.ToString(); });
                    //    if (sample != null)
                    //    {
                    //            sample.MatchSpecListId = 0;
                    //            sample.IsMatch = false;
                    //            sample.MatchSpecName = "";
                    //            updateSampleId += sample.Id.ToString()+",";
                    //    }
                    //}
                    this.dgvMatchData.Rows.Remove(tt);
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            btnApplication_Click(null, null);
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            EDXRFHelper.GotoMainPage(this);
        }

        private void UCWorkCurveMatch_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;
            this.ColMatch.Visible = FormType == 0;
            //this.ColMatchName.HeaderText = FormType == 0 ? Info.MatchSpec : Info.VirtualSpec;
            this.chBSelectTotal.Visible = btnUp.Visible = FormType == 0;
            if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
            {
                if (FormType == 1)
                {
                    string getRefId = WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr;
                    this.dgvMatchData.Rows.Clear();
                    this.dgvMatchData.Rows.Add(true, getRefId);
                    return;
                }
                string getMatchId = WorkCurveHelper.WorkCurveCurrent.ElementList.MatchSpecListIdStr;
                if (!string.IsNullOrEmpty(getMatchId))
                {
                    string[] str = getMatchId.Split(',');
                    int i = 0;
                    while(i < str.Length)
                    {
                        if (!string.IsNullOrEmpty(str[i]))
                        {
                            bool checkedMatch = str[i + 1] == "1" ? true : false;
                            if (!IsExistsDgv(str[i]))
                                this.dgvMatchData.Rows.Add(checkedMatch, str[i]);
                        }
                        i += 2;
                    }
                }
                //foreach(StandSample sample in WorkCurveHelper.WorkCurveCurrent.ElementList.Items[0].Samples)
                //    if (sample.MatchSpecListId > 0&&!IsExistsDgv(sample.MatchSpecListId))
                //    {
                //            this.dgvMatchData.Rows.Add(sample.MatchSpecListId, sample.IsMatch, SpecList.FindById(sample.MatchSpecListId).Name);
                //    }
                DataGridViewCellEventArgs dgvce = new DataGridViewCellEventArgs(ColMatch.Index, 0);
                dgvMatchData_CellContentClick(null, dgvce);
            }
            
        }

        private void btnApplication_Click(object sender, EventArgs e)
        {
            if (FormType == 1)
            {
                string RefTemp = string.Empty;
                if (this.dgvMatchData.Rows.Count > 0)
                {
                    RefTemp = this.dgvMatchData.Rows[0].Cells["ColMatchName"].Value.ToString();
                }
                string sqlR = "Update ElementList Set RefSpecListIdStr = '" + RefTemp + "' Where Id = " + WorkCurveHelper.WorkCurveCurrent.ElementList.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlR);
                WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
                DifferenceDevice.interClassMain.ReloadVirtualByWorkCurve();
                return;
            }

            string matchTemp = string.Empty;
            if (this.dgvMatchData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in this.dgvMatchData.Rows)
                {
                    string checkedInt = bool.Parse(row.Cells["ColMatch"].Value.ToString()) ? "1" : "0";
                    matchTemp += row.Cells["ColMatchName"].Value.ToString() + "," + checkedInt + ",";
                }
                matchTemp = matchTemp.Substring(0, matchTemp.Length - 1);
            }
            else
                matchTemp = "";
            //string sqlUpdateSample = "Update StandSample Set IsMatch = 0 , MatchSpecListId  = 0,MatchSpecName = '' Where id in (" + updateSampleId.TrimEnd(',') + ")";
            //Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlUpdateSample);
            //WorkCurveHelper.WorkCurveCurrent.ElementList.Save();
            string sql = "Update ElementList Set MatchSpecListIdStr = '" + matchTemp + "' Where Id = " + WorkCurveHelper.WorkCurveCurrent.ElementList.Id;
            Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
        }

        private bool upflag = false;

        private void btnUp_Click(object sender, EventArgs e)
        {
            btnSubmit_Click(null, null);
            upflag = true;
        }

        public override void ExcuteEndProcess(params object[] objs)
        {
            if (upflag)
            {
                ToolStripControls test = MenuLoadHelper.MenuStripCollection.Find(w => w.CurrentNaviItem.Name == "EditData");
                EDXRFHelper.RecurseUpUC(test);
            }
        }
        
        private void dgvMatchData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0||dgvMatchData.RowCount<=0||dgvMatchData.ColumnCount<=2)
                return;
            if (e.ColumnIndex == ColMatch.Index)
            {
                DataGridViewCell cell = dgvMatchData.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.EditedFormattedValue.ToString() == false.ToString())
                    this.chBSelectTotal.Checked = false;
                else
                {
                    bool flag = true;
                    foreach (DataGridViewRow row in dgvMatchData.Rows)
                    {
                        if (e.RowIndex != row.Index)
                        {
                            flag = flag && bool.Parse(row.Cells[ColMatch.Name].Value.ToString());
                        }
                    }
                        this.chBSelectTotal.Checked = flag;
                }
            }
        }

        private void chBSelectTotal_Click(object sender, EventArgs e)
        {
            if (this.dgvMatchData.Rows.Count == 0)
                return;
            foreach (DataGridViewRow row in this.dgvMatchData.Rows)
            {
                row.Cells["ColMatch"].Value = chBSelectTotal.Checked;
            }
        }

        private void dgvMatchData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            dgvMatchData_CellContentClick(null,new DataGridViewCellEventArgs(e.ColumnIndex,e.RowIndex));            
        }
    }
}
