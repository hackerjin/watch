using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Print
{
    public partial class FrmTreeNodeInfo : Skyray.Language.MultipleForm
    {
        public TreeNodeInfo add =new TreeNodeInfo();
        public List<TreeNodeInfo> lstTreeNodeInfos = new List<TreeNodeInfo>();
        public Image image;
        public FrmTreeNodeInfo()
        {
            InitializeComponent();
            LoadLocalDataSource();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            add.GroupName = txtGroup.Text;
            add .Name = txtName.Text;
            add.Caption = txtText.Text;//txtLabel.Text;
            add.Type =(CtrlType) Enum.Parse(typeof(CtrlType),cboCtrlType.Text);
            switch (add.Type)
            {                
                case CtrlType.Label:
                    add.Text = txtLabel.Text;//txtText.Text;            
                    break;
                case CtrlType.Field:
                    break;
                case CtrlType.Image:
                    add.Image = image;
                    break;
                case CtrlType.Grid:
                    break;
                case CtrlType.ComposeTable:
                    break;
                
            }

            if (!Skyray.EDX.Common.ValidateHelper.IllegalCheck(txtName))
            {
                txtName.Focus();
                txtName.SelectAll();
                return;
            }
            if (!Skyray.EDX.Common.ValidateHelper.IllegalCheck(txtText))
            {
                txtText.Focus();
                txtText.SelectAll();
                return;
            }
            if (lstTreeNodeInfos.Find(w => w.Name == add.Name && w.Type == add.Type) == null)
                lstTreeNodeInfos.Add(add);
            else
            {
                Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.TreeNodeExists);
                return;
            }
            PrintHelper.ObjToFile(lstTreeNodeInfos, AppDomain.CurrentDomain.BaseDirectory + "LocalDataSource" + LanguageShortName + ".tni");
            this.DialogResult = DialogResult.OK;
            this.Close();    
        }        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();            
        }

        private void btnPicPath_Click(object sender, EventArgs e)
        {
            openFileDialogPicture.Filter = "*.bmp;*.gif;*.jpg;*.png|*.bmp;*.gif;*.jpg;*.png";
            if(openFileDialogPicture.ShowDialog()== DialogResult.OK)
            {
                image = Image.FromFile(openFileDialogPicture.FileName) ;
            }
        }

        private void FrmTreeNodeInfo_Load(object sender, EventArgs e)
        {
            //this.cboCtrlType.Items.AddRange(Enum.GetNames (typeof(CtrlType)));
            this.cboCtrlType.Items.Add(CtrlType.Label);
            //this.cboCtrlType.Items.Add(CtrlType.Field);
            this.cboCtrlType.Items.Add(CtrlType.Image);
            //this.cboCtrlType.Items.Add(CtrlType.Grid);
            //this.cboCtrlType.Items.Add(CtrlType.ComposeTable);
            this.cboCtrlType.SelectedIndex = this.cboCtrlType.Items.IndexOf(CtrlType.Image);            
        }

        public string LanguageShortName
        {
            get
            {
                string Language = "";
                Language = Skyray.Language.Lang.Model.CurrentLang.ShortName;
                if (Language.ToLower() == "english") Language = "EN";
                else if (Language.ToLower() == "chinese") Language = "CN";
                return Language;
            }
        }

        public void LoadLocalDataSource()
        {
            lstTreeNodeInfos = PrintHelper.FileToObj<List<TreeNodeInfo>>(AppDomain.CurrentDomain.BaseDirectory + "LocalDataSource" + LanguageShortName + ".tni");
            if (lstTreeNodeInfos == null)
                lstTreeNodeInfos = new List<TreeNodeInfo>();
        }

        public void DeleteTreeNodeInfo(TreeNodeInfo node)
        {
            if (lstTreeNodeInfos.Find(w => w.Name == node.Name && w.Type == node.Type)!=null)
            {
                lstTreeNodeInfos.Remove(lstTreeNodeInfos.Find(w => w.Name == node.Name && w.Type == node.Type));
            }
            PrintHelper.ObjToFile(lstTreeNodeInfos, AppDomain.CurrentDomain.BaseDirectory + "LocalDataSource" + LanguageShortName + ".tni");
        }

        private void cboCtrlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((CtrlType) Enum.Parse(typeof(CtrlType),cboCtrlType.Text))
            {
                case CtrlType.Label:
                    txtLabel.Enabled = true;
                    btnPicPath.Enabled = false;
                    break;
                case CtrlType.Field:
                    btnPicPath.Enabled = false;
                    txtLabel.Enabled = false;
                    break;
                case CtrlType.Image:
                    btnPicPath.Enabled = true;
                    txtLabel.Enabled = false;
                    break;
                case CtrlType.Grid:
                case CtrlType.ComposeTable:
                    btnPicPath.Enabled = false;
                    txtLabel.Enabled = false;
                    break;
            }
        }
    }
}
