using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    
    public abstract partial class Atom : DbObjectModel<Atom>
    {
        /// <summary>
        /// 原子序号
        /// </summary>
        public abstract int AtomID { get; set; }

        /// <summary>
        /// 元素名称
        /// </summary>
        [Length(ColLength.AtomName)]
        public abstract string AtomName { get; set; }

        /// <summary>
        /// 元素英文名称
        /// </summary>
        [Length(ColLength.AtomNameEN)]
        public abstract string AtomNameEN { get; set; }

        /// <summary>
        /// 元素名称
        /// </summary>
        [Length(ColLength.AtomNameCN)]
        public abstract string AtomNameCN { get; set; }
        ///
        public abstract double AtomDensity { get; set; }///<纯物质密度
        public abstract double AtomWeight { get; set; }///<原子量
        public abstract XLine DefaultLine { get; set; } ///<默认的检测线
        public abstract string AtomColor { get; set; }     ///<显示的颜色  
        public abstract double Ka { get; set; }
        public abstract double Kb { get; set; }
        public abstract double La { get; set; }
        public abstract double Lb { get; set; }
        public abstract double Lr { get; set; }
        public abstract double Le { get; set; }
        public abstract Atom Init(int AtomID,
           string AtomName,
            string AtomNameCN,
            string AtomNameEN,
            double AtomDensity,
            double AtomWeight,
            XLine DefaultLine,
            string AtomColor,
               double Ka,
              double Kb,
                  double La,
                      double Lb,
                           double Lr,
                             double Le
            );
    }
}