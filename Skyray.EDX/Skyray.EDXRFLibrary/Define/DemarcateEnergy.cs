using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lephone.Data.Common;
using Lephone.Data.Definition;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract partial class DemarcateEnergy : DbObjectModel<DemarcateEnergy>
    {
        [BelongsTo, DbColumn("Condition_Id")]
        public abstract Condition Condition { get; set; }

        [LengthAttribute(ColLength.ElementName)]
        public abstract string ElementName { get; set; } 

        public abstract XLine Line { get; set; }

        public abstract double Energy { get; set; }

        public abstract double Channel { get; set; }

        public abstract DemarcateEnergy Init(string ElementName, XLine Line, double Energy, double Channel);

        public DemarcateEnergyEntity ConvertToDemEntity()
        {
            DemarcateEnergyEntity entity = new DemarcateEnergyEntity(this.ElementName,this.Line,this.Energy,this.Channel);
            return entity;
        }
    }

    public static class DemarcateEnergyHelp
    {
        public static double k0;
        public static double k1;

       
        //public static void CalParam(List<DemarcateEnergy> demarcateEnergy)
        //{
        //    k0 = 0;
        //    k1 = 0;
        //    double x = 0;
        //    double y = 0;
        //    double xy = 0;
        //    double xx = 0;

        //    double x1 = 0;
        //    double y1 = 0;
        //    for (int i = 0; i < demarcateEnergy.Count; i++)
        //    {
        //        x1 += demarcateEnergy[i].Channel;
        //        y1 += demarcateEnergy[i].Energy;
        //    }
        //    x1 = x1 / demarcateEnergy.Count;
        //    y1 = y1 / demarcateEnergy.Count;

        //    for (int i = 0; i < demarcateEnergy.Count; i++)
        //    {
        //        x += demarcateEnergy[i].Channel;
        //        y += demarcateEnergy[i].Energy;
        //        xy += (demarcateEnergy[i].Channel - x1) * (demarcateEnergy[i].Energy - y1);
        //        xx += (demarcateEnergy[i].Channel - x1) * (demarcateEnergy[i].Channel - x1);
        //    }

        //    if ((demarcateEnergy.Count * xx - x * x) != 0)
        //    {
        //        k1 = xy / xx;
        //        k0 = y1-k1*x1;
        //    }

        //}

        public static void CalParam(DemarcateEnergyEntity[] DemarcateEnergys)
        {
            k0 = 0;
            k1 = 0;
            double x = 0;
            double y = 0;
            double xy = 0;
            double xx = 0;
            for (int i = 0; i < DemarcateEnergys.Length; i++)
            {
                x += DemarcateEnergys[i].Channel;
                y += DemarcateEnergys[i].Energy;
                xy += DemarcateEnergys[i].Channel * DemarcateEnergys[i].Energy;
                xx += DemarcateEnergys[i].Channel * DemarcateEnergys[i].Channel;
            }
            if ((DemarcateEnergys.Length * xx - x * x) != 0)
            {
                k1 = (DemarcateEnergys.Length * xy - x * y) / (DemarcateEnergys.Length * xx - x * x);
                k0 = (y - k1 * x) / DemarcateEnergys.Length;
            }
        }

        public static void CalParam(List<DemarcateEnergy> demarcateEnergy)
        {
            k0 = 0;
            k1 = 0;
            double x = 0;
            double y = 0;
            double xy = 0;
            double xx = 0;
            for (int i = 0; i < demarcateEnergy.Count; i++)
            {
                x += demarcateEnergy[i].Channel;
                y += demarcateEnergy[i].Energy;
                xy += demarcateEnergy[i].Channel * demarcateEnergy[i].Energy;
                xx += demarcateEnergy[i].Channel * demarcateEnergy[i].Channel;
            }
            if ((demarcateEnergy.Count * xx - x * x) != 0)
            {
                k1 = (demarcateEnergy.Count * xy - x * y) / (demarcateEnergy.Count * xx - x * x);
                k0 = (y - k1 * x) / demarcateEnergy.Count;
            }
        }

        public static void CalParam(DbObjectList<DemarcateEnergy> demarcateEnergy)
        {
            CalParam(demarcateEnergy.ToList());
        }
        public static void CalParam(HasMany<DemarcateEnergy> demarcateEnergy)
        {
            CalParam(demarcateEnergy.ToList());
        }

        /// <summary>
        ///获取能量相应的道址(插值)
        /// </summary>
        /// <param name="energy">能量</param>
        /// <returns>道址；如果没有做好能量刻度则返回0</returns>
        public static int GetChannel2(double energy, List<DemarcateEnergy> demarcateEnergy)
        {
            if (demarcateEnergy.Count <= 2)
            {
                double tempk0 = 0;
                double tempk1 = 0;
                double x = 0;
                double y = 0;
                double xy = 0;
                double xx = 0;
                for (int i = 0; i < demarcateEnergy.Count; i++)
                {
                    x += demarcateEnergy[i].Channel;
                    y += demarcateEnergy[i].Energy;
                    xy += demarcateEnergy[i].Channel * demarcateEnergy[i].Energy;
                    xx += demarcateEnergy[i].Channel * demarcateEnergy[i].Channel;
                }
                if ((demarcateEnergy.Count * xx - x * x) != 0)
                {
                    tempk1 = (demarcateEnergy.Count * xy - x * y) / (demarcateEnergy.Count * xx - x * x);
                    tempk0 = (y - tempk1 * x) / demarcateEnergy.Count;
                }
                if (tempk1 > 0)
                {
                    int channel = (int)Math.Round((energy - tempk0) / tempk1, MidpointRounding.AwayFromZero);
                    return channel > 0 ? channel : 0;
                    //return (int)Math.Round((energy - k0) / k1, MidpointRounding.AwayFromZero);
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                List<DemarcateEnergy> lstTemp = demarcateEnergy.OrderBy(wde => wde.Energy).ToList();
                int i = 0;
                for (i = 0; i < lstTemp.Count; i++)
                {
                    if (lstTemp[i].Energy > energy)
                    {
                        break;
                    }
                }
                double tempk1 = 0;
                double tempk0 = 0;
                if (i == 0)
                {
                    tempk1 = (lstTemp[i].Energy - lstTemp[i + 1].Energy) / (lstTemp[i].Channel - lstTemp[i + 1].Channel);
                    tempk0 = lstTemp[i].Energy - tempk1 * lstTemp[i].Channel;
                }
                else if (i < lstTemp.Count)
                {
                    tempk1 = (lstTemp[i].Energy - lstTemp[i - 1].Energy) / (lstTemp[i].Channel - lstTemp[i - 1].Channel);
                    tempk0 = lstTemp[i].Energy - tempk1 * lstTemp[i].Channel;
                }
                else
                {
                    tempk1 = (lstTemp[i - 1].Energy - lstTemp[i - 2].Energy) / (lstTemp[i - 1].Channel - lstTemp[i - 2].Channel);
                    tempk0 = lstTemp[i - 1].Energy - tempk1 * lstTemp[i - 1].Channel;
                }
                if (tempk1 <= 0)
                {
                    return 0;
                }
                return (int)Math.Round((energy - tempk0) / tempk1, MidpointRounding.AwayFromZero);
            }
        }
        /// <summary>
        ///获取能量相应的道址
        /// </summary>
        /// <param name="energy">能量</param>
        /// <returns>道址；如果没有做好能量刻度则返回0</returns>
        public static int GetChannel(double energy)
        {
            if (k1 > 0)
            {
                int channel=(int)Math.Round((energy - k0) / k1,MidpointRounding.AwayFromZero);
                return channel>0?channel:0;
                //return (int)Math.Round((energy - k0) / k1, MidpointRounding.AwayFromZero);
            }
            else
            {
                return 0;
            }
        }
        public static double GetDoubleChannel(double energy)
        {
            if (k1 > 0)
            {
                return (energy - k0) / k1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取道址相应的能量
        /// </summary>
        /// <param name="channel">道址</param>
        /// <returns>能量</returns>
        public static double GetEnergy(int channel)
        {
            return channel * k1 + k0;
        }

    }
}