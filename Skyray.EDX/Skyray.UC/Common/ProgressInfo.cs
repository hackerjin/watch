using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDX.Common.Component;
namespace Skyray.UC
{
    public class ProgressInfo
    {
        public ToolStripStatusLabel ToolStripStatusLabelMeasureTime { get; set; }
        public Label LabelMeasureTime { get; set; }
        private ToolStripStatusLabel toolStripStatusLabelSurplusTime;
        public ToolStripStatusLabel ToolStripStatusLabelSurplusTime
        { get { return toolStripStatusLabelSurplusTime;}
            set { toolStripStatusLabelSurplusTime = value;
            SetVisibleValue(toolStripStatusLabelSurplusTime, !IsAutoIncrease);
            }
        }
        private Label labelSurplusTime;
        public Label LabelSurplusTime
        { get { return labelSurplusTime;}
            set { labelSurplusTime = value ;
            SetVisibleValue(labelSurplusTime, !IsAutoIncrease);
            }
        }

        public ToolStripProgressBar ToolStripProgressBar { get; set; }
        public ProgressBar ProgressBar { get; set; }
        
        private object titleSurplusTime;
        /// <summary>
        /// 剩余时间标题控件
        /// </summary>
        public object TitleSurplusTime
        { get { return titleSurplusTime;}
            set {
                titleSurplusTime = value;
                SetVisibleValue(titleSurplusTime, !IsAutoIncrease);//如果为自增模式剩余时间标题控件不可见
            }
        }
        /// <summary>
        /// 测量时间标题控件
        /// </summary>
        public object TitleMeasureTime { get; set; }

        private void SetVisibleValue(object obj,bool flag)
        {
            if (obj == null) return;
            Type type = obj.GetType();
            System.Reflection.PropertyInfo visible = type.GetProperty("Visible");
            if (visible != null)
                visible.SetValue(obj, flag, null);
        }

        public static int increase =0;
        private Timer timer= new Timer();
        public bool IsAutoIncrease { get { return Skyray.EDX.Common.UIHelper.MeasureTimeType.AutoIncrease == WorkCurveHelper.MeasureTimeType; } }
        public string MeasureTime
        {
            set
            {
                if (IsAutoIncrease)//拦截其他赋值
                    value = increase.ToString()+"s";
                if (!value.IsNullOrEmpty())
                {
                    if (ToolStripStatusLabelMeasureTime != null)
                        ToolStripStatusLabelMeasureTime.Text = value;
                    if (LabelMeasureTime != null)
                        LabelMeasureTime.Text = value;
                }
            }
            get
            {
                if (ToolStripStatusLabelMeasureTime != null)
                    return ToolStripStatusLabelMeasureTime.Text;
                if (LabelMeasureTime != null)
                    return LabelMeasureTime.Text;
                return string.Empty;
            }
        }

        public string SurplusTime
        {
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    if (ToolStripStatusLabelSurplusTime != null)
                        ToolStripStatusLabelSurplusTime.Text = value;
                    if (LabelSurplusTime != null)
                        LabelSurplusTime.Text = value;
                }
            }
            get
            {
                if (ToolStripStatusLabelSurplusTime != null)
                    return ToolStripStatusLabelSurplusTime.Text;
                if (LabelSurplusTime != null)
                    return LabelSurplusTime.Text;
                return string.Empty;
            }
        }

        public int Value
        {
            set
            {                
                if (value >= 0)
                {
                    if (ToolStripProgressBar != null)
                        ToolStripProgressBar.Value = value;
                    if (ProgressBar != null)
                        ProgressBar.Value = value;
                }
            }
            get
            {
                if (ToolStripProgressBar != null)
                    return ToolStripProgressBar.Value;
                if (ProgressBar != null)
                    return ProgressBar.Value;
                return 0;
            }
        }


        public void AutoIncrease(bool isEnabled)
        {
            timer.Enabled = isEnabled;
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.State == DeviceState.Test
                || DifferenceDevice.interClassMain.deviceMeasure.interfacce.State == DeviceState.Init
                )
            {
                increase = int.Parse(DifferenceDevice.interClassMain.deviceMeasure.interfacce.usedTime.ToString());
                MeasureTime = increase.ToString();
            }
            else if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.State == DeviceState.PreHeat)
            {
                increase = int.Parse(DifferenceDevice.interClassMain.deviceMeasure.interfacce.PreHeatusedTime.ToString());
                MeasureTime = increase.ToString();
            }
            
        }

        public int Maximum
        {
            set
            {
                if (value >= 0)
                {
                    if (ToolStripProgressBar != null)
                        ToolStripProgressBar.Maximum = value;
                    if (ProgressBar != null)
                        ProgressBar.Maximum = value;
                }
            }
            get
            {
                if (ToolStripProgressBar != null)
                    return ToolStripProgressBar.Maximum;
                if (ProgressBar != null)
                    return ProgressBar.Maximum;
                return 0;
            }
        }

        public int Minimum
        {
            get
            {

                if (ToolStripProgressBar != null)
                    return ToolStripProgressBar.Minimum;
                if (ProgressBar != null)
                    return ProgressBar.Minimum;
                return 0;
            }
            set
            {
                if (value >= 0)
                {
                    if (ToolStripProgressBar != null)
                        ToolStripProgressBar.Minimum = value;
                    if (ProgressBar != null)
                        ProgressBar.Minimum = value;
                }
            }
        }

        public ProgressInfo()
        {
            timer.Tick +=new EventHandler(timer_Tick);
            timer.Interval = 1000;
        }
    }
}
