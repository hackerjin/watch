using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lephone.Data.Definition;
using Lephone.Util;
using Skyray.EDXRFLibrary;

namespace Skyray.MessageInfo
{
    [Auto("定性分析")]
    public class Qualitative : BaseMessage
    {
        [Auto("Count", "计数")]
        public int Count { set; get; }

        [Auto("Channel", "通道")]
        public int Channel { set; get; }

        [Auto("Energy", "能量")]
        public string Energy { set; get; }

        [Auto("Ka")]
        public string Ka { set; get; }

        [Auto("Kb")]
        public string Kb { set; get; }

        [Auto("La")]
        public string La { set; get; }

        [Auto("Lb")]
        public string Lb { set; get; }

        [Auto("Lr")]
        public string Lr { set; get; }

        [Auto("Le")]
        public string Le { set; get; }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public Qualitative()
        {
            this.Position = 2;
            this.IsFixed = true;
            this.type = DataGridViewType.Quality;
        }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="count"></param>
        /// <param name="chanel"></param>
        /// <param name="energy"></param>
        /// <param name="ka"></param>
        /// <param name="kb"></param>
        /// <param name="la"></param>
        /// <param name="lb"></param>
        /// <param name="lr"></param>
        /// <param name="le"></param>
        public Qualitative(int count, int chanel, string energy, string ka,
                           string kb, string la, string lb, string lr, string le)
            : this()
        {
            this.Channel = chanel;
            this.Energy = energy;
            this.Count = count;
            this.Ka = ka;
            this.Kb = kb;
            this.La = la;
            this.Lb = lb;
            this.Le = le;
            this.Lr = lr;
        }

    }
}
