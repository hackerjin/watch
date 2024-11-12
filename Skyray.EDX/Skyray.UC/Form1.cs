using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Lephone.Data.Common;
using Skyray.Controls;
using Skyray.EDX.Common;
using ZedGraph;
using Skyray.Controls.Extension;
using System.Linq;
using System.IO;
using System.Security.AccessControl;
using System.Collections;
using System.Data;
using System.Data.SQLite;
using Lephone.Data;

namespace Skyray.UC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public DataSet ds_SpecList = null;

        public List<string> selectList = new List<string>();

        public int ROWS_PER_PAGE = 56;
        public int PAGE = 0; 
        private void GetSample()
        {
            this.listViewWSpecList.Items.Clear();
            ds_SpecList = GetDataSet("select id,name from SpecList order by Id   limit " + ROWS_PER_PAGE + "  offset " + ROWS_PER_PAGE * PAGE + ";select count(*) from SpecList");
            DataTable dt_SpecList = ds_SpecList.Tables[0];
            hScrollBar1.Maximum = int.Parse(ds_SpecList.Tables[1].Rows[0][0].ToString());
            foreach (DataRow row in dt_SpecList.Rows)
            {
                this.listViewWSpecList.Items.Add(row["name"].ToString());
                this.listViewWSpecList.Items[this.listViewWSpecList.Items.Count - 1].Name = row["id"].ToString();

                if (selectList.Exists(delegate(string v) { return v == row["name"].ToString(); }))
                    this.listViewWSpecList.Items[row["name"].ToString()].Selected = true;
            }
        }


        private DataTable GetData(string strSql)
        {
            DataTable dt = new DataTable();
            string connectionString = DbEntry.Context.Driver.ConnectionString;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(strSql, connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        private DataSet GetDataSet(string strSql)
        {
            DataSet ds = new DataSet();
            string connectionString = DbEntry.Context.Driver.ConnectionString;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(strSql, connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    adapter.Fill(ds);
                }
            }
            return ds;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            GetSample();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i=0;i<=int.Parse(txtAddColmunName.Text);i++)
                Lephone.Data.DbEntry.Context.ExecuteNonQuery("insert into speclist(workcurveId,Name,SampleName,SpecType,Color,virtualColor,condition_Id) values('sadgadgadgdasgd_" + i.ToString("D4") + "','erewtrewteqwt_" + i.ToString("D4") + "','qewtedsagdasgdagadsg_" + i.ToString("D4") + "',0,	-16776961,	-2987746,	48) ");
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            //if (hScrollBar1.Value == null) return;
            PAGE = (int.Parse(hScrollBar1.Value.ToString()) / ROWS_PER_PAGE);
            GetSample();
        }

        private void listViewWSpecList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                if (!selectList.Exists(delegate(string v) { return v == e.Item.Name.ToString(); }))
                    selectList.Add(e.Item.Name.ToString());
                
            }
        }
    }
}
