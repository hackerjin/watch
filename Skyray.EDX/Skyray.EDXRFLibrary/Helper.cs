using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Skyray.EDXRFLibrary
{
    public class Helper
    {
        public static readonly char[] splitChar = new char[] { ',',';' };

        public static bool IGetGradeContent = false;

        public static ExperienceCoeffs ExperienceCoeffs= new ExperienceCoeffs();

        public static string ToString(byte[] byts)
        {
            return Encoding.ASCII.GetString(byts);
        }

        public static byte[] ToBytes(string strs)
        {
            return Encoding.ASCII.GetBytes(strs);
        }

        public static void LoadExperienceCoeffs(string filename)
        {
            ExperienceCoeffs.LoadExperienceCoeffs(filename);
        }

        //public static void LoadGradeCoeffs(string filename)
        //{
        //    ExperienceCoeffs.LoadGradeCoeffs(filename);
        //}
        public static int[] ToInts(string str)
        {
            string[] strs = str.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            int[] ints = new int[strs.Length];
            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = Convert.ToInt32(strs[i]);
            }
            //GC.Collect();
            return ints;
        }

        public static int[] ToInts(string[] strs)
        {
            int[] ints = new int[strs.Length];
            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = Convert.ToInt32(strs[i]);
            }
            return ints;
        }
        public static int[] ToInts(float[] flo)
        {
            int[] ints = new int[flo.Length];
            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = (int)Math.Round(double.Parse(flo[i].ToString()),MidpointRounding.AwayFromZero);
            }
            return ints;
        }
        public static int[] ToInts(double[] dou)
        {
            int[] ints = new int[dou.Length];
            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = (int)Math.Round(dou[i], MidpointRounding.AwayFromZero);
            }
            return ints;
        }

        public static double[] ToDoubles(string str)
        {
            string[] strs = str.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            double[] dbls = new double[strs.Length];
            for (int i = 0; i < dbls.Length; i++)
            {
                dbls[i] = Convert.ToDouble(strs[i]);
            }
            //GC.Collect();
            return dbls;
        }
        public static double[] ToDoubles(string[] strs)
        {
            double[] dbls = new double[strs.Length];
            for (int i = 0; i < dbls.Length; i++)
            {
                dbls[i] = Convert.ToDouble(strs[i]);
            }
            return dbls;
        }
        public static double[] ToDoubles(int[] ints)
        {
            double[] dbls = new double[ints.Length];
            for (int i = 0; i < dbls.Length; i++)
            {
                dbls[i] = Convert.ToDouble(ints[i]);
            }
            return dbls;
        }

        public static string TransforToDivTime(string datas, double times)
        {
            StringBuilder sb = new StringBuilder();
            double[] dbl = ToDoubles(datas);
            foreach (var i in dbl)
            {
                sb.Append((i / times).ToString() + ",");
            }
            return sb.ToString();
        }

        public static string TransforToDivTimeAndCurrent(string datas,double times, double Current)
        {
            StringBuilder sb = new StringBuilder();
            double[] dbl = ToDoubles(datas);
            foreach (var i in dbl)
            {
                sb.Append((i / times / Current).ToString() + ",");
              //sb.Append((i / times).ToString() + ",");
            }
            return sb.ToString();
        }


        public static string TransforDivTimeSmooth(string datas, double times,int smoothTimes)
        {
            StringBuilder sb = new StringBuilder();
            int[] dbl = ToInts(datas);
            dbl = Smooth(dbl, smoothTimes);
            foreach (var i in dbl)
            {
                sb.Append(((double)i / times).ToString() + ",");
            }
            return sb.ToString();
        }

        public static string ToStrs<T>(T[] data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var i in data)
            {
                sb.Append(i.ToString() + ",");
            }
            return sb.ToString();
        }

        public static string ToStrsRemoveLast<T>(T[] data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var i in data)
            {
                sb.Append(i.ToString() + ",");
            }
            string str = sb.ToString();
            return str.Remove(str.Length - 1); ;
        }

        public static string[] ToStrs(string str)
        {
            return str.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 平滑譜
        /// </summary>        
        public static int[] Smooth(int[] specData, int smoothTimes)
        {
            int iCount = 0;
            int specLength = specData.Length;
            int[] Data = (int[])specData.Clone();
            int[] DataCopy = (int[])specData.Clone();
            while (smoothTimes-- > 0)
            {
                for (iCount = 3; iCount < specLength - 3; iCount++)
                {
                    DataCopy[iCount] = (int)Math.Round((Data[iCount - 2]
                        + 4 * Data[iCount - 1]
                        + 6 * Data[iCount]
                        + 4 * Data[iCount + 1]
                        + Data[iCount + 2]) / 16.0f);
                }
                Array.Copy(DataCopy, Data, Data.Length);
            }
            return Data;
        }

        //public string GetConfigValue(string key)
        //{
        //    return ConfigurationSettings.AppSettings[key];
        //}
        ///// <summary>
        ///// 平滑譜
        ///// </summary>        
        //public static int[] Smooth(int[] specData, int specLength)
        //{
        //    int iCount = 0;
        //    //int[] Data = new int[specData.Length];
        //    int[] Data = (int[])specData.Clone();
        //    //添加平滑次数
        //    int smoothTimes = 1;
        //    if (Data.Length < specLength)
        //        return Data;
        //    while (smoothTimes-- > 0)
        //    {

        //        for (iCount = 3; iCount < specLength - 3; iCount++)
        //        {
        //            Data[iCount] = (Data[iCount - 2] + 4 * Data[iCount - 1] + 6 * Data[iCount] + 4 * Data[iCount + 1] + Data[iCount + 2]) / 16;
        //        }
        //    }
        //    return Data;
        //}

    }
}
