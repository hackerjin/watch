using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lephone.Data;
using Skyray.EDXRFLibrary;
using Skyray.Controls;
using Skyray.EDX.Common;


namespace Skyray.UC
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public partial class FrmUser : Skyray.Language.UCMultiple
    {
        /// <summary>
        /// 保存对象
        /// </summary>
        private List<string> listString = new List<string>();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public FrmUser()
        {
            InitializeComponent();
            Right.DataSource = new string[] { Info.Administrator, Info.StandandUser, Info.CommonUser };
        }
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void PageLoad(object sender, EventArgs e)
        {
            base.PageLoad(sender, e);
        }

        /// <summary>
        /// 删除用户登录记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewUser.CurrentRow != null)
            {
                if (DialogResult.No == Msg.Show(Info.ConfirmDel, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
                if (this.dataGridViewUser.CurrentRow.Cells["Id"].Value != null)
                {
                    var str = this.dataGridViewUser.CurrentRow.Cells["Id"].Value.ToString();
                    if (string.IsNullOrEmpty(str)) return;
                    long id = long.Parse(str);
                    User user = User.FindOne(w => w.Name == FrmLogon.userName);
                    if (user == null || user.Id == id)
                        return;
                    User.DeleteAll(w => w.Id == id);
                }
                this.dataGridViewUser.Rows.Remove(this.dataGridViewUser.CurrentRow);
                this.dataGridViewUser.Refresh();
            }
        }

        /// <summary>
        /// 确认重新保存用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWSubmit_Click(object sender, EventArgs e)
        {
            this.listString.Clear();
            if (this.dataGridViewUser.Rows.Count > 0)
            {
                foreach (DataGridViewRow rows in this.dataGridViewUser.Rows)
                {
                    if (InvalidateDataGridRow(rows))
                    {
                        Msg.Show(Info.ValidateUserInput, Info.WarningUserInput, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                foreach (DataGridViewRow rows in this.dataGridViewUser.Rows)
                {
                    User user = User.New;
                    if (rows.Cells["Id"].Value != null)
                    {
                        long id = long.Parse(rows.Cells["Id"].Value.ToString());
                        User.DeleteAll(w => w.Id == id);
                    }
                    user.Name = rows.Cells["UserName"].Value.ToString();
                    user.Password = rows.Cells["Password"].Value == null?"":rows.Cells["Password"].Value.ToString();
                    user.RePassword = rows.Cells["Password"].Value == null ? "" : rows.Cells["Password"].Value.ToString();
                    Role role = Role.New;
                    role.RoleName = rows.Cells["Right"].Value.ToString();
                    role.RoleType = (role.RoleName == Info.Administrator ? 0 : (role.RoleName == Info.StandandUser ? 1 : 2));
                    user.Role = role;
                    user.Save();
                }
            }
            EDXRFHelper.GotoMainPage(this);
        }

        /// <summary>
        /// 验证数据容器的单元格
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private bool InvalidateDataGridRow(DataGridViewRow rows)
        {
            if (rows.Cells.Count > 0)
            {
                for (int i = 1; i < rows.Cells.Count; i++)
                {
                    if ((rows.Cells[i].Value == null && i != 2) ||
                        i == 1 && listString.Find(delegate(string str) {return String.Compare(str,rows.Cells["UserName"].Value.ToString(),true)==0;})!= null)
                         return true;
                    else if (i == 1)
                        listString.Add(rows.Cells["UserName"].Value.ToString());
                }
            }
            return false;
        }

        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
           
        }

        /// <summary>
        /// 加载用户管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmUser_Load(object sender, EventArgs e)
        {
            List<User> listUser = User.FindAll();

            if (listUser.Count == 0)
                return;
            foreach (User user in listUser)
            {
                this.dataGridViewUser.Rows.Add(new string[] {user.Id.ToString(), 
                user.Name, 
                user.Password, 
                user.Role == null ? "" 
                :  (user.Role.RoleType==0?Info.Administrator: 
                (user.Role.RoleType==1?Info.StandandUser:Info.CommonUser))                   
            });
            }
            //User userd = User.FindOne(w => w.Name == FrmLogon.userName);
            //if (userd.Role.RoleType != 0)
            //{
            //    this.dataGridViewUser.Enabled = false;
            //    this.buttonWAdd.Enabled = false;
            //    this.buttonWCancel.Enabled = false;
            //    this.buttonWDelete.Enabled = false;
            //    this.buttonWSubmit.Enabled = false;
            //}
            this.dataGridViewUser.Refresh();
        }

        /// <summary>
        /// 增加一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWAdd_Click(object sender, EventArgs e)
        {
            this.dataGridViewUser.Rows.Add();
            int index = this.dataGridViewUser.Rows.Count - 1;
            this.dataGridViewUser["Right", index].Value = Info.CommonUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grouper"></param>
        /// <param name="flag"></param>
        private void UnCheckedRadio(Grouper grouper, bool flag)
        {
            if (grouper == null)
                return;
            foreach (Control control in grouper.Controls)
            {
                if (control is RadioButton)
                    (control as RadioButton).Checked = flag;
            }
        }
    }
}
