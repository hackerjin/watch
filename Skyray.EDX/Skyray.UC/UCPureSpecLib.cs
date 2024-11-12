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
using Skyray.Controls;
using System.Xml;

using Aspose.Cells;
namespace Skyray.UC
{
    public partial class UCPureSpecLib :UCMultiple
    {
        private List<PureSpecParam> ElemList = new List<PureSpecParam>();
        private DataTable DtAll = new DataTable();
        private string curElem=string.Empty;
        public UCPureSpecLib()
        {
            InitializeComponent();
           // string sql = "select * from PureSpecParam where DeviceName ='" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "' group by samplename";
            LoadDB();
        }

        private void LoadDB()
        {
            if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.Condition != null)
            {
                string sql = "select * from PureSpecParam where DeviceName ='" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "' group by ElementName";
                ElemList = PureSpecParam.FindBySql(sql);
                 
            }
        }

        public void InitElem()
        {
            if (ElemList != null && ElemList.Count > 0)
            {
                dgvwElement.Rows.Clear();
                dgvwElement.Rows.Add(ElemList.Count);

                for (int i = 0; i < ElemList.Count; i++)
                {
                    dgvwElement[0, i].Value = ElemList[i].SampleName;
                    double[] coeff = new double[4];
                    CalcCurve(ref coeff, ElemList[i].SampleName);
                   // dgvwElement[1, i].Value = coeff[0].ToString("f6") + "*x*x" + (coeff[1] >= 0 ? "+" : "") + coeff[1].ToString("f6") + "*x" + (coeff[2] >= 0 ? "+" : "") + coeff[2].ToString("f6");
                    ShowCalc(coeff, i);
                }
                foreach (CurveElement ce in WorkCurveHelper.WorkCurveCurrent.ElementList.Items)
                {
                    if (ElemList.Find(w => w.SampleName == ce.Caption) ==null)
                    {
                        dgvwElement.Rows.Add();
                        dgvwElement[0, dgvwElement.Rows.Count - 1].Value = ce.Caption;
                    }
                }
                dgvwElement.Rows[0].Selected = true;
            }
            else
            {
                if (WorkCurveHelper.WorkCurveCurrent.ElementList != null)
                {
                    if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
                    {
                        dgvwElement.Rows.Clear();
                        dgvwElement.Rows.Add(WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count);
                        for (int i = 0; i < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; i++)
                        {
                            dgvwElement[0, i].Value = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Caption;
                        }
                        dgvwElement.Rows[0].Selected = true;

                    }
                }
            }
        }

        /// <summary>
        /// 计算公式
        /// </summary>
        private void CalcFormula()
        {
            if (WorkCurveHelper.PureCalcType == 2)
            { 
            
            }
            else if (WorkCurveHelper.PureCalcType == 3)
            { 
            
            }
        }



        private void InitPurSpecParam(string elemName)
        {
            this.dgvPureData.Rows.Clear();
            this.dgvPureData.DataSource = null;
            BindingSource bs = new BindingSource();
           // string sql = "select * from PureSpecParam where DeviceName ='" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "' and samplename ='" + elemName + "'";
            string sql = "select * from PureSpecParam where DeviceName ='" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "' and ElementName ='" + elemName + "'";
            List<PureSpecParam> pureList = PureSpecParam.FindBySql(sql);
            
            foreach (var temp in pureList)
            {
                bs.Add(temp);
            }
            this.dgvPureData.AutoGenerateColumns = false;
            this.dgvPureData.DataSource = bs;//绑定数据源

        }

        private void dgvwElement_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
           
            InitPurSpecParam(dgvwElement[0, e.RowIndex].Value.ToString());
        }

        private void UCPureSpecLib_Load(object sender, EventArgs e)
        {
            if (WorkCurveHelper.PureCalcType == 1)
                radioInsert.Checked = true;
            else if (WorkCurveHelper.PureCalcType == 2)
                radioConic.Checked = true;
            else
                radioThird.Checked = true;
           
            InitElem();
            if (ElemList == null || ElemList.Count <= 0) return;
            InitPurSpecParam(ElemList[0].SampleName);
            chkShowPure.Checked = WorkCurveHelper.IsShowPureSpec;
        }

        private void btbDel_Click(object sender, EventArgs e)
        {
            if (dgvwElement.Rows.Count <= 0 || dgvPureData.Rows.Count <= 0) return;
            if (dgvPureData.SelectedRows.Count <= 0) return;

           // string sql = "delete * from PureSpecParam where DeviceName ='" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "' and name='" + dgvPureData[0, dgvPureData.CurrentRow.Index].ToString()+"'";
            PureSpecParam.DeleteAll(w => w.Name == dgvPureData[0, dgvPureData.CurrentRow.Index].Value.ToString());
            InitPurSpecParam(dgvwElement[0, dgvwElement.CurrentRow.Index].Value.ToString());

            if (dgvPureData.Rows.Count < 3)
            {
                SkyrayMsgBox.Show(Info.NeedConicPoint);
                return;
            }
            double[] coeff = new double[4];
            CalcCurve(ref coeff, dgvwElement[0, dgvPureData.CurrentRow.Index].Value.ToString());
           // dgvwElement[1, dgvwElement.CurrentRow.Index].Value = coeff[0].ToString("f6") + "*x*x" + (coeff[1] >= 0 ? "+" : "") + coeff[1].ToString("f6") + "*x" + (coeff[2] >= 0 ? "+" : "") + coeff[2].ToString("f6");
            ShowCalc(coeff,dgvwElement.CurrentRow.Index);

        }

        private void radioInsert_CheckedChanged(object sender, EventArgs e)
        {
            if (radioInsert.Checked)
            {
                WorkCurveHelper.PureCalcType = 1;
                radioConic.Checked = false;
                radioThird.Checked = false;
            }

        }

        private void radioConic_CheckedChanged(object sender, EventArgs e)
        {
            if (radioConic.Checked)
            {
                WorkCurveHelper.PureCalcType = 2;
                radioInsert.Checked = false;
                radioThird.Checked = false;
            }
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            WorkCurveHelper.IsShowPureSpec = chkShowPure.Checked;
            ReportTemplateHelper.SaveSpecifiedValueandCreate("EncoderCoeff", "CalcType", WorkCurveHelper.PureCalcType.ToString());
        }

        /// <summary>
        /// 计算线性关系
        /// </summary>
        private void CalcCurve(ref double[] coeff, string elemName)
        {
            string sql = "select * from PureSpecParam where DeviceName ='" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "' and ElementName ='" + elemName + "'";
            List<PureSpecParam> pureList = PureSpecParam.FindBySql(sql);
            if (WorkCurveHelper.PureCalcType == 3)
            {
                if (pureList.Count < 4)
                {
                    SkyrayMsgBox.Show(Info.NeedThirdPoint);
                    return;
                }
            }
            else
            {
                if (pureList.Count < 3)
                {
                    SkyrayMsgBox.Show(Info.NeedConicPoint);
                    return;
                }
            }
            List<PointF> lstActivePoint = new List<PointF>();
            for (int i = 0; i < pureList.Count; i++)
            {
                PointF tempPoint = new PointF((float)pureList[i].Height, (float)pureList[i].TotalCount);
                lstActivePoint.Add(tempPoint);
            }
            var pp = lstActivePoint.Distinct().ToArray();
            if(WorkCurveHelper.PureCalcType ==3)
                DifferenceDevice.CalculateCurve(pp, 3, false, coeff);
            else
                DifferenceDevice.CalculateCurve(pp, 2, false, coeff);

           
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            //if (dgvwElement.Rows.Count <= 0)
            //{
            //    Msg.Show(Info.NeedSpecs);
            //    return;
            //}
            List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false, false);
            if (returnResult == null || returnResult.Count == 0)
                return;
            foreach (SpecListEntity specList in returnResult)
            {
                PureSpecParam purr = PureSpecParam.New;
                // PureSpecParam pur = PureSpecParam.New.Init(specList.Name, specList.Height, specList.DeviceName,
                //specList.TotalCount, specList.WorkCurveName, specList.SpecType, specList.SampleName, obj, specList.SpecDate);
                purr.Name = specList.Name;
                purr.Height = specList.Height;
                purr.DeviceName = specList.DeviceName;
                //if (WorkCurveHelper.IsPureElemCurrentUnify)
                //    purr.TotalCount = specList.CountRate / specList.ActualCurrent; //计数率
                //else
                purr.TotalCount = specList.CountRate;
                purr.CurrentUnifyCount = specList.CountRate / specList.ActualCurrent; //除管流计数率
                purr.WorkCurveName = specList.WorkCurveName;
                purr.SpecTypeValue = specList.SpecType;
                purr.SampleName = dgvwElement[0, dgvwElement.CurrentRow.Index].Value.ToString();
                purr.SpecDate = specList.SpecDate;
                purr.UsedTime = specList.Specs[0].UsedTime;
                purr.Condition = WorkCurveHelper.WorkCurveCurrent.Condition;
                purr.ElementName = dgvwElement[0, dgvwElement.CurrentRow.Index].Value.ToString();
                purr.Current = specList.ActualCurrent;
                purr.Save();
            }
            if (dgvwElement.CurrentRow.Index >= 0)
                InitPurSpecParam(dgvwElement[0, dgvwElement.CurrentRow.Index].Value.ToString());

            if (dgvPureData.Rows.Count < 3)
            {
                SkyrayMsgBox.Show(Info.NeedConicPoint);
                return;
            }
            //重新计算
            double[] coeff = new double[4];
            CalcCurve(ref coeff, dgvwElement[0, dgvwElement.CurrentRow.Index].Value.ToString());

            //dgvwElement[1, dgvwElement.CurrentRow.Index].Value = coeff[0].ToString("f6") + "*x*x" + (coeff[1] >= 0 ? "+" : "") + coeff[1].ToString("f6") + "*x" + (coeff[2] >= 0 ? "+" : "") + coeff[2].ToString("f6");
            ShowCalc(coeff, dgvwElement.CurrentRow.Index);

        }

        private void ShowCalc(double[] coeff , int rowindex)
        {
            string x2 = "x"+Convert.ToChar(0x00b2);
            string x3 = "x" + Convert.ToChar(0x00b3);

            if (WorkCurveHelper.PureCalcType == 3)
            {
                dgvwElement[1, rowindex].Value = coeff[0].ToString("f6") + x3 + (coeff[1] >= 0 ? "+" : "") + coeff[1].ToString("f6") + x2 + (coeff[2] >= 0 ? "+" : "") + coeff[2].ToString("f6") + "x" + (coeff[3] >= 0 ? "+" : "") + coeff[3].ToString("f6");
            }
            else
                dgvwElement[1, rowindex].Value = coeff[1].ToString("f6") + x2 + (coeff[2] >= 0 ? "+" : "") + coeff[2].ToString("f6") + "x" + (coeff[3] >= 0 ? "+" : "") + coeff[3].ToString("f6");

        }

        private void radioThird_CheckedChanged(object sender, EventArgs e)
        {
            if (radioThird.Checked)
            {
                WorkCurveHelper.PureCalcType = 3;
                radioInsert.Checked = false;
                radioConic.Checked = false;
            }
        }

        private void btnAdjust_Click(object sender, EventArgs e)
        {

        }

        private void btnOutPut_Click(object sender, EventArgs e)
        {
            if (dgvwElement.Rows.Count <= 0) return;
            try
            {

                if (DialogResult.OK == this.saveFileDialog.ShowDialog())
                {
                    string sql = "select * from PureSpecParam where DeviceName ='" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "';";
                    List<PureSpecParam> lstAll = PureSpecParam.FindBySql(sql);
                    string fileName = this.saveFileDialog.FileName;
                    saveFileDialog.Filter = "Excel File(*.xls)|*.xls";
                    Workbook workbook = new Workbook();
                    Worksheet ws = workbook.Worksheets[0];
                    ws.Name = "pureLib";
                    var properties = typeof(PureSpecParam).GetProperties();
                    int columns = 0;

                    ws.Cells[0, 0].Value = "Name";
                    ws.Cells[0, 1].Value = "Height";
                    ws.Cells[0, 2].Value = "DeviceName";
                    ws.Cells[0, 3].Value = "TotalCount";
                    ws.Cells[0, 4].Value = "WorkCurveName";
                    ws.Cells[0, 5].Value = "SpecTypeValue";
                    ws.Cells[0, 6].Value = "SampleName";
                    ws.Cells[0, 7].Value = "SpecDate";
                    ws.Cells[0, 8].Value = "UsedTime";
                    ws.Cells[0, 9].Value = "Condition.Id";
                    ws.Cells[0, 10].Value = "ElementName";
                    ws.Cells[0, 11].Value = "Current";
                    ws.Cells[0, 12].Value = "CurrentUnifyCount";
                    for (int i = 0; i < lstAll.Count; i++)
                    {
                       
                        ws.Cells[i+1, 0].Value = lstAll[i].Name;
                        ws.Cells[i + 1, 1].Value = lstAll[i].Height;
                        ws.Cells[i + 1, 2].Value = lstAll[i].DeviceName;
                        ws.Cells[i + 1, 3].Value = lstAll[i].TotalCount;
                        ws.Cells[i + 1, 4].Value = lstAll[i].WorkCurveName;
                        ws.Cells[i + 1, 5].Value = lstAll[i].SpecTypeValue;
                        ws.Cells[i + 1, 6].Value = lstAll[i].SampleName;
                        ws.Cells[i + 1, 7].Value = lstAll[i].SpecDate.ToString();
                        ws.Cells[i + 1, 8].Value = lstAll[i].UsedTime;
                        ws.Cells[i + 1, 9].Value = lstAll[i].Condition.Id;
                        ws.Cells[i + 1, 10].Value = lstAll[i].ElementName;
                        ws.Cells[i + 1, 11].Value = lstAll[i].Current;
                        ws.Cells[i + 1, 12].Value = lstAll[i].CurrentUnifyCount;
                    }
                    if (workbook != null)
                    {
                        workbook.SetEncryptionOptions(EncryptionType.XOR, 40);// Specify Strong Encryption type (RC4,Microsoft Strong Cryptographic Provider)
                        workbook.SetEncryptionOptions(EncryptionType.StrongCryptographicProvider, 128);// Password protect the file
                        workbook.Settings.Password = "thickinSkyray";// Save the excel file

                        workbook.Save(fileName);
                        SkyrayMsgBox.Show(Info.GradeFileExportSuccess, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                    }
                }
            }
            catch
            { }
        }

        private void dgvPureData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnImput_Click(object sender, EventArgs e)
        {
            try
            {
              
                System.Windows.Forms.OpenFileDialog fd = new OpenFileDialog();
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    PureSpecParam.FindBySql("delete from PureSpecParam where DeviceName = '" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "';");

                    string fileName = fd.FileName;
                    Workbook wb = new Workbook(FileFormatType.Excel97To2003);
                    wb.Settings.Password = "thickinSkyray";
                    wb.Open(fileName);
                    Worksheet ws = wb.Worksheets[0];
                    DataTable dt =  ws.Cells.ExportDataTable(0, 0, ws.Cells.MaxRow+1, ws.Cells.MaxColumn+1,true);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            PureSpecParam purr = PureSpecParam.New;
                           
                            // PureSpecParam pur = PureSpecParam.New.Init(specList.Name, specList.Height, specList.DeviceName,
                            //specList.TotalCount, specList.WorkCurveName, specList.SpecType, specList.SampleName, obj, specList.SpecDate);
                            purr.Name = dr["Name"].ToString();
                            purr.Height = Convert.ToDouble(dr["Height"].ToString());
                            purr.DeviceName = dr["DeviceName"].ToString();
                            //if (WorkCurveHelper.IsPureElemCurrentUnify)
                            //    purr.TotalCount = specList.CountRate / specList.ActualCurrent; //计数率
                            //else
                            purr.TotalCount = Convert.ToDouble(dr["TotalCount"].ToString());
                            purr.WorkCurveName = dr["WorkCurveName"].ToString();
                            purr.SpecTypeValue = (SpecType)Convert.ToInt32(dr["SpecTypeValue"].ToString());
                            purr.SampleName = dr["SampleName"].ToString();
                            purr.SpecDate = Convert.ToDateTime(dr["SpecDate"].ToString());
                            purr.UsedTime = Convert.ToDouble(dr["UsedTime"].ToString());
                            purr.Condition = WorkCurveHelper.WorkCurveCurrent.Condition;
                            purr.ElementName = dr["ElementName"].ToString();
                            purr.Current = Convert.ToDouble(dr["Current"].ToString());
                            purr.CurrentUnifyCount = Convert.ToDouble(dr["CurrentUnifyCount"].ToString());
                            purr.Save();
                        }
                        LoadDB();
                        UCPureSpecLib_Load(null, null);
                    }


                }  

               
            }
            catch
            { 
                
            }
        }


     


        private void dgvwElement_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvwElement.Columns[e.ColumnIndex].Name == "ColElement")
            {
                dgvwElement.Columns["ColElement"].ReadOnly = false;
            }

        }

        private void dgvwElement_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dgvwElement_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            if (dgvwElement.Columns[e.ColumnIndex].Name == "ColElement")
            {
                Atom ato = Atoms.AtomList.Find(s => s.AtomName == dgvwElement[e.ColumnIndex, e.RowIndex].Value.ToString());
                if (ato == null)
                {
                    Msg.Show(Info.NotInAtoms);
                    dgvwElement[e.ColumnIndex, e.RowIndex].Value = curElem;
                }
                else
                { 
                    
                    string sql = "update PureSpecParam  set SampleName = '" + ato.AtomName + "' , ElementName = '" +  ato.AtomName + "' where  SampleName ='" + curElem + "' and DeviceName =  '" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "';";
                    PureSpecParam.FindBySql(sql);
                    InitPurSpecParam(ato.AtomName);
                }
                dgvwElement.Columns[e.ColumnIndex].ReadOnly = true;
            }
        }

        private void dgvwElement_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvwElement_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            curElem = dgvwElement[0, e.RowIndex].Value.ToString();
        }



     
    }

    public class DefineElem
    {
        private string Name;
        private string Expression;
    }
}
