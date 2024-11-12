using System;
using System.Drawing;
using System.Windows.Forms;
using Lephone.Data.Common;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using System.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.UC
{
    /// <summary>
    /// 构造纯元素谱类
    /// </summary>
    public partial class UCPureElementsTable : Skyray.Language.UCMultiple
    {


        private List<ElementSpectrum> specElementList = new List<ElementSpectrum>();
        private List<int> removeElementSpec = new List<int>();

        public UCPureElementsTable()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加纯元素谱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWAdd_Click(object sender, EventArgs e)
        {
            List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false,false);
            if (returnResult == null || returnResult.Count == 0)
                return;
            ss_OnAddPureElements(returnResult);
        }

        /// <summary>
        /// 添加元素谱计算强度
        /// </summary>
        /// <param name="specListCurrent"></param>
        private void ss_OnAddPureElements(List<SpecListEntity> specListCurrent)
        {
            if (specListCurrent == null || specListCurrent.Count == 0)
                return;
            for (int i=0;i<specListCurrent.Count;i++)
            {
                SpecEntity spec = specListCurrent[i].Specs[0];
               
                    string atomName = specListCurrent[i].SampleName;
                    Atom atom = Atoms.AtomList.Find(w => String.Compare(w.AtomName,atomName,true) == 0);
                    if (atom == null) continue;
                    if (AssertSimilarRecord(atom.AtomID))
                        continue;

                    ElementList elementlist = ElementList.New;
                    for (int j = 0; j < 4;j++ )
                    {
                        CurveElement curveElem = CurveElement.New;
                        curveElem.SSpectrumData = "";
                        curveElem.Caption = atom.AtomName;
                        curveElem.Formula = atom.AtomName;
                        curveElem.AtomicNumber = atom.AtomID;
                        curveElem.LayerNumber = 1;
                        curveElem.Flag = ElementFlag.Calculated;
                        switch (j)
                        {
                            case 0:
                                curveElem.AnalyteLine = XLine.Ka;
                                break;
                            case 1:
                                curveElem.AnalyteLine = XLine.Kb;
                                break;
                            case 2:
                                curveElem.AnalyteLine = XLine.La;
                                break;
                            case 3:
                                curveElem.AnalyteLine = XLine.Lb;
                                break;
                        }
                        curveElem.IntensityWay = IntensityWay.FPGauss;
                        curveElem.ConditionID = 0;
                        curveElem.SInfluenceElements = "";
                        curveElem.SInfluenceCoefficients = "";
                        curveElem.ContentUnit = ContentUnit.per;
                        curveElem.Flag = ElementFlag.Calculated;
                        elementlist.Items.Add(curveElem);
                    }
                    WorkCurveHelper.WorkCurveCurrent.ElementList = elementlist;
                    WorkCurveHelper.WorkCurveCurrent.CaculateIntensity(specListCurrent[i]);

                    ElementSpectrum elementSpectrum = ElementSpectrum.New;
                    elementSpectrum.ElementID = atom.AtomID;
                    elementSpectrum.KaIntensity = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.First(wde=>wde.AnalyteLine==XLine.Ka).Intensity;
                    elementSpectrum.KbIntensity = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.First(wde => wde.AnalyteLine == XLine.Kb).Intensity;
                    elementSpectrum.LaIntensity = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.First(wde => wde.AnalyteLine == XLine.La).Intensity;
                    elementSpectrum.LbIntensity = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.First(wde => wde.AnalyteLine == XLine.Lb).Intensity;

                    int ch;
                    double fwhm = 0;
                    double ph = 0;
                    double time = spec.UsedTime;
                    DemarcateEnergyHelp.CalParam(Default.ConvertFromNewOld(specListCurrent[i].DemarcateEnergys.ToList(), WorkCurveHelper.DeviceCurrent.SpecLength));
                    //Atom atom = Atoms.AtomList.Find(w => w.AtomID == elementSpectrum.ElementID);//得到原子
                    int sysFwhm = DemarcateEnergyHelp.GetChannel(WorkCurveHelper.DeviceCurrent.Detector.Fwhm * 2 / 1000)
                                - DemarcateEnergyHelp.GetChannel(WorkCurveHelper.DeviceCurrent.Detector.Fwhm / 1000);
                    foreach (XLine line in Enum.GetValues(typeof(XLine)))
                    {
                        double energy = 0d;
                        switch (line)
                        {
                            case XLine.Ka:
                                energy = atom.Ka;
                                break;
                            case XLine.Kb:
                                energy = atom.Kb;
                                break;
                            case XLine.La:
                                energy = atom.La;
                                break;
                            case XLine.Lb:
                                energy = atom.Lb;
                                break;
                            case XLine.Le:
                                energy = atom.Le;
                                break;
                            case XLine.Lr:
                                energy = atom.Lr;
                                break;
                        }
                        ch = DemarcateEnergyHelp.GetChannel(energy);
                        if (energy < 1.2)
                        {
                            ch = 0;
                        }
                        TabControlHelper.FitPeakGauss(ch, sysFwhm, ref fwhm, ref ph, spec.SpecDatac);
                        switch (line)
                        {
                            case XLine.Ka:
                                elementSpectrum.KaFwhm = fwhm;
                                elementSpectrum.KaHight = ph / time;
                                break;
                            case XLine.Kb:
                                elementSpectrum.kbFwhm = fwhm;
                                elementSpectrum.KbHight = ph / time;
                                break;
                            case XLine.La:
                                elementSpectrum.LaFwhm = fwhm;
                                elementSpectrum.LaHight = ph / time;
                                break;
                            case XLine.Lb:
                                elementSpectrum.LbFwhm = fwhm;
                                elementSpectrum.LbHight = ph / time;
                                break;
                        }
                    }

                    ////double _ka, _kb, _la, _lb;
                    //elementSpectrum.KaIntensity
                    // = double.IsNaN(elementSpectrum.KaArea) ? 0 : elementSpectrum.KaArea;
                    //elementSpectrum.KbIntensity = double.IsNaN(elementSpectrum.KbArea) ? 0 : elementSpectrum.KbArea;
                    //elementSpectrum.LaIntensity = double.IsNaN(elementSpectrum.LaArea) ? 0 : elementSpectrum.LaArea;
                    //elementSpectrum.LbIntensity = double.IsNaN(elementSpectrum.LbArea) ? 0 : elementSpectrum.LbArea;
                    //数据构造
                    string[] row = new string[]
                {
                    elementSpectrum.ElementID.ToString(),
                    atom.AtomName,
                    elementSpectrum.KaIntensity.ToString("f"+WorkCurveHelper.SoftWareIntensityBit.ToString()),
                    elementSpectrum.KbIntensity.ToString("f"+WorkCurveHelper.SoftWareIntensityBit.ToString()),
                    elementSpectrum.LaIntensity.ToString("f"+WorkCurveHelper.SoftWareIntensityBit.ToString()),
                    elementSpectrum.LbIntensity.ToString("f"+WorkCurveHelper.SoftWareIntensityBit.ToString()),
                };
                    this.dataGridViewW1.Rows.Add(row);
                    specElementList.Add(elementSpectrum);
            }
        }

        private bool AssertSimilarRecord(int atomId)
        {
            if (this.dataGridViewW1.Rows.Count == 0)
                return false;
            foreach (DataGridViewRow row in this.dataGridViewW1.Rows)
            {
                int ElementID = Convert.ToInt32(row.Cells["ID"].Value.ToString());
                if (atomId == ElementID)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 移除元素谱数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWRemove_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewW1.SelectedRows == null || this.dataGridViewW1.SelectedRows.Count == 0)
                return;
            foreach (DataGridViewRow row in this.dataGridViewW1.SelectedRows)
            {
                int ElementID = Convert.ToInt32(row.Cells["ID"].Value.ToString());
                specElementList.RemoveAll(w => w.ElementID == ElementID);
                ElementSpectrum tempElemnt = ElementSpectrum.FindOne(w => w.ElementID == ElementID);
                if (tempElemnt != null)
                    removeElementSpec.Add(ElementID);
                this.dataGridViewW1.Rows.Remove(row);
            }
            
        }

        /// <summary>
        /// 保存纯元素谱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWSubmit_Click(object sender, EventArgs e)
        {
            if (this.removeElementSpec != null && this.removeElementSpec.Count > 0)
            {
                foreach (int a in this.removeElementSpec)
                {
                    ElementSpectrum tempElemnt = ElementSpectrum.FindOne(w => w.ElementID == a);
                    if (tempElemnt != null)
                        tempElemnt.Delete();
                }
            }

            if (this.specElementList != null && this.specElementList.Count > 0)
            {
                foreach (ElementSpectrum specTre in this.specElementList)
                        specTre.Save();
            }
            EDXRFHelper.GotoMainPage(this);//转到主界面
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//转到主界面
        }

        private void UCPureElementsTable_Load(object sender, EventArgs e)
        {
            var allElementSpectrum = ElementSpectrum.FindBySql("select * from ElementSpectrum");
            foreach (ElementSpectrum spec in allElementSpectrum)
            {
                string elmentName = Atoms.AtomList.Find(w => w.AtomID == spec.ElementID).AtomName;
                this.dataGridViewW1.Rows.Add(new string[] { spec.ElementID.ToString(), elmentName, spec.KaIntensity.ToString("f"+WorkCurveHelper.SoftWareIntensityBit.ToString()), 
                    spec.KbIntensity.ToString("f"+WorkCurveHelper.SoftWareIntensityBit.ToString()), spec.LaIntensity.ToString("f"+WorkCurveHelper.SoftWareIntensityBit.ToString()), spec.LbIntensity.ToString("f"+WorkCurveHelper.SoftWareIntensityBit.ToString()) });
                specElementList.Add(spec);//已经存在的纯元素谱强度修改
            }
        }

        private void dataGridViewW1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewW1.Rows[e.RowIndex].ErrorText = string.Empty;

        }

        private void dataGridViewW1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == ID.Index || e.ColumnIndex == ElementName.Index) return;
            string strTemp = e.FormattedValue.ToString();
            if (!strTemp.IsNum())
            {
                Msg.Show(Info.DataError);
                e.Cancel = true;
                return;
            }
            int idElem = int.Parse(dataGridViewW1.Rows[e.RowIndex].Cells[ID.Index].Value.ToString());
            ElementSpectrum spec = specElementList.Find(wde => wde.ElementID == idElem);
            if (spec == null || spec.ElementID != idElem) return;
            if (e.ColumnIndex==Ka.Index)
            {
                spec.KaIntensity = double.Parse(strTemp);
            }
            else if (e.ColumnIndex==Kb.Index)
            {
                spec.KbIntensity = double.Parse(strTemp);
            }
            else if (e.ColumnIndex==La.Index)
            {
                spec.LaIntensity = double.Parse(strTemp);
            }
            else if (e.ColumnIndex==Lb.Index)
            {
                spec.LbIntensity = double.Parse(strTemp);
            }
        }
    }
} 
