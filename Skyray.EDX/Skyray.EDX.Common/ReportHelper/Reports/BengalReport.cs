using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Cells;
using System.IO;
using System.Drawing;
using Skyray.EDXRFLibrary;
using System.Windows.Forms;
using System.Xml;
using System.Data;
using System.Drawing.Imaging;
using Skyray.Language;

namespace Skyray.EDX.Common.ReportHelper
{
    public class BengalReport : Report
    {
        /// <summary>
        /// 单次测量生成报告
        /// </summary>
        public override string GenerateReport(string fileName, bool flag)
        {
            try
            {
                if (!File.Exists(TempletFileName))
                {
                    Msg.Show(Info.TemplateNoExists);
                    return string.Empty;
                }
                var wb = new Workbook(FileFormatType.Excel97To2003);
                wb.Open(TempletFileName);
                var ws = wb.Worksheets[0];
                ReplaceCellText(ref ws, "%Operator%", operateMember);
                if (specList != null)
                {
                    ReplaceCellText(ref ws, "%SampleName%", specList.SampleName);
                    ReplaceCellText(ref ws, "%SpecName%", this.specList.Name);
                    ReplaceCellText(ref ws, "%Supplier%", specList.Supplier);
                    ReplaceCellText(ref ws, "%TestTime%", specList.Specs[0].UsedTime.ToString() + "(s)");//待调试
                    ReplaceCellText(ref ws, "%Voltage%", specList.Specs[0].TubVoltage.ToString("f0") + "(KV)");
                    ReplaceCellText(ref ws, "%Current%", specList.Specs[0].TubCurrent.ToString("f0") + "(μA)");
                    ReplaceCellText(ref ws, "%TestDate%", ((DateTime)specList.SpecDate).ToString());
                    ReplaceCellText(ref ws, "%TestShortDate%", ((DateTime)specList.SpecDate).ToShortDateString());
                    ReplaceCellText(ref ws, "%TestShortDateFormat%", ((DateTime)specList.SpecDate).ToString("yyyy-MM-dd"));
                    ReplaceCellText(ref ws, "%TestShortTime%", ((DateTime)specList.SpecDate).ToShortTimeString());
                    ReplaceCellText(ref ws, "%ReportDate%", DateTime.Now.ToShortDateString());
                    ReplaceCellText(ref ws, "%ReportTime%", DateTime.Now.ToString("M/d/yyyy h:mm:ss tt"));
                    ReplaceCellText(ref ws, "%Shape%", this.specList.Shape);
                    ReplaceCellText(ref ws, "%Description%", this.specList.SpecSummary);
                    if (FindTextCell(ws, "%SamplePhoto%") != null && specList.ImageShow)
                    {
                        string fileNameFull = WorkCurveHelper.SaveSamplePath + "\\" + specList.Name + ".jpg";
                        var infoIf = new FileInfo(fileNameFull);
                        if (infoIf.Exists)
                        {
                            using (var tempImage = (Bitmap)Image.FromFile(fileNameFull))
                            {
                                ReplaceCellImage(ref ws, "%SamplePhoto%", tempImage);
                            }
                        }
                    }
                    ReplaceConditionCells(ref ws, "%CTestTime%");
                    ReplaceConditionCells(ref ws, "%CVoltage%");
                    ReplaceConditionCells(ref ws, "%CCurrent%");

                }
                ReplaceCellText(ref ws, "%DeviceName%", WorkCurveHelper.DeviceCurrent.Name);
                ReplaceCellText(ref ws, "%SampleWeight%", (dWeight > 0 ? dWeight.ToString() + "g" : ""));
                ReplaceCellText(ref ws, "%Specification%", Specification);//填充规格

                var address = ReportTemplateHelper.LoadSpecifiedNode("Report/BrassReport", "Address");
                ReplaceCellText(ref ws, "%Address%", Info.Address + ":" + (address == null ? "" : address.InnerText));

                if (WorkCurveHelper.isShowND)
                {
                    ReplaceCellText(ref ws, "%Remarks%", Info.Remarks + WorkCurveHelper.NDValue.ToString() + "ppm");
                }
                else
                {
                    ReplaceCellText(ref ws, "%Remarks%", "");
                }

                if (Spec != null)
                {
                    string filterElement = (WorkCurveHelper.DeviceCurrent.Filter.Count > 0 && Spec.DeviceParameter.FilterIdx > 0) ? "(" + WorkCurveHelper.DeviceCurrent.Filter[Spec.DeviceParameter.FilterIdx - 1].Caption + ")" : "";
                    ReplaceCellText(ref ws, "%FilterIdx%", Spec.DeviceParameter.FilterIdx + filterElement);
                    filterElement = (WorkCurveHelper.DeviceCurrent.Collimators.Count > 0 && Spec.DeviceParameter.CollimatorIdx > 0) ? "(" + WorkCurveHelper.DeviceCurrent.Collimators[Spec.DeviceParameter.CollimatorIdx - 1].Diameter + "mm)" : "";
                    ReplaceCellText(ref ws, "%CollimatorIdx%", Spec.DeviceParameter.CollimatorIdx + filterElement);
                }

                ReplaceCellText(ref ws, "%WorkCurve%", WorkCurveName);
                //编号
                Cell cell = FindTextCell(ws, "%ReadingNo%");
                if (cell != null)
                {
                    if (historyRecordid != null && historyRecordid != "")
                    {
                        HistoryRecord historyRecord = HistoryRecord.FindById(long.Parse(historyRecordid));
                        if (historyRecord != null) ReadingNo = historyRecord.HistoryRecordCode;
                    }
                    ReplaceCellText(ref ws, "%ReadingNo%", ReadingNo == null ? "" : ReadingNo);
                }
                var m = "au";
                if (Elements != null && Elements.MainElementToCalcKarat != null && Elements.MainElementToCalcKarat.Trim().Length > 0)
                {
                    m = Elements.MainElementToCalcKarat.Trim().ToLower();
                }
                var ceTemp = Elements.Items.ToList().Find(l => l.Caption.ToLower().Equals(m));
                if (ceTemp != null)
                {
                    //var Au = (from l in Elements.Items where string.Compare(l.Caption, "Au", true) == 0 select l.Content).FirstOrDefault();
                    double dKValue = ceTemp.Content * 24 / WorkCurveHelper.KaratTranslater;
                    ReplaceCellText(ref ws, "%Karat%", dKValue.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()));
                    var sunit = (ceTemp.ContentUnit.ToString() == "per") ? "%" : ((ceTemp.ContentUnit.ToString() == "permillage") ? "‰" : "ppm");
                    ReplaceCellText(ref ws, "%MainContent%", ceTemp.Content.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + sunit);
                    var at = Atoms.AtomList.Find(s => s.AtomName == ceTemp.Caption);
                    var fn = (at == null) ? "" : at.AtomNameEN;
                    ReplaceCellText(ref ws, "%MainFullName%", fn);
                }


                //填充元素名 含英文全称
                ReplaceElementsTable(ref ws, "%ElemName%");
                ReplaceElementsTable(ref ws, "%XRFLimits%");
                ReplaceElementsTable(ref ws, "%NothinginResult%");

                //填充元素全名称 中文全称
                ReplaceElementsTable(ref ws, "%ElemAllName%");
                ReplaceElementsTable(ref ws, "%ElemNameAll%");
                //填充强度
                ReplaceElementsTable(ref ws, "%Intensity%");
                //填充判定结果
                ReplaceElementsTable(ref ws, "%Results%");
                //填充:
                ReplaceElementsTable(ref ws, "%:%");
                //填充含量
                ReplaceElementsTable(ref ws, "%Content%");
                //填充误差
                ReplaceElementsTable(ref ws, "%Error%");
                //填充限定标准
                ReplaceElementsTable(ref ws, "%Limits%");
                //填充限定标准
                ReplaceElementsTable(ref ws, "%Weight%");


                //公司其它信息
                Dictionary<string, string> dReportOtherInfo = new Dictionary<string, string>();
                GetReportInfo(ref dReportOtherInfo);

                foreach (string sKey in dReportOtherInfo.Keys)
                {
                    ReplaceCellText(ref ws, "%" + sKey + "%", dReportOtherInfo[sKey]);
                }



                int width = 0;
                int height = 0;
                //填充谱图
                cell = FindTextCell(ws, "%Spectrum%", out width, out height);
                if (cell != null)
                {
                    if (!WorkCurveHelper.ReportSaveScreen)
                    {
                        //Bitmap bmp = new Bitmap(120, 60);
                        Bitmap bmp = new Bitmap(640, 160);
                        DrawSpec(ref bmp, false);
                        ReplaceCellImage(ref ws, "%Spectrum%", bmp);
                    }
                    else
                    {
                        string fileNameFull = Application.StartupPath + "\\Screenshots\\" + specList.Name + ".jpg";
                        FileInfo infoIf = new FileInfo(fileNameFull);
                        if (infoIf.Exists)
                        {
                            Bitmap tempImage = (Bitmap)Image.FromFile(fileNameFull);
                            ReplaceCellImage(ref ws, "%Spectrum%", tempImage);
                        }
                    }
                }
                //填充谱图
                cell = FindTextCell(ws, "%SpectrumWithOutWidth%", out width, out height);
                if (cell != null)
                {
                    if (!WorkCurveHelper.ReportSaveScreen)
                    {
                        //Bitmap bmp = new Bitmap(120, 60);
                        Bitmap bmp = new Bitmap(640, 480);
                        DrawSpec(ref bmp, true);
                        ReplaceCellImage(ref ws, "%SpectrumWithOutWidth%", bmp);
                    }
                    else
                    {
                        string fileNameFull = Application.StartupPath + "\\Screenshots\\" + specList.Name + ".jpg";
                        FileInfo infoIf = new FileInfo(fileNameFull);
                        if (infoIf.Exists)
                        {
                            using (var tempImage = (Bitmap)Image.FromFile(fileNameFull))
                            {
                                ReplaceCellImage(ref ws, "%SpectrumWithOutWidth%", tempImage);
                            }
                        }
                    }
                }


                //填充元素图
                cell = FindTextCell(ws, "%ElemSpec%");
                if (cell != null)
                {
                    Bitmap bmp = new Bitmap(640, 160);
                    //Graphics g = Graphics.FromImage(bmp);
                    //int width = (int)Math.Round(Convert.ToInt32(rang.Width) * g.DpiX / 72.0);
                    //int height = (int)Math.Round(Convert.ToInt32(rang.Height) * g.DpiY / 72.0);
                    //bmp = new Bitmap(width, height);
                    DrawInterstringElems(ref bmp);
                    ReplaceCellImage(ref ws, "%ElemSpec%", bmp);

                }
                //清除多余的%%之的值
                ReplaceExcelMatchText(ref ws, "%.?%|%.+%", null);
                DeleteEmptyRowsOrColumns(ref ws, DeleteEmptyType.Row);
                string strPassword = string.Empty;
                if (ReportTemplateHelper.IsEncryption)
                {

                    strPassword = GetPassWord(); //保护工作表
                    ws.Protect(ProtectionType.All, strPassword, "");
                }

                string StrSavePath = string.Empty;
                if (flag)
                {
                    switch (ReportFileType)
                    {
                        case 2:
                            StrSavePath = ReportPath + fileName + ".csv";
                            wb.Save(StrSavePath, Aspose.Cells.SaveFormat.CSV);
                            break;
                        case 1:
                            StrSavePath = ReportPath + fileName + ".pdf";
                            Aspose.Cells.CellsHelper.FontDir = System.Environment.GetEnvironmentVariable("windir") + "\\Fonts";
                            wb.Save(StrSavePath, Aspose.Cells.SaveFormat.Pdf);
                            break;
                        case 0:
                        default:
                            StrSavePath = ReportPath + fileName + ".xls";
                            wb.Save(StrSavePath, SaveFormat.Excel97To2003);
                            break;
                    }

                }
                else
                {
                    if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count > 0)
                    {
                        Aspose.Cells.Rendering.ImageOrPrintOptions Io = new Aspose.Cells.Rendering.ImageOrPrintOptions();
                        Io.HorizontalResolution = 200;
                        Io.VerticalResolution = 200;
                        Io.IsCellAutoFit = true;
                        Io.IsImageFitToPage = true;
                        Io.ChartImageType = System.Drawing.Imaging.ImageFormat.Png;
                        Io.ImageFormat = System.Drawing.Imaging.ImageFormat.Tiff;
                        Io.OnePagePerSheet = false;
                        Io.PrintingPage = PrintingPageType.IgnoreStyle;
                        Aspose.Cells.Rendering.SheetRender ss = new Aspose.Cells.Rendering.SheetRender(ws, Io);
                        System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
                        string printerName = doc.PrinterSettings.PrinterName;
                        ss.ToPrinter(printerName);
                    }
                }
                return StrSavePath;
            }
            catch (Exception ce)
            {
                Msg.Show(ce.Message);
            }
            finally
            {
            }
            return string.Empty;
        }

        public override string GenerateRetestReport(string fileName, DataTable dataTable, bool flag, bool AddStatistic)//多次测量一个表格
        {
            string iManyTimeElementRowMun = (ExcelTemplateParams.ManyTimeElementRowMun == null || ExcelTemplateParams.ManyTimeElementRowMun == "") ? "B9" : ExcelTemplateParams.ManyTimeElementRowMun;
            string iHorizontalManyTimeElementRowMun = (ExcelTemplateParams.HorizontalManyTimeElementRowMun == null || ExcelTemplateParams.HorizontalManyTimeElementRowMun == "") ? "B8" : ExcelTemplateParams.HorizontalManyTimeElementRowMun;
            if (ExcelTemplateParams.ManyTimeTemplate.Contains("Horizontal"))
                iManyTimeElementRowMun = iHorizontalManyTimeElementRowMun;
            int startIndex = (ExcelTemplateParams.ManyTimeElementValueRowMun == null || ExcelTemplateParams.ManyTimeElementValueRowMun == "") ? 10 : int.Parse(ExcelTemplateParams.ManyTimeElementValueRowMun);
            int startIndexHorizontal = (ExcelTemplateParams.HorizontalManyTimeElementValueRowMun == null || ExcelTemplateParams.HorizontalManyTimeElementValueRowMun == "") ? 9 : int.Parse(ExcelTemplateParams.HorizontalManyTimeElementValueRowMun);
            if (ExcelTemplateParams.ManyTimeTemplate.Contains("Horizontal"))
                startIndex = startIndexHorizontal;
            int iTotalColumnMun = (ExcelTemplateParams.TotalColumnMun == null || ExcelTemplateParams.TotalColumnMun == "") ? 9 : int.Parse(ExcelTemplateParams.TotalColumnMun);
            int iHorizontalTotalColumnMun = (ExcelTemplateParams.HorizontalTotalColumnMun == null || ExcelTemplateParams.HorizontalTotalColumnMun == "") ? 9 : int.Parse(ExcelTemplateParams.HorizontalTotalColumnMun);
            if (ExcelTemplateParams.ManyTimeTemplate.Contains("Horizontal"))
                iTotalColumnMun = iHorizontalTotalColumnMun;

            string StrSaveFileName = string.Empty;
            if (!System.IO.File.Exists(RetestFileName))
            {
                Msg.Show(Info.TemplateNoExists);
                return StrSaveFileName;
            }
            try
            {
                var wb = new Workbook(FileFormatType.Excel97To2003);
                wb.Open(RetestFileName);
                var ws = wb.Worksheets[0];
                ReplaceCellText(ref ws, "%Operator%", operateMember);
                if (specList != null)
                {
                    ReplaceCellText(ref ws, "%SampleName%", specList.SampleName);
                    ReplaceCellText(ref ws, "%SpecName%", this.specList.Name);
                    ReplaceCellText(ref ws, "%Supplier%", specList.Supplier);
                    ReplaceCellText(ref ws, "%TestTime%", specList.Specs[0].UsedTime.ToString() + "(s)");//待调试
                    ReplaceCellText(ref ws, "%Voltage%", specList.Specs[0].TubVoltage.ToString("f0") + "(KV)");
                    ReplaceCellText(ref ws, "%Current%", specList.Specs[0].TubCurrent.ToString("f0") + "(μA)");
                    ReplaceCellText(ref ws, "%TestDate%", ((DateTime)specList.SpecDate).ToString());
                    ReplaceCellText(ref ws, "%TestShortDate%", ((DateTime)specList.SpecDate).ToShortDateString());
                    ReplaceCellText(ref ws, "%TestShortTime%", ((DateTime)specList.SpecDate).ToShortTimeString());
                    ReplaceCellText(ref ws, "%ReportDate%", DateTime.Now.ToShortDateString());
                    ReplaceCellText(ref ws, "%ReportTime%", DateTime.Now.ToString("M/d/yyyy h:mm:ss tt"));
                    ReplaceCellText(ref ws, "%Shape%", this.specList.Shape);
                    ReplaceCellText(ref ws, "%Description%", this.specList.SpecSummary);
                    if (FindTextCell(ws, "%SamplePhoto%") != null && specList.ImageShow)
                    {
                        string fileNameFull = WorkCurveHelper.SaveSamplePath + "\\" + specList.Name + ".jpg";
                        FileInfo infoIf = new FileInfo(fileNameFull);
                        if (infoIf.Exists)
                        {
                            Bitmap tempImage = (Bitmap)Image.FromFile(fileNameFull);
                            ReplaceCellImage(ref ws, "%SamplePhoto%", tempImage);
                        }
                    }
                }
                ReplaceCellText(ref ws, "%DeviceName%", WorkCurveHelper.DeviceCurrent.Name);
                ReplaceCellText(ref ws, "%SampleWeight%", (dWeight > 0 ? dWeight.ToString() + "g" : ""));
                ReplaceCellText(ref ws, "%Specification%", Specification);//填充规格

                var address = ReportTemplateHelper.LoadSpecifiedNode("Report/BrassReport", "Address");
                ReplaceCellText(ref ws, "%Address%", Info.Address + ":" + (address == null ? "" : address.InnerText));

                if (WorkCurveHelper.isShowND)
                {
                    ReplaceCellText(ref ws, "%Remarks%", Info.Remarks + WorkCurveHelper.NDValue.ToString() + "ppm");
                }
                else
                {
                    ReplaceCellText(ref ws, "%Remarks%", "");
                }

                if (Spec != null)
                {
                    string filterElement = (WorkCurveHelper.DeviceCurrent.Filter.Count > 0 && Spec.DeviceParameter.FilterIdx > 0) ? "(" + WorkCurveHelper.DeviceCurrent.Filter[Spec.DeviceParameter.FilterIdx - 1].Caption + ")" : "";
                    ReplaceCellText(ref ws, "%FilterIdx%", Spec.DeviceParameter.FilterIdx + filterElement);
                    filterElement = (WorkCurveHelper.DeviceCurrent.Collimators.Count > 0 && Spec.DeviceParameter.CollimatorIdx > 0) ? "(" + WorkCurveHelper.DeviceCurrent.Collimators[Spec.DeviceParameter.CollimatorIdx - 1].Diameter + "mm)" : "";
                    ReplaceCellText(ref ws, "%CollimatorIdx%", Spec.DeviceParameter.CollimatorIdx + filterElement);
                }

                ReplaceCellText(ref ws, "%WorkCurve%", WorkCurveName);
                //编号
                Cell cell = FindTextCell(ws, "%ReadingNo%");
                if (cell != null)
                {
                    if (historyRecordid != null && historyRecordid != "")
                    {
                        HistoryRecord historyRecord = HistoryRecord.FindById(long.Parse(historyRecordid));
                        if (historyRecord != null) ReadingNo = historyRecord.HistoryRecordCode;
                    }
                    ReplaceCellText(ref ws, "%ReadingNo%", ReadingNo == null ? "" : ReadingNo);
                }


                //公司其它信息
                Dictionary<string, string> dReportOtherInfo = new Dictionary<string, string>();
                GetReportInfo(ref dReportOtherInfo);

                foreach (string sKey in dReportOtherInfo.Keys)
                {
                    ReplaceCellText(ref ws, "%" + sKey + "%", dReportOtherInfo[sKey]);
                }
                //填充:

                ReplaceElementsTable(ref ws, "%ElemAllName%");
                ReplaceElementsTable(ref ws, "%Average%");
                ReplaceElementsTable(ref ws, "%:%");
                var m = "au";
                if (Elements != null && Elements.MainElementToCalcKarat != null && Elements.MainElementToCalcKarat.Trim().Length > 0)
                {
                    m = Elements.MainElementToCalcKarat.Trim().ToLower();
                }
                var ceTemp = Elements.Items.ToList().Find(l => l.Caption.ToLower().Equals(m));
                if (ceTemp != null)
                {
                    if (elementListPDF != null && elementListPDF.Count > 0)
                    {

                        //double dSum = 0.0;
                        //double dValue = 0.0;
                        //for (int i = 0; i < selectLong.Count; i++)
                        //{

                        //    double.TryParse(dataTable.Rows[i]["Karat"].ToString(), out dValue);
                        //    dSum += dValue;
                        //}
                        //double dKarat = dSum / selectLong.Count;
                        var heList = elementListPDF.FindAll(v => v.elementName == ceTemp.Caption);
                        var avgK = CalcAvgKarat(heList);
                        if (avgK.HasValue)
                        {
                            ReplaceCellText(ref ws, "%Karat%", avgK.Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()));
                        }

                    }
                    else
                    {
                        var kVal = ceTemp.Content * 24 / WorkCurveHelper.KaratTranslater;
                        ReplaceCellText(ref ws, "%Karat%", kVal.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()));
                    }
                    var at = Atoms.AtomList.Find(s => s.AtomName == ceTemp.Caption);
                    var fn = (at == null) ? "" : at.AtomNameEN;
                    ReplaceCellText(ref ws, "%MainFullName%", fn);
                    var sunit = "";
                    var list = elementListPDF.FindAll(v => v.elementName == ceTemp.Caption);
                    var avg = CalcAvgContent(list);
                    if (avg.HasValue)
                    {
                        if (IsAnalyser)
                        {
                            sunit = (list[0].unitValue.ToString() == "1") ? "%" : ((list[0].unitValue.ToString() == "3") ? "‰" : "ppm");
                        }

                        var r = avg.Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + sunit;
                        ReplaceCellText(ref ws, "%MainAvgContent%", r); 
                    }
                }

                       
            Finish:

                //填充谱图
                cell = FindTextCell(ws, "%Spectrum%");
                if (cell != null)
                {
                    Bitmap bmp = new Bitmap(640, 160);
                    // Graphics g = Graphics.FromImage(bmp);
                    DrawSpec(ref bmp, false);
                    ReplaceCellImage(ref ws, "%Spectrum%", bmp);
                }

                //填充谱图
                cell = FindTextCell(ws, "%SpectrumWithOutWidth%");
                if (cell != null)
                {
                    if (cell != null)
                    {
                        Bitmap bmp = new Bitmap(640, 480);
                        DrawSpec(ref bmp, true);
                        ReplaceCellImage(ref ws, "%SpectrumWithOutWidth%", bmp);
                    }

                }


                //填充元素图
                cell = FindTextCell(ws, "%ElemSpec%");
                if (cell != null)
                {
                    Bitmap bmp = new Bitmap(640, 160);
                    DrawInterstringElems(ref bmp);
                    ReplaceCellImage(ref ws, "%ElemSpec%", bmp);

                }

                #region 删除多余行
                //清除多余的%%之的值
                ReplaceExcelMatchText(ref ws, "%.?%|%.+%", null);
                DeleteEmptyRowsOrColumns(ref ws, DeleteEmptyType.Row);
                #endregion
                string strPassword = string.Empty;
                if (ReportTemplateHelper.IsEncryption)
                {

                    strPassword = GetPassWord(); //保护工作表
                    ws.Protect(ProtectionType.All, strPassword, "");
                }
                

                //string StrSavePath = string.Empty;
                if (flag)
                {
                    switch (ReportFileType)
                    {
                        case 2:
                            StrSaveFileName = ReportPath + fileName + ".csv";
                            wb.Save(StrSaveFileName, Aspose.Cells.SaveFormat.CSV);
                            break;
                        case 1:
                            StrSaveFileName = ReportPath + fileName + ".pdf";
                            Aspose.Cells.CellsHelper.FontDir = System.Environment.GetEnvironmentVariable("windir") + "\\Fonts";
                            wb.Save(StrSaveFileName, Aspose.Cells.SaveFormat.Pdf);
                            break;
                        case 0:
                        default:
                            StrSaveFileName = ReportPath + fileName + ".xls";
                            wb.Save(StrSaveFileName, SaveFormat.Excel97To2003);
                            break;
                    }

                }
                else
                {
                    if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count > 0)
                    {
                        Aspose.Cells.Rendering.ImageOrPrintOptions Io = new Aspose.Cells.Rendering.ImageOrPrintOptions();
                        Io.HorizontalResolution = 200;
                        Io.VerticalResolution = 200;
                        Io.IsCellAutoFit = true;
                        Io.IsImageFitToPage = true;
                        Io.ChartImageType = System.Drawing.Imaging.ImageFormat.Png;
                        Io.ImageFormat = System.Drawing.Imaging.ImageFormat.Tiff;
                        Io.OnePagePerSheet = false;
                        Io.PrintingPage = PrintingPageType.IgnoreStyle;
                        Aspose.Cells.Rendering.SheetRender ss = new Aspose.Cells.Rendering.SheetRender(ws, Io);
                        System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
                        string printerName = doc.PrinterSettings.PrinterName;
                        ss.ToPrinter(printerName);
                    }
                }
                return StrSaveFileName;
            }
            catch (Exception ce)
            {
                Msg.Show(ce.Message);
            }
            finally
            {
            }
            return StrSaveFileName;
        }

        protected override void ReplaceCellImage(ref Worksheet worksheet, string strLabel, Bitmap Image)
        {
            var cell = worksheet.Cells.FindStringContains(strLabel, null);
            if (cell != null)
            {
                var mstream = new MemoryStream();
                Image.Save(mstream, ImageFormat.Bmp);
                if (cell.IsMerged)
                {
                    var celllst = worksheet.Cells.MergedCells;
                    for (int i = 0; i < celllst.Count; i++)
                    {
                        var CellAreaTemp = (Aspose.Cells.CellArea)celllst[i];
                        if (CellAreaTemp.StartColumn == cell.Column && CellAreaTemp.StartRow == cell.Row)
                        {
                            worksheet.Pictures.Add(cell.Row, cell.Column, CellAreaTemp.EndRow + 1, CellAreaTemp.EndColumn + 1, mstream);
                            break;
                        }

                    }
                }
                else worksheet.Pictures.Add(cell.Row, cell.Column, mstream);
            }
        }

        protected override void ReplaceElementsTable(ref Worksheet worksheet, string strLabel)
        {
            List<Cell> lst = new List<Cell>();
            //寻找cells
            Cell cellTemp = worksheet.Cells.FindStringContains(strLabel, null);
            while (cellTemp != null)
            {
                lst.Add(cellTemp);
                cellTemp = worksheet.Cells.FindStringContains(strLabel, cellTemp);
            }
            if (lst.Count <= 0) return;
            //判断显示元素
            List<string> ElementNames = ReportTemplateHelper.ReportAtomNames == null ? new List<string>() : ReportTemplateHelper.ReportAtomNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            int count = lst.Count;

            //以平均值含量降序排列元素名称
            var itemList = Elements.Items.ToList();
            var avgDic = new Dictionary<string, double>();
            if (elementListPDF != null && elementListPDF.Count > 0)
            {
                foreach (var item in itemList)
                {
                    var heList = elementListPDF.FindAll(v => v.elementName == item.Caption);
                    var avg = CalcAvgContent(heList);
                    var val = avg.HasValue ? avg.Value : 0d;
                    if (avgDic.ContainsKey(item.Caption))
                        continue;
                    avgDic.Add(item.Caption, val);
                }
            }
            else
            {
                foreach (var item in itemList)
                {
                    if (avgDic.ContainsKey(item.Caption))
                        continue;
                    avgDic.Add(item.Caption, item.Content);
                }
            }
           
            var sortedDic = from item in avgDic
                    orderby item.Value descending, item.Key ascending
                    select item;
            
            if (ElementNames.Count <= 0)
            {
                var tmpNames = new List<string>();
                var elelist = Elements.Items.ToList().OrderByDescending(w => w.Content);
                foreach (var em in elelist)
                {
                    if (WorkCurveHelper.IsReoprtShowQualityElem)
                    {
                        if (!em.IsDisplay || !em.IsShowElement) continue;
                        if (em.Id == 0) continue;
                    }
                    else
                    {
                        if (!em.IsDisplay && !em.IsShowContent && !em.IsShowElement) continue;
                    }
                    tmpNames.Add(em.Caption);
                }
                foreach (var item in sortedDic)
                {
                    if (!tmpNames.Contains(item.Key))
                        continue;
                    ElementNames.Add(item.Key);
                }
            }
            else
            {
                List<string> remove = new List<string>();
                foreach (var en in ElementNames)
                {
                    var em = Elements.Items.ToList().OrderBy(w => w.Caption.ToLower().CompareTo(en.ToLower()) == 0);
                    if (em == null) remove.Add(en);

                }
                if (remove.Count > 0)
                {
                    foreach (var en in remove)
                    {
                        ElementNames.Remove(en);
                    }
                }
            }
            int countTotal = ElementNames.Count;

            if (count <= 0) return;
            int j = 0;
            double totalContent = 0d;
            string totalStatus = ExcelTemplateParams.PassResults;
            string sunit = "";

            if (count < countTotal)
            {
                while (count < countTotal)
                {
                    worksheet.Cells.InsertRow(lst[0].Row + 1);
                    worksheet.Cells.CopyRow(worksheet.Cells, lst[0].Row, lst[0].Row + 1);
                    count++;
                }
                lst.Clear();
                cellTemp = worksheet.Cells.FindStringContains(strLabel, null);
                while (cellTemp != null)
                {
                    lst.Add(cellTemp);
                    cellTemp = worksheet.Cells.FindStringContains(strLabel, cellTemp);
                }
                if (lst.Count < countTotal) return;
            }
            Dictionary<string, string> DElementRetult = new Dictionary<string, string>();

            string sTestRetult = "";
            CustomStandard standard = CustomStandard.FindById(historyStandID);
            if (strLabel.ToLower().CompareTo("%results%") == 0 && historyStandID > -1 && historyRecordid != "") //判定结果时
            {
                if (WorkCurveHelper.isShowXRFStandard == 1)
                    sTestRetult = ExcelTemplateParams.TestResultForXrf(historyRecordid, ref DElementRetult);
                else
                    sTestRetult = ExcelTemplateParams.TestRetult(historyRecordid, ref DElementRetult);

            }
            if (Elements.LayerElemsInAnalyzer == null) Elements.LayerElemsInAnalyzer = "";
            string[] strElemsLayer = Helper.ToStrs(Elements.LayerElemsInAnalyzer);
            //替换%ElemName%
            for (int i = 0; i < countTotal; i++)
            {

                // var em = Elements.Items.ToList().Find(w => w.Caption.ToLower().CompareTo(ElementNames[i].ToLower()) == 0);
                var em = Elements.Items.ToList().FindAll(m => m.IsShowElement).Find(w => w.Caption.ToLower().CompareTo(ElementNames[i].ToLower()) == 0);
                if (em == null) continue;
                string StrReplace = lst[i].Value.ToString();
                string StrNewText = "";
                switch (strLabel.ToLower())
                {
                    case "%elemname%"://元素名0
                        //string elementName = Elements.Items[i].Caption;
                        //if (Elements.Items[i].IsOxide) elementName = Elements.Items[i].Formula;
                        string elementName = em.IsOxide ? em.Formula : em.Caption;
                        string strContentU = "%";
                        switch (em.ContentUnit)
                        {
                            case ContentUnit.permillage:
                                strContentU = "‰";
                                break;
                            case ContentUnit.ppm:
                                strContentU = "ppm";
                                break;
                            default:
                                strContentU = "%";
                                break;
                        }
                        StrNewText = IsAnalyser ? elementName : elementName + "(" + strContentU + ")";
                        if (ReportTemplateHelper.ExcelModeType == 18)
                        {
                            Style styleTemp = lst[i].GetDisplayStyle();
                            styleTemp.ForegroundColor = Color.Gray;
                            styleTemp.Pattern = BackgroundType.ThinDiagonalCrosshatch;
                            lst[i].SetStyle(styleTemp);
                        }
                        StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        lst[i].PutValue(StrReplace);

                        break;
                    case "%elemallname%"://元素全名6
                        Atom atom = Atoms.AtomList.Find(s => s.AtomName == em.Caption);
                        string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                        string atomNameEN = (atom == null) ? "" : atom.AtomNameEN;
                        StrNewText = Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN : atomNameEN;
                        StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        lst[i].PutValue(StrReplace);

                        break;
                    case "%elemnameall%"://元素全名6
                        Atom atomall = Atoms.AtomList.Find(s => s.AtomName == em.Caption);
                        string atomNameallCN = (atomall == null) ? "" : atomall.AtomNameCN;
                        StrNewText = Lang.Model.CurrentLang.IsDefaultLang ? atomNameallCN + "(" + em.Caption + ")" : em.Caption;
                        StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        lst[i].PutValue(StrReplace);

                        break;
                    case "%intensity%"://强度1
                        //StrNewText = em.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString());
                        lst[i].PutValue(Math.Round(em.Intensity, WorkCurveHelper.SoftWareIntensityBit));
                        break;
                    case "%atomnum%"://原子序号19
                        //StrNewText = em.AtomicNumber.ToString();
                        lst[i].PutValue(em.AtomicNumber);
                        break;
                    case "%:%":
                        lst[i].PutValue(":");
                        break;
                    case "%content%"://含量2
                        if (IsAnalyser)
                        {
                            sunit = "";
                            sunit = (em.ContentUnit.ToString() == "per") ? "%" : ((em.ContentUnit.ToString() == "permillage") ? "‰" : "ppm");
                            //if (Elements.RhIsLayer && em.Caption.ToUpper().Equals("RH")) 
                            if (Elements.RhIsLayer && strElemsLayer.Contains(em.Caption))
                                //StrNewText=Elements.Items[i].Thickness.ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(um)";
                                StrNewText = em.Thickness.ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(um)";
                            else
                                StrNewText = em.Content.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + sunit;
                            StrReplace = StrReplace.Replace(strLabel, StrNewText);
                            lst[i].PutValue(StrReplace);
                        }
                        else
                        {
                            if (em.Content * 10000 < WorkCurveHelper.NDValue && isShowND)
                            {
                                StrNewText = "ND";
                                StrReplace = StrReplace.Replace(strLabel, StrNewText);
                                lst[i].PutValue(StrReplace);
                            }
                            else
                            {
                                double dblContent = 0;
                                if (em.ContentUnit == ContentUnit.per)
                                    dblContent = em.Content;
                                //StrNewText = em.Content.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                                else if (em.ContentUnit == ContentUnit.permillage)
                                    //StrNewText = (em.Content * 10).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                                    dblContent = em.Content * 10;
                                else
                                    // StrNewText = (em.Content * 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                                    dblContent = em.Content * 10000;
                                // lst[i].PutValue(Math.Round(dblContent, WorkCurveHelper.SoftWareContentBit));
                                lst[i].PutValue(dblContent.ToString("f" + WorkCurveHelper.SoftWareContentBit));
                            }
                            //if (historyStandID > -1)
                            //{
                            //    CustomStandard standard = CustomStandard.FindById(historyStandID);
                            if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0 && standard.IsSelectTotal)
                            {
                                StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                                {
                                    return string.Compare(w.ElementCaption, em.Caption, true) == 0;
                                });
                                if (standSample != null)
                                {
                                    //totalContent += (Elements.Items[i].Content * 10000);
                                    totalContent += (em.Content * 10000);
                                    if (totalContent > standard.TotalContentStandard)
                                        totalStatus = strSampleFalse;
                                    else
                                        totalStatus = strSamplePass;
                                }
                            }
                            //}
                        }

                        break;
                    case "%error%"://误差3
                        switch (em.ContentUnit)
                        {
                            case ContentUnit.ppm:
                                StrNewText = (em.Error * 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                                break;
                            case ContentUnit.permillage:
                                StrNewText = (em.Error * 10).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                                break;
                            case ContentUnit.per:
                            default:
                                StrNewText = em.Error.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                                break;

                        }
                        StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        lst[i].PutValue(StrReplace);
                        break;
                    case "%xrflimits%":
                        if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
                        {
                            StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                            {
                                return string.Compare(w.ElementCaption, em.Caption, true) == 0;
                            });
                            if (standSample != null)
                                StrNewText = standSample.StartStandardContent.ToString() + " - " + standSample.StandardContent.ToString();
                            else
                                StrNewText = "0-0";
                        }
                        //}
                        else
                            StrNewText = "0-0";
                        StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        lst[i].PutValue(StrReplace);
                        break;
                    case "%nothinginresult%":
                        StrNewText = "－－";
                        StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        lst[i].PutValue(StrReplace);
                        break;
                    case "%limits%"://限定值4

                        if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
                        {
                            StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                            {
                                return string.Compare(w.ElementCaption, em.Caption, true) == 0;
                            });
                            if (standSample != null)
                                StrNewText = standSample.StandardContent.ToString();
                            else
                                //StrNewText = "0";
                                StrNewText = "  ";
                        }
                        else
                            StrNewText = "0";
                        StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        //lst[i].PutValue(Convert.ToDouble(StrReplace));
                        lst[i].PutValue(StrReplace);
                        break;
                    case "%thicklimits%"://限定值4
                        //if (historyStandID > -1)
                        //{
                        //    CustomStandard standard = CustomStandard.FindById(historyStandID);
                        if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
                        {
                            StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                            {
                                return string.Compare(w.ElementCaption, em.Caption, true) == 0;
                            });
                            if (standSample != null)
                                StrNewText = standSample.StandardThick.ToString() + " -" + standSample.StandardThickMax.ToString();
                            else
                                StrNewText = "0-0";
                        }
                        //}
                        else
                            StrNewText = "0-0";
                        StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        lst[i].PutValue(StrReplace);
                        break;
                    case "%results%"://判定5
                        string strJust = " ";
                        if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
                        {
                            StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                            {
                                return string.Compare(w.ElementCaption, em.Caption, true) == 0;
                            });
                            if (standSample != null)
                                DElementRetult.TryGetValue(em.Caption, out strJust);
                            else
                                strJust = "  ";
                        }
                        StrNewText = (strJust == null) ? "" : strJust;
                        StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        lst[i].PutValue(StrReplace);
                        break;
                    
                    case "%average%"://含量平均值12
                        var heList = elementListPDF.FindAll(v => v.elementName == em.Caption);
                        double? avg = null;
                        var dic = sortedDic.ToDictionary(a =>a.Key, b => b.Value);
                        if(dic != null && dic.ContainsKey(em.Caption))
                        {
                            avg = dic[em.Caption];
                        }
                        if (IsAnalyser)
                        {
                            sunit = "";
                            sunit = (heList[0].unitValue.ToString() == "1") ? "%" : ((heList[0].unitValue.ToString() == "3") ? "‰" : "ppm");
                        }

                        if (avg.HasValue)
                        {
                            if (sunit == "")
                            { lst[i].PutValue(Math.Round(avg.Value, WorkCurveHelper.SoftWareContentBit)); }
                            else
                            {
                                StrNewText =  avg.Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + sunit;
                                StrReplace = StrReplace.Replace(strLabel, StrNewText);
                                lst[i].PutValue(StrReplace);
                            }
                        }
                        
                        break;
                    case "%weight%"://重量8
                        double dEleWeight = 0;
                        if (dWeight > 0)
                        {
                            double dEleContent = em.Content;
                            if (em.ContentUnit.ToString() == "permillage") dEleContent = dEleContent / 10;

                            dEleWeight = (dWeight ?? 0.0) / 100 * float.Parse(dEleContent.ToString());
                        }
                        StrNewText = dEleWeight.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(g)";
                        StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        lst[i].PutValue(StrReplace);
                        break;
                    case "%min%"://最小值9
                        double dMin = 0;
                        List<HistoryElement> hMinElementList = elementListPDF.FindAll(delegate(HistoryElement v) { return v.elementName == em.Caption; });
                        if (hMinElementList.Count > 0) dMin = double.MaxValue;
                        foreach (HistoryElement he in hMinElementList)
                        {
                            double dValue = 0.0;
                            double.TryParse(he.contextelementValue, out dValue);
                            if (Math.Round(dValue, WorkCurveHelper.SoftWareContentBit) < dMin) dMin = Math.Round(dValue, WorkCurveHelper.SoftWareContentBit);
                        }
                        if (IsAnalyser)
                        {
                            sunit = "";
                            sunit = (hMinElementList[0].unitValue.ToString() == "1") ? "%" : ((hMinElementList[0].unitValue.ToString() == "3") ? "‰" : "ppm");
                        }
                        // StrNewText = dMin.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + ((sunit == "") ? "" : "(" + sunit + ")");
                        if (sunit == "")
                        {
                            lst[i].PutValue(Math.Round(dMin, WorkCurveHelper.SoftWareContentBit));
                        }
                        else
                        {
                            StrNewText = dMin.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + ((sunit == "") ? "" : "(" + sunit + ")");
                            StrReplace = StrReplace.Replace(strLabel, StrNewText);
                            lst[i].PutValue(StrReplace);
                        }
                        break;
                    case "%max%"://最大值10
                        double dMax = 0;
                        List<HistoryElement> hMaxElementList = elementListPDF.FindAll(delegate(HistoryElement v) { return v.elementName == em.Caption; });
                        if (hMaxElementList.Count > 0) dMax = double.MinValue;//Math.Round(double.Parse(hMaxElementList[0].contextelementValue), WorkCurveHelper.SoftWareContentBit);
                        foreach (HistoryElement he in hMaxElementList)
                        {
                            double dValue = 0.0;
                            double.TryParse(he.contextelementValue, out dValue);
                            if (Math.Round(dValue, WorkCurveHelper.SoftWareContentBit) > dMax) dMax = Math.Round(dValue, WorkCurveHelper.SoftWareContentBit);
                            //dMax = (dMax == 0) ? Math.Round(double.Parse(he.contextelementValue), 4) : ((Math.Round(double.Parse(he.contextelementValue), 4) > dMax) ? Math.Round(double.Parse(he.contextelementValue), 4) : dMax);
                        }

                        if (IsAnalyser)
                        {
                            sunit = "";
                            sunit = (hMaxElementList[0].unitValue.ToString() == "1") ? "%" : ((hMaxElementList[0].unitValue.ToString() == "3") ? "‰" : "ppm");

                        }

                        //StrNewText =  dMax.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + ((sunit == "") ? "" : "(" + sunit + ")");
                        if (sunit == "")
                        {
                            lst[i].PutValue(Math.Round(dMax, WorkCurveHelper.SoftWareContentBit));
                        }
                        else
                        {
                            StrNewText = dMax.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + ((sunit == "") ? "" : "(" + sunit + ")");
                            StrReplace = StrReplace.Replace(strLabel, StrNewText);
                            lst[i].PutValue(StrReplace);
                        }
                        break;
                    case "%sd%"://SD
                        List<HistoryElement> hSDElementList = elementListPDF.FindAll(delegate(HistoryElement v) { return v.elementName == em.Caption; });
                        double totalMun = 0;
                        foreach (HistoryElement he in hSDElementList)
                        {
                            double dValue = 0.0;
                            double.TryParse(he.contextelementValue, out dValue);
                            totalMun += Math.Round(dValue, WorkCurveHelper.SoftWareContentBit);
                        }
                        double p = totalMun / hSDElementList.Count;
                        p = Math.Round(p, WorkCurveHelper.SoftWareContentBit);

                        double s2 = 0;
                        foreach (HistoryElement he in hSDElementList)
                        {
                            double dValue = 0.0;
                            double.TryParse(he.contextelementValue, out dValue);
                            s2 += Math.Pow((dValue - p), 2);
                        }
                        s2 = ((hSDElementList.Count - 1) == 0 || s2 == 0) ? 0 : (Math.Sqrt(s2 / (hSDElementList.Count - 1)));


                        if (IsAnalyser)
                        {
                            sunit = "";
                            sunit = (hSDElementList[0].unitValue.ToString() == "1") ? "%" : ((hSDElementList[0].unitValue.ToString() == "3") ? "‰" : "ppm");

                        }
                        // StrNewText = s2.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + ((sunit == "") ? "" : "(" + sunit + ")");
                        if (sunit == "")
                        {
                            lst[i].PutValue(Math.Round(s2, WorkCurveHelper.SoftWareContentBit));
                        }
                        else
                        {
                            StrNewText = s2.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + ((sunit == "") ? "" : "(" + sunit + ")");
                            StrReplace = StrReplace.Replace(strLabel, StrNewText);
                            lst[i].PutValue(StrReplace);
                        }
                        break;

                }

            }

        }

        protected double? CalcAvgContent(List<HistoryElement> list)
        {
            if (list == null || list.Count <=0)
                return null;
            double sum = 0d, val = 0d;
            foreach (HistoryElement item in list)
            {
                val = 0d;
                double.TryParse(item.contextelementValue, out val);
                sum += val;
            }
            return sum / list.Count;
        }

        protected double? CalcAvgKarat(List<HistoryElement> list)
        {
            if (list == null || list.Count <= 0)
                return null;
            double sum = 0d, val = 0d, k = 0d;
            foreach (HistoryElement item in list)
            {
                val = 0d;
                double.TryParse(item.contextelementValue, out val);
                k = val * 24 / WorkCurveHelper.KaratTranslater;
                sum += k;
            }
            return sum / list.Count;
        }
    }
}
