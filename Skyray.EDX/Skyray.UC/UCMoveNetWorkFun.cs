using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.UC
{
    public partial class UCMoveNetWorkFun : Skyray.Language.UCMultiple
    {
        /// 定义多点事件
        /// </summary>
        public event Skyray.UC.EventDelegate.ReturnCameralState OnReturnCameralMultiPoint;

        /// <summary>
        /// 定义网格事件
        /// </summary>
        public event Skyray.UC.EventDelegate.ReturnCameralState OnReturnCameralNetWork;

        /// <summary>
        /// 定义固定步长事件
        /// </summary>
        //public event Skyray.UC.EventDelegate.ReturnCameralState OnFixedWalk;

        /// <summary>
        /// 定义单点事件
        /// </summary>
        public event Skyray.UC.EventDelegate.ReturnCameralState OnReturnCameralSinglePoint;

        /// <summary>
        /// 定义移动事件
        /// </summary>
        public event Skyray.UC.EventDelegate.ReturnCameralState OnReturnCameralMove;

        /// <summary>
        /// 定义校正事件
        /// </summary>
        public event Skyray.UC.EventDelegate.ReturnCameralState OnReturnCameralCheck;

        /// <summary>
        /// 定义开始事件
        /// </summary>
        public event Skyray.UC.EventDelegate.CameralOperation OnCameralStart;

        /// <summary>
        /// 定义停止事件
        /// </summary>
        public event Skyray.UC.EventDelegate.CameralOperation OnCameralStop;

        /// <summary>
        /// 定义复位事件
        /// </summary>
        public event Skyray.UC.EventDelegate.CameralOperation OnCameralReset;

        public UCMoveNetWorkFun()
        {
            InitializeComponent();
        }

        private void radioButtonMove_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMove.Checked)
                buttonWStart.Enabled = false;
            if (OnReturnCameralMove != null)
                OnReturnCameralMove(radioButtonMove.Checked);
        }

        private void radioButtonCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCheck.Checked)
                buttonWStart.Enabled = false;
            if (OnReturnCameralCheck != null)
                OnReturnCameralCheck(radioButtonCheck.Checked);
        }

        private void radioButtonSingle_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSingle.Checked)
                buttonWStart.Enabled = false;
            if (OnReturnCameralSinglePoint != null)
                OnReturnCameralSinglePoint(radioButtonSingle.Checked);
        }

        private void radioButtonMany_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMany.Checked)
                buttonWStart.Enabled = true;
            if (OnReturnCameralMultiPoint != null)
                OnReturnCameralMultiPoint(radioButtonMany.Checked);
        }

        private void radioButtonNetwork_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonNetwork.Checked)
                buttonWStart.Enabled = true;
            if (OnReturnCameralNetWork != null)
                OnReturnCameralNetWork(radioButtonNetwork.Checked);
        }

        private void buttonWStart_Click(object sender, EventArgs e)
        {
            if (OnCameralStart != null)
            {
                OnCameralStart();
                DifferenceDevice.IsConnect = true;
            }
        }

        private void buttonWReset_Click(object sender, EventArgs e)
        {
            if (OnCameralStop != null)
            {
                OnCameralStop();
                DifferenceDevice.IsConnect = false;
            }
        }

        private void buttonWStop_Click(object sender, EventArgs e)
        {
            if (OnCameralReset != null)
                OnCameralReset();
        }
    }
}
