using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Skyray.EDX.Common
{

    [Serializable]
    public class DragDropMenuItem
    {
        private int _position;

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private string _currentText;

        public string CurrentText
        {
            get { return _currentText; }
            set { _currentText = value; }
        }

        private string _currentItemName;

        public string CurrentItemName
        {
            get { return _currentItemName; }
            set { _currentItemName = value; }
        }

        private bool _isRoot;

        public bool IsRoot
        {
            get { return _isRoot; }
            set { _isRoot = value; }
        }

        private bool _isExistsChild;

        public bool IsExistsChild
        {
            get { return _isExistsChild; }
            set { _isExistsChild = value; }
        }

        private string _preItemName;

        public string PreItemName
        {
            get { return _preItemName; }
            set { _preItemName = value; }
        }

        private int _showToolsPositoin;

        public int ShowToolPositoin
        {
            get { return _showToolsPositoin; }
            set { _showToolsPositoin = value; }
        }

        private string _parentItemName;

        public string ParentItemName
        {
            get { return _parentItemName; }
            set { _parentItemName = value; }
        }

        public DragDropMenuItem(int _position, string _currentText, string _currentItemName, bool _isRoot, bool _isExistChild, string _preItemName, string _parentItemName)
        {
            this.Position = _position;
            this.CurrentItemName = _currentItemName;
            this.IsRoot = _isRoot;
            this.IsExistsChild = _isExistChild;
            this.PreItemName = _preItemName;
            this.ParentItemName = _parentItemName;
            this.CurrentText = _currentText;
        }
    }

    public class MTlSToDragHelper
    {
        /// <summary>
        /// 菜单对象转为可序列化对象
        /// </summary>
        /// <param name="stripControl"></param>
        /// <param name="dropItemList"></param>
        public static void AddToolStripControlToDragDrop(ToolStripControls stripControl, ref List<DragDropMenuItem> dropItemList)
        {
            if (stripControl == null || stripControl.CurrentNaviItem == null|| stripControl.parentStripMeauItem == null || stripControl.parentStripMeauItem.CurrentNaviItem == null)
                return;
            DragDropMenuItem dropItem = new DragDropMenuItem(stripControl.Postion, stripControl.CurrentNaviItem.Text, stripControl.CurrentNaviItem.Name, stripControl.isRoot, stripControl.isExistsChild, stripControl.preToolStripMeauItem == null ? "" : stripControl.preToolStripMeauItem.CurrentNaviItem.Name, stripControl.parentStripMeauItem.CurrentNaviItem.Name);
            if (dropItemList.Find(w => w.CurrentItemName == dropItem.CurrentItemName) == null)
                dropItemList.Add(dropItem);
        }
        /// <summary>
        /// 序列化对象转为菜单对象
        /// </summary>
        /// <param name="tls"></param>
        /// <param name="item"></param>
        public static void TranslateToOrignMenuObj(List<ToolStripControls> tls, DragDropMenuItem item)
        {
            if (tls == null || tls.Count == 0 || item == null)
                return;
            var temp = tls.Find(w => w.CurrentNaviItem.Name == item.CurrentItemName);
            if (temp != null)
            {
                temp.Postion = item.Position;
                var preItem = tls.Find(w => w.CurrentNaviItem.Name == item.PreItemName);
                if (preItem != null)
                    temp.preToolStripMeauItem = preItem;
                var parentItem = tls.Find(w => w.CurrentNaviItem.Name == item.ParentItemName);
                if (parentItem != null)
                temp.parentStripMeauItem = parentItem;
            }
        }

        public static object GetPassedUserDataInitialization(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists(fileName))
            {
                using (FileStream _FileStream = new System.IO.FileStream(fileName,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read,
                    System.IO.FileShare.None
                    ))
                {
                    _FileStream.Position = 0;
                    _FileStream.Seek(0, SeekOrigin.Begin);
                    return formatter.Deserialize(_FileStream);
                }
            }
            else
                return null;
        }
    }
}
