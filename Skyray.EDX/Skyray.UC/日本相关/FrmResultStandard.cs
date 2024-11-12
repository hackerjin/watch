using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common.Component;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using System.Xml;

namespace Skyray.UC
{
    public partial class FrmResultStandard : Skyray.Language.UCMultiple
    {
        StandardService standardModel;

        public FrmResultStandard()
        {
            InitializeComponent();
            standardModel = WorkCurveHelper.JapanStandard;
            LoadKaratParams();
            BindCtrToData();
            BindToCombox();
        }

        #region yuzhaozhu：private events

        private void LoadKaratParams()
        {
            WorkCurveHelper.JapanStandard.Clear();
            XmlNode node = ReportTemplateHelper.LoadSpecifiedNode("KaratCode", null);
            foreach (XmlNode nd in node.ChildNodes)
            {
                WorkCurveHelper.JapanStandard.AddOne(ResultStandard.ParseFromString(nd.InnerText));
            }
        }

        private void SaveKaratParams()
        {
            ReportTemplateHelper.ClearSpecifiedTag("KaratCode");
            BindingList<ResultStandard> temp = standardModel.StandardList;
            foreach (ResultStandard standard in temp)
                ReportTemplateHelper.CreateSpecifiedValue("KaratCode", "Item", standard.ParseToString());
        }

        private void BindCtrToData()
        {
            this.dgvStandard.DataSource = standardModel.StandardList;
        }

        private void BindToCombox()
        {
            var WorkCurveList = WorkCurve.FindBySql("select * from WorkCurve as a join Condition as b on a.Condition_Id = b.Id join Device as d on d.Id=b.Device_Id where a.Name !='智能模式' and d.Id =" + WorkCurveHelper.DeviceCurrent.Id);
            ComboxHelper ch = new ComboxHelper(this.dgvStandard, WorkCurveList); 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            standardModel.AddOne(ResultStandard.Default);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            standardModel.RemoveAt(dgvStandard.CurrentRow.Index);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            SaveKaratParams();
            EDXRFHelper.GotoMainPage(this);
        }
        private void btnApplication_Click(object sender, EventArgs e)
        {
            SaveKaratParams();
        }

        #endregion
    }

    /// <summary>
    /// Combox帮助类，实现DataGridView中所有Combox的处理
    /// </summary>
    public class ComboxHelper
    {
        ComboBox _cmbHelper;
        DataGridView _grd;
        List<WorkCurve> _bsCurve;

        public ComboxHelper(DataGridView grd, List<WorkCurve> bsCurve)
        {
            _bsCurve = bsCurve;
            _grd = grd;
            _cmbHelper = new ComboBox();
            _cmbHelper.SelectedValueChanged += new EventHandler(_cmbHelper_SelectedValueChanged);
            _cmbHelper.DropDownClosed += new EventHandler(_cmbHelper_DropDownClosed);
            _cmbHelper.Visible = false;
            _grd.Controls.Add(_cmbHelper);
            //_grd.CellBeginEdit += new DataGridViewCellCancelEventHandler(grd_CellBeginEdit);
            _grd.CellClick += new DataGridViewCellEventHandler(_grd_CellClick);
        }

        void _cmbHelper_SelectedValueChanged(object sender, EventArgs e)
        {
            _grd.CurrentCell.Value = _cmbHelper.SelectedValue;
            _cmbHelper.Visible = false;
        }

        void _cmbHelper_DropDownClosed(object sender, EventArgs e)
        {
            _cmbHelper.Visible = false;
        }

        void _grd_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                _cmbHelper.DataSource = null;
                _cmbHelper.DataSource = _bsCurve;
                _cmbHelper.DisplayMember = "Name";
                _cmbHelper.ValueMember = "Name";
                Rectangle rectangle = _grd.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _cmbHelper.SetBounds(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
                _cmbHelper.Visible = true;
                _cmbHelper.DroppedDown = true;
            }
            else if (e.ColumnIndex == 1)
            {
                WorkCurve workcurve = _bsCurve.Find(wc => wc.Name == _grd.CurrentRow.Cells[0].Value.ToString());
                if (workcurve == null || workcurve.ElementList == null)
                    return;
                _cmbHelper.DataSource = null;
                _cmbHelper.DataSource = workcurve.ElementList.Items.ToList<CurveElement>();
                _cmbHelper.DisplayMember = "Caption";
                _cmbHelper.ValueMember = "Caption";
                Rectangle rectangle = _grd.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _cmbHelper.SetBounds(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
                _cmbHelper.Visible = true;
                _cmbHelper.DroppedDown = true;
            }
        }


        void grd_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                _cmbHelper.DataSource = null;
                _cmbHelper.DataSource = _bsCurve;
                _cmbHelper.DisplayMember = "Name";
                _cmbHelper.ValueMember = "Name";
                Rectangle rectangle = _grd.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _cmbHelper.SetBounds(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
                _cmbHelper.Visible = true;
                _cmbHelper.DroppedDown = true;
            }
            else if (e.ColumnIndex == 1)
            {
                WorkCurve workcurve = _bsCurve.Find(wc => wc.Name == _grd.CurrentRow.Cells[0].Value.ToString());
                if (workcurve == null || workcurve.ElementList == null)
                    return;
                _cmbHelper.DataSource = null;
                _cmbHelper.DataSource = workcurve.ElementList.Items.ToList<CurveElement>();
                _cmbHelper.DisplayMember = "Caption";
                _cmbHelper.ValueMember = "Caption";
                Rectangle rectangle = _grd.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _cmbHelper.SetBounds(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
                _cmbHelper.Visible = true;
                _cmbHelper.DroppedDown = true;
            }
        }

    }
}
