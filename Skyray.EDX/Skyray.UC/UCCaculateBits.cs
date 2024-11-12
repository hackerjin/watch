using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.Language;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    public partial class UCCaculateBits : UCMultiple
    {
        private List<int> layers = new List<int>();
        private List<double> layerDensity = new List<double>();
        public UCCaculateBits()
        {
            InitializeComponent();
            this.nUpContent.Value = Convert.ToInt32(ReportTemplateHelper.LoadSpecifiedValue("SoftWareCaculate", "ContentBit"));
            this.nUpThick.Value = Convert.ToInt32(ReportTemplateHelper.LoadSpecifiedValue("SoftWareCaculate", "ThickBit"));
            //this.numDensity.Value = Convert.ToDecimal(ReportTemplateHelper.LoadSpecifiedValue("SoftWareCaculate", "AreaDensity"));
            //yuzhao130523:改正数据转换类型从Int到Decimal
            //this.numDensityCoef.Value = Convert.ToDecimal(ReportTemplateHelper.LoadSpecifiedValue("SoftWareCaculate", "AreaDensityCoef"));
           // this.c
            if (WorkCurveHelper.WorkCurveCurrent!=null
               && WorkCurveHelper.WorkCurveCurrent.FuncType == FuncType.Thick
               &&WorkCurveHelper.WorkCurveCurrent.ElementList!=null
               && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count>0)
               {
                   int maxLayer = 0;
                   
                   for (int i = 0; i < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; i++)
                   {
                       if (!layers.Contains(WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].LayerNumber))
                       {
                           layers.Add(WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].LayerNumber);
                           layerDensity.Add(WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].LayerDensity);
                           if (maxLayer < WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].LayerNumber)
                           {
                               maxLayer = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].LayerNumber;
                           }
                           //if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].LayerNumber == 1)
                           //{
                           //    this.numDensity.Value = Convert.ToDecimal(WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].LayerDensity);
                           //}
                       }
                       
                   }
                   for (int i = 0; i < maxLayer;i++ )
                   {
                       string layer=string.Empty;
                       switch (i)
                       { 
                           case 0:
                               layer = Info.FirstLayer;
                               break;
                           case 1:
                               layer = Info.SecondLayer;
                               break;
                           case 2:
                               layer = Info.ThirdLayer;
                               break;
                           case 3:
                               layer = Info.ForthLayer;
                               break;
                           case 4:
                               layer = Info.FifthLayer;
                               break;
                       }
                       if (i + 1 == maxLayer)
                       {
                           layer = Info.Substrate;
                       }
                      this.comboBoxLayers.Items.Insert(i,layer);
                   }
                   if (maxLayer>0)
                   {
                       this.comboBoxLayers.SelectedIndex = 0;
                   }
               }
               else
               {
                   this.grouper2.Visible = false;
               }
        }

        private void btWCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void btWSubmit_Click(object sender, EventArgs e)
        {
            //if (DifferenceDevice.ithick != null)
            {
                //判断输入的值是否在有效范围
                if (nUpThick.Value.ToString() == "" || nUpThick.Value < nUpThick.Minimum || nUpThick.Value > nUpThick.Maximum
                    || nUpContent.Value.ToString() == "" || nUpContent.Value < nUpContent.Minimum || nUpContent.Value > nUpContent.Maximum)
                {
                    Msg.Show(Info.strThickAccuracy);
                    return;
                }
                if (DifferenceDevice.ithick != null)
                {
                    DifferenceDevice.ithick.CountBits(Convert.ToInt32(this.nUpThick.Value), Convert.ToInt32(this.nUpContent.Value));
                }
                ReportTemplateHelper.SaveSpecifiedValue("SoftWareCaculate", "ContentBit", this.nUpContent.Value.ToString().Trim());
                ReportTemplateHelper.SaveSpecifiedValue("SoftWareCaculate", "ThickBit", this.nUpThick.Value.ToString().Trim());

                //ReportTemplateHelper.SaveSpecifiedValue("SoftWareCaculate", "AreaDensity", this.numDensity.Value.ToString().Trim());
                //ReportTemplateHelper.SaveSpecifiedValue("SoftWareCaculate", "AreaDensityCoef", this.numDensityCoef.Value.ToString().Trim());
                //读取计算精度
                WorkCurveHelper.SoftWareContentBit = Convert.ToInt32(ReportTemplateHelper.LoadSpecifiedValue("SoftWareCaculate", "ContentBit"));
                WorkCurveHelper.ThickBit = Convert.ToInt32(ReportTemplateHelper.LoadSpecifiedValue("SoftWareCaculate", "ThickBit"));

                //WorkCurveHelper.AreaDensity = (double)this.numDensity.Value;
                //WorkCurveHelper.AreaDensityCoef = (double)this.numDensityCoef.Value;
                SaveLayerDensity();

            }
            EDXRFHelper.GotoMainPage(this);
        }

        private void comboBoxLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            int layer = this.comboBoxLayers.SelectedIndex+1;
            if (layer < 0) return;
            int index = layers.FindIndex(0,layers.Count,wde=>wde==layer);
            if (index >=0 )
                this.numDensity.Value = Convert.ToDecimal(layerDensity[index]);
            else
                this.numDensity.Value = -1;
        }


        private void SaveLayerDensity()
        {
            if (WorkCurveHelper.WorkCurveCurrent != null
                 && WorkCurveHelper.WorkCurveCurrent.FuncType == FuncType.Thick
                 && WorkCurveHelper.WorkCurveCurrent.ElementList != null
                 && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
            {

                for (int i = 0; i < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; i++)
                {
                    int layer = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].LayerNumber;
                    int index = layers.FindIndex(0, layers.Count, wde => wde == layer);
                    if (index >= 0)
                        WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].LayerDensity =layerDensity[index];
                    WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Save();
                }
            }
        }

        private void numDensity_ValueChanged(object sender, EventArgs e)
        {
            int layer = this.comboBoxLayers.SelectedIndex+1;
            if (layer < 0) return;
            int index = layers.FindIndex(0, layers.Count, wde => wde == layer);
            layerDensity[index] = Convert.ToDouble(numDensity.Value);
        }
    }
}
