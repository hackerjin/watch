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
    public partial class UCVirtualSelect :UCMultiple
    {
        //public Dictionary<SpecList, bool> VirtualSpecListAdditionalBackUp = new Dictionary<SpecList, bool>();
        ////修改：何晓明 20110719 增加取消功能_添加取消选中事件

        //public override void ExcuteCloseProcess(params object[] objs)
        //{
        //    if (VirtualSpecListAdditionalBackUp.Count > 0)
        //    {
        //        foreach (KeyValuePair<SpecList, bool> pair in VirtualSpecListAdditionalBackUp)
        //        {
        //            WorkCurveHelper.VirtualSpecListAdditional.Remove(pair.Key);
        //            VColor colorTemp = WorkCurveHelper.DefaultVirtualColor.Find(delegate(VColor ww) { return ww.Color.ToArgb() == pair.Key.VirtualColor; });
        //            if (colorTemp != null)
        //                colorTemp.BeSelected = false;
        //        }
        //    }
        //}

        public UCVirtualSelect()
        {
            InitializeComponent();
            IsRatioVisual = true;
        }

        //存储所有操作的谱对象，包括新增、删除、和原有对象
        List<SpecListEntity> VirtualSpecList = new List<SpecListEntity>();
        private void btnNew_Click(object sender, EventArgs e)
        {
            List<SpecListEntity> NewAddVirtualSpecList = new List<SpecListEntity>();
            List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false,true);
            if (returnResult == null || returnResult.Count == 0)
                return;

            //修改：何晓明 20110701 谱名称3列显示 打开对比谱时传回已加载的对比谱
            if (returnResult == null || returnResult.Count == 0)
                return;
            foreach (SpecListEntity temp in returnResult)
            {
                if (temp == null)
                    continue;
                var test = VirtualSpecList.Find(delegate(SpecListEntity tt) { return tt.Name == temp.Name; });
                if (test == null)
                {
                    NewAddVirtualSpecList.Add(temp);
                }
                else
                {
                    VColor colorTemp = WorkCurveHelper.DefaultVirtualColor.Find(delegate(VColor ww) { return ww.Color.ToArgb() == temp.VirtualColor; });
                    if (colorTemp != null)
                        colorTemp.BeSelected = false;
                }
            }
            string VisualSpecNumber = Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenSelectSampleType", "VisualSpecNumber");
            //加对比谱一个个的加数量不能达到六个
            if (NewAddVirtualSpecList.Count + VirtualSpecList.Count > int.Parse(VisualSpecNumber))
            {
                Msg.Show(string.Format(Info.VisualSpecOutRange, VisualSpecNumber));
                //修改：何晓明 20110203 当选取谱数量大于6个时此次所选的颜色恢复为未选中状态
                foreach (SpecListEntity temp in returnResult)
                {
                    if (temp == null) continue;
                    VColor colorTemp = WorkCurveHelper.DefaultVirtualColor.Find(delegate(VColor ww) { return ww.Color.ToArgb() == temp.VirtualColor; });
                    if (colorTemp != null)
                        colorTemp.BeSelected = false;
                }
                return;
            }
            foreach (SpecListEntity temp in NewAddVirtualSpecList)
            {
                if (temp == null)
                    continue;
                VirtualSpecList.Add(temp);
                this.dataGridVirtual.Rows.Add(temp.Name, temp.VirtualColor, temp.SpecDate.HasValue ? temp.SpecDate.Value.Date.ToString() : "",
                         temp.Specs.Length > 0 ? temp.Specs[0].UsedTime : 0, (temp.Specs.Length > 0) ? temp.Specs[0].TubVoltage : 0, (temp.Specs.Length > 0) ? temp.Specs[0].TubCurrent : 0,
                         (temp.Specs.Length > 0 && temp.Specs[0].DeviceParameter != null) ? temp.Specs[0].DeviceParameter.FilterIdx : 0, true);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridVirtual.CurrentRow != null)
            {
                string name = this.dataGridVirtual.CurrentRow.Cells["SpecListName"].Value.ToString();
                if (WorkCurveHelper.WorkCurveCurrent != null 
                    && WorkCurveHelper.WorkCurveCurrent.ElementList != null 
                    && WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr!=null
                    &&WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr!=string.Empty
                    &&name.Equals(WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr))
                {
                    Msg.Show(Info.WorkCurveBeUsed);
                    return;
                }
                var test = VirtualSpecList.ToList().Find(w => w.Name == name);
                if (test != null)
                {
                    VColor colorTemp = WorkCurveHelper.DefaultVirtualColor.Find(d => d.Color.ToArgb() == test.VirtualColor);
                    VirtualSpecList.Remove(test);
                    //VColor colorTemp = WorkCurveHelper.DefaultVirtualColor.Find(d => d.Color == this.dataGridVirtual.CurrentRow.Cells["SpecColor"].Style.BackColor);
                    this.dataGridVirtual.Rows.Remove(this.dataGridVirtual.CurrentRow);
                    //VColor colorTemp = WorkCurveHelper.DefaultVirtualColor.Find(delegate(VColor ww) 
                    //{ return ww.Color.ToArgb() == this.dataGridVirtual.CurrentRow.Cells["SpecColor"].Style.BackColor.ToArgb();});

                    if (colorTemp != null)
                        colorTemp.BeSelected = false;
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (WorkCurveHelper.VirtualSpecList != null)
                WorkCurveHelper.VirtualSpecList.Clear();

            if (WorkCurveHelper.VirtualSpecListAdditional != null)
                WorkCurveHelper.VirtualSpecListAdditional.Clear();

            if (WorkCurveHelper.NoActiveVirtualSpecList != null)
                WorkCurveHelper.NoActiveVirtualSpecList.Clear();

            foreach (VColor w in WorkCurveHelper.DefaultVirtualColor)
                w.BeSelected = false;

            foreach (DataGridViewRow row in this.dataGridVirtual.Rows)
            {
                string name = row.Cells["SpecListName"].Value.ToString();
                bool enable = (bool)row.Cells["ColumnEnable"].EditedFormattedValue;
                var testcol = VirtualSpecList.Find(w => w.Name == name);
                //var test = SpecList.FindById(id);
                //if (test != null || id==0)
                //{
                if (!string.IsNullOrEmpty(name))
                {
                    WorkCurveHelper.VirtualSpecListAdditional.Add(name, enable);

                    VColor colorTemp = WorkCurveHelper.DefaultVirtualColor.Find(delegate(VColor ww) { return ww.Color.ToArgb() == row.Cells["SpecColor"].Style.BackColor.ToArgb(); });
                    if (colorTemp != null)
                    {
                        colorTemp.BeSelected = true;
                        testcol.VirtualColor = colorTemp.Color.ToArgb();
                    }
                    else
                        testcol.VirtualColor = testcol.VirtualColor;

                    //VColor colorTemp = WorkCurveHelper.DefaultVirtualColor.Find(delegate(VColor ww) { return ww.Color.ToArgb() == testcol.VirtualColor; });
                    //if (colorTemp != null)
                    //{
                    //    colorTemp.BeSelected = true;
                    //    test.VirtualColor = colorTemp.Color.ToArgb();
                    //}                        
                }
                if (enable)
                    WorkCurveHelper.VirtualSpecList.Add(testcol);
                else WorkCurveHelper.NoActiveVirtualSpecList.Add(testcol);
                //}
            }
            
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);
        }

        private void UCVirtualSelect_Load(object sender, EventArgs e)
        {
            if (WorkCurveHelper.VirtualSpecList == null) WorkCurveHelper.VirtualSpecList = new List<SpecListEntity>();
            if (WorkCurveHelper.NoActiveVirtualSpecList == null) WorkCurveHelper.NoActiveVirtualSpecList = new List<SpecListEntity>();
            foreach (VColor w in WorkCurveHelper.DefaultVirtualColor)
                w.BeSelected = false;

            if (WorkCurveHelper.VirtualSpecListAdditional != null && WorkCurveHelper.VirtualSpecListAdditional.Count > 0)
            {
                foreach (KeyValuePair<string, bool> temp in WorkCurveHelper.VirtualSpecListAdditional)
                {
                    if (string.IsNullOrEmpty(temp.Key))
                        continue;
                    SpecListEntity tempResult = WorkCurveHelper.VirtualSpecList.Find(w => w.Name == temp.Key);
                    if (tempResult == null)
                        tempResult = WorkCurveHelper.NoActiveVirtualSpecList.Find(w => w.Name == temp.Key);
                    if (tempResult == null)
                        continue;
                    VColor colorTemp = WorkCurveHelper.DefaultVirtualColor.Find(delegate(VColor ww) { return ww.Color.ToArgb() == tempResult.VirtualColor; });
                    if (colorTemp != null) colorTemp.BeSelected = true;

                    VirtualSpecList.Add(tempResult);
                    this.dataGridVirtual.Rows.Add(tempResult.Name, tempResult.VirtualColor, tempResult.SpecDate.HasValue ? tempResult.SpecDate.Value.Date.ToString() : "",
                        tempResult.Specs.Length > 0 ? tempResult.Specs[0].UsedTime : 0, (tempResult.Specs.Length > 0) ? tempResult.Specs[0].TubVoltage : 0, (tempResult.Specs.Length > 0) ? tempResult.Specs[0].TubCurrent : 0,
                        (tempResult.Specs.Length > 0 && tempResult.Specs[0].DeviceParameter != null) ? tempResult.Specs[0].DeviceParameter.FilterIdx : 0, temp.Value);
                }
            }
        }

        private void btnCacel_Click(object sender, EventArgs e)
        {

            this.dialogResult = DialogResult.Cancel;
            EDXRFHelper.GotoMainPage(this);
        }

        private void dataGridVirtual_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var cell = dataGridVirtual[e.ColumnIndex, e.RowIndex];
                if (e.ColumnIndex == this.dataGridVirtual.Columns["SpecColor"].Index)
                {
                    //long id = (long)this.dataGridVirtual.Rows[e.RowIndex].Cells["SpecListId"].Value;
                   string name = this.dataGridVirtual.Rows[e.RowIndex].Cells["SpecListName"].Value.ToString();
                   var test = VirtualSpecList.Find(w => w.Name == name);
                    if (test != null)
                    {
                        cell.Style.BackColor = cell.Style.ForeColor =
                        cell.Style.SelectionBackColor =
                        cell.Style.SelectionForeColor = Color.FromArgb(test.VirtualColor);
                    }
                }
            }
        }

        #region 备注
        //private void btnNew_Click(object sender, EventArgs e)
        //{
        //    Dictionary<SpecList, bool> NewVirtualSpecListAdditionalBackUp = new Dictionary<SpecList, bool>();
        //    //修改：何晓明 20110701 谱名称3列显示 打开对比谱时传回已加载的对比谱
        //    SelectSample sample = new SelectSample(new List<SpecList>());
        //    WorkCurveHelper.OpenUC(sample, false, Info.OpenVirtualSpec);
        //    if (DialogResult.OK == sample.dialogResult)
        //    {
        //        List<SpecList> listSpec = sample.SelectedSpecList;
        //        //修改：何晓明 20110701 谱名称3列显示 打开对比谱时传回已加载的对比谱
        //        if (listSpec == null || listSpec.Count == 0)
        //            return;
        //        foreach (SpecList temp in listSpec)
        //        {
        //            if (temp == null)
        //                continue;
        //            ////修改：何晓明 20110701 谱名称3列显示
        //            //foreach (var c in WorkCurveHelper.DefaultVirtualColor)
        //            //{
        //            //    if (!c.BeSelected)
        //            //    {
        //            //        temp.VirtualColor = c.Color.ToArgb();
        //            //        c.BeSelected = true;
        //            //        break;
        //            //    }
        //            //}
        //            var test = WorkCurveHelper.VirtualSpecListAdditional.ToList().Find(delegate(KeyValuePair<SpecList, bool> tt) { return tt.Key.Id == temp.Id; });
        //            if (test.Key == null)
        //            {
        //                //修改：何晓明 20110719 增加取消功能_确认后添加信息
        //                NewVirtualSpecListAdditionalBackUp.Add(temp, true);
        //            }
        //        }
        //        string VisualSpecNumber = Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenSelectSampleType", "VisualSpecNumber");
        //        //修改：何晓明 20110919 加对比谱一个个的加数量不能达到六个
        //        if (WorkCurveHelper.VirtualSpecListAdditional.Count + NewVirtualSpecListAdditionalBackUp.Count > int.Parse(VisualSpecNumber))
        //        //if (WorkCurveHelper.VirtualSpecListAdditional.Count + VirtualSpecListAdditionalBackUp.Count + NewVirtualSpecListAdditionalBackUp.Count > int.Parse(VisualSpecNumber))
        //        //
        //        {
        //            Msg.Show(string.Format(Info.VisualSpecOutRange, VisualSpecNumber));
        //            return;
        //        }
        //        foreach (KeyValuePair<SpecList, bool> tempPair in NewVirtualSpecListAdditionalBackUp)
        //        {
        //            SpecList temp = tempPair.Key;
        //            if (temp == null)
        //                continue;
        //            VirtualSpecListAdditionalBackUp.Add(temp, true);
        //            WorkCurveHelper.VirtualSpecListAdditional.Add(temp, true);
        //            this.dataGridVirtual.Rows.Add(temp.Id, temp.Name, "", temp.SpecDate.HasValue ? temp.SpecDate.Value.Date.ToString() : "",
        //                     temp.Specs.Count > 0 ? temp.Specs[0].UsedTime : 0, (temp.Specs.Count > 0) ? temp.Specs[0].TubVoltage : 0, (temp.Specs.Count > 0) ? temp.Specs[0].TubCurrent : 0,
        //                     (temp.Specs.Count > 0 && temp.Specs[0].DeviceParameter != null) ? temp.Specs[0].DeviceParameter.FilterIdx : 0, true);
        //        }
        //    }
        //}

        ////修改：何晓明 20110923 点击删除再点取消对比谱已删除_增加删除列表
        //List<KeyValuePair<SpecList, bool>> lstDel = new List<KeyValuePair<SpecList, bool>>();
        ////
        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    if (this.dataGridVirtual.CurrentRow != null)
        //    {
        //        long id = (long)this.dataGridVirtual.CurrentRow.Cells["SpecListId"].Value;
        //        var test = WorkCurveHelper.VirtualSpecListAdditional.ToList().Find(w => w.Key.Id == id);
        //        if (test.Key != null)
        //        {
        //            //修改：何晓明 20110923 点击删除再点取消对比谱已删除_添加删除项
        //            //WorkCurveHelper.VirtualSpecListAdditional.Remove(test.Key);
        //            lstDel.Add(test);
        //            //
        //            this.dataGridVirtual.Rows.Remove(this.dataGridVirtual.CurrentRow);
        //            VColor colorTemp = WorkCurveHelper.DefaultVirtualColor.Find(delegate(VColor ww) { return ww.Color.ToArgb() == test.Key.VirtualColor; });
        //            if (colorTemp != null)
        //                colorTemp.BeSelected = false;
        //        }
        //    }
        //}

        //private void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    if (WorkCurveHelper.VirtualSpecList != null)
        //        WorkCurveHelper.VirtualSpecList.Clear();
        //    foreach (DataGridViewRow row in this.dataGridVirtual.Rows)
        //    {
        //        long id = (long)row.Cells["SpecListId"].Value;
        //        bool enable = (bool)row.Cells["ColumnEnable"].EditedFormattedValue;
        //        var test = WorkCurveHelper.VirtualSpecListAdditional.ToList().Find(w => w.Key.Id == id);
        //        if (test.Key != null)
        //        {
        //            WorkCurveHelper.VirtualSpecListAdditional.Remove(test.Key);
        //            WorkCurveHelper.VirtualSpecListAdditional.Add(test.Key, enable);
        //            if (enable)
        //                WorkCurveHelper.VirtualSpecList.Add(test.Key);
        //        }
        //    }
        //    //修改：何晓明 20110923 点击删除再点取消对比谱已删除_点击确认后删除
        //    foreach (var test in lstDel)
        //    {
        //        WorkCurveHelper.VirtualSpecListAdditional.Remove(test.Key);
        //    }
        //    //
        //    if (this.ParentForm != null)
        //        this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
        //    EDXRFHelper.GotoMainPage(this);
        //}

        //private void UCVirtualSelect_Load(object sender, EventArgs e)
        //{
        //    if (WorkCurveHelper.VirtualSpecListAdditional != null && WorkCurveHelper.VirtualSpecListAdditional.Count > 0)
        //    {
        //        foreach (KeyValuePair<SpecList, bool> temp in WorkCurveHelper.VirtualSpecListAdditional)
        //        {
        //            if (SpecList.FindById(temp.Key.Id) == null)
        //                continue;
        //            if (temp.Key == null)
        //                continue;
        //            this.dataGridVirtual.Rows.Add(temp.Key.Id, temp.Key.Name, "", temp.Key.SpecDate.HasValue ? temp.Key.SpecDate.Value.Date.ToString() : "",
        //                temp.Key.Specs.Count > 0 ? temp.Key.Specs[0].UsedTime : 0, (temp.Key.Specs.Count > 0) ? temp.Key.Specs[0].TubVoltage : 0, (temp.Key.Specs.Count > 0) ? temp.Key.Specs[0].TubCurrent : 0,
        //                (temp.Key.Specs.Count > 0 && temp.Key.Specs[0].DeviceParameter != null) ? temp.Key.Specs[0].DeviceParameter.FilterIdx : 0, temp.Value);
        //        }
        //    }
        //}

        //private void btnCacel_Click(object sender, EventArgs e)
        //{
        //    this.dialogResult = DialogResult.Cancel;
        //    EDXRFHelper.GotoMainPage(this);
        //}

        //private void dataGridVirtual_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        //{
        //    if (e.RowIndex != -1)
        //    {
        //        var cell = dataGridVirtual[e.ColumnIndex, e.RowIndex];
        //        if (e.ColumnIndex == this.dataGridVirtual.Columns["SpecColor"].Index)
        //        {
        //            long id = (long)this.dataGridVirtual.Rows[e.RowIndex].Cells["SpecListId"].Value;
        //            var test = WorkCurveHelper.VirtualSpecListAdditional.ToList().Find(w => w.Key.Id == id);
        //            if (test.Key != null)
        //            {
        //                cell.Style.BackColor = cell.Style.ForeColor =
        //                cell.Style.SelectionBackColor =
        //                cell.Style.SelectionForeColor = Color.FromArgb(test.Key.VirtualColor);
        //            }
        //        }
        //    }
        //}

        #endregion



      

        private bool IsRatioVisual = false;    //是否比例对比谱

        public override void ExcuteEndProcess(params object[] objs)
        {
            if (IsRatioVisual)
                DifferenceDevice.MediumAccess.SelectRatioVirtualSpec(WorkCurveHelper.VirtualSpecList);
            else
                DifferenceDevice.MediumAccess.OpenVirtualWorkSpectrum(WorkCurveHelper.VirtualSpecList);
        }

        private void btnRatioVisual_Click(object sender, EventArgs e)
        {
            IsRatioVisual = false;
            btnSubmit_Click(null, null);
        }

        private void dataGridVirtual_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            if (dataGridVirtual.Columns[e.ColumnIndex].Name.Equals("SpecColor"))//选择颜色
            {
                //选择的颜色，如果在进行选择其他颜色，则原颜色的标识位释放出来
                int befColor=-1;
                //long id = (long)dataGridVirtual.Rows[e.RowIndex].Cells["SpecListId"].Value;
                string name = dataGridVirtual.Rows[e.RowIndex].Cells["SpecListName"].Value.ToString();
                var test = VirtualSpecList.Find(w => w.Name == name);
                if (test != null)
                {
                    befColor=test.VirtualColor;
                }                
                ColorDialog cd = new ColorDialog();
                cd.FullOpen = true;
                if (VirtualSpecList != null && VirtualSpecList.Count > 0)
                {
                    SpecListEntity temp = VirtualSpecList.Find(w => w.Name == name);
                    cd.Color = Color.FromArgb(temp.VirtualColor);
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        dataGridVirtual[e.ColumnIndex, e.RowIndex].Style.BackColor = dataGridVirtual[e.ColumnIndex, e.RowIndex].Style.ForeColor =
                            dataGridVirtual[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor =
                            dataGridVirtual[e.ColumnIndex, e.RowIndex].Style.SelectionForeColor = cd.Color;
                        temp.VirtualColor = cd.Color.ToArgb();

                        if (befColor != temp.VirtualColor)
                        {
                            VColor colorTemp = WorkCurveHelper.DefaultVirtualColor.Find(delegate(VColor ww) { return ww.Color.ToArgb() == befColor; });

                            //------
                            if (colorTemp != null) colorTemp.BeSelected = false;

                            ////修改：何晓明 20111124 新增的颜色在默认颜色列表外导致显示为黑色
                            ////新增的颜色替换掉原来的颜色
                            //if (colorTemp != null)
                            //    WorkCurveHelper.DefaultVirtualColor.Remove(colorTemp);
                            //WorkCurveHelper.DefaultVirtualColor.Add(new VColor { BeSelected = true, Color = Color.FromArgb(temp.VirtualColor) });

                            //if (colorTemp != null) colorTemp.BeSelected = false;
                        }
                    }
                }
            }
        }

        
    }
}
