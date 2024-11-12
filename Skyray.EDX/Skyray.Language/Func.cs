using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Lephone.Data;
using System.Data;
using System.Data.SQLite;
using Lephone.Data.Definition;
using System.Collections;
using System.Text.RegularExpressions;
namespace Skyray.Language
{
    public class Func
    {
        public static Dictionary<string, string> GetLanguageData(long LangId)
        {
            Dictionary<string, string> dt = new Dictionary<string, string>();
            
            var connStr = DbEntry.GetContext("Lang").Driver.ConnectionString;
            var cmdText = @"select Key,Value from LanguageData where Language_Id=" + LangId;
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(cmdText, conn);
                SQLiteDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    string key = sdr[0].ToString();
                    if (!dt.ContainsKey(key)) dt.Add(key, sdr[1].ToString());
                }
            }
            return dt;
        }

        #region 私有变量
        //private static Languages CurrentLang;
        //private static Dictionary<string, string> LangData;
        //private static Languages DefaultLang;

        #endregion

        #region 数据校验

        //public static bool CheckString(ref string str)
        //{
        //    str = str.TrimStart().TrimEnd();
        //    bool IsLegalString = !String.IsNullOrEmpty(str);

        //    //如果中文为默认语言，则判断字符中含有中文
        //    if (Param.NONeedChangeToCN)
        //        IsLegalString = IsLegalString && StringContainCn(str);
        //    return IsLegalString;
        //}

        ///// <summary>
        ///// 判断字符串包含中文
        ///// </summary>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //public static bool StringContainCn(string str)
        //{
        //    bool b = false;
        //    Regex rx = new Regex("^[\u4e00-\u9fa5]$");
        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        b = b || rx.IsMatch(str[i].ToString());
        //        if (b) break;
        //    }
        //    return b;
        //}

        #endregion

        #region 获取文本信息

        public static List<LangTextInfo> GetLangTextInfos(bool IsForm,
            string[] excludeTypes,
            string[] PropertyNames,
            object[] objs)
        {
            var res = new List<LangTextInfo>();
            foreach (var obj in objs)
            {
                res.AddRange(LangTextInfos(IsForm, obj, excludeTypes, PropertyNames));
            }
            return res;
        }
        #endregion

        #region 获取字段信息在数据库中对应的关键字
        public static string GetObjKey(LangTextInfo textInfo)
        {
            return textInfo.Name + "." + textInfo.PropertyName;
        }

        #endregion

        #region 获取对象私有字段集合
        private static FieldInfo[] GetFieldInfo(object obj)
        {
            return obj.GetType().GetFields(Param.RefFlag);
        }
        #endregion

        #region 获取对象字段文本信息

        private static List<LangTextInfo> LangTextInfos(bool IsForm, object obj, string[] excludeTypes, string[] PropertyNames)
        {
            var lst = new List<LangTextInfo>();

            if (obj is IEnumerable)
            {
                foreach (var o in (IEnumerable)obj)
                {
                    lst.AddRange(LangTextInfos(IsForm, o, excludeTypes, PropertyNames));
                }
            }
            else
            {
                lst.AddRange(SingleObjTextInfo(IsForm, obj, excludeTypes, PropertyNames));
            }
            return lst;
        }

        private static List<LangTextInfo> GetPropertyTextInfo(object obj, string[] PropertyNames)
        {
            List<LangTextInfo> lstInfos = new List<LangTextInfo>(); //定义集合
            if (obj != null)
            {
                Type typ = obj.GetType();
                var nameInfo = typ.GetProperty("Name");

                foreach (string s in PropertyNames)
                {
                    var propertyInfo = typ.GetProperty(s);
                    if (propertyInfo != null)
                    {
                        string value = Convert.ToString(propertyInfo.GetValue(obj, null));

                        lstInfos.Add(new LangTextInfo(
                         nameInfo == null ? obj.ToString() : nameInfo.GetValue(obj, null).ToString(),
                               obj,
                               propertyInfo.PropertyType,
                               propertyInfo.Name,
                               value));
                    }
                }
                //if (typ.GetProperty("Font") != null)
                //    typ.GetProperty("Font").SetValue(obj, LanguageModel.newFont, null);
            }
            lstInfos.TrimExcess();
            return lstInfos;
        }

        private static List<LangTextInfo> GetFieldTextInfo(object obj, string[] excludeTypes, string[] PropertyNames)
        {
            List<LangTextInfo> lstInfos = new List<LangTextInfo>(); //定义集合
            if (obj != null)
            {
                //获取Fields集合
                FieldInfo[] fieldInfos = GetFieldInfo(obj);
                int intLength = fieldInfos.Length;

                //临时变量
                object objSource;//字段对象
                string strFieldInfoName;//字段名称          
                List<String[]> lstPropertyCollection;  //二维属性集合[属性名称+属性值]

                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    strFieldInfoName = fieldInfo.Name;
                    objSource = fieldInfo.GetValue(obj);//取得字段对象

                    //获取FieldInfo属性集合
                    if (objSource != null)
                    {
                        //获取二维属性集合
                        lstPropertyCollection = GetFieldPropertys(fieldInfo, objSource, excludeTypes, PropertyNames);

                        foreach (String[] strs in lstPropertyCollection)
                        {
                            //添加ObjectFieldsInfo对象值List集合
                            lstInfos.Add(new LangTextInfo(
                                fieldInfo.Name,
                                objSource,
                                fieldInfo.FieldType,
                                strs[0],
                                strs[1]));
                        }
                    }
                }

                if (obj is Form)//针对窗体本身的处理
                {
                    var form = obj as Form;
                    lstInfos.Add(new LangTextInfo(
                                 form.Name,
                                 obj,
                                 typeof(string),
                                 "Text",
                                 form.Text));
                }
            }
            lstInfos.TrimExcess();
            return lstInfos;
        }

        private static List<LangTextInfo> SingleObjTextInfo(bool IsForm, object obj, string[] excludeTypes, string[] PropertyNames)
        {
            if (IsForm)
            {
                return GetFieldTextInfo(obj, excludeTypes, PropertyNames);
            }
            else
            {
                return GetPropertyTextInfo(obj, PropertyNames);
            }
        }

        #endregion

        #region 获取FieldInfo属性名称和属性值

        /// <summary>
        /// 获取FieldInfo属性名称和属性值
        /// </summary>
        /// <param name="fieldInfo">字段</param>
        /// <param name="objSource">字段值</param>
        /// <returns></returns>
        private static List<String[]> GetFieldPropertys(FieldInfo fieldInfo,
            object objSource,
            string[] excludeTypes,
            string[] PropertyNames)
        {
            List<String[]> lstPropertys = new List<String[]>(PropertyNames.Length);
            Type typ = fieldInfo.FieldType;
            //排除Type集合内控件类型   
            if (excludeTypes == null) excludeTypes = Param.ExcludeTypes;
            bool bTypeIN = excludeTypes.Contains(typ.FullName);

            if (!bTypeIN)
            {
                //string[] strTexts = new string[PropertyNames.Length];
                var propertyInfos = new PropertyInfo[PropertyNames.Length];
                PropertyInfo propertyInfo;

                for (int i = 0; i < PropertyNames.Length; i++)
                {
                    propertyInfo = propertyInfos[i];

                    //propertyInfo = typ.GetProperty(PropertyNames[i],
                    //    PropertyNames[i] == "Tag" ? typeof(object) : typeof(string));

                    propertyInfo = typ.GetProperty(PropertyNames[i]);

                    if (propertyInfo != null)
                    {
                        string value = Convert.ToString(propertyInfo.GetValue(objSource, null));

                     
                        //if (CheckString(ref value))
                        {
                            lstPropertys.Add(new string[]
                            {
                                PropertyNames[i],
                                value
                            });
                        }
                    }
                }
            }
            //if (objSource.GetType().GetProperty("Font") != null)
            //    objSource.GetType().GetProperty("Font").SetValue(objSource, LanguageModel.newFont, null);
            lstPropertys.TrimExcess();
            return lstPropertys;
        }
        #endregion

    }
}
