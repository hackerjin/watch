using System.Drawing;
namespace Skyray.UC
{
    partial class FrmElementTable
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbxSampleConstruct = new System.Windows.Forms.GroupBox();
            this.lblSampleStructLayer5 = new System.Windows.Forms.Label();
            this.lblSampleStructLayer4 = new System.Windows.Forms.Label();
            this.lblSampleStructLayer3 = new System.Windows.Forms.Label();
            this.lblSampleStructLayer2 = new System.Windows.Forms.Label();
            this.lblSampleStructLayer1 = new System.Windows.Forms.Label();
            this.lblSampleStructLayerBase = new System.Windows.Forms.Label();
            this.dgvSampleConstruct = new Skyray.Controls.DataGridViewW();
            this.ctxtMenuSampleConstruct = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSampleConstructInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSampleConstructDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnDelLayer = new Skyray.Controls.ButtonW();
            this.btnAddLayer = new Skyray.Controls.ButtonW();
            this.tabCompounds = new System.Windows.Forms.TabPage();
            this.txtAdd = new Skyray.Controls.TextBoxW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.clbCompounds = new System.Windows.Forms.CheckedListBox();
            this.tabElement = new System.Windows.Forms.TabPage();
            this.elementTable = new Skyray.Controls.ElementTable.ElementTable();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabEleList = new Skyray.Controls.TabControlW();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gbxSampleConstruct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSampleConstruct)).BeginInit();
            this.ctxtMenuSampleConstruct.SuspendLayout();
            this.tabCompounds.SuspendLayout();
            this.tabElement.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabEleList.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxSampleConstruct
            // 
            this.gbxSampleConstruct.BackColor = System.Drawing.Color.Transparent;
            this.gbxSampleConstruct.Controls.Add(this.lblSampleStructLayer5);
            this.gbxSampleConstruct.Controls.Add(this.lblSampleStructLayer4);
            this.gbxSampleConstruct.Controls.Add(this.lblSampleStructLayer3);
            this.gbxSampleConstruct.Controls.Add(this.lblSampleStructLayer2);
            this.gbxSampleConstruct.Controls.Add(this.lblSampleStructLayer1);
            this.gbxSampleConstruct.Controls.Add(this.lblSampleStructLayerBase);
            this.gbxSampleConstruct.Controls.Add(this.dgvSampleConstruct);
            this.gbxSampleConstruct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxSampleConstruct.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbxSampleConstruct.ForeColor = System.Drawing.Color.Red;
            this.gbxSampleConstruct.Location = new System.Drawing.Point(10, 11);
            this.gbxSampleConstruct.Name = "gbxSampleConstruct";
            this.gbxSampleConstruct.Size = new System.Drawing.Size(811, 168);
            this.gbxSampleConstruct.TabIndex = 7;
            this.gbxSampleConstruct.TabStop = false;
            this.gbxSampleConstruct.Text = "样品结构(第一层为最外层)";
            // 
            // lblSampleStructLayer5
            // 
            this.lblSampleStructLayer5.AutoSize = true;
            this.lblSampleStructLayer5.ForeColor = System.Drawing.Color.DarkCyan;
            this.lblSampleStructLayer5.Location = new System.Drawing.Point(23, 145);
            this.lblSampleStructLayer5.Name = "lblSampleStructLayer5";
            this.lblSampleStructLayer5.Size = new System.Drawing.Size(53, 12);
            this.lblSampleStructLayer5.TabIndex = 9;
            this.lblSampleStructLayer5.Text = "第 五 层";
            // 
            // lblSampleStructLayer4
            // 
            this.lblSampleStructLayer4.AutoSize = true;
            this.lblSampleStructLayer4.ForeColor = System.Drawing.Color.DarkCyan;
            this.lblSampleStructLayer4.Location = new System.Drawing.Point(23, 122);
            this.lblSampleStructLayer4.Name = "lblSampleStructLayer4";
            this.lblSampleStructLayer4.Size = new System.Drawing.Size(53, 12);
            this.lblSampleStructLayer4.TabIndex = 9;
            this.lblSampleStructLayer4.Text = "第 四 层";
            // 
            // lblSampleStructLayer3
            // 
            this.lblSampleStructLayer3.AutoSize = true;
            this.lblSampleStructLayer3.ForeColor = System.Drawing.Color.DarkCyan;
            this.lblSampleStructLayer3.Location = new System.Drawing.Point(23, 99);
            this.lblSampleStructLayer3.Name = "lblSampleStructLayer3";
            this.lblSampleStructLayer3.Size = new System.Drawing.Size(53, 12);
            this.lblSampleStructLayer3.TabIndex = 9;
            this.lblSampleStructLayer3.Text = "第 三 层";
            // 
            // lblSampleStructLayer2
            // 
            this.lblSampleStructLayer2.AutoSize = true;
            this.lblSampleStructLayer2.ForeColor = System.Drawing.Color.DarkCyan;
            this.lblSampleStructLayer2.Location = new System.Drawing.Point(23, 75);
            this.lblSampleStructLayer2.Name = "lblSampleStructLayer2";
            this.lblSampleStructLayer2.Size = new System.Drawing.Size(53, 12);
            this.lblSampleStructLayer2.TabIndex = 9;
            this.lblSampleStructLayer2.Text = "第 二 层";
            // 
            // lblSampleStructLayer1
            // 
            this.lblSampleStructLayer1.AutoSize = true;
            this.lblSampleStructLayer1.ForeColor = System.Drawing.Color.DarkCyan;
            this.lblSampleStructLayer1.Location = new System.Drawing.Point(23, 51);
            this.lblSampleStructLayer1.Name = "lblSampleStructLayer1";
            this.lblSampleStructLayer1.Size = new System.Drawing.Size(53, 12);
            this.lblSampleStructLayer1.TabIndex = 9;
            this.lblSampleStructLayer1.Text = "第 一 层";
            // 
            // lblSampleStructLayerBase
            // 
            this.lblSampleStructLayerBase.AutoSize = true;
            this.lblSampleStructLayerBase.ForeColor = System.Drawing.Color.Maroon;
            this.lblSampleStructLayerBase.Location = new System.Drawing.Point(23, 27);
            this.lblSampleStructLayerBase.Name = "lblSampleStructLayerBase";
            this.lblSampleStructLayerBase.Size = new System.Drawing.Size(53, 12);
            this.lblSampleStructLayerBase.TabIndex = 9;
            this.lblSampleStructLayerBase.Text = "基    材";
            // 
            // dgvSampleConstruct
            // 
            this.dgvSampleConstruct.AllowUserToAddRows = false;
            this.dgvSampleConstruct.AllowUserToDeleteRows = false;
            this.dgvSampleConstruct.AllowUserToResizeColumns = false;
            this.dgvSampleConstruct.AllowUserToResizeRows = false;
            this.dgvSampleConstruct.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSampleConstruct.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvSampleConstruct.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvSampleConstruct.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSampleConstruct.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSampleConstruct.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvSampleConstruct.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSampleConstruct.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSampleConstruct.ColumnHeadersHeight = 4;
            this.dgvSampleConstruct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSampleConstruct.ColumnHeadersVisible = false;
            this.dgvSampleConstruct.ContextMenuStrip = this.ctxtMenuSampleConstruct;
            this.dgvSampleConstruct.Location = new System.Drawing.Point(82, 27);
            this.dgvSampleConstruct.MultiSelect = false;
            this.dgvSampleConstruct.Name = "dgvSampleConstruct";
            this.dgvSampleConstruct.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvSampleConstruct.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvSampleConstruct.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvSampleConstruct.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSampleConstruct.RowHeadersVisible = false;
            this.dgvSampleConstruct.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Cyan;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSampleConstruct.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSampleConstruct.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvSampleConstruct.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvSampleConstruct.RowTemplate.Height = 23;
            this.dgvSampleConstruct.RowTemplate.ReadOnly = true;
            this.dgvSampleConstruct.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSampleConstruct.SecondaryLength = 1;
            this.dgvSampleConstruct.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvSampleConstruct.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvSampleConstruct.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvSampleConstruct.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvSampleConstruct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSampleConstruct.ShowEportContextMenu = true;
            this.dgvSampleConstruct.Size = new System.Drawing.Size(720, 133);
            this.dgvSampleConstruct.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvSampleConstruct.TabIndex = 6;
            this.dgvSampleConstruct.ToPrintCols = null;
            this.dgvSampleConstruct.ToPrintRows = null;
            this.dgvSampleConstruct.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSampleConstruct_CellMouseDown);
            this.dgvSampleConstruct.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvSampleConstruct_RowsAdded);
            this.dgvSampleConstruct.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvSampleConstruct_RowsRemoved);
            // 
            // ctxtMenuSampleConstruct
            // 
            this.ctxtMenuSampleConstruct.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ctxtMenuSampleConstruct.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSampleConstructInsert,
            this.tsmiSampleConstructDelete});
            this.ctxtMenuSampleConstruct.Name = "contextMenuSampleConstruct";
            this.ctxtMenuSampleConstruct.Size = new System.Drawing.Size(95, 48);
            // 
            // tsmiSampleConstructInsert
            // 
            this.tsmiSampleConstructInsert.Name = "tsmiSampleConstructInsert";
            this.tsmiSampleConstructInsert.Size = new System.Drawing.Size(94, 22);
            this.tsmiSampleConstructInsert.Text = "增加";
            this.tsmiSampleConstructInsert.Click += new System.EventHandler(this.tsmiSampleConstructInsert_Click);
            // 
            // tsmiSampleConstructDelete
            // 
            this.tsmiSampleConstructDelete.Name = "tsmiSampleConstructDelete";
            this.tsmiSampleConstructDelete.Size = new System.Drawing.Size(94, 22);
            this.tsmiSampleConstructDelete.Text = "删除";
            this.tsmiSampleConstructDelete.Click += new System.EventHandler(this.tsmiSampleConstructDelete_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(719, 13);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(101, 25);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnDelLayer
            // 
            this.btnDelLayer.bSilver = false;
            this.btnDelLayer.Location = new System.Drawing.Point(586, 13);
            this.btnDelLayer.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDelLayer.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDelLayer.Name = "btnDelLayer";
            this.btnDelLayer.Size = new System.Drawing.Size(101, 25);
            this.btnDelLayer.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDelLayer.TabIndex = 8;
            this.btnDelLayer.Text = "删除层";
            this.btnDelLayer.ToFocused = false;
            this.btnDelLayer.UseVisualStyleBackColor = true;
            this.btnDelLayer.Visible = false;
            this.btnDelLayer.Click += new System.EventHandler(this.tsmiSampleConstructDelete_Click);
            // 
            // btnAddLayer
            // 
            this.btnAddLayer.bSilver = false;
            this.btnAddLayer.Location = new System.Drawing.Point(453, 13);
            this.btnAddLayer.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAddLayer.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAddLayer.Name = "btnAddLayer";
            this.btnAddLayer.Size = new System.Drawing.Size(101, 25);
            this.btnAddLayer.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAddLayer.TabIndex = 9;
            this.btnAddLayer.Text = "添加层";
            this.btnAddLayer.ToFocused = false;
            this.btnAddLayer.UseVisualStyleBackColor = true;
            this.btnAddLayer.Visible = false;
            this.btnAddLayer.Click += new System.EventHandler(this.tsmiSampleConstructInsert_Click);
            // 
            // tabCompounds
            // 
            this.tabCompounds.Controls.Add(this.txtAdd);
            this.tabCompounds.Controls.Add(this.btnDel);
            this.tabCompounds.Controls.Add(this.btnAdd);
            this.tabCompounds.Controls.Add(this.clbCompounds);
            this.tabCompounds.Location = new System.Drawing.Point(4, 26);
            this.tabCompounds.Name = "tabCompounds";
            this.tabCompounds.Padding = new System.Windows.Forms.Padding(3);
            this.tabCompounds.Size = new System.Drawing.Size(803, 465);
            this.tabCompounds.TabIndex = 1;
            this.tabCompounds.Text = "自定义";
            // 
            // txtAdd
            // 
            this.txtAdd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtAdd.Location = new System.Drawing.Point(5, 350);
            this.txtAdd.MaxLength = 20;
            this.txtAdd.Name = "txtAdd";
            this.txtAdd.Size = new System.Drawing.Size(163, 20);
            this.txtAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtAdd.TabIndex = 5;
            this.txtAdd.Visible = false;
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(275, 348);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(82, 25);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 4;
            this.btnDel.Text = "删除";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Visible = false;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(174, 348);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(82, 25);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "添加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // clbCompounds
            // 
            this.clbCompounds.BackColor = System.Drawing.Color.GhostWhite;
            this.clbCompounds.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbCompounds.CheckOnClick = true;
            this.clbCompounds.FormattingEnabled = true;
            this.clbCompounds.Location = new System.Drawing.Point(4, 5);
            this.clbCompounds.Name = "clbCompounds";
            this.clbCompounds.Size = new System.Drawing.Size(797, 315);
            this.clbCompounds.TabIndex = 0;
            this.clbCompounds.MouseUp += new System.Windows.Forms.MouseEventHandler(this.clbCompounds_MouseUp);
            // 
            // tabElement
            // 
            this.tabElement.Controls.Add(this.elementTable);
            this.tabElement.Location = new System.Drawing.Point(4, 26);
            this.tabElement.Name = "tabElement";
            this.tabElement.Size = new System.Drawing.Size(803, 465);
            this.tabElement.TabIndex = 0;
            this.tabElement.Text = "元素";
            this.tabElement.UseVisualStyleBackColor = true;
            // 
            // elementTable
            // 
            this.elementTable.BackColor = System.Drawing.Color.GhostWhite;
            this.elementTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.elementTable.GridHeight = 50;
            this.elementTable.GridWidth = 44;
            this.elementTable.Location = new System.Drawing.Point(0, 0);
            this.elementTable.MaxLayElement = 35;
            this.elementTable.MultiSelect = true;
            this.elementTable.Name = "elementTable";
            this.elementTable.SelectColor = System.Drawing.Color.Orange;
            this.elementTable.ShowKL = false;
            this.elementTable.SignelLine = false;
            this.elementTable.Size = new System.Drawing.Size(803, 465);
            this.elementTable.TabIndex = 1;
            this.elementTable.UnselectColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.GhostWhite;
            this.panel1.Controls.Add(this.tabEleList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.panel1.Size = new System.Drawing.Size(831, 495);
            this.panel1.TabIndex = 2;
            // 
            // tabEleList
            // 
            this.tabEleList.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(79)))), ((int)(((byte)(125)))));
            this.tabEleList.BackColor = System.Drawing.Color.GhostWhite;
            this.tabEleList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.tabEleList.Controls.Add(this.tabElement);
            this.tabEleList.Controls.Add(this.tabCompounds);
            this.tabEleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabEleList.Location = new System.Drawing.Point(10, 0);
            this.tabEleList.Name = "tabEleList";
            this.tabEleList.Padding = new System.Drawing.Point(3, 3);
            this.tabEleList.SelectedIndex = 0;
            this.tabEleList.ShowTabs = true;
            this.tabEleList.Size = new System.Drawing.Size(811, 495);
            this.tabEleList.Style = Skyray.Controls.Style.Office2007Blue;
            this.tabEleList.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gbxSampleConstruct);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 495);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10, 11, 10, 0);
            this.panel2.Size = new System.Drawing.Size(831, 179);
            this.panel2.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnDelLayer);
            this.panel3.Controls.Add(this.btnOK);
            this.panel3.Controls.Add(this.btnAddLayer);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 674);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(831, 51);
            this.panel3.TabIndex = 11;
            // 
            // FrmElementTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FrmElementTable";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.Size = new System.Drawing.Size(831, 750);
            this.Load += new System.EventHandler(this.FrmElementTable_Load);
            this.gbxSampleConstruct.ResumeLayout(false);
            this.gbxSampleConstruct.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSampleConstruct)).EndInit();
            this.ctxtMenuSampleConstruct.ResumeLayout(false);
            this.tabCompounds.ResumeLayout(false);
            this.tabCompounds.PerformLayout();
            this.tabElement.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabEleList.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW btnOK;
        private System.Windows.Forms.GroupBox gbxSampleConstruct;
        private System.Windows.Forms.Label lblSampleStructLayer5;
        private System.Windows.Forms.Label lblSampleStructLayer4;
        private System.Windows.Forms.Label lblSampleStructLayer3;
        private System.Windows.Forms.Label lblSampleStructLayer2;
        private System.Windows.Forms.Label lblSampleStructLayer1;
        private System.Windows.Forms.Label lblSampleStructLayerBase;
        private Skyray.Controls.DataGridViewW dgvSampleConstruct;
        private System.Windows.Forms.ContextMenuStrip ctxtMenuSampleConstruct;
        private System.Windows.Forms.ToolStripMenuItem tsmiSampleConstructInsert;
        private System.Windows.Forms.ToolStripMenuItem tsmiSampleConstructDelete;
        private Skyray.Controls.ButtonW btnDelLayer;
        private Skyray.Controls.ButtonW btnAddLayer;
        private System.Windows.Forms.TabPage tabCompounds;
        private Skyray.Controls.TextBoxW txtAdd;
        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.ButtonW btnAdd;
        private System.Windows.Forms.CheckedListBox clbCompounds;
        private System.Windows.Forms.TabPage tabElement;
        private System.Windows.Forms.Panel panel1;
        private Skyray.Controls.ElementTable.ElementTable elementTable;
        private Skyray.Controls.TabControlW tabEleList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;


    }
}