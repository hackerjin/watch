using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.Print
{
    public class PrintInfo
    {
        public static string TemplateTooLong = "模板超出纸张长度！";

        public static string SystemVersion = "打印模板V2.02测试版";
        /// <summary>
        /// 模板文件
        /// </summary>
        public static string TemplateFile = "模板文件";
        /// <summary>
        /// Part
        /// </summary>
        public static string Part = "Part ";
        /// <summary>
        /// 列
        /// </summary>
        public static string ContinueCol = "列";
        /// <summary>
        /// 表
        /// </summary>
        public static string ContinueTable = "表";
        /// <summary>
        /// 置于顶层
        /// </summary>
        public static string BringToFront = "置于顶层";
        /// <summary>
        /// 置于底层
        /// </summary>
        public static string BringToBottom = "置于底层";
        /// <summary>
        /// 模板无数据
        /// </summary>
        public static string DeleteClickCtrl = "删除选择";

        /// <summary>
        /// 模板无数据
        /// </summary>
        public static string PageProperty = "页面属性";

        public static string InsertTable = "插入表格";

        /// <summary>
        /// 模板无数据
        /// </summary>
        public static string GroupErr = "模板分组错误！";

        public static string TopInsertRow = "向上方插入行";

        public static string BottomInsertRow = "向下方插入行";

        public static string MergeCell = "合并单元格";

        public static string DeleteRow = "删除当前行";

        public static  string LeftInsertColumn = "向左插入列";
        public static string RightInsertColumn = "向右插入列";
        public static string DeleteCurrentColumn = "删除当前列";
        public static string SpecifiedDataSource = "指定数据源";

        /// <summary>
        /// 模板无数据
        /// </summary>
        public static string TemplateNoData = "模板无数据！";

        /// <summary>
        /// 未连接打印机
        /// </summary>
        public static string NoPrinter = "未连接打印机！";
        /// <summary>
        /// 报表设计
        /// </summary>
        public static string DesignPage = "报表设计";

        /// <summary>
        /// 预览
        /// </summary>
        //public static string PagePriview = "预览";

        /// <summary>
        /// 数据加载中
        /// </summary>
        //public static string DataLoading = "数据加载中...";

        /// <summary>
        /// 删除
        /// </summary>
        public static string Delete = "删除";

        /// <summary>
        /// 添加
        /// </summary>
        public static string Add = "添加子节点";

        /// <summary>
        /// 确认删除
        /// </summary>
        public static string ConfirmDel = "确认删除吗？";
        /// <summary>
        /// 添加根节点
        /// </summary>
        public static string AddRoot = "添加根节点";

        /// <summary>
        /// 编辑节点属性
        /// </summary>
        public static string EditTag = "编辑节点属性";

        /// <summary>
        /// 保存失败
        /// </summary>
        public static string SaveFailed = "保存失败！";
        /// <summary>
        /// 保存成功
        /// </summary>
        public static string SaveSuccess = "保存成功！";

        /// <summary>
        /// 标签
        /// </summary>
        public static string Label = "标签";
        /// <summary>
        /// 字段
        /// </summary>
        public static string Field = "字段";
        /// <summary>
        /// 图形
        /// </summary>
        public static string Image = "图形";
        /// <summary>
        /// 表格
        /// </summary>
        public static string Grid = "表格";

        public static string CompositeTable = "复合表格";

        /// <summary>
        /// 模板
        /// </summary>
        public static string Template = "模板";
        /// <summary>
        /// 属性
        /// </summary>
        public static string Property = "属性";
        /// <summary>
        /// 数据源
        /// </summary>
        public static string Source = "数据源";

        public static string OpenNow = "导出成功，是否打开查看";

        //修改：何晓明 2011-01-12
        //原因：覆盖正打开的Excel时为系统提示
        /// <summary>
        /// 提示
        /// </summary>
        public static string Tip = "提示";
        /// <summary>
        /// 文件正被使用
        /// </summary>
        public static string FileUsed = "文件正被其他程序所占用";
        //
        //修改：何晓明 2011-01-13
        //原因：定制保存提示信息
        /// <summary>
        /// 文件
        /// </summary>
        public static string File = "文件";
        /// <summary>
        /// 已经存在
        /// </summary>
        public static string FileExists = "已经存在。";
        /// <summary>
        /// 是否覆盖
        /// </summary>
        public static string OverWrite = "要替换它吗？";
        //
        //Update by HeXiaoMing 2011-01-19
        //Reason: 表格重叠时导致分组异常提醒
        /// <summary>
        /// 表格重叠提醒
        /// </summary>
        public static string TableVSpaceError = "表格重叠造成被重叠表格丢失，如需全部显示请增大相邻表格的间距";//基本不用
        /// <summary>
        /// 标题
        /// </summary>
        public static string TableVSpaceTip = "分组异常";//基本不用
        //

        //修改：何晓明 2011-02-21
        //原因：打印机异常报错且无法与语言对应
        /// <summary>
        /// 打印失败
        /// </summary>
        public static string PrintExceptionMessage = "打印失败，请确认打印机是否连接正常且可用";

        /// <summary>
        /// 打印模板内容可能不能用
        /// </summary>
        public static string PrintTemplateIsNull = "模板内容可能不可用，请重新设置模板";

        public static string TreeNodeExists = "节点内容已经存在";

        public static string NoLoadSource = "数据源加载失败！";
    }
}
