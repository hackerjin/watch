using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using Lephone.Data;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.Print;

namespace Skyray.UC
{
    public interface IMediumAccess
    {

        /// <summary>
        /// 打开工作曲线，界面相应的动作处理函数
        /// </summary>
        void OpenCurveSubmit();

        /// <summary>
        /// 执行新的扫谱
        /// </summary>
        /// <param name="testParams"></param>
        void ExcuteTestStart(TestDevicePassedParams testParams);

        /// <summary>
        /// 执行初始化
        /// </summary>
        void TestInitialization();

        /// <summary>
        /// 停止扫谱操作
        /// </summary>
        void TestStop();

        /// <summary>
        /// 执行计算
        /// </summary>
        void ExcuteCaculate();

        /// <summary>
        /// 执行自动定峰标
        /// </summary>
        void ExcuteAutoDemarcateEnergy();

        void ExcuteAutoFPGAIntercept();
        /// <summary>
        /// 增加感兴趣元素操作
        /// </summary>
        void AddIntrestedElements();

        /// <summary>
        /// 工具栏选择模式
        /// </summary>
        void SelectMode(int index);

        /// <summary>
        /// 显示峰标识
        /// </summary>
        void DisplayPeak(NaviItem item);

        /// <summary>
        /// 报规格
        /// </summary>
        void ReportSpecification(NaviItem item);

        /// <summary>
        /// 自动分析
        /// </summary>
        void AutoAnalysis();

        /// <summary>
        /// 显示元素
        /// </summary>
        /// <param name="item"></param>
        void DisplayElement(NaviItem item);

        /// <summary>
        /// 选择工作谱操作
        /// </summary>
        void OpenWorkSpectrumSelect(List<SpecListEntity> spelist);

        /// <summary>
        /// 选择原始对比谱
        /// </summary>
        /// <param name="splist"></param>
        void OpenVirtualWorkSpectrum(List<SpecListEntity> splist);

        /// <summary>
        /// 选择比例对比谱
        /// </summary>
        /// <param name="splist"></param>
        void SelectRatioVirtualSpec(List<SpecListEntity> splist);

        /// <summary>
        /// 手动分析
        /// </summary>
        /// <param name="index"></param>
        /// <param name="atoms"></param>
        void ManAnalysis(int[] index, string[] atoms);

        /// <summary>
        /// 打印模板
        /// </summary>
        /// <param name="template"></param>
        void DirectPrint(List<TreeNodeInfo> lsit);

        /// <summary>
        /// 保存模板，更新列表
        /// </summary>
        void SaveTemplateUpdateEvent();

        /// <summary>
        /// 自定义选择
        /// </summary>
        void CustomFileld();

        /// <summary>
        /// 切换设备
        /// </summary>
        void DevicceChange();

        /// <summary>
        /// 保存设备
        /// </summary>
        void SaveDevice(NaviItem item, ContainerObject panel);

        /// <summary>
        /// 保存标准
        /// </summary>
        void SaveChangeStandand();

        /// <summary>
        /// 削本底
        /// </summary>
        void DisappearBk();

        /// <summary>
        /// 建立感兴趣区
        /// </summary>
        void CreateIntRegion();

        /// <summary>
        /// 计算感兴趣面积
        /// </summary>
        void CaculateIntRegion();

        /// <summary>
        /// 生成强度报告
        /// </summary>
        /// <returns></returns>
        ElementList CaculateIntensityReport(out string sampleName,out bool IsExplore);

        /// <summary>
        /// 显示对数谱
        /// </summary>
        void DisplayLogData();

        /// <summary>
        /// 定性分析的结果
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        string[] QualityResult(out int[] lines);

        /// <summary>
        /// 预热功能列表
        /// </summary>
        /// <param name="paramsValue"></param>
        void StartPreHeatProcess(PreHeatParams paramsValue);

        /// <summary>
        /// 选择规格
        /// </summary>
        void SelectSpecification();

        /// <summary>
        /// 连接仪器
        /// </summary>
        void ConnectDevice();

        bool IPSettings(string IP, string SubNet, string GateWay, string DNS);

        bool SetSurfaceSource(ushort firstLight, ushort secondLight, ushort thirdLight, ushort fourthLight);

        void CopyCurrentWorkCurve();

        void UpdateTitleICO();

        //void OpenOldSpec();

        //void SpectrumImport(string path);

        //void SpectrumExport(string path);

        void RefreshHistory();

    }
}
