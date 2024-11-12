using System;
using System.Threading;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Lephone.Data.Common;
using Skyray.Controls;
using Skyray.EDX.Common.Component;
using System.Data;
using System.Data.SQLite;
using Lephone.Data;
using Microsoft.Win32;
using Skyray.Dog;

namespace Skyray.UC
{
    /// <summary>
    /// 登入界面
    /// </summary>
    public partial class FrmLogon : Skyray.Language.MultipleForm
    {
        /// <summary>
        /// 
        /// </summary>
        public static string userName;

        ///// <summary>
        ///// 扫描对象
        ///// </summary>
        //public DeviceMeasure deviceMeasure = new DeviceMeasure();

        /// <summary>
        /// 
        /// </summary>
        private DbObjectList<User> lstUser = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// 
        public FrmLogon()
        {
            //if (WorkCurveHelper.type != InterfaceType.NetWork)
            //    MotorInstance.LoadDLL(MotorInstance.UpdateKeyFile,null);

            InitializeComponent();
            LoadP();
            RegistrySoftWarePath();
            if (DifferenceDevice.IsXRF)
                this.Icon = new System.Drawing.Icon("ICO/XRF.ico");
            else if (DifferenceDevice.IsRohs)
                this.Icon = new System.Drawing.Icon("ICO/RoHS.ico");
            else if (DifferenceDevice.IsAnalyser)
                this.Icon = new System.Drawing.Icon("ICO/mainicon.ico");
            else if (DifferenceDevice.IsThick)
                this.Icon = new System.Drawing.Icon("ICO/FpThick_Logo.ico");
            btnActiveShow();

        }


        public static void LoadP()
        {
            if (!InitSysParams.EncryptDBControl())
            {
                Environment.Exit(1);
            }
            bool bInitSuccess = InitSysParams.Init();  //500ms
            if (!bInitSuccess)
            {
                Msg.Show(Info.StartInitFailure, Info.WarningInitInfo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //1054ms

            DifferenceDevice.CreateInstance();
            WorkCurveHelper.deviceMeasure = new DeviceMeasure();
            DifferenceDevice.interClassMain.InitalizeInterface();
            //if (!InitSysParams.ComTypeControl())
            //{

            //    Environment.Exit(1);
            //}

        }

        /// <summary>
        /// 注册表中设置安装目录
        /// </summary>
        public static void RegistrySoftWarePath()
        {
            try
            {
                if (GetRegeditItemPath() != AppDomain.CurrentDomain.BaseDirectory)
                    SetRegitItemPath();
            }
            catch
            { }
        }
        /// <summary>
        /// 判断是否存在Skyray节点
        /// </summary>
        /// <returns></returns>
        private static string GetRegeditItemPath()
        {
            string[] subkeyNames;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE");
            RegistryKey skyray = software.OpenSubKey("SKYRAY");
            if (skyray == null) return "";
            subkeyNames = skyray.GetValueNames();//skyray.GetSubKeyNames();
            foreach (string keyName in subkeyNames)   //遍历整个数组  
            {
                if (keyName == "Path") //判断Value的名称  
                {
                    string value = (string)skyray.GetValue("Path");
                    skyray.Close();
                    software.Close();
                    hkml.Close();
                    return value;
                }
            }
            skyray.Close();
            software.Close();
            hkml.Close();
            return "";
        }
        private static void SetRegitItemPath()
        {
            RegistryKey software = Registry.LocalMachine.OpenSubKey("SoftWare");
            RegistryKey skyray = software.OpenSubKey("Skyray", true);
            if (skyray == null)
                skyray = Registry.LocalMachine.CreateSubKey("SoftWare/Skyray", RegistryKeyPermissionCheck.ReadWriteSubTree);
            skyray.SetValue("Path", AppDomain.CurrentDomain.BaseDirectory, RegistryValueKind.String);
            skyray.Close();
            software.Close();
        }
        /// <summary>
        /// 得到用户列表
        /// </summary>
        void GetUserList()
        {
            string sql = "select * from User order by Id desc";
            lstUser = User.FindBySql(sql);


            if (lstUser.Count == 0)
            {
                var admin = User.New.Init("Admin", "", "");
                // Lephone.Util.Logging.ConsoleMessageRecorder
                var roles = Role.FindAll();
                Role roleAdmin = null;
                if (roles.Count == 0)
                {
                    roleAdmin = Role.New.Init(Info.Administrator, 0);
                    roleAdmin.Save();//保存
                    Role.New.Init(Info.StandandUser, 1).Save();
                    Role.New.Init(Info.CommonUser, 2).Save();
                    admin.Role = roleAdmin;
                }
                else
                {
                    admin.Role = roles.Find(r => r.RoleType == 0);
                }
                admin.Save();

                lstUser.Add(admin);
            }
        }

        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        void BindToComboBox()
        {
            BeginInvoke((MethodInvoker)delegate
            {
                cboUser.DataSource = lstUser;
                cboUser.DisplayMember = "Name";
                cboUser.ValueMember = "Password";
                cboUser.ColumnNames = "Name";
                cboUser.ColumnWidths = this.cboUser.Width.ToString();
                btnLogon.Enabled = true;
            });
        }

        /// <summary>
        /// 异步获取用户名及密码列表
        /// </summary>
        private void AsyncGetUserList()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                GetUserList();
                BindToComboBox();
            });
        }

        /// <summary>
        /// 点击登入界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogon_Click(object sender, EventArgs e)
        {


            if (cboUser.SelectedValue.ToString() == this.txtPwd.Text)
            {
                DialogResult = DialogResult.OK;
                GP.UserName = userName = cboUser.Text;
                User user = lstUser.Find(w => w.Name == userName);
                if (user != null)
                {
                    GP.CurrentUser = user;
                }
                Close();
            }
            else
            {
                Msg.Show(Info.PWDError);//提示密码错误
                txtPwd.Focus();//聚焦
                txtPwd.SelectAll();//选择全部文本
            }


        }

        /// <summary>
        /// 加载登入界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmLogon_Load(object sender, EventArgs e)
        {
            AsyncGetUserList();
        }

        private void btnActive_Click(object sender, EventArgs e)
        {

            bool isInSevenDays = false;
            string surPlus = string.Empty;
            int type = (int)HardwareDog.SNConfirm(WorkCurveHelper.snFilePath, out isInSevenDays, out surPlus);
            HardwareDog.Deadline = surPlus;   //deadline赋值
            if (type != -2 || type != -3)
            {

                FrmHardwareDogCA fhd = new FrmHardwareDogCA(type);
                fhd.ShowDialog();
            }
        }


        private void btnActiveShow()
        {
            bool isInSevenDays = false;
            string surPlus = string.Empty;
            int type = (int)HardwareDog.SNConfirm(WorkCurveHelper.snFilePath, out isInSevenDays, out surPlus);
            HardwareDog.Deadline = surPlus;   //deadline赋值
            if (type == 1)
            {
                FrmHardwareDogCA fhd = new FrmHardwareDogCA(type);
                fhd.ShowDialog();
            }
            ////取消硬件加密管控,改为在连接仪器后读取接口板信息
            //else if (type == -2 || type == -3)
            //{
            //    Msg.Show(Info.FailedToGetDevInfo);
            //    Environment.Exit(1);
            //}

            if (WorkCurveHelper.IsShowEncryptButton)
            {
                if (type == -3)
                {
                    this.btnActive.Visible = false;
                }
                else
                {
                    this.btnActive.Visible = true;
                }
            }
            else
            {
                this.btnActive.Visible = false;
            }


        }





    }
}