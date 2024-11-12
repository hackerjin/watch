using System;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;
using Skyray.Controls.Tree.NodeControls;

namespace Skyray.Controls.Tree
{
    public class TreeViewAdvEx : TreeViewAdv
    {
        /// <summary>
        /// 是否启用自动拖拽事件
        /// </summary>
        public bool AutoItemDrag { get; set; }

        /// <summary>
        /// 是否启用标签重组
        /// </summary>
        public bool SelfLevelEdit { get; set; }

        public NodeTextBox NodeTextBox { get; set; }

        public TreeViewAdvEx()
        {
            NodeTextBox = new NodeTextBox();
            NodeTextBox.DataPropertyName = "Text";
            NodeTextBox.EditEnabled = true;
            this.NodeControls.Add(NodeTextBox);

            this.DragOver += new DragEventHandler(_tree_DragOver);
            this.DragDrop += new DragEventHandler(_tree_DragDrop);
            this.ItemDrag += new ItemDragEventHandler(_tree_ItemDrag);
        }

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
            this.BeginUpdate();

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

            this.EndUpdate();
        }

    }

    /// <summary>
    /// Using a System.Data.DataTable to represent a tree structure 
    /// </summary>
    public class DataTableTreeModel : TreeModelBase
    {
        private DataRowNode m_root;

        DataTable m_table;
        string m_IDColumnName;

        public DataTableTreeModel(DataTable table, string idColumnName)
        {
            m_table = table;
            m_IDColumnName = idColumnName;
            DataRow[] rows = table.Select(m_IDColumnName + " = ParentID");
            if (rows.Length == 0)
            {
                throw new Exception("DataTableModel Requires a root Node");
            }
            m_root = new DataRowNode(rows[0], rows[0]["Name"].ToString());
            m_root.Row = rows[0];
        }

        public override IEnumerable GetChildren(TreePath treePath)
        {
            List<DataRowNode> items = new List<DataRowNode>();

            if (treePath.IsEmpty())
            {
                items.Add(m_root);
            }
            else
            {
                DataRowNode n = treePath.LastNode as DataRowNode;

                DataRow row = n.Row;
                int id = Convert.ToInt32(row[m_IDColumnName]);

                DataRow[] rows = m_table.Select("ParentID = " + id + " and " + m_IDColumnName + " <> " + id);
                foreach (DataRow r in rows)
                {
                    DataRowNode node = new DataRowNode(r, r["Name"].ToString());
                    node.Row = r;
                    //SampleApp.Properties.Resources.ResourceManager.
                    //node.Icon = new Bitmap(SampleApp.Properties.Resources.Records,new Size(15,15));
                    items.Add(node);
                }
            }
            return items;
        }

        public override bool IsLeaf(TreePath treePath)
        {
            DataRowNode n = treePath.LastNode as DataRowNode;
            if (n.Row["IsFolder"] == DBNull.Value)
                return false;
            return !Convert.ToBoolean(n.Row["IsFolder"]);
        }


        //public event EventHandler<TreeModelEventArgs> NodesChanged;

        //public event EventHandler<TreeModelEventArgs> NodesInserted;

        //public event EventHandler<TreeModelEventArgs> NodesRemoved;

        //public event EventHandler<TreePathEventArgs> StructureChanged;


        public void AddChild(TreePath parent, string text)
        {
            DataRowNode n = parent.LastNode as DataRowNode;

            DataRow r = m_table.NewRow();
            r["ID"] = GetNextID();
            r["ParentID"] = n.Row["ID"];
            r["IsFolder"] = false;
            r["Name"] = text;
            r["Tag"] = "";
            m_table.Rows.Add(r);
            DataRowNode child = new DataRowNode(r, text);
            OnStructureChanged(new TreePathEventArgs(parent));
        }

        private int GetNextID()
        {
            int max = 1;
            for (int i = 0; i < m_table.Rows.Count; i++)
            {
                int id = Convert.ToInt32(m_table.Rows[i]["ID"]);
                if (id > max)
                    max = id;
            }

            return max + 1;
        }
    }

    public class DataRowNode
    {
        public DataRowNode(DataRow row, string text)
        {
            Row = row;
            Text = text;
        }
        public string Text { get; set; }
        public Image Icon { get; set; }
        public DataRow Row { get; set; }
    }
}
