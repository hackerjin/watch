using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Drawing;

namespace Skyray.EDX.Common
{
    public class MathFunc
    {
        public static double[] Smooth(double[] dblData)
        {
            int length = dblData.Length;
            double[] numArray = new double[length];
            numArray[0] = dblData[0];
            numArray[1] = dblData[1];
            for (int i = 2; i < (length - 2); i++)
            {
                numArray[i] = ((((dblData[i - 2] + (4.0 * dblData[i - 1])) + (6.0 * dblData[i])) + (4.0 * dblData[i + 1])) + dblData[i + 2]) / 16.0;
            }
            numArray[length - 2] = dblData[length - 2];
            numArray[length - 1] = dblData[length - 1];
            return numArray;
        }

        // Methods
        public static double Avg(double[] doubles)
        {
            return (Sum(doubles) / ((double)doubles.Length));
        }

        public static double Avg(List<double> doubles)
        {
            return (Sum(doubles) / ((double)doubles.Count));
        }

        public static string ConvertBytesToHex(byte[] arrByte, bool reverse)
        {
            StringBuilder builder = new StringBuilder();
            if (reverse)
            {
                Array.Reverse(arrByte);
            }
            foreach (byte num in arrByte)
            {
                builder.AppendFormat("{0:x2}", num);
            }
            return builder.ToString();
        }

        public static int ConvertBytesToInt(byte[] arrByte, int offset)
        {
            return BitConverter.ToInt32(arrByte, offset);
        }

        public static byte[] ConvertHexToBytes(string value)
        {
            int num = value.Length / 2;
            byte[] buffer = new byte[num];
            for (int i = 0; i < num; i++)
            {
                buffer[i] = (byte)Convert.ToInt32(value.Substring(i * 2, 2), 0x10);
            }
            return buffer;
        }

        public static byte[] ConvertIntToBytes(int value, bool reverse)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (reverse)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        /// <summary>
        /// 求决定系数
        /// </summary>
        /// <param name="lstPoints"></param>
        /// <returns></returns>
        public static double CalcCoefficientR(PointF[] lstPoints)
        {
            double r = 0;
            double XAvg = 0;
            double YAvg = 0;
            int num = lstPoints.Length;
            if (num <= 0)
            {
                return r;
            }

            for (int i = 0; i < num; i++)
            {
                XAvg += lstPoints[i].X;
                YAvg += lstPoints[i].Y;
            }
            XAvg = XAvg / num;
            YAvg = YAvg / num;

            double sumNumerator = 0;
            double SumAvgX = 0;
            double SumAvgY = 0;
            for (int i = 0; i < num; i++)
            {
                sumNumerator += (lstPoints[i].X - XAvg) * (lstPoints[i].Y - YAvg);
                SumAvgX += Math.Pow((lstPoints[i].X - XAvg), 2);
                SumAvgY += Math.Pow((lstPoints[i].Y - YAvg), 2);
            }
            r = sumNumerator / Math.Sqrt(SumAvgX * SumAvgY);
            return r;
        }


        /// <summary>
        /// 求决定系数
        /// </summary>
        /// <param name="lstPoints"></param>
        /// <returns></returns>
        public static double CalcCoefficientR(double[,] lstPoints)
        {
            double r = 0;
            double XAvg = 0;
            double YAvg = 0;
            int num = lstPoints.GetLength(0);
            if (num <= 0)
            {
                return r;
            }

            for (int i = 0; i < num; i++)
            {
                XAvg += lstPoints[i, 0];
                YAvg += lstPoints[i, 1];
            }
            XAvg = XAvg / num;
            YAvg = YAvg / num;

            double sumNumerator = 0;
            double SumAvgX = 0;
            double SumAvgY = 0;
            for (int i = 0; i < num; i++)
            {
                sumNumerator += (lstPoints[i, 0] - XAvg) * (lstPoints[i, 1] - YAvg);
                SumAvgX += Math.Pow((lstPoints[i, 0] - XAvg), 2);
                SumAvgY += Math.Pow((lstPoints[i, 1] - YAvg), 2);
            }

            r = sumNumerator / Math.Sqrt(SumAvgX * SumAvgY);


            return r;
        }


        /// <summary>
        /// 一次曲线相关系数
        /// </summary>
        /// <param name="pnts"></param>
        /// <returns></returns>
        public static double GetRCoefficient(PointF[] pnts)
        {
            int length = pnts.Length;
            float num2 = 0f;
            float num3 = 0f;
            float num4 = 0f;
            float num5 = 0f;
            float num6 = 0f;
            for (int i = 0; i < length; i++)
            {
                num2 += pnts[i].X * pnts[i].Y;
                num3 += pnts[i].X;
                num4 += pnts[i].Y;
                num5 += pnts[i].X * pnts[i].X;
                num6 += pnts[i].Y * pnts[i].Y;
            }
            double num8 = ((double)(num2 - ((num3 * num4) / ((float)length)))) / Math.Sqrt((double)((num5 - ((num3 * num3) / ((float)length))) * (num6 - ((num4 * num4) / ((float)length)))));
            return num8;
        }

        public static double GetRSD(double[] doubles)
        {
            if (Avg(doubles) == 0.0)
            {
                return 0.0;
            }
            return (GetSD(doubles) / Avg(doubles));
        }

        public static double GetRSD(List<double> doubles)
        {
            if (Avg(doubles) == 0.0)
            {
                return 0.0;
            }
            return (GetSD(doubles) / Avg(doubles));
        }

        public static double GetSD(double[] doubles)
        {
            double num = Avg(doubles);
            double num2 = 0.0;
            foreach (double num3 in doubles)
            {
                num2 += Math.Pow(num3 - num, 2.0);
            }
            return Math.Sqrt(num2 / ((double)(doubles.Length - 1)));
        }

        public static double GetSD(List<double> doubles)
        {
            double num = Avg(doubles);
            double num2 = 0.0;
            foreach (double num3 in doubles)
            {
                num2 += Math.Pow(num3 - num, 2.0);
            }
            return Math.Sqrt(num2 / ((double)(doubles.Count - 1)));
        }

        public static double Sum(double[] doubles)
        {
            double num = 0.0;
            foreach (double num2 in doubles)
            {
                num += num2;
            }
            return num;
        }

        public static double Sum(List<double> doubles)
        {
            double num = 0.0;
            foreach (double num2 in doubles)
            {
                num += num2;
            }
            return num;
        }

        public static byte ToHex(int i)
        {
            return byte.Parse(Convert.ToString(i, 0x10), NumberStyles.HexNumber);
        }

        public static byte ToHex(string s)
        {
            return byte.Parse(s);
        }
    }
}
