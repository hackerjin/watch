using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Skyray.Controls;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Define;
using Lephone.Data.Common;
using System.Collections.Generic;
using Skyray.Language;
using System.Data;

namespace Skyray.UC
{
    /// <summary>
    /// 元素周期表
    /// </summary>
    public partial class FrmElementTable : UCMultiple
    {

        #region Fields

        //public event EventDelegate.ManAnalysis OnTableAnalysis;
        //public event EventDelegate.ManAnalysis OnSettingSampleType;
        /// <summary>
        /// 当前层数
        /// </summary>
        private int currentRowIndex = 0;

        /// <summary>
        /// 是否手动分析
        /// </summary>
        private bool _IsManAnalysis;

        public int CurrentRowIndex
        {
            get { return currentRowIndex; }
            set
            {
                //if (value == 0)
                //{ elementTable.MultiSelect = false; }
                //else
                //{ elementTable.MultiSelect = true; }
                currentRowIndex = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private const int MIN_LAYER_NUMBER_INT = 2;

        /// <summary>
        /// 
        /// </summary>
        private const int MAX_LAYER_CONSTITUENT_INT = 15;

        /// <summary>
        /// 
        /// </summary>
        private const int MIN_SAMPLE_SPECTRUM_NUMBER_INT = 0;

        /// <summary>
        /// 
        /// </summary>
        private const int MAX_STANDARDS_NUMBER_INT = 30;

        /// <summary>
        /// 
        /// </summary>
        private const int MAX_ANALYTE_NUMBER_INT = 35;
        /// <summary>
        /// 元素列表
        /// </summary>
        public string[] names;
        /// <summary>
        /// 线系列表
        /// </summary>
        public int[] lines;

        //private DbObjectList<Compounds> lstCompounds;
        private DbObjectList<DefinePureElement> lstCompounds;
        /// <summary>
        /// 测厚元素列表
        /// </summary>
        public string[][] lstName;
        /// <summary>
        /// 测厚线系列表
        /// </summary>
        public int[][] lstLine;
        /// <summary>
        /// 返回对话
        /// </summary>
        public DialogResult DialogResult { get; set; }
        /// <summary>
        /// 原元素集合
        /// </summary>
        //public ElementList OldElementList { get; set; }
        ///// <summary>
        ///// 新元素集合
        ///// </summary>
        //public ElementList NewElementList { get; set; }

        public bool ShowKL
        {
            get { return elementTable == null ? false : elementTable.ShowKL; }
            set { elementTable.ShowKL = value; }
        }

        #endregion

        #region Init

        public FrmElementTable()
        { }

        /// <summary>
        /// 重构
        /// </summary>
        /// <param name="names"></param>
        /// <param name="lines"></param>
        public FrmElementTable(string[] names, int[] lines, ElementList elementList, bool IsManAnalysis, bool IsEcInit)
        {
            InitializeComponent();
            _IsManAnalysis = IsManAnalysis;
            if (_IsManAnalysis) tabEleList.TabPages.RemoveAt(1);
            //elementTable.SignelLine = false;

            this.names = names;
            this.lines = lines;
            this.elementTable.MouseDown += new MouseEventHandler(elementTable_MouseDown);
            if (WorkCurveHelper.WorkCurveCurrent != null && tabEleList.TabPages.Contains(tabCompounds))
            {
                //lstCompounds = Compounds.Find(c => c.WorkCurve.Id == WorkCurveHelper.WorkCurveCurrent.Id);
                lstCompounds = DefinePureElement.FindAll();
                InitAllCompounds();
            }
            int ElemCount = 0;
            #region 添加样品结构表的列
            if (DifferenceDevice.IsThick && !_IsManAnalysis)//测厚
            {
                if (WorkCurveHelper.CalcType == CalcType.PeakDivBase)
                    elementTable.MaxLayElement = 1;
                else
                    elementTable.MaxLayElement = MAX_LAYER_CONSTITUENT_INT;
                CurrentRowIndex = 0;
                gbxSampleConstruct.Enabled = true;
                dgvSampleConstruct.RowCount = MIN_LAYER_NUMBER_INT;
                lstName = new string[DifferenceDevice.MAX_LAYER_NUMBER_INT][];
                lstLine = new int[DifferenceDevice.MAX_LAYER_NUMBER_INT][];
                if (WorkCurveHelper.CalcType == CalcType.PeakDivBase)
                    ElemCount = 1;
                else
                    ElemCount = MAX_LAYER_CONSTITUENT_INT;
                for (int i = 1; i <= ElemCount; i++)
                {
                    dgvSampleConstruct.Columns.Add("col" + i.ToString(), i.ToString());
                }
                if (elementList == null) return;
                if (elementList.Items.Count > 0)
                {
                    ////获取基材元素
                    CurveElement ele = elementList.Items.ToList().OrderByDescending(element => element.LayerNumber).ToList<CurveElement>()[0];
                    List<CurveElement> lce = elementList.Items.ToList().FindAll(el => el.LayerNumber == ele.LayerNumber);
                    var arrNames = (from p in lce select p.Caption).ToArray();
                    var arrLines = (from p in lce select (int)p.AnalyteLine).ToArray();
                    if (WorkCurveHelper.CalcType == CalcType.EC)
                    {
                        names = lstName[0] = arrNames[0].Split(';');
                        lines = lstLine[0] = new int[names.Length];
                    }
                    else
                    {
                        //基材名称
                        names = lstName[0] = arrNames;
                        //基材线系
                        lines = lstLine[0] = arrLines;
                    }
                    //基材行写值
                    this.dgvSampleConstruct.Rows[0].SetValues((object[])Array.CreateInstance(typeof(Object), this.dgvSampleConstruct.ColumnCount));
                    this.dgvSampleConstruct.Rows[0].SetValues(lstName[0]);
                    ////初始选中元素
                    this.elementTable.SetSelectedItem(lstName[0], lstLine[0]);
                    //遍历其它层
                    for (int i = 1; i < ele.LayerNumber; i++)
                    {
                        List<CurveElement> lcst = elementList.Items.ToList().FindAll(e => e.LayerNumber == i);
                        if (lcst != null && lcst.Count > 0)
                        {
                            if (WorkCurveHelper.CalcType == CalcType.EC)
                            {
                                lstName[i] = lcst[0].Caption.Split(';');
                                lstLine[i] = new int[lstName[i].Length];
                            }
                            else
                            {
                                lstName[i] = new string[lcst.Count];
                                lstLine[i] = new int[lcst.Count];
                                for (int j = 0; j < lcst.Count; j++)
                                {
                                    lstName[i][j] = lcst[j].Caption;
                                    lstLine[i][j] = (int)lcst[j].AnalyteLine;
                                }
                            }
                            if (i > 1)
                            {
                                dgvSampleConstruct.Rows.Insert(i, (object[])Array.CreateInstance(typeof(Object), dgvSampleConstruct.ColumnCount));
                            }
                            this.dgvSampleConstruct.Rows[i].SetValues((object[])Array.CreateInstance(typeof(Object), this.dgvSampleConstruct.ColumnCount));
                            this.dgvSampleConstruct.Rows[i].SetValues(lstName[i]);
                        }
                    }
                    SelectCompounds(lstName[0]);
                    if (IsEcInit)
                    {
                        for (int i = 0; i < lstName.Length; i++)
                        {
                            if (lstName[i] != null)
                            {
                                string name = string.Join(";", lstName[i]);
                                lstName[i] = new string[] { name };
                            }
                        }
                    }
                    return;
                }
            }
            else
            {
                if (!_IsManAnalysis)
                SelectCompounds(names);
            }
            #endregion
            this.elementTable.SetSelectedItem(names, lines);
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmElementTable_Load(object sender, EventArgs e)
        {
            
            if (DifferenceDevice.IsThick && !_IsManAnalysis)
            {
                //if (WorkCurveHelper.CalcType == CalcType.EC)
                //{
                //    this.chkIsAbsorb.Visible = true;
                //    BindHelper.BindCheckedToCtrl(chkIsAbsorb, WorkCurveHelper.WorkCurveCurrent, "IsAbsorb");
                //}
                //else
                //{
                //    this.chkShowContent.Visible = true;
                //    BindHelper.BindCheckedToCtrl(chkShowContent, WorkCurveHelper.WorkCurveCurrent, "IsThickShowContent");
                //}
                this.btnAddLayer.Visible =
                this.btnDelLayer.Visible =
                panel2.Visible = true;
            }
            else
            {
                panel2.Visible = false;
                this.ParentForm.Height -= panel2.Height;
            }

            if (DifferenceDevice.IsThick && WorkCurveHelper.CalcType == CalcType.PeakDivBase)
            {
                btnAddLayer.Visible = btnDelLayer.Visible = false;
            }
        }

        /// <summary>
        /// 初始化化合物
        /// </summary>
        private void InitAllCompounds()
        {
            for (int i = 0; i < lstCompounds.Count; i++)
            {
                this.clbCompounds.Items.Add(lstCompounds[i].Name);
            }
        }

        /// <summary>
        /// 选择化合物
        /// </summary>
        /// <param name="Temp"></param>
        private void SelectCompounds(string[] Temp)
        {
            for (int i = 0; i < lstCompounds.Count; i++)
            {
                if (!_IsManAnalysis && Temp != null && Temp.Length > 0)
                {
                    string nameCheck = Temp.ToList().Find(n => n.ToString() == lstCompounds[i].Name);
                    if (nameCheck != null && nameCheck != "")
                    {
                        clbCompounds.SetItemChecked(i, true);
                    }
                    else
                    {
                        clbCompounds.SetItemChecked(i, false);
                    }
                }
                else
                { clbCompounds.SetItemChecked(i, false); }
            }
        }

        #endregion

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (DifferenceDevice.IsThick && WorkCurveHelper.CalcType == CalcType.EC && !_IsManAnalysis)
            {
                for (int i = 0; i < lstName.Length; i++)
                {
                    if (lstName[i] != null)
                    {
                        string name = string.Join(";", lstName[i]);
                        lstName[i] = new string[] { name };
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            DifferenceDevice.MediumAccess.ManAnalysis(lines, names);
            EDXRFHelper.GotoMainPage(this);
        }

        #region Compound

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.CustomFields.Count > 0)
            //{
            //    CustomField cf = WorkCurveHelper.WorkCurveCurrent.ElementList.CustomFields.ToList().Find(w => w.Name == txtAdd.Text);
            //    if (cf != null)
            //    {
            //        Msg.Show(Info.ExistCustomName);
            //        return;
            //    }
            //}
            //if (ValidateHelper.IllegalCheck(txtAdd))
            //{
            //    if (lstCompounds.Find(c => c.Name == txtAdd.Text) != null)
            //    {
            //        Msg.Show(Info.ExistName);
            //    }
            //    else if (Atoms.AtomList.Find(a => a.AtomName.ToLower() == txtAdd.Text.ToLower()) != null)
            //    {
            //        Msg.Show(Info.CustomNameMustDifferenceWithElement);
            //    }
            //    else if (txtAdd.Text.GetAnsiLength() > 20)
            //    {
            //        Msg.Show(Info.CaptionMaxLength);
            //    }
            //    else
            //    {
            //        this.clbCompounds.Items.Add(txtAdd.Text);
            //        Compounds com = Compounds.New.Init(txtAdd.Text);
            //        com.WorkCurve= WorkCurveHelper.WorkCurveCurrent;
            //        com.Save();
            //        //string sql = "insert into Compounds values(null,'" + txtAdd.Text + "'," + WorkCurveHelper.WorkCurveCurrent.Id + ") ";
            //        //Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            //        lstCompounds.Add(com);
            //        txtAdd.Text = "";
            //    }
            //}
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            //if (this.clbCompounds.CheckedItems.Count < 0)
            //{
            //    SkyrayMsgBox.Show(Info.NoSelect);
            //}
            //else
            //{
            //    int count = clbCompounds.Items.Count;

            //    for (int i = count - 1; i > -1; i--)
            //    {
            //        if (clbCompounds.GetItemChecked(i))
            //        {
            //            Compounds com = lstCompounds[i];
            //            com.Delete();
            //            clbCompounds.Items.RemoveAt(i);
            //            lstCompounds.Remove(com);
            //        }
            //    }
            //}
            //GetEleComNames();
        }

        #endregion

        /// <summary>
        /// //选择元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void elementTable_MouseDown(object sender, MouseEventArgs e)
        {
            GetEleComNames();
        }

        private void GetEleComNames()
        {
            this.elementTable.GetSelectedItem(out names, out lines);
            List<string> lstNames = names.ToList();
            List<int> lstLines = lines.ToList();
            foreach (var item in clbCompounds.CheckedItems)
            {
                lstNames.Add(item.ToString());
                lstLines.Add(0);
            }
            names = lstNames.ToArray<string>();
            lines = lstLines.ToArray<int>();
            if (DifferenceDevice.IsThick && !_IsManAnalysis)
            {
                foreach (var a in names)
                {
                    for (int i = 0; i < lstName.Length; i++)
                    {
                        if (lstName[i] == null) break;
                        if (i == CurrentRowIndex) continue;
                        for (int j = 0; j < lstName[i].Length; j++)
                        {
                            if (lstName[i][j].Equals(a))
                            {
                                this.elementTable.SetSelectedItem(lstName[CurrentRowIndex], lstLine[CurrentRowIndex]);
                                return;
                            }
                        }
                    }
                }
                this.dgvSampleConstruct.CurrentRow.SetValues((object[])Array.CreateInstance(typeof(Object), this.dgvSampleConstruct.ColumnCount));
                this.dgvSampleConstruct.CurrentRow.SetValues(names);

                lstName[CurrentRowIndex] = names;
                lstLine[CurrentRowIndex] = lines;
            }
        }

        private void clbCompounds_MouseUp(object sender, MouseEventArgs e)
        {
            GetEleComNames();
        }

        /// <summary>
        /// 选择层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSampleConstruct_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0 || e.RowIndex > 5)
            {
                return;
            }
            CurrentRowIndex = e.RowIndex;
            this.elementTable.SetSelectedItem(lstName[CurrentRowIndex], lstLine[CurrentRowIndex]);
            //dgvSampleConstruct.Rows[e.RowIndex].Selected = true;
            if (!_IsManAnalysis)
                SelectCompounds(lstName[CurrentRowIndex]);
        }

        /// <summary>
        /// 增加行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiSampleConstructInsert_Click(object sender, EventArgs e)
        {
            
            if (dgvSampleConstruct.RowCount < DifferenceDevice.MAX_LAYER_NUMBER_INT)
            {
                dgvSampleConstruct.Rows.Add((object[])Array.CreateInstance(typeof(Object), dgvSampleConstruct.ColumnCount));
            }
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiSampleConstructDelete_Click(object sender, EventArgs e)
        {
            if (dgvSampleConstruct.SelectedRows.Count <= 0)
            {
                return;
            }
            if (dgvSampleConstruct.RowCount > MIN_LAYER_NUMBER_INT)
            {
                int index = dgvSampleConstruct.SelectedRows[0].Index;
                if (index > 1)
                {
                    for (int i = 2; i < lstName.Length; i++)
                    {
                        if (i > index)
                        {
                            lstName[i - 1] = lstName[i];
                        }
                        if (i == lstName.Length - 1)
                        {
                            //lstName[i] = new string[] { };
                            lstName[i] = null;
                            break;
                        }
                    }
                    dgvSampleConstruct.Rows.Remove(dgvSampleConstruct.SelectedRows[0]);
                    int rowCount = dgvSampleConstruct.RowCount;
                    if (index >= rowCount)
                    {
                        this.elementTable.SetSelectedItem(lstName[rowCount - 1], lstLine[rowCount - 1]);
                        CurrentRowIndex = rowCount - 1;
                    }
                    else
                    {
                        this.elementTable.SetSelectedItem(lstName[index], lstLine[index]);
                        CurrentRowIndex = index;
                    }
                }
            }
        }

        /// <summary>
        /// RowsAdded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSampleConstruct_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int rowCount = dgvSampleConstruct.RowCount;
            switch (rowCount)
            {
                case 2:
                    lblSampleStructLayerBase.Visible = true;
                    lblSampleStructLayer1.Visible = true;
                    lblSampleStructLayer2.Visible = false;
                    lblSampleStructLayer3.Visible = false;
                    lblSampleStructLayer4.Visible = false;
                    lblSampleStructLayer5.Visible = false;
                    break;
                case 3:
                    lblSampleStructLayerBase.Visible = true;
                    lblSampleStructLayer1.Visible = true;
                    lblSampleStructLayer2.Visible = true;
                    lblSampleStructLayer3.Visible = false;
                    lblSampleStructLayer4.Visible = false;
                    lblSampleStructLayer5.Visible = false;
                    break;
                case 4:
                    lblSampleStructLayerBase.Visible = true;
                    lblSampleStructLayer1.Visible = true;
                    lblSampleStructLayer2.Visible = true;
                    lblSampleStructLayer3.Visible = true;
                    lblSampleStructLayer4.Visible = false;
                    lblSampleStructLayer5.Visible = false;
                    break;
                case 5:
                    lblSampleStructLayerBase.Visible = true;
                    lblSampleStructLayer1.Visible = true;
                    lblSampleStructLayer2.Visible = true;
                    lblSampleStructLayer3.Visible = true;
                    lblSampleStructLayer4.Visible = true;
                    lblSampleStructLayer5.Visible = false;
                    break;
                case 6:
                    lblSampleStructLayerBase.Visible = true;
                    lblSampleStructLayer1.Visible = true;
                    lblSampleStructLayer2.Visible = true;
                    lblSampleStructLayer3.Visible = true;
                    lblSampleStructLayer4.Visible = true;
                    lblSampleStructLayer5.Visible = true;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// RowsRemoved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSampleConstruct_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            int rowCount = dgvSampleConstruct.RowCount;
            switch (rowCount)
            {
                case 2:
                    lblSampleStructLayer1.Visible = true;
                    lblSampleStructLayer2.Visible = false;
                    lblSampleStructLayer3.Visible = false;
                    lblSampleStructLayer4.Visible = false;
                    lblSampleStructLayer5.Visible = false;
                    break;
                case 3:

                    lblSampleStructLayer1.Visible = true;
                    lblSampleStructLayer2.Visible = true;
                    lblSampleStructLayer3.Visible = false;
                    lblSampleStructLayer4.Visible = false;
                    lblSampleStructLayer5.Visible = false;
                    break;
                case 4:
                    lblSampleStructLayerBase.Visible = true;
                    lblSampleStructLayer1.Visible = true;
                    lblSampleStructLayer2.Visible = true;
                    lblSampleStructLayer3.Visible = true;
                    lblSampleStructLayer4.Visible = false;
                    lblSampleStructLayer5.Visible = false;
                    break;
                case 5:
                    lblSampleStructLayerBase.Visible = true;
                    lblSampleStructLayer1.Visible = true;
                    lblSampleStructLayer2.Visible = true;
                    lblSampleStructLayer3.Visible = true;
                    lblSampleStructLayer4.Visible = true;
                    lblSampleStructLayer5.Visible = false;
                    break;
                case 6:
                    lblSampleStructLayerBase.Visible = true;
                    lblSampleStructLayer1.Visible = true;
                    lblSampleStructLayer2.Visible = true;
                    lblSampleStructLayer3.Visible = true;
                    lblSampleStructLayer4.Visible = true;
                    lblSampleStructLayer5.Visible = true;
                    break;
                default:
                    break;
            }
        }
    }
}
