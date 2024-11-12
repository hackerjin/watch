using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Cells;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Data;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common.ReportHelper
{
    public class LYReport : Report
    {
        public override string GenerateRetestReport(string fileName, DataTable dataTable, bool flag, bool AddStatistic)//多次测量一个表格
        {
            int modelColumnCount = (ExcelTemplateParams.TotalColumnMun == null || ExcelTemplateParams.TotalColumnMun == "") ? 15: int.Parse(ExcelTemplateParams.TotalColumnMun);
            int rowStartIndex = (ExcelTemplateParams.ManyTimeElementValueRowMun == null || ExcelTemplateParams.ManyTimeElementValueRowMun == "") ? 5: int.Parse(ExcelTemplateParams.ManyTimeElementValueRowMun);

            string StrSaveFileName = string.Empty;
            if (!System.IO.File.Exists(RetestFileName))
            {
                Msg.Show(Info.TemplateNoExists);
                return StrSaveFileName;
            }
            try
            {
                Workbook wb = new Workbook(FileFormatType.Excel97To2003);
                wb.Open(RetestFileName);
                Worksheet ws = wb.Worksheets[0];
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
                    ReplaceCellText(ref ws, "%ReportDate%", DateTime.Now.ToString());
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


                if (WorkCurveHelper.isShowND)
                {
                    ReplaceCellText(ref ws, "%Remarks%", Info.Remarks + WorkCurveHelper.NDValue.ToString() + "ppm");
                }
                else
                {
                    ReplaceCellText(ref ws, "%Remarks%", "");
                }
                ReplaceCellText(ref ws, "%WorkCurve%", WorkCurveName);

                //公司其它信息
                Dictionary<string, string> dReportOtherInfo = new Dictionary<string, string>();
                GetReportInfo(ref dReportOtherInfo);

                foreach (string sKey in dReportOtherInfo.Keys)
                {
                    ReplaceCellText(ref ws, "%" + sKey + "%", dReportOtherInfo[sKey]);
                }
                var columnCount = dataTable.Columns.Count;
                var diff = modelColumnCount - columnCount;
                if(diff > 0)
                {
                    ws.Cells.DeleteColumns(columnCount, diff, false);
                } 
                else if(diff < 0)
                {
                    ws.Cells.InsertColumns(modelColumnCount - 1, -diff);
                    for (int i = 0; i < -diff; i++)
			        {
                        ws.Cells.CopyColumn(ws.Cells, modelColumnCount - diff - 1, modelColumnCount + i - 1);
			        }
                }

                ws.Cells.InsertRows(rowStartIndex, columnCount + 1);

                object[,] cellsValue = new object[dataTable.Rows.Count + 1, dataTable.Columns.Count];
                int cols = 0;
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    for (int j = 0; j < dataTable.Rows.Count + 1; j++)
                    {
                        if (j == 0)
                        {
                            var name = dataTable.Columns[cols].ColumnName;
                            List<HistoryElement> hElementList = elementListPDF.FindAll(v => v.elementName == name);
                            if (hElementList != null && hElementList.Count > 0)
                            {
                                var sunit = (hElementList[0].unitValue.ToString() == "1") ? "(%)" : ((hElementList[0].unitValue.ToString() == "3") ? "(‰)" : "(ppm)");
                                cellsValue[j, cols] = dataTable.Columns[cols].ColumnName + sunit;
                            }
                            else
                            {
                                cellsValue[j, cols] = dataTable.Columns[cols].ColumnName;
                                if (cellsValue[j, cols] == "Time")
                                    cellsValue[j, cols] = "";
                            }

                        }
                        else
                        {
                            cellsValue[j, cols] = dataTable.Rows[j - 1][i].ToString();
                        }
                        
                    }
                    cols++;
                }
                Range range = ws.Cells.CreateRange(rowStartIndex, ws.Cells[0].Column, dataTable.Rows.Count + 1, dataTable.Columns.Count);
                Style style = ws.Cells.GetCellStyle(rowStartIndex, ws.Cells[0].Column);
                style.Font.Size = 10;
                style.HorizontalAlignment = TextAlignmentType.Center;
                style.VerticalAlignment = TextAlignmentType.Center;
                range.SetStyle(style);
                ws.Cells.ImportTwoDimensionArray(cellsValue, rowStartIndex, ws.Cells[0].Column);


               

                ReplaceElementsTable(ref ws, "%Average%");


                CurveElement ceTemp = Elements.Items.ToList().Find(l => l.Caption.ToLower().Equals("au"));
                if (ceTemp != null)
                {
                    if (selectLong.Count > 1 && dataTable != null && dataTable.Rows.Count > 0 && dataTable.Columns.Contains("Karat"))
                    {

                        // bool isShowKarat = dataTable.Columns.Contains("Karat");
                        double dSum = 0.0;
                        double dValue = 0.0;
                        for (int i = 0; i < selectLong.Count; i++)
                        {

                            double.TryParse(dataTable.Rows[i][dataTable.Columns.Count - 1].ToString(), out dValue);
                            dSum += dValue;
                        }
                        double dKarat = dSum / selectLong.Count;
                        ReplaceCellText(ref ws, "%Karat%", dKarat.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(k)");
                    }
                    else
                    {
                        //var Au = (from l in Elements.Items where string.Compare(l.Caption, "Au", true) == 0 select l.Content).FirstOrDefault();
                        double dKValue = ceTemp.Content * 24 / WorkCurveHelper.KaratTranslater;
                        ReplaceCellText(ref ws, "%Karat%", dKValue.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(k)");
                    }

                }
            Finish:

                //填充谱图
                Cell cell = FindTextCell(ws, "%Spectrum%");
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
                        //Bitmap bmp = new Bitmap(120, 60);
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
                    //Graphics g = Graphics.FromImage(bmp);
                    //int width = (int)Math.Round(Convert.ToInt32(rang.Width) * g.DpiX / 72.0);
                    //int height = (int)Math.Round(Convert.ToInt32(rang.Height) * g.DpiY / 72.0);
                    //bmp = new Bitmap(width, height);
                    DrawInterstringElems(ref bmp);
                    ReplaceCellImage(ref ws, "%ElemSpec%", bmp);

                }


                //#region 单元格边框
                //Range range1 = ws.Cells.CreateRange(ws.Cells[0].Row, ws.Cells[0].Column, ws.Cells.Rows.Count, ws.Cells.Columns.Count);
                //range1.SetOutlineBorder(BorderType.TopBorder, CellBorderType.Thin, Color.Black);
                //range1.SetOutlineBorder(BorderType.LeftBorder, CellBorderType.Thin, Color.Black);
                //range1.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
                //range1.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Black);
                //#endregion
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

    }
}
