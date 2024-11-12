using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Printing;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Define;
using Skyray.Language;
using System.Linq;
using System.Xml.Linq;
using System.Collections;
using System.Runtime.InteropServices;
using Skyray.EDXRFLibrary.Spectrum;
using Aspose.Cells;

namespace Skyray.EDX.Common.ReportHelper
{
    public class Report
    {
        public ElementList Elements;///<报告的所有元素信息
        public SpecEntity Spec; ///<当前谱
     
        private string _strTempletFileName;
        private string _retestFileName;
        public SpecEntity unitSpec; ///连测谱

        public SpecListEntity specList;
        public int specCount;
        public int curElemCount;///<当前曲线实际计算的元素
        protected const int OFFSET = 50;///<画单个元素谱图时取其边界的偏移
        public List<int> FirstContIntr;
        public int InterestElemCount;///<感兴趣元素的个数(指除去定性分析的元素)
        public bool IsDrawQualeElems;///<是否画定性分析元素
        public int QualeElemCount;///<定性分析元素个数
                                     ///
        public ElemsStandard ElemsStd;

        public long historyStandID;

        public bool isShowND;

        public bool IsPlasticUnit;

        public string strSamplePass = ExcelTemplateParams.PassResults;///<判定样品含量不超标
        public string strSampleFalse = ExcelTemplateParams.FalseResults;///<判定样品含量超标
        public string strSampleSTD = ExcelTemplateParams.STDResults;///<判定样品含量  我也不知道是咋样

        public string Specification = string.Empty;//规格
        public List<long> lstWorkCurveId = new List<long>();//workCurveId列表

        public bool IsAnalyser;
        public double? dWeight=0.0;//样品重量

        public List<long> selectLong = new List<long>();

        public int ReportFileType = 0;//0，EXCEl,1,PDF

        public List<HistoryElement> elementListPDF = new List<HistoryElement>();

        public string operateMember;
        public string WorkCurveName;
        public string WorkCurveID;
        public string historyRecordid;
        public string ReadingNo;
        public bool NeedMultiResults = true;
       /// <summary>
        /// 构造函数
        /// </summary>
        public Report()
        {
            specCount = 1;
            unitSpec = new SpecEntity();
            FirstContIntr = new List<int>();
           
        }

        public ElementList elements
        {
            get {
                return Elements;
            }
            set {
                Elements = value;
            }
        }

        /// <summary>
        /// 多次测量模板文件名
        /// </summary>
        public string RetestFileName
        {
            get
            {
                return _retestFileName;
            }
            set
            {
                _retestFileName = value;
            }
        }

        /// <summary>
        /// 模板文件名
        /// </summary>
        public string TempletFileName
        {
            get
            {
                return _strTempletFileName;
            }
            set
            {
                _strTempletFileName = value;
            }
        }
        /// <summary>
        /// 自定义分析数据方法委托
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public delegate double StatisticsDelegate(object sender);
        public List<StatisticsDelegate> ListStatisticsMethods = new List<StatisticsDelegate>();
        private string _strReportPath;
        /// <summary>
        /// 报告保存路径
        /// </summary>
        public string ReportPath
        {
            get
            {
                if ((!Directory.Exists(_strReportPath)) || (_strReportPath == ""))
                {
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Report\\"))
                    {
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Report\\");
                    }
                    _strReportPath = AppDomain.CurrentDomain.BaseDirectory + "Report\\";
                }
                if (!_strReportPath.EndsWith("\\"))
                {
                    _strReportPath += "\\";
                }
                return _strReportPath;
            }
            set
            {
                _strReportPath = value;
            }
        }

        private bool _bProtect = false;
        /// <summary>
        /// 是否保护不被修改
        /// </summary>
        public bool bProtect
        {
            set
            {
                _bProtect = value;
            }
        }

        private string _strOperator;

        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator
        {
            get
            {
                return _strOperator;
            }
            set
            {
                _strOperator = value;
            }
        }

        private string _strManuFacturer;

        /// <summary>
        /// 检测单位
        /// </summary>
        public string ManuFacturer
        {
            get
            {
                return _strManuFacturer;
            }
            set
            {
                _strManuFacturer = value;
            }
        }

      

        /// <summary>
        /// 画谱图
        /// </summary>
        /// <param name="bitmap"></param>
        public void DrawSpec(ref Bitmap bitmap ,bool isWithoutWidth)
        {
            SpecEntity tempSpec = new SpecEntity();
            int intSpace = 20;//间距
            Graphics g = Graphics.FromImage(bitmap);
            Brush backBrush = new SolidBrush(Color.White);
            Brush textBrush = new SolidBrush(Color.Black);
            System.Drawing.Font font = new System.Drawing.Font("Tahoma", 10, FontStyle.Regular);
            Pen linePen = new Pen(Color.Black,0.5f);
            Pen borderPen = new Pen(Color.Black, 0.5f);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            g.FillRectangle(backBrush, rect);
            Rectangle rectLine = new Rectangle(1, 1, bitmap.Width - 2, bitmap.Height - 2);
            if (isWithoutWidth)
            {
                rectLine = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                borderPen = new Pen(Color.White, 0.1f);
            }
            
           // Rectangle rectLine = new Rectangle(0, 0, bitmap.Width , bitmap.Height );
            g.DrawRectangle(borderPen, rectLine);
            Size textSize = g.MeasureString("123456", font).ToSize(); //字符串大小
            int wMargin = Convert.ToInt32(2 * bitmap.Width / 100.0);//左右边距
            int hMargin = Convert.ToInt32(bitmap.Height / 100.0) + 2;//上下边距
            int textWidth = textSize.Width;
            int textHeight = textSize.Height;
            specCount = specList.Specs.Length;
            int tempWidth = Convert.ToInt32((bitmap.Width - 2 * wMargin - (specCount - 1) * intSpace - specCount * textWidth) / specCount * 1.0);// 谱图实际宽度
            int tempHeight = bitmap.Height - 2 * (hMargin + textHeight);//谱图实际高度
            for (int n = 1; n <= specCount; n++)
            {
                // pfby 2015 0616 在报告中画谱图，要与软件中谱图界面的放大倍数显示一致
                if (WorkCurveHelper.IsReoprtShowAxias)
                {
                    
                        string[] strSpec = specList.Specs[n - 1].SpecData.Split(',');
                        string newstrSpec = "0,";
                      //  for (int i = ReportTemplateHelper.ReoprtxAxisScale; i < strSpec.Length - 3; i++)
                        for (int i = ReportTemplateHelper.ReoprtxAxisScale; i < ReportTemplateHelper.ReportXAxisScaleMax; i++)
                        {
                            newstrSpec += strSpec[i] + ",";
                        }
                        newstrSpec += "0";//strSpec[strSpec.Length - 1];
                        tempSpec.SpecData = newstrSpec;//specList.Specs[n - 1].SpecData;
                }
                else
                {
                    tempSpec.SpecData = specList.Specs[n - 1].SpecData;
                    ReportTemplateHelper.ReoprtxAxisScale = 0;
                    ReportTemplateHelper.ReportXAxisScaleMax = 4095;
                }
               int  MaxAxisScale = ReportTemplateHelper.ReportXAxisScaleMax;
               int range = MaxAxisScale - ReportTemplateHelper.ReoprtxAxisScale;
                tempSpec.IsSmooth = specList.Specs[n - 1].IsSmooth;
                int[] firstSpecDatas = tempSpec.SpecDatas;          //yuzhao20131029:连测报告谱图为同一个谱图bug fix
                int maxValue = SpecHelper.GetHighSpecValue(firstSpecDatas);//(tempSpec.SpecDatas);//转换字符串为数字取消
                double pixX = tempWidth * 1.0 / firstSpecDatas.Length;//tempSpec.SpecDatas.Length;//取消字符串转化数组// 1600;//取消固定值
                double pixY = tempHeight / (maxValue * 1.0);
                if (maxValue == 0)
                {
                    pixY = 0;
                }
                int orgX = wMargin + n * textWidth + (n - 1) * (tempWidth + intSpace);
                g.DrawLine(linePen, orgX, tempHeight + hMargin + textHeight, orgX + tempWidth, tempHeight + hMargin + textHeight); //画X轴
                g.DrawLine(linePen, orgX, tempHeight + hMargin + textHeight, orgX, hMargin + textHeight);//画Y轴

                int titleW = 0;
                for (int i = 0; i <= 5; i++) //画Y轴刻度
                {
                    int k = (int)Math.Round(maxValue * (5 - i) / 5.0);
                    titleW = g.MeasureString(k.ToString(), font).ToSize().Width;
                    int y = hMargin + textHeight + (int)Math.Round(tempHeight * i / 5.0);
                    g.DrawLine(linePen, orgX, y, orgX - 4, y);
                    g.DrawString(k.ToString(), font, textBrush, orgX - 4 - titleW, y - textHeight / 2);
                }

                Point[] point = new Point[firstSpecDatas.Length]; ;
                for (int i = 0; i < firstSpecDatas.Length; i++)
                {
                    point[i].X = orgX + Convert.ToInt32(Math.Round(pixX * i));
                    point[i].Y = hMargin + textHeight + tempHeight - Convert.ToInt32(Math.Round(pixY * ((i >= firstSpecDatas.Length) ?
                        0 : firstSpecDatas[i])));
                    if (point[i].Y < hMargin + textHeight)
                    {
                        point[i].Y = hMargin + textHeight;
                    }

                    if (WorkCurveHelper.IsReoprtShowAxias)
                    {

                        if ((i + ReportTemplateHelper.ReoprtxAxisScale) % ReportTemplateHelper.ReportXStep == 0)
                        {
                            g.DrawLine(linePen, point[i].X, hMargin + textHeight + tempHeight, point[i].X, hMargin + textHeight + tempHeight + 4);
                            titleW = g.MeasureString((i + ReportTemplateHelper.ReoprtxAxisScale).ToString(), font).ToSize().Width / 2;
                            g.DrawString((i + ReportTemplateHelper.ReoprtxAxisScale).ToString(), font, textBrush, point[i].X - titleW, hMargin + textHeight + tempHeight + 4);
                        }
                    }
                    else
                    {
                        if ((i + ReportTemplateHelper.ReoprtxAxisScale) % 500 == 0)
                        {

                            g.DrawLine(linePen, point[i].X, hMargin + textHeight + tempHeight, point[i].X, hMargin + textHeight + tempHeight + 4);
                            titleW = g.MeasureString((i + ReportTemplateHelper.ReoprtxAxisScale).ToString(), font).ToSize().Width / 2;
                            g.DrawString((i + ReportTemplateHelper.ReoprtxAxisScale).ToString(), font, textBrush, point[i].X - titleW, hMargin + textHeight + tempHeight + 4);

                        }
                        else if (i % 100 == 0 && i > 0)
                        {
                            g.DrawLine(linePen, point[i].X, hMargin + textHeight + tempHeight, point[i].X + 2, hMargin + textHeight + tempHeight);

                        }
                    }
                }
                //g.FillPolygon(backBrush, point);
                g.DrawPolygon(linePen, point);

                // pfby 20150616 报告中显示定性分析元素
                if (WorkCurveHelper.IsReoprtShowQualityElem )
                {
                    if (!ReportTemplateHelper.IsSpecShowElem) return;
                    if (WorkCurveHelper.Atoms == null) return;
                    if (WorkCurveHelper.Atoms.Length < 1) return;
                    /// pfby 2015-0616 画感兴趣元素
                    int startID = 0;
                    int endID = 0;
                    startID = 0;
                    endID = WorkCurveHelper.Atoms.Length - 1;
                    string[] lines = { "Ka", "Kb", "La", "Lb", "Lr", "Le" };
                    titleW = g.MeasureString("Ag", font).ToSize().Width / 2;
                    for (int i = startID; i <= endID; i++)
                    {
                        XLine line = (XLine)WorkCurveHelper.Lines[i];
                        if (line == XLine.Ka || line == XLine.Kb)
                        {
                            Atom atom = Atoms.GetAtom(WorkCurveHelper.Atoms[i].ToString());
                            int KaChannel = DemarcateEnergyHelp.GetChannel(Atoms.GetEnergyByXLine(XLine.Ka, atom));// -tempLeft;
                            int KbChannel = DemarcateEnergyHelp.GetChannel(Atoms.GetEnergyByXLine(XLine.Kb, atom));// -tempLeft;
                            if (KaChannel - ReportTemplateHelper.ReoprtxAxisScale >= 0)
                            {
                                g.DrawLine(new Pen(Color.Black), point[KaChannel - ReportTemplateHelper.ReoprtxAxisScale].X, hMargin + textHeight + tempHeight, point[KaChannel - ReportTemplateHelper.ReoprtxAxisScale].X, hMargin + textHeight);
                                g.DrawString(WorkCurveHelper.Atoms[i].ToString() + "-" + "Ka", font, new SolidBrush(Color.Red), point[KaChannel - ReportTemplateHelper.ReoprtxAxisScale].X - titleW, hMargin + textHeight);
                            }
                            if (KbChannel - ReportTemplateHelper.ReoprtxAxisScale >= 0)
                            {
                                g.DrawLine(new Pen(Color.Black), point[KbChannel - ReportTemplateHelper.ReoprtxAxisScale].X, hMargin + textHeight + tempHeight, point[KbChannel - ReportTemplateHelper.ReoprtxAxisScale].X, hMargin + textHeight);
                                g.DrawString(WorkCurveHelper.Atoms[i].ToString() + "-" + "Kb", font, new SolidBrush(Color.Red), point[KbChannel - ReportTemplateHelper.ReoprtxAxisScale].X - titleW, hMargin + textHeight);
                            }
                        }
                        else
                        {
                            Atom atom = Atoms.GetAtom(WorkCurveHelper.Atoms[i].ToString());
                            int LaChannel = DemarcateEnergyHelp.GetChannel(Atoms.GetEnergyByXLine(XLine.La, atom));// -tempLeft;
                            int LbChannel = DemarcateEnergyHelp.GetChannel(Atoms.GetEnergyByXLine(XLine.Lb, atom));// -tempLeft;
                            if (LaChannel - ReportTemplateHelper.ReoprtxAxisScale >= 0)
                            {
                                g.DrawLine(new Pen(Color.Black), point[LaChannel - ReportTemplateHelper.ReoprtxAxisScale].X, hMargin + textHeight + tempHeight, point[LaChannel - ReportTemplateHelper.ReoprtxAxisScale].X, hMargin + textHeight);
                                g.DrawString(WorkCurveHelper.Atoms[i].ToString() + "-" + "La", font, new SolidBrush(Color.Red), point[LaChannel - ReportTemplateHelper.ReoprtxAxisScale].X - titleW, hMargin + textHeight);

                            }
                            if (LbChannel - ReportTemplateHelper.ReoprtxAxisScale >= 0)
                            {
                                g.DrawLine(new Pen(Color.Black), point[LbChannel - ReportTemplateHelper.ReoprtxAxisScale].X, hMargin + textHeight + tempHeight, point[LbChannel - ReportTemplateHelper.ReoprtxAxisScale].X, hMargin + textHeight);
                                g.DrawString(WorkCurveHelper.Atoms[i].ToString() + "-" + "Lb", font, new SolidBrush(Color.Red), point[LbChannel - ReportTemplateHelper.ReoprtxAxisScale].X - titleW, hMargin + textHeight);
                            }
                        }

                    }

                }
                // pfby 20150616  end
                else
                {
                    ///  --------
                    //  以下画感兴趣元素
                    int startID = 0;
                    int endID = 0;
                    startID = 0;
                    endID = InterestElemCount - 1;
                    titleW = g.MeasureString("Ag", font).ToSize().Width / 2;
                    for (int i = startID; i <= endID; i++)
                    {
                        if (Elements.Items[i].PeakHigh > Elements.Items[i].PeakLow)
                        {
                            int l = Elements.Items[i].PeakLow;
                            int r = Elements.Items[i].PeakHigh;
                            int w = r - l;
                            Point[] boundPoints = new Point[w + 3];
                            for (int j = l; j <= r; j++)
                            {
                                boundPoints[j - l].X = point[j].X;
                                boundPoints[j - l].Y = point[j].Y;
                            }
                            boundPoints[w + 1].X = boundPoints[w].X;
                            boundPoints[w + 1].Y = hMargin + textHeight + tempHeight;
                            boundPoints[w + 2].X = boundPoints[0].X;
                            boundPoints[w + 2].Y = boundPoints[w + 1].Y;
                            g.FillPolygon(new SolidBrush(Color.FromArgb(Elements.Items[i].Color)), boundPoints);
                            int peakCh = (l + r) / 2;
                            if (peakCh < ReportTemplateHelper.ReoprtxAxisScale) return;
                            if (WorkCurveHelper.SpecType == 0)
                                g.DrawString(Elements.Items[i].Caption, font, new SolidBrush(Color.FromArgb(Elements.Items[i].Color)), point[peakCh- ReportTemplateHelper.ReoprtxAxisScale].X - titleW, hMargin + textHeight + tempHeight + 3);
                            else if (WorkCurveHelper.SpecType == 1)
                            {
                                g.DrawLine(new Pen(Color.Black), point[peakCh - ReportTemplateHelper.ReoprtxAxisScale].X, hMargin + textHeight + tempHeight, point[peakCh - ReportTemplateHelper.ReoprtxAxisScale].X, hMargin + textHeight + 13);
                                g.DrawString(Elements.Items[i].Caption, font, new SolidBrush(Color.FromArgb(Elements.Items[i].Color)), point[peakCh- ReportTemplateHelper.ReoprtxAxisScale].X - titleW, hMargin + textHeight);
                            }
                        }
                    }
                }
            }
          }





        /// <summary>
        /// 清空点数据
        /// </summary>
        /// <param name="p"></param>
        private void ClearPoint(ref Point[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                p[i].X = 0;
                p[i].Y = 0;
            }
        }

        /// <summary>
        /// 画感兴趣元素图
        /// </summary>
        /// <param name="bitmap"></param>
        public void DrawInterstringElems(ref Bitmap bitmap)
        {
            SpecEntity tempSpec = new SpecEntity();
            int maxCh = 500;
            Graphics g = Graphics.FromImage(bitmap);
            Brush backBrush = new SolidBrush(Color.White);
            System.Drawing.Font font = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
            Pen linePen = new Pen(Color.Black);
            Brush textBrush = new SolidBrush(Color.Black);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            g.FillRectangle(backBrush, rect);
            Rectangle rectLine = new Rectangle(1, 1, bitmap.Width - 2, bitmap.Height - 2);
            g.DrawRectangle(linePen, rectLine);
            Size textSize = g.MeasureString("123456", font).ToSize(); //字符串大小
            int wMargin = Convert.ToInt32(2 * bitmap.Width / 100.0);//左右边距
            int hMargin = Convert.ToInt32(bitmap.Height / 100.0);//上下边距
            int textWidth = textSize.Width;
            int textHeight = textSize.Height;

            int tempWidth = (bitmap.Width - 2 * wMargin) / (int)Math.Round((InterestElemCount + 0.2) / 2);//根据元素个数分列
            int tempHeight = bitmap.Height / 2;
            tempHeight -= 2 * hMargin;
            int maxH = tempHeight - 2 * textHeight;
            int tempY = hMargin + textHeight;
            Point[] p = new Point[maxCh];
            Point[] pp = new Point[maxCh];
            int tempLeft = 0;
            int tempRight = 0;
            int row = 0;
            int col = 0;
            int tempCol = 0;
            int x = 0;
            int y = 0;
            int titleW = 0;
            string tempEnergy = string.Empty;
            double pixX = 0f;
            double pixY = 0f;
            int[] maxValue = new int[InterestElemCount];//每个感兴趣元素区域的最大值
            int Elemi = -1;
            //标识峰的线系 0722
            string[] lines = { "Ka", "Kb", "La", "Lb", "Lr", "Le" };
            for (int i = 0; i < InterestElemCount; i++)
            {
                Elemi++;//修改隐藏元素不显示其谱图
                if (Elemi >= Elements.Items.Count) break;
                if (Elements.Items[Elemi].PeakLow == 0 || !Elements.Items[Elemi].IsShowElement)
                {
                    i--;
                    continue;
                }
                XLine line = Elements.Items[Elemi].AnalyteLine;
                Atom atom = Atoms.GetAtom(Elements.Items[Elemi].Caption);
                ClearPoint(ref p);
                if (specCount > 1) //连测则有2个谱
                {
                    //if (i <= curElemCount)
                    //{
                    //    tempSpec.SpecData = Spec.SpecData;
                    //    tempSpec.IsSmooth = Spec.IsSmooth;
                    //}
                    //else
                    //{
                    //    //修改：何晓明 20110831 dataFountain.specList.Specs.Count == 0 导致unitSpec为null
                    //    if (unitSpec != null)
                    //    //
                    //    {
                    //        tempSpec.SpecData = unitSpec.SpecData;
                    //        tempSpec.IsSmooth = unitSpec.IsSmooth;
                    //    }
                    //}
                    tempSpec.SpecData = specList.Specs[Elements.Items[Elemi].ConditionID].SpecData;
                    tempSpec.IsSmooth = specList.Specs[Elements.Items[Elemi].ConditionID].IsSmooth;
                }
                else
                {
                    tempSpec.SpecData = Spec.SpecData;
                    tempSpec.IsSmooth = Spec.IsSmooth;
                }
                if (WorkCurveHelper.SpecType == 0)
                {
                    tempLeft = Elements.Items[Elemi].PeakLow - OFFSET;
                    tempRight = Elements.Items[Elemi].PeakHigh + OFFSET;
                }
                else
                {
                    if (line == XLine.Ka || line == XLine.Kb)
                    {
                        tempLeft = DemarcateEnergyHelp.GetChannel(atom.Ka) - OFFSET;
                        tempRight = DemarcateEnergyHelp.GetChannel(atom.Kb) + OFFSET;

                    }
                    else
                    {
                        tempLeft = DemarcateEnergyHelp.GetChannel(atom.La) - OFFSET -10;
                        tempRight = DemarcateEnergyHelp.GetChannel(atom.Lb) + OFFSET;

                    }
                }
                //数组越界的错误控制
                tempLeft = tempLeft < 0 ? 0 : tempLeft;
                tempRight = tempRight >= Spec.SpecData.Length ? (Spec.SpecData.Length - 1) : tempRight;
                //获取每个感兴趣元素区域的最大值
                try
                {
                    maxValue[i] = tempSpec.SpecDatas[tempLeft];
                }
                catch
                {
                    //    MessageBoxEx.Show(Info.PeakSetError, Info.Hint, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;
                }

                //var queryMax = from cust in tempSpec.SpecDatas where cust == "London" orderby cust.Name ascending select cust;

                int iMaxValue = tempSpec.SpecDatas.ToList().GetRange(tempLeft, tempRight - tempLeft).Max();
                if (iMaxValue > maxValue[i]) maxValue[i] = iMaxValue;


                if (maxValue[i] < 5)
                {
                    maxValue[i] = 5;
                }

                //判断元素属于哪一行、列
                if (i >= InterestElemCount / 2.0)
                {
                    row = 1;
                    col = i - (int)Math.Round((InterestElemCount + 0.2) / 2);
                }
                else
                {
                    row = 0;
                    col = i;
                }



                tempCol = col * tempWidth;
                pixX = tempWidth / (maxCh * 1.0);
                pixY = maxH * 1.0 / maxValue[i];

                //画Y轴
                x = wMargin + textWidth + tempCol;
                y = tempY + maxH + tempHeight * row;
                g.DrawLine(linePen, x, hMargin + tempHeight * row, x, y);

                //画X轴
                g.DrawLine(linePen, x, y, x + tempWidth - textWidth, y);
                g.DrawString("KeV", font, textBrush, x + tempWidth - textWidth - 20, y + 4);

                for (int k = 0; k <= 3; k++)
                {
                    int m = (int)Math.Round(maxValue[i] * (3 - k) / 3.0);
                    titleW = g.MeasureString(m.ToString(), font).ToSize().Width;
                    y = tempY + row * tempHeight + (int)Math.Round(maxH * k / 3.0);
                    g.DrawLine(linePen, x, y, x - 4, y);
                    g.DrawString(m.ToString(), font, textBrush, x - 4 - titleW, y - textHeight / 2);
                }
                int count = 0;
                for (int j = 0; j < maxCh; j++)
                {
                    if ((j <= tempRight - tempLeft))
                    {
                        p[j].X = x + (int)Math.Round(pixX * j);
                        p[j].Y = y - (int)Math.Round(pixY * tempSpec.SpecDatas[j + tempLeft]);
                    }
                    else
                    {
                        p[j].X = x + (int)Math.Round(pixX * (tempRight - tempLeft));
                        p[j].Y = y;
                    }
                    if (p[j].Y < tempY + row * tempHeight)
                    {
                        p[j].Y = tempY + row * tempHeight;
                    }
                    if ((j % 50 == 0) && (j <= tempRight - tempLeft) && (count < 5))
                    {
                        count++;
                        g.DrawLine(linePen, p[j].X, y, p[j].X, y + 4);
                        tempEnergy = DemarcateEnergyHelp.GetEnergy(j + tempLeft).ToString("f1"); //通道转化为能量
                        titleW = g.MeasureString(tempEnergy, font).ToSize().Width / 2;
                        g.DrawString(tempEnergy, font, textBrush, p[j].X - titleW, y + 4); ;
                    }
                }

                //设置起始端Y值相等
                p[0].Y = y;
                p[1].Y = y;
                p[0].X = p[1].X;
                p[maxCh - 1].Y = y;

                g.DrawPolygon(linePen, p); //画谱

                titleW = g.MeasureString("Ag-Ka", font).ToSize().Width / 2;
                //int l = IsPreview ? Elements.Items[i].PeakLow : kbChannel - middel;
                //int r = IsPreview ? Elements.Items[i].PeakHigh : kbChannel + middel;
                if (WorkCurveHelper.SpecType == 0)
                {
                    int l = Elements.Items[Elemi].PeakLow;
                    int r = Elements.Items[Elemi].PeakHigh;
                    int w = r - l;
                    Point[] boundPoints = new Point[w + 3];
                    for (int j = 0; j <= w; j++)
                    {
                        boundPoints[j].X = p[j + OFFSET].X;
                        boundPoints[j].Y = p[j + OFFSET].Y;
                    }
                    boundPoints[w + 1].X = boundPoints[w].X;
                    boundPoints[w + 1].Y = y;
                    boundPoints[w + 2].X = boundPoints[0].X;
                    boundPoints[w + 2].Y = boundPoints[w + 1].Y;


                    g.FillPolygon(new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), boundPoints);
                }
                ////if (WorkCurveHelper.SpecType == 0)
                ////{
                ////    int index = (int)line;
                ////    int peakCh = w / 2 + OFFSET;
                ////    g.DrawString(Elements.Items[Elemi].Caption + "-" + lines[index], font, new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), p[peakCh].X - titleW, p[peakCh].Y - 16);
                ////}
                 //Atom atom = Atoms.GetAtom(Elements.Items[Elemi].Caption);

                 if (WorkCurveHelper.SpecType == 0)
                 {
                     int index = (int)line;
                     int peakCh = DemarcateEnergyHelp.GetChannel(Atoms.GetEnergyByXLine(line, atom)) - tempLeft;
                     g.DrawString(Elements.Items[Elemi].Caption + "-" + lines[index], font, new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), p[peakCh].X - titleW, p[peakCh].Y - 16);
                 }
                 else
                 {
                     int Length = 40;
                     Point[] boundPoints = new Point[Length + 3];
                     if (line == XLine.Ka || line == XLine.Kb)
                     {
                         int ac = Math.Abs(DemarcateEnergyHelp.GetChannel(atom.Ka) - tempLeft) < maxCh - Length/2 ? Math.Abs(DemarcateEnergyHelp.GetChannel(atom.Ka) - tempLeft) : maxCh - Length/2 - 1;
                         int bc = Math.Abs(DemarcateEnergyHelp.GetChannel(atom.Kb) - tempLeft) < maxCh - Length/2 ? Math.Abs(DemarcateEnergyHelp.GetChannel(atom.Kb) - tempLeft) : maxCh -Length/2 -1;
                         for (int k = 0; k <= Length; k++)
                         {
                             boundPoints[k].X = p[k + ac - Length/2].X;
                             boundPoints[k].Y = p[k + ac - Length / 2].Y;
                         }
                         boundPoints[Length + 1].X = boundPoints[Length].X;
                         boundPoints[Length + 1].Y = y;
                         boundPoints[Length + 2].X = boundPoints[0].X;
                         boundPoints[Length + 2].Y = boundPoints[Length + 1].Y;
                         g.FillPolygon(new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), boundPoints);
                         g.DrawLine(new Pen(Color.Black), p[ac].X, hMargin + tempHeight * (row + 1) -15, p[ac].X, hMargin + tempHeight * row + 12);
                         g.DrawString(Elements.Items[Elemi].Caption + "-" + "Ka", font, new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), p[ac].X - titleW, hMargin + tempHeight * row);

                         for (int j = 0; j <= Length; j++)
                         {
                             boundPoints[j].X = p[j + bc - Length / 2].X;
                             boundPoints[j].Y = p[j + bc - Length / 2].Y;
                         }
                         boundPoints[Length + 1].X = boundPoints[Length].X;
                         boundPoints[Length + 1].Y = y;
                         boundPoints[Length + 2].X = boundPoints[0].X;
                         boundPoints[Length + 2].Y = boundPoints[Length + 1].Y;
                         g.FillPolygon(new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), boundPoints);
                         g.DrawLine(new Pen(Color.Black), p[bc].X, hMargin + tempHeight * (row + 1) - 15, p[bc].X, hMargin + tempHeight * row + 12);
                         g.DrawString(Elements.Items[Elemi].Caption + "-" + "Kb", font, new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), p[bc].X - titleW, hMargin + tempHeight * row);

                       
                     }
                     else
                     {
                         int lac = Math.Abs(DemarcateEnergyHelp.GetChannel(atom.La) - tempLeft) < maxCh - Length / 2 ? Math.Abs(DemarcateEnergyHelp.GetChannel(atom.La) - tempLeft) : maxCh - Length / 2 - 1;
                         int lbc = Math.Abs(DemarcateEnergyHelp.GetChannel(atom.Lb) - tempLeft) < maxCh - Length / 2 ? Math.Abs(DemarcateEnergyHelp.GetChannel(atom.Lb) - tempLeft) : maxCh - Length / 2 - 1;
                         for (int k = 0; k <= Length; k++)
                         {
                             boundPoints[k].X = p[k + lac - Length / 2].X;
                             boundPoints[k].Y = p[k + lac - Length / 2].Y;
                         }
                         boundPoints[Length + 1].X = boundPoints[Length].X;
                         boundPoints[Length + 1].Y = y;
                         boundPoints[Length + 2].X = boundPoints[0].X;
                         boundPoints[Length + 2].Y = boundPoints[Length + 1].Y;
                         g.FillPolygon(new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), boundPoints);
                         g.DrawLine(new Pen(Color.Black), p[lac].X, hMargin + tempHeight * (row + 1) - 15, p[lac].X, hMargin + tempHeight * row + 12);
                         g.DrawString(Elements.Items[Elemi].Caption + "-" + "La", font, new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), p[lac].X - titleW, hMargin + tempHeight * row);

                         for (int j = 0; j <= Length; j++)
                         {
                             boundPoints[j].X = p[j + lbc - Length / 2].X;
                             boundPoints[j].Y = p[j + lbc - Length / 2].Y;
                         }
                         boundPoints[Length + 1].X = boundPoints[Length].X;
                         boundPoints[Length + 1].Y = y;
                         boundPoints[Length + 2].X = boundPoints[0].X;
                         boundPoints[Length + 2].Y = boundPoints[Length + 1].Y;
                         g.FillPolygon(new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), boundPoints);
                         g.DrawLine(new Pen(Color.Black), p[lbc].X, hMargin + tempHeight * (row + 1) - 15, p[lbc].X, hMargin + tempHeight * row + 12);
                         g.DrawString(Elements.Items[Elemi].Caption + "-" + "Lb", font, new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), p[lbc].X - titleW, hMargin + tempHeight * row);
  
                      
                     }
                 }

                //if (line == XLine.Ka || line == XLine.Kb)
                //{
                //    int ac = DemarcateEnergyHelp.GetChannel(atom.Ka);
                //    int bc = DemarcateEnergyHelp.GetChannel(atom.Kb);
                //    if (WorkCurveHelper.SpecType == 1)
                //    {
                //        g.DrawLine(new Pen(Color.Black), p[ac].X, p[ac].Y, p[ac].X, 0);
                //        g.DrawString(Elements.Items[Elemi].Caption + "-" + "Ka", font, new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), p[ac].X - titleW, p[ac].Y - 16);

                //        g.DrawLine(new Pen(Color.Black), p[bc].X, p[bc].Y, p[bc].X, 0);
                //        g.DrawString(Elements.Items[Elemi].Caption + "-" + "Kb", font, new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), p[bc].X - titleW, p[bc].Y - 16);
                //    }
                //    else if (WorkCurveHelper.SpecType == 0)
                //    {
                //        int index = (int)line;
                //        int peakCh = DemarcateEnergyHelp.GetChannel(Atoms.GetEnergyByXLine(line, atom));
                //        g.DrawString(Elements.Items[Elemi].Caption + "-" + lines[index], font, new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), p[peakCh].X - titleW, p[peakCh].Y - 16);
                //    }
                //}
                //else
                //{
                //    int lac = DemarcateEnergyHelp.GetChannel(atom.La);
                //    int lbc = DemarcateEnergyHelp.GetChannel(atom.Lb);
                //    if (WorkCurveHelper.SpecType == 1)
                //    {
                //        g.DrawLine(new Pen(Color.Black), p[lac].X, p[lac].Y, p[lac].X, 0);
                //        g.DrawString(Elements.Items[Elemi].Caption + "-" + "La", font, new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), p[lac].X - titleW, p[lac].Y - 16);

                //        g.DrawLine(new Pen(Color.Black), p[lbc].X, p[lbc].Y, p[lbc].X, 0);
                //        g.DrawString(Elements.Items[Elemi].Caption + "-" + "Lb", font, new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), p[lbc].X - titleW, p[lbc].Y - 16);
                //    }
                //    else if (WorkCurveHelper.SpecType == 0)
                //    {
                //        int index = (int)line;
                //        int peakCh = DemarcateEnergyHelp.GetChannel(Atoms.GetEnergyByXLine(line, atom));
                //        g.DrawString(Elements.Items[Elemi].Caption + "-" + lines[index], font, new SolidBrush(Color.FromArgb(Elements.Items[Elemi].Color)), p[peakCh].X - titleW, p[peakCh].Y - 16);
                //    }
                //}
            }
        }


        /// <summary>
        /// 随机生成密码
        /// </summary>
        /// <returns></returns>
        protected string GetPassWord()
        {
            string strResult = string.Empty;
            Random rndPassWord = new Random();
            for (int i = 0; i < 10; i++)
            {
                strResult += rndPassWord.Next(9).ToString();
            }
            return strResult;
        }

        /// <summary>
        /// 生成报告
        /// </summary>
        /// <param name="fileName">保存文件名，为空时直接打印</param>
        public  string SelfCheckReport(string fileName, bool flag, SelfCheckObject SelfCheckObject)
        {
            string StrSavePath=string.Empty;
            try
            {
                if (!System.IO.File.Exists(TempletFileName))
                {
                    Msg.Show(Info.TemplateNoExists);
                    return StrSavePath;
                }
                Workbook wb = new Workbook(FileFormatType.Excel97To2003);
                wb.Open(TempletFileName);  
                Worksheet ws = wb.Worksheets[0];
                //MarkEmptyRow(excelReport.excelWorksheet);
                ReplaceCellText(ref ws,"%HVLock%", SelfCheckObject.HVLock);
                ReplaceCellText(ref ws,"%Collimator%", SelfCheckObject.Collimator);
                ReplaceCellText(ref ws,"%Filter%", SelfCheckObject.Filter);
                ReplaceCellText(ref ws,"%HVoltage%", SelfCheckObject.HVoltage);
                ReplaceCellText(ref ws,"%Resolve%",  SelfCheckObject.Resolve);
                ReplaceCellText(ref ws,"%Peak%", SelfCheckObject.Peak);

                ReplaceCellText(ref ws,"%Pump%",  SelfCheckObject.Pump);
                ReplaceCellText(ref ws,"%Operator%", SelfCheckObject.Operator);
                ReplaceCellText(ref ws,"%Date%",  SelfCheckObject.Date);
                ReplaceCellText(ref ws,"%Times%",SelfCheckObject.SpecTimes);
                ReplaceCellText(ref ws,"%CollimatorId%",  SelfCheckObject.CollimatorId);
                ReplaceCellText(ref ws,"%FilterId%",  SelfCheckObject.FilterId);
                ReplaceCellText(ref ws, "%Voltage%", SelfCheckObject.Voltage);
                ReplaceCellText(ref ws, "%Current%", SelfCheckObject.Current);
                ReplaceCellText(ref ws,"%Result%",  SelfCheckObject.Result);
                ReplaceCellText(ref ws,"%CountRate%",SelfCheckObject.CountRate);
                ReplaceCellText(ref ws,"%HalfWidth%", SelfCheckObject.HalfWidth);
                ReplaceCellText(ref ws,"%PeakSecChannel%", SelfCheckObject.PeakSec);
  
                //填充谱图
                Cell  cell = FindTextCell(ws, "%Spectrum%");
                if (cell != null)
                {
                    Bitmap bmp = new Bitmap(640, 160);
                    // Graphics g = Graphics.FromImage(bmp);
                    DrawSpec(ref bmp,false);
                    ReplaceCellImage(ref ws, "%Spectrum%", bmp);
                }

                if (flag)
                {
                    switch (ReportFileType)
                        {
                            case 2:
                                StrSavePath = ReportPath + fileName + ".csv";
                                wb.Save(StrSavePath, Aspose.Cells.SaveFormat.CSV);
                                break;
                            case 1:
                                StrSavePath =ReportPath + fileName + ".pdf";
                                Aspose.Cells.CellsHelper.FontDir = System.Environment.GetEnvironmentVariable("windir") + "\\Fonts";
                                wb.Save(StrSavePath, Aspose.Cells.SaveFormat.Pdf);
                                break;
                            case 0:
                            default:
                                StrSavePath = ReportPath  + fileName + ".xls";
                                wb.Save(StrSavePath, SaveFormat.Excel97To2003);
                                break;
                        };
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
               // excelReport.CloseFile();
            }
            catch (Exception ce)
            {
                Msg.Show(ce.Message);
            }
            finally
            {
              
            }
            return StrSavePath;
        }
      
        protected string sTotalResult = "";



        /// <summary>
        /// 生成报告
        /// </summary>
        /// <param name="fileName">保存文件名，为空时直接打印</param>

        public virtual string GenerateReport(string fileName,bool flag)
        {
                 try
                {
                    if (!System.IO.File.Exists(TempletFileName))
                    {
                        Msg.Show(Info.TemplateNoExists);
                        return string.Empty;
                    }
                    Workbook wb = new Workbook(FileFormatType.Excel97To2003);
                    wb.Open(TempletFileName);  
                    Worksheet ws = wb.Worksheets[0];  
                    ReplaceCellText(ref ws, "%Operator%", operateMember);
                    if (specList != null)
                    {
                        ReplaceCellText(ref ws, "%SampleName%", specList.SampleName);
                        ReplaceCellText(ref ws, "%SpecName%", this.specList.Name);
                        ReplaceCellText(ref ws, "%Supplier%", specList.Supplier);
                        ReplaceCellText(ref ws,"%TestTime%",specList.Specs[0].UsedTime.ToString()+"(s)");//待调试
                        ReplaceCellText(ref ws, "%Voltage%", specList.Specs[0].TubVoltage.ToString("f0") + "(KV)");
                        ReplaceCellText(ref ws, "%Current%", specList.Specs[0].TubCurrent.ToString("f0") + "(μA)");
                        ReplaceCellText(ref ws, "%TestDate%", ((DateTime)specList.SpecDate).ToString());
                        ReplaceCellText(ref ws, "%TestShortDate%", ((DateTime)specList.SpecDate).ToShortDateString());
                        ReplaceCellText(ref ws, "%TestShortDateFormat%", ((DateTime)specList.SpecDate).ToString("yyyy-MM-dd"));
                        ReplaceCellText(ref ws, "%TestShortTime%", ((DateTime)specList.SpecDate).ToShortTimeString());
                        ReplaceCellText(ref ws, "%ReportDate%", DateTime.Now.ToShortDateString());
                        ReplaceCellText(ref ws, "%Shape%", this.specList.Shape);
                        ReplaceCellText(ref ws, "%Description%", this.specList.SpecSummary);
                        if (FindTextCell(ws, "%SamplePhoto%")!=null && specList.ImageShow)
                        {
                            string fileNameFull = WorkCurveHelper.SaveSamplePath + "\\" + specList.Name + ".jpg";
                            FileInfo infoIf = new FileInfo(fileNameFull);
                            if (infoIf.Exists)
                            {
                                Bitmap tempImage = (Bitmap)Image.FromFile(fileNameFull);
                                ReplaceCellImage(ref ws, "%SamplePhoto%", tempImage);
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

                    //待调试
                    //if (Spec != null)
                    //{
                    //    ReplaceCellText(ref ws, "%TestTime%", Spec.UsedTime + "(s)");
                    //    ReplaceCellText(ref ws, "%Voltage%", Spec.DeviceParameter!=null ? Spec.DeviceParameter.TubVoltage + "(KV)":"");
                    //    ReplaceCellText(ref ws, "%Current%", Spec.DeviceParameter!=null ? Spec.DeviceParameter.TubCurrent + "(μA)":"");
                    //    ReplaceCellText(ref ws, "%TestTime%", Spec.UsedTime + "(s)");
                    //}

                    if (Spec != null)
                    {
                        string filterElement = (WorkCurveHelper.DeviceCurrent.Filter.Count > 0 && Spec.DeviceParameter.FilterIdx > 0) ? "(" + WorkCurveHelper.DeviceCurrent.Filter[Spec.DeviceParameter.FilterIdx-1].Caption + ")" : "";
                        ReplaceCellText(ref ws, "%FilterIdx%", Spec.DeviceParameter.FilterIdx + filterElement);
                        filterElement = (WorkCurveHelper.DeviceCurrent.Collimators.Count > 0 && Spec.DeviceParameter.CollimatorIdx > 0) ? "(" + WorkCurveHelper.DeviceCurrent.Collimators[Spec.DeviceParameter.CollimatorIdx-1].Diameter + "mm)" : "";
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
                    //var Au = (from l in Elements.Items where string.Compare(l.Caption, "Au", true) == 0 select l.Content).FirstOrDefault();
                    //double dKValue = Au * 24 / 99.995;
                    //ReplaceCellText(ref ws, "%Karat%", dKValue.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(k)");
                    CurveElement ceTemp = Elements.Items.ToList().Find(l => l.Caption.ToLower().Equals("au"));
                    if (ceTemp != null)
                    {
                        //var Au = (from l in Elements.Items where string.Compare(l.Caption, "Au", true) == 0 select l.Content).FirstOrDefault();
                        double dKValue = ceTemp.Content * 24 / WorkCurveHelper.KaratTranslater;
                        ReplaceCellText(ref ws, "%Karat%", dKValue.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(k)");

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
                     cell = FindTextCell(ws, "%Spectrum%",out width,out height);
                    if (cell != null)
                    {
                        if (!WorkCurveHelper.ReportSaveScreen)
                        {
                            //Bitmap bmp = new Bitmap(120, 60);
                            Bitmap bmp = new Bitmap(640, 160);
                            DrawSpec(ref bmp,false);
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
                            DrawSpec(ref bmp,true);
                            ReplaceCellImage(ref ws, "%SpectrumWithOutWidth%", bmp);
                        }
                        else
                        {
                            string fileNameFull = Application.StartupPath + "\\Screenshots\\" + specList.Name + ".jpg";
                            FileInfo infoIf = new FileInfo(fileNameFull);
                            if (infoIf.Exists)
                            {
                                Bitmap tempImage = (Bitmap)Image.FromFile(fileNameFull);
                                ReplaceCellImage(ref ws, "%SpectrumWithOutWidth%", tempImage);
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
                        ws.Protect(ProtectionType.All,strPassword,"");
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
                                StrSavePath = ReportPath  + fileName + ".pdf";
                                Aspose.Cells.CellsHelper.FontDir = System.Environment.GetEnvironmentVariable("windir") + "\\Fonts";
                                wb.Save(StrSavePath, Aspose.Cells.SaveFormat.Pdf);
                                break;
                            case 0:
                            default:
                                StrSavePath = ReportPath  + fileName + ".xls";
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
                catch(Exception ce)
                { 
                Msg.Show(ce.Message);
                }
                finally
                {
                }
                return string.Empty;
        }  //单次测量

        public string GenerateRepeaterReportBrass(string fileName, ContextMenuStrip contextMenuStrip, bool flag, List<long> selectLong)//(黄铜)每次测量一个表格
        {
               string StrSavePath=string.Empty;
                if (!System.IO.File.Exists(TempletFileName))
                {
                    Msg.Show(Info.TemplateNoExists);
                    return StrSavePath;
                }
                Workbook wb = new Workbook(FileFormatType.Excel97To2003);
                wb.Open(TempletFileName);  
                Worksheet ws=wb.Worksheets[0];
                string workCurveName = string.Empty;
                SpecEntity spec = new SpecEntity();
                SpecListEntity tempList = new SpecListEntity();
                List<string> SeleWorkCurveNameList = new List<string>();
                object[,] data = ws.Cells.ExportArray(0,0,ws.Cells.Rows.Count,ws.Cells.Columns.Count);
                ExportTableOptions option = new ExportTableOptions();
                option.CheckMixedValueType = false;
                option.SkipErrorValue = true;
                DataTable dataCopy= ws.Cells.ExportDataTable(0, 0, ws.Cells.Rows.Count, ws.Cells.Columns.Count, option);
                Range RangStyle = ws.Cells.CreateRange(0, 0, ws.Cells.Rows.Count, ws.Cells.Columns.Count);
                Range RangTem = null;
                if (selectLong.Count > 1)
                {
                    ws = wb.Worksheets.Add("report");
                    ws.Cells.ImportDataTable(dataCopy, false, ws.Cells[0, 0].Name);
                    RangTem= ws.Cells.CreateRange(0, 0, RangStyle.RowCount, RangStyle.ColumnCount);
                    RangTem.CopyStyle(RangStyle);
                }
                
                int rowCount =ws.Cells.Rows.Count;
                int columnCount = ws.Cells.Columns.Count;
                int StartIndex = 0; 
                for (int iSele = 0; iSele < selectLong.Count; iSele++)
                {
                    #region 初始化 Report
                    ElementList elementList = ElementList.New;
                    HistoryRecord record = HistoryRecord.FindById(selectLong[iSele]);
                    if (record == null)
                        continue;
                    WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                    if (workCurve == null)
                        continue;

                    WorkCurveID = workCurve.Id.ToString();
                    workCurveName = workCurve.Name;
                    historyRecordid = selectLong[iSele].ToString();
                    this.ReadingNo = record.HistoryRecordCode;
                    HistoryRecord listHis = HistoryRecord.FindById(selectLong[iSele]);
                    var lstSpecList = DataBaseHelper.QueryByEdition(listHis.SpecListName, listHis.FilePath, listHis.EditionType);
                    //var lstSpecList = SpecList.FindBySql("select * from speclist where  in );
                    this.Specification = lstSpecList.SpecSummary;
                    this.dWeight = lstSpecList.Weight;
                    this.operateMember = lstSpecList.Operater;
                    if (!SeleWorkCurveNameList.Contains(workCurve.Name)) SeleWorkCurveNameList.Add(workCurve.Name);
                    tempList = DataBaseHelper.QueryByEdition(record.SpecListName, record.FilePath, record.EditionType);

                    if (tempList != null && tempList.Specs.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData))
                            spec = tempList.Specs[0];
                    }
                    else
                    {
                        Msg.Show(Info.DataDelete);
                        return string.Empty;
                    }
                    var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
                    elementListPDF.AddRange(elements.ToList());
                    foreach (var element in elements)
                    {
                        var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList != null && curveElement.ElementList.WorkCurve != null && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
                        if (temp == null)
                            continue;
                        double content = 0.0;
                        double.TryParse(element.contextelementValue, out content);
                        temp.Intensity = element.CaculateIntensity;
                        switch(element.unitValue)
                        {
                            case  2:
                                temp.Error = temp.Error / 10000;
                                temp.Content = content / 10000;
                                break;
                            case 3:
                                temp.Error = temp.Error / 10;
                                temp.Content = content / 10;
                                break;
                            default :
                                temp.Content = content;
                                temp.Error = element.Error;
                                break;
                            

                        }
                        elementList.Items.Add(temp);
                    }
                    if (contextMenuStrip != null)
                    {
                        foreach (ToolStripMenuItem tempItem in contextMenuStrip.Items)
                        {
                            if (Atoms.AtomList.ToList().Find(w => w.AtomName == tempItem.Name) != null)
                            {
                                if (!tempItem.Checked)
                                {
                                    CurveElement tempFind = elementList.Items.ToList().Find(w => w.Caption == tempItem.Name);
                                    if (tempFind != null)
                                        elementList.Items.Remove(tempFind);
                                }
                                else if (tempItem.Checked)
                                {
                                    CurveElement tempFind = elementList.Items.ToList().Find(w => w.Caption == tempItem.Name);
                                    if (tempFind == null)
                                    {
                                        tempFind = CurveElement.New;
                                        tempFind.Caption = tempItem.Name;
                                        tempFind.RowsIndex = contextMenuStrip.Items.IndexOf(tempItem);
                                        elementList.Items.Add(tempFind);
                                    }
                                }
                            }
                        }
                    }

                    this.Spec = spec;
                    this.specList = tempList;
                    var tt = elementList.Items.ToList().OrderBy(w => w.RowsIndex);
                    this.Elements = elementList;//this.Elements?? elementList;
                    if (object.ReferenceEquals(this.Elements, elementList))//如果感兴趣元素取的时内部值而非外部赋值则需要重新排序
                    {
                        this.Elements.Items.Clear();
                        tt.ToList().ForEach(w =>
                        {
                            this.Elements.Items.Add(w);
                        });
                    }
                    this.WorkCurveName = workCurveName;
                    this.FirstContIntr.Add(this.Elements.Items.Count);
                    this.InterestElemCount = this.Elements.Items.ToList().FindAll(w => w.IsShowElement).Count;
                    #endregion

                    ReplaceCellText(ref ws, "%Operator%", operateMember);
                    #region 替换
                    if (specList != null)
                    {
                            ReplaceCellText(ref ws, "%SampleName%", specList.SampleName);
                            ReplaceCellText(ref ws, "%SpecName%", this.specList.Name);
                            ReplaceCellText(ref ws, "%Supplier%", specList.Supplier);
                            ReplaceCellText(ref ws,"%TestTime%",specList.Specs[0].UsedTime.ToString()+"(s)");//待调试
                            ReplaceCellText(ref ws, "%Voltage%", specList.Specs[0].TubVoltage.ToString("f0") + "(KV)");
                            ReplaceCellText(ref ws, "%Current%", specList.Specs[0].TubCurrent.ToString("f0") + "(μA)");
                            ReplaceCellText(ref ws, "%TestDate%", ((DateTime)specList.SpecDate).ToString());
                            ReplaceCellText(ref ws, "%TestShortDate%", ((DateTime)specList.SpecDate).ToShortDateString());
                            ReplaceCellText(ref ws, "%TestShortTime%", ((DateTime)specList.SpecDate).ToShortTimeString());
                            ReplaceCellText(ref ws, "%ReportDate%", DateTime.Now.ToShortDateString());
                            ReplaceCellText(ref ws, "%Shape%", this.specList.Shape);
                            ReplaceCellText(ref ws, "%Description%", this.specList.SpecSummary);
                            if (FindTextCell(ws, "%SamplePhoto%")!=null && specList.ImageShow)
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
                    ReplaceCellText(ref ws, "%WorkCurve%", WorkCurveName);
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
                        string filterElement = (WorkCurveHelper.DeviceCurrent.Filter.Count > 0 && Spec.DeviceParameter.FilterIdx > 0) ? "(" + WorkCurveHelper.DeviceCurrent.Filter[Spec.DeviceParameter.FilterIdx-1].Caption + ")" : "";
                        ReplaceCellText(ref ws, "%FilterIdx%", Spec.DeviceParameter.FilterIdx + filterElement);
                        filterElement = (WorkCurveHelper.DeviceCurrent.Collimators.Count > 0 && Spec.DeviceParameter.CollimatorIdx > 0) ? "(" + WorkCurveHelper.DeviceCurrent.Collimators[Spec.DeviceParameter.CollimatorIdx-1].Diameter + "mm)" : "";
                        ReplaceCellText(ref ws, "%CollimatorIdx%", Spec.DeviceParameter.CollimatorIdx + filterElement);
                    }
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
                    //var Au = (from l in Elements.Items where string.Compare(l.Caption, "Au", true) == 0 select l.Content).FirstOrDefault();
                    //double dKValue = Au * 24 / 99.995;
                    //ReplaceCellText(ref ws, "%Karat%", dKValue.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(k)");
                    CurveElement ceTemp = Elements.Items.ToList().Find(l => l.Caption.ToLower().Equals("au"));
                    if (ceTemp != null)
                    {
                        //var Au = (from l in Elements.Items where string.Compare(l.Caption, "Au", true) == 0 select l.Content).FirstOrDefault();
                        double dKValue = ceTemp.Content * 24 / WorkCurveHelper.KaratTranslater;
                        ReplaceCellText(ref ws, "%Karat%", dKValue.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(k)");

                    }

                    //填充元素名 含英文全称
                    ReplaceElementsTable(ref ws, "%ElemName%");
                    //填充元素全名称 中文全称
                    ReplaceElementsTable(ref ws, "%ElemAllName%");
                    ReplaceElementsTable(ref ws, "%ElemNameAll%");
                    //填充强度
                    ReplaceElementsTable(ref ws, "%Intensity%");
                    //填充判定结果
                    ReplaceElementsTable(ref ws, "%Results%");
                    //填充含量
                    ReplaceElementsTable(ref ws, "%Content%");
                    //填充误差
                    ReplaceElementsTable(ref ws, "%Error%");
                    //填充限定标准
                    ReplaceElementsTable(ref ws, "%Limits%");


                    //公司其它信息
                    Dictionary<string, string> dReportOtherInfo = new Dictionary<string, string>();
                    GetReportInfo(ref dReportOtherInfo);

                    foreach (string sKey in dReportOtherInfo.Keys)
                    {
                        ReplaceCellText(ref ws, "%" + sKey + "%", dReportOtherInfo[sKey]);
                    }


                    DeleteEmptyRowsOrColumns(ref ws,DeleteEmptyType.Row);
                         //填充谱图
                    cell = FindTextCell(ws, "%Spectrum%");
                    if (cell != null && selectLong.Count <= 1)//
                    {
                        Bitmap bmp = new Bitmap(640, 160);
                        // Graphics g = Graphics.FromImage(bmp);
                        DrawSpec(ref bmp,false);
                        ReplaceCellImage(ref ws, "%Spectrum%", bmp);
                    }
                    else if (cell != null)
                    {
                       // 删除谱图存储空间;
                        if (cell.IsMerged)
                        {
                            Range ca = cell.GetMergedRange();
                            ws.Cells.DeleteRows(ca.FirstRow, ca.RowCount);
                        }
                        else ws.Cells.DeleteRow(cell.Row);
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
                    #endregion
                    #region 删除多余行
                    //清除多余的%%之的值
                    ReplaceExcelMatchText(ref ws, "%.?%|%.+%", null);
                    DeleteEmptyRowsOrColumns(ref ws, DeleteEmptyType.Row);
                    #endregion
                    #region 复制粘贴重复模板内容
                    if (iSele != selectLong.Count - 1)
                    {
                            //Range currRng = ws.Cells.CreateRange(ws.Cells[(iSele + 1) * rowCount + 1, 0].Name,ws.Cells[(iSele + 2) * rowCount, columnCount-1].Name);
                           // ws.Cells.ImportTwoDimensionArray(data,ws.Cells[(iSele + 1) * rowCount + 1, 0].Row,ws.Cells[(iSele + 1) * rowCount + 1, 0].Column);
                        StartIndex = RangTem.RowCount + RangTem.FirstRow;
                        ws.Cells.ImportDataTable(dataCopy, false, ws.Cells[StartIndex + 1, 0].Name);
                        RangTem = ws.Cells.CreateRange(StartIndex + 1, 0, RangStyle.RowCount, RangStyle.ColumnCount);
                        RangTem.CopyStyle(RangStyle);
                    }
                    #endregion
                }

                #region 添加统计信息
                bool flg = false;
                bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flg);
                if (selectLong.Count > 1 && flg)
                {
                    rowCount = ws.Cells.MaxRow+1;
                    columnCount = ws.Cells.MaxColumn+1;
                    Worksheet sheet = wb.Worksheets[1];
                    object[,] dataTemp = sheet.Cells.ExportArray(sheet.Cells[0].Row, sheet.Cells[0].Column, sheet.Cells.Rows.Count, sheet.Cells.Columns.Count);
                    Range RangeStatic = sheet.Cells.CreateRange(0, 0, sheet.Cells.MaxRow+1, sheet.Cells.MaxColumn+1);
                    ws.Cells.ImportTwoDimensionArray(dataTemp, sheet.Cells[0].Row + rowCount, sheet.Cells[0].Column);
                    RangTem = ws.Cells.CreateRange(sheet.Cells[0].Row + rowCount, 0, RangeStatic.RowCount, RangeStatic.ColumnCount);
                    RangTem.CopyStyle(RangeStatic);
                    //填充元素平均值

                     //填充元素名 含英文全称
                     ReplaceElementsTable(ref ws, "%ElemName%");
                     ReplaceElementsTable(ref ws, "%ElemNameAll%");
                     ReplaceElementsTable(ref ws, "%Average%");
                     ReplaceElementsTable(ref ws, "%Min%");
                     ReplaceElementsTable(ref ws, "%Max%");
                     ReplaceElementsTable(ref ws, "%SD%");

                }
                #endregion
                #region 删除多余行
                //清除多余的%%之的值
                ReplaceExcelMatchText(ref ws, "%.?%|%.+%", null);
                DeleteEmptyRowsOrColumns(ref ws, DeleteEmptyType.Row);
                #endregion
                


                for(int i=wb.Worksheets.Count-1;i>=0;i--)
                {
                    if (wb.Worksheets[i] != ws) wb.Worksheets.RemoveAt(i);
                }
                string strPassword = string.Empty;
                if (ReportTemplateHelper.IsEncryption)
                {

                    strPassword = GetPassWord(); //保护工作表
                    ws.Protect(ProtectionType.All,strPassword,"");
                }
                try
                {
                    if (flag)
                    {
                        switch (ReportFileType)
                        {
                            case 2:
                                StrSavePath = ReportPath + fileName + ".csv";
                                wb.Save(StrSavePath, Aspose.Cells.SaveFormat.CSV);
                                break;
                            case 1:
                                StrSavePath = ReportPath  + fileName + ".pdf";
                                Aspose.Cells.CellsHelper.FontDir = System.Environment.GetEnvironmentVariable("windir") + "\\Fonts";
                                wb.Save(StrSavePath, Aspose.Cells.SaveFormat.Pdf);
                                break;
                            case 0:
                            default:
                                StrSavePath = ReportPath  + fileName + ".xls";
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
               
                }
                catch (Exception ce)
                {
                    Msg.Show(ce.Message);
                }
            return StrSavePath;
         }

        //public string GenerateRepeaterReport(string fileName, ContextMenuStrip contextMenuStrip, bool flag, List<long> selectLong)
        //{
        //    string StrSavePath = string.Empty;
        //    if (!System.IO.File.Exists(TempletFileName))
        //    {
        //        Msg.Show(Info.TemplateNoExists);
        //        return StrSavePath;
        //    }
        //    Workbook wb = new Workbook(FileFormatType.Excel97To2003);
        //    wb.Open(TempletFileName);
        //    Worksheet ws = wb.Worksheets[0];
        //    string workCurveName = string.Empty;
        //    SpecEntity spec = new SpecEntity();
        //    SpecListEntity tempList = new SpecListEntity();
        //    List<string> SeleWorkCurveNameList = new List<string>();
        //    //excelReport.excelWorksheet.UsedRange.Copy(Missing.Value);
        //    int rowCount = ws.Cells.Rows.Count;
        //    int columnCount = ws.Cells.Columns.Count;
        //    for (int iSele = 0; iSele < selectLong.Count; iSele++)
        //    {
        //        #region 初始化 Report
        //        ElementList elementList = ElementList.New;
        //        HistoryRecord record = HistoryRecord.FindById(selectLong[iSele]);
        //        if (record == null)
        //            continue;
        //        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
        //        if (workCurve == null)
        //            continue;

        //        WorkCurveID = workCurve.Id.ToString();
        //        workCurveName = workCurve.Name;
        //        historyRecordid = selectLong[iSele].ToString();
        //        this.ReadingNo = record.HistoryRecordCode;
        //        HistoryRecord listHis = HistoryRecord.FindById(selectLong[iSele]);
        //        var lstSpecList = DataBaseHelper.QueryByEdition(listHis.SpecListName, listHis.FilePath, listHis.EditionType);
        //        //var lstSpecList = SpecList.FindBySql("select * from speclist where  in );
        //        this.Specification = lstSpecList.SpecSummary;
        //        this.dWeight = lstSpecList.Weight;
        //        this.operateMember = lstSpecList.Operater;
        //        if (!SeleWorkCurveNameList.Contains(workCurve.Name)) SeleWorkCurveNameList.Add(workCurve.Name);
        //        tempList = DataBaseHelper.QueryByEdition(record.SpecListName, record.FilePath, record.EditionType);

        //        if (tempList != null && tempList.Specs.Length > 0)
        //        {
        //            if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData))
        //                spec = tempList.Specs[0];
        //        }
        //        else
        //        {
        //            Msg.Show(Info.DataDelete);
        //            return string.Empty;
        //        }
        //        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
        //        elementListPDF.AddRange(elements.ToList());
        //        foreach (var element in elements)
        //        {
        //            var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList != null && curveElement.ElementList.WorkCurve != null && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
        //            if (temp == null)
        //                continue;
        //            double content = 0.0;
        //            double.TryParse(element.contextelementValue, out content);
        //            temp.Intensity = element.CaculateIntensity;
        //            switch (element.unitValue)
        //            {
        //                case 2:
        //                    temp.Error = temp.Error / 10000;
        //                    temp.Content = content / 10000;
        //                    break;
        //                case 3:
        //                    temp.Error = temp.Error / 10;
        //                    temp.Content = content / 10;
        //                    break;
        //                default:
        //                    temp.Content = content;
        //                    temp.Error = element.Error;
        //                    break;


        //            }
        //            elementList.Items.Add(temp);
        //        }
        //        if (contextMenuStrip != null)
        //        {
        //            foreach (ToolStripMenuItem tempItem in contextMenuStrip.Items)
        //            {
        //                if (Atoms.AtomList.ToList().Find(w => w.AtomName == tempItem.Name) != null)
        //                {
        //                    if (!tempItem.Checked)
        //                    {
        //                        CurveElement tempFind = elementList.Items.ToList().Find(w => w.Caption == tempItem.Name);
        //                        if (tempFind != null)
        //                            elementList.Items.Remove(tempFind);
        //                    }
        //                    else if (tempItem.Checked)
        //                    {
        //                        CurveElement tempFind = elementList.Items.ToList().Find(w => w.Caption == tempItem.Name);
        //                        if (tempFind == null)
        //                        {
        //                            tempFind = CurveElement.New;
        //                            tempFind.Caption = tempItem.Name;
        //                            tempFind.RowsIndex = contextMenuStrip.Items.IndexOf(tempItem);
        //                            elementList.Items.Add(tempFind);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        this.Spec = spec;
        //        this.specList = tempList;
        //        var tt = elementList.Items.ToList().OrderBy(w => w.RowsIndex);
        //        this.Elements = elementList;//this.Elements?? elementList;
        //        if (object.ReferenceEquals(this.Elements, elementList))//如果感兴趣元素取的时内部值而非外部赋值则需要重新排序
        //        {
        //            this.Elements.Items.Clear();
        //            tt.ToList().ForEach(w =>
        //            {
        //                this.Elements.Items.Add(w);
        //            });
        //        }
        //        this.WorkCurveName = workCurveName;
        //        this.FirstContIntr.Add(this.Elements.Items.Count);
        //        this.InterestElemCount = this.Elements.Items.ToList().FindAll(w => w.IsShowElement).Count;
        //        #endregion

        //        #region 替换
        //        ReplaceCellText(ref ws, "%Operator%", operateMember);
        //        if (specList != null)
        //        {
        //            ReplaceCellText(ref ws, "%SampleName%", specList.SampleName);
        //            ReplaceCellText(ref ws, "%SpecName%", this.specList.Name);
        //            ReplaceCellText(ref ws, "%Supplier%", specList.Supplier);
        //            ReplaceCellText(ref ws, "%TestTime%", specList.Specs[0].UsedTime.ToString() + "(s)");//待调试
        //            ReplaceCellText(ref ws, "%Voltage%", specList.Specs[0].TubVoltage.ToString() + "(KV)");
        //            ReplaceCellText(ref ws, "%Current%", specList.Specs[0].TubCurrent + "(μA)");
        //            ReplaceCellText(ref ws, "%TestDate%", ((DateTime)specList.SpecDate).ToString());
        //            ReplaceCellText(ref ws, "%TestShortDate%", ((DateTime)specList.SpecDate).ToShortDateString());
        //            ReplaceCellText(ref ws, "%TestShortTime%", ((DateTime)specList.SpecDate).ToShortTimeString());
        //            ReplaceCellText(ref ws, "%ReportDate%", DateTime.Now.ToShortDateString());
        //            ReplaceCellText(ref ws, "%Shape%", this.specList.Shape);
        //            ReplaceCellText(ref ws, "%Description%", this.specList.SpecSummary);
        //            if (FindTextCell(ws, "%SamplePhoto%") != null && specList.ImageShow)
        //            {
        //                string fileNameFull = WorkCurveHelper.SaveSamplePath + "\\" + specList.Name + ".jpg";
        //                FileInfo infoIf = new FileInfo(fileNameFull);
        //                if (infoIf.Exists)
        //                {
        //                    Bitmap tempImage = (Bitmap)Image.FromFile(fileNameFull);
        //                    ReplaceCellImage(ref ws, "%SamplePhoto%", tempImage);
        //                }
        //            }
        //        }
        //        ReplaceCellText(ref ws, "%WorkCurve%", WorkCurveName);
        //        ReplaceCellText(ref ws, "%DeviceName%", WorkCurveHelper.DeviceCurrent.Name);
        //        ReplaceCellText(ref ws, "%SampleWeight%", (dWeight > 0 ? dWeight.ToString() + "g" : ""));
        //        ReplaceCellText(ref ws, "%Specification%", Specification);//填充规格

        //        var address = ReportTemplateHelper.LoadSpecifiedNode("Report/BrassReport", "Address");
        //        ReplaceCellText(ref ws, "%Address%", Info.Address + ":" + (address == null ? "" : address.InnerText));

        //        if (WorkCurveHelper.isShowND)
        //        {
        //            ReplaceCellText(ref ws, "%Remarks%", Info.Remarks + WorkCurveHelper.NDValue.ToString() + "ppm");
        //        }
        //        else
        //        {
        //            ReplaceCellText(ref ws, "%Remarks%", "");
        //        }
        //        if (Spec != null)
        //        {
        //            string filterElement = (WorkCurveHelper.DeviceCurrent.Filter.Count > 0 && Spec.DeviceParameter.FilterIdx > 0) ? "(" + WorkCurveHelper.DeviceCurrent.Filter[Spec.DeviceParameter.FilterIdx-1].Caption + ")" : "";
        //            ReplaceCellText(ref ws, "%FilterIdx%", Spec.DeviceParameter.FilterIdx + filterElement);
        //            filterElement = (WorkCurveHelper.DeviceCurrent.Collimators.Count > 0 && Spec.DeviceParameter.CollimatorIdx > 0) ? "(" + WorkCurveHelper.DeviceCurrent.Collimators[Spec.DeviceParameter.CollimatorIdx-1].Diameter + "mm)" : "";
        //            ReplaceCellText(ref ws, "%CollimatorIdx%", Spec.DeviceParameter.CollimatorIdx + filterElement);
        //        }
        //        //编号
        //        Cell cell = FindTextCell(ws, "%ReadingNo%");
        //        if (cell != null)
        //        {
        //            if (historyRecordid != null && historyRecordid != "")
        //            {
        //                HistoryRecord historyRecord = HistoryRecord.FindById(long.Parse(historyRecordid));
        //                if (historyRecord != null) ReadingNo = historyRecord.HistoryRecordCode;
        //            }
        //            ReplaceCellText(ref ws, "%ReadingNo%", ReadingNo==null?"":ReadingNo);
        //        }
        //        CurveElement ceTemp = Elements.Items.ToList().Find(l => l.Caption.ToLower().Equals("au"));
        //        if (ceTemp != null)
        //        {
        //            //var Au = (from l in Elements.Items where string.Compare(l.Caption, "Au", true) == 0 select l.Content).FirstOrDefault();
        //            double dKValue = ceTemp.Content * 24 / 99.995;
        //            ReplaceCellText(ref ws, "%Karat%", dKValue.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(k)");
 
        //        }
               

        //        //填充元素名 含英文全称
        //        ReplaceElementsTable(ref ws, "%ElemName%");
        //        //填充元素全名称 中文全称
        //        ReplaceElementsTable(ref ws, "%ElemAllName%");
        //        //填充强度
        //        ReplaceElementsTable(ref ws, "%Intensity%");
        //        //填充判定结果
        //        ReplaceElementsTable(ref ws, "%Results%");
        //        //填充含量
        //        ReplaceElementsTable(ref ws, "%Content%");
        //        //填充误差
        //        ReplaceElementsTable(ref ws, "%Error%");
        //        //填充限定标准
        //        ReplaceElementsTable(ref ws, "%Limits%");


        //        //公司其它信息
        //        Dictionary<string, string> dReportOtherInfo = new Dictionary<string, string>();
        //        GetReportInfo(ref dReportOtherInfo);

        //        foreach (string sKey in dReportOtherInfo.Keys)
        //        {
        //            ReplaceCellText(ref ws, "%" + sKey + "%", dReportOtherInfo[sKey]);
        //        }


        //        DeleteEmptyRowsOrColumns(ref ws, DeleteEmptyType.Row);
        //        //填充谱图
        //        cell = FindTextCell(ws, "%Spectrum%");
        //        if (cell != null)
        //        {
        //            Bitmap bmp = new Bitmap(640, 160);
        //            // Graphics g = Graphics.FromImage(bmp);
        //            DrawSpec(ref bmp);
        //            ReplaceCellImage(ref ws, "%Spectrum%", bmp);
        //        }

        //        //填充元素图
        //        cell = FindTextCell(ws, "%ElemSpec%");
        //        if (cell != null)
        //        {
        //            Bitmap bmp = new Bitmap(640, 160);
        //            //Graphics g = Graphics.FromImage(bmp);
        //            //int width = (int)Math.Round(Convert.ToInt32(rang.Width) * g.DpiX / 72.0);
        //            //int height = (int)Math.Round(Convert.ToInt32(rang.Height) * g.DpiY / 72.0);
        //            //bmp = new Bitmap(width, height);
        //            DrawInterstringElems(ref bmp);
        //            ReplaceCellImage(ref ws, "%ElemSpec%", bmp);

        //        }
        //        #endregion
        //    }

        //    #region 添加统计信息
        //    bool flg = false;
        //    bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flg);
        //    if (selectLong.Count > 1 && flg)
        //    {
        //        //填充元素名 含英文全称
        //        ReplaceElementsTable(ref ws, "%ElemName%");
        //        ReplaceElementsTable(ref ws, "%Average%");
        //        ReplaceElementsTable(ref ws, "%Min%");
        //        ReplaceElementsTable(ref ws, "%Max%");
        //        ReplaceElementsTable(ref ws, "%SD%");

        //    }
        //    #endregion

        //    #region 删除多余行
        //    //清除多余的%%之的值
        //    ReplaceExcelMatchText(ref ws, "%.?%|%.+%", null);
        //    DeleteEmptyRowsOrColumns(ref ws, DeleteEmptyType.Row);
        //    #endregion

        //    #region 单元格边框
        //    Range range = ws.Cells.CreateRange(ws.Cells[0].Row, ws.Cells[0].Column, ws.Cells.Rows.Count, ws.Cells.Columns.Count);
        //    range.SetOutlineBorder(BorderType.TopBorder, CellBorderType.Thin, Color.Black);
        //    range.SetOutlineBorder(BorderType.LeftBorder, CellBorderType.Thin, Color.Black);
        //    range.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
        //    range.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Black);
        //    #endregion



        //    string strPassword = string.Empty;
        //    if (ReportTemplateHelper.IsEncryption)
        //    {

        //        strPassword = GetPassWord(); //保护工作表
        //        ws.Protect(ProtectionType.All, strPassword, "");
        //    }
        //    try
        //    {
        //        if (flag)
        //        {
        //            switch (ReportFileType)
        //            {
        //                case 2:
        //                    StrSavePath = ReportPath + fileName + ".csv";
        //                    wb.Save(StrSavePath, Aspose.Cells.SaveFormat.CSV);
        //                    break;
        //                case 1:
        //                    StrSavePath = ReportPath + fileName + ".pdf";
        //                    Aspose.Cells.CellsHelper.FontDir = System.Environment.GetEnvironmentVariable("windir") + "\\Fonts";
        //                    wb.Save(StrSavePath, Aspose.Cells.SaveFormat.Pdf);
        //                    break;
        //                case 0:
        //                default:
        //                    StrSavePath = ReportPath + fileName + ".xls";
        //                    wb.Save(StrSavePath, SaveFormat.Excel97To2003);
        //                    break;
        //            }

        //        }
        //        else
        //        {
        //            if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count > 0)
        //            {
        //                Aspose.Cells.Rendering.ImageOrPrintOptions Io = new Aspose.Cells.Rendering.ImageOrPrintOptions();
        //                Io.HorizontalResolution = 200;
        //                Io.VerticalResolution = 200;
        //                Io.ChartImageType = System.Drawing.Imaging.ImageFormat.Png;
        //                Io.ImageFormat = System.Drawing.Imaging.ImageFormat.Tiff;
        //                Io.OnePagePerSheet = false;
        //                Io.PrintingPage = PrintingPageType.IgnoreStyle;
        //                Aspose.Cells.Rendering.SheetRender ss = new Aspose.Cells.Rendering.SheetRender(ws, Io);
        //                System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
        //                string printerName = doc.PrinterSettings.PrinterName;
        //                ss.ToPrinter(printerName);
        //            }
        //        }

        //    }
        //    catch (Exception ce)
        //    {
        //        Msg.Show(ce.Message);
        //    }
        //    finally
        //    { 
        //    }
        //    return StrSavePath;
        //}

        public string GenerateRetestReport(string fileName, bool flag, List<HistoryRecord> historyRecordList)
        {
            if (historyRecordList.Count <= 1)
            {
               return GenerateReport(fileName, flag);

            }
            this.elementListPDF.Clear();
            string newFileName = fileName;
            List<HistoryElement> historyElementList = HistoryElement.FindBySql("select * from historyelement where historyrecord_Id=" + historyRecordList[0].Id.ToString());
            DataTable dt = new DataTable();

            dt.Columns.Clear();
            dt.Columns.Add("Time", typeof(string));
            foreach (HistoryElement curEle in historyElementList)
            {
                if (!dt.Columns.Contains(curEle.elementName))
                {
                    dt.Columns.Add(curEle.elementName, typeof(string));
                    dt.Columns[dt.Columns.Count - 1].Caption = Atoms.GetAtom(curEle.elementName).AtomNameEN;
                }
            }
            int cont = 0;
            string[] strElemsLayer = Helper.ToStrs(Elements.LayerElemsInAnalyzer == null ? "" : Elements.LayerElemsInAnalyzer);
            string sContentBit = WorkCurveHelper.SoftWareContentBit.ToString();
            for (int j = 0; j < historyRecordList.Count; j++)
            {
                DataRow rowNew = dt.NewRow();
                rowNew["Time"] = ++cont;
                foreach (DataColumn colname in dt.Columns)
                {
                    if (colname.ColumnName == "Time") continue;
                    HistoryElement element = HistoryElement.FindOne(w => w.elementName == colname.ColumnName && w.HistoryRecord.Id == historyRecordList[j].Id);
                    this.elementListPDF.Add(element);
                    if (element != null && dt.Columns.Contains(colname.ColumnName))
                    {
                       // if (Elements.RhIsLayer && element.elementName.ToUpper().Equals("RH"))
                        if (Elements.RhIsLayer && strElemsLayer.Contains(element.elementName))
                        {
                            string valueStr = element.thickelementValue;
                            if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                            {
                                double Value = double.Parse(valueStr);
                                rowNew[colname.ColumnName] = Value.ToString("f" + WorkCurveHelper.ThickBit.ToString());
                            }
                        }
                        else
                        {
                            string valueStr = element.contextelementValue;
                            if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                            {
                                double Value = double.Parse(valueStr);
                                rowNew[colname.ColumnName] = Value.ToString("f" + sContentBit);
                            }
                        }
                    }
                    else if (element == null && dt.Columns.Contains(colname.ColumnName))//无数据时处理方式
                    {
                        rowNew[colname.ColumnName] = default(double).ToString("f" + sContentBit);
                    }
                }
                dt.Rows.Add(rowNew);
            }
            return GenerateRetestReport(fileName, dt, flag,true);
        }

        public string GenerateRetestReport(string fileName, bool flag, List<long> historyRecordIDList)
        {
            if (historyRecordIDList.Count <= 1)
            {
                return GenerateReport(fileName, flag);

            }
            //return GenerateRepeaterReport(fileName, null, flag, historyRecordIDList);
            string historyRecordID = historyRecordIDList[0].ToString();
            for (int i = 1; i < historyRecordIDList.Count; i++)
            {
                historyRecordID += "," + historyRecordIDList[i].ToString();
            }
            List<HistoryRecord> historyRecordList = new List<HistoryRecord>();
            historyRecordList.AddRange(HistoryRecord.FindBySql("select * from historyrecord where id IN (" + historyRecordID + ")"));
            return GenerateRetestReport(fileName, flag, historyRecordList);
        }


        /// <summary>
        /// 生成多次测量报告
        /// </summary>
        /// <param name="fileName">保存文件名，为空时直接打印</param>
        ///<param name="dataTable">单次测量结果以及统计信息</param>
        ///<param name="flag">是否是保存文件</param>
        ///<param name="AddStatistic">列排统计结果</param>
        public virtual string GenerateRetestReport(string fileName, DataTable dataTable, bool flag, bool AddStatistic)//多次测量一个表格
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
            //RetestFileName = ExcelTemplateParams.ManyTimeTemplate;
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
                    ReplaceCellText(ref ws, "%ReportDate%", DateTime.Now.ToShortDateString());
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

                //待调试
                //if (Spec != null)
                //{
                //    ReplaceCellText(ref ws, "%TestTime%", Spec.UsedTime + "(s)");
                //    ReplaceCellText(ref ws, "%Voltage%", Spec.DeviceParameter!=null ? Spec.DeviceParameter.TubVoltage + "(KV)":"");
                //    ReplaceCellText(ref ws, "%Current%", Spec.DeviceParameter!=null ? Spec.DeviceParameter.TubCurrent + "(μA)":"");
                //    ReplaceCellText(ref ws, "%TestTime%", Spec.UsedTime + "(s)");
                //}

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
                ////var Au = (from l in Elements.Items where string.Compare(l.Caption, "Au", true) == 0 select l.Content).FirstOrDefault();
                ////double dKValue = Au * 24 / 99.995;
                ////ReplaceCellText(ref ws, "%Karat%", dKValue.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(k)");


                //填充元素名 含英文全称
                //ReplaceElementsTable(ref ws, "%ElemName%");
                ////填充元素全名称 中文全称
                //ReplaceElementsTable(ref ws, "%ElemAllName%");
                ////填充强度
                //ReplaceElementsTable(ref ws, "%Intensity%");
                ////填充判定结果
                //ReplaceElementsTable(ref ws, "%Results%");
                ////填充含量
                //ReplaceElementsTable(ref ws, "%Content%");
                ////填充误差
                //ReplaceElementsTable(ref ws, "%Error%");
                ////填充限定标准
                //ReplaceElementsTable(ref ws, "%Limits%");


                //公司其它信息
                Dictionary<string, string> dReportOtherInfo = new Dictionary<string, string>();
                GetReportInfo(ref dReportOtherInfo);

                foreach (string sKey in dReportOtherInfo.Keys)
                {
                    ReplaceCellText(ref ws, "%" + sKey + "%", dReportOtherInfo[sKey]);
                }
                #region  插入dataTable的值
                if (NeedMultiResults && startIndex > 0)
                {
                    startIndex -= 2;//去除title行
                    NeedsTitle needsTitle = NeedsTitle.OnlyOnce;//是否需要列名
                    bool needsRowHeader = true;
                    bool needsMultiRowHeader = false;
                    try
                    {
                        string sReTestNeedTitle = "";
                        string sReTestNeedRowHeader = "";
                        string sReTestNeedMultiRowHeader = "";
                        XmlNode xmlnode = ReportTemplateHelper.LoadSpecifiedNode("Report", "CommonReport");
                        if (xmlnode != null && xmlnode.ChildNodes.Count > 0)
                        {
                            for (int i = 0; i < xmlnode.ChildNodes.Count; i++)
                            {
                                if (xmlnode.ChildNodes[i].Name == "ReTestNeedTitle") sReTestNeedTitle = xmlnode.ChildNodes[i].InnerText;
                                if (xmlnode.ChildNodes[i].Name == "ReTestNeedRowHeader") sReTestNeedRowHeader = xmlnode.ChildNodes[i].InnerText;
                                if (xmlnode.ChildNodes[i].Name == "ReTestNeedMultiRowHeader") sReTestNeedMultiRowHeader = xmlnode.ChildNodes[i].InnerText;
                            }
                        }

                        needsTitle = (NeedsTitle)int.Parse(sReTestNeedTitle);
                        needsRowHeader = bool.Parse(sReTestNeedRowHeader);
                        needsMultiRowHeader = bool.Parse(sReTestNeedMultiRowHeader);
                    }
                    catch
                    {
                        needsTitle = NeedsTitle.OnlyOnce;//默认NeedsTitle.OnlyOnce;
                        needsRowHeader = true;//默认显示第0列如顺序等
                        needsMultiRowHeader = false;
                    }

                    var table = SplitDataRow(dataTable, iTotalColumnMun, ref needsTitle, needsRowHeader, needsMultiRowHeader);
                    if (table == null || table.Rows.Count == 0) goto Finish;
                    object[,] cellsValue = new object[table.Rows.Count, iTotalColumnMun];
                    int cols = 0;
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        for (int j = 0; j < table.Rows.Count; j++)
                        {
                            cellsValue[j, cols] = table.Rows[j][i].ToString();
                        }
                        cols++;
                    }
                    InsertRows(ref ws, startIndex + 2, table.Rows.Count - 1, -1);//至少留有一个标题和内容
                    Range range = ws.Cells.CreateRange(startIndex, ws.Cells[0].Column, table.Rows.Count + 1, iTotalColumnMun);

                    range.RowHeight = 20;
                    Style style = ws.Cells.GetCellStyle(startIndex, ws.Cells[0].Column);
                    style.Font.Size = 10;
                    style.HorizontalAlignment = TextAlignmentType.Center;
                    SetResultsBorderStyle(style, CellBorderType.Thin);
                    range.SetStyle(style);
                    ws.Cells.ImportTwoDimensionArray(cellsValue, startIndex, ws.Cells[0].Column);
                    ws.AutoFitRows(startIndex, startIndex + table.Rows.Count);
                }

                #endregion
                #region 添加统计信息

                if (selectLong.Count > 1 && AddStatistic)
                {
                    // 填充元素名 含英文全称
                    if (!NeedMultiResults)
                    {
                        ReplaceElementsTable(ref ws, "%ElemName%");
                        ReplaceElementsTable(ref ws, "%ElemAllName%");
                    }
                    ReplaceElementsTable(ref ws, "%Average%");
                    ReplaceElementsTable(ref ws, "%Min%");
                    ReplaceElementsTable(ref ws, "%Max%");
                    ReplaceElementsTable(ref ws, "%SD%");

                }

                #endregion


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
                // ws.AutoFitRows();
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


                int dataEndRow = 0;
                int staStartRow = 0;

                if (dataTable.Rows.Count != 1)
                {
                    dataEndRow = dataTable.Rows.Count + 2;
                    staStartRow = dataTable.Rows.Count + 2;

                }
                else
                {

                    dataEndRow = dataTable.Rows.Count + 8;

                }
                Worksheet sheet = ws;


                //对第二行的样品名称特殊处理

                if (sheet.Cells[1, 1].Value.ToString().Contains("#"))
                    sheet.Cells[1, 1].Value = sheet.Cells[1, 1].Value.ToString().Split(new char[] { '#' })[0];

                int eleNum = 0;
                ArrayList indexList = new ArrayList();
                for (int i = 0; i < sheet.Cells.Columns.Count; i++)
                {
                    if (sheet.Cells[7, i].Value.ToString().Contains("厚度") || sheet.Cells[7, i].Value.ToString().Contains("Thickness") || sheet.Cells[7, i].Value.ToString().Contains("面密度") || sheet.Cells[7, i].Value.ToString().Contains("Density") || sheet.Cells[7, i].Value.ToString().Contains("含量") || sheet.Cells[7, i].Value.ToString().Contains("Concentration"))
                    {

                        eleNum++;
                        indexList.Add(i);
                    }
                }

                if (eleNum >= 1)
                {
                    for (int i = 8; i < dataEndRow; i++)
                    {
                       
                        for (int j = 0; j < eleNum; j++)
                        {
                            double curThick = 0;

                            try
                            {
                                curThick = double.Parse(double.Parse(sheet.Cells[i, (int)indexList[j]].Value.ToString().Trim()).ToString("f" + WorkCurveHelper.ThickBit));
                            }
                            catch
                            {
                                break;

                            }

                            sheet.Cells[i, (int)indexList[j]].Value = curThick;
                            
                        }

                    }

                }


                double res = 0;
                string formula = "";
                string rows = "";
                //设置公式

                if (dataTable.Rows.Count != 1)
                {

                    for (int i = 0; i < sheet.Cells.Columns.Count; i++)
                    {


                        if ((sheet.Cells[7, i].Value.ToString().Contains("含量") || sheet.Cells[7, i].Value.ToString().Contains("Concentration")) && !sheet.Cells[7, i].Value.ToString().Contains("%"))
                        {
                            sheet.Cells[7, i].Value = sheet.Cells[7, i].Value.ToString() + "(%)";
                            for (int row = 8; row < dataEndRow; row++)
                            {
                                if (sheet.Cells[row, i].Value.ToString().Contains("%"))
                                    sheet.Cells[row, i].Value =double.Parse(double.Parse(sheet.Cells[row, i].Value.ToString().Split(new char[] { '(' })[0].Trim()).ToString("f" + WorkCurveHelper.ThickBit));
                            }

                        }


                        if (sheet.Cells[7, i].Value.ToString().Contains("样品名称") || sheet.Cells[7, i].Value.ToString().Contains("Sample")) 
                        {
                            for (int row = 8; row < dataEndRow; row++)
                            {
                                if (sheet.Cells[row, i].Value.ToString().Contains("#"))

                                    sheet.Cells[row, i].Value = sheet.Cells[row, i].Value.ToString().Split(new char[] { '#' })[0];
                            }

                        }



                        if (sheet.Cells[7, i].Value.ToString().Contains("厚度") || sheet.Cells[7, i].Value.ToString().Contains("Thickness") || sheet.Cells[7, i].Value.ToString().Contains("面密度") || sheet.Cells[7, i].Value.ToString().Contains("Density") || sheet.Cells[7, i].Value.ToString().Contains("含量") || sheet.Cells[7, i].Value.ToString().Contains("Concentration"))
                        {
                            rows = "";
                            for (int row = 8; row < dataEndRow; row++)
                            {
                                if (double.TryParse(sheet.Cells[row, i].Value.ToString(), out  res))
                                    rows += ((char)(65 + i)) + (row + 1).ToString() + ",";

                            }


                            if (rows == "")
                            {
                                sheet.Cells[staStartRow, i].Value = "--";
                                sheet.Cells[staStartRow + 1, i].Value = "--";
                                sheet.Cells[staStartRow + 2, i].Value = "--";
                                sheet.Cells[staStartRow + 3, i].Value = "--";
                                sheet.Cells[staStartRow + 4, i].Value = "--";
                                sheet.Cells[staStartRow + 5, i].Value = "--";

                                continue;
                            }

                            rows = rows.Substring(0, rows.Length - 1);


                            formula = "=ROUND(AVERAGE(";
                            formula += rows;
                            formula += ")," + WorkCurveHelper.ThickBit.ToString() + ")";
                            sheet.Cells[staStartRow, i].Formula = formula;


                            formula = "=ROUND(STDEV(";
                            formula += rows;
                            formula += ")," + WorkCurveHelper.ThickBit.ToString() + ")";
                            sheet.Cells[staStartRow + 1, i].Formula = formula;


                            formula = "=TEXT(ROUND(STDEV(";
                            formula += rows;
                            formula += ")/AVERAGE(";
                            formula += rows;
                            formula += ")," + WorkCurveHelper.ThickBit.ToString() + "),\"0.0000%\")";
                            sheet.Cells[staStartRow + 2, i].Formula = formula;



                            formula = "=ROUND(MAX(";
                            formula += rows;
                            formula += ")," + WorkCurveHelper.ThickBit.ToString() + ")";
                            sheet.Cells[staStartRow + 3, i].Formula = formula;


                            formula = "=ROUND(MIN(";
                            formula += rows;
                            formula += ")," + WorkCurveHelper.ThickBit.ToString() + ")";
                            sheet.Cells[staStartRow + 4, i].Formula = formula;



                            formula = "=ROUND(MAX(";
                            formula += rows;
                            formula += ")-MIN(";
                            formula += rows;
                            formula += ")," + WorkCurveHelper.ThickBit.ToString() + ")";
                            sheet.Cells[staStartRow + 5, i].Formula = formula;




                        }
                    }

                }

                wb.Save(StrSaveFileName, SaveFormat.Excel97To2003);


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



        public virtual void SetResultsBorderStyle(Style style, CellBorderType type)
        {
            style.Borders[BorderType.Horizontal].LineStyle = type;
            style.Borders[BorderType.TopBorder].LineStyle = type;
            style.Borders[BorderType.RightBorder].LineStyle = type;
            style.Borders[BorderType.LeftBorder].LineStyle = type;
            style.Borders[BorderType.BottomBorder].LineStyle = type;
        }
        /// <summary>
        /// 将DataTable分行
        /// </summary>
        /// <param name="table">原DataTable</param>
        /// <param name="iColumns">每行显示列数</param>
        /// <param name="hasHeadColumn">是否需要行头，如顺序等</param>
        /// <param name="HeadColumnName">行头名称</param>
        ///  <param name="hasMultiHeadColumn">是否多个需要行头，如顺序等 ， 换行时是否还需要行头</param>
        /// <returns>返回分行后的DataTable</returns>
        protected DataTable SplitDataRow(DataTable table, int iColumns, ref NeedsTitle needsTitle, bool hasHeadColumn,bool hasMultiHeadColumn, params string[] HeadColumnName)
        {
            NeedsTitle mark = needsTitle;
            List<DataRow> lstRows = new List<DataRow>();
            DataTable tableSource = CreateTemplateDataTable(table, iColumns);
            //List<string> lstColumnHead = new List<string>();
            //for (int i = 0; i < tableSource.Rows.Count;i++ )
            //{
            //    lstColumnHead.Add(tableSource.Rows[i][0].ToString());
            //}
            ////有行头先当做无行头处理
            //if (hasHeadColumn)
            //    tableSource.Columns.Remove(0);
            ////生成的Table需少一列，最后再加行头
            //iColumns--;

            foreach (DataRow row in table.Rows)
            {
                lstRows.AddRange(SplitDataRow(tableSource, row, iColumns, ref needsTitle, hasMultiHeadColumn));
            }
            string strColumnHead = string.Empty;
            //列头名称数组不为空累加字符串
            if (HeadColumnName != null)
                for (int i = 0; i < HeadColumnName.Length; i++)
                {
                    strColumnHead += HeadColumnName[i];
                }
            if (lstRows.FirstOrDefault() == null) return new DataTable();
            DataTable backTable = lstRows[0].Table;
            if (mark != NeedsTitle.Total)//非整体换行模式可以直接加行
            {
                for (int i = 0; i < lstRows.Count; i++)
                {
                    backTable.Rows.Add(lstRows[i]);
                }
            }
            else//整体换行模式重新加载数据行
            {
                int iSplitNum = lstRows.Count / (table.Rows.Count + 1); //(int)Math.Ceiling(table.Columns.Count * 1.0 / iColumns);
                for (int iSplit = 0; iSplit < iSplitNum; iSplit++)
                {
                    for (int iRow = 0; iRow < table.Rows.Count + 1; iRow++)//列头也变成行所以总行数+1
                    {
                        backTable.Rows.Add(lstRows[iRow * iSplitNum + iSplit]);//每行乘上一个分隔行数+第几个分割的小行数即新的行数
                    }
                }
            }

            //不需要行头删除0列
            if (!hasHeadColumn)
                backTable.Columns.RemoveAt(0);



            return backTable;
        }

        public List<DataRow> SplitDataRow(DataTable tableDestination, DataRow row, int iColumns, ref NeedsTitle needsTitle, bool hasMulitHeadColumn)
        {
            List<DataRow> lstRows = new List<DataRow>();
            int iUseColumnCount = row.ItemArray.Length;//该DataRow总共要进行分行的列数
            for (int i = 0; i < (int)Math.Ceiling(iUseColumnCount * 1.0 / iColumns); i++)
            {
                DataRow rowBackHeader = tableDestination.NewRow();
                DataRow rowBack = tableDestination.NewRow();

                if (i == 0)
                {
                    int iFirstRowsColumnsEmpty = 0;//第一行元素个数不足列的个数填充空白的起始位置
                    //每个新Row行的内容为iColumns-1个或者总列数剩余的不足iColumns-1个
                    for (int iFirstRowColumns = 0; iFirstRowColumns < iColumns && iFirstRowColumns < row.ItemArray.Length; iFirstRowColumns++)
                    {
                        //去掉0列标题，否则合并后每行0列都含标题
                        if (iFirstRowColumns != 0)
                        {
                            rowBackHeader[iFirstRowColumns] = row.Table.Columns[iFirstRowColumns].Caption;
                        }
                        rowBack[iFirstRowColumns] = row[iFirstRowColumns];
                        if (iFirstRowColumns == row.ItemArray.Length - 1 && (needsTitle == NeedsTitle.OnlyOnce || needsTitle == NeedsTitle.Total))
                            needsTitle = NeedsTitle.Finished;//如果只需要一次title 一次执行完后改为不需要
                        iFirstRowsColumnsEmpty = iFirstRowColumns + 1;
                    }
                    //元素个数不足以填充满第一行的部分 填充空白
                    for (; iFirstRowsColumnsEmpty < iColumns; iFirstRowsColumnsEmpty++)
                    {
                        rowBack[iFirstRowsColumnsEmpty] = "";
                        rowBackHeader[iFirstRowsColumnsEmpty] = "";
                    }
                }
                else
                {
                    //第0列为列头可能除行分行的第一行需要外其余行不需要
                   // rowBack[0] = "";
                    //rowBackHeader[0] = "";
                    if (hasMulitHeadColumn)
                        rowBack[0] = row[0];    //第一列的统计信息要加上20190417
                    else
                        rowBack[0] = "";
                    rowBackHeader[0] = "";
                    iUseColumnCount++;//新增了一列空白列等效于该DataRow新增了一列

                    int iOtherRowsColumnsEmpty = 0;
                    //每个新Row行的内容为iColumns-1个或者总列数剩余的不足iColumns-1个
                    for (int iOtherRowsColumns = 0; (iColumns + (i - 1) * (iColumns - 1)/*第一行外，其余行只增加了iColumn-1列*/ + iOtherRowsColumns < row.ItemArray.Length) && (iOtherRowsColumns < iColumns - 1); iOtherRowsColumns++)
                    {
                        rowBackHeader[iOtherRowsColumns + 1] //加一表示从1列开始，第0列不需要赋值
                            = row.Table.Columns[iColumns + (i - 1) * (iColumns - 1)/*第一行外，其余行只增加了iColumn-1列*/ + iOtherRowsColumns].Caption;//源数据不需要+1
                        rowBack[iOtherRowsColumns + 1] //加一表示从1列开始，第0列不需要赋值
                            = row[iColumns + (i - 1) * (iColumns - 1)/*第一行外，其余行只增加了iColumn-1列*/  + iOtherRowsColumns];//源数据不需要+1                        
                        if ((iColumns + (i - 1) * (iColumns - 1)/*第一行外，其余行只增加了iColumn-1列*/ + iOtherRowsColumns) == row.ItemArray.Length - 1 && (needsTitle == NeedsTitle.OnlyOnce || needsTitle == NeedsTitle.Total))
                        {
                            needsTitle = NeedsTitle.Finished;//如果只需要一次title 一次执行完后改为不需要                            
                        }
                        iOtherRowsColumnsEmpty = iOtherRowsColumns + 1;//终止时新Row所在的列的下一列 
                    }
                    //新Row剩余的列赋值为空 每个新Row行的内容为iColumns-1个或者总列数剩余的不足iColumns-1个
                    for (; iOtherRowsColumnsEmpty < iColumns - 1; iOtherRowsColumnsEmpty++)//若为上次分行iOtherRowsColumnsEmpty>=iColumns - 1
                    {
                        rowBackHeader[iOtherRowsColumnsEmpty + 1] = "";//加一表示从1列开始，第0列不需要赋值
                        rowBack[iOtherRowsColumnsEmpty + 1] = "";//加一表示从1列开始，第0列不需要赋值
                    }
                }
                if (needsTitle == NeedsTitle.None)//不需title
                {
                    //不加载rowBackHeader
                    lstRows.Add(rowBack);
                }
                else if (needsTitle == NeedsTitle.OnlyOnce || needsTitle == NeedsTitle.Finished)//需要一个或处于完成状态
                {
                    lstRows.Insert(lstRows.Count / 2, rowBackHeader);
                    if (needsTitle == NeedsTitle.Finished)
                    {
                        needsTitle = NeedsTitle.None;//一次执行完后不需要
                    }
                    lstRows.Add(rowBack);
                }
                else if (needsTitle == NeedsTitle.Always)//反复出现
                {
                    lstRows.Add(rowBackHeader);
                    lstRows.Add(rowBack);
                }
                else if (needsTitle == NeedsTitle.Total || needsTitle == NeedsTitle.Finished)
                {
                    lstRows.Insert(lstRows.Count / 2, rowBackHeader);
                    if (needsTitle == NeedsTitle.Finished)
                    {
                        needsTitle = NeedsTitle.None;//一次执行完后不需要
                    }
                    lstRows.Add(rowBack);
                }

            }
            return lstRows;
        }


        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="sheet">需要操作的工作簿</param>
        /// <param name="positionRowIndex">插入行位置</param>
        /// <param name="count">插入行数</param>
        /// <param name="copyRowIndex">复制行数的内容</param>
        protected bool InsertRows(ref Worksheet sheet, int positionRowIndex, int count, int copyRowIndex)
        {
            if (sheet == null || count == 0 || positionRowIndex <= 0) return false;
            if (count > 0)
            {
                for (int inst = 0; inst < count; inst++)
                {
                    sheet.Cells.InsertRow(positionRowIndex);
                    if(copyRowIndex >= 1) sheet.Cells.CopyRow(sheet.Cells, copyRowIndex, positionRowIndex);
                }
            }
            else if (count < 0)
            {
                sheet.Cells.DeleteRows(positionRowIndex,-count);
            }
            return true;
        }

        /// <summary>
        /// 依据传入DataTable创建只有开始iColumns列的DataTable
        /// </summary>
        /// <param name="table">源DataTable</param>
        /// <param name="iColumns">每行显示列数</param>
        /// <returns>返回DataTable</returns>
        public DataTable CreateTemplateDataTable(DataTable table, int iColumns)
        {
            DataTable tableSource = table.Clone();
            if (tableSource.Columns.Count > iColumns)
                for (int i = 0; i < tableSource.Columns.Count; i++)
                {
                    if (i > iColumns - 1)
                    {
                        tableSource.Columns.RemoveAt(i);
                        i--;
                    }
                }
            else
            {
                for (int i = tableSource.Columns.Count; i < iColumns; i++)
                {
                    tableSource.Columns.Add();
                }
            }
            return tableSource;
        }

        private  void MathSwap(ref int iRowCount, ref int iColumnCount)
        {
            var temp = iRowCount;
            iRowCount = iColumnCount;
            iColumnCount = temp;
        }
        protected void DeleteEmptyRowsOrColumns(ref Worksheet sheet, DeleteEmptyType type)
        {
            if (type == DeleteEmptyType.Both)
            {
                DeleteEmptyRowsOrColumns( ref sheet, DeleteEmptyType.Row);
                DeleteEmptyRowsOrColumns(ref sheet, DeleteEmptyType.Column);
                return;
            }
            //删除空白行 列
            //int iRowCount = sheet.Cells.Rows.Count;
            //int iColumnCount = sheet.Cells.Columns.Count;
            int iRowCount = sheet.Cells.MaxRow+1;
            int iColumnCount = sheet.Cells.MaxColumn+1; 
            if (type == DeleteEmptyType.Column) MathSwap(ref iRowCount, ref iColumnCount);

            for (int ii = 0; ii < iRowCount; ii++) 
            {
                for (int jj = 0; jj < iColumnCount; jj++) 
                {
                  
                    bool TorF = false;
                    int i = ii, j = jj;
                    if (type == DeleteEmptyType.Column) MathSwap(ref i, ref j);//只能在局部交换起作用，不能交换到循环递增中起作用
                    var objs = sheet.Cells[i, j].Value;
                    if (sheet.Pictures.Find(w => w.UpperLeftRow <=i && w.UpperLeftColumn <=j && w.LowerRightRow >= i && w.LowerRightColumn >= j) != null)
                        break;
                    if (sheet.Cells[i, j].IsMerged )
                    {
                       Range ca= sheet.Cells[i, j].GetMergedRange();
                       if(type == DeleteEmptyType.Row && ca.RowCount > 1) break;
                       if(type == DeleteEmptyType.Column && ca.ColumnCount > 1)break;
                    }
                    TorF = (objs == null) ? true : false;
                    if (TorF)
                    {  //没有合并的为字符串结果 合并的结果为数组
                        if (jj == iColumnCount - 1)
                        {
                            if (type == DeleteEmptyType.Row)
                                sheet.Cells.DeleteRow(sheet.Cells[i, j].Row);//删除一行
                            else
                                sheet.Cells.DeleteColumn(sheet.Cells[i, j].Column);
                            ii--;//删除一行后row恢复之前的值行数减一
                            iRowCount--;//删除一行后总行数减一 或同一列
                        }
                        else
                            continue;
                    }
                    else
                        break;
                }
            }
        }





        protected Cell FindTextCell(Worksheet worksheet, string strLabel)
        {
            Cell cell = worksheet.Cells.FindStringContains(strLabel, null);
            return cell;
        }
        protected Cell FindTextCell(Worksheet worksheet, string strLabel, out int width, out int height)
        {
            Cell cell = worksheet.Cells.FindStringContains(strLabel, null);
                
            if(cell!=null&&!cell.IsMerged) 
            {
                width = (int)worksheet.Cells.GetColumnWidth(cell.Column);
                height = (int)worksheet.Cells.GetRowHeight(cell.Row);
            }
            else if (cell != null)
            {
                Range range = cell.GetMergedRange();
                width = (int)range.ColumnWidth * range.ColumnCount;
                height = (int)range.RowHeight * range.RowCount;
            }
            else
            {
                width = 0;
                height = 0;
            }
            return cell;
        }
        protected virtual void ReplaceCellImage(ref Worksheet worksheet, string strLabel, Bitmap Image)
        {
            Cell cell = worksheet.Cells.FindStringContains(strLabel, worksheet.Cells[0]);
            if (cell != null)
            {
                System.IO.MemoryStream mstream = new System.IO.MemoryStream();
                Image.Save(mstream, System.Drawing.Imaging.ImageFormat.Bmp);
                if (cell.IsMerged)
                {
                    System.Collections.ArrayList celllst = worksheet.Cells.MergedCells;
                    for (int i = 0; i < celllst.Count; i++)
                    {
                        Aspose.Cells.CellArea CellAreaTemp = (Aspose.Cells.CellArea)celllst[i];
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

        protected void ReplaceConditionCells(ref Worksheet worksheet, string strLabel)
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

            if (specList != null && specList.Specs.Length > 0)
            {
                for (int i = 0; i < specList.Specs.Length; i++)
                {
                    string StrReplace = lst[i].Value.ToString();
                    string StrNewText = "";
                    switch (strLabel.ToLower())
                    {
                        case "%ctesttime%"://元素名0
                            StrNewText = specList.Specs[i].SpecTime.ToString("f0");
                            StrReplace = StrReplace.Replace(strLabel, StrNewText);
                            lst[i].PutValue(StrReplace);
                            break;
                        case "%cvoltage%":
                            StrNewText = specList.Specs[i].TubVoltage.ToString("f0");
                            StrReplace = StrReplace.Replace(strLabel, StrNewText);
                            lst[i].PutValue(StrReplace);
                            break;
                        case "%ccurrent%":
                            StrNewText = specList.Specs[i].TubCurrent.ToString("f0");
                            StrReplace = StrReplace.Replace(strLabel, StrNewText);
                            lst[i].PutValue(StrReplace);
                            break;

                    }
                    
                }

            }
       
        }

        protected void ReplaceCellText(ref Worksheet worksheet, string strLabel, string strInputText)
        {
            Cell cell = worksheet.Cells.FindStringContains(strLabel, null);
            if (cell != null)
            {
                string strConent = cell.Value.ToString();
                strConent = strConent.Replace(strLabel, strInputText);
                cell.PutValue(strConent);
                // worksheet.AutoFitRows(cell.Row, cell.Row);
            } 
        }
        protected virtual void ReplaceElementsTable(ref Worksheet worksheet, string strLabel)
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
            if (ElementNames.Count <= 0)
            {
                var elelist = Elements.Items.ToList().OrderBy(w => w.RowsIndex);
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
                    
                    //if (WorkCurveHelper.IsPdRh && (em.Caption.ToUpper().Equals("RH") || em.Caption.ToUpper().Equals("PD"))) continue;
                    ElementNames.Add(em.Caption);
                }
            }
            else
            {
                List<string> remove = new List<string>();
                foreach (var en in ElementNames)
                {
                    var em= Elements.Items.ToList().OrderBy(w => w.Caption.ToLower().CompareTo(en.ToLower())==0);
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
            if (strLabel.ToLower().CompareTo("%results%")==0&&historyStandID > -1 && historyRecordid != "") //判定结果时
            {
                if (WorkCurveHelper.isShowXRFStandard ==1)
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
                        StrNewText =IsAnalyser?elementName: elementName + "(" + strContentU + ")";
                        if (ReportTemplateHelper.ExcelModeType == 18)
                        {
                            Style styleTemp = lst[i].GetDisplayStyle();
                            styleTemp.ForegroundColor = Color.Gray;
                            styleTemp.Pattern = BackgroundType.ThinDiagonalCrosshatch;
                            //styleTemp.Borders.SetColor(Color.Black);
                            //styleTemp.Borders.SetStyle(CellBorderType.Thin);
                            //styleTemp.BackgroundThemeColor = Color.LightGray;
                            //styleTemp.BackgroundColor = Color.LightGray;
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
                                StrNewText = em.Content.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(" + sunit + ")";
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
                                StrNewText = (em.Error*10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                                break;
                            case ContentUnit.permillage:
                                StrNewText = (em.Error*10).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
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
                                //StrNewText = "0-0";
                                StrNewText = "  ";
                        }
                        //}
                        else
                            StrNewText = " ";
                        StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        lst[i].PutValue(StrNewText);
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
                            //StrNewText = "0";
                            StrNewText = "  ";
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
                                StrNewText = standSample.StandardThick.ToString() +" -" +standSample.StandardThickMax.ToString();
                            else
                                StrNewText = "0-0";
                        }
                        //}
                        else
                            StrNewText = " ";
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
                        StrNewText=(strJust == null) ? "" : strJust;
                        StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        lst[i].PutValue(StrReplace);
                        break;
                    case "%average%"://含量平均值12
                        List<HistoryElement> hElementList = elementListPDF.FindAll(delegate(HistoryElement v) { return v.elementName == em.Caption; });
                        double dEAverage = 0;
                        
                        foreach (HistoryElement he in hElementList)
                        {
                            double dValue = 0.0;
                            double.TryParse(he.contextelementValue, out dValue);
                            dEAverage += Math.Round(dValue, WorkCurveHelper.SoftWareContentBit);
                        }
                        dEAverage = hElementList.Count <= 0 ? 0 : (dEAverage / hElementList.Count);
                        if (IsAnalyser && !(this is LYReport))
                        {
                            sunit = "";
                            sunit = (hElementList[0].unitValue.ToString() == "1") ? "%" : ((hElementList[0].unitValue.ToString() == "3") ? "‰" : "ppm");

                        }

                        if (sunit == "")
                        { lst[i].PutValue(Math.Round(dEAverage, WorkCurveHelper.SoftWareContentBit)); }
                        else
                        {
                            StrNewText = dEAverage.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + ((sunit == "") ? "" : "(" + sunit + ")");
                            StrReplace = StrReplace.Replace(strLabel, StrNewText);
                            lst[i].PutValue(StrReplace);
                        }
                        
                        //StrNewText = dEAverage.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + ((sunit == "") ? "" : "(" + sunit + ")");
                        //em.Content = dEAverage;
                        ////if(em.Caption.ToLower().Equals("au"))
                        ////{
                        ////    double dKaAver = 0;
                        ////    dKaAver = (hElementList[0].unitValue.ToString() == "1") ? dEAverage : ((hElementList[0].unitValue.ToString() == "3") ? dEAverage / 10 : dEAverage / 10000);
                        ////    double dKValue = dKaAver * 24 / WorkCurveHelper.KaratTranslater;
                        ////    ReplaceCellText(ref ws, "%Karat%", dKValue.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(k)");
                        ////}
                        //StrReplace = StrReplace.Replace(strLabel, StrNewText);
                        //lst[i].PutValue(StrReplace);
                        break;
                    case "%weight%"://重量8
                        double dEleWeight = 0;
                        if (dWeight > 0)
                        {
                            double dEleContent = em.Content;
                            if (em.ContentUnit.ToString() == "permillage") dEleContent = dEleContent / 10;

                            dEleWeight = (dWeight ?? 0.0) / 100 * float.Parse(dEleContent.ToString());
                        }
                        StrNewText =dEleWeight.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(g)";
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
                    //case "%elemallname"://元素全名
                    //    break;

                }
                //StrReplace = StrReplace.Replace(strLabel, StrNewText);
                //lst[i].PutValue(StrReplace);
                if (ReportTemplateHelper.ExcelModeType == 18)
                {
                    Style styleTemp = lst[i].GetDisplayStyle();
                    //styleTemp.Borders.SetColor(Color.Black);
                    styleTemp.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    styleTemp.Borders[BorderType.LeftBorder].Color = Color.Black;
                    styleTemp.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    styleTemp.Borders[BorderType.RightBorder].Color = Color.Black;
                    styleTemp.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    styleTemp.Borders[BorderType.TopBorder].Color = Color.Black;
                    styleTemp.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                    styleTemp.Borders[BorderType.BottomBorder].Color = Color.Black;
                    lst[i].SetStyle(styleTemp);
                }
               
            }

        }
        
        protected enum DeleteEmptyType
        {
            Row = 0,
            Column =1,
            Both = 2,
        }
       

        private DataTable CreateReTestTable(ElementList elementList)
        {

            DataTable reTestTable = new DataTable();
            reTestTable.Columns.Clear();
            reTestTable.Columns.Add("Time", typeof(string));
            foreach (CurveElement curEle in elementList.Items)
            {
                if (!reTestTable.Columns.Contains(curEle.Caption))
                    reTestTable.Columns.Add(curEle.Caption, typeof(string));
            }
            return reTestTable;
        }

        protected void GetReportInfo(ref Dictionary<string, string> dReportOtherInfo)
        {
            string sReportPath = AppDomain.CurrentDomain.BaseDirectory + "//printxml//CompanyInfo.xml";
            XmlDocument xmlDocReport = new XmlDocument();
            xmlDocReport.Load(sReportPath);
            string strWhere = "";
            if (ReportTemplateHelper.ExcelModeType == 2)
                strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.LoadReportName() + "']";
            else
                strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.ExcelModeType + "']";            

            XmlNodeList xmlnodelist = xmlDocReport.SelectNodes(strWhere);
            if (xmlnodelist == null || xmlnodelist.Count == 0) return;
            
            foreach (XmlNode xmlnode in xmlnodelist)
            {
                //获取支节点信息
                foreach (XmlNode childxmlnode in xmlnode.ChildNodes)
                {
                    string sd = childxmlnode.Attributes[WorkCurveHelper.LanguageShortName].Value;
                    if (childxmlnode.Attributes["Target"] == null || childxmlnode.Attributes["Target"].Value == null) continue;
                    string sTarget = childxmlnode.Attributes["Target"].Value;
                    string strsql = "select * from historycompanyotherinfo a " +
                                                                   " left outer join companyothersinfo b on b.id=a.companyothersinfo_id " +
                                                                   " where a.workcurveid in (select workcurveid from historyrecord where id='" + historyRecordid + "') and a.history_id='" + historyRecordid + "' and b.ExcelModeTarget='" + sTarget + "' and b.isreport=1";
                    List<HistoryCompanyOtherInfo> hisCompanyOtherInfoList = HistoryCompanyOtherInfo.FindBySql(strsql);
                    string sReplaceText = "";
                    if (hisCompanyOtherInfoList != null && hisCompanyOtherInfoList.Count > 0)
                        sReplaceText = hisCompanyOtherInfoList[0].ListInfo;
                    dReportOtherInfo.Add(sTarget, sReplaceText);


                }
            }
        }

        
        

        ///// <summary>
        ///// 生成多次测量报告
        ///// </summary>
        ///// <param name="fileName">保存文件名，为空时直接打印</param>
        //public  abstract void GenerateRetestReport(string fileName, DataTable dataTable, bool flag, int count, bool samenessSample);




        #region 老版本的报告
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="templateName"></param>
        ///// <param name="fileName"></param>
        ///// <param name="selectHisRecordId"></param>
        ///// <returns></returns>
        //public string GenerateGenericReport(string templateName, string fileName, List<long> selectHisRecordId)
        //{
        //    if (TxtFile.IsExcelInstall()) //添加判断是否安装了Excel 0319
        //    {
        //        this.selectLong = selectHisRecordId;
        //        this.ReportPath = WorkCurveHelper.ExcelPath;
        //        this.TempletFileName = templateName;
        //        string newFileName = fileName;
        //        string sFileSaveName = ReportPath + newFileName + (selectLong.Count > 1 ? "_Retry" : ""); //+ ".xls"
        //        try
        //        {
        //            if (!System.IO.File.Exists(TempletFileName))
        //            {
        //                Msg.Show(Info.TemplateNoExists);
        //                return string.Empty;
        //            }
        //            Workbook wb = new Workbook(FileFormatType.Excel97To2003);
        //            wb.Open(templateName);
        //            string mes = string.Empty;
        //            System.Diagnostics.Stopwatch watcher = new System.Diagnostics.Stopwatch();
        //            watcher.Start();
        //            AddDefaultDataSourceObjects();  //添加默认数据源
        //            watcher.Stop();
        //            mes = watcher.ElapsedMilliseconds.ToString();
        //            watcher.Reset();
        //            watcher.Start();
        //            AppendRepeatRows();//追加可能不够的行
        //            watcher.Stop();
        //            mes += ":" + watcher.ElapsedMilliseconds.ToString();
        //            watcher.Reset();
        //            watcher.Start();
        //            FillReportProperties();//填充属性为主的数据
        //            watcher.Stop();
        //            mes += ":" + watcher.ElapsedMilliseconds.ToString();
        //            watcher.Reset();
        //            watcher.Start();
        //            if (File.Exists(sFileSaveName))
        //            {
        //                File.Delete(sFileSaveName);
        //            }
        //            if (!newFileName.Equals(string.Empty))
        //            {
        //                //excelReport.SaveAs(sFileSaveName);//保存
        //                sFileSaveName = excelReport.SaveReport(sFileSaveName, ReportFileType);

        //            }
        //            else
        //            {
        //                //excelReport.PrintReport();//打印
        //            }
        //            watcher.Stop();
        //            mes += ":" + watcher.ElapsedMilliseconds.ToString();
        //            //if(System.Diagnostics.Debugger.IsAttached) MessageBox.Show(mes);
        //        }
        //        catch //(Exception ex)
        //        {
        //            //Console.WriteLine("save fail!");
        //            //Msg.Show(ex.ToString());
        //            sFileSaveName = string.Empty;
        //        }
        //        finally
        //        {
        //            //excelReport.CloseFile();
        //            excelReport.stopExcel();
        //        }
        //        return sFileSaveName;
        //    }
        //    else
        //    {
        //        Msg.Show(Info.ExcelInstallHint);
        //        return string.Empty;
        //    }


        //}
        //private void AddDefaultDataSourceObjects()
        //{
        //    long HistoryId = selectLong.FirstOrDefault();//single value  
        //    HistoryRecord record = HistoryRecord.FindById(HistoryId).ConvertToType<HistoryRecord>();
        //    if (record != null)
        //    {
        //        SpecListEntity entiy = DataBaseHelper.QueryByEdition(record.SpecListName, record.FilePath, record.EditionType);
        //        this.Spec = entiy.Specs.FirstOrDefault();
        //        lstObjs.Add(Spec);//single value
        //        lstObjs.Add(entiy);//single value
        //        lstObjs.Add(Spec.DeviceParameter);
        //        lstObjs.Add(record);
        //        WorkCurve workCurve = (WorkCurve)WorkCurve.FindById(record.WorkCurveId);//single value
        //        lstObjs.Add(workCurve);
        //        if (workCurve != null)
        //            this.WorkCurveID = workCurve.Id.ToString();//公司其他信息中用到            
        //        this.historyRecordid = HistoryId.ToString();//公司其他信息中用到
        //        lstObjs.Add(this.Elements);//感兴趣元素由外部决定，workCurve中的不一定包含历史记录所有的 需外部处理
        //        foreach (var ele in Elements.Items.OrderBy(w => w.RowsIndex))
        //        {
        //            lstObjs.Add(ele);
        //        }
        //        if (selectLong.Count > 0)
        //        {
        //            HistoryStatistics historyStatics = new HistoryStatistics(selectLong, this.Elements, (StandardJudgeType)WorkCurveHelper.iResultJudgingType);
        //            foreach (var hisStatisticsEle in historyStatics.ListHistoryStatisticsElement)
        //            {
        //                lstObjs.Add(hisStatisticsEle);
        //            }
        //            lstObjs.Add(historyStatics);
        //        }
        //        lstObjs.RemoveAll(w => w == null);
        //        GetReportProperties();
        //    }
        //}
        ///// <summary>
        ///// 自动增加行
        ///// </summary>
        //private void AppendRepeatRows()
        //{
        //    ResetListRepeatInfo();
        //    var listReplace = NeedAppend(listRepeat);
        //    //先垂直方向增加行
        //    var vRepeats = listRepeat.FindAll(w => (w.Direct & RepeatDirect.VRepeat) == RepeatDirect.VRepeat).ToList();
        //    if (vRepeats == null || vRepeats.Count == 0) goto Horizon;
        //    var dicPointObjects = GetRangeRepeatObjects(vRepeats, RepeatDirect.VRepeat);
        //    int Account = 0;//累计已加行数
        //    foreach (var key in dicPointObjects.Keys.OrderBy(w => w.Y))
        //    {
        //        var Needs = listReplace.FindAll(w => dicPointObjects[key].Contains(w.Obj));
        //        bool needsCount = Needs.All(w => w.CountValue > 0);
        //        int maxNeeds = needsCount ? Needs.Max(w => w.CountValue)
        //            : Needs.Max(w => w.Account);
        //        int lastRow = key.Y + Account;//下移已加行数
        //        int count = maxNeeds - (key.Y - key.X + 1);
        //        int position = lastRow + (count > 0 ? 0 : count);
        //        InsertRows(excelReport.excelWorksheet, position, count, lastRow);
        //        Account += count;
        //    }
        //  //水平重复
        //  Horizon:
        //    ResetListRepeatInfo();
        //    Account = 0;
        //    var hRepeats = listRepeat.FindAll(w => (w.Direct & RepeatDirect.HRepeat) == RepeatDirect.HRepeat).ToList();//
        //    if (hRepeats == null || hRepeats.Count == 0) return;
        //    var dicPointOjbectH = GetRangeRepeatObjects(hRepeats, RepeatDirect.HRepeat);
        //    int hMinRow = hRepeats.Select(w => w.RepeatPosition[RepeatDirect.HRepeat].Min()).Min();
        //    int hMaxRow = hRepeats.Select(w => w.RepeatPosition[RepeatDirect.HRepeat].Max()).Max();
        //    int hCloneBeginRow = 0;
        //    int hCloneEndRow = hMaxRow;
        //    List<RepeatInfo> listRangeRepeat = new List<RepeatInfo>();
        //    List<int> listRowCount = new List<int>();
        //    for (int i = hMinRow; i <= hMaxRow + 1; i++)
        //    {
        //        bool bContinue = hRepeats.Exists(w => w.RepeatPosition[RepeatDirect.HRepeat].Exists(p => p == i));
        //        if (bContinue)
        //        {
        //            if (hCloneBeginRow == 0) hCloneBeginRow = i + Account;
        //            listRangeRepeat.Add(hRepeats.FirstOrDefault(w => w.RepeatPosition[RepeatDirect.HRepeat].Contains(i)));//获取区间内的重复内容

        //        }
        //        if (!bContinue)
        //        {
        //            hCloneEndRow = i - 1 + Account;
        //            var listRangeReplace = listReplace.FindAll(w => listRangeRepeat.Any(p => p.Obj.Equals(w.Obj)));//获取区间内的替换内容
        //            bool needsCount = listRangeReplace.All(w => w.CountValue > 0);
        //            int maxNeedRows = 0;
        //            foreach (var replace in listRangeReplace)
        //            {
        //                var repeat = listRangeRepeat.FirstOrDefault(w => w.Obj.Equals(replace.Obj));//重复对象与对应的替换对象
        //                int needs = needsCount ? replace.CountValue : replace.Account;
        //                var exists = repeat.Ranges[RepeatDirect.HRepeat].FirstOrDefault().Y - repeat.Ranges[RepeatDirect.HRepeat].FirstOrDefault().X + 1;//假设宽度一致
        //                int rowCount = Math.Ceiling(needs * 1.0 / exists).ConvertToType<int>();
        //                listRowCount.Add(rowCount);
        //            }
        //            maxNeedRows = listRowCount.Max() - 1;//已经至少存在一行
        //            for (int iRow = 0; iRow < maxNeedRows; iRow++)
        //                for (int iPosition = hCloneEndRow; iPosition >= hCloneBeginRow; iPosition--)
        //                {
        //                    InsertRows(excelReport.excelWorksheet, i, 1, iPosition);
        //                }
        //            Account += (hCloneEndRow - hCloneBeginRow + 1) * maxNeedRows;
        //            listRangeRepeat.Clear();
        //            hCloneBeginRow = 0;
        //        }
        //    }

        //}
        ///// <summary>
        ///// 填充数据源信息至Excel
        ///// </summary>
        //private void FillReportProperties()
        //{
        //    if (ReportBeforeDataBind != null)
        //        ReportBeforeDataBind(this, new EventArgs());
        //    //var lstValues = excelReport.GetUsedRangeValue();
        //    XDocument doc = LoadPropertyAllFile();
        //    //过滤数据源
        //    ResetListRepeatInfo();
        //    //lstObjs = lstObjs.FindAll(w => listReplace.Exists(r => r.ClassName == w.GetType().Name));
        //    foreach (object o in lstObjs)
        //    {
        //        PropertyInfo[] pros = o.GetType().GetProperties();
        //        string sTypeName = "";//o.GetType().Name + ".";
        //        Excel.Range rngAtom = null;
        //        List<string> existItems = new List<string>();//TableItems
        //        foreach (var pro in pros)
        //        {
        //            string tag = string.Empty;
        //            Excel.Range rng = null;
        //            string tags = GetPropertyTags(doc, pro).Replace("%", "");//获取字段对应的标签去除“%” 
        //            FindValidStringRangPair(sTypeName, tags, ref tag, ref rng);
        //            object obj = pro.GetValue(o, null);
        //            string sValue = string.Empty;
        //            if (obj == null) continue;
        //            Type objType = obj.GetType();
        //            if (rng != null)
        //            {
        //                if (obj.GetType().IsGenericType && objType.GetInterface("IEnumerable") != null)//如果是泛型 集合 元素
        //                {
        //                    object count = objType.GetProperty("Count").GetValue(obj, null);
        //                    int iValue = 0;
        //                    int.TryParse(count.ToString(), out iValue);
        //                    for (int iGen = 0; rng != null && iGen < iValue; iGen++)//元素关联项
        //                    {
        //                        if (rngAtom == null && existItems.Exists(w => string.Compare(w, pro.Name, true) == 0)) break;//预留元素个数不够 属于TableItems但TableHead不存在
        //                        else if (rngAtom == null) rngAtom = rng;//否则如果是TableItems外的查找自己的二维单元格
        //                        rng = excelReport.FindTwoDRng(rngAtom, "%" + sTypeName + tag + "%");
        //                        if (rng == null) break;//预留历史记录个数不够
        //                        object item = objType.GetProperty("Item").GetValue(obj, new object[] { iGen });
        //                        sValue = item == null ? "" : object.Equals(item, double.NaN) ? "ND" : item.ToString();//判断ND值
        //                        FillMatchRange(sTypeName, tag, rng, sValue);
        //                    }
        //                    if (ReportHistoryRecordItemBinded != null)
        //                        ReportHistoryRecordItemBinded(this, new EventArgs());
        //                }
        //                else//简单类型
        //                {
        //                    sValue = obj.ToString();
        //                    //if (Atoms.GetAtom(sValue) != null || Oxide.FindOne(w => w.OxideName == sValue) != null) rngAtom = rng;
        //                    if (IsTableHeader(pro, ref existItems))
        //                        rngAtom = rng;
        //                    FillMatchRange(sTypeName, tag, rng, sValue);
        //                }
        //            }
        //            #region 统计方法
        //            //if (rngAtom == null) continue;//元素统计信息    
        //            if (!(obj.GetType().IsGenericType && objType.GetInterface("IEnumerable") != null)) continue;//非集合继续循环
        //            if (objType.GetProperty("Count").GetValue(obj, null).ConvertToType<int>() <= 1) continue;//如果集合个数小于2继续循环
        //            //List<string> listStatistics = new List<string> { "Max", "Min", "Average", "SD", "RSD" };//元素统计信息数据源
        //            var listMethodNames = ListStatisticsMethods.ConvertAll<string>(w => w.Method.Name);//获取新增统计信息方法名称
        //            listStatistics.AddRange(listMethodNames);
        //            foreach (string statistics in ListStatistics)
        //            {
        //                //rng = excelReport.FindRang("%" + sTypeName + tag + statistics + "%");//当选择统计的内容部显示时无法找到该选项//excelReport.FindTwoDRng(rngAtom, "%" + sTypeName + tag + statistics + "%");
        //                var sArray = tags.Split(',');
        //                foreach (var item in sArray)
        //                {
        //                    rng = excelReport.FindRang("%" + sTypeName + item + statistics + "%");
        //                    if (rng != null)
        //                    { tag = item; break; }
        //                }
        //                if (rng != null)
        //                {
        //                    if (statistics == "SD" || statistics == "RSD")
        //                    {
        //                        double SD;
        //                        double RSD;
        //                        GetSDRSDValue(obj, objType, out SD, out RSD);
        //                        if (statistics == "SD")
        //                            sValue = SD.ToString();
        //                        else if (statistics == "RSD")
        //                            sValue = RSD.ToString();
        //                        FillMatchRange(sTypeName, tag, rng, sValue, statistics);
        //                    }
        //                    else if (statistics == "Max" || statistics == "Min" || statistics == "Average")
        //                    {
        //                        object result = GetEnumerableStaticMethodValue(statistics, obj);
        //                        sValue = result == null ? "0" : result.ToString();
        //                        FillMatchRange(sTypeName, tag, rng, sValue, statistics);
        //                    }
        //                    else//自定义统计方法
        //                    {
        //                        StatisticsDelegate method = ListStatisticsMethods.Find(w => string.Compare(w.Method.Name, statistics, true) == 0);
        //                        if (method != null)
        //                        {
        //                            sValue = method(obj).ToString();
        //                            FillMatchRange(sTypeName, tag, rng, sValue, statistics);
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion
        //        }
        //    }
        //    FillPictures(excelReport);//填充谱图
        //    FillCompanyOthers(excelReport);//填充公司其他信息
        //    ReplaceExcelMatchText("%.?%|%.+%", "");//直接操作Excel单元格
        //    DeleteEmptyRowsOrColumns(DeleteEmptyType.Both);//删除空白行列
        //    Protect(excelReport);//加密Excel文件
        //}

        ///// <summary>
        ///// 填充Excel匹配单元格值
        ///// </summary>
        ///// <param name="sTypeName"></param>
        ///// <param name="pro"></param>
        ///// <param name="rng"></param>
        ///// <param name="sValue"></param>
        ///// <param name="suffix"></param>
        //private static void FillMatchRange(string sTypeName, string tag, Excel.Range rng, string sValue, params string[] suffix)
        //{
        //    if (rng.Value2.GetType().IsArray)//单元格类型
        //    {
        //        var value = ((object[,])rng.Value2)[1, 1];//合并单元格
        //        rng.Value2 = value.ToString().Replace("%" + sTypeName + tag + suffix.FirstOrDefault() + "%", sValue);
        //    }
        //    else
        //        rng.Value2 = rng.Value2.ToString().Replace("%" + sTypeName + tag + suffix.FirstOrDefault() + "%", sValue);

        //}

        //public void FillPictures(VkExcel excelReport)
        //{
        //    //填充样品图
        //    Excel.Range rang = excelReport.FindRang("%SamplePhoto%");
        //    if (rang != null && this.specList != null)
        //    {
        //        //if (!this.specList.ImageShow && this.specList.Image != null)
        //        //{
        //        //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(this.specList.Image))
        //        //    {
        //        //        Bitmap b = new Bitmap(ms);
        //        //        InsertImage(ref rang, b);
        //        //    }
        //        //}
        //        if (this.specList.ImageShow)
        //        {
        //            string fileNameFull = WorkCurveHelper.SaveSamplePath + "\\" + this.specList.Name + ".jpg";
        //            FileInfo infoIf = new FileInfo(fileNameFull);
        //            if (infoIf.Exists)
        //            {
        //                Image tempImage = Image.FromFile(fileNameFull);
        //                InsertImage(ref rang, tempImage);
        //            }
        //        }
        //    }
        //    //填充谱图
        //    rang = excelReport.FindRang("%Spectrum%");
        //    if (rang != null)
        //    {
        //        Bitmap bmp = new Bitmap(120, 60);
        //        Graphics g = Graphics.FromImage(bmp);
        //        int width = (int)Math.Round(Convert.ToInt32(rang.Width) * g.DpiX / 72.0);
        //        int height = (int)Math.Round(Convert.ToInt32(rang.Height) * g.DpiY / 72.0);
        //        bmp = new Bitmap(width, height);
        //        DrawSpec(ref bmp);
        //        excelReport.InsertPicture(rang, bmp);
        //        //excelReport.
        //    }

        //    //填充元素图
        //    rang = excelReport.FindRang("%ElemSpec%");
        //    if (rang != null)
        //    {
        //        Bitmap bmp = new Bitmap(120, 60);
        //        Graphics g = Graphics.FromImage(bmp);
        //        int width = (int)Math.Round(Convert.ToInt32(rang.Width) * g.DpiX / 72.0);
        //        int height = (int)Math.Round(Convert.ToInt32(rang.Height) * g.DpiY / 72.0);
        //        bmp = new Bitmap(width, height);
        //        DrawInterstringElems(ref bmp);
        //        excelReport.InsertPicture(rang, bmp);
        //    }

        //}

        //public void FillCompanyOthers(VkExcel excelReport)
        //{
        //    //公司其它信息
        //    Dictionary<string, string> dReportOtherInfo = new Dictionary<string, string>();
        //    GetReportInfo(ref dReportOtherInfo);

        //    foreach (string sKey in dReportOtherInfo.Keys)
        //    {
        //        if (excelReport.FindText("%" + sKey + "%").Count > 0)
        //            excelReport.ReplaceText("%" + sKey + "%", dReportOtherInfo[sKey]);
        //    }
        //}

        #endregion
        
      
        /// <summary>
        /// 是否需要列头枚举
        /// </summary>
        public enum NeedsTitle
        {
            /// <summary>
            /// 无需表头
            /// </summary>
            None,
            /// <summary>
            /// 只需一次表头
            /// </summary>
            OnlyOnce,
            /// <summary>
            /// 数据和表头始终以其显示
            /// </summary>
            Always,
            /// <summary>
            /// 整体换行
            /// </summary>
            Total,
            /// <summary>
            /// 结束状态
            /// </summary>
            Finished
        }
      
        /// <summary>
        /// 导出EXCEL总的列数
        /// </summary>
        int iTotalColumnMun = 0;
        ///// <summary>
        ///// 生成多次测量报告
        ///// </summary>
        ///// <param name="fileName">保存文件名，为空时直接打印</param>
        //public abstract void GenerateRetestReport_Brass(string fileName, List<BrassReport> brassReportList, DataTable dt_BrassStatistics, bool flag);

        //public abstract void GenerateThickRetestReport(string fileName, DataTable dataTable, bool flag);
     

        public ReportDirection Direction = ReportDirection.Vertical;
        public double dColumnWidth = Math.Max((int)ReportDirection.Horizontal * 1.0 / 14,
            (int)ReportDirection.Vertical * 1.0 / 9);
        /// <summary>
        /// 报表方向
        /// </summary>
        public enum ReportDirection
        {
            /// <summary>
            /// 横向 ExcelCOM组件中的宽度
            /// </summary>
            Horizontal =110,//横向A4纸列宽总数
            /// <summary>
            /// 纵向 ExcelCOM组件中的宽度
            /// </summary>
            Vertical = 70//纵向A4纸列宽总数
        }

        /// <summary>
        /// 正则表达式替换Excel工作薄匹配内容
        /// </summary>
        /// <param name="workSheet">需要操作的Excel工作簿</param>
        /// <param name="match">匹配字符串</param>
        /// <param name="replacement">替换内容</param>
        protected void ReplaceExcelMatchText(ref Worksheet workSheet, string match, string replacement)
        {
            if (workSheet.Cells.Count<=0) return;
            for (int i = 0; i < workSheet.Cells.Count; i++)
            {
                object obj = workSheet.Cells[i].Value;
                if (obj != null && System.Text.RegularExpressions.Regex.IsMatch(obj.ToString(), match))
                {
                    workSheet.Cells[i].Value = replacement;
                }
            }
        }

        /// <summary>
        /// 制作不确定度报告
        /// </summary>
        /// <param name="fileName">模板路径</param>
        /// <param name="sDataTable">标准样数据</param>
        /// <param name="rDataTable">实际测试数据</param>
        /// <param name="flag"></param>
        public void GenerateUncertaintyReport(string fileName, DataTable sDataTable, DataTable rDataTable, bool flag, double[] Y, double[] X, UncertaintyResult UResult, string similarCurve)
        {
            if (TxtFile.IsExcelInstall()) //添加判断是否安装了Excel 0319
            {
                try
                {
                    if (!System.IO.File.Exists(fileName))
                    {
                        Msg.Show(Info.TemplateNoExists);
                        return;
                    }
                    Workbook wb = new Workbook(FileFormatType.Excel97To2003);
                    wb.Open(fileName);
                    Worksheet ws = wb.Worksheets[0];  

                    //excelReport = new VkExcel(false);
                    //excelReport.OpenFile(fileName, "");

                    int iTotalColumnMun = 5;
                    int startIndex = 3;
                    NeedsTitle needsTitle = NeedsTitle.OnlyOnce;//是否需要列名

                    //Workbook wbook = excelReport.excelWorkbook;
                    //Excel.Worksheet sheet = (Excel.Worksheet)wbook.Sheets[1];
                    string standradRowNum = "A1";
                    //Excel.Range range = sheet.get_Range(standradRowNum, Missing.Value);
                    Cell range = FindTextCell(ws, "%Chart1%");
                    if (range != null)
                    {
                        startIndex = range.Row;
                        InsertDataTable(ws, sDataTable, startIndex, iTotalColumnMun, false, "A");
                    }

                    range = FindTextCell(ws,"%Chart2%");
                    if (range != null)
                    {
                        int rowcount = range.Row;
                        InsertDataTable(ws, rDataTable, rowcount, 3, true, "A");
                    }


                    //var table = SplitDataRow(sDataTable, iTotalColumnMun, ref needsTitle, false);
                    //if (table == null || table.Rows.Count == 0) return;
                    //range = sheet.get_Range("B" + startIndex, Missing.Value);
                    //object[,] cellsValue = new object[table.Rows.Count, table.Columns.Count];
                    //int cols = 0;
                    //for (int i = 0; i < table.Columns.Count; i++)
                    //{
                    //    for (int j = 0; j < table.Rows.Count; j++)
                    //    {
                    //        cellsValue[j, cols] = table.Rows[j][i].ToString();
                    //    }
                    //    cols++;
                    //}

                    //InsertRows(sheet, startIndex, table.Rows.Count, -1);//至少留有一个标题和内容

                    //range = sheet.get_Range("B" + startIndex, Missing.Value);
                    //range = range.get_Resize(table.Rows.Count, table.Columns.Count);
                    //range.Font.Size = 10;
                    //range.set_Value(Missing.Value, cellsValue);
                    //range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
                    //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;
                    //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                    //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
                    //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;
                    //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                    //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
                    //range.RowHeight = 20;
                    ////ReplaceExcelMatchText(sheet, "%.?%|%.+%", "");//直接操作Excel单元格{遍历次数太多，耗费资源 Strong 2012/10/12}


                    Bitmap bmpSpectrum = null;
                    //填充谱图
                    range = FindTextCell(ws,"%Photo%");
                    if (range != null)
                    {
                        Bitmap bmp = new Bitmap(120, 60);
                        Graphics g = Graphics.FromImage(bmp);
                        //int width = (int)Math.Round(Convert.ToInt32(range.) * g.DpiX / 72.0);
                        //int height = (int)Math.Round(Convert.ToInt32(range.Height) * g.DpiY / 72.0);
                        bmp = new Bitmap(400, 300);
                        DrawAdjustCurve(Y, X, ref bmp);

                        //range.Select();
                        System.Windows.Forms.Clipboard.SetDataObject(bmp, true);
                        //bmp.Save("D:\\photo.jpg");
                        //sheet.Paste(range, bmp);

                        ReplaceCellImage(ref ws, "%Photo%", bmp);
                        //if (dataTable.Rows.Count >= 1)
                        //    bmpSpectrum = bmp;
                        //else
                        //   excelReport.InsertPicture(range, bmp);           
                    }

                    //Excel.Range range2 = excelReport.FindRang(sheet, "%Photo%");
                    //if (range2 != null && bmpSpectrum != null)
                    //{
                    //    range2.Select();
                    //    System.Windows.Forms.Clipboard.SetDataObject(bmpSpectrum, true);
                    //    bmpSpectrum.Save("D:\\photo.jpg");
                    //    sheet.Paste(range2, bmpSpectrum);
                    //}


                    ReplaceCellText(ref ws, "%Ua%", "测试重复性与样品均匀性不确定度 Ua=" + UResult.Uncertainty.Ua.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%");
                    ReplaceCellText(ref ws,"%Ub%", "标准物质不确定度Ub以接近样品测量值的 " + similarCurve + " 为准Ub=" + UResult.Uncertainty.Ub.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%");
                    ReplaceCellText(ref ws, "%Uc%", "校正曲线先行不确定度 Uc=" + UResult.Uncertainty.Uc.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%");
                    ReplaceCellText(ref ws, "%Ud%", "样品不一致性引入的不确定度 Ud=" + UResult.Uncertainty.Ud.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%");
                    ReplaceCellText(ref ws, "%Ux%", "合成不确定度 Ux=" + UResult.Uncertainty.Ux.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%");
                    ReplaceCellText(ref ws, "%U%", "扩展不确定度为 U=" + UResult.Uncertainty.U.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "% (K=2)");
                    ReplaceCellText(ref ws, "%Result%", "最终测量结果范围可表示为 (" + UResult.AveResult.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "±" + UResult.Uncertainty.U.ToString("F" + WorkCurveHelper.SoftWareContentBit) + ")% (K=2)");

                    ReplaceCellText(ref ws, "%Chart1%", string.Empty);
                    ReplaceCellText(ref ws, "%Chart2%", string.Empty);
                    DeleteEmptyRowsOrColumns(ref ws, DeleteEmptyType.Both);
                    //range = FindTextCell(ws,"%Chart1%");
                    //if (range != null)
                    //    range.EntireRow.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);

                    //range = excelReport.FindRang("%Chart2%");
                    //if (range != null)
                    //    range.EntireRow.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);

                    
                    SaveFileDialog dialog = null;
                    try
                    {
                        if (flag)
                        {
                            dialog = new SaveFileDialog();
                            dialog.Filter = "Excel|*.xls";
                            dialog.ShowDialog();
                            wb.Save(dialog.FileName);
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



                        //if (ReportTemplateHelper.IsEncryption)
                        //{
                        //    excelReport.Protect(GetPassWord()); //保护工作表
                        //}
                        //excelReport.CloseFile();

                    }
                    finally
                    {
                        //excelReport.stopExcel();
                        if (dialog != null)
                        {
                            if (Skyray.Controls.SkyrayMsgBox.Show(Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                string excelOnePath = dialog.FileName;
                                Help.ShowHelp(null, excelOnePath);
                            }
                        }
                    }
                }
                catch (Exception e)
                { Msg.Show(e.ToString()); }
            }


        }

        public void DrawAdjustCurve(double[] YArray, double[] XArry, ref Bitmap bitmap)
        {
            double A = 0;
            double B = 0;
            Report.LineFitting(XArry, YArray, XArry.Length, ref A, ref B);

            #region 画坐标

            int intSpace = 20;//间距
            Graphics g = Graphics.FromImage(bitmap);
            Brush backBrush = new SolidBrush(Color.White);
            System.Drawing.Font font = new System.Drawing.Font("Tahoma", 10, FontStyle.Regular);
            Pen linePen = new Pen(Color.Black);
            Brush textBrush = new SolidBrush(Color.Black);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            g.FillRectangle(backBrush, rect);
            Rectangle rectLine = new Rectangle(1, 1, bitmap.Width - 2, bitmap.Height - 2);
            g.DrawRectangle(linePen, rectLine);
            Size textSize = g.MeasureString("1256", font).ToSize(); //字符串大小
            int textWidth = textSize.Width;
            int textHeight = textSize.Height;
            int wMargin = textWidth; ;//左右边距
            int hMargin = textHeight + 5;//上下边距

            int orgx = wMargin;
            int orgy = bitmap.Height - hMargin;

            int tempWidth = Convert.ToInt32(bitmap.Width - 2 * orgx);// 谱图实际宽度
            int tempHeight = bitmap.Height - 2 * hMargin;//谱图实际高度

            int xCount = 5, yCount = 5;
            int xorgValue = (int)XArry.Min() / 10;
            xorgValue *= 10;
            int xInternalValue = (100 - xorgValue) / xCount;

            int yorgValue = (int)YArray.Min() / 10;
            yorgValue *= 10;
            int yInternalValue = (100 - yorgValue) / yCount;

            //int xorgValue = 30, yorgValue = 30;
            //int xInternalValue = 10, yInternalValue = 10;
            //int[] YMesure = new int[] {30, 50, 70, 90, 110 };

            g.DrawLine(linePen, orgx, orgy, bitmap.Width - orgx, orgy); //X轴线
            g.DrawLine(linePen, orgx, orgy, orgx, bitmap.Height - orgy); //Y轴线

            int titleW = 0;

            for (int i = 0; i <= xCount; i++) //画X轴刻度
            {
                int k = xorgValue + i * xInternalValue;
                titleW = g.MeasureString(k.ToString(), font).ToSize().Width;
                int x = orgx + (int)Math.Round(1.0 * tempWidth * i / xCount);
                g.DrawLine(linePen, x, orgy, x, orgy - 4);
                g.DrawString(k.ToString(), font, textBrush, x - textWidth / 4, orgy);
            }

            for (int j = 0; j <= yCount; j++) //画Y轴刻度
            {
                int k = yorgValue + j * yInternalValue;
                titleW = g.MeasureString(k.ToString(), font).ToSize().Width;
                int y = bitmap.Height - (hMargin + (int)Math.Round(1.0 * tempHeight * j / yCount));
                g.DrawLine(linePen, orgx, y, orgx + 4, y);
                g.DrawString(k.ToString(), font, textBrush, textWidth / 3, y - textHeight / 2);
            }


            #endregion

            #region  画校正曲线

            Point p1 = new Point(xorgValue, (int)Math.Round(A * xorgValue + B));
            Point p2 = new Point((int)Math.Round((100 - B) / A), 100);

            float ratex = 1.0f * tempWidth / ((xCount) * xInternalValue);
            float ratey = 1.0f * tempHeight / ((yCount) * yInternalValue);

            g.DrawLine(linePen, Math.Abs(p1.X - xorgValue) * ratex + orgx, orgy - Math.Abs(p1.Y - yorgValue) * ratey, Math.Abs(p2.X - xorgValue) * ratex + orgx, orgy - Math.Abs(p2.Y - yorgValue) * ratey);

            #endregion

            for (int n = 0; n < XArry.Length; n++)  //画点
            {
                g.FillEllipse(textBrush, (float)Math.Abs((XArry[n] - xorgValue) * ratex) + orgx - 2, orgy - (float)Math.Abs((YArray[n] - yorgValue) * ratey) - 2, 4, 4);
            }

            g.DrawString("y = " + A.ToString("F" + WorkCurveHelper.SoftWareContentBit) + (B >= 0 ? " x + " : " x ") + B.ToString("F" + WorkCurveHelper.SoftWareContentBit), font, textBrush, bitmap.Width - 6 * textWidth, hMargin);

            PointF[] pf = new PointF[YArray.Length];
            for (int m = 0; m < YArray.Length; m++)
                pf[m] = new PointF((float)XArry[m], (float)YArray[m]);
            var R = Math.Pow(MathFunc.CalcCoefficientR(pf), 2d);
            g.DrawString("R^2 = " + R.ToString("F" + WorkCurveHelper.SoftWareContentBit), font, textBrush, bitmap.Width - 6 * textWidth, hMargin + textHeight + 5);
        }

        public void InsertDataTable(Worksheet sheet, DataTable sDataTable, int startIndex, int iTotalColumnMun, bool hasHeadName, string columnHead)
        {
            NeedsTitle needsTitle = NeedsTitle.OnlyOnce;//是否需要列名
            var table = SplitDataRow(sDataTable, iTotalColumnMun, ref needsTitle, hasHeadName,false);
            if (table == null || table.Rows.Count == 0) return;
            object[,] cellsValue = new object[table.Rows.Count, table.Columns.Count];
            int cols = 0;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    cellsValue[j, cols] = table.Rows[j][i].ToString();
                }
                cols++;
            }

            ////////
            //InsertRows(ref ws, startIndex + 2, table.Rows.Count - 1, -1);//至少留有一个标题和内容
            //Range range = ws.Cells.CreateRange(startIndex, ws.Cells[0].Column, table.Rows.Count + 1, iTotalColumnMun);
            //range.RowHeight = 20;
            //Style style = ws.Cells.GetCellStyle(startIndex, ws.Cells[0].Column);
            //style.Font.Size = 10;
            //style.HorizontalAlignment = TextAlignmentType.Center;
            //style.Borders[BorderType.Horizontal].LineStyle = CellBorderType.Thin;
            //style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            //style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            //style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            //style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            //range.SetStyle(style);
            //ws.Cells.ImportTwoDimensionArray(cellsValue, startIndex, ws.Cells[0].Column);
            ////////

            InsertRows(ref sheet, startIndex, table.Rows.Count, -1);//至少留有一个标题和内容

            Range range = sheet.Cells.CreateRange(startIndex, sheet.Cells[0].Column, table.Rows.Count + 1, iTotalColumnMun);
            range.RowHeight = 20;
            Style style = sheet.Cells.GetCellStyle(startIndex, sheet.Cells[0].Column);
            style.Font.Size = 10;
            style.HorizontalAlignment = TextAlignmentType.Center;
            style.Borders[BorderType.Horizontal].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            range.SetStyle(style);
            sheet.Cells.ImportTwoDimensionArray(cellsValue, startIndex, sheet.Cells[0].Column);
            //range.SetStyle(style);
            //range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
            //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
            //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
            //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;
            //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            //range.Rows.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
            //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
            //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
            //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;
            //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
            //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            //range.Columns.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
            //range.RowHeight = 20;
        }

        public static void LineFitting(double[] x, double[] y, int size, ref double A, ref double B)
        {
            double xmean = 0.0;
            double ymean = 0.0;
            for (int i = 0; i < size; i++)
            {
                xmean += x[i];
                ymean += y[i];
            }
            xmean /= size;
            ymean /= size;

            double sumx2 = 0.0;
            double sumxy = 0.0;
            for (int i = 0; i < size; i++)
            {
                sumx2 += (x[i] - xmean) * (x[i] - xmean);
                sumxy += (y[i] - ymean) * (x[i] - xmean);
            }

            A = sumxy / sumx2;
            B = ymean - A * xmean;
        }

     
}
    public class Generic<T>
   where T : class
    {
        /// <summary>
        /// 获取T类型的属性和对应的用户特性的对应关系Xml
        /// </summary>
        /// <param name="xElem">Xml节点起点</param>
        /// <returns >返回储存T类信息的节点</returns>
        public static XElement GetPropertyCustomerAttribute(XElement xElem)
        {
            XElement xele;
            XElement currentClass = new XElement(typeof(T).Name, new XAttribute("StrongName", typeof(T).FullName + "," + typeof(T).Assembly.FullName));
            if (!xElem.Elements().Contains(currentClass))
            {
                xElem.Add(currentClass);
                xElem = xElem.Element(currentClass.Name);
            }
            Array.ForEach<PropertyInfo>(typeof(T).GetProperties(), p =>
            {
                Auto auto = p.GetCustomAttributes(typeof(Auto), true).FirstOrDefault() as Auto;
                if (auto != null)
                    xElem.Add(new XElement("Name", new XAttribute("id", p.Name), new XAttribute("Description", auto.Text), new XText("%" + p.Name + "%")));
                else
                    xElem.Add(new XElement("Name", new XAttribute("id", p.Name), new XAttribute("Description", ""), new XText("%" + p.Name + "%")));
            });
            xele = xElem;
            return xele;
        }
    }

    public class UncertaintySample
    {
        public Atom atom { get; set; }
        public string SampleName { get; set; }
        public double SampleValue { get; set; }
        public string SampleUncertainty { get; set; }
        public string CurveName { get; set; }
        public double ActualValue { get; set; }
        public static List<UncertaintySample> LoadUnSmple(string label, List<StandSample> sample)
        {
            List<UncertaintySample> sampeList = new List<UncertaintySample>();
            XmlNodeList node = ReportTemplateHelper.LoadNodes("Uncertainty", "Item");
            Dictionary<string, string> CurveSampleDic = new Dictionary<string, string>();
            foreach (XmlNode xnl in node)
            {
                CurveSampleDic.Add(xnl.Attributes["CurveName"].InnerText.ToLower(), xnl.Attributes["Name"].InnerText);
            }
            foreach (StandSample ss in sample)
            {
                if (ss.Active)
                {
                    UncertaintySample us = new UncertaintySample();
                    us.atom = Atoms.AtomList.Find(s => s.AtomName.ToLower() == ss.ElementName.ToLower());
                    us.SampleUncertainty = ss.Uncertainty;
                    us.CurveName = ss.SampleName;
                    us.SampleValue = double.Parse(ss.Y);
                    us.ActualValue = ss.TheoryX;
                    if (node != null && CurveSampleDic.ContainsKey(ss.SampleName.Split('(')[0].ToLower()))
                    {
                        us.SampleName = CurveSampleDic[ss.SampleName.Split('(')[0].ToLower()];
                    }
                    else
                        us.SampleName = ss.SampleName;
                    sampeList.Add(us);

                }
            }

            //foreach (XmlNode xnl in node)
            //{
            //    UncertaintySample us = new UncertaintySample();
            //    us.atom = Atoms.AtomList.Find(s => s.AtomName.ToLower() == label.ToLower());
            //    us.SampleName = xnl.Attributes["Name"].InnerText;
            //    //us.SampleValue = double.Parse(xnl.Attributes["Content"].InnerText);
            //    us.SampleUncertainty = xnl.Attributes["Uncertainty"].InnerText;
            //    us.CurveName = xnl.Attributes["CurveName"].InnerText;
            //    if (sample.Count > 0)
            //    {
            //        StandSample ss = sample.Find(s => s.SampleName.ToLower().Contains(us.CurveName));
            //        if (ss != null)
            //        {
            //            us.SampleValue = double.Parse(ss.Y);
            //            us.ActualValue = ss.TheoryX;
            //        }
            //        else
            //            continue;
            //    }
            //    sampeList.Add(us);
            //}
            return sampeList;
        }

    }

    public class Uncertainty
    {
        public Uncertainty(double ua, double ub, double uc, double ud, double ux, double u)
        {
            Ua = ua;
            Ub = ub;
            Ud = ud;
            Uc = uc;
            Ux = ux;
            U = u;
        }

        public double Ua { get; set; }
        public double Ub { get; set; }
        public double Uc { get; set; }
        public double Ud { get; set; }
        public double Ux { get; set; }
        public double U { get; set; }
    }

    public class UncertaintyResult
    {
        public UncertaintyResult(Uncertainty uc, double aresult)
        {
            this.Uncertainty = uc;
            this.AveResult = aresult;
        }

        public Uncertainty Uncertainty { get; set; }
        public double AveResult { get; set; }
    }

    #region BrassReport
    public class BrassReport
    {
        public string SampleName { get; set; }
        public DateTime TestTime { get; set; }
        public string WorkCurveName { get; set; }
        public string Specification { get; set; }
        public string SupplierName { get; set; }
        public string Weight { get; set; }
        public string Operater { get; set; }
        public string Address { get; set; }
        public DataTable dt_Brass { get; set; }
        public BrassReport(string SampleName, DateTime TestTime, string WorkCurveName, string Specification
            , string SupplierName, string Weight, string Operater, string Address, DataTable dt_Brass)
        {
            this.SampleName = SampleName;
            this.TestTime = TestTime;
            this.WorkCurveName = WorkCurveName;
            this.Specification = Specification;
            this.SupplierName = SupplierName;
            this.Weight = Weight;
            this.Operater = Operater;
            this.Address = Address;
            this.dt_Brass = dt_Brass;
        }
    }
    #endregion
}