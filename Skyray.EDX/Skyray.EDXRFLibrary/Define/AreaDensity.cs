
using Lephone.Data.Definition;


namespace Skyray.EDXRFLibrary
{
    public abstract partial class AreaDensityUnit : DbObjectModel<AreaDensityUnit>
    {
        /// <summary>
        /// 原子序号
        /// </summary>
        public abstract string Name{ get; set; }

        //倍数系数
        public abstract string cofeK { get; set; }
        
        //预留
        public abstract string cofeB { get; set; }

        public abstract AreaDensityUnit Init(
            string Name,
            string cofeK,
            string cofeB
            );
    }
}
