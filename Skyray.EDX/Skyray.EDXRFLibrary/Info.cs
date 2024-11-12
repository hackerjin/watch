using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDXRFLibrary
{
    public class LibraryInfo
    {
        public static string CAPIERR_MATRIXERROR = "曲线拟合错误！";
        public static string CAPIERR_NOREFELEMERROR = "没有拟合元素！";
        public static string CAPIERR_LESSSAMPLEERROR = "样品数量不够！";
        public static string CAPIERR_N0SAMPLEERROR = "未设置标准样品！";
        public static string CAPIERR_MAINELEMINTENSITYZEROERROR = "内标元素强度为0！";
        public static string CAPIERR_RESULTOVERLIMIT = "测量结果超出范围！";
        public static string CAPIERR_NOELEM = "没有测量元素！"; 
        public static string CAPIERR_OTHER = "无法计算！";
    }
}
