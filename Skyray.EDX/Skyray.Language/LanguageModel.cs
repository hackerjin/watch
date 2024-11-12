using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data;
using System.Windows.Forms;
using System.Reflection;
using Lephone.Data.Definition;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;
using System.Drawing;
using Skyray.Controls.Tree;

namespace Skyray.Language
{
    public class LangInfo
    {
        public bool IsForm { get; set; }
        public string[] ExcludeTypes { get; set; }
        public string[] PropertyNames { get; set; }
        public object ObjToChangeLang { get; set; }
    }

    public class LanguageModel
    {
        public List<Type> InfoTypes = new List<Type>();

        public List<LangInfo> LangObj = new List<LangInfo>();

        public List<DataGridView> DGVS = new List<DataGridView>();

        //public List<TreeViewAdv> TreeAdv = new List<TreeViewAdv>();

        public Languages CurrentLang { get; set; }

        public Languages OldLang { get; set; }

        public static Font newFont { get; set; }
        public static int imageSize { get; set; }

        public event EventHandler LanguageChanged;
        public event EventHandler preLanChange;

        public long _CurrentLangId;

        public long _CNLangId;

        public Dictionary<string, string> LangData { get; set; }

        public Dictionary<string, string> CNLangData { get; set; }

        public static bool ChangedFontByNewFont = true;


        //public long CurrentOldId
        //{
        //    get { return Languages.FindOne(w=>w.IsCurrentLang }
        //}

        public long CurrentLangId
        {
            get { return _CurrentLangId; }
            set
            {
                _CurrentLangId = value;
                CurrentLang = Languages.FindById(_CurrentLangId);

                CurrentLang.IsCurrentLang = true;
                CurrentLang.Save();
                if (OldLang != null)
                {
                    OldLang.IsCurrentLang = false;
                    OldLang.Save();
                }
                OldLang = CurrentLang;

                //LangData = CurrentLang.IsDefaultLang ? CNLangData : Func.GetLanguageData(_CurrentLangId);
                LangData = Func.GetLanguageData(_CurrentLangId);
            }
        }

        public LanguageModel()
        {
            CurrentLang = Languages.FindOne(l => l.IsCurrentLang == true);
            if (CurrentLang == null)
            {
                LanguageData.DeleteAll();//清除之前数据库
                Languages.DeleteAll();

                CurrentLang = Lang.GetDefaultLang();
                CurrentLang.Save();
                _CNLangId = CurrentLang.Id;
            }
            CurrentLangId = CurrentLang.Id;

            if (CurrentLang.IsDefaultLang)
            {
                CNLangData = Func.GetLanguageData(CurrentLang.Id);
            }
            else
            {
                var cnLang = Languages.FindOne(l => l.IsDefaultLang == true);
                if (cnLang != null) CNLangData = Func.GetLanguageData(cnLang.Id);
            }
        }

        #region 设置文本
        /// <summary>
        /// 设置对象文本
        /// </summary>
        /// <param name="IsForm">是否为窗体类型，如果是则反射私有且仅为定义的字段，一般位于Designer.cs的最后</param>
        /// <param name="excludeTypes"></param>
        /// <param name="PropertyNames"></param>
        /// <param name="objs"></param>
        public void SetText(bool IsForm, string[] excludeTypes, string[] PropertyNames, params object[] objs)
        {
            if (LangData == null) return;

            if (PropertyNames == null) PropertyNames = Param.PropertyNames;
            var textInfos = Func.GetLangTextInfos(IsForm, excludeTypes, PropertyNames, objs);
            string newText = string.Empty;
            string key = string.Empty;
            bool hasValue = false;
            foreach (var textInfo in textInfos)
            {
                key = Func.GetObjKey(textInfo);
                hasValue = LangData.ContainsKey(key);
                if (hasValue)
                {
                    newText = LangData[key];
                    if (!string.IsNullOrEmpty(newText))
                    {
                        try
                        {
                            // if (IsForm)
                            //{
                            //    textInfo.Type.InvokeMember(textInfo.PropertyName,
                            //        BindingFlags.SetProperty, null,
                            //        textInfo.Value,
                            //        new object[] { newText });
                            //}
                            //else
                            // {
                            //DataGridViewTextBoxColumn tt = new DataGridViewTextBoxColumn();
                            //tt.
                            textInfo.Value.GetType()
                                .GetProperty(textInfo.PropertyName)
                                .SetValue(textInfo.Value, newText, null);
                            // }
                            if (textInfo.Value.GetType().GetProperty("Font") != null&&ChangedFontByNewFont)
                                textInfo.Value.GetType().GetProperty("Font").SetValue(textInfo.Value, newFont, null);
                        }
                        catch
                        {
                            continue;
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 更改语言时，更改Info类中字段值
        /// </summary>
        public void SetInfoText()
        {
            SetInfoText(InfoTypes.ToArray());
        }

        /// <summary>
        /// 更改语言时，更改Info类中字段值
        /// </summary>
        public void SetInfoText(params Type[] typs)
        {
            string newText = string.Empty;
            string key = string.Empty;
            bool hasValue = false;
            foreach (Type typ in typs)
            {
                FieldInfo[] fieldInfos = typ.GetFields();
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    key = typ.Name + "." + fieldInfo.Name;
                    if (LangData == null)
                        return;
                    hasValue = LangData.ContainsKey(key);
                    if (hasValue)
                    {
                        newText = LangData[key];
                        if (!string.IsNullOrEmpty(newText))
                            fieldInfo.SetValue(null, newText); //设置字段对应的值
                    }
                }
            }
        }

        private void SetColHeaderText()
        {
            foreach (var dgv in DGVS)
            {
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    SetColHeaderText(col);
                }
                var method = dgv.GetType().GetMethod("HeaderTextRefresh");
                if (method != null)
                {
                    //method.Invoke(dgv, null);//慢
                    Delegate func = Delegate.CreateDelegate(typeof(Action), dgv, method);
                    func.DynamicInvoke();
                }
            }
        }

        public void SetColHeaderText(DataGridViewColumn col)
        {
            string key = col.Name + ".HeaderText";

            if (LangData != null && LangData.ContainsKey(key))
            {
                col.HeaderText = LangData[key];
            }
        }

        #endregion

        /// <summary>
        /// 注册列增加事件
        /// </summary>
        public void RegistDGVColAddedEvent()
        {
            foreach (var dgv in DGVS)
            {
                RegistDGVColAddedEvent(dgv);
            }
        }

        /// <summary>
        /// 注册列增加事件
        /// </summary>
        public void RegistDGVColAddedEvent(DataGridView dgv)
        {
            dgv.ColumnAdded += (sender, e) => SetColHeaderText(e.Column);
            if (CheckToSaveText())
            {
                dgv.ColumnAdded += (sender, e) =>
                    {
                        SaveData(GetColHeaderText(e.Column));
                    };
            }
        }

        #region 控件属性

        public void SaveHeaderTextProperty(params object[] objs)
        {
            SaveData(GetTextList(false, null, Param.HeaderTextProperty, objs));
        }

        public void SaveTextProperty(params object[] objs)
        {
            SaveData(GetTextList(false, null, Param.TextProperty, objs));
        }

        public void SaveProperty(string[] PropertyNames, params object[] objs)
        {
            SaveData(GetTextList(false, null, PropertyNames, objs));
        }

        public void SetTextProperty(params object[] objs)
        {
            SetText(false, null, Param.TextProperty, objs);
        }
        public void SetHeaderTextProperty(params object[] objs)
        {
            SetText(false, null, Param.HeaderTextProperty, objs);
        }
        public void SetProperty(string[] PropertyNames, params object[] objs)
        {
            SetText(false, null, PropertyNames, objs);
        }

        #endregion

        #region 窗体字段

        public void SetFormText(params object[] objs)
        {
            SetText(true, Param.ExcludeTypes, Param.PropertyNames, objs);
        }

        public void SaveFormText(params object[] objs)
        {
            var lstData = GetTextList(true, Param.ExcludeTypes, Param.PropertyNames, objs);

            if (lstData.Count > 0) SaveData(lstData);
        }

        #endregion

        /// <summary>
        /// 更改语言
        /// </summary>
        public void ChangeLanguage()
        {
            if (preLanChange != null) preLanChange(null, null);

            SetInfoText(InfoTypes.ToArray());

            SetColHeaderText();

            foreach (var langInfo in LangObj)
            {
                SetText(langInfo.IsForm, langInfo.ExcludeTypes, langInfo.PropertyNames, langInfo.ObjToChangeLang);
            }

            if (LanguageChanged != null) LanguageChanged(null, null);
        }

        /// <summary>
        /// 保存所有文本
        /// </summary>
        public void SaveAllText()
        {
            List<LanguageData> lstData = new List<LanguageData>();

            lstData.AddRange(GetColHeaderTextList());
            //lstData.AddRange(GetTreeViewAdvExTextList());
            lstData.AddRange(GetInfoTextList(InfoTypes.ToArray()));

            foreach (var langInfo in LangObj)
            {
                lstData.AddRange(GetTextList(langInfo.IsForm, langInfo.ExcludeTypes,
                    langInfo.PropertyNames, langInfo.ObjToChangeLang));
            }
            //var listCurrent = from temp in lstData group temp by temp.MultipleLanguage.Id into langGroup select langGroup;
            //foreach(var tt in listCurrent)
            //{
            //     var currentData = from temp in lstData where temp.MultipleLanguage.Id == tt.Key select temp;
                
            //}
            DbEntry.GetContext("Lang").FastSaveList(lstData, CurrentLangId);
            UpdateCNDB();
        }

        private void SaveData(LanguageData data)
        {
            if (data != null)
            {
                DbEntry.GetContext("Lang").FastSaveList(new LanguageData[] { data }, CurrentLangId);
                UpdateCNDB();
            }
        }

        private void SaveData(List<LanguageData> lstData)
        {
            if (lstData != null && lstData.Count > 0)
            {
                DbEntry.GetContext("Lang").FastSaveList(lstData, CurrentLangId);
                UpdateCNDB();
            }
        }

        private void UpdateCNDB()
        {
            CNLangData = Func.GetLanguageData(CurrentLangId);
        }

        private List<LanguageData> GetColHeaderTextList()
        {
            List<LanguageData> lst = new List<LanguageData>();
            foreach (var dgv in DGVS)
            {
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    var data = InsertData(col.Name + ".HeaderText", col.HeaderText);
                    if (data != null) lst.Add(data);
                }
            }
            lst.TrimExcess();
            return lst;
        }

        //public List<LanguageData> TreeData = new List<LanguageData>();


        //public void SaveTreeViewData()
        //{
        //    TreeData = TreeData.FindAll(w => w != null);
        //    if (TreeData != null && TreeData.Count > 0)
        //    {
        //        DbEntry.GetContext("Lang").FastSaveList(TreeData, CurrentLangId);
        //        UpdateCNDB();
        //    }
        //}

        private LanguageData GetColHeaderText(DataGridViewColumn col)
        {
            return InsertData(col.Name + ".HeaderText", col.HeaderText);
        }

        private List<LanguageData> GetInfoTextList(params Type[] typs)
        {
            List<LanguageData> lst = new List<LanguageData>();
            if (CheckToSaveText())
            {
                string value = string.Empty;
                string key = string.Empty;

                foreach (Type typ in typs)
                {
                    FieldInfo[] fieldInfos = typ.GetFields();
                    int intLength = fieldInfos.Length;
                    if (intLength > 0)
                    {
                        for (int i = 0; i < intLength; i++)
                        {
                            key = typ.Name + "." + fieldInfos[i].Name;
                            value = fieldInfos[i].GetValue(null).ToString();
                            var data = InsertData(key, value);
                             if (data != null) lst.Add(data);
                             //if (!StringContainCn(value))
                             //{
                             //    List<Languages> listLanguage = Languages.Find(w => w.IsDefaultLang != true);
                             //    listLanguage.ForEach(lang => {
                             //        LanguageData findData = LanguageData.FindOne(w => w.Key == key && w.MultipleLanguage.Id == lang.Id);
                             //        if (findData == null)
                             //        {
                             //            var dataTemp = LanguageData.New.Init(key, value);
                             //            dataTemp.MultipleLanguage = lang;
                             //            lst.Add(dataTemp);
                             //        }
                             //    });
                             //}
                           

                        }
                    }
                }
            }
            lst.TrimExcess();
            return lst;
        }

        public List<LanguageData> GetTextList(bool IsForm, string[] excludeTypes,
            string[] PropertyNames, params object[] objs)
        {
            List<LanguageData> lst = new List<LanguageData>();
            if (CheckToSaveText())
            {
                var res = Func.GetLangTextInfos(IsForm, excludeTypes, PropertyNames, objs);

                foreach (var textInfo in res)
                {
                    var data = InsertData(Func.GetObjKey(textInfo), textInfo.Text);
                    if (data != null) lst.Add(data);
                }
            }
            lst.TrimExcess();
            return lst;
        }

        public LanguageData InsertData(string key, string value)
        {
            if (!CheckData(key) && CheckString(ref value))
            {
                var data = LanguageData.New.Init(key, value);
                data.MultipleLanguage = CurrentLang;
                return data;
            }
            return null;
        }

        private bool CheckData(LanguageData data)
        {
            return CheckData(data.Key);
        }

        private bool CheckData(string key)
        {
            return CNLangData.ContainsKey(key);
        }

        private bool CheckString(ref string str)
        {
            str = str.TrimStart().TrimEnd();
            bool IsLegalString = !String.IsNullOrEmpty(str);

            //如果中文为默认语言，则判断字符中含有中文          
            IsLegalString = IsLegalString && StringContainCn(str);
            return IsLegalString;
        }

        /// <summary>
        /// 判断字符串包含中文
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool StringContainCn(string str)
        {
            bool b = false;
            Regex rx = new Regex("^[\u4e00-\u9fa5]$");
            for (int i = 0; i < str.Length; i++)
            {
                b = b || rx.IsMatch(str[i].ToString());
                if (b) break;
            }
            b = b || str.EndsWith(".xls");
            return b;
        }

        public bool CheckToSaveText()
        {
            return CurrentLang.IsDefaultLang && Param.SaveTextToDB;
        }

        ///// <summary>
        ///// 翻译方法
        ///// </summary>
        ///// <param name="textstr">需要翻译的内容</param>
        ///// <param name="language">被翻译的语言</param>
        ///// <param name="tolanguage">翻译成的语言</param>
        ///// <returns></returns>
        //public static string GetGoogtextStr(string textstr, string language, string tolanguage)
        //{

        //    WebClient web = new WebClient();
        //    WebHeaderCollection headers = new WebHeaderCollection();
        //    headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded; charset=utf-8";
        //    headers[HttpRequestHeader.Referer] = "http://translate.google.cn/";
        //    web.Headers = headers;
        //    string text = textstr;
        //    string url = string.Format("http://ajax.googleapis.com/ajax/services/language/translate?v=1.0&q={0}&langpair={1}%7C{2}", text, language, tolanguage);
        //    byte[] bystr = web.DownloadData(url);
        //    string urldata = GetText(System.Web.HttpUtility.UrlDecode(bystr, Encoding.UTF8));
        //    return urldata;
        //}

        public event EventHandler<TranslateEventArgs> OnTranslate;

        public void Translate(HasMany<LanguageData> lstData, long TargetLangId)
        {
            int[] ints = new int[lstData.Count];

            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = i;
            }
            Translate(lstData, TargetLangId, ints);
        }

        public void Translate(HasMany<LanguageData> lstData, long TargetLangId, params int[] rowIndexs)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                GoogleTranslate gt = new GoogleTranslate();

                foreach (var i in rowIndexs)
                {
                    var data = lstData[i];

                    string s = string.Empty;

                    string res = gt.TranslateChineseToEnglish(data.Value);

                    var targets = LanguageData.Find(x => x.Key == data.Key
                        && x.MultipleLanguage.Id == TargetLangId);

                    if (targets.Count > 0)
                    {
                        var target = targets[0];
                        target.Value = res;
                        target.Save();
                        s = "更新成功！";
                    }
                    else
                    {
                        s = "未找到数据！";
                    }

                    if (OnTranslate != null)
                    {
                        OnTranslate(null, new TranslateEventArgs(i, s, res));
                    }
                    Thread.Sleep(500);
                }
            });

        }

        public class TranslateEventArgs : EventArgs
        {
            public int Line { get; set; }
            public string Message { get; set; }
            public string Result { get; set; }
            public TranslateEventArgs(int i, string s, string res)
            {
                Line = i;
                Message = s;
                Result = res;
            }

        }
    }
}
