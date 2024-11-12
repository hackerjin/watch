using System;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Lephone.Data.Common;
using Skyray.Controls;
using Skyray.EDX.Common;
using System.Collections.Generic;

namespace Skyray.UC
{
    public partial class UCShape : Skyray.Language.UCMultiple
    {

        #region Fields

        /// <summary>
        /// 数据库中查询所得的信息列表
        /// </summary>
        DbObjectList<Shape> lstShape;

        private List<Shape> lstReadyDel;

        #endregion

        #region Init
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCShape()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shape_Load(object sender, EventArgs e)
        {
            lstShape = Shape.FindAll();//查询形状
            InstanceDGV();//初始化数据
            lstReadyDel = new List<Shape>();
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InstanceDGV()
        {
            BindingSource bs = new BindingSource();
            foreach (Shape shape in lstShape)
            {
                bs.Add(shape);
            }
            dgvShape.AutoGenerateColumns = false;
            dgvShape.DataSource = bs;//绑定数据源
        }

        #endregion

        #region Events
        /// <summary>
        /// 添加形状
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateHelper.IllegalCheck(txtShape))
            {
                if (lstShape.Find(l => l.Name == txtShape.Text) != null)
                {
                    SkyrayMsgBox.Show(Info.ExistName);//命名重复
                }
                else
                {
                    var v = Shape.New.Init(txtShape.Text, DateTime.Today.ToShortDateString());
                    lstShape.Add(v);
                    InstanceDGV();
                }
            }
            txtShape.Text = "";
        }
        /// <summary>
        /// 删除形状
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvShape.SelectedRows.Count <= 0)
            {
                SkyrayMsgBox.Show(Info.NoSelect);
            }
            else
            {
                int index = dgvShape.SelectedRows[0].Index;
                if (lstShape[index].Id != 0)
                {
                    lstReadyDel.Add(lstShape[index]);
                }
                lstShape.RemoveAt(index);
                InstanceDGV();

                int count = lstShape.Count;
                if (count > 1)
                {
                    if (index >= count)
                    {
                        this.dgvShape.Rows[0].Selected = false;
                        this.dgvShape.Rows[count - 1].Selected = true;
                    }
                    else
                    {
                        this.dgvShape.Rows[0].Selected = false;
                        this.dgvShape.Rows[index].Selected = true;
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
            foreach (var v in lstShape)
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
            foreach (var v in lstShape)
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
