namespace Skyray.UC
{
    partial class ToolsNaviForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Skyray.Controls.Office2007Renderer office2007Renderer1 = new Skyray.Controls.Office2007Renderer();
            Skyray.Controls.Office2007Renderer office2007Renderer2 = new Skyray.Controls.Office2007Renderer();
            this.buttonWSubmit = new Skyray.Controls.ButtonW();
            this.buttonWCancel = new Skyray.Controls.ButtonW();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TlsMenuItemUp = new System.Windows.Forms.ToolStripMenuItem();
            this.TlsMenuItemDown = new System.Windows.Forms.ToolStripMenuItem();
            this.btnApplication = new Skyray.Controls.ButtonW();
            this.tbPage1 = new Skyray.Controls.TabControlW();
            this.tpPageMainSetting = new System.Windows.Forms.TabPage();
            this.containerObject1 = new Skyray.EDX.Common.ContainerObject();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.grpSettings = new Skyray.Controls.Grouper();
            this.grpShortcutKey = new Skyray.Controls.Grouper();
            this.cmbModifier = new System.Windows.Forms.ComboBox();
            this.cmbKey = new System.Windows.Forms.ComboBox();
            this.chbEnabelControl = new Skyray.Controls.CheckBoxW();
            this.buttonSetting = new System.Windows.Forms.Button();
            this.textBoxW1 = new Skyray.Controls.TextBoxW();
            this.lblItemText = new Skyray.Controls.LabelW();
            this.chkIsInMenu = new Skyray.Controls.CheckBoxW();
            this.numericTextBox1 = new Skyray.Controls.NumericTextBox();
            this.lblItemType = new Skyray.Controls.LabelW();
            this.chkIsInTools = new Skyray.Controls.CheckBoxW();
            this.chkIsInMainForm = new Skyray.Controls.CheckBoxW();
            this.tbPageTools = new System.Windows.Forms.TabPage();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.containerObject2 = new Skyray.EDX.Common.ContainerObject();
            this.txtSaveFile = new Skyray.Controls.TextBoxW();
            this.lbWRolePath = new Skyray.Controls.LabelW();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lbWRoleCategory = new Skyray.Controls.LabelW();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1.SuspendLayout();
            this.tbPage1.SuspendLayout();
            this.tpPageMainSetting.SuspendLayout();
            this.containerObject1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpSettings.SuspendLayout();
            this.grpShortcutKey.SuspendLayout();
            this.tbPageTools.SuspendLayout();
            this.containerObject2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonWSubmit
            // 
            this.buttonWSubmit.bSilver = false;
            this.buttonWSubmit.Location = new System.Drawing.Point(446, 538);
            this.buttonWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.Name = "buttonWSubmit";
            this.buttonWSubmit.Size = new System.Drawing.Size(75, 25);
            this.buttonWSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWSubmit.TabIndex = 3;
            this.buttonWSubmit.Text = "确定";
            this.buttonWSubmit.ToFocused = false;
            this.buttonWSubmit.UseVisualStyleBackColor = true;
            this.buttonWSubmit.Click += new System.EventHandler(this.buttonWSubmit_Click);
            // 
            // buttonWCancel
            // 
            this.buttonWCancel.bSilver = false;
            this.buttonWCancel.Location = new System.Drawing.Point(553, 538);
            this.buttonWCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWCancel.Name = "buttonWCancel";
            this.buttonWCancel.Size = new System.Drawing.Size(75, 25);
            this.buttonWCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWCancel.TabIndex = 4;
            this.buttonWCancel.Text = "取消";
            this.buttonWCancel.ToFocused = false;
            this.buttonWCancel.UseVisualStyleBackColor = true;
            this.buttonWCancel.Click += new System.EventHandler(this.buttonWCancel_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TlsMenuItemUp,
            this.TlsMenuItemDown});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // TlsMenuItemUp
            // 
            this.TlsMenuItemUp.Name = "TlsMenuItemUp";
            this.TlsMenuItemUp.Size = new System.Drawing.Size(100, 22);
            this.TlsMenuItemUp.Text = "上移";
            this.TlsMenuItemUp.Click += new System.EventHandler(this.TlsMenuItemUp_Click);
            // 
            // TlsMenuItemDown
            // 
            this.TlsMenuItemDown.Name = "TlsMenuItemDown";
            this.TlsMenuItemDown.Size = new System.Drawing.Size(100, 22);
            this.TlsMenuItemDown.Text = "下移";
            this.TlsMenuItemDown.Click += new System.EventHandler(this.TlsMenuItemDown_Click);
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(339, 538);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(75, 25);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 7;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // tbPage1
            // 
            this.tbPage1.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(79)))), ((int)(((byte)(125)))));
            this.tbPage1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.tbPage1.Controls.Add(this.tpPageMainSetting);
            this.tbPage1.Controls.Add(this.tbPageTools);
            this.tbPage1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbPage1.Location = new System.Drawing.Point(8, 76);
            this.tbPage1.Name = "tbPage1";
            this.tbPage1.SelectedIndex = 0;
            this.tbPage1.ShowTabs = true;
            this.tbPage1.Size = new System.Drawing.Size(665, 456);
            this.tbPage1.Style = Skyray.Controls.Style.Office2007Blue;
            this.tbPage1.TabIndex = 10;
            this.tbPage1.SelectedIndexChanged += new System.EventHandler(this.tbPage1_SelectedIndexChanged);
            // 
            // tpPageMainSetting
            // 
            this.tpPageMainSetting.Controls.Add(this.containerObject1);
            this.tpPageMainSetting.Location = new System.Drawing.Point(4, 26);
            this.tpPageMainSetting.Name = "tpPageMainSetting";
            this.tpPageMainSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tpPageMainSetting.Size = new System.Drawing.Size(657, 426);
            this.tpPageMainSetting.TabIndex = 0;
            this.tpPageMainSetting.Text = "菜单栏";
            this.tpPageMainSetting.UseVisualStyleBackColor = true;
            // 
            // containerObject1
            // 
            this.containerObject1.AutoScroll = true;
            this.containerObject1.BigImage = null;
            this.containerObject1.ContainerAttribute = false;
            this.containerObject1.ContainerLabel = null;
            this.containerObject1.ControlInternal = 0;
            this.containerObject1.Controls.Add(this.splitContainer1);
            this.containerObject1.CurrentPanelIndex = 0;
            this.containerObject1.CurrentPlanningNumber = 0;
            this.containerObject1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerObject1.IncludeInnerCoordinate = false;
            this.containerObject1.IsReverseEmbeded = false;
            this.containerObject1.Location = new System.Drawing.Point(3, 3);
            this.containerObject1.Name = "containerObject1";
            this.containerObject1.Name1 = null;
            office2007Renderer1.RoundedEdges = true;
            this.containerObject1.Renderer = office2007Renderer1;
            this.containerObject1.Size = new System.Drawing.Size(651, 420);
            this.containerObject1.SmallImage = null;
            this.containerObject1.Style = Skyray.Controls.Style.Custom;
            this.containerObject1.TabIndex = 5;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grpSettings);
            this.splitContainer1.Size = new System.Drawing.Size(651, 420);
            this.splitContainer1.SplitterDistance = 215;
            this.splitContainer1.TabIndex = 2;
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(217, 451);
            this.treeView1.TabIndex = 9;
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
            // 
            // grpSettings
            // 
            this.grpSettings.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpSettings.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpSettings.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpSettings.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpSettings.BorderThickness = 1F;
            this.grpSettings.BorderTopOnly = false;
            this.grpSettings.Controls.Add(this.grpShortcutKey);
            this.grpSettings.Controls.Add(this.chbEnabelControl);
            this.grpSettings.Controls.Add(this.buttonSetting);
            this.grpSettings.Controls.Add(this.textBoxW1);
            this.grpSettings.Controls.Add(this.lblItemText);
            this.grpSettings.Controls.Add(this.chkIsInMenu);
            this.grpSettings.Controls.Add(this.numericTextBox1);
            this.grpSettings.Controls.Add(this.lblItemType);
            this.grpSettings.Controls.Add(this.chkIsInTools);
            this.grpSettings.Controls.Add(this.chkIsInMainForm);
            this.grpSettings.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSettings.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpSettings.GroupImage = null;
            this.grpSettings.GroupTitle = "设置";
            this.grpSettings.HeaderRoundCorners = 4;
            this.grpSettings.Location = new System.Drawing.Point(0, 0);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.PaintGroupBox = false;
            this.grpSettings.RoundCorners = 4;
            this.grpSettings.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpSettings.ShadowControl = false;
            this.grpSettings.ShadowThickness = 3;
            this.grpSettings.Size = new System.Drawing.Size(432, 420);
            this.grpSettings.TabIndex = 1;
            this.grpSettings.TextLineSpace = 2;
            this.grpSettings.TitleLeftSpace = 18;
            this.grpSettings.Load += new System.EventHandler(this.grpSettings_Load);
            // 
            // grpShortcutKey
            // 
            this.grpShortcutKey.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpShortcutKey.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpShortcutKey.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpShortcutKey.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpShortcutKey.BorderThickness = 1F;
            this.grpShortcutKey.BorderTopOnly = false;
            this.grpShortcutKey.Controls.Add(this.cmbModifier);
            this.grpShortcutKey.Controls.Add(this.cmbKey);
            this.grpShortcutKey.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpShortcutKey.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpShortcutKey.GroupImage = null;
            this.grpShortcutKey.GroupTitle = "热键";
            this.grpShortcutKey.HeaderRoundCorners = 4;
            this.grpShortcutKey.Location = new System.Drawing.Point(36, 262);
            this.grpShortcutKey.Name = "grpShortcutKey";
            this.grpShortcutKey.PaintGroupBox = false;
            this.grpShortcutKey.RoundCorners = 4;
            this.grpShortcutKey.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpShortcutKey.ShadowControl = false;
            this.grpShortcutKey.ShadowThickness = 3;
            this.grpShortcutKey.Size = new System.Drawing.Size(258, 60);
            this.grpShortcutKey.TabIndex = 9;
            this.grpShortcutKey.TextLineSpace = 2;
            this.grpShortcutKey.TitleLeftSpace = 18;
            // 
            // cmbModifier
            // 
            this.cmbModifier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModifier.FormattingEnabled = true;
            this.cmbModifier.Location = new System.Drawing.Point(3, 27);
            this.cmbModifier.Name = "cmbModifier";
            this.cmbModifier.Size = new System.Drawing.Size(100, 21);
            this.cmbModifier.TabIndex = 2;
            // 
            // cmbKey
            // 
            this.cmbKey.FormattingEnabled = true;
            this.cmbKey.Location = new System.Drawing.Point(126, 27);
            this.cmbKey.Name = "cmbKey";
            this.cmbKey.Size = new System.Drawing.Size(100, 21);
            this.cmbKey.TabIndex = 1;
            // 
            // chbEnabelControl
            // 
            this.chbEnabelControl.AutoSize = true;
            this.chbEnabelControl.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chbEnabelControl.Location = new System.Drawing.Point(249, 142);
            this.chbEnabelControl.Name = "chbEnabelControl";
            this.chbEnabelControl.Size = new System.Drawing.Size(74, 17);
            this.chbEnabelControl.Style = Skyray.Controls.Style.Office2007Blue;
            this.chbEnabelControl.TabIndex = 8;
            this.chbEnabelControl.Text = "是否灰色";
            this.chbEnabelControl.UseVisualStyleBackColor = true;
            // 
            // buttonSetting
            // 
            this.buttonSetting.Location = new System.Drawing.Point(130, 328);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(75, 25);
            this.buttonSetting.TabIndex = 7;
            this.buttonSetting.Text = "设置";
            this.buttonSetting.UseVisualStyleBackColor = true;
            this.buttonSetting.Click += new System.EventHandler(this.buttonSetting_Click);
            // 
            // textBoxW1
            // 
            this.textBoxW1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textBoxW1.Location = new System.Drawing.Point(162, 233);
            this.textBoxW1.Name = "textBoxW1";
            this.textBoxW1.Size = new System.Drawing.Size(100, 20);
            this.textBoxW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.textBoxW1.TabIndex = 6;
            this.textBoxW1.Visible = false;
            // 
            // lblItemText
            // 
            this.lblItemText.AutoSize = true;
            this.lblItemText.BackColor = System.Drawing.Color.Transparent;
            this.lblItemText.Location = new System.Drawing.Point(34, 236);
            this.lblItemText.Name = "lblItemText";
            this.lblItemText.Size = new System.Drawing.Size(91, 13);
            this.lblItemText.TabIndex = 5;
            this.lblItemText.Text = "显示项的文本：";
            this.lblItemText.Visible = false;
            // 
            // chkIsInMenu
            // 
            this.chkIsInMenu.AutoSize = true;
            this.chkIsInMenu.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkIsInMenu.Location = new System.Drawing.Point(36, 142);
            this.chkIsInMenu.Name = "chkIsInMenu";
            this.chkIsInMenu.Size = new System.Drawing.Size(74, 17);
            this.chkIsInMenu.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkIsInMenu.TabIndex = 4;
            this.chkIsInMenu.Text = "是否显示";
            this.chkIsInMenu.UseVisualStyleBackColor = true;
            // 
            // numericTextBox1
            // 
            this.numericTextBox1.Location = new System.Drawing.Point(162, 187);
            this.numericTextBox1.Name = "numericTextBox1";
            this.numericTextBox1.Size = new System.Drawing.Size(100, 20);
            this.numericTextBox1.TabIndex = 3;
            this.numericTextBox1.Visible = false;
            // 
            // lblItemType
            // 
            this.lblItemType.AutoSize = true;
            this.lblItemType.BackColor = System.Drawing.Color.Transparent;
            this.lblItemType.Location = new System.Drawing.Point(34, 191);
            this.lblItemType.Name = "lblItemType";
            this.lblItemType.Size = new System.Drawing.Size(91, 13);
            this.lblItemType.TabIndex = 2;
            this.lblItemType.Text = "显示项的类型：";
            this.lblItemType.Visible = false;
            // 
            // chkIsInTools
            // 
            this.chkIsInTools.AutoSize = true;
            this.chkIsInTools.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkIsInTools.Location = new System.Drawing.Point(36, 95);
            this.chkIsInTools.Name = "chkIsInTools";
            this.chkIsInTools.Size = new System.Drawing.Size(122, 17);
            this.chkIsInTools.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkIsInTools.TabIndex = 1;
            this.chkIsInTools.Text = "是否显示在工具栏";
            this.chkIsInTools.UseVisualStyleBackColor = true;
            // 
            // chkIsInMainForm
            // 
            this.chkIsInMainForm.AutoSize = true;
            this.chkIsInMainForm.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkIsInMainForm.Location = new System.Drawing.Point(36, 44);
            this.chkIsInMainForm.Name = "chkIsInMainForm";
            this.chkIsInMainForm.Size = new System.Drawing.Size(134, 17);
            this.chkIsInMainForm.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkIsInMainForm.TabIndex = 0;
            this.chkIsInMainForm.Text = "是否显示在主界面中";
            this.chkIsInMainForm.UseVisualStyleBackColor = true;
            // 
            // tbPageTools
            // 
            this.tbPageTools.Controls.Add(this.treeView2);
            this.tbPageTools.Location = new System.Drawing.Point(4, 26);
            this.tbPageTools.Name = "tbPageTools";
            this.tbPageTools.Padding = new System.Windows.Forms.Padding(3);
            this.tbPageTools.Size = new System.Drawing.Size(647, 426);
            this.tbPageTools.TabIndex = 1;
            this.tbPageTools.Text = "工具栏";
            this.tbPageTools.UseVisualStyleBackColor = true;
            // 
            // treeView2
            // 
            this.treeView2.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView2.Location = new System.Drawing.Point(3, 3);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(641, 420);
            this.treeView2.TabIndex = 0;
            this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
            // 
            // containerObject2
            // 
            this.containerObject2.AutoScroll = true;
            this.containerObject2.BigImage = null;
            this.containerObject2.ContainerAttribute = false;
            this.containerObject2.ContainerLabel = null;
            this.containerObject2.ControlInternal = 0;
            this.containerObject2.Controls.Add(this.txtSaveFile);
            this.containerObject2.Controls.Add(this.lbWRolePath);
            this.containerObject2.Controls.Add(this.comboBox1);
            this.containerObject2.Controls.Add(this.lbWRoleCategory);
            this.containerObject2.CurrentPanelIndex = 0;
            this.containerObject2.CurrentPlanningNumber = 0;
            this.containerObject2.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerObject2.IncludeInnerCoordinate = false;
            this.containerObject2.IsReverseEmbeded = false;
            this.containerObject2.Location = new System.Drawing.Point(8, 9);
            this.containerObject2.Name = "containerObject2";
            this.containerObject2.Name1 = null;
            office2007Renderer2.RoundedEdges = true;
            this.containerObject2.Renderer = office2007Renderer2;
            this.containerObject2.Size = new System.Drawing.Size(665, 67);
            this.containerObject2.SmallImage = null;
            this.containerObject2.Style = Skyray.Controls.Style.Custom;
            this.containerObject2.TabIndex = 6;
            // 
            // txtSaveFile
            // 
            this.txtSaveFile.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtSaveFile.Location = new System.Drawing.Point(383, 29);
            this.txtSaveFile.Name = "txtSaveFile";
            this.txtSaveFile.Size = new System.Drawing.Size(178, 20);
            this.txtSaveFile.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtSaveFile.TabIndex = 6;
            this.txtSaveFile.Visible = false;
            // 
            // lbWRolePath
            // 
            this.lbWRolePath.AutoSize = true;
            this.lbWRolePath.BackColor = System.Drawing.Color.Transparent;
            this.lbWRolePath.Location = new System.Drawing.Point(292, 33);
            this.lbWRolePath.Name = "lbWRolePath";
            this.lbWRolePath.Size = new System.Drawing.Size(91, 13);
            this.lbWRolePath.TabIndex = 5;
            this.lbWRolePath.Text = "权限保存文件：";
            this.lbWRolePath.Visible = false;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(92, 24);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(129, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lbWRoleCategory
            // 
            this.lbWRoleCategory.AutoSize = true;
            this.lbWRoleCategory.BackColor = System.Drawing.Color.Transparent;
            this.lbWRoleCategory.Location = new System.Drawing.Point(11, 29);
            this.lbWRoleCategory.Name = "lbWRoleCategory";
            this.lbWRoleCategory.Size = new System.Drawing.Size(67, 13);
            this.lbWRoleCategory.TabIndex = 0;
            this.lbWRoleCategory.Text = "权限类别：";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "项名称";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // ToolsNaviForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.tbPage1);
            this.Controls.Add(this.containerObject2);
            this.Controls.Add(this.buttonWCancel);
            this.Controls.Add(this.buttonWSubmit);
            this.Name = "ToolsNaviForm";
            this.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Size = new System.Drawing.Size(681, 570);
            this.Load += new System.EventHandler(this.ToolsNaviForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tbPage1.ResumeLayout(false);
            this.tpPageMainSetting.ResumeLayout(false);
            this.containerObject1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpShortcutKey.ResumeLayout(false);
            this.tbPageTools.ResumeLayout(false);
            this.containerObject2.ResumeLayout(false);
            this.containerObject2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.Grouper grpSettings;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Skyray.Controls.CheckBoxW chkIsInMainForm;
        private Skyray.Controls.CheckBoxW chkIsInTools;
        private Skyray.Controls.NumericTextBox numericTextBox1;
        private Skyray.Controls.LabelW lblItemType;
        private Skyray.Controls.CheckBoxW chkIsInMenu;
        private Skyray.Controls.TextBoxW textBoxW1;
        private Skyray.Controls.LabelW lblItemText;
        private Skyray.Controls.ButtonW buttonWSubmit;
        private Skyray.Controls.ButtonW buttonWCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.Button buttonSetting;
        private Skyray.Controls.CheckBoxW chbEnabelControl;
        private Skyray.EDX.Common.ContainerObject containerObject1;
        private Skyray.EDX.Common.ContainerObject containerObject2;
        private Skyray.Controls.LabelW lbWRoleCategory;
        private System.Windows.Forms.ComboBox comboBox1;
        private Skyray.Controls.TextBoxW txtSaveFile;
        private Skyray.Controls.LabelW lbWRolePath;
        private System.Windows.Forms.TreeView treeView1;
        private Skyray.Controls.Grouper grpShortcutKey;
        private System.Windows.Forms.ComboBox cmbKey;
        private System.Windows.Forms.ComboBox cmbModifier;
        private Skyray.Controls.ButtonW btnApplication;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem TlsMenuItemUp;
        private System.Windows.Forms.ToolStripMenuItem TlsMenuItemDown;
        private Skyray.Controls.TabControlW tbPage1;
        private System.Windows.Forms.TabPage tpPageMainSetting;
        private System.Windows.Forms.TabPage tbPageTools;
        private System.Windows.Forms.TreeView treeView2;
    }
}