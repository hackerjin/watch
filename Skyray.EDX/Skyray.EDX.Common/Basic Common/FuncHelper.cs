using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Skyray.EDX.Common
{
    public class FuncHelper
    {
        public delegate void CallbackVoidHandler();// 无参数无返回值
        public delegate void CallbackObjectHandler<T>(T o);//有一个参数无返回值
        public delegate void CallbackObjectHandler2<T, T1>(T o, T1 o1);//有二个参数无返回值
        public delegate void CallbackObjectHandler3<T, T1, T2>(T o, T1 o1, T2 o2);//有三个参数无返回值

        public delegate TR RCallbackHandler<TR>();//无参数有返回值
        public delegate TR RCallbackHandler1<T, TR>(T o);//有一个参数有返回值
        public delegate TR RCallbackHandler2<T, T1, TR>(T o, T1 o1);//有二个参数有返回值
        public delegate TR RCallbackHandler3<T, T1, T2, TR>(T o, T1 o1, T2 o2);//有三个参数有返回值



        public static void CatchAll(CallbackVoidHandler callback)
        {
            IfCatchException(true, callback);
        }

        public static void IfCatchException(bool CatchException, CallbackVoidHandler callback)
        {
            if (CatchException)
            {
                try
                {
                    callback();
                }
                catch (Exception ex)
                {
                    Msg.Show(ex.Message);
                    try
                    {
                        Log.Error("ERROR".ToString(), ex);
                    }
                    catch { }
                }
            }
            else
            {
                callback();
            }
        }

       
        
        public static object RCatchAll(RCallbackHandler3<object, object, object, object> callback, object o, object o1, object o2)
        {
            return IfCatchException(true, callback, o, o1, o2);
        }
        public static object RCatchAll(RCallbackHandler2<object, object, object> callback, object o, object o1)
        {
            return IfCatchException(true, callback, o, o1);
        }
        public static object RCatchAll(RCallbackHandler1<object, object> callback, object o)
        {
            return IfCatchException(true, callback, o);
        }
        public static object RCatchAll(RCallbackHandler<object> callback)
        {
            return IfCatchException(true, callback);
        }

        public static object IfCatchException(bool CatchException, RCallbackHandler3<object, object, object, object> callback, object o, object o1, object o2)
        {
            if (CatchException)
            {
                try
                {
                    return callback(o, o1, o2);
                }
                catch (Exception ex)
                {
                    Msg.Show(ex.Message);
                    try
                    {
                        Log.Error("ERROR".ToString(), ex);
                    }
                    catch { }
                    return null;
                }
            }
            else
            {
                return callback(o, o1, o2);
            }
        }

        public static object IfCatchException(bool CatchException,
            RCallbackHandler2<object, object, object> callback, object o, object o1)
        {
            if (CatchException)
            {
                try
                {
                    return callback(o, o1);
                }
                catch (Exception ex)
                {
                    Msg.Show(ex.Message);
                    try
                    {
                        Log.Error("ERROR".ToString(), ex);
                    }
                    catch { }
                    return null;
                }
            }
            else
            {
                return callback(o, o1);
            }
        }

        public static object IfCatchException(bool CatchException, RCallbackHandler1<object, object> callback, object o)
        {
            if (CatchException)
            {
                try
                {
                    return callback(o);
                }
                catch (Exception ex)
                {
                    Msg.Show(ex.Message);
                    try
                    {
                        Log.Error("ERROR".ToString(), ex);
                    }
                    catch { }
                    return null;
                }
            }
            else
            {
                return callback(o);
            }
        }



        public static object IfCatchException(bool CatchException, RCallbackHandler<object> callback)
        {
            if (CatchException)
            {
                try
                {
                    return callback();
                }
                catch (Exception ex)
                {
                    Msg.Show(ex.Message);
                    try
                    {
                        Log.Error("ERROR", ex);
                    }
                    catch { }
                    return null;
                }
            }
            else
            {
                return callback();
            }
        }





        //public static void TryEnumerate(object obj, CallbackObjectHandler<object> callback)
        //{
        //    if (obj != null)
        //    {
        //        if (obj is IEnumerable)
        //        {
        //            foreach (object o in (IEnumerable)obj)
        //            {
        //                callback(o);
        //            }
        //        }
        //        else
        //        {
        //            callback(obj);
        //        }
        //    }
        //}
    }
}
