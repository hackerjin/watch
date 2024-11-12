using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Skyray.Language;

namespace Skyray.UC
{
    public partial class UCWorkRegion :UCMultiple
    {
        public static List<Auto> AutoDic = new List<Auto>();
        public UCWorkRegion()
        {
            InitializeComponent();
          
        }

        public event EventHandler OnClickWorkCurve;


       
        public bool Enable
        {
            set { this.dataViewWorkregion.Enabled = value; }
        }


        public string WorkName
        {
            set 
            {
                foreach (DataGridViewRow row in this.dataViewWorkregion.Rows)
                {
                    row.Selected = false;
                    if (row.Cells["ColumnWorkRegion"].Value != null && row.Cells["ColumnWorkRegion"].Value.ToString().Equals(value))
                            row.Selected = true;
                }

            }
        }

        public bool IsReLoad
        {
            set
            {
                if (value)
                {
                    this.dataViewWorkregion.Rows.Clear();
                    LoadWorkRegion();
                    if (this.dataViewWorkregion.RowCount > 0)
                    {
                        this.dataViewWorkregion.Rows[0].Selected = true;
                        WorkCurveHelper.CurrentWorkRegion = WorkRegion.FindOne(w => w.Caption == this.dataViewWorkregion.Rows[0].Cells["ColumnWorkRegion"].Value.ToString());
                    }
                }
            }
        }

        public void LoadWorkRegion()
        {
            List<WorkRegion> regionList = WorkRegion.FindAll();
            this.dataViewWorkregion.Rows.Clear();
            foreach (WorkRegion s in regionList)
            {
                this.dataViewWorkregion.Rows.Add(s.Caption);
                //Auto atom = AutoDic.Find(delegate(Auto k) { return k.Key == s.Name; });
                //if (atom == null)
                //    AutoDic.Add(s.Name,);
            }
            WorkRegion regionWork = regionList.Find(w => w.IsDefaultWorkRegion);
            if (regionWork!=null)
            {
                LoadDefaultCurve(regionWork);
            }
            
        }

        void Model_LanguageChanged(object sender, EventArgs e)
        {
            WorkCurveHelper.CurrentWorkRegion = WorkRegion.FindById(WorkCurveHelper.CurrentWorkRegion.Id);
            LoadWorkRegion();
        }

        private void dataViewWorkregion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string celllValue = this.dataViewWorkregion.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            WorkRegion regionWork = WorkRegion.FindOne(w => w.Caption == celllValue);
            if (regionWork != null)
            {
                WorkCurveHelper.CurrentWorkRegion = regionWork;
                LoadDefaultCurve(regionWork);
            }
            if (OnClickWorkCurve != null)
                OnClickWorkCurve(null, null);
        }

        private void LoadDefaultCurve(WorkRegion regionWork)
        {
            if (WorkCurveHelper.CurrentWorkRegion != null)
            {
                regionWork = WorkCurveHelper.CurrentWorkRegion;
                this.WorkName = WorkCurveHelper.CurrentWorkRegion.Caption;
            }
            var deviceId = WorkCurveHelper.DeviceCurrent.Id;
            WorkCurve openCurve = null;
            string sql = "select * from WorkCurve where Condition_Id in (select Id from condition where Device_id = "
                   + deviceId + ") and FuncType=" + (int)WorkCurveHelper.DeviceFunctype + " and WorkRegion_Id=" + (int)regionWork.Id + " and IsDefaultWorkCurve=1";
            List<WorkCurve> workCurve = WorkCurve.FindBySql(sql);
            if (workCurve != null && workCurve.Count > 0)
            {
                openCurve = workCurve[0];
                //if (WorkCurveHelper.WorkCurveCurrent == null || !workCurve.Contains(WorkCurveHelper.WorkCurveCurrent))
                if (WorkCurveHelper.WorkCurveCurrent == null || openCurve.Id != WorkCurveHelper.WorkCurveCurrent.Id)
                {
                    
                    WorkCurveHelper.WorkCurveCurrent = workCurve[0];
                    DifferenceDevice.MediumAccess.OpenCurveSubmit();
                    if (DifferenceDevice.interClassMain.specList != null && DifferenceDevice.interClassMain.specList.Specs!=null && DifferenceDevice.interClassMain.specList.Specs.Length > 0)

                    {
                        DifferenceDevice.interClassMain.currentTestTimes = 1;
                        DifferenceDevice.interClassMain.deviceParamSelectIndex = 0;
                        List<DeviceParameter> listParams = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.ToList();
                        if (WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
                            DifferenceDevice.interClassMain.DeviceParameterByElementList(listParams);
                        else
                            DifferenceDevice.interClassMain.deviceParamsList = listParams;

                        if (DifferenceDevice.interClassMain.deviceParamsList.Count == 0)
                        {
                            Msg.Show(Info.MeasureConditionInvalidate);
                        }
                        //DifferenceDevice.irohs.MatchWorkCurve(ref matter, ref id,1);
                        DifferenceDevice.MediumAccess.AddIntrestedElements();
                    }
                }
                
            }
            else
            {
                WorkCurveHelper.WorkCurveCurrent = null;
                DifferenceDevice.interClassMain.refreshFillinof.ContructMeasureRefreshData(1, null);
                DifferenceDevice.interClassMain.refreshFillinof.CreateContructStatis(null);
                EDXRFHelper.DisplayWorkCurveControls();
            }
            //DifferenceDevice.MediumAccess.AddIntrestedElements();
        }

        private void UCWorkRegion_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;
            Skyray.Language.Lang.Model.LanguageChanged += new EventHandler(Model_LanguageChanged);
        }

    }
}
