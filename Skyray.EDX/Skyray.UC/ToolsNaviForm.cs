using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.UC;
using Skyray.EDX.Common;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Skyray.Language;
using Skyray.EDXRFLibrary;
using Skyray.Controls.Tree.NodeControls;
//.....................................................
namespace Skyray.UC
{
    public partial class ToolsNaviForm : UCMultiple
    {
        private List<MenuConfig> lstConfigLoad = new List<MenuConfig>();
        private List<DragDropMenuItem> dropItemList = new List<DragDropMenuItem>();
        private string originalPath = string.Empty;
        //修改：何晓明 20110713 增加快捷键_初始化快捷键下拉框        
        private void InitComboBox()
        {
            this.cmbModifier.Items.AddRange(new object[]{"None","Control","Shift","Alt"});            
            //this.cmbKey.Items.AddRange(new object[]{Keys.None,Keys.F1,Keys.F2,Keys.F3,Keys.F4,Keys.F5,Keys.F6,Keys.F7,Keys.F8,Keys.F9,Keys.F10,Keys.F11,Keys.F12});
            this.cmbKey.Items.Add(((Keys)0).ToString());
            for (int i = 65; i < 91;i++ )
            {
                this.cmbKey.Items.Add(((Keys)i).ToString());
            }
            for(int j = 112;j<124;j++)
            {
                this.cmbKey.Items.Add(((Keys)j).ToString());
            }
            if (cmbModifier.Items.Count > 0) cmbModifier.SelectedIndex = 0;
            if (cmbKey.Items.Count > 0) cmbKey.SelectedIndex = 0;            
        }
        //
        public ToolsNaviForm()
        {
            InitializeComponent();
            this.comboBox1.DataSource = new string[] { Info.Administrator, Info.StandandUser, Info.CommonUser };
            originalPath = this.textBoxW1.Text;
        }

        private void buttonWSubmit_Click(object sender, EventArgs e)
        {
            btnApplication_Click(null, null);
            EDXRFHelper.GotoMainPage(this);
        }

        private void buttonWCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void ToolsNaviForm_Load(object sender, EventArgs e)
        {
            ExceptClientGrobal  ClientGrobal  = new ExceptClientGrobal();
            GetNaviItems();
            //修改：何晓明 20110713 增加快捷键_初始化快捷键下拉框
            InitComboBox();
        }

        private bool RecurveMenuItem(ToolStripControls preControls, int position, ToolStripControls rootControls,TreeNode parentNode)
        {
            if (preControls == null || preControls.CurrentNaviItem == null ||
                preControls.CurrentNaviItem.MenuStripItem == null)
                return false;
            bool flag = false;
            TreeNode nextNode = new TreeNode(preControls.CurrentNaviItem.Text);
            nextNode.Tag = preControls.CurrentNaviItem.Name;
            parentNode.Nodes.Add(nextNode);
            if (preControls.isExistsChild)
            {
                ToolStripControls toolsControl = MenuLoadHelper.MenuStripCollection.Find(w => w.Postion == position && w.parentStripMeauItem == preControls && w.preToolStripMeauItem == null);
                flag = RecurveMenuItem(toolsControl, position, preControls, nextNode);
            }

            if (!flag)
            {
                ToolStripControls toolsControl = MenuLoadHelper.MenuStripCollection.Find(w => w.Postion == position && w.preToolStripMeauItem == preControls);
                flag = RecurveMenuItem(toolsControl, position, rootControls, parentNode);
            }
            return flag;
        }

