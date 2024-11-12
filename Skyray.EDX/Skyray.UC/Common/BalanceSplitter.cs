//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Windows.Forms;
//using Skyray.Controls;
//using Skyray.EDX.Common;

//namespace Skyray.UC
//{
//    public class BalanceSplitter
//    {
//        public InterfaceClass interfaceClass;

//        public ContainerObject container;

//        public FormWindowState currentWindowsState;

//        public List<SplitSetingObject> splitterDic;

//        //public MainForm mainForm;

//        public Dictionary<Control, List<MTSplitter>> allSplitter;

//        public void StartThread()
//        {
//            Thread thread = new Thread(StartMoveSplitter);
//            thread.Start();
//        }

//        private delegate void SplitterMove(MTSplitter splitter, SplitSetingObject splitterdf, Control control);
//        public void StartMoveSplitter()
//        {
//            if (this.container != null && (allSplitter == null || allSplitter.Count == 0))
//                allSplitter = FindAllSplitterFromForm(this.container);
//            if (allSplitter != null && allSplitter.Count > 0)
//            {
//                foreach (KeyValuePair<Control, List<MTSplitter>> pair in allSplitter)
//                {
//                    if (currentWindowsState == FormWindowState.Normal)
//                        AverageSplitPannelNormal(pair);
//                    else if (currentWindowsState == FormWindowState.Maximized)
//                        AverageSplitPannel(pair);
//                }
//            }
//        }

//        private void AverageSplitPannelNormal(KeyValuePair<Control, List<MTSplitter>> mtSplitter)
//        {
//            if (mtSplitter.Value == null || mtSplitter.Value.Count == 0)
//                return;
//            foreach (MTSplitter temp in mtSplitter.Value)
//            {
//                SplitSetingObject splitter = this.splitterDic.Find(w => w.SplitterName == temp.Name);
//                if (splitter != null)
//                    mainForm.Invoke(new SplitterMove(test), temp, splitter, mtSplitter.Key);
//            }
//        }

//        private void AverageSplitPannel(KeyValuePair<Control, List<MTSplitter>> mtSplitter)
//        {
//            if (mtSplitter.Value == null || mtSplitter.Value.Count == 0)
//                return;
//            foreach (MTSplitter temp in mtSplitter.Value)
//            {
//                if (temp.InvokeRequired)
//                {
//                    SplitSetingObject splitter = this.splitterDic.Find(w => w.SplitterName == temp.Name);
//                    if (splitter != null)
//                        mainForm.Invoke(new SplitterMove(test), temp, splitter, mtSplitter.Key);
//                }
//            }
//        }

//        private void test(MTSplitter splitter, SplitSetingObject splitterObj, Control control)
//        {
//            if (currentWindowsState == FormWindowState.Maximized)
//            {
//                splitter.SplitPosition = (int)(splitterObj.currentTarget * splitterObj.rate);
//                //if (DifferenceDevice.IsAnalyser)
//                //    splitter.Visible = false;
//            }
//            else if (currentWindowsState == FormWindowState.Normal)
//            {
//                splitter.SplitPosition = (int)(splitterObj.currentMinnum * splitterObj.minRate);
//                //if (DifferenceDevice.IsAnalyser)
//                //    splitter.Visible = false;
//            }
//        }

//        public Dictionary<Control, List<MTSplitter>> FindAllSplitterFromForm(Control clientPannel)
//        {
//            Dictionary<Control, List<MTSplitter>> mtList = new Dictionary<Control, List<MTSplitter>>();
//            if (clientPannel.Controls.Count == 0)
//                return null;
//            List<MTSplitter> mtOutput = new List<MTSplitter>();
//            foreach (Control control in clientPannel.Controls)
//            {
//                if (control is MTSplitter)
//                {
//                    if (mtList.ContainsKey(clientPannel))
//                    {
//                        mtList.TryGetValue(clientPannel, out mtOutput);
//                        mtOutput.Add((MTSplitter)control);
//                    }
//                    else
//                    {
//                        mtOutput.Add((MTSplitter)control);
//                        mtList.Add(clientPannel, mtOutput);
//                    }
//                }
//                else if (control.Controls.Count > 0)
//                {
//                    Dictionary<Control, List<MTSplitter>> tempSplitter = FindAllSplitterFromForm(control);
//                    if (tempSplitter != null && tempSplitter.Count > 0)
//                    {
//                        foreach (KeyValuePair<Control, List<MTSplitter>> pair in tempSplitter)
//                            mtList.Add(pair.Key, pair.Value);
//                    }
//                }
//            }
//            return mtList;
//        }
//    }
//}
