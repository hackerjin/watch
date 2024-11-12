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
    public partial class UCDefinePureElem : Skyray.Language.UCMultiple
    {
        #region Fields

        /// <summary>
        /// 数据库中查询所得的信息列表
        /// </summary>
        DbObjectList<DefinePureElement> lstDefinePureElement;

        private List<DefinePureElement> lstReadyDel;
        #endregion

        #region Init
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCDefinePureElem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCDefinePureElem_Load(object sender, EventArgs e)
        {

            lstDefinePureElement = DefinePureElement.FindAll();//查询所有自定义元素名称
            InstanceDGV();//加载供应商
            lstReadyDel = new List<DefinePureElement>();
        }

        /// <summary>
        /// 初始化供应商
        /// </summary>
        private void InstanceDGV()
        {
            BindingSource bs = new BindingSource();
            foreach (DefinePureElement sup in lstDefinePureElement)
            {
                bs.Add(sup);
            }
            dgvDefineElem.AutoGenerateColumns = false;
            dgvDefineElem.DataSource = bs;//绑定数据源
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
            if (ValidateHelper.IllegalCheck(txtElementName))
            {
                if (lstDefinePureElement.Find(l => l.Name == txtElementName.Text) != null)
                {
                    SkyrayMsgBox.Show(Info.ExistName);//命名重复
                }
                else
                {
                    var v = DefinePureElement.New.Init(txtElementName.Text, DateTime.Today.ToShortDateString());
                    lstDefinePureElement.Add(v);
                    InstanceDGV();
                }
            }
            txtElementName.Text = "";
        }
        /// <summary>
        /// 删除供应商
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvDefineElem.SelectedRows.Count <= 0)
            {
                SkyrayMsgBox.Show(Info.NoSelect);
            }
            else
            {
                int index = dgvDefineElem.SelectedRows[0].Index;
                if (lstDefinePureElement[index].Id != 0)
                {
                    lstReadyDel.Add(lstDefinePureElement[index]);
                }
                lstDefinePureElement.RemoveAt(index);
                InstanceDGV();
                int count = lstDefinePureElement.Count;
                if (count > 1)
                {
                    if (index >= count)
                    {
                        this.dgvDefineElem.Rows[0].Selected = false;
                        this.dgvDefineElem.Rows[count - 1].Selected = true;
                    }
                    else
                    {
                        this.dgvDefineElem.Rows[0].Selected = false;
                        this.dgvDefineElem.Rows[index].Selected = true;
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
            foreach (var v in lstDefinePureElement)
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
            foreach (var v in lstDefinePureElement)
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