        private void GetNaviItems()
        {
            this.treeView1.Nodes.Clear();
            List<ToolStripControls> ToolPList = MenuLoadHelper.MenuStripCollection.FindAll(delegate(ToolStripControls v) { return v.isRoot == true; });
            var VToolPList = from item in ToolPList orderby item.Postion select item;
            foreach (ToolStripControls P in VToolPList)
            {
                TreeNode nextNode = new TreeNode(P.CurrentNaviItem.Text);
                nextNode.Tag = P.CurrentNaviItem.Name;

                ToolStripControls toolsRootPre = MenuLoadHelper.MenuStripCollection.Find(delegate(ToolStripControls ss)
                { return ss.Postion == P.Postion && ss.preToolStripMeauItem == P; });

                RecurveMenuItem(toolsRootPre, P.Postion, P,nextNode);
                this.treeView1.Nodes.Add(nextNode);
            }
           
            //获取其他
            List<NaviItem> OtherList = new List<NaviItem>();
            foreach (NaviItem item in WorkCurveHelper.NaviItems)
                if (!MenuLoadHelper.MenuStripCollection.Exists(delegate(ToolStripControls v) { return v.CurrentNaviItem.Text == item.Text&&v.Postion!=10000; }))
                    OtherList.Add(item);
            if (OtherList.Count > 0)
            {
                TreeNode nextONode = new TreeNode(Info.strOther);
                foreach (NaviItem i in OtherList)
                {
                    TreeNode tt = new TreeNode(i.Text);
                    tt.Tag = i.Name;
                    nextONode.Nodes.Add(tt);
                }
                nextONode.Tag = "Others";
                this.treeView1.Nodes.Add(nextONode);
            }


            ////获取其他
            //List<NaviItem> CamerList = new List<NaviItem>();
            //foreach (NaviItem item in WorkCurveHelper.NaviItems)
            //    if (!MenuLoadHelper.MenuStripCollection.Exists(delegate(ToolStripControls v) { return v.CurrentNaviItem.Text == item.Text && v.Postion != 10000; }))
            //        CamerList.Add(item);
            //if (CamerList.Count > 0)
            //{
            //    TreeNode nextONode = new TreeNode(Info.strOther);
            //    foreach (NaviItem i in CamerList)
            //    {
            //        TreeNode tt = new TreeNode(i.Text);
            //        tt.Tag = i.Name;
            //        nextONode.Nodes.Add(tt);
            //    }
            //    nextONode.Tag = "Camera";
            //    this.treeView1.Nodes.Add(nextONode);
            //}
        }

      

        private void buttonSetting_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Count == 0 || treeView1.SelectedNode == null) return;
            string itemName = treeView1.SelectedNode.Tag.ToString();
            NaviItem item = WorkCurveHelper.NaviItems.Find(w => w.Name == itemName);
            if (item != null)
            {
                bool showInMain = this.chkIsInMainForm.Checked;
                int flagType =0;
                try
                {
                    flagType = int.Parse(this.numericTextBox1.Text.ToString());
                }
                catch (FormatException)
                {
                    Msg.Show("输入格式错误！");
                    return;
                }
                bool showInToolBar = this.chkIsInTools.Checked;
                bool showInMenu = this.chkIsInMenu.Checked;
                //string text =this.textBoxW1.Text;
                bool enableControl = !this.chbEnabelControl.Checked;
                if (cmbKey.Text == "None") 
                    cmbModifier.SelectedIndex = cmbModifier.Items.IndexOf("None");
                var hotKeyOwner = lstConfigLoad.Find(w => w.ShortcutKeys != "None" && w.ShortcutKeys == ((Keys)Enum.Parse(typeof(Keys), cmbModifier.Text) | (Keys)Enum.Parse(typeof(Keys), cmbKey.Text)).ToString()
                    && w.ItemText != item.Text);
                if (hotKeyOwner != null)
                {
                    string text = WorkCurveHelper.NaviItems.Find(w => w.Name == hotKeyOwner.ItemName).Text;
                    strPath = string.Empty;
                    if (DialogResult.OK != Msg.Show("\"" + GetPath(text) + "\"" + Info.HotKeyExists, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                    {
                        item.MenuStripItem.ShortcutKeys = (Keys)Enum.Parse(typeof(Keys), "None") | (Keys)Enum.Parse(typeof(Keys), "None");
                        cmbKey.SelectedIndex = 0;
                        cmbModifier.SelectedIndex = 0;
                    }
                    else
                    {
                        hotKeyOwner.ShortcutKeys = "None";
                        item.MenuStripItem.ShortcutKeys = (Keys)Enum.Parse(typeof(Keys), cmbModifier.Text) | (Keys)Enum.Parse(typeof(Keys), cmbKey.Text);
                    }
                }
                else
                {
                    if (!(((int)(Keys)Enum.Parse(typeof(Keys), cmbKey.Text) > 64) && ((int)(Keys)Enum.Parse(typeof(Keys), cmbKey.Text)) < 91) || cmbModifier.Text == "Control" || cmbModifier.Text == "Alt")
                        item.MenuStripItem.ShortcutKeys = (Keys)Enum.Parse(typeof(Keys), cmbModifier.Text) | (Keys)Enum.Parse(typeof(Keys), cmbKey.Text);
                    else if (cmbModifier.Text == "None" || cmbModifier.Text == "Shift")
                        item.MenuStripItem.ShortcutKeys = (Keys)Enum.Parse(typeof(Keys), "Control") | (Keys)Enum.Parse(typeof(Keys), cmbKey.Text);
                }
                if ((this.comboBox1.SelectedIndex == 0 || this.comboBox1.SelectedIndex == 1) && (item.Name == "Tools" || item.Name == "CreateToolsAppConfig") && !showInMenu)
                    showInMenu = true;
                if (this.comboBox1.SelectedIndex != 2)
                {
                    item.FlagType = flagType;
                    item.ShowInMain = showInMain;
                    item.ShowInToolBar = showInToolBar;
                    item.Enabled = showInMenu;
                    //item.Text = text;
                    item.EnabledControl = enableControl;
  
                    //修改：何晓明 20110713 增加快捷键_赋值快捷键
                }
                MenuConfig config = null;
                if (lstConfigLoad != null && lstConfigLoad.Count > 0)
                {
                    config = lstConfigLoad.Find(w => w.ItemName == itemName);
                    if (config != null)
                    {
                        config.ShowInMain = showInMain;
                        config.ShowInTools = showInToolBar;
                        config.Enable = showInMenu;
                        config.ItemName = item.Name;
                        //config.ItemText = text;
                        config.FlagType = flagType;
                        config.EnableControl = enableControl;
                        //修改： 何晓明 20110713 增加快捷键
                        config.ShortcutKeys = item.MenuStripItem.ShortcutKeys.ToString();
                    }
                }
            }
            IsSetting = true;
            Msg.Show(Info.SetSuccess);
        }
        //修改：何晓明 20110713 增加快捷键
        string strPath = string.Empty;
        bool IsSetting = false;
        private string GetPath(string Child)
        {
            strPath +=Child+"<-";
            string strReturn = string.Empty;
            var menu = MenuLoadHelper.MenuStripCollection.Find(w => w.CurrentNaviItem.Text == Child);
            if(menu.preToolStripMeauItem!=null)
            GetPath(menu.parentStripMeauItem.CurrentNaviItem.Text);
            string[] strPaths = strPath.Split("<-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = strPaths.Length-1; i >= 0;i-- )
            {
                strReturn += "->" + strPaths[i];
            }
            return strReturn.Substring(2);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsSetting)
                btnApplication_Click(null, null);
            if ((this.comboBox1.SelectedIndex == 0||this.comboBox1.SelectedIndex == 1) && DifferenceDevice.IsAnalyser)
                this.txtSaveFile.Text = "EDX3000ToolsConfig_Admin.txt";
            else if ((this.comboBox1.SelectedIndex == 2) && DifferenceDevice.IsAnalyser)
                this.txtSaveFile.Text = "EDX3000ToolsConfig1_CommonUser.txt";
            else if ((this.comboBox1.SelectedIndex == 0 || this.comboBox1.SelectedIndex == 1) && DifferenceDevice.IsXRF)
                this.txtSaveFile.Text = "XRFToolsConfig_Admin.txt";
            else if ((this.comboBox1.SelectedIndex == 2) && DifferenceDevice.IsXRF)
                this.txtSaveFile.Text = "XRFToolsConfig1_CommonUser.txt";
            else if ((this.comboBox1.SelectedIndex == 0||this.comboBox1.SelectedIndex == 1) && DifferenceDevice.IsRohs)
                this.txtSaveFile.Text = "RohsToolsConfigNew_Admin.txt";
            else if ((this.comboBox1.SelectedIndex == 2) && DifferenceDevice.IsRohs)
                this.txtSaveFile.Text = "RohsToolsConfig1_CommonUser.txt";
            else if ((this.comboBox1.SelectedIndex == 0||this.comboBox1.SelectedIndex == 1) && DifferenceDevice.IsThick)
                this.txtSaveFile.Text = "ThickToolsConfig_Admin.txt";
            else if ((this.comboBox1.SelectedIndex == 2) && DifferenceDevice.IsThick)
                this.txtSaveFile.Text = "ThickToolsConfig1_CommonUser.txt";
            lstConfigLoad = (List<MenuConfig>)MTlSToDragHelper.GetPassedUserDataInitialization(Application.StartupPath + "\\" + this.txtSaveFile.Text);
          
            //从文件中得到配置
            List<DragDropMenuItem> lstDrapItem = (List<DragDropMenuItem>)MTlSToDragHelper.GetPassedUserDataInitialization(Application.StartupPath + "\\" + Path.GetFileNameWithoutExtension(this.txtSaveFile.Text));
            if (lstDrapItem != null && lstDrapItem.Count > 0)
            {
                //设置菜单对象关系
                lstDrapItem.ForEach(w => { MTlSToDragHelper.TranslateToOrignMenuObj(MenuLoadHelper.MenuStripCollection, w); });
                dropItemList = lstDrapItem;
            }
            else
            {
                //根据菜单关系重新生成文件列表
                MenuLoadHelper.MenuStripCollection.ForEach(w => { MTlSToDragHelper.AddToolStripControlToDragDrop(w, ref dropItemList); });
            }

            //菜单栏配置树收起
            this.treeView1.CollapseAll();
            
            //从新加载工具栏顺序
            if (this.tbPage1.SelectedIndex == 1)
            {
                ToolsOrderFunc();
            }
            this.treeView2.CollapseAll();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                selectedNode = e.Node;
                string itemName=e.Node.Tag.ToString();
                bool flag = false;
                if (lstConfigLoad != null && lstConfigLoad.Count > 0)
                {
                    MenuConfig config = lstConfigLoad.Find(delegate(MenuConfig v) { return v.ItemName == itemName; });
                    if (config != null)
                    {
                        this.chkIsInMainForm.Checked = config.ShowInMain;
                        this.chkIsInTools.Checked =config.ShowInTools;
                        this.chkIsInMenu.Checked = config.Enable;
                        this.chbEnabelControl.Checked = !config.EnableControl;
                        //修改：何晓明 20110713 增加快捷键                    
                        if (config.ShortcutKeys != null && config.ShortcutKeys != "None")
                        {
                            string[] keys = config.ShortcutKeys.Split(',');
                            //this.cmbKey.SelectedIndex = cmbKey.Items.ToString().IndexOf(keys[0]);
                            foreach (var key in cmbKey.Items)
                            {
                                if (key.ToString() == keys[0].Trim())
                                {
                                    cmbKey.SelectedIndex = cmbKey.Items.IndexOf(key);
                                    break;
                                }
                            }
                            if (keys.Length > 1)
                                this.cmbModifier.SelectedIndex = cmbModifier.Items.IndexOf(keys[1].Trim());                                
                        }
                        else
                        {
                            cmbKey.SelectedIndex = 0;
                            cmbModifier.SelectedIndex = 0;
                        }
                        this.numericTextBox1.Text = config.FlagType.ToString();
                        this.textBoxW1.Text = config.ItemText;
                        flag = true;
                    }
                }

                if (!flag)
                {
                    NaviItem item = WorkCurveHelper.NaviItems.Find(w => w.Name == itemName);
                    if (item != null)
                    {
                        if (lstConfigLoad == null)
                        {
                            lstConfigLoad = new List<MenuConfig>();
                        }
                       //增加菜单文件栏
                        MenuConfig config = new MenuConfig();
                        lstConfigLoad.Add(config);

                        config.ItemName = item.Name;
                        config.ShowInMain = false;
                        config.ShowInTools = false;
                        config.Enable = false;
                        config.EnableControl = false;

                        //显示在界面上
                        this.chkIsInMainForm.Checked = false;
                        this.chkIsInTools.Checked = false;
                        this.chkIsInMenu.Checked = false;
                        this.chbEnabelControl.Checked = false;

                        //修改：何晓明 20110713 增加快捷键                    
                        if (item.MenuStripItem.ShortcutKeys.ToString() != "None")
                        {
                            string[] keys = item.MenuStripItem.ShortcutKeys.ToString().Split(',');
                            this.cmbKey.SelectedIndex = cmbKey.Items.IndexOf(keys[0]);
                            if (keys.Length > 1)
                                this.cmbModifier.SelectedIndex = cmbModifier.Items.IndexOf(keys[1]);
                        }
                        this.numericTextBox1.Text = item.FlagType.ToString();
                        this.textBoxW1.Text = item.Text;
                    }
                }

            }
        }

