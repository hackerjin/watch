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
using Skyray.EDXRFLibrary;
using System.Threading;
 

namespace Skyray.UC
{
    public partial class UCMultiSet  : Skyray.Language.UCMultiple
    {

       

       // private List<MultiPoints> lstMultiPoints = new List<MultiPoints>();
        private List<SelectPointInfo> lstPoints = new List<SelectPointInfo>();
        private List<string> lstAllNames = new List<string>();
        private string path = Application.StartupPath + "\\MultiPoints";


        public UCMultiSet()
        {
            InitializeComponent();
            //RefreshAll();
        }

        private bool _isStartStep;
        public bool IsStartStep
        {
            get { return _isStartStep; }
            set { _isStartStep = value; }
        }


        private Point _currentPoint;
        public Point CurrentPoint
        {
            get { return _currentPoint; }
            set { _currentPoint = value; }
        }

        public void RefreshAll()
        {
            InitMultiNames();
            if (lstMultiNames.Items.Count > 0)
                InitMultiData(lstMultiNames.SelectedItem.ToString());
            
        }



        private void InitMultiData(string pointsName)
        {
            this.dgvMultiDatas.Rows.Clear();
            if (lstMultiNames.Items.Count == 0)
            {
                return;
            }
            for (int i = 0; i < lstPoints.Count; i++)
            {
                dgvMultiDatas.Rows.Add();
                dgvMultiDatas.Rows[i].Cells[0].Value = lstPoints[i].Number;
                dgvMultiDatas.Rows[i].Cells[1].Value = lstPoints[i].X;
                dgvMultiDatas.Rows[i].Cells[2].Value = lstPoints[i].Y;

            }

        }

        private void InitMultiNames()
        {
            lstMultiNames.Items.Clear();
            lstAllNames.Clear();
            DirectoryInfo dir = new DirectoryInfo(path);
            string hzm = ".xml";
            FileInfo[] fi = dir.GetFiles();
            if (fi.Length == 0)
            {
                return;
            }
            foreach (FileInfo f in fi)
            {
                if (Path.GetExtension(f.Name) == hzm)
                {
                    lstMultiNames.Items.Add(f.Name.Replace(hzm, ""));
                    lstAllNames.Add(f.Name.Replace(hzm, ""));
                }
            }
            if (lstMultiNames.Items.Count > 0)
                lstMultiNames.SelectedIndex = 0;
        }



        private void LoadPointsXML(string filename)
        {
            XmlDocument doc = new XmlDocument();
            string param = string.Empty;
            doc.Load(filename);
            XmlNode MultiPointsList = doc.SelectSingleNode("Camera");
            XmlNodeList MultiPointsNodes = MultiPointsList.SelectNodes("Point");
            lstPoints.Clear();
            for (int i = 0; i < MultiPointsNodes.Count; i++)
            {
                int x, y = 0;
                bool isx = int.TryParse(MultiPointsNodes[i].Attributes["X"].InnerText, out x);
                bool isy = int.TryParse(MultiPointsNodes[i].Attributes["Y"].InnerText, out y);
                SelectPointInfo sp = new SelectPointInfo();
                sp.Number = i + 1;
                sp.X = x;
                sp.Y = y;
                lstPoints.Add(sp);
            }
        }



        //private void LoadDB()
        //{
        //    string sql = "select * from MultiPoints group by Name";
        //    lstMultiPoints = MultiPoints.FindBySql(sql);
        //}


        //private void InitOncePoint(string pointName)
        //{
        //    this.dgvMultiDatas.Rows.Clear();
        //    this.dgvMultiDatas.DataSource = null;
        //    BindingSource bs = new BindingSource();
        //    // string sql = "select * from PureSpecParam where DeviceName ='" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "' and samplename ='" + elemName + "'";
        //    string sql = "select * from MultiPoints where  Name ='" + pointName + "'";
        //    List<MultiPoints> multiList = MultiPoints.FindBySql(sql);

        //    foreach (var temp in multiList)
        //    {
        //        bs.Add(temp);
        //    }
        //    this.dgvMultiDatas.AutoGenerateColumns = false;
        //    this.dgvMultiDatas.DataSource = bs;//绑定数据源

        //}
        bool first = false;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmNewPointsName fnew = new FrmNewPointsName();
            if (fnew.ShowDialog() == DialogResult.OK)
            {

                bool isContains = lstAllNames.Contains(fnew.Name);

                if (!isContains)
                {

                    first = true;
                   

                    lstMultiNames.Items.Add(fnew.Name);
                    lstAllNames.Add(fnew.Name);
                    lstPoints.Clear();
                    lstMultiNames.SelectedIndex = lstMultiNames.Items.Count - 1;
                    InitMultiData(fnew.Name);

                    dgvMultiDatas.Rows.Add();
                    dgvMultiDatas.Rows[0].Cells[0].Value = 1;
                    dgvMultiDatas.Rows[0].Cells[1].Value = 0;
                    dgvMultiDatas.Rows[0].Cells[2].Value = 0;

                   
                   
                   // InitMultiData(fnew.Name)

                    //var tempMulti = MultiPoints.New.Init(fnew.Name, 1, 0, 0);
                    //lstMultiPoints.Add(tempMulti);
                    //tempMulti.Save();
                    //InitOncePoint(fnew.Name);
                    WorkCurveHelper.MultiStartPosition = new Point(0, 0);
                }
                else
                {
                    Msg.Show(Info.ExistName);
                }

                if (first)
                {
                    Console.WriteLine("---------------new -------------");
                    
                    this.btnAddPoint.Visible = true;
                    
                    first = false;
                }
            }


            //FrmNewPointsName fnew = new FrmNewPointsName();
            //if (fnew.ShowDialog() == DialogResult.OK)
            //{
                
            //    MultiPoints cur = lstMultiPoints.Find(d => d.Name == fnew.Name);

            //    if (cur == null)
            //    {

            //        btnAddPoint.Enabled = true;
            //        btnReback.Enabled = true;
            //        lstMultiNames.Items.Add(fnew.Name);
            //        lstMultiNames.SelectedIndex = lstMultiNames.Items.Count - 1;
            //        var tempMulti = MultiPoints.New.Init(fnew.Name, 1, 0, 0);
            //        lstMultiPoints.Add(tempMulti);
            //        tempMulti.Save();
            //        InitOncePoint(fnew.Name);
            //        WorkCurveHelper.MultiStartPosition = new Point(0, 0);
            //    }
            //    else
            //    {
            //        Msg.Show(Info.ExistName);
            //    }
            //}
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

            if (lstMultiNames.Items.Count <= 0) return;
            DialogResult dr = Msg.Show(Info.DeleteSure, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.OK)
            {
                if (lstMultiNames.SelectedItem == null) return;
                string filename = path + "\\" + lstMultiNames.SelectedItem.ToString() + ".xml";
                lstAllNames.Remove(lstMultiNames.SelectedItem.ToString());
                lstMultiNames.Items.RemoveAt(lstMultiNames.SelectedIndex);
                if (lstMultiNames.Items.Count > 0)
                    lstMultiNames.SelectedIndex = 0;
              
                if (File.Exists(filename))
                    File.Delete(filename);

                //string delsql = "delete from MultiPoints where Name = '" + lstMultiNames.SelectedItem.ToString() + "';";
                //MultiPoints.FindBySql(delsql);
                //lstMultiPoints.RemoveAll(w => w.Name == lstMultiNames.SelectedItem.ToString());
                //dgvMultiDatas.Rows.Clear();
                //lstMultiNames.Items.RemoveAt(lstMultiNames.SelectedIndex);
                //if (lstMultiNames.Items.Count > 0)
                //{
                //    int selectindex = lstMultiNames.Items.Count - 1; ;
                //    lstMultiNames.SelectedIndex = selectindex;
                //    InitOncePoint(lstMultiNames.Items[selectindex].ToString());
                //}


            }

            //DialogResult dr = Msg.Show(Info.DeleteSure, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //if (dr == DialogResult.OK)
            //{
            //   // MultiPoints temp = MultiPoints.FindOne(w => w.Name == lstMultiNames.SelectedItem.ToString());
            //    string delsql = "delete from MultiPoints where Name = '" + lstMultiNames.SelectedItem.ToString() + "';";
            //    MultiPoints.FindBySql(delsql);
            //    lstMultiPoints.RemoveAll(w => w.Name == lstMultiNames.SelectedItem.ToString());
            //    dgvMultiDatas.Rows.Clear();
            //    lstMultiNames.Items.RemoveAt(lstMultiNames.SelectedIndex);
            //    if (lstMultiNames.Items.Count > 0)
            //    {
            //        int selectindex = lstMultiNames.Items.Count - 1; ;
            //        lstMultiNames.SelectedIndex = selectindex;
            //        InitOncePoint(lstMultiNames.Items[selectindex].ToString());
            //    }
               
                
            //}
        }


     

        //private void InitMultiNames()
        //{
        //    lstMultiNames.Items.Clear();
        //    if (lstMultiPoints.Count > 0)
        //    {
        //        foreach (MultiPoints m in lstMultiPoints)
        //        {
        //            lstMultiNames.Items.Add(m.Name);
        //        }
        //        lstMultiNames.SelectedIndex = 0;
        //    }
        //}

        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            //if (lstMultiNames.SelectedItem == null) return;
            //MultiPoints temppoint = MultiPoints.New.Init(lstMultiNames.SelectedItem.ToString(), dgvMultiDatas.Rows.Count + 1, WorkCurveHelper.MultiStartPosition.X, WorkCurveHelper.MultiStartPosition.Y);
            //temppoint.Save();
            //InitOncePoint(lstMultiNames.SelectedItem.ToString());
            int index = dgvMultiDatas.Rows.Count +1;
            dgvMultiDatas.Rows.Add(index, WorkCurveHelper.MultiStartPosition.X, WorkCurveHelper.MultiStartPosition.Y);
            
        }

        private void lstMultiNames_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void btnDelPoint_Click(object sender, EventArgs e)
        {
            //if (lstMultiNames == null || lstMultiNames.Items.Count <= 0 || dgvMultiDatas.Rows.Count<=0) return;
            //if (dgvMultiDatas.CurrentRow.Index >= 0)
            //{
            //   // MultiPoints.DeleteAll(w => w.Number == Convert.ToInt32(dgvMultiDatas[0, dgvMultiDatas.CurrentRow.Index].Value) );
            //   string delsql = "delete from MultiPoints where Number = " + Convert.ToInt32(dgvMultiDatas[0, dgvMultiDatas.CurrentRow.Index].Value) + " and Name = '" + lstMultiNames.SelectedItem.ToString() + "';";
            //   MultiPoints.FindBySql(delsql);
            //   InitOncePoint(lstMultiNames.SelectedItem.ToString());
            //}

            int index = dgvMultiDatas.CurrentRow.Index;
            dgvMultiDatas.Rows.RemoveAt(index);
           
        }

        private void lstMultiNames_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (!first)
            {
                Console.WriteLine("---------------select -------------");
                this.btnAddPoint.Visible = false;
                this.btnReback.Visible = false;
            }
            if (lstMultiNames.SelectedIndex > -1)
            {
                string str = lstMultiNames.SelectedItem.ToString();
                string filename = path + "\\" + str + ".xml";
                if (File.Exists(filename))
                {
                    LoadPointsXML(filename);
                    InitMultiData(filename);
                }
            }
            //btnAddPoint.Enabled = false;
            //btnReback.Enabled = false;
            //if (lstMultiNames.SelectedIndex > -1 && lstMultiNames.SelectedIndex < lstMultiPoints.Count)
            //    InitOncePoint(lstMultiNames.SelectedItem.ToString());
        }

        private void btnReback_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMultiDatas.Rows.Count <= 0) return;
                int x = Convert.ToInt32(dgvMultiDatas[1, dgvMultiDatas.Rows.Count - 1].Value);
                int y = Convert.ToInt32(dgvMultiDatas[2, dgvMultiDatas.Rows.Count - 1].Value);

                DifferenceDevice.interClassMain.RebackZero(new Point(-x, -y));
                btnReback.Visible = false;
                btnAddPoint.Visible = false;
                
            }
            catch
            { }

                   
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvMultiDatas.Rows.Count ; i++)
            {
                foreach (DataGridViewCell cell in dgvMultiDatas.Rows[i].Cells)
                {
                    if (cell.Value == null)
                    {
                        MessageBox.Show(this, "data can not be null", "Tips", MessageBoxButtons.OK);
                        return;
                    }
                }
            }
            try
            {
                lstPoints.Clear();
                foreach (DataGridViewRow row in dgvMultiDatas.Rows)
                {
                    if (row.Index < dgvMultiDatas.RowCount )
                    {
                        SelectPointInfo tempData = new SelectPointInfo();
                        tempData.Number = Convert.ToInt32(row.Cells[0].Value);
                        tempData.X = Convert.ToInt32(row.Cells[1].Value);
                        tempData.Y = Convert.ToInt32(row.Cells[2].Value);
                        lstPoints.Add(tempData);
                    }
                }
            }
            catch
            {
                MessageBox.Show(this, "Error data", "Error", MessageBoxButtons.OK);
                return;
            }

            if (lstMultiNames.SelectedItem != null)
            {
                if(btnAddPoint.Visible)
                this.btnReback.Visible = true;
                string filename = lstMultiNames.SelectedItem.ToString();
                SaveCurrentPoints(path + "\\" + filename + ".xml");
               
            }


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
                for (int i = 0; i < lstPoints.Count; i++)
                {
                    writer.WriteStartElement("Point");
                    writer.WriteAttributeString("X", (lstPoints[i]).X.ToString());
                    writer.WriteAttributeString("Y", (lstPoints[i]).Y.ToString());
                    writer.WriteEndElement();
                }
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

    }
}
