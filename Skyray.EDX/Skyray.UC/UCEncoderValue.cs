using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;
using Skyray.Controls;
using Skyray.EDX.Common;

using Lephone.Data.Common;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    public partial class UCEncoderValue : Skyray.Language.UCMultiple
    {
       
        public List<string> Sels = null;
        
        /// <summary>
        /// 数据库中查询所得的信息列表
        /// </summary>
        DbObjectList<EncodeXY> lstEncoder;

        public UCEncoderValue()
        {
            InitializeComponent();
            chkOpenorClose.Checked = WorkCurveHelper.isShowEncoder;
           



        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InstanceDGV()
        {
            BindingSource bs = new BindingSource();
            foreach (EncodeXY exy in lstEncoder)
            {
                bs.Add(exy);
            }
            dgvEncoder.AutoGenerateColumns = false;
            dgvEncoder.DataSource = bs;//绑定数据源
        }


        private void InitData()
        {
            //if (WorkCurveHelper.EncoderPoint1.Split(',').Length > 1)
            //{
            //    this.txtX1.Text = WorkCurveHelper.EncoderPoint1.Split(',')[0];
            //    this.txtY1.Text = WorkCurveHelper.EncoderPoint1.Split(',')[1];
            //}
            //else
            //{
            //    this.txtX1.Text = "0";
            //    this.txtY1.Text = "0";
                
            //}
            //if (WorkCurveHelper.EncoderPoint1.Split(',').Length > 1)
            //{
            //    this.txtX2.Text = WorkCurveHelper.EncoderPoint2.Split(',')[0];
            //    this.txtY2.Text = WorkCurveHelper.EncoderPoint2.Split(',')[1];
            //}
            //else
            //{
            //    this.txtX2.Text = "4095";
            //    this.txtY2.Text = "4095";
            //}
            //this.txtFormula.Text = WorkCurveHelper.EncoderFormula;
        }

        private void UCEncoderValue_Load(object sender, EventArgs e)
        {
            lstEncoder = EncodeXY.FindAll();
            InstanceDGV();
            this.txtFormula.Text = WorkCurveHelper.DeviceCurrent.EncoderFormula;
            if (WorkCurveHelper.EncoderCalcWay == 0)
                radTwoNoForcedOrigin.Checked = true;
            else
                radTwoForcedOrigin.Checked = true;
        }

        private void btncalc_Click(object sender, EventArgs e)
        {
            bool iszero = false;
            if (WorkCurveHelper.EncoderCalcWay == 1)
            {
                iszero = true;
            }
            double[] coeff = new double[3];
            CalcCurve(ref coeff, iszero);

            if (!iszero)
                this.txtFormula.Text = coeff[0].ToString("f8") + "*x*x" + (coeff[1] >= 0 ? "+" : "") + coeff[1].ToString("f8") + "*x" + (coeff[2] >= 0 ? "+" : "") + coeff[2].ToString("f8");
            else
                this.txtFormula.Text = coeff[1].ToString("f8") + "*x*x" + (coeff[0] >= 0 ? "+" : "") + coeff[0].ToString("f8") + "*x";
           

        }

        /// <summary>
        /// 计算线性关系
        /// </summary>
        private void CalcCurve(ref double[] coeff ,bool isZero)
        { 
             if (lstEncoder.Count < 3)
            {
                SkyrayMsgBox.Show(Info.NeedConicPoint);
                return;
            }
            List<PointF> lstActivePoint = new List<PointF>();
            for (int i = 0; i < lstEncoder.Count; i++)
            {
                PointF tempPoint = new PointF((float)lstEncoder[i].X, (float)lstEncoder[i].Y);
                lstActivePoint.Add(tempPoint);
            }
            var pp = lstActivePoint.Distinct().ToArray();
            DifferenceDevice.CalculateCurve(pp, 2, isZero, coeff);
   
        }


      

      

        private void btnOK_Click(object sender, EventArgs e)
        {

            WorkCurveHelper.isShowEncoder = chkOpenorClose.Checked;
            

            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/EncoderCoeff/isShowEncoder", WorkCurveHelper.isShowEncoder.ToString());

            if (txtFormula.Text == "")
            {
                Msg.Show(Info.IsNull);
                return;
            }
            foreach (var v in lstEncoder)
            {
                v.Save();
            }

            //WorkCurveHelper.EncoderPoint1 = txtX1.Text + "," + txtY1.Text;
            //WorkCurveHelper.EncoderPoint2 = txtX2.Text + "," + txtY2.Text;
            //WorkCurveHelper.EncoderFormula = txtFormula.Text;
            WorkCurveHelper.DeviceCurrent.EncoderFormula = txtFormula.Text;
            WorkCurveHelper.DeviceCurrent.Save();
            ReportTemplateHelper.SaveSpecifiedValueandCreate("EncoderCoeff", "EncoderCalcWay", WorkCurveHelper.EncoderCalcWay.ToString());
       
            //ReportTemplateHelper.SaveSpecifiedValueandCreate("EncoderCoeff", "point1", WorkCurveHelper.EncoderPoint1);
            //ReportTemplateHelper.SaveSpecifiedValueandCreate("EncoderCoeff", "point2", WorkCurveHelper.EncoderPoint2);

            //ReportTemplateHelper.SaveSpecifiedValueandCreate("EncoderCoeff", "formula", WorkCurveHelper.EncoderFormula);


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var v = EncodeXY.New.Init(0, 0);
            lstEncoder.Add(v);
            InstanceDGV();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvEncoder.SelectedRows.Count <= 0)
            {
                SkyrayMsgBox.Show(Info.NoSelect);
            }
            else
            {
                int index = dgvEncoder.SelectedRows[0].Index;
                EncodeXY.DeleteAll(w => w.X == Convert.ToDouble(dgvEncoder[0, index].Value));
                lstEncoder.RemoveAt(index);

                
                InstanceDGV();
                int count = lstEncoder.Count;
                if (count > 1)
                {
                    if (index >= count)
                    {
                        this.dgvEncoder.Rows[0].Selected = false;
                        this.dgvEncoder.Rows[count - 1].Selected = true;
                    }
                    else
                    {
                        this.dgvEncoder.Rows[0].Selected = false;
                        this.dgvEncoder.Rows[index].Selected = true;
                    }
                }
            }
        }

        private void dgvEncoder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radTwoNoForcedOrigin_CheckedChanged(object sender, EventArgs e)
        {
            if (radTwoNoForcedOrigin.Checked)
            {
                WorkCurveHelper.EncoderCalcWay = 0;
            }
            else
            {
                WorkCurveHelper.EncoderCalcWay = 1;
            }
        }
       
       
    }
}
