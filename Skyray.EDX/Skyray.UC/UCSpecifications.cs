using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Lephone.Data.Common;
using Skyray.EDX.Common;
using Skyray.Controls;

namespace Skyray.UC
{
    public partial class UCSpecifications : Skyray.Language.UCMultiple
    {
        #region Fields 

        private DbObjectList<SpecificationsCategory> lstCategory;

        private SpecificationsCategory currentCategory;

        private SpecificationsExample currentExample;

        #endregion

        #region Init

        public UCSpecifications()
        {
            InitializeComponent();
        }

        private void UCSpecifications_Load(object sender, EventArgs e)
        {
            lstCategory = SpecificationsCategory.FindAll();
            BindCbo();
        }

        #endregion

        #region Event

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cboCategory.SelectedIndex;
            if (index >= 0)
            {
                currentCategory = lstCategory[index];
                bindExample();
                btnAddExample.Enabled = btnDeleteExample.Enabled = btnAddElement.Enabled = btnDeleteElement.Enabled = btnSave.Enabled = btnDelCategory.Enabled = true;
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            btnAddCategory.Visible = btnDelCategory.Visible = false;
            txtCategoryName.Visible = btnSaveNew.Visible = btnCancel.Visible = true;

        }

        private void btnDelCategory_Click(object sender, EventArgs e)
        {
            int index = cboCategory.SelectedIndex;
            if (WorkCurveHelper.CategoryCurrent != null)
            {
                if (lstCategory[index].CategoryName != WorkCurveHelper.CategoryCurrent.CategoryName)
                {
                    lstCategory[index].Delete();
                    lstCategory.RemoveAt(index);
                    BindCbo();
                }
                else
                {
                    SkyrayMsgBox.Show(Info.NotDeleteCurrentCategory);
                }
            }
            else
            {
                lstCategory[index].Delete();
                lstCategory.RemoveAt(index);
                BindCbo();
            }
        }

        /// <summary>
        /// 保存类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            btnAddCategory.Visible = btnDelCategory.Visible = true;
            txtCategoryName.Visible = btnSaveNew.Visible = btnCancel.Visible = false;
            if (ValidateHelper.IllegalCheck(txtCategoryName))
            {
                if (lstCategory.Find(l => l.CategoryName == txtCategoryName.Text) == null)
                {
                    SpecificationsCategory specificationsCategory = SpecificationsCategory.New;
                    specificationsCategory.CategoryName = txtCategoryName.Text;
                    //specificationsCategory.Save();
                    lstCategory.Add(specificationsCategory);
                    BindCbo();
                    cboCategory.SelectedItem = specificationsCategory.CategoryName;
                }
                else
                {
                    SkyrayMsgBox.Show(Info.ExistName);
                }
            }
        }

        private void btnCancelSave_Click(object sender, EventArgs e)
        {
            btnAddCategory.Visible = btnDelCategory.Visible = true;
            txtCategoryName.Visible = btnSaveNew.Visible = btnCancel.Visible = false;
        }

        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (var category in lstCategory)
            {
                if (category.Specifications.ToList().Find(s => s.ExampleName == "") != null)
                {
                    SkyrayMsgBox.Show(Info.EnterFullExampleName);
                    return;
                }
                if (category.CategoryName != currentCategory.CategoryName)
                {
                    category.IsCurrentCategory = false;
                }
                else
                {
                    category.IsCurrentCategory = true;
                }
                category.Save();
            }
            WorkCurveHelper.CategoryCurrent = currentCategory;
            MessageBox.Show(Info.strSpecifications);

            //EDXRFHelper.GotoMainPage(this);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void dgvExample_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvExample.Rows.Count > 0)
            {
                int index = dgvExample.SelectedRows[0].Index;
                currentExample = currentCategory.Specifications[index];
                bindElements();
            }
            else
            {
                dgvElements.Rows.Clear();
            }
        }

        private void BindCbo()
        {
            cboCategory.Items.Clear();
            foreach (var a in lstCategory)
            {
                cboCategory.Items.Add(a.CategoryName);
            }
            if (WorkCurveHelper.CategoryCurrent != null)
            {
                cboCategory.SelectedItem = lstCategory.Find(l => l.IsCurrentCategory == true).CategoryName;
            }
            else if (cboCategory.Items.Count > 0)
            {
                cboCategory.SelectedIndex = 0;
            }
        }

        private void bindExample()
        {
            BindingSource bs = new BindingSource();
            for (int i = 0; i < currentCategory.Specifications.Count; i++)
            {
                bs.Add(currentCategory.Specifications[i]);
            }
            dgvExample.AutoGenerateColumns = false;
            dgvExample.DataSource = bs;

            dgvExample_CellContentClick(null, null);
        }

        private void bindElements()
        {
            BindingSource bs = new BindingSource();
            for (int i = 0; i < currentExample.IncludeElements.Count; i++)
            {
                bs.Add(currentExample.IncludeElements[i]);
            }
            dgvElements.AutoGenerateColumns = false;
            dgvElements.DataSource = bs;
        }

        /// <summary>
        /// 增加规格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddExample_Click(object sender, EventArgs e)
        {
            SpecificationsExample specificationsExample = SpecificationsExample.New;
            specificationsExample.Category = currentCategory;
            specificationsExample.ExampleName = "";
            specificationsExample.ExampleType = "";
            specificationsExample.CreateExampleDate = DateTime.Now.Date;
            specificationsExample.UpdateExampleDate = DateTime.Now.Date;
            //specificationsExample.Save();
            currentCategory.Specifications.Add(specificationsExample);
            bindExample();
        }

        /// <summary>
        /// 删除规格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteExample_Click(object sender, EventArgs e)
        {
            int count = dgvExample.SelectedRows.Count;
            int index = dgvExample.SelectedRows[0].Index;
            if (count > 0)
            {
                //currentExample.Delete();
                currentCategory.Specifications.RemoveAt(index);
                bindExample();
            }
        }

        /// <summary>
        /// 增加元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddElement_Click(object sender, EventArgs e)
        {
            ElementTableAtom table = new ElementTableAtom();

            string[] strs = new string[dgvElements.Rows.Count];
            for (int i = 0; i < dgvElements.Rows.Count; i++)
            {
                strs[i] = dgvElements.Rows[i].Cells[0].Value.ToString();
            }

            table.MultiSelect = true;
            table.SelectedItems = strs;

            WorkCurveHelper.OpenUC(table, false, Info.SelectElement,true);//打开元素周期表

            if (table.SelectedItems != null && table.SelectedItems.Length > 0)
            {
                var atomnames = table.SelectedItems;
                bool hasElement = false;
                for (int i = 0; i < atomnames.Length; i++)//遍历新数组
                {
                    hasElement = false;

                    for (int j = 0; j < currentExample.IncludeElements.Count; j++)
                    {
                        if (currentExample.IncludeElements[j].ElementName.Equals(atomnames[i]))
                        {
                            hasElement = true;
                            break;
                        }
                    }
                    if (!hasElement)
                    {
                        SpecificationElement specificationElement = SpecificationElement.New;
                        specificationElement.ElementName = atomnames[i];
                        specificationElement.SpecificationsExample = currentExample;
                        specificationElement.MaxValue = 0;
                        specificationElement.MinValue = 0;
                        currentExample.IncludeElements.Add(specificationElement);
                    }
                }
                for (int i = currentExample.IncludeElements.Count - 1; i >= 0; i--)//遍历增加后的元素列表
                {
                    hasElement = false;
                    for (int j = 0; j < atomnames.Length; j++)
                    {
                        if (atomnames[j].Equals(currentExample.IncludeElements[i].ElementName))
                        {
                            hasElement = true;
                            break;
                        }
                    }
                    if (!hasElement)
                    {
                        currentExample.IncludeElements.RemoveAt(i);
                    }
                }
            }
            else
            {
                currentExample.IncludeElements.Clear();
            }
            bindElements();
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteElement_Click(object sender, EventArgs e)
        {
            int count = dgvElements.SelectedRows.Count;
            if (count <= 0)
            {
                SkyrayMsgBox.Show(Info.NoSelect);
                return;
            }
            for (int i = dgvElements.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvElements.SelectedRows[i].Index;
                currentExample.IncludeElements.RemoveAt(index);
            }
            bindElements();//刷新标准列表
        }

        private void dgvElements_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvElements.Columns[e.ColumnIndex].Name.Equals("ExampleMaxValue") || dgvElements.Columns[e.ColumnIndex].Name.Equals("ExampleMinValue"))
            {
                decimal Max = Ranges.RangeDictionary[dgvElements.Columns[e.ColumnIndex].Name].Max;
                decimal Min = Ranges.RangeDictionary[dgvElements.Columns[e.ColumnIndex].Name].Min;
                try { double.Parse(e.FormattedValue.ToString()); }
                catch (Exception)
                {
                    e.Cancel = true;
                    dgvElements.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    return;
                }
                if (double.Parse(e.FormattedValue.ToString()) > double.Parse(Max.ToString()))
                {
                    dgvElements.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    e.Cancel = true;
                }
                if (double.Parse(e.FormattedValue.ToString()) < double.Parse(Min.ToString()))
                {
                    dgvElements.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    e.Cancel = true;
                }
                if (dgvElements.Columns[e.ColumnIndex].Name.Equals("ExampleMaxValue"))
                {
                    if (double.Parse(dgvElements["ExampleMinValue", e.RowIndex].Value.ToString()) > double.Parse(e.FormattedValue.ToString()))
                    {
                        dgvElements.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                        e.Cancel = true;
                    }
                    return;
                }
                if (dgvElements.Columns[e.ColumnIndex].Name.Equals("ExampleMinValue"))
                {
                    if (double.Parse(dgvElements["ExampleMaxValue", e.RowIndex].Value.ToString()) < double.Parse(e.FormattedValue.ToString()))
                    {
                        dgvElements.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                        e.Cancel = true;
                    }
                    return;
                }
            }
        }
    }
}
