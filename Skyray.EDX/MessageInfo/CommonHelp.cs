using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;
using Skyray.EDXRFLibrary;

namespace Skyray.MessageInfo
{
    public static class CommonHelp
    {   
        public static List<Atom> listAtom = Atoms.AtomList;
        /// <summary>
        /// 刷新定性分析DataGridViewW 
        /// </summary>
        /// <param name="cout">测试次数</param>
        /// <param name="dgvQualitative">定性分析对象</param>
        /// <param name="icurrentChanle">信道的当前值</param>
        /// <param name="listAtom">所有的原子</param>
        public static void RefreshDataQualitative(int cout, Qualitative dgvQualitative, int icurrentChanle, double energy)
        {
            dgvQualitative.Count = cout;//当前值
            dgvQualitative.Channel = icurrentChanle;//当前道
            dgvQualitative.Energy = energy.ToString("0.000");
            //dgvQualitative.Ka = GetString(listAtom.FindAll(delegate(Atom s) { return Math.Abs(s.Ka - energya) <= 0.165; }));//得到特定能量特定谱线的元素
            //dgvQualitative.Kb = GetString(listAtom.FindAll(delegate(Atom s) { return Math.Abs(s.Kb - energya) <= 0.165; }));
            //dgvQualitative.La = GetString(listAtom.FindAll(delegate(Atom s) { return Math.Abs(s.La - energya) <= 0.165; }));
            //dgvQualitative.Lb = GetString(listAtom.FindAll(delegate(Atom s) { return Math.Abs(s.Lb - energya) <= 0.165; }));
            //dgvQualitative.Lr = GetString(listAtom.FindAll(delegate(Atom s) { return Math.Abs(s.Lr - energya) <= 0.165; }));
            //dgvQualitative.Le = GetString(listAtom.FindAll(delegate(Atom s) { return Math.Abs(s.Le - energya) <= 0.165; }));
            //energya.ToString("f4");
            if (icurrentChanle != 0)
                FillQualitative(dgvQualitative, energy);
        }

        /// <summary>
        /// 填充定性分析对象
        /// </summary>
        /// <param name="dgvQualitative"></param>
        /// <param name="energya"></param>
        private static void FillQualitative(Qualitative dgvQualitative, double energya)
        {
            List<Atom> findAtomKa = new List<Atom>();
            List<Atom> findAtomKb = new List<Atom>();
            List<Atom> findAtomLa = new List<Atom>();
            List<Atom> findAtomLb = new List<Atom>();
            List<Atom> findAtomLr = new List<Atom>();
            List<Atom> findAtomLe = new List<Atom>();
            foreach (Atom atom in listAtom)
            {
                if (Math.Abs(atom.Ka - energya) <= EnergyWindow)
                    findAtomKa.Add(atom);
                if (Math.Abs(atom.Kb - energya) <= EnergyWindow)
                    findAtomKb.Add(atom);
                if (Math.Abs(atom.La - energya) <= EnergyWindow)
                    findAtomLa.Add(atom);
                if (Math.Abs(atom.Lb - energya) <= EnergyWindow)
                    findAtomLb.Add(atom);
                if (Math.Abs(atom.Lr - energya) <= EnergyWindow)
                    findAtomLr.Add(atom);
                if (Math.Abs(atom.Le - energya) <= EnergyWindow)
                    findAtomLe.Add(atom);
            }
            dgvQualitative.Ka = GetString(findAtomKa);
            dgvQualitative.Kb = GetString(findAtomKb);
            dgvQualitative.La = GetString(findAtomLa);
            dgvQualitative.Lb = GetString(findAtomLb);
            dgvQualitative.Lr = GetString(findAtomLr);
            dgvQualitative.Le = GetString(findAtomLe);
        }


        /// <summary>
        /// 查找所有的原子信息
        /// </summary>
        /// <param name="listAtom"></param>
        /// <returns></returns>
        public static string GetString(List<Atom> listAtom)
        {
            if (listAtom == null || listAtom.Count == 0)
                return null;
            string stringList = string.Empty;
            foreach (Atom atom in listAtom)
                stringList += atom.AtomName + " ";
            return stringList;
        }


        private static double EnergyWindow = 0.3;
    }
}
