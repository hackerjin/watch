using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.Controls.Extension
{
    public class Guass
    {

        /// <summary>
        /// 计算吸光度，浓度曲线，计算结果是一个形如Y=x[0]+x[1]*X+...形式的方程
        /// </summary>
        /// <param name="abs">(X)</param>
        /// <param name="con">(Y)</param>
        /// <param name="dim">曲线阶次</param>
        /// <param name="izero">true过零点，false不过零点</param>
        public static void CalculateCurve(double[] abs, double[] con, int dim, bool izero, double[] x)
        {
            if (izero)
            {
                double[,] a = new double[dim + 1, dim];
                double[] b = new double[dim + 1];
                double[] solu = new double[dim];
                for (int i = 0; i < dim + 1; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        a[i, j] = 0;
                        for (int k = 0; k < abs.Length; k++)
                        {
                            a[i, j] += Math.Pow(abs[k], i + j + 1);
                        }
                    }
                }
                for (int i = 0; i < dim + 1; i++)
                {
                    b[i] = 0;
                    for (int k = 0; k < con.Length; k++)
                    {
                        b[i] += Math.Pow(abs[k], i) * con[k];
                    }
                }
                MatrixEquation(dim + 1, dim, a, b, solu);
                x[0] = 0;
                for (int i = 1; i < x.Length; i++)
                {
                    x[i] = solu[i - 1];
                }
            }
            else
            {
                double[,] a = new double[dim + 1, dim + 1];
                double[] b = new double[dim + 1];
                for (int i = 0; i < dim + 1; i++)
                {
                    for (int j = 0; j < dim + 1; j++)
                    {
                        a[i, j] = 0;
                        for (int k = 0; k < abs.Length; k++)
                        {
                            a[i, j] += Math.Pow(abs[k], i + j);
                        }
                    }
                }
                for (int i = 0; i < dim + 1; i++)
                {
                    b[i] = 0;
                    for (int k = 0; k < con.Length; k++)
                    {
                        b[i] += Math.Pow(abs[k], i) * con[k];
                    }
                }
                MatrixEquation(dim + 1, dim + 1, a, b, x);
            }
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = Convert.ToDouble(string.Format("{0:N6}", x[i]));
            }
        }

        public static void Trmul(int m, int n, int k, double[,] a, double[,] b, double[,] c) //矩阵相乘
        {
            // a[m,n] b[n,k] c[m,k]
            int i, j, L;
            for (i = 0; i <= m - 1; i++)
                for (j = 0; j <= k - 1; j++)
                {
                    c[i, j] = 0.0;
                    for (L = 0; L <= n - 1; L++)
                        c[i, j] = c[i, j] + a[i, L] * b[L, j];
                }
        }

        public static void Rinv(int n, double[,] a)//矩阵的逆
        {
            int[] si = new int[n];
            int[] sj = new int[n];
            int i, j, k;
            double d, p;

            for (k = 0; k < n; k++)
            {
                d = 0.0;
                for (i = k; i < n; i++)
                    for (j = k; j < n; j++)
                    {
                        p = System.Math.Abs(a[i, j]);
                        if (p > d)
                        {
                            d = p; si[k] = i; sj[k] = j;
                        }
                    }
                if ((d + 1.0) == 1.0)
                {
                    throw new Exception("");
                }
                if (si[k] != k)
                    for (j = 0; j < n; j++)
                    {
                        p = a[k, j];
                        a[k, j] = a[si[k], j];
                        a[si[k], j] = p;
                    }
                if (sj[k] != k)
                    for (i = 0; i < n; i++)
                    {
                        p = a[i, k];
                        a[i, k] = a[i, sj[k]];
                        a[i, sj[k]] = p;
                    }

                a[k, k] = 1.0 / a[k, k];
                for (j = 0; j < n; j++)
                    if (j != k)
                        a[k, j] = a[k, j] * a[k, k];
                for (i = 0; i < n; i++)
                    if (i != k)
                        for (j = 0; j < n; j++)
                            if (j != k)
                                a[i, j] = a[i, j] - a[i, k] * a[k, j];
                for (i = 0; i < n; i++)
                    if (i != k)
                        a[i, k] = -a[i, k] * a[k, k];
            }

            for (k = n - 1; k >= 0; k--)
            {
                if (sj[k] != k)
                    for (j = 0; j <= n - 1; j++)
                    {
                        p = a[k, j]; a[k, j] = a[sj[k], j]; a[sj[k], j] = p;
                    }
                if (si[k] != k)
                    for (i = 0; i <= n - 1; i++)
                    {
                        p = a[i, k]; a[i, k] = a[i, si[k]]; a[i, si[k]] = p;
                    }
            }
        }

        /// <summary>
        /// 获取半高宽
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static double GetHalfHeight(double[] datas)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = Math.Log(datas[i]);
            }
            double[,] a = new double[datas.Length, 3];
            double[] x = new double[3];
            for (int i = 0; i < datas.Length; i++)
            {
                a[i, 0] = 1;
                a[i, 1] = i;
                a[i, 2] = a[i, 1] * a[i, 1];
            }
            MatrixEquation(datas.Length, 3, a, datas, x);

            double C, A, B;
            C = -1.0 / x[2];

            A = Math.Log(2) * C;

            B = Math.Sqrt(A) * 2;

            return B;
        }
        [DllImport("EDXRF.dll", EntryPoint = "MaxtrixEquation", CallingConvention = CallingConvention.StdCall)]
        internal static extern void MaxtrixEquation(int m, int n, double[,] a, double[] b, double[] x);
        /// <summary>
        /// 高斯拟合
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static double[] GuassCoee(double[] datas)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = Math.Log(datas[i]);
            }
            double[,] a = new double[datas.Length, 3];
            double[] x = new double[3];
            for (int i = 0; i < datas.Length; i++)
            {
                a[i, 0] = 1;
                a[i, 1] = i;
                a[i, 2] = a[i, 1] * a[i, 1];
            }
            //Guass.MatrixEquation(datas.Length, 3, a, datas, x);
            MaxtrixEquation(datas.Length, 3, a, datas,x);
            double A, B, C;
            C = -1.0 / x[2];
            B = (C * x[1]) / 2.0;
            A = Math.Exp(B * B / C + x[0]);

            for (int i = 0; i < datas.Length; i++)
            {
                double coe = -Math.Pow((i - B), 2) / C;
                datas[i] = A * Math.Exp(coe);
            }

            return datas;
        }

        //解矩阵方程(Ax=B)
        public static void MatrixEquation(int m, int n, double[,] a, double[] b, double[] x)
        {
            double[,] at = new double[n, m]; //A转置矩阵
            double[,] a1 = new double[n, n]; //B逆矩阵
            double[,] c = new double[m, 2];
            double[,] b2 = new double[m, 2];
            for (int i = 0; i <= n - 1; i++)
                for (int j = 0; j <= m - 1; j++)
                {
                    at[i, j] = a[j, i];
                }
            Trmul(n, m, n, at, a, a1);
            Rinv(n, a1);
            for (int i = 0; i <= m - 1; i++)
                b2[i, 0] = b[i];
            Trmul(n, m, 1, at, b2, c);
            Trmul(n, n, 1, a1, c, at);
            for (int i = 0; i <= n - 1; i++)
                x[i] = at[i, 0];
        }
    }
}
