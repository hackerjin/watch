using System;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Lephone.Data.Common;
using Skyray.Controls;
using Skyray.EDX.Common;
using System.Collections.Generic;

namespace Skyray.UC
{
    /// <summary>
    /// 供应商类
    /// </summary>
    public partial class UCSupplier : Skyray.Language.UCMultiple
    {
        #region Fields

        /// <summary>
        /// 数据库中查询所得的信息列表
        /// </summary>
        DbObjectList<Supplier> lstSupplier;

        private List<Supplier> lstReadyDel;
        #endregion

        #region Init
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCSupplier()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCSupplier_Load(object sender, EventArgs e)
        {
            lstSupplier = Supplier.FindAll();//查询所有供应商
            InstanceDGV();//加载供应商
            lstReadyDel = new List<Supplier>();
        }

        /// <summary>
        /// 初始化供应商
        /// </summary>
        private void InstanceDGV()
        {
            BindingSource bs = new BindingSource();
            foreach (Supplier sup in lstSupplier)
            {
                bs.Add(sup);
            }
            dgvSupplier.AutoGenerateColumns = false;
            dgvSupplier.DataSource = bs;//绑定数据源
        }

        #endregion

        #region Event
        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateHelper.IllegalCheck(txtSupplier))
            {
                if (lstSupplier.Find(l => l.Name == txtSupplier.Text) != null)
                {
                    SkyrayMsgBox.Show(Info.ExistName);//命名重复
                }
                else
                {
                    var v = Supplier.New.Init(txtSupplier.Text, DateTime.Today.ToShortDateString());
                    lstSupplier.Add(v);
                    InstanceDGV();
                }
            }
            txtSupplier.Text = "";
        }
        /// <summary>
        /// 删除供应商
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvSupplier.SelectedRows.Count <= 0)
            {
                SkyrayMsgBox.Show(Info.NoSelect);
            }
            else
            {
                int index = dgvSupplier.SelectedRows[0].Index;
                if (lstSupplier[index].Id != 0)
                {
                    lstReadyDel.Add(lstSupplier[index]);
                }
                lstSupplier.RemoveAt(index);
                InstanceDGV();
                int count = lstSupplier.Count;
                if (count > 1)
                {
                    if (index >= count)
                    {
                        this.dgvSupplier.Rows[0].Selected = false;
                        this.dgvSupplier.Rows[count - 1].Selected = true;
                    }
                    else
                    {
                        this.dgvSupplier.Rows[0].Selected = false;
                        this.dgvSupplier.Rows[index].Selected = true;
                    }
                }
            }
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (var v in lstSupplier)
            {
                v.Save();
            }
            foreach (var v in lstReadyDel)
            {
                v.Delete();
            }
            EDXRFHelper.GotoMainPage(this);//转到主界面
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//转到主界面
        }

        #endregion

        private void btnApplication_Click(object sender, EventArgs e)
        {
            foreach (var v in lstSupplier)
            {
                v.Save();
            }
            foreach (var v in lstReadyDel)
            {
                v.Delete();
            }
        }
    }
}
