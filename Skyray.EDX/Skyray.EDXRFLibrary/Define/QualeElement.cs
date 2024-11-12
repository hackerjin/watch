using Lephone.Data.Definition;

using Lephone.Util;

namespace Skyray.EDXRFLibrary
{
    /// <summary>
    /// 定性分析参数
    /// </summary>
    [Auto("分析参数")]
    public abstract partial class QualeElement : DbObjectModel<QualeElement>
    {
        [Auto("半高宽")]
        public abstract int ChannFWHM { get; set; }//峰的半高宽，用道宽表达
        [Auto("窗口宽度")]
        public abstract int WindowWidth { get; set; }  //窗口宽度 
        [Auto("灵敏度阈值")]
        public abstract double Trh1 { get; set; }     //灵敏度阈值 
        [Auto("峰谷距阈值")]
        public abstract double ValleyDistance { get; set; }  //峰谷距阈值 
        [Auto("面积比阈值")]
        public abstract double AreaLimt { get; set; }        //面积比阈值 

        [Length(ColLength.AvoidElem)]
        public abstract string AvoidElem { get; set; }       //无效元素

        public abstract QualeElement Init(int ChannFWHM,
            int WindowWidth,
            double Trh1,
            double ValleyDistance,
            double AreaLimt,
            string AvoidElem);
    }
}
