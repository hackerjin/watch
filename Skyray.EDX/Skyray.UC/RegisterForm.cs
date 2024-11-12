using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
namespace Skyray.UC
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            serialTextBox.Text = RegisterLibrary.Register.SerialGenerate();
        } 

        private void registerButton_Click(object sender, EventArgs e)
        {
            string code = RegisterLibrary.Register.LicenselGenerate(serialTextBox.Text);
            if (code.Equals(licenseTextBox.Text.Trim()))
            {
               //注册成功 保存注册码到注册表
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);
                key = key.CreateSubKey("Skyray");
                key.SetValue("Skyray", code);                
                MessageBox.Show("注册成功！");
                this.DialogResult = DialogResult.OK;
            }
            else MessageBox.Show("注册失败！");
        }
    }
}
