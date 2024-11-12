using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using Skyray.EDX.Common;

namespace Skyray.Camera
{

   

    /// <summary>
    /// 窗体类：
    /// </summary>
    public partial class FrmEditMultiPoints : Form
    {


        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();


        private DataGridViewTextBoxEditingControl EditCell;
        #region 属性
       
        private List<int> heights = new List<int>();
        private string path = null;
        private string mode = "";
        #endregion



        private SkyrayCamera camera = null;

        private bool isInit = true;

        #region 构造器

        /// <summary>
        /// 默认实例构造器
        /// </summary>
        public FrmEditMultiPoints(String Mode)
        {
            InitializeComponent();

           
            this.lblmultiName.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblmultiName") ? SkyrayCamera.SkyrayCameraLangDic["lblmultiName"] : "多点名称：";
            this.btnDel.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnDel") ? SkyrayCamera.SkyrayCameraLangDic["btnDel"] : "删除";
            
            this.btnDeletePoint.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnDeletePoint") ? SkyrayCamera.SkyrayCameraLangDic["btnDeletePoint"] : "删除点";
            this.btnSave.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnSave") ? SkyrayCamera.SkyrayCameraLangDic["btnSave"] : "保存";
            this.btnAccept.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnAccept") ? SkyrayCamera.SkyrayCameraLangDic["btnAccept"] : "保存";


            this.mode = Mode;

            if (Mode == "Multiple")
            {
                path = Application.StartupPath + "\\MultiPoints\\";
                this.colZ.Visible = true;
            }
            else if (Mode == "dotMatrix")
                path = Application.StartupPath + "\\Matrix\\" + "\\dotMatrix\\";
            else if (Mode == "matrixDot")
                path = Application.StartupPath + "\\Matrix\\" + "\\matrixDot\\";
            else if (Mode == "dotDot")
                path = Application.StartupPath + "\\Matrix\\" + "\\dotDot\\";
        }

        #endregion
        private void FrmEditMultiPoints_Load(object sender, EventArgs e)
        {

            camera = (SkyrayCamera)WorkCurveHelper.camera;
            Point temp = camera.PointToScreen(Point.Empty);
            temp.Y += camera.Size.Height;
            this.Location = temp;

            InitMultiNames();


           
        }

        private void InitMultiNames()
        {
            cboMultiNames.Items.Clear();
           
            DirectoryInfo dir = new DirectoryInfo(path);
            string hzm=".xml";
            FileInfo[] fi = dir.GetFiles();
            

            foreach (FileInfo f in fi)
            {
                if (Path.GetExtension(f.Name) == hzm)
                    cboMultiNames.Items.Add(f.Name.Replace(hzm, ""));
            }
            if (this.camera.alTempTestPoints.Count > 0)
            {
                if (this.camera.curMultiName == string.Empty)
                {
                    cboMultiNames.Items.Add("编辑中");
                    for (int i = 0; i < cboMultiNames.Items.Count; i++)
                    {
                        if (cboMultiNames.Items[i].ToString() == "编辑中")
                        {
                            cboMultiNames.SelectedItem = cboMultiNames.Items[i];
                        }

                    }
                    

                }
                else
                {
                    for (int i = 0; i < cboMultiNames.Items.Count; i++)
                    {
                        if (cboMultiNames.Items[i].ToString() == this.camera.curMultiName.Split(new char[] { '.' })[0])
                        {
                            cboMultiNames.SelectedItem = cboMultiNames.Items[i];
                        }

                    }

                }
            }
            else
            {
                if (cboMultiNames.Items.Count > 0)
                    cboMultiNames.SelectedIndex = 0;
            }
        }




        private void cboMultiNames_SelectedIndexChanged(object sender, EventArgs e)
        {

            string str = this.cboMultiNames.SelectedItem.ToString().Split(new char[] { '.' })[0];
            string filename = path + "\\" + str + ".xml";
            
            System.Reflection.MethodInfo myMethod = WorkCurveHelper.thickType.GetMethod("isTesting");
            bool isTesting =  (bool)myMethod.Invoke(WorkCurveHelper.curThick, null);
            if (!isTesting && !WorkCurveHelper.testDemoing && !filename.Contains("编辑中")) 
            {
                this.camera.curMultiName = str;
                LoadPoints(filename);
                this.camera.currentMultiName = str + ".xml";

            }
            InitMultiData();         
            
        }

     


        private void LoadPoints(string filename)
        {


            XmlDocument doc = new XmlDocument();
            string param = string.Empty;
            doc.Load(filename);
            XmlNode MultiPointsList = doc.SelectSingleNode("Camera");
            XmlNodeList MultiPointsNodes = MultiPointsList.SelectNodes("Point");

            
            this.camera.alContiTestPoints.Clear();
            this.camera.alTempTestPoints.Clear();
            this.camera.alContiTestHeights.Clear();


            for (int i = 0; i < MultiPointsNodes.Count; i++)
            {
                int x, y,z = 0;
                bool isx = int.TryParse(MultiPointsNodes[i].Attributes["X"].InnerText, out x);
                bool isy = int.TryParse(MultiPointsNodes[i].Attributes["Y"].InnerText, out y);


               

                this.camera.alContiTestPoints.Add(new Point(x,y));


                if (this.mode == "Multiple")
                {
                    int.TryParse(MultiPointsNodes[i].Attributes["Z"].InnerText, out z);
                    this.camera.alContiTestHeights.Add(z);

                }          
            }
            //将alTemp与alConti进行同步
            this.camera.alTempTestPoints = (System.Collections.ArrayList)this.camera.alContiTestPoints.Clone();

        }

        public void updateLargeViewNow()
        {

            System.Reflection.MethodInfo myMethod1 = WorkCurveHelper.ucCameraType.GetMethod("updateLargeViewNow");
            myMethod1.Invoke(WorkCurveHelper.ucCamera, null);
        }
        public void InitMultiData()
        {
            if (cboMultiNames.Items.Count == 0)
            {
                return;
            }
            this.dgvMultiDatas.Rows.Clear();

            for (int i = 0; i < this.camera.alTempTestPoints.Count; i++)
            {
                dgvMultiDatas.Rows.Add();
                dgvMultiDatas.Rows[i].Cells[0].Value = i + 1;
                dgvMultiDatas.Rows[i].Cells[1].Value = WorkCurveHelper.TabWidth -  ((Point)this.camera.alTempTestPoints[i]).X/WorkCurveHelper.XCoeff;
                dgvMultiDatas.Rows[i].Cells[2].Value = WorkCurveHelper.TabHeight -  ((Point)this.camera.alTempTestPoints[i]).Y/WorkCurveHelper.YCoeff ;
                if (this.mode == "Multiple")
                    dgvMultiDatas.Rows[i].Cells[3].Value = this.camera.alContiTestHeights[i]/WorkCurveHelper.ZCoeff;
                else
                    dgvMultiDatas.Rows[i].Cells[3].Value = 0;
            }


            if (!isInit)
            {
                System.Reflection.MethodInfo myMethod = WorkCurveHelper.thickType.GetMethod("goBack");
                myMethod.Invoke(WorkCurveHelper.curThick, null);
            }
            else
                isInit = false;

            new System.Threading.Thread(new System.Threading.ThreadStart(updateLargeViewNow)).Start();

            
        }


        /// <summary>
        /// 保存多点
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="srcPoint"></param>
        public void SaveCurrentPoints(string fileName)
        {
           
            try
            {
                XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.Unicode);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("Camera");

                for (int i = 0; i < this.camera.alTempTestPoints.Count; i++)
                {
                    writer.WriteStartElement("Point");
                    writer.WriteAttributeString("X", ((Point)this.camera.alTempTestPoints[i]).X.ToString());
                    writer.WriteAttributeString("Y", ((Point)this.camera.alTempTestPoints[i]).Y.ToString());

                    
                    if (this.mode == "Multiple")
                    {
                        writer.WriteAttributeString("Z",this.camera.alContiTestHeights[i].ToString());                 
                    }

                    writer.WriteEndElement();
                }
                writer.WriteStartElement("Steps");
                writer.WriteAttributeString("X", WorkCurveHelper.xSteps.ToString());
                writer.WriteAttributeString("Y", WorkCurveHelper.ySteps.ToString());
                writer.WriteEndElement();
                
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();

                    
            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message);
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        
            foreach (DataGridViewRow row in dgvMultiDatas.Rows)
            {
                if (row.Index == dgvMultiDatas.Rows.Count - 1)
                    continue;
                foreach (DataGridViewCell cell in row.Cells)
                {
                     
                    if (cell.Value == null)
                    {
                        MessageBox.Show(this, "数据不可为空！","Tips", MessageBoxButtons.OK);
                        return;
                    }
                }
            }

            try
            {
                
                foreach (DataGridViewRow row in dgvMultiDatas.Rows)
                {
                    if (row.Index == dgvMultiDatas.Rows.Count - 1)
                        continue;
                    foreach (DataGridViewCell cell in row.Cells)
                    {             
                       
                        Convert.ToInt32(cell.Value);

                    }
                }
            }
            catch
            {
                MessageBox.Show(this, "数据错误！", "Error", MessageBoxButtons.OK);
                return;
            }


           
             FrmNewPointsName fnew = new FrmNewPointsName();
             string fileName = "";
             if(this.cboMultiNames.SelectedItem.ToString() == "编辑中")
                fnew.Name =  string.Empty;
             else
                fnew.Name =  this.cboMultiNames.SelectedItem.ToString();
             fileName = fnew.Name;

             if (fnew.ShowDialog() == DialogResult.OK)
             {
                 if (fnew.Name != string.Empty)
                 {
                     if(fnew.Name == fileName)
                        SaveCurrentPoints(path + "\\" + fnew.Name + ".xml");
                     else
                     {
                         if (System.IO.File.Exists(path + "\\" + fnew.Name + ".xml"))
                         {
                             if (MessageBox.Show(this,
                                                   CameraInfo.CamerFileExist,
                                                   "Infomation",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Information) == DialogResult.No)
                             {
                                 return;
                             }
                             SaveCurrentPoints(path + "\\" + fnew.Name + ".xml");
                         }
                         else
                         {
                             SaveCurrentPoints(path + "\\" + fnew.Name + ".xml");
                             cboMultiNames.Items.Add(fnew.Name);
                         }
                     }
                 }
                 

             }
          
            

            System.Reflection.MethodInfo myMethod = WorkCurveHelper.thickType.GetMethod("goBack");
            myMethod.Invoke(WorkCurveHelper.curThick, null);
            
        }

      


        private void dgvMultiDatas_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            EditCell = (DataGridViewTextBoxEditingControl)e.Control;
            EditCell.SelectAll();
            EditCell.KeyPress += new KeyPressEventHandler(EditCell_KeyPress);
        }


        void EditCell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dgvMultiDatas.CurrentCell.EditedFormattedValue != null)
            {
                if (dgvMultiDatas.CurrentCell.EditedFormattedValue.ToString().Contains("-"))
                {
                    if (e.KeyChar == '-')
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
            if (Char.IsNumber(e.KeyChar) || (Keys)e.KeyChar == Keys.Back || e.KeyChar == (char)45)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            this.camera.fed = null;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.camera.fed = null;
            this.Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboMultiNames.Items.Count <= 0) return;

                if (MessageBox.Show("delete ?", "tips", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (cboMultiNames.SelectedItem == null) return;
                    string filename = path + "\\" + cboMultiNames.SelectedItem.ToString() + ".xml";
                    cboMultiNames.Items.RemoveAt(cboMultiNames.SelectedIndex);
                    if (cboMultiNames.Items.Count > 0)
                    {
                        isInit = true;
                        cboMultiNames.SelectedIndex = 0;
                    }

                    if (File.Exists(filename))
                        File.Delete(filename);
                }
            }
            catch
            { }
        }




        private void btnDeletePoint_Click(object sender, EventArgs e)
        {



            //由于现在采用绝对坐标，所以第一个点也可以删除，但是不可以删除多点文件中的最后一个点
            if (dgvMultiDatas != null && dgvMultiDatas.Rows.Count > 2)
            {
                int index = dgvMultiDatas.CurrentRow.Index;
                dgvMultiDatas.Rows.RemoveAt(index);

                this.camera.alContiTestPoints.RemoveAt(index);
                this.camera.alTempTestPoints = (System.Collections.ArrayList)this.camera.alContiTestPoints.Clone();
                this.camera.alContiTestHeights.RemoveAt(index);
                
            }
        }

        private void btnAddPoint_Click(object sender, EventArgs e)
        {


            //由于现在采用绝对坐标，所以第一个点也可以删除，但是不可以删除多点文件中的最后一个点
            if (dgvMultiDatas != null)
            {
                int index = dgvMultiDatas.CurrentRow.Index;
                DataGridViewRow addRow = (DataGridViewRow)dgvMultiDatas.Rows[0].Clone();
                addRow.Cells[0].Value = "--";
                dgvMultiDatas.Rows.Insert(index+1, addRow);
            }

        }

        private void FrmEditMultiPoints_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.camera.fed = null;
        }



        private void dgvMultiDatas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 &&  e.ColumnIndex == 0 && e.RowIndex < this.dgvMultiDatas.RowCount-1)
            {
                System.Reflection.MethodInfo myMethod = WorkCurveHelper.thickType.GetMethod("gotoTestPoint");
                myMethod.Invoke(WorkCurveHelper.curThick, new object[] { this.camera.alTempTestPoints[e.RowIndex], this.camera.alContiTestHeights[e.RowIndex] });

                
            }
        }

  
      

  
    }


}