using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;
using Lephone.Data.Common;

namespace Skyray.EDXRFLibrary
{
    /// <summary>
    /// 原子列表类
    /// </summary>
    public static class Atoms
    {
        public static double EnergyWindow = 0.100; ///<能量窗口 0.100;
        public static DbObjectList<Atom> AtomList = null;

        /// <summary>
        /// 获得特定线系能量附近的元素
        /// </summary>
        /// <param name="Energy">线系能量</param>
        /// <param name="Line">线系</param>
        /// <returns></returns>
        public static string GetAtoms(double energy, XLine line)
        {
            string names = string.Empty;
            if (energy < 0.02)
            {
                return names;
            }
            for (int i = 0; i < AtomList.Count; i++)
            {
                if (AtomList[i].AtomID < 11
                   || AtomList[i].AtomID == 18
                   || AtomList[i].AtomID == 36
                   || AtomList[i].AtomID == 54
                   || AtomList[i].AtomID == 86
                   || AtomList[i].AtomID > 88)//惰性气体的元素不分析
                {
                    continue;
                }
                if (AtomList[i].AtomID<39&&(line==XLine.La||line==XLine.Lb))//39号之前的元素不分析L系
                {
                    continue;
                }
                if (Math.Abs(GetEnergyByXLine(line, AtomList[i]) - energy) <= EnergyWindow)
                {
                    names = names + AtomList[i].AtomName + " ";
                }
            }
            return names;
        }

        public static string GetAtom(double energy, XLine line)
        {
            string names = string.Empty;
            if (energy < 0.02)
            {
                return names;
            }
            for (int i = 0; i < AtomList.Count; i++)
            {
                if (Math.Abs(GetEnergyByXLine(line, AtomList[i]) - energy) <= EnergyWindow)
                {
                    names = AtomList[i].AtomName;
                }
            }
            return names;
        }

        /// <summary>
        /// 射线与能量之间的
        /// </summary>
        /// <param name="line"></param>
        /// <param name="atom"></param>
        /// <returns></returns>
        public static double GetEnergyByXLine(XLine line, Atom atom)
        {
            double returnDouble = double.Epsilon;
            switch (line)
            {
                case XLine.Ka:
                    returnDouble = atom.Ka;
                    break;
                case XLine.Kb:
                    returnDouble = atom.Kb;
                    break;
                case XLine.La:
                    returnDouble = atom.La;
                    break;
                case XLine.Lb:
                    returnDouble = atom.Lb;
                    break;
                //case XLine.Le:
                //    returnDouble = atom.Le;
                //    break;
                //case XLine.Lr:
                //    returnDouble = atom.Lr;
                //    break;
                default:
                    break;
            }
            return returnDouble;
        }

        /// <summary>
        /// 获取元素的索引
        /// </summary>
        /// <param name="id">原子序号</param>
        /// <returns></returns>
        public static int GetIndex(int id)
        {
            int index = -1;

            for (int i = 0; i < AtomList.Count; ++i)
            {
                if (id == AtomList[i].AtomID)
                {
                    return i;
                }
            }
            return index;
        }

        /// <summary>
        /// 获取元素的索引
        /// </summary>
        /// <param name="name">元素名称</param>
        /// <returns></returns>
        public static int GetIndex(string name)
        {
            int index = -1;
            for (int i = 0; i < AtomList.Count; ++i)
            {
                if (name.Equals(AtomList[i].AtomName))
                {
                    return i;
                }
            }
            return index;
        }

        public static Atom GetAtom(string name)
        {
            int index = GetIndex(name);
            return index>-1? AtomList[index]:null;
        }
    }
}
