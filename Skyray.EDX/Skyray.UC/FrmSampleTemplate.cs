using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    public partial class FrmSampleTemplate : UCMultiple
    {
        private DataTable SampleTemplateDT;
        private CustomStandard _currentStandard;
        private ElementList elementList;
        public FrmSampleTemplate(CustomStandard currentStandard, ElementList elementList)
        {
            InitializeComponent();
            this._currentStandard = currentStandard;
            this.elementList = elementList;
            Load += new EventHandler(FrmSampleTemplate_Load);
        }

        void FrmSampleTemplate_Load(object sender, EventArgs e)
        {

            this.dgvwStandardDatas.DataSource = GetTemplateData(_currentStandard);
            this.dgvwStandardDatas.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgvwStandardDatas_DataBindingComplete);

        }

        void dgvwStandardDatas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 1; i < dgvwStandardDatas.Columns.Count; i++)
            {
                DataGridViewCell cell = dgvwStandardDatas[i, 4];
                if (cell.Value.ToString() == "OK")
                    cell.Style.ForeColor = Color.Green;
                else if (cell.Value.ToString() == "NG")
                    cell.Style.ForeColor = Color.Red;
            }
        }



        private DataTable GetTemplateData(CustomStandard customStandard)
        {
            DataTable dt = new DataTable("template");
            DataColumn column = new DataColumn("元素");
            column.Caption = "Element";
            dt.Columns.Add(column);
            foreach (StandardData data in customStandard.StandardDatas)
            {
                Atom atom = Atoms.GetAtom(data.ElementCaption);
                string name = atom != null ? atom.AtomNameCN+"("+atom.AtomName+")" : data.ElementCaption;
                DataColumn dc = new DataColumn(name);
                dc.Caption = data.ElementCaption;
                dt.Columns.Add(dc);
            }
            
            DataRow row = dt.NewRow();
            row[0] = "标准件含量(ppm)";
            for(int i = 1; i < dt.Columns.Count;i++)
                row[i] = customStandard.StandardDatas[i-1].StandardContent;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row[0] = "实际测试含量(ppm)";
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                CurveElement element = elementList.Items.FirstOrDefault<CurveElement>(ce => ce.Caption.Equals(dt.Columns[i].Caption));
                if (element != null)
                    row[i] = element.Content * 10000;  //注意单位ppm
                else
                    row[i] = "/";
            }
            dt.Rows.Add(row);

            row = dt.NewRow();
            row[0] = "标准偏差(20%)(ppm)";
            for (int i = 1; i < dt.Columns.Count; i++)
                row[i] = customStandard.StandardDatas[i - 1].StandardContent*0.2;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row[0] = "实际偏差(ppm)";
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                CurveElement element = elementList.Items.FirstOrDefault<CurveElement>(ce => ce.Caption.Equals(dt.Columns[i].Caption));
                if (element != null)
                {
                    row[i] = element.Content * 10000 - customStandard.StandardDatas[i - 1].StandardContent;  //注意单位
                }
                else
                    row[i] = "/";
            }
            dt.Rows.Add(row);

            row = dt.NewRow();
            row[0] = "判断";
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                CurveElement element = elementList.Items.FirstOrDefault<CurveElement>(ce => ce.Caption.Equals(dt.Columns[i].Caption));
                if (element != null)
                {
                    if (Math.Abs(element.Content * 10000 - customStandard.StandardDatas[i - 1].StandardContent) < customStandard.StandardDatas[i - 1].StandardContent * 0.2)
                    {
                        row[i] = "OK";
                    }
                    else
                        row[i] = "NG";
                }
                else
                    row[i] = "?";
            }
            dt.Rows.Add(row);

            return dt;
        }

    }
}
