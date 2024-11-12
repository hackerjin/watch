using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Skyray.UC
{

    public class DataType
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Weight { get; set; }
    }
    public class HeatMapImage
    {
        /// <summary>
        /// width of img
        /// </summary>
        private int w;

        /// <summary>
        /// height of img
        /// </summary>
        private int h;

        /// <summary>
        /// gaussian kernel size
        /// </summary>
        private int gSize;

        /// <summary>
        /// gaussian kernel sigma
        /// </summary>
        private double gSigma;

        /// <summary>
        /// radius
        /// </summary>
        private int r;

        /// <summary>
        /// Two dimensional matrix corresponding to data list
        /// </summary>
        private double[,] heatVals;

        /// <summary>
        /// Color map matrix
        /// </summary>
        private byte[] ColorArgbValues;

        /// <summary>
        /// gaussian kernel
        /// </summary>
        private double[,] kernel;

        /// <summary>
        /// color numbers
        /// </summary>
        private const int NUMCOLORS = 255;



        /// <summary>
        /// width of img
        /// </summary>
        public int W
        {
            get { return w; }
            set { w = value; }
        }

        /// <summary>
        /// height of img
        /// </summary>
        public int H
        {
            get { return h; }
            set { h = value; }
        }

        /// <summary>
        /// gaussian kernel
        /// </summary>
        public double[,] Kernel
        {
            get { return kernel; }
            set { kernel = value; }
        }

        /// <summary>
        /// Two dimensional matrix corresponding to data list
        /// </summary>
        public double[,] HeatVals
        {
            get { return heatVals; }
            set { heatVals = value; }

        }

        /// <summary>
        /// construction
        /// </summary>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <param name="gSize">gaussian kernel size</param>
        /// <param name="gSigma">gaussian kernel sigma</param>
        public HeatMapImage(int width, int height, int gSize, double gSigma)
        {
            this.w = width;
            this.h = height;
            CreateColorMap();

            //对高斯核尺寸进行判断
            if (gSize < 3 || gSize > 400)
            {
                throw new Exception("Kernel size is invalid");
            }
            this.gSize = gSize % 2 == 0 ? gSize + 1 : gSize;
            //高斯的sigma值，计算半径r
            this.r = this.gSize / 2;
            this.gSigma = gSigma;
            //计算高斯核
            kernel = new double[this.gSize, this.gSize];
            this.gaussiankernel();
            //初始化高斯累加图
            heatVals = new double[h, w];
        }

        /// <summary>
        /// 创建一个ColorMap，用于根据数值查询颜色
        /// </summary>
        private void CreateColorMap()
        {
            ColorBlend colorBlend = new ColorBlend(6);
            colorBlend.Colors = new Color[6]
        {
            //Color.FromArgb(0, 255, 255, 255),
            //Color.FromArgb(10, 128, 0, 128),
            //Color.FromArgb(30, 128, 0, 255),
            //Color.FromArgb(70, 0, 0, 255),
            //Color.FromArgb(110, 0, 255, 0),
            //Color.FromArgb(130, 255, 255, 0),
            //Color.FromArgb(145, 255, 128, 0),
            //Color.FromArgb(155, 255, 0, 0)

             Color.FromArgb(0, 255, 255, 255),
             Color.FromArgb(100, 7, 4, 138),
            Color.FromArgb(100, 46, 248, 232),
            Color.FromArgb(100, 0, 255, 0),
            Color.FromArgb(100, 255, 255, 0),
            Color.FromArgb(100, 255, 0, 0)
        };
           // colorBlend.Positions = new float[8] { 0.0f, 0.1f, 0.25f, 0.4f, 0.65f, 0.75f, 0.9f, 1.0f };
            colorBlend.Positions = new float[6] { 0.0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
           
            Color startColor = colorBlend.Colors[0];
            Color endColor = colorBlend.Colors[colorBlend.Colors.Length - 1];

            using (Bitmap colorMapBitmap = new Bitmap(NUMCOLORS, 5, PixelFormat.Format32bppArgb))
            {
                Rectangle colorRect = new Rectangle(0, 0, colorMapBitmap.Width, colorMapBitmap.Height);
                using (Graphics bitmapGraphics = Graphics.FromImage(colorMapBitmap))
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(colorRect, startColor, endColor, LinearGradientMode.Horizontal))
                    {
                        brush.InterpolationColors = colorBlend;
                        bitmapGraphics.FillRectangle(brush, colorRect);
                    }
                }
                BitmapData colorMapData = colorMapBitmap.LockBits(colorRect, ImageLockMode.ReadOnly, colorMapBitmap.PixelFormat);
                IntPtr colorPtr = colorMapData.Scan0;
                int colorBytes = Math.Abs(colorMapData.Stride) * colorMapBitmap.Height;
                ColorArgbValues = new byte[colorBytes];
                System.Runtime.InteropServices.Marshal.Copy(colorPtr, ColorArgbValues, 0, colorBytes);
                colorMapBitmap.UnlockBits(colorMapData);
               // colorMapBitmap.Save("D://2.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);//保存图片到D盘
                
            }

        }

        /// <summary>
        /// 高斯核
        /// </summary>
        private void gaussiankernel()
        {
            for (int y = -r, i = 0; i < gSize; y++, i++)
            {
                for (int x = -r, j = 0; j < gSize; x++, j++)
                {
                    kernel[i, j] = Math.Exp(((x * x) + (y * y)) / (-2 * gSigma * gSigma)) / (2 * Math.PI * gSigma * gSigma);
                }
            }
        }

        /// <summary>
        /// 高斯核乘上权重
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        private double[,] MultiplyKernel(double weight)
        {
            double[,] wKernel = (double[,])kernel.Clone();
            for (int i = 0; i < gSize; i++)
            {
                for (int j = 0; j < gSize; j++)
                {
                    wKernel[i, j] *= weight;
                }
            }
            return wKernel;
        }

        /// <summary>
        /// 对所有权重进行归一化，压缩到颜色数量以内的值
        /// </summary>
        private void RescaleArray()
        {
            float max = 0;
            foreach (float value in heatVals)
            {
                if (value > max)
                {
                    max = value;
                }
            }

            for (int i = 0; i < heatVals.GetLength(0); i++)
            {
                for (int j = 0; j < heatVals.GetLength(1); j++)
                {
                    heatVals[i, j] *= (NUMCOLORS - 1) / max;
                    if (heatVals[i, j] > NUMCOLORS - 1)
                    {
                        heatVals[i, j] = NUMCOLORS - 1;
                    }
                }
            }
        }

        /// <summary>
        /// 一次性设置一组数据
        /// </summary>
        /// <param name="datas"></param>
        public void SetDatas(List<DataType> datas)
        {
            foreach (DataType data in datas)
            {
                int i, j, tx, ty, ir, jr;
                int radius = gSize >> 1;

                int x = data.X;
                int y = data.Y;
                double[,] kernelMultiplied = MultiplyKernel(data.Weight);

                for (i = 0; i < gSize; i++)
                {
                    ir = i - radius;
                    ty = y + ir;

                    if (ty < 0)
                    {
                        continue;
                    }

                    if (ty >= h)
                    {
                        break;
                    }

                    for (j = 0; j < gSize; j++)
                    {
                        jr = j - radius;
                        tx = x + jr;

                        // skip column
                        if (tx < 0)
                        {
                            continue;
                        }

                        if (tx < w)
                        {
                            heatVals[ty, tx] += kernelMultiplied[i, j];
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 逐个数据一个一个的存入
        /// </summary>
        /// <param name="data"></param>
        public void SetAData(DataType data)
        {
            int i, j, tx, ty, ir, jr;
            int radius = gSize >> 1;

            int x = data.X;
            int y = data.Y;
            double[,] kernelMultiplied = MultiplyKernel(data.Weight);

            for (i = 0; i < gSize; i++)
            {
                ir = i - radius;
                ty = y + ir;

                if (ty < 0)
                {
                    continue;
                }

                if (ty >= h)
                {
                    break;
                }

                for (j = 0; j < gSize; j++)
                {
                    jr = j - radius;
                    tx = x + jr;

                    if (tx < 0)
                    {
                        continue;
                    }

                    if (tx < w)
                    {
                        heatVals[ty, tx] += kernelMultiplied[i, j];
                    }
                }
            }
        }


        /// <summary>
        /// 基于存储的数据进行计算heatMap
        /// </summary>
        /// <returns></returns>
        public Bitmap GetHeatMap()
        {
            RescaleArray();
            Bitmap heatMap = new Bitmap(W, H, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, heatMap.Width, heatMap.Height);

            BitmapData heatMapData = heatMap.LockBits(rect, ImageLockMode.WriteOnly, heatMap.PixelFormat);
            IntPtr ptrw = heatMapData.Scan0;
            int wbytes = Math.Abs(heatMapData.Stride) * heatMap.Height;
            byte[] argbValuesW = new byte[wbytes];
            System.Runtime.InteropServices.Marshal.Copy(ptrw, argbValuesW, 0, wbytes);


            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    int colorIndex = double.IsNaN(heatVals[i, j]) ? 0 : (int)heatVals[i, j];
                    //if(colorIndex<10)
                    //Console.WriteLine("colorIndex:" + colorIndex.ToString());
                    int index = (i * heatMap.Width + j) * 4;
                    argbValuesW[index] = ColorArgbValues[4 * colorIndex];
                    argbValuesW[index + 1] = ColorArgbValues[4 * colorIndex + 1];
                    argbValuesW[index + 2] = ColorArgbValues[4 * colorIndex + 2];
                    argbValuesW[index + 3] = ColorArgbValues[4 * colorIndex + 3];
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(argbValuesW, 0, ptrw, wbytes);
            heatMap.UnlockBits(heatMapData);
            return heatMap;
        }

        /// <summary>
        /// 输出查看一下颜色图谱。
        /// </summary>
        /// <returns></returns>
        public Bitmap CheckColorMap()
        {
            Bitmap checkColor = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            int step = (NUMCOLORS - w) / h + 1;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if ((i * step) + j >= NUMCOLORS)
                    {
                        return checkColor;
                    }
                    Color color = Color.FromArgb(ColorArgbValues[i * step * 4],
                                                 ColorArgbValues[i * step * 4 + 1],
                                                 ColorArgbValues[i * step * 4 + 2],
                                                 ColorArgbValues[i * step * 4 + 3]);
                    checkColor.SetPixel(j, i, color);
                }
            }
            return checkColor;
        }

    }


}
