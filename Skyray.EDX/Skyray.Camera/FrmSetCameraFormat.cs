using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AForge.Video.DirectShow;

namespace Skyray.Camera
{
    public delegate void DoCameraOp(int openValue);
    public delegate void ChangeFrameSizeOp(Size operValue);
    public partial class FrmSetCameraFormat :Form
    {
        public Int32 CaptureSize = (Int32)Skyray.Camera.SkyrayCamera.CaptureFormat.Larger;
        public DoCameraOp EditVideoIndex = null;
        public DoCameraOp EditBmpSize = null;
        public ChangeFrameSizeOp EditResolution = null;
   
        private string[] _VideoNames=new string[0];
        public Int32 CurrentVideoIndex = 0;
        public Size CapabilityFrameSize ;
        private bool flag = false;
        FilterInfoCollection videoDevices = null;
        public SkyrayCamera camera = null;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="sc"></param>
        public FrmSetCameraFormat(SkyrayCamera sc)
        {
            InitializeComponent();
            this.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiCameraFormat") ? SkyrayCamera.SkyrayCameraLangDic["tsmiCameraFormat"] : "视频格式";
            this.label1.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblCameraSize") ? SkyrayCamera.SkyrayCameraLangDic["lblCameraSize"] : "像 素";
            this.lblVideoDev.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblVideoDev") ? SkyrayCamera.SkyrayCameraLangDic["lblVideoDev"] : "视频设备";

            videoDevices = sc.videoDevices;
            //string[] ds = Enum.GetNames(typeof(Skyray.Camera.SkyrayCamera.CaptureFormat));
            string[] ds = Enum.GetNames(typeof(Skyray.Camera.SkyrayCamera.CaptureFormat));
                foreach (string str in ds)
                    this.cboCameraFormats.Items.Add(str);
           
            CaptureSize = (Int32)sc.Format;
            _VideoNames = sc.GetVideoNames();
            if(!sc.largeCameraSelect)
                CurrentVideoIndex = sc.VideoIndex;
            else
                CurrentVideoIndex = int.Parse(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "Camera.xml", "Camera/LargeCameraCaptureIndex")) - 1;

            this.camera = sc;
            //foreach (VideoCapabilities vcb in sc.videoSourceDevice.VideoCapabilities)
            //{
            //    if (vcb.FrameSize.Width * vcb.FrameSize.Height > SkyrayCamera.MaxResolution) continue; //像素超过2000000的不给选择
            //    cboVideoCapabilities.Items.Add(vcb.FrameSize);
            //}
            //if (cboVideoCapabilities.Items.Count > 0)
            //{
            //    CapabilityFrameSize = sc.videoSourceDevice.VideoResolution.FrameSize;
            //    cboVideoCapabilities.SelectedItem = CapabilityFrameSize;
                
            //}
            //this.cboCameraFormats.Enabled = sc.IsAdminUse!="false";
            //this.btnAcceptdefault.Enabled = sc.IsAdminUse!="false";


            //sc.
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            //CaptureSize = (Int32)(Skyray.Camera.SkyrayCamera.CaptureFormat)(Enum.Parse(typeof(Skyray.Camera.SkyrayCamera.CaptureFormat), 
            //    cboCameraFormats.SelectedItem.ToString(), false));
            CaptureSize = (Int32)Skyray.Camera.SkyrayCamera.CaptureFormat.Larger;
            cboCameraFormats.Text = "Larger";
           // camera.SetCaptureFormat(CaptureSize);
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSetCameraFormat_Load(object sender, EventArgs e)
        {
            cboCameraFormats.SelectedItem = ((Skyray.Camera.SkyrayCamera.CaptureFormat)CaptureSize).ToString();
            flag = true;
            if (_VideoNames != null)
            {
                cboVideoDev.Items.AddRange(_VideoNames);
                if(CurrentVideoIndex >= 0)
                    cboVideoDev.SelectedItem = _VideoNames[CurrentVideoIndex];
            }
            
        }
       
        /// <summary>
        /// 改变视频大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboCameraFormats_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (flag)
            {
                CaptureSize = (Int32)(Skyray.Camera.SkyrayCamera.CaptureFormat)(Enum.Parse(typeof(Skyray.Camera.SkyrayCamera.CaptureFormat),
                    cboCameraFormats.SelectedItem.ToString(), false));
                //camera.SetCaptureFormat(CaptureSize);
            }
        }

        private void cboVideoDev_SelectedValueChanged(object sender, EventArgs e)
        {
            if (flag)
            {
                this.camera.VideoIndex = cboVideoDev.SelectedIndex;
                
                VideoCaptureDevice vcd = new VideoCaptureDevice(videoDevices[cboVideoDev.SelectedIndex].MonikerString);
                cboVideoCapabilities.Items.Clear();
                cboVideoCapabilities.SelectedItem=null;
                if (EditVideoIndex != null) EditVideoIndex(cboVideoDev.SelectedIndex);

                if (vcd.VideoCapabilities.Length > 0)
                {
                    foreach (VideoCapabilities vcb in vcd.VideoCapabilities)
                    {
                        if (vcb.FrameSize.Width * vcb.FrameSize.Height > SkyrayCamera.MaxResolution) continue; //像素超过2000000的不给选择
                        cboVideoCapabilities.Items.Add(vcb.FrameSize);
                    }

                    if (EditVideoIndex != null) EditVideoIndex(cboVideoDev.SelectedIndex);
                    if (cboVideoCapabilities.Items.Count > 0)
                    {
                        if (vcd.VideoResolution == null) CapabilityFrameSize = vcd.VideoCapabilities[0].FrameSize;
                        else CapabilityFrameSize = vcd.VideoResolution.FrameSize;
                        cboVideoCapabilities.SelectedItem = CapabilityFrameSize;

                    }
                }
                else
                {
                    cboVideoCapabilities.Items.Add(new Size(480,360));
                    cboVideoCapabilities.Items.Add(new Size(640, 480));

                }
                
            }
        }

        private void cboVideoCapabilities_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboVideoCapabilities.Items.Count > 0)
            {
                CapabilityFrameSize = (Size)cboVideoCapabilities.SelectedItem;
                if (EditResolution != null) EditResolution(CapabilityFrameSize);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

       
    }
}
