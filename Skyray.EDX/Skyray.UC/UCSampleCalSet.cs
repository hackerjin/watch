using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCSampleCalSet : Skyray.Language.UCMultiple
    {
        private string StrFilePath = string.Empty;
        private CSampleCal m_SampleCal = new CSampleCal();
        DataTable dtblCal = new DataTable();
        public UCSampleCalSet()
        {
            InitializeComponent();
        }
        public UCSampleCalSet(string strFilename):this()
        {
            StrFilePath = strFilename;
            
            dtblCal.Columns.Add(Info.ElementName);
            dtblCal.Columns.Add(Info.Content);
            dtblCal.Columns.Add(Info.strError);
            bool bLoad=m_SampleCal.LoadDatasFromFile(strFilename);
            if (!bLoad)
            {
                m_SampleCal.ListSampleDatas = new List<CSampleCalData>();
                m_SampleCal.ControlSampleName = string.Empty;
                
            }
            txtNewStandard.Text = m_SampleCal.ControlSampleName;
            foreach (CSampleCalData cscd in m_SampleCal.ListSampleDatas)
            {
                object[] row ={
                                  cscd.ElemCaption,
                                  cscd.ElemContent,
                                  cscd.Error
                               };
                dtblCal.Rows.Add(row);
            }
       
            dgvwStandardDatas.DataSource = dtblCal;
            dgvwStandardDatas.Columns[Info.ElementName].ReadOnly = true;
            dgvwStandardDatas.Columns[Info.Content].ReadOnly = false;
            dgvwStandardDatas.Columns[Info.strError].ReadOnly = false;
        }

        private void btnSelectElement_Click(object sender, EventArgs e)
        {

            ElementTableAtom table = new ElementTableAtom();

            string[] strs = new string[dtblCal.Rows.Count];
            for (int i = 0; i < dtblCal.Rows.Count; i++)
            {
                strs[i] = dtblCal.Rows[i][Info.ElementName].ToString();
            }

            table.MultiSelect = true;
            table.SelectedItems = strs;

            WorkCurveHelper.OpenUC(table, false, Info.SelectElement, true);//打开元素周期表


            if (table.SelectedItems != null && table.SelectedItems.Length > 0)
            {
                var atomnames = table.SelectedItems;
                bool hasElement = false;
                for (int i = 0; i < atomnames.Length; i++)//遍历新数组
                {
                    hasElement = false;

                    for (int j = 0; j < m_SampleCal.ListSampleDatas.Count; j++)
                    {
                        if (m_SampleCal.ListSampleDatas[j].ElemCaption.Equals(atomnames[i]))
                        {
                            hasElement = true;
                            break;
                        }
                    }
                    if (!hasElement)
                    {
                        m_SampleCal.ListSampleDatas.Add(new CSampleCalData(atomnames[i],0,0));
                        object[] row={
                                      atomnames[i],
                                      0,
                                      0
                                   };
                        dtblCal.Rows.Add(row);
                    }
                }
                for (int i = m_SampleCal.ListSampleDatas.Count - 1; i >= 0; i--)//遍历增加后的元素列表
                {
                    hasElement = false;
                    string ElemName = m_SampleCal.ListSampleDatas[i].ElemCaption;
                    for (int j = 0; j < atomnames.Length; j++)
                    {
                        if (atomnames[j].Equals(ElemName))
                        {
                            hasElement = true;
                            break;
                        }
                    }
                    if (!hasElement)
                    {
                        m_SampleCal.ListSampleDatas.RemoveAt(i);
                        for (int k = dgvwStandardDatas.Rows.Count; k >= 0; k--)
                        {
                            if (dtblCal.Rows[k][Info.ElementName].ToString() == ElemName)
                            {
                                dtblCal.Rows.RemoveAt(k);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                m_SampleCal.ListSampleDatas.Clear();
                dtblCal.Rows.Clear();
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int k = 0;k<dgvwStandardDatas.Rows.Count; k++)
            {
                string ElemName = dgvwStandardDatas.Rows[k].Cells[Info.ElementName].Value.ToString();
                float elemCont = Convert.ToSingle(dgvwStandardDatas.Rows[k].Cells[Info.Content].Value);
                float elemerr = Convert.ToSingle(dgvwStandardDatas.Rows[k].Cells[Info.strError].Value);
                for (int j = 0; j < m_SampleCal.ListSampleDatas.Count; j++)
                {
                    if (m_SampleCal.ListSampleDatas[j].ElemCaption.Equals(ElemName))
                    {
                        m_SampleCal.ListSampleDatas[j].ElemContent=elemCont;
                        m_SampleCal.ListSampleDatas[j].Error = elemerr;
                        break;
                    }
                }
            }
            m_SampleCal.ControlSampleName = txtNewStandard.Text.Trim();
            m_SampleCal.SaveDatasToFile(StrFilePath);
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = DialogResult.OK;
            this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = DialogResult.Cancel;
            this.dialogResult = DialogResult.Cancel;
            EDXRFHelper.GotoMainPage(this);
        }
    }
}
