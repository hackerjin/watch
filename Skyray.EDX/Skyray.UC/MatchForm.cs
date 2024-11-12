/********************************************************************
	created:	2009/11/06
	created:	6:11:2009   9:33
	filename: 	D:\work\xfP2.0\xFP2.0\XRF\MatchForm.cs
	file path:	D:\work\xfP2.0\xFP2.0\XRF
	file base:	MatchForm
	file ext:	cs
	author:		
	
	purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using System.Linq;

namespace Skyray.UC
{
    /// <summary>
    /// ƥ�乤������ѡ��
    /// </summary>
    public partial class MatchForm : Skyray.Language.MultipleForm
    {
        /// <summary>
        /// ƥ������
        /// </summary>
        public WorkCurve SelectedCurve;

        public double MatchLevel;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="infos"></param>
        public MatchForm(MatchEntity[] infos)
        {
            InitializeComponent();//��ʼ��
            List<MatchEntity> tempEntity = infos.OrderByDescending(w=>w.Matching).ToList();
            for (int i = 0; i < tempEntity.Count; i++)
            {
                //ƥ����Ϣ�����
                dgvwMatch.Rows.Add(new string[] { tempEntity[i].workCurve.Id.ToString(), tempEntity[i].workCurve.Name, tempEntity[i].Matching.ToString("f1") + "%" });
            }
        }

        public MatchForm(List<WorkCurve> listCurve)
        {
            InitializeComponent();//��ʼ��
            for (int i = 0; i < listCurve.Count; i++)
            {
                //ƥ����Ϣ�����
                dgvwMatch.Rows.Add(new string[] { listCurve[i].Id.ToString(), listCurve[i].Name,"0.0000" });
            }
        }
        /// <summary>
        /// ѡ��ָ���Ĺ������ߴ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (dgvwMatch.SelectedRows.Count > 0)
            {
                int index = dgvwMatch.SelectedRows[0].Index;
                long id = long.Parse(dgvwMatch[0, index].Value.ToString());
                //string curveName = dataGridViewW1[1, index].Value.ToString();//ѡ��������
                SelectedCurve = WorkCurve.FindById(id);
                MatchLevel = double.Parse(dgvwMatch[2, index].Value.ToString().Replace("%",""));
            }
            //EDXRFHelper.GotoMainPage(this);
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// ȡ����ť������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}