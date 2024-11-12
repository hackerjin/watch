using System;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;
using Skyray.Controls.Tree.NodeControls;
using System.Linq;
using System.ComponentModel;
using Skyray.Controls.Tree;
using Skyray.Controls;

namespace Skyray.Print
{
    public class TreeViewAdvEx : TreeViewAdv
    {
        private NodeTextBox NodeTextBox;

        private TreeModel _Model;

        /// <summary>
        /// 是否启用标签重组
        /// </summary>
        [DefaultValue(true)]
        public bool SelfLevelEdit { get; set; }

        /// <summary>
        /// 是否启用自动拖拽事件
        /// </summary>
        [DefaultValue(true)]
        public bool AutoItemDrag { get; set; }

        [DefaultValue(true)]
        public bool EnableEditMenu { get; set; }


        [DefaultValue(true)]
        public bool ShowConfirmDelDlg { get; set; }

        [DefaultValue(true)]
        public bool ExpandOnAddNode { get; set; }

        [DefaultValue(true)]
        public bool AllowDeleteNode { get; set; }

        [DefaultValue(true)]
        public bool AllowEditTag { get; set; }

        [DefaultValue(typeof(Image), "null")]
        public Image AddImage { get; set; }

        [DefaultValue(typeof(Image), "null")]
        public Image DelImage { get; set; }

        [DefaultValue(typeof(Image), "null")]
        public Image RootNodeImage { get; set; }

        [DefaultValue(typeof(Image), "null")]
        public Image EditTagImage { get; set; }

        [DefaultValue(typeof(ContextMenuStrip), "null")]
        public ContextMenuStrip ConTextMenu { get; set; }

        private ToolStripItem addItem;
        private ToolStripItem delItem;
        private ToolStripItem addRootItem;
        private ToolStripItem EditTagItem;

        [Browsable(false), DefaultValue(typeof(List<TreeNodeInfo>), "null")]
        private List<TreeNodeInfo> _ReportDataSource;
        public List<TreeNodeInfo> ReportDataSource
        {
            get { return _ReportDataSource; }
            set
            {
                _ReportDataSource = value;
                this.Model = _Model=null;
                this.BeginUpdate();
                if (_ReportDataSource != null)
                { 
                    _Model = new TreeModel();

                    Node nodeLabel = new Node(PrintInfo.Label);
                    Node nodeField = new Node(PrintInfo.Field);
                    Node nodeImage = new Node(PrintInfo.Image);
                    Node nodeGrid = new Node(PrintInfo.Grid);
                    //修改：何晓明 2011-03-17
                    //原因：暂不使用复合表格
                    Node nodeCompositeGrid = new Node(PrintInfo.CompositeTable);
                    //
                    Node node = null;
                    Node nodeParent = null;
                    Node nodeGroup = null;
                    Dictionary<string, Node> dic = new Dictionary<string, Node>();

                    _ReportDataSource.ForEach(info =>
                    {
                        node = new Node().SetParam(info);
                        switch (info.Type)
                        {
                            case CtrlType.Label:
                                nodeParent = nodeLabel;
                                break;
                            case CtrlType.Field:
                                nodeParent = nodeField;
                                break;
                            case CtrlType.Image:
                                nodeParent = nodeImage;
                                break;
                            case CtrlType.Grid:
                                nodeParent = nodeGrid;
                                break;
                            //修改：何晓明 2011-03-17
                            //原因：暂不使用复合表格
                            case CtrlType.ComposeTable:
                                nodeParent = nodeCompositeGrid;
                                break;
                            //
                        }
                        bool IsGroup = !string.IsNullOrEmpty(info.GroupName);
                        if (IsGroup)
                        {
                            string key = info.Type.ToString() + info.GroupName;
                            if (dic.ContainsKey(key))
                            {
                                nodeParent = dic[key];
                            }
                            else
                            {
                                TreeNodeInfo infoNew = new TreeNodeInfo();
                                //nodeGroup = new Node(info.GroupName);
                                infoNew.GroupName = info.GroupName;
                                infoNew.Parent = true;
                                infoNew.Type = info.Type;
                                infoNew.Caption = info.GroupName;
                                //nodeGroup = new Node(info.GroupName);
                                nodeGroup = new Node().SetParam(infoNew);
                                dic.Add(key, nodeGroup);
                                nodeParent.Nodes.Add(nodeGroup);
                                nodeParent = nodeGroup;//欲添加的节点为组节点
                            }
                        }
                        nodeParent.Nodes.Add(node);//添加节点                      

                    });

                    _Model.Nodes.Add(nodeLabel);
                    _Model.Nodes.Add(nodeField);
                    _Model.Nodes.Add(nodeImage);
                    _Model.Nodes.Add(nodeGrid);                    
                    //修改：何晓明 2011-03-17
                    //原因：暂不使用复合表格
                    _Model.Nodes.Add(nodeCompositeGrid);
                    //
                }
                //if (Skyray.Language.Lang.Model.CheckToSaveText() && Skyray.Language.Lang.Model.TreeData.Count>0)
                //{
                //    Skyray.Language.Lang.Model.SaveTreeViewData();
                //}
                //TreeNode nodes = null;
                this.Model = _Model;
                this.EndUpdate();
                GC.Collect(3);
            }
        }
        public TreeViewAdvEx()
        {
            SelfLevelEdit = false;
            //AutoItemDrag = false;
            EnableEditMenu = true;
            ShowConfirmDelDlg = true;
            ExpandOnAddNode = true;
            AllowDeleteNode = true;
            AllowEditTag = true;
            AllowDrop = true;
            RowHeight = 18;
            DisplayDraggingNodes = true;
            HighlightDropPosition = false;
            BorderStyle = BorderStyle.None;

            NodeTextBox = new NodeTextBox();
            NodeTextBox.DataPropertyName = "Text";
            //NodeTextBox.EditEnabled = true;
            //NodeTextBox.EditOnClick = true;            
            //NodeTextBox.IncrementalSearchEnabled = true;
            this.NodeControls.Add(NodeTextBox);

            ConTextMenu = new ContextMenuStrip();
            addItem = ConTextMenu.Items.Add(PrintInfo.Add, AddImage, AddNode);
            addItem.Click += new EventHandler(addItem_Click);
            delItem = ConTextMenu.Items.Add(PrintInfo.Delete, DelImage, DelNode);
            addRootItem = ConTextMenu.Items.Add(PrintInfo.AddRoot, RootNodeImage, AddRoot);
            EditTagItem = ConTextMenu.Items.Add(PrintInfo.EditTag, EditTagImage, EditNodeTag);
            addRootItem.Visible = false;
            EditTagItem.Visible = AllowEditTag;

            this.DragOver += new DragEventHandler(_tree_DragOver);
            this.DragDrop += new DragEventHandler(_tree_DragDrop);
            this.ItemDrag += new ItemDragEventHandler(_tree_ItemDrag);
            this.MouseClick += new MouseEventHandler(TreeViewAdvEx_MouseClick);
        }

        private void TreeViewAdvEx_MouseClick(object sender, MouseEventArgs e)
        {
            bool bShowMenu = e.Button == MouseButtons.Right && EnableEditMenu;//判断是否显示右键菜单
            if (bShowMenu)
            {
                //addRootItem.Visible = SelectedNode == null ? true : false;//判断是否显示添加根节点菜单
                delItem.Visible = AllowDeleteNode && SelectedNode != null ? true : false;//是否显示删除按钮
                addItem.Visible = SelectedNode == null ? false : true;//判断是否显示添加按钮                
                //EditTagItem.Visible = SelectedNode == null ? false : true;//编辑节点属性
                EditTagItem.Visible = false;
                ConTextMenu.Show(this.PointToScreen(e.Location));
            }
        }
        FrmTreeNodeInfo frm ;
        void addItem_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();             
            frm = new FrmTreeNodeInfo();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                _ReportDataSource.Add( frm.add);
                ReportDataSource = _ReportDataSource;
                this.ExpandAll();
            }
        }

        private void EditNodeTag(object sender, EventArgs e)
        {
            if (SelectedNode == null) return;
            var node = SelectedNode.Tag as Node;
            //new FrmNodeInfo(ref node).ShowDialog();
        }

        private void AddNode(object sender, EventArgs e)
        {
            if (SelectedNode == null) return;
            Node nodeNew = new Node().SetParam(string.Empty);
            //if (new FrmNodeInfo(ref nodeNew).ShowDialog() == DialogResult.OK)
            //{
            //    (SelectedNode.Tag as Node).Nodes.Add(nodeNew);
            //    if (ExpandOnAddNode) SelectedNode.IsExpanded = true;
            //}
        }

        private void AddRoot(object sender, EventArgs e)
        {
            Node nodeNew = new Node().SetParam(string.Empty);
            //if (new FrmNodeInfo(ref nodeNew).ShowDialog() == DialogResult.OK)
            //{
            //    if (this.Model == null) this.Model = new TreeModel();
            //    ((TreeModel)this.Model).Nodes.Add(nodeNew);
            //}
        }

        private void DelNode(object sender, EventArgs e)
        {
            if (ShowConfirmDelDlg && SkyrayMsgBox.Show(PrintInfo.ConfirmDel, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;//删除确认
            var nodes = new TreeNodeAdv[SelectedNodes.Count];
            SelectedNodes.CopyTo(nodes, 0);

            this.BeginUpdate();
            foreach (var node in nodes)
            {
                if (node != null) (node.Tag as Node).Parent = null;//删除节点   
                TreeNodeInfo treeNodeInfo =(node.Tag as Node).Tag as TreeNodeInfo ;
                frm = new FrmTreeNodeInfo();
                frm.DeleteTreeNodeInfo(treeNodeInfo);  
                var uc = this.TopLevelControl.Controls[0] as UCPrint;
                uc.DataSource.Remove(treeNodeInfo);
                uc.InitTree();
            }
            this.EndUpdate();
            
        }

        //private List<Node> GetAllNodes()
        //{
        //    List<Node> lst = new List<Node>();
        //    var model = this.Model as TreeModel;
        //    if (model != null)
        //    {
        //        int Row = 0;
        //        TreeNodeInfo info;
        //        foreach (Node node in model.Nodes)
        //        {
        //            info = node.Tag as TreeNodeInfo;
        //            info.Row = Row;//所在行
        //            info.RowParent = -1;//父节点所在行                    
        //            lst.Add(node);
        //            GetAllChildNodes(node, lst, ref Row);

        //            Row++;
        //        }
        //    }
        //    return lst;
        //}

        //private void GetAllChildNodes(Node node, List<Node> lst, ref int Row)
        //{
        //    TreeNodeInfo info;
        //    foreach (Node no in node.Nodes)
        //    {
        //        info = no.Tag as TreeNodeInfo;
        //        Row++;
        //        info.Row = Row;
        //        info.RowParent = (node.Tag as TreeNodeInfo).Row;
        //        lst.Add(no);
        //        GetAllChildNodes(no, lst, ref Row);
        //    }
        //}

        //private List<TreeNodeInfo> GetDataSource()
        //{
        //    List<TreeNodeInfo> lstInfo = new List<TreeNodeInfo>();
        //    GetAllNodes().ForEach(node => lstInfo.Add(node.Tag as TreeNodeInfo));
        //    lstInfo.TrimExcess();
        //    return lstInfo.Count == 0 ? null : lstInfo;
        //}

        private void _tree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (AutoItemDrag) this.DoDragDropSelectedNodes(DragDropEffects.Move);
        }

        private void _tree_DragOver(object sender, DragEventArgs e)
        {
            if (!SelfLevelEdit) return;

            if (e.Data.GetDataPresent(typeof(TreeNodeAdv[])) && this.DropPosition.Node != null)
            {
                TreeNodeAdv[] nodes = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];
                TreeNodeAdv parent = this.DropPosition.Node;
                if (this.DropPosition.Position != NodePosition.Inside)
                    parent = parent.Parent;

                foreach (TreeNodeAdv node in nodes)
                    if (!CheckNodeParent(parent, node))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }

                e.Effect = e.AllowedEffect;
            }
        }

        private bool CheckNodeParent(TreeNodeAdv parent, TreeNodeAdv node)
        {
            while (parent != null)
            {
                if (node == parent)
                    return false;
                else
                    parent = parent.Parent;
            }
            return true;
        }

        private void _tree_DragDrop(object sender, DragEventArgs e)
        {
            if (!SelfLevelEdit) return;

            this.BeginUpdate();//开始编辑

            TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
            Node dropNode = this.DropPosition.Node.Tag as Node;
            if (this.DropPosition.Position == NodePosition.Inside)
            {
                foreach (TreeNodeAdv n in nodes)
                {
                    (n.Tag as Node).Parent = dropNode;
                }
                this.DropPosition.Node.IsExpanded = true;
            }
            else
            {
                Node parent = dropNode.Parent;
                Node nextItem = dropNode;
                if (this.DropPosition.Position == NodePosition.After)
                    nextItem = dropNode.NextNode;

                foreach (TreeNodeAdv node in nodes)
                    (node.Tag as Node).Parent = null;

                int index = -1;
                index = parent.Nodes.IndexOf(nextItem);
                foreach (TreeNodeAdv node in nodes)
                {
                    Node item = node.Tag as Node;
                    if (index == -1)
                        parent.Nodes.Add(item);
                    else
                    {
                        parent.Nodes.Insert(index, item);
                        index++;
                    }
                }
            }

            this.EndUpdate();//结束编辑
        }
    }
    public static class NodeExtension
    {
        public static Node SetParam(this Node node, string text)
        {
            return SetParam(node, text, new TreeNodeInfo());
        }
        public static Node SetParam(this Node node, TreeNodeInfo info)
        {
            if (info == null) return node;
            else
                return SetParam(node, info.Caption, info);
        }
        public static Node SetParam(this Node node, string text, TreeNodeInfo info)
        {
            //if (Skyray.Language.Lang.Model.CheckToSaveText())
            //{
            //    Skyray.Language.Lang.Model.TreeData.Add(Skyray.Language.Lang.Model.InsertData(info.Name, text));
            //    node.Text = text;
            //}
            //else
            //{
            //    if (Skyray.Language.Lang.Model.LangData.ContainsKey(info.Name))
            //        node.Text = Skyray.Language.Lang.Model.LangData[info.Name];
            //}
            node.Text = text;
            node.Tag = info;
            return node;
        }
    }
}
