using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Skyray.EDX.Common;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Skyray.Controls;

namespace Skyray.UC
{
    public partial class UIConfig : Skyray.Language.UCMultiple
    {
        public static BindingSource bs = new BindingSource();
        public static List<ContainerObject > lst = new List<ContainerObject>();
        public static Skyray.Controls.Style style = new Skyray.Controls.Style();
        public UIConfig()
        {
            InitializeComponent();
        }

        private void dataGridViewWContainObjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string itemName = string.Empty;
            if (dataGridViewWContainObjects.CurrentCell != null && this.dataGridViewWContainObjects.CurrentCell.Value!=null)
                itemName = this.dataGridViewWContainObjects.CurrentCell.Value.ToString();

            List<ContainerObject> objList = lst.FindAll(w => w.Name1 == itemName);

            ContainerObject obj = lst.Find(w => w.Name1 == itemName);
            this.chkVisible.Checked = obj.Visible;


            if (obj.Name == "containerObjectSpec" || obj.Name == "containerObjectTubeStatus"
                    || obj.Name == "containerObjectCurve")
            {
                grb_ChildControl.Controls.Clear();
                grb_ChildControl.Visible = true;
                GetChildControl(obj.Name);
                
            }
            else grb_ChildControl.Visible = false;
        }


        #region 控制工作曲线、设备、谱线信息
        private void GetChildControl(string sContainerObjectName)
        {
            switch (sContainerObjectName)
            {
                case "containerObjectCurve":
                    AddCheckBoxW("WorkingCurve", Info.WorkingCurve, grb_ChildControl, GetContainerObjects(sContainerObjectName, "WorkingCurve"));
                    AddCheckBoxW("CurveType", Info.CurveType, grb_ChildControl, GetContainerObjects(sContainerObjectName, "CurveType"));
                    AddCheckBoxW("ConditionName", Info.ConditionName, grb_ChildControl, GetContainerObjects(sContainerObjectName, "ConditionName"));
                    if (WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasTarget && WorkCurveHelper.DeviceCurrent.Target != null && WorkCurveHelper.DeviceCurrent.Target.Count > 0)
                    {
                        AddCheckBoxW("TargetIndex", Info.TargetIndex, grb_ChildControl, GetContainerObjects(sContainerObjectName, "TargetIndex"));
                        AddCheckBoxW("TargetMode", Info.TargetMode, grb_ChildControl, GetContainerObjects(sContainerObjectName, "TargetMode"));
                        AddCheckBoxW("TubCurrentRatio", Info.TubCurrentRatio, grb_ChildControl, GetContainerObjects(sContainerObjectName, "TubCurrentRatio"));
                    }
                    break;
                case "containerObjectSpec":
                    AddCheckBoxW("SampleName", Info.SampleName, grb_ChildControl, GetContainerObjects(sContainerObjectName, "SampleName"));
                    AddCheckBoxW("SpecDate", Info.SpecDate, grb_ChildControl, GetContainerObjects(sContainerObjectName, "SpecDate"));
                    AddCheckBoxW("MeasureTime", Info.MeasureTime, grb_ChildControl, GetContainerObjects(sContainerObjectName, "MeasureTime"));
                    AddCheckBoxW("Voltage", Info.Voltage, grb_ChildControl, GetContainerObjects(sContainerObjectName, "Voltage"));
                    AddCheckBoxW("Current", Info.Current, grb_ChildControl, GetContainerObjects(sContainerObjectName, "Current"));
                    AddCheckBoxW("Filter", Info.Filter, grb_ChildControl, GetContainerObjects(sContainerObjectName, "Filter"));
                    AddCheckBoxW("Collimator", Info.Collimator, grb_ChildControl, GetContainerObjects(sContainerObjectName, "Collimator"));
                    break;
                case "containerObjectTubeStatus":
                    AddCheckBoxW("CountRate", Info.CountRate, grb_ChildControl, GetContainerObjects(sContainerObjectName, "CountRate"));
                    AddCheckBoxW("Voltage", Info.Voltage, grb_ChildControl, GetContainerObjects(sContainerObjectName, "Voltage"));
                    AddCheckBoxW("Current", Info.Current, grb_ChildControl, GetContainerObjects(sContainerObjectName, "Current"));
                    AddCheckBoxW("PeakChannel", Info.PeakChannel, grb_ChildControl, GetContainerObjects(sContainerObjectName, "PeakChannel"));
                    AddCheckBoxW("VacuumDegree", Info.VacuumDegree, grb_ChildControl, GetContainerObjects(sContainerObjectName, "VacuumDegree"));
                    AddCheckBoxW("Gain", Info.Gain, grb_ChildControl, GetContainerObjects(sContainerObjectName, "Gain"));
                    AddCheckBoxW("FineGain", Info.FineGain, grb_ChildControl, GetContainerObjects(sContainerObjectName, "FineGain"));
                    break;
            }
            
        }

        private bool GetContainerObjects(string sContainerObjectName, string sContainerObjectChildName)
        {
            bool isVisible=false;
            ContainerObjects objs = lContainerObjectsChild.Find(a => a.Name == sContainerObjectName);
            ContainerObjects bojschild = (objs != null && objs.ChildControl != null) ? objs.ChildControl.Find(a => a.Name == sContainerObjectChildName) : null;

            return isVisible = (bojschild == null) ? false : bojschild.Visible;
        }

        private void AddCheckBoxW(string id,string name,Control control,bool isCheck)
        {
            int x = 4;
            int y = 18;
            y = control.Controls.Count*y + y;
            CheckBoxW chbw = new CheckBoxW();
            chbw.Name = id;
            chbw.Text = name;
            chbw.Checked = isCheck;
            chbw.Location = new System.Drawing.Point(x, y);
            control.Controls.Add(chbw);
        }


        public static List<ContainerObjects> lContainerObjectsChild = new List<ContainerObjects>();
        private void SetChildControl(TabPage tabpage, ContainerObject obj)
        {
            
            if (tabpage.Controls[0].Controls[0].GetType() == typeof(DataGridViewW))
            {
                ((DataGridViewW)tabpage.Controls[0].Controls[0]).Rows.Clear();
                ContainerObjects objs = lContainerObjectsChild.Find(a => a.Name == obj.Name);
                if (objs == null)
                {
                    objs = new ContainerObjects();
                    objs.Name = obj.Name;
                    objs.Visible = obj.Visible;
                    objs.Style = style;
                    objs.Dock = obj.Dock;
                    lContainerObjectsChild.Add(objs);
                }

                List<ContainerObjects> lChileContainerObjects = new List<ContainerObjects>();
                int i = 0;
                string rowName = string.Empty;
                if (obj.Name == "containerObjectSpec") rowName = "ColumnSpecLabel";
                else if (obj.Name == "containerObjectTubeStatus") rowName = "ColumnDeviceLabel";
                else if (obj.Name == "containerObjectCurve") rowName = "ColumnCurveLabel";

                int iCount = 0;
                int iControlCount = 0;
                foreach (Control conc in grb_ChildControl.Controls)
                    if (conc.GetType() == typeof(CheckBoxW))
                    {
                        iControlCount++;
                        if (((CheckBoxW)conc).Checked) iCount++;
                    }

                ((DataGridViewW)tabpage.Controls[0].Controls[0]).RowCount = (iCount == 0) ? iControlCount : iCount;
                if (iCount == 0)
                {
                    foreach (Control conc in grb_ChildControl.Controls)
                        if (conc.GetType() == typeof(CheckBoxW))
                        {
                            ((DataGridViewW)tabpage.Controls[0].Controls[0]).Rows[i].Cells[rowName].Value = conc.Text;
                            i++;
                        }
                }

                i = 0;
                foreach (Control conc in grb_ChildControl.Controls)
                    if (conc.GetType() == typeof(CheckBoxW))
                    {
                        ContainerObjects objsChild = (objs.ChildControl == null) ? null : objs.ChildControl.Find(a => a.Name == conc.Name);
                        if (objsChild == null)
                        {
                            objsChild = new ContainerObjects();
                            objsChild.Name = conc.Name;
                            objsChild.Visible = ((CheckBoxW)conc).Checked;
                        }
                        else objsChild.Visible = ((CheckBoxW)conc).Checked;
                        if (((CheckBoxW)conc).Checked) {
                            ((DataGridViewW)tabpage.Controls[0].Controls[0]).Rows[i].Cells[rowName].Value = conc.Text;
                            i++; }

                        
                        lChileContainerObjects.Add(objsChild);

                    }
                objs.ChildControl = lChileContainerObjects;
            }

                
        }


        private void LoadChildControl()
        {
            List<ContainerObjects> ContainerObjectslist = GetContainerObjects();

            ContainerObjects obj = ContainerObjectslist.Find(w => w.Name == "containerObjectSpec");
            if (obj!=null && obj.ChildControl != null)
                lContainerObjectsChild.Add(obj);

            obj = null;
            obj = ContainerObjectslist.Find(w => w.Name == "containerObjectTubeStatus");
            if (obj != null && obj.ChildControl != null)
                lContainerObjectsChild.Add(obj);

            obj = null;
            obj = ContainerObjectslist.Find(w => w.Name == "containerObjectCurve");
            if (obj != null && obj.ChildControl != null)
                lContainerObjectsChild.Add(obj);
        }

        private List<ContainerObjects> GetContainerObjects()
        {
            string path = Application.StartupPath + "\\frm3000.txt";
            if (File.Exists(path))
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                using (System.IO.Stream _FileStream = new System.IO.FileStream(path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None
                    ))
                {
                    _FileStream.Position = 0;
                    _FileStream.Seek(0, SeekOrigin.Begin);
                    return (List<ContainerObjects>)formatter.Deserialize(_FileStream);
                }
            }
            else
                return null;
        }


        #endregion

        private void UIConfig_Load(object sender, EventArgs e)
        {
            if (DifferenceDevice.IsAnalyser != true)
                return;
            this.dataGridViewWContainObjects.AutoGenerateColumns = false;
            //this.dataGridViewWContainObjects.AutoGenerateColumns = true;
            this.dataGridViewWContainObjects.DataSource = bs;
            LoadChildControl();
            dataGridViewWContainObjects_CellClick(null, null);
            SetStyle(this,style);
        }

        private void SetStyle(Control control ,Skyray.Controls.Style style)
        {
            if (control.GetType().GetInterface("ISkyrayStyle") != null)
            {
                (control as ISkyrayStyle).SetStyle(style);
            }
            if (control.Controls != null)
                foreach (Control ctrl in control.Controls)
                    SetStyle(ctrl,style);            
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            string itemName = string.Empty;
            if (dataGridViewWContainObjects.CurrentCell == null || this.dataGridViewWContainObjects.CurrentCell.Value == null)
                return;
             itemName = this.dataGridViewWContainObjects.CurrentCell.Value.ToString();
            ContainerObject obj = lst.Find(w=>w.Name1==itemName);
                obj.Visible = chkVisible.Checked;
                ((Control)obj).Visible = chkVisible.Checked;
                if (obj.Name == "containerObjectCameral" || obj.Name == "containerObjectGraph")
                {
                    ContainerObject container = lst.Find(w => w.Name == "containerObjectGraphAndSample");
                    if (container == null || container.Visible == false) return;
                    TabControlW dd = container.Controls[0] as TabControlW;//obj.Parent.Parent as TabControlW;
                    if (dd!=null && dd.TabPages.IndexOf(obj.Parent as TabPage) != -1 && !chkVisible.Checked)
                    dd.TabPages.Remove(obj.Parent as TabPage);
                    else if (dd != null && dd.TabPages.IndexOf(obj.Parent as TabPage) == -1 && chkVisible.Checked)
                        dd.TabPages.Add(obj.Parent as TabPage);                        
                   
                }
                if (obj.Name == "containerObjectSpec" || obj.Name == "containerObjectTubeStatus"
                    || obj.Name == "containerObjectCurve")
                {
                    ContainerObject container = lst.Find(w => w.Name == "containerObjectSpecAndTubeStatus");
                    if (container == null || container.Visible == false) return;
                    TabControlW dd = container.Controls[0] as TabControlW;//obj.Parent.Parent as TabControlW;
                    if (dd != null && dd.TabPages.IndexOf(obj.Parent as TabPage) != -1 && !chkVisible.Checked)
                        dd.TabPages.Remove(obj.Parent as TabPage);
                    else if (dd != null && dd.TabPages.IndexOf(obj.Parent as TabPage) == -1 && chkVisible.Checked)
                        dd.TabPages.Add(obj.Parent as TabPage);
                    if (container.Visible == true && dd.TabPages.Count == 0)
                    {
                        ((Control)container).Visible = false;
                        container.Visible = false;
                    }

                    SetChildControl(obj.Parent as TabPage, obj);
                }
                Control containerObjectButtons = lst.Find(w => w.Name == "containerObjectButtons");
                Control containerObjectDrop = lst.Find(w => w.Name == "containerObjectDrop");
                if (containerObjectButtons.Visible == true && containerObjectDrop.Visible == true)
                {
                    containerObjectButtons.Size =new Size(112,112);
                    containerObjectDrop.Size = new Size(112, 112);
                    containerObjectButtons.Dock = DockStyle.Left;
                    containerObjectDrop.Dock = DockStyle.Fill;
                }
                else if (containerObjectButtons.Visible == true && containerObjectDrop.Visible == false)
                {
                    containerObjectButtons.Dock = DockStyle.Fill;
                }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveUIVisibleAndStyle();
            this.ParentForm.Close();
        }
        public static void SaveUIVisibleAndStyle()
        {
            List<ContainerObjects> lstContainer = new List<ContainerObjects>();
            ContainerObjects objs;
            foreach (ContainerObject obj in lst)
            {
                objs = new ContainerObjects();
                objs.Name = obj.Name;
                objs.Visible = obj.Visible;
                objs.Style = style;
                objs.Dock = obj.Dock;

                ContainerObjects childContainerObjects = lContainerObjectsChild.Find(a => a.Name == obj.Name);
                if (childContainerObjects != null) objs.ChildControl = childContainerObjects.ChildControl;

                lstContainer.Add(objs);
            }
            IFormatter formatter = new BinaryFormatter();
            string path = Application.StartupPath + "\\frm3000.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, lstContainer);
            stream.Close();
        }
    }
    [Serializable]
    public class ContainerObjects
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private bool visible;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        private Skyray.Controls.Style style;

        public Skyray.Controls.Style Style
        {
            get { return style; }
            set { style = value; }
        }
        private System.Windows.Forms.DockStyle dock;

        public System.Windows.Forms.DockStyle Dock
        {
            get { return dock; }
            set { dock = value; }
        }

        private List<ContainerObjects> childControl;

        public List<ContainerObjects> ChildControl
        {
            get { return childControl; }
            set { childControl = value; }
        }

    }
}
