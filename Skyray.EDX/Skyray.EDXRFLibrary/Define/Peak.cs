using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    /// <summary>
    /// 峰
    /// </summary>
    public abstract class Peak : DbObjectModel<Peak>
    {
        [Length(20)]
        public abstract string Text { get; set; }    //名称+特征线        
        public abstract double Energy { get; set; } //能量
        public abstract int Channel { get; set; }    //通道       
        public abstract Peak Init(string text, double energy, int channel);
    }
}
