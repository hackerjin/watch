using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.Camera
{
    public class SelectPointInfo
    {
        public int Number { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

    }
    public class CameraInfo
    {
        /// <summary>
        /// 提示
        /// </summary>
        public static string NoCamera = "未找到摄像头！";

        /// <summary>
        /// 提示
        /// </summary>
        public static string MessageBoxCaptionTip = "提示";

        /// 摄像头未打开或参数设置错误
        /// </summary>
        public static string MessageBoxTextCameraHasProblem = "摄像头出错，请检查！";
        /// <summary>
        /// 摄像头未打开
        /// </summary>
        public static string MessageBoxTextCameraNotOpened = "摄像头未打开，请检查！";
        /// <summary>
        /// 确定要修改十字中心位置吗
        /// </summary>
        public static string MessageBoxTextCameraIsSureChangeFocus = "确定要修改十字中心位置吗？";

        public static string SpotShapeRectangle = "矩   形";

        public static string SpotShapeEllipse = "椭   圆";

        public static string CamerFileExist = "同名文件已存在,需要覆盖吗？";
    }
}