        private void btnApplication_Click(object sender, EventArgs e)
        {
            if (WorkCurveHelper.NaviItems.Count == 0)
                return;
            List<MenuConfig> lsConfig = new List<MenuConfig>();
            foreach (NaviItem item in WorkCurveHelper.NaviItems)
            {
                MenuConfig config = null;
                if (lstConfigLoad != null && lstConfigLoad.Count > 0)
                {
                    config = lstConfigLoad.Find(w => w.ItemName == item.Name);
                    if (config != null)
                        lsConfig.Add(config);
                }

                if (config == null)
                {
                    if (lstConfigLoad == null)
                        lstConfigLoad = new List<MenuConfig>();
                    MenuConfig newConfig = new MenuConfig();
                    newConfig.ShowInMain = item.ShowInMain;
                    newConfig.ShowInTools = item.ShowInToolBar;
                    newConfig.Enable = item.Enabled;
                    newConfig.ItemName = item.Name;
                    newConfig.ItemText = item.Text;
                    newConfig.FlagType = item.FlagType;
                    newConfig.EnableControl = item.EnabledControl;
                    lsConfig.Add(newConfig);
                }
            }
            if (lsConfig.Count > 0)
            {
                string path = Application.StartupPath + "\\" + this.txtSaveFile.Text;
                IFormatter formatter = new BinaryFormatter();
                if (File.Exists(path))
                    File.Delete(path);
                Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, lsConfig);
                stream.Close();
                IsSetting = false;
            }

