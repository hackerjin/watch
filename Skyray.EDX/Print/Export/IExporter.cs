using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.Print
{
    /// <summary>
    /// 数据导出接口
    /// </summary>
    public interface IExporter
    {
        //PageExporter PageExporter { get; set; }
        /// <summary>
        /// 导出方法
        /// </summary>
        void Export();
        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <returns></returns>
        string GetFileName();
    }
}
