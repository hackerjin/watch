using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common.Component;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Skyray.EDX.Common
{
    public static class ClassExtender
    {
        private static Dictionary<Enum, double> unitDictionary;
        public static Dictionary<Enum, double> UnitDictionary
        {
            get
            {   
                if (unitDictionary==null)
                {
                    unitDictionary = new Dictionary<Enum,double>();
                    unitDictionary.Add(ContentUnit.per, 100);
                    unitDictionary.Add(ContentUnit.permillage, 1000);
                    unitDictionary.Add(ContentUnit.ppm,1000000);
                    unitDictionary.Add(ThicknessUnit.um, 1);
                    unitDictionary.Add(ThicknessUnit.ur, 1 * 0.0254);
                }
                return unitDictionary;
            }
            private set { unitDictionary = value; }
        }
            
        /// <summary>
        /// 感兴趣元素转换为历史记录元素
        /// </summary>
        /// <param name="curveElement">感兴趣元素本身</param>
        /// <param name="ignoreUnit">是否保留原单位,False保留原各自单位数据需换算,True单位一起交换数据不变</param>
        /// <returns></returns>
        public static HistoryElement ToHistroyElement(this CurveElement curveElement)
        {
            if (curveElement == null) return null;
            string sFormat = string.Empty;//"f" + WorkCurveHelper.SoftWareContentBit.ToString();
            HistoryElement historyElement = HistoryElement.New;
            //SwapCurveHistroyElement(curveElement, historyElement, false);
            historyElement.elementName = curveElement.Caption.ConvertToType(historyElement.elementName);
            historyElement.unitValue = curveElement.ContentUnit.ConvertToType(historyElement.unitValue);
            if (!(curveElement.Content > double.Epsilon && curveElement.Content < double.PositiveInfinity))
                curveElement.Content = 0d;
            historyElement.contextelementValue = curveElement.Content.ConvertToType(historyElement.contextelementValue);
            historyElement.Error = curveElement.Error.ConvertToType(historyElement.Error);
            historyElement.thickunitValue = curveElement.ThicknessUnit.ConvertToType(historyElement.thickunitValue);
            historyElement.thickelementValue = curveElement.Thickness.ConvertToType(historyElement.thickelementValue);
            historyElement.CaculateIntensity = curveElement.Intensity.ConvertToType(historyElement.CaculateIntensity);
            
            return historyElement;
        }
        /// <summary>
        /// 历史记录转换为感兴趣元素
        /// </summary>
        /// <param name="historyElement">历史记录本身</param>
        /// <param name="ignoreUnit">是否保留原单位,False数据换算后交换单位不交换,True单位交换数据不换算直接交换</param>
        /// <returns></returns>

        public static CurveElement ToCurveElement(this HistoryElement historyElement)
        {
            if (historyElement == null) return null;
            var curEle = historyElement.MatchCurveElement();
            CurveElement curveElement = curEle ?? CurveElement.New;

            //SwapCurveHistroyElement(curveElement, historyElement,false);
            curveElement.Caption = historyElement.elementName.ConvertToType(historyElement.elementName);
            curveElement.ContentUnit = historyElement.unitValue.ConvertToType(curveElement.ContentUnit);
            curveElement.ThicknessUnit = historyElement.thickunitValue.ConvertToType(curveElement.ThicknessUnit);
            curveElement.Content = historyElement.contextelementValue.ConvertToType(curveElement.Content);
            if (!(curveElement.Content > double.Epsilon && curveElement.Content < double.PositiveInfinity))
                curveElement.Content = 0d;
            curveElement.Error = historyElement.Error.ConvertToType(curveElement.Error);
            curveElement.Intensity = historyElement.CaculateIntensity.ConvertToType(curveElement.Intensity);
            curveElement.Thickness = historyElement.thickelementValue.ConvertToType(curveElement.Thickness);

            return curveElement;
        }
        /// <summary>
        /// 从现有单位转换为指定单位的换算率
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t1"></param>
        /// <param name="t2">转换到的单位</param>
        /// <returns></returns>
        public static double UnitExChangeTo<T>(this T t1,T t2) where T: struct
        {
            double back = 1.0;
            if (typeof(T).IsEnum)
            {
                Enum t1Value = t1.ConvertToType<Enum>();
                Enum t2Value = t2.ConvertToType<Enum>();
                if (UnitDictionary.Keys.Contains(t1Value) && UnitDictionary.Keys.Contains(t2Value))
                    back = UnitDictionary[t2Value] * 1.0 / (UnitDictionary[t1Value] == 0 ? UnitDictionary[t2Value] : UnitDictionary[t1Value]);
            }
            return back;
        }

        /// <summary>
        /// 将数据按单位转换成换算后该类型数据
        /// </summary>
        /// <typeparam name="T">数据源类型也是返回的类型</typeparam>
        /// <typeparam name="T1">数据源单位类型</typeparam>
        /// <typeparam name="T2">目标数据源单位类型</typeparam>
        /// <typeparam name="T3">源单位和目标单位的标准单位</typeparam>
        /// <param name="TSourceResult">数据源</param>
        /// <param name="TSourceUnit">数据源单位</param>
        /// <param name="TDestinationUnit">转换到的目标单位</param>
        /// <param name="TStandard">标准单位类型的任意取值</param>
        /// <returns></returns>
        public static T ValueExChangeByUnit<T, T1, T2,T3>(this T TSourceResult, T1 TSourceUnit, T2 TDestinationUnit,T3 TStandard)where T3:struct
            where T:IComparable
        {
            try
            {
                T3 tempSource = TSourceUnit.ConvertToType<T3>();
                T3 tempDestination = TDestinationUnit.ConvertToType<T3>();
                double dSource = TSourceResult.ConvertToType<double>();
                double dExChange = tempSource.UnitExChangeTo(tempDestination);
                return (dSource * dExChange).ConvertToType<T>();
            }
            catch { }
            return TSourceResult;
        }

        /// <summary>
        /// 感兴趣元素与历史记录元素互换
        /// </summary>
        /// <param name="curveElement">感兴趣元素</param>
        /// <param name="historyElement">历史记录元素</param>
        /// <param name="ignoreUnit">是否保留原单位,False保留原各自单位数据需换算,True单位一起交换数据不变</param>
        /// <returns></returns>
        public static void SwapCurveHistroyElement(CurveElement curveElement, HistoryElement historyElement, bool ignoreUnit)
        {
            if (curveElement == null || historyElement == null)
                return;
            object value = curveElement.Caption;
            curveElement.Caption = historyElement.elementName;
            historyElement.elementName = value.ConvertToType<string>();

            if (ignoreUnit)//不将值进行单位换算，直接将单位互换值不变
            {
                value = curveElement.ContentUnit;
                curveElement.ContentUnit = historyElement.unitValue.ConvertToType(curveElement.ContentUnit);
                historyElement.unitValue = value.ConvertToType(historyElement.unitValue);

                value = curveElement.ThicknessUnit;
                curveElement.ThicknessUnit = historyElement.thickunitValue.ConvertToType(curveElement.ThicknessUnit);
                historyElement.thickunitValue = value.ConvertToType(historyElement.thickunitValue);
            }
            
            value = curveElement.Content;
            curveElement.Content = historyElement.contextelementValue.ConvertToType<double>();            
            historyElement.contextelementValue = value.ConvertToType<string>();
            
            value = curveElement.Intensity;
            curveElement.Intensity = historyElement.ElementIntensity.ConvertToType(curveElement.Intensity);
            historyElement.ElementIntensity = value.ConvertToType(historyElement.ElementIntensity);

            value = curveElement.Error;
            curveElement.Error = historyElement.Error.ConvertToType(curveElement.Error);
            historyElement.Error = value.ConvertToType(historyElement.Error);

            value = curveElement.Thickness;
            curveElement.Thickness = historyElement.thickelementValue.ConvertToType(curveElement.Thickness);
            historyElement.thickunitValue = value.ConvertToType(historyElement.thickunitValue);
            if (!ignoreUnit)//不置换单位即需要单位换算
            {
                curveElement.Content = curveElement.Content.ValueExChangeByUnit(historyElement.unitValue, curveElement.ContentUnit, ContentUnit.per);//历史记录单位转换为感兴趣元素单位
                curveElement.Error = curveElement.Error.ValueExChangeByUnit(historyElement.unitValue, curveElement.ContentUnit, ContentUnit.per);
                curveElement.Thickness = curveElement.Thickness.ValueExChangeByUnit(historyElement.thickunitValue, curveElement.ThicknessUnit, ThicknessUnit.ur);
                historyElement.contextelementValue = historyElement.contextelementValue.ValueExChangeByUnit(curveElement.ContentUnit, historyElement.unitValue, ContentUnit.per);
                historyElement.Error = historyElement.Error.ValueExChangeByUnit(curveElement.ContentUnit, historyElement.unitValue, ContentUnit.per);
                historyElement.thickelementValue = historyElement.thickelementValue.ValueExChangeByUnit(curveElement.ThicknessUnit, historyElement.thickunitValue, ThicknessUnit.ur);
            }
        }

        public static void Swap<T>(ref T t1,ref T t2)
        {
            T temp = default(T);
            temp = t1;
            t1 = t2;
            t2 = temp;
        }

        public static void SwapDiff<T1, T2>(ref T1 t1, ref T2 t2)
        {
            T1 temp = default(T1);
            temp = t2.ConvertToType(t1);
            Swap(ref t1,ref temp);
            t2 = temp.ConvertToType(t2);
        }
        /// <summary>
        /// 泛型类型转换 针对一般简单类型和实现了强制转换 TryParse的类型
        /// </summary>
        /// <typeparam name="T">要转换的基础类型</typeparam>
        /// <param name="val">要转换的值</param>
        /// <param name="target">需要转换到并可能存入的对象</param>
        /// <returns></returns>
        /// <remark>
        /// </remark>
        public static T ConvertToType<T>(this object val,params T[] target)
        {
            if (val == null) return default(T);//返回类型的默认值
            try
            {
                return (T)val;
            }
            catch(InvalidCastException) { }
            Type tp = typeof(T);
            if (tp.IsGenericType)//泛型Nullable判断，取其中的类型
                tp = tp.GetGenericArguments()[0];
            if (tp == typeof(string))//string直接返回转换
                return val.ToString().ConvertToType<T>();            
            //反射获取TryParse方法
            var TryParse = tp.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
                                            new Type[] { typeof(string), tp.MakeByRefType() },
                                            new ParameterModifier[] { new ParameterModifier(2) });
            var parameters = new object[] { val.ToString(), Activator.CreateInstance(tp) };//必须先声明数组否则无法返回
            bool success = TryParse==null?false: (bool)TryParse.Invoke(null, parameters);
            if (success)//成功返回转换后的值，否则返回类型的默认值
                return (T)parameters[1];
            return default(T);
        }

        /// <summary>
        /// 历史记录匹配感兴趣元素
        /// </summary>
        /// <param name="historyElement"></param>
        /// <returns></returns>
        public static CurveElement MatchCurveElement(this HistoryElement historyElement)
        {
            var _HistoryRecord = historyElement.HistoryRecord;
            if (_HistoryRecord == null) return null;
            var _WorkcCurve = WorkCurve.FindById(_HistoryRecord.WorkCurveId);
            if (_WorkcCurve == null) return null;
            var _ElementList = _WorkcCurve.ElementList;
            if (_ElementList == null) return null;
            var _Items = _ElementList.Items;
            if (_Items == null || _Items.Count == 0) return null;
            return _Items.FirstOrDefault(w=>w.Caption==historyElement.elementName);
        }
        /// <summary>
        /// 字典增加相同类型的字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dicBase"></param>
        /// <param name="dicParts"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> AddRangs<TKey, TValue>(this Dictionary<TKey, TValue> dicBase, params Dictionary<TKey, TValue>[] dicParts)
        {
            foreach(var dic in dicParts.SkipWhile(w=>w==null))
            {
                foreach (var key in dic.Keys)
                {
                    if(!dicBase.Keys.Contains(key))
                        dicBase.Add(key, dic[key]);
                }
            }
            return dicBase;
        }
        /// <summary>
        /// 根据其他集合排序现有集合
        /// </summary>
        /// <typeparam name="T1">类型1</typeparam>
        /// <typeparam name="T2">类型2</typeparam>
        /// <param name="enumer1">基础集合</param>
        /// <param name="enumer2">排序集合</param>
        /// <param name="func">排序方式</param>
        /// <returns>返回原集合</returns>
        public static IEnumerable<T1> OrderBy<T1, T2>(this IEnumerable<T1> enumer1, IEnumerable<T2> enumer2, Func<T2, T1> func)
        {
            if (func == null||enumer2==null)
                foreach (var enu in enumer1)
                    yield return enu;
            foreach (var enu in enumer2)
                yield return func(enu);
        }
        /// <summary>
        /// 根据其他集合排序生成集合型object
        /// </summary>
        /// <typeparam name="T1">本身集合类型</typeparam>
        /// <typeparam name="T2">对象集合类型</typeparam>
        /// <param name="enumer1">本身集合</param>
        /// <param name="enumer2">排序集合</param>
        /// <param name="func">排序方法</param>
        /// <returns></returns>
        public static object ObjectOrderBy<T1, T2>(this IEnumerable<T1> enumer1, IEnumerable<T2> enumer2, Func<T2, T1> func)
        {
            if (func == null||enumer1==null||enumer2==null)
                return enumer1;
            Type type = enumer1.GetType();
            object obj = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("Add");
            foreach (var enu in enumer2.SkipWhile(w => w == null))
            {
                T1 o = func(enu);
                method.Invoke(obj, new object[] { o });
            }
            return obj;
        }
        /// <summary>
        /// 集合根据其他集合排序返回本身类型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t"></param>
        /// <param name="enumer2"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T OrderBy<T,T1,T2>(this T t,IEnumerable<T2> enumer2,Func<T2,T1> func) where T :IEnumerable<T1>
        {
            IEnumerable<T1> enumer = t as IEnumerable<T1>;
            if (enumer == null) return t;
            object o = enumer.ObjectOrderBy(enumer2, func);
            return (T)o;
        }
        /// <summary>
        /// 自身排序返回自身类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T OrderBySelf<T, T1, T2>(this T t, Func<T1, T2> func) where T:IEnumerable<T1>
        {
            IEnumerable<T1> enumer = t as IEnumerable<T1>;
            if (t == null || enumer == null) return t;
            object obj = CreateInstance(t);
            MethodInfo method = t.GetType().GetMethod("Add");
            foreach (var value in enumer.OrderBy(func).SkipWhile(w => w == null))
            {
                method.Invoke(obj, new object[] { value });
            }
            return (T)obj;
        }
        /// <summary>
        /// 按构造函数类型创建默认对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(this T t)
        {
            Type type = typeof(T);
            object obj = type.CreateInstanceLoop();
            return (T)obj;
        }
        /// <summary>
        /// 根据Type创建实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateInstanceLoop(this Type type)
        {
            ConstructorInfo[] cis = type.GetConstructors();
            List<object> objs = new List<object>();
            if (cis == null||cis.Count()==0)
                return Activator.CreateInstance(type,objs.ToArray());//无构造函数直接返回
            var ci = cis.OrderBy(w => w.GetParameters().Count()).FirstOrDefault();            
            ParameterInfo[] pis = ci.GetParameters();            
            foreach (var p in pis)
            {
                object o=null;
                if (p.ParameterType.IsClass && p.ParameterType != typeof(string))//类获取构造函数构造
                    o = p.ParameterType.CreateInstanceLoop();//递归获取参数类型的参数
                else if (p.ParameterType == typeof(string))//string赋值空
                    o = string.Empty;
                else
                    o = Activator.CreateInstance(p.ParameterType);
                objs.Add(o);
            }
            return Activator.CreateInstance(type, objs.ToArray());//根据构造函数参数构造对象
        }

        public static double Round(this double dSource, int decimals)
        {
            return Math.Round(dSource, decimals);
        }

        public static double Round(this double dSource, int decimals, double ndValue)
        {
            double dBack = dSource.Round(decimals);
            return dBack <= ndValue ? double.NaN : dBack;
        }

    }
}
