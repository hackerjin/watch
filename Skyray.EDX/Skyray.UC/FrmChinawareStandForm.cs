using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chinaware;
namespace Skyray.UC
{
    public partial class FrmChinawareStandForm : Skyray.Language.MultipleForm
    {

        //private  List<Element> elementList = new List<Element>();

        //public  List<Element> ElementList
        //{
        //    get { return elementList; }
        //    set { elementList = value; }
        //}
        private EDXRFLibrary.ElementList elementList = EDXRFLibrary.ElementList.New;
        public EDXRFLibrary.ElementList ElementList
        {
            get { return elementList; }
            set { elementList = value; }
        }
        public string ChinawareInfo = string.Empty;
        public string Centainty = string.Empty;
        private List<Element> ChiawareElement = new List<Element>();
        public List<Skyray.EDXRFLibrary.HistoryElement> OutChiawareElements
        {
            get {
                List<Skyray.EDXRFLibrary.HistoryElement> lstelems = new List<Skyray.EDXRFLibrary.HistoryElement>();
                foreach (var em in ChiawareElement)
                {
                    Skyray.EDXRFLibrary.HistoryElement he = Skyray.EDXRFLibrary.HistoryElement.New;
                    he.elementName = em.Text;
                    he.contextelementValue = em.Content.ToString();
                    lstelems.Add(he);
                }
                return lstelems;
            }
        }

        public FrmChinawareStandForm()
        {
            InitializeComponent();
        }

        private void FrmChinawareStandForm_Shown(object sender, EventArgs e)
        {
            for (int j = 0; j < StandardLibrary.ElementList.Count;j++ )
            {
                StandardLibrary.ElementList[j].Enabled = false;
                //for (int i = 0; i < ElementList.Count;i++ )
                for (int i = 0; i < ElementList.Items.Count; i++)
                {
                    if (StandardLibrary.ElementList[j].Text.Equals(ElementList.Items[i].Caption))
                    {
                        StandardLibrary.ElementList[j].Content = ElementList.Items[i].Content;
                        ////test
                        //if (ElementList.Items[i].Caption.Equals("Pb")) StandardLibrary.ElementList[j].Content = 87.9439;
                        //else if (ElementList.Items[i].Caption.Equals("Sn")) StandardLibrary.ElementList[j].Content = 0.0839;
                        //else if (ElementList.Items[i].Caption.Equals("Zn")) StandardLibrary.ElementList[j].Content = 1.3437;
                        //else if (ElementList.Items[i].Caption.Equals("Cu")) StandardLibrary.ElementList[j].Content = 10.4144;
                        //else if (ElementList.Items[i].Caption.Equals("Ni")) StandardLibrary.ElementList[j].Content = 0.0231;
                        //else if (ElementList.Items[i].Caption.Equals("Fe")) StandardLibrary.ElementList[j].Content = 0.1910;
                        ////test
                        StandardLibrary.ElementList[j].Enabled = true;
                        break;
                    }
                }
            }
            double maxRow = DifferenceDevice.MaxCount;
            double Genuine = StandardLibrary.GenuineProbability();
            double StumerValue = DifferenceDevice.StumerValue;
            IOrderedEnumerable<KeyValuePair<string, double>> list = StandardLibrary.Analyse();

            if (Genuine < DifferenceDevice.StumerValue)
            {
                dataGridViewW.Rows.Add(new string[]{"赝品",((1-Genuine)*100).ToString("f2")});
                maxRow++;
            }
            foreach (KeyValuePair<string, double> pair in list)
            {
                if (dataGridViewW.Rows.Count < maxRow)
                {
                    bool flag=false;
                    for (int i = 0; i < dataGridViewW.Rows.Count; i++)
                    {
                        string name = dataGridViewW[0, i].Value.ToString();
                        if (name.Equals(pair.Key))
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        dataGridViewW.Rows.Add(new string[] { pair.Key, (pair.Value * 100).ToString("f2") });
                    }
                }
                else break;
            }
        }

        private void dataGridViewW_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ChiawareElement.Clear();
            ChinawareInfo = string.Empty;
            Centainty = string.Empty;
            if (dataGridViewW.SelectedRows.Count <= 0) return;
            string strID = dataGridViewW.SelectedRows[0].Cells[0].Value.ToString().Split(new char[] { ' ' })[0];
            ChinawareInfo = dataGridViewW.SelectedRows[0].Cells[0].Value.ToString();
            Centainty = dataGridViewW.SelectedRows[0].Cells[1].Value.ToString();
            ChiawareElement = StandardLibrary.GetChinawareElems(strID);
            this.DialogResult = DialogResult.OK;
        }


        




    }
}
