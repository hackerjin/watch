﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    public abstract class MultipleLanguage : DbObjectModel<MultipleLanguage>
    {
        /// <summary>
        /// 包含语言信息
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<LanguageData> LanguageDatas { get; set; }

        [Length(ColLength.FullName)]
        /// <summary>
        /// 语言全称
        /// </summary>
        public abstract string FullName { get; set; }

        /// <summary>
        /// 语言简称
        /// </summary>
        [Length(ColLength.ShortName)]
        public abstract string ShortName { get; set; }

        /// <summary>
        /// 是否当前语言
        /// </summary>
        public abstract bool IsCurrentLang { get; set; }

        public abstract MultipleLanguage Init(string FullName, string ShortName, bool IsCurrentLang);
    }

    public abstract class LanguageData : DbObjectModel<LanguageData>
    {
        /// <summary>
        /// 所属语言
        /// </summary>
        [BelongsTo, DbColumn("MultipleLanguage_Id")]
        public abstract MultipleLanguage MultipleLanguage { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        [Length(ColLength.LanguageDataKey)]
        public abstract string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Length(ColLength.LanguageDataValue)]
        public abstract string Value { get; set; }

        public abstract LanguageData Init(string Key, string Value);
    }
}