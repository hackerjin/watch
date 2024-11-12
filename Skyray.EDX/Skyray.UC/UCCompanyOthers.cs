using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCCompanyOthers :Skyray.Language.UCMultiple
    {
        public UCCompanyOthers()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.cbxInpoutItem.Items.Count == 0) return;
            List<XmlNode> tempOut = new List<XmlNode>();
            bool result = this.listString.TryGetValue(this.cbxInpoutItem.Items[this.cbxInpoutItem.SelectedIndex].ToString(), out tempOut);
            if (!result)
                return;
            int addValue = ++maxId;
            XmlNode createNode = doc.CreateElement("value");
            XmlAttribute att = doc.CreateAttribute("Id");
            att.Value = addValue.ToString();
            createNode.Attributes.SetNamedItem(att);
            att = doc.CreateAttribute("isCurrent");
            att.Value = "0";
            createNode.Attributes.SetNamedItem(att);
            createNode.InnerText = "";
            tempOut.Add(createNode);
            this.dgvValues.Rows.Add(false, addValue, "");
            this.listString.Remove(this.cbxInpoutItem.SelectedItem.ToString());
            this.listString.Add(this.cbxInpoutItem.SelectedItem.ToString(), tempOut);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<XmlNode> tempOut = new List<XmlNode>();
            if (this.dgvValues.CurrentRow == null)
                return;
            if (this.listString.TryGetValue(this.cbxInpoutItem.Items[this.cbxInpoutItem.SelectedIndex].ToString(), out tempOut))
            {
                XmlNode deleteNode = tempOut.Find(w => w.Attributes["Id"].Value.ToString() == this.dgvValues.CurrentRow.Cells["dgvId"].Value.ToString());
                tempOut.Remove(deleteNode);
            }
            this.dgvValues.Rows.Remove(this.dgvValues.CurrentRow);
        }

        private XmlNode currentTemplateNode;
        private Dictionary<string, List<XmlNode>> listString = new Dictionary<string, List<XmlNode>>();
        private XmlDocument doc;
        private string dicPath;
        private int maxId;

        private void UCCompanyOthers_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            dicPath = AppDomain.CurrentDomain.BaseDirectory + "//printxml";
            DirectoryInfo info = new DirectoryInfo(dicPath);
            if (!info.Exists)
                return;
            dicPath += "//CompanyInfo.xml";
            if (!File.Exists(dicPath))
                return;
            doc = new XmlDocument();
            doc.Load(dicPath);
            XmlNodeList xmlNodes = doc.SelectNodes("Data/template");
            foreach(XmlNode temp in xmlNodes)
                if (temp.Attributes["Name"].Value.ToString().Equals(ReportTemplateHelper.LoadReportName(),StringComparison.OrdinalIgnoreCase))
                {
                    currentTemplateNode = temp;
                    XmlNodeList listTemps = temp.SelectNodes("lable");
                    foreach (XmlNode tt in listTemps)
                    {
                        string name = tt.Attributes["Name"].Value.ToString();
                        this.cbxInpoutItem.Items.Add(name);
                        XmlNodeList valuesList = tt.SelectNodes("value");
                        List<XmlNode> valueStr = new List<XmlNode>();
                        foreach (XmlNode valueTemp in valuesList)
                            valueStr.Add(valueTemp);
                        listString.Add(name, valueStr);
                    }
                }
            if (this.cbxInpoutItem.Items.Count > 0)
                this.cbxInpoutItem.SelectedIndex = 0;
        }

        private void cbxInpoutItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listString.Count == 0)
                return;
            this.dgvValues.Rows.Clear();
            List<XmlNode> tempOut = new List<XmlNode>();
            if (this.listString.TryGetValue(this.cbxInpoutItem.Items[this.cbxInpoutItem.SelectedIndex].ToString(), out tempOut))
            {
                tempOut.ForEach(w =>
                {
                    if (w.Attributes["Id"] != null)
                    {
                        int currentId = int.Parse(w.Attributes["Id"].Value);
                        if (maxId < currentId)
                            maxId = currentId;
                        this.dgvValues.Rows.Add(false,maxId, w.InnerText);
                        if (int.Parse(w.Attributes["isCurrent"].Value.ToString()) == 1)
                        {
                            this.dgvValues.CurrentCell = this.dgvValues.Rows[this.dgvValues.Rows.Count - 1].Cells["dgvValueColumn"];
                            this.dgvValues.Rows[this.dgvValues.Rows.Count - 1].Cells["dgvSelect"].Value = true;
                        }
                    }
                
                });
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.listString.Count > 0 && currentTemplateNode != null)
            {
                foreach(KeyValuePair<string,List<XmlNode>> pair in this.listString)
                {
                   XmlNode replaceNode = null;
                   XmlNodeList currentListNode = currentTemplateNode.SelectNodes("lable");
                   foreach(XmlNode tt in currentListNode)
                           if (tt.Attributes["Name"].Value.ToString().Equals(pair.Key.ToString()))
                           {
                               replaceNode = tt;
                           }
                   XmlElement addNode = doc.CreateElement("lable");
                   XmlAttribute attr = doc.CreateAttribute("Name");
                   attr.Value = replaceNode.Attributes["Name"].Value;
                   addNode.Attributes.SetNamedItem(attr);
                   int ih = 1;
                   foreach (XmlNode ttInstance in pair.Value)
                   {
                      XmlNode createNode = doc.CreateElement("value");
                      attr = doc.CreateAttribute("Id");
                      attr.Value = (ih++).ToString();
                      createNode.Attributes.SetNamedItem(attr);
                      attr = doc.CreateAttribute("isCurrent");
                      attr.Value = ttInstance.Attributes["isCurrent"].Value;
                      createNode.Attributes.SetNamedItem(attr);
                      createNode.InnerText = ttInstance.InnerText;
                      addNode.AppendChild(createNode);
                   }
                   currentTemplateNode.ReplaceChild(addNode, replaceNode);
                }
                doc.Save(dicPath);
            }
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnCacel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void dgvValues_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0||this.dgvValues.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                return;
            List<XmlNode> tempOut = new List<XmlNode>();
            bool result = this.listString.TryGetValue(this.cbxInpoutItem.Items[this.cbxInpoutItem.SelectedIndex].ToString(), out tempOut);
            if (!result)
                return;
            if (e.ColumnIndex == 2)
            {
               XmlNode findNode = tempOut.Find(w=>w.Attributes["Id"].Value.Equals( this.dgvValues.Rows[e.RowIndex].Cells["dgvId"].Value.ToString()));
                if (findNode != null)
                    findNode.InnerText = this.dgvValues.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
        }

        private void dgvValues_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0 || this.dgvValues.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                return;
            List<XmlNode> tempOut = new List<XmlNode>();
            bool result = this.listString.TryGetValue(this.cbxInpoutItem.Items[this.cbxInpoutItem.SelectedIndex].ToString(), out tempOut);
            if (!result)
                return;
            if (e.ColumnIndex == 0)
            {
                bool checkState = bool.Parse(this.dgvValues.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                if (checkState)
                    this.dgvValues.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                else
                    this.dgvValues.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                tempOut.ForEach(w =>
                {
                    if (w.Attributes["Id"].Value.Equals(this.dgvValues.Rows[e.RowIndex].Cells["dgvId"].Value.ToString()))
                    {
                        w.Attributes["isCurrent"].Value = (checkState == true)?"0":"1";
                    }
                    else
                        w.Attributes["isCurrent"].Value = "0";
                });
                foreach (DataGridViewRow ee in this.dgvValues.Rows)
                {
                    if (ee.Index != e.RowIndex)
                        ee.Cells[e.ColumnIndex].Value = false;
                }
            }
        }
    }
}
