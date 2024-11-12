using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;
using System.Data;

namespace Skyray.Print
{
    public class RecurseTreeObj : Skyray.Language.UCMultiple
    {
        private TreeView treeView1;
        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem 填充位置ToolStripMenuItem;
        private ToolStripMenuItem 附加信息ToolStripMenuItem;
        private ToolStripMenuItem 是否编辑ToolStripMenuItem;
        private ToolStripMenuItem 复制对象ToolStripMenuItem;
        private ToolStripMenuItem 移除节点ToolStripMenuItem;
        public event EventHandler SelectDataEnd;

        private Type _obj;

        public Type Obj
        {
            get { return _obj; }
            set { 
                _obj = value;
            }
        }

        public RecurseTreeObj()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.填充位置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.附加信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制对象ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移除节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.是否编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(530, 467);
            this.treeView1.TabIndex = 0;
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.填充位置ToolStripMenuItem,
            this.附加信息ToolStripMenuItem,
            this.复制对象ToolStripMenuItem,
            this.移除节点ToolStripMenuItem,
            this.是否编辑ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 114);
            // 
            // 填充位置ToolStripMenuItem
            // 
            this.填充位置ToolStripMenuItem.Name = "填充位置ToolStripMenuItem";
            this.填充位置ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.填充位置ToolStripMenuItem.Text = "填充位置";
            this.填充位置ToolStripMenuItem.Click += new System.EventHandler(this.填充位置ToolStripMenuItem_Click);
            // 
            // 附加信息ToolStripMenuItem
            // 
            this.附加信息ToolStripMenuItem.Name = "附加信息ToolStripMenuItem";
            this.附加信息ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.附加信息ToolStripMenuItem.Text = "附加信息";
            // 
            // 复制对象ToolStripMenuItem
            // 
            this.复制对象ToolStripMenuItem.Name = "复制对象ToolStripMenuItem";
            this.复制对象ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.复制对象ToolStripMenuItem.Text = "复制对象";
            this.复制对象ToolStripMenuItem.Click += new System.EventHandler(this.复制对象ToolStripMenuItem_Click);
            // 
            // 移除节点ToolStripMenuItem
            // 
            this.移除节点ToolStripMenuItem.Name = "移除节点ToolStripMenuItem";
            this.移除节点ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.移除节点ToolStripMenuItem.Text = "移除节点";
            this.移除节点ToolStripMenuItem.Click += new System.EventHandler(this.移除节点ToolStripMenuItem_Click);
            // 
            // 是否编辑ToolStripMenuItem
            // 
            this.是否编辑ToolStripMenuItem.Name = "是否编辑ToolStripMenuItem";
            this.是否编辑ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.是否编辑ToolStripMenuItem.Text = "是否编辑";
            // 
            // RecurseTreeObj
            // 
            this.Controls.Add(this.treeView1);
            this.Name = "RecurseTreeObj";
            this.Size = new System.Drawing.Size(530, 467);
            this.Load += new System.EventHandler(this.RecurseTreeObj_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void RecurseTreeObj_Load(object sender, EventArgs e)
        {
            
            TreeNode node = new TreeNode("Class");
            this.treeView1.Nodes.Add(node);
            RecurseClassojb(_obj,node);
        }

        private void RecurseClassojb(Type type,TreeNode node)
        {
            PropertyInfo[] properties = type.GetProperties();
            if (properties == null || properties.Length == 0)
                return;
            foreach (PropertyInfo info in properties)
            {
                TreeNode childNode = new TreeNode(info.Name);
                node.Nodes.Add(childNode);
                if (info.PropertyType == typeof(DataTable))
                {
                    childNode.Tag = true;
                }
                else 
                    childNode.Tag = false;
            }
        }

        private void 复制对象ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeView1.SelectedNode;
            if (node.Parent != null)
            {
                TreeNode tempNode = (TreeNode)node.Clone();
                node.Parent.Nodes.Insert(node.Index + 1, tempNode);
            }
        }

        private void 移除节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
             TreeNode node = this.treeView1.SelectedNode;
             if (node.Parent != null)
             {
                 node.Parent.Nodes.Remove(node);
             }
        }

        private void 填充位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (SelectDataEnd != null)
            {
                SelectDataEnd(this.treeView1.SelectedNode, null);
            }
            var form = this.ParentForm;
            if (form != null) form.Close();
        }
      
    }
}