            if (this.dropItemList.Count > 0)
            {
                string path = Application.StartupPath + "\\" + this.txtSaveFile.Text;
                path = Path.GetFileNameWithoutExtension(path);
                BinaryFormatter formatter = new BinaryFormatter();
                if (File.Exists(path))
                    File.Delete(path);
                Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, this.dropItemList);
                stream.Close();
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            // Enable timer for scrolling dragged item
            e.Effect = DragDropEffects.Move; 
        }


        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode newNode;
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt;
                TreeNode destinationNode;
                pt = this.treeView1.PointToClient(new Point(e.X, e.Y));
                destinationNode = this.treeView1.GetNodeAt(pt);
                newNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if (!destinationNode.Equals(newNode))
                {
                    DragDropMenuItem controlTemp = this.dropItemList.Find(w => w.CurrentItemName == newNode.Tag.ToString());
                    if (string.IsNullOrEmpty(controlTemp.PreItemName))
                        return;
                    if (controlTemp != null)
                    {
                        controlTemp.ParentItemName = destinationNode.Tag.ToString();
                        DragDropMenuItem controlTemp1 = this.dropItemList.Find(w => w.PreItemName == newNode.Tag.ToString());
                        if (controlTemp1 != null)
                        {
                            controlTemp1.PreItemName = controlTemp.PreItemName;
                        }
                        if (destinationNode.Nodes.Count > 0)
                            controlTemp.PreItemName = destinationNode.Nodes[destinationNode.Nodes.Count - 1].Tag.ToString();
                        else
                            controlTemp.PreItemName = destinationNode.Tag.ToString();
                        DragDropMenuItem parentlTemp = this.dropItemList.Find(w => w.CurrentItemName == destinationNode.Tag.ToString());
                        if (parentlTemp != null)
                            controlTemp.Position = parentlTemp.Position;
                    }
                    destinationNode.Nodes.Add((TreeNode)newNode.Clone());
                    destinationNode.Expand();
                    //Remove original node 
                    newNode.Remove();
                }
            } 

        }

        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            TreeView tv = sender as TreeView;
            Point pt = tv.PointToClient(new Point(e.X, e.Y));
            int delta = tv.Height - pt.Y;
            if ((delta < tv.Height / 2) && (delta > 0))
            {
                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
                if (tn.NextVisibleNode != null)
                    tn.NextVisibleNode.EnsureVisible();
            }
            if ((delta > tv.Height / 2) && (delta < tv.Height))
            {
                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
                if (tn.PrevVisibleNode != null)
                    tn.PrevVisibleNode.EnsureVisible();
            } 

        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move); 
        }

        TreeNode selectedNode = null;

        private void TlsMenuItemUp_Click(object sender, EventArgs e)
        {
            if (selectedNode == null)
                return;
            if (selectedNode.Index == 0 || selectedNode.Level == 0)
                return;
            TreeNode parentNode = selectedNode.Parent;
            TreeNode preNode = selectedNode.PrevNode;
            TreeNode nextSibNode = selectedNode.NextNode;

            //如果选中节点为最顶节点  
            DragDropMenuItem currentItem = dropItemList.Find(w => w.CurrentItemName == selectedNode.Tag.ToString());
            DragDropMenuItem preItem = dropItemList.Find(w => w.CurrentItemName == preNode.Tag.ToString());
            DragDropMenuItem nextItem = null;
            if (nextSibNode != null)
                nextItem = dropItemList.Find(w => w.CurrentItemName == nextSibNode.Tag.ToString());

            TreeNode newnode = (TreeNode)selectedNode.Clone();
            parentNode.Nodes.Insert(preNode.Index, newnode);
            parentNode.Nodes.Remove(selectedNode);
            //修改存储对象
            if (this.tbPage1.SelectedIndex == 0)
            {
                currentItem.PreItemName = preItem.PreItemName;
                preItem.PreItemName = currentItem.CurrentItemName;
                if (nextItem != null)
                    nextItem.PreItemName = preItem.CurrentItemName;
                this.treeView1.SelectedNode = newnode;
            }
            else if (this.tbPage1.SelectedIndex == 1)
            {
                int temp = currentItem.ShowToolPositoin;
                currentItem.ShowToolPositoin = preItem.ShowToolPositoin;
                preItem.ShowToolPositoin = temp;
                this.treeView2.SelectedNode = newnode;
            }
        }

        private void TlsMenuItemDown_Click(object sender, EventArgs e)
        {
            if (selectedNode == null)
                return;
            //根节点
            if (selectedNode.Level == 0)
                return;
            //最后一个孩子节点
            if (selectedNode.Index == selectedNode.Parent.Nodes.Count - 1)
                return;
            TreeNode parentNode = selectedNode.Parent;
            TreeNode nextSibNode = selectedNode.NextNode;

            DragDropMenuItem currentItem = dropItemList.Find(w => w.CurrentItemName == selectedNode.Tag.ToString());
            DragDropMenuItem nextItem = dropItemList.Find(w => w.CurrentItemName == nextSibNode.Tag.ToString());
            DragDropMenuItem nextTItem = null;
            if (nextSibNode.NextNode != null)
                nextTItem = dropItemList.Find(w => w.CurrentItemName == nextSibNode.NextNode.Tag.ToString());

            TreeNode newnode = (TreeNode)selectedNode.Clone();
            parentNode.Nodes.Insert(selectedNode.NextNode.Index + 1, newnode);
            parentNode.Nodes.Remove(selectedNode);
            //修改存储对象
            if (this.tbPage1.SelectedIndex == 0)
            {
                nextItem.PreItemName = currentItem.PreItemName;
                currentItem.PreItemName = nextItem.CurrentItemName;
                if (nextTItem != null)
                    nextTItem.PreItemName = currentItem.CurrentItemName;
                this.treeView1.SelectedNode = newnode;
            }
            else if (this.tbPage1.SelectedIndex == 1)
            {
                int temp = currentItem.ShowToolPositoin;
                currentItem.ShowToolPositoin = nextItem.ShowToolPositoin;
                nextItem.ShowToolPositoin = temp;
                this.treeView2.SelectedNode = newnode;
            }
           
        }

        private void ToolsOrderFunc()
        {
            this.treeView2.Nodes.Clear();
            TreeNode treeNode = new TreeNode(Info.Others);

            //工具栏位置排序
            var VToolPList = from item in dropItemList orderby item.ShowToolPositoin select item;
            List<DragDropMenuItem> itemList = new List<DragDropMenuItem>();
            foreach (DragDropMenuItem item in VToolPList)
            {
                MenuConfig findConfig = lstConfigLoad.Find(w => w.ShowInTools == true && w.ItemName == item.CurrentItemName);
                if (findConfig != null)
                {
                    var itemsc = WorkCurveHelper.NaviItems.Find(w => w.Name == findConfig.ItemName);
                    if (itemsc != null)
                    {
                        TreeNode node = new TreeNode(itemsc.Text);
                        node.Tag = findConfig.ItemName;
                        itemList.Add(item);
                        treeNode.Nodes.Add(node);
                    }
                }
            }
            this.treeView2.Nodes.Add(treeNode);
            if (itemList.Count == 0)
                return;
            int index = 0;
            itemList.ForEach(w => { w.ShowToolPositoin = index++; });
        }
     
        private void tbPage1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tbPage1.SelectedIndex == 1)
            {
                ToolsOrderFunc();
            }
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectedNode = e.Node;
        }

        private void grpSettings_Load(object sender, EventArgs e)
        {

        }
    }
}
