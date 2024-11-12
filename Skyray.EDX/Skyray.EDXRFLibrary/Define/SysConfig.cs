using System;
using Lephone.Data.Definition;
using System.Windows.Forms;

namespace Skyray.EDXRFLibrary
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [Serializable]
    [Auto("系统配置")]
    public abstract class SysConfig : DbObjectModel<SysConfig>
    {
        //所属设备
        [BelongsTo, DbColumn("Device_Id")]
        public abstract Device Device { get; set; }
        //热键
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<HotKeys> HotKeys { get; set; }
        //测试提示音
        public abstract bool IsTipSound { get; set; }

        //文件命名
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<FileNamed> FileNameds { get; set; }

        public abstract SysConfig Init(bool IsTipSound);
    }
    /// <summary>
    /// 快捷键
    /// </summary>
    [Serializable]
    [Auto("快捷键")]
    public abstract class HotKeys : DbObjectModel<HotKeys>
    {
        [BelongsTo, DbColumn("SysConfig_Id")]
        public abstract SysConfig SysConfig { get; set; }

        public abstract KeyModifiers KeyModifiers { get; set; }

        public abstract Keys Keys { get; set; }

        public abstract string Name { get; set; }

        public abstract SysFeatures SysFeatures { get; set; }

        public abstract string BackUp { get; set; }

        public abstract HotKeys Init(KeyModifiers KeyModifiers, Keys Keys, string Name, SysFeatures SysFeatures, string BackUp);
    }
    /// <summary>
    /// 文件命名
    /// </summary>
    [Serializable]
    [Auto("文件命名")]
    public abstract class FileNamed : DbObjectModel<FileNamed>
    {
        [BelongsTo, DbColumn("SysConfig_Id")]
        public abstract SysConfig SysConfig { get; set; }

        //样品类型
        public abstract SampleType SampleType { get; set; }

        public abstract bool DefaultNamed { get; set; }

        [HasMany(OrderBy = "Id")]
        public abstract HasMany<DisplayType> DisplayTypes { get; set; }
    }
    /// <summary>
    /// 选择方式
    /// </summary>
    [Serializable]
    [Auto("选择方式")]
    public abstract class DisplayType : DbObjectModel<DisplayType>
    {
        [BelongsTo, DbColumn("FileNamed_Id")]
        public abstract FileNamed FileNamed { get; set; }

        public abstract SelectType SelectType { get; set; }

        public abstract bool Display { get; set; }

        public abstract bool UnDisplay { get; set; }

        public abstract bool IsDefault { get; set; }

        public abstract bool Custom { get; set; }

        public abstract int Index { get; set; }
    }
}
