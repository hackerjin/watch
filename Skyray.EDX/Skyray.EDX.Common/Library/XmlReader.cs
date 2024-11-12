using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Skyray.EDXRFLibrary;
using System.Reflection;
using System.Data;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common;

namespace Skyray.EDX.Common.Library
{
    /// <summary>
    /// XmlReader
    /// </summary>
    class XmlReader
    {

        
        private const string _error = "Error Occured";

        private const string _fileNotExists = "File Not Exists";

        private const string _directoryNotExists = "Directory Not Exists";

        private const string _fileReadException = "File Read Exception";

        public enum Type
        {
            String,
            Int,
            Long,
            Imag,
            DateTime,
            SpecType
        }

        public static bool IsFileExist(string strFileName)
        {
            bool bFlag = false;
            if (string.IsNullOrEmpty(strFileName))
                return bFlag;
            bFlag = File.Exists(strFileName);
            return bFlag;
        }

        public static bool IsDirectoryExists(string strPath)
        {
            bool bFlag = false;
            if (string.IsNullOrEmpty(strPath))
                return bFlag;
            bFlag = Directory.Exists(strPath);
            return bFlag;
        }

        public static string GetNodeInnerText(XmlNode rootXmlNode, string nodeName)
        {
            string strValue = string.Empty;
            if (rootXmlNode == null)
                return strValue;
            XmlNode Param = rootXmlNode.SelectSingleNode(nodeName);
            if (Param == null)
                return strValue;
            strValue = Param.InnerText;
            return strValue;
        }

        public static T GetNodeInnerText<T>(XmlNode rootXmlNode, string nodeName, Type type) //where T: 
        {
            if (type == Type.Int)
            {
                return (T)(Object)ConvertToInt(GetNodeInnerText(rootXmlNode, nodeName));
            }
            else if (type == Type.DateTime)
            {
                return (T)(Object)ConvertToDateTime(GetNodeInnerText(rootXmlNode, nodeName));
            }
            else if (type == Type.Imag)
            {
                return (T)(Object)ConvertToByteArray(GetNodeInnerText(rootXmlNode, nodeName));
            }
            else if (type == Type.Long)
            {
                return (T)(Object)ConvertToLong(GetNodeInnerText(rootXmlNode, nodeName));
            }
            else if (type == Type.SpecType)
            {
                return (T)(Object)ConvertToSpecType(GetNodeInnerText(rootXmlNode, nodeName));
            }
            else
            {
                return (T)(Object)GetNodeInnerText(rootXmlNode, nodeName);
            }
        }
        //public static T GetNodeInnerText<T>(XmlNode rootXmlNode, string nodeName)
        //{
        //    if (System.Type.GetTypeCode(typeof(T)) == TypeCode.Int32)
        //    {
        //        return (T)(Object)ConvertToInt(GetNodeInnerText(rootXmlNode, nodeName));
        //    }
        //    else if (System.Type.GetTypeCode(typeof(T)) == TypeCode.DateTime)
        //    {
        //        return (T)(Object)ConvertToDateTime(GetNodeInnerText(rootXmlNode, nodeName));
        //    }
        //    else if (System.Type.GetTypeCode(T) == Type.Imag)
        //    {
        //        return (T)(Object)ConvertToByteArray(GetNodeInnerText(rootXmlNode, nodeName));
        //    }
        //    else if (System.Type.GetTypeCode(T) == Type.Long)
        //    {
        //        return (T)(Object)ConvertToLong(GetNodeInnerText(rootXmlNode, nodeName));
        //    }
        //    else if (System.Type.GetTypeCode(T) == Type.SpecType)
        //    {
        //        return (T)(Object)ConvertToSpecType(GetNodeInnerText(rootXmlNode, nodeName));
        //    }
        //    else
        //    {
        //        return (T)(Object)GetNodeInnerText(rootXmlNode, nodeName);
        //    }
        //}
        public static int ConvertToInt(string strValue)
        {
            int iValue = 0;
            if(string.IsNullOrEmpty(strValue))
            return iValue;
            if (int.TryParse(strValue, out iValue))
            {
                return iValue;
            }
            else
            {
                return 0;
            }
        }

        public static DateTime ConvertToDateTime(string strValue)
        {
            DateTime dtValue = System.DateTime.Now;
            if (string.IsNullOrEmpty(strValue))
                return dtValue;
            if(DateTime.TryParse(strValue ,out dtValue ))
            {
                return dtValue ;
            }
            else
            {
                return dtValue;
            }
        }

        public static SpecType ConvertToSpecType(string strValue)
        {
            try
            {
                return (SpecType)Enum.Parse(typeof(SpecType), strValue);
            }
            catch
            {
                return SpecType.StandSpec;
            }
        }

        public static long ConvertToLong(string strValue)
        { 
            long lValue =(long) 0.0;
            if(string.IsNullOrEmpty(strValue))
            return lValue;
            if (long.TryParse(strValue, out lValue))
            {
                return lValue;
            }
            else
            {
                return lValue;
            }
        }
        public static byte[] ConvertToByteArray(string strValue)
        { 
            byte[] btValue =new byte[1];
            if(string.IsNullOrEmpty(strValue))
            return btValue;
            try
            {
                //return new byte[1] { Convert.ToByte(strValue) };
                return Convert.FromBase64String(strValue);
            }
            catch
            {
                return btValue;
            }
        }

        public static T ConvertType<T>(object value,T defaultValue)
        {
            try
            {
                return (T)ConvertToT<T>(value,defaultValue);
            }
            catch
            {
                return default(T);
            }
        }
        private static object ConvertToT<T>(object myvalue,T defaultValue)
        {
            TypeCode typeCode = System.Type.GetTypeCode(typeof(T));
            if (myvalue != null)
            {
                string value = Convert.ToString(myvalue);
                #region 类型判断和转换
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        bool flag = false;
                        if (bool.TryParse(value, out flag))
                        {
                            return flag;
                        }
                        break;
                    case TypeCode.Char:
                        char c;
                        if (Char.TryParse(value, out c))
                        {
                            return c;
                        }
                        break;
                    case TypeCode.SByte:
                        sbyte s = 0;
                        if (SByte.TryParse(value, out s))
                        {
                            return s;
                        }
                        break;
                    case TypeCode.Byte:
                        byte b = 0;
                        if (Byte.TryParse(value, out b))
                        {
                            return b;
                        }
                        break;
                    case TypeCode.Int16:
                        Int16 i16 = 0;
                        if (Int16.TryParse(value, out i16))
                        {
                            return i16;
                        }
                        break;
                    case TypeCode.UInt16:
                        UInt16 ui16 = 0;
                        if (UInt16.TryParse(value, out ui16))
                            return ui16;
                        break;
                    case TypeCode.Int32:
                        int i = 0;
                        if (Int32.TryParse(value, out i))
                        {
                            return i;
                        }
                        break;
                    case TypeCode.UInt32:
                        UInt32 ui32 = 0;
                        if (UInt32.TryParse(value, out ui32))
                        {
                            return ui32;
                        }
                        break;
                    case TypeCode.Int64:
                        Int64 i64 = 0;
                        if (Int64.TryParse(value, out i64))
                        {
                            return i64;
                        }
                        break;
                    case TypeCode.UInt64:
                        UInt64 ui64 = 0;
                        if (UInt64.TryParse(value, out ui64))
                            return ui64;
                        break;
                    case TypeCode.Single:
                        Single single = 0;
                        if (Single.TryParse(value, out single))
                        {
                            return single;
                        }
                        break;
                    case TypeCode.Double:
                        double d = 0;
                        if (Double.TryParse(value, out d))
                        {
                            return d;
                        }
                        break;
                    case TypeCode.Decimal:
                        decimal de = 0;
                        if (Decimal.TryParse(value, out de))
                        {
                            return de;
                        }
                        break;
                    case TypeCode.DateTime:
                        DateTime dt;
                        if (DateTime.TryParse(value, out dt))
                        {
                            return dt;
                        }
                        break;
                    case TypeCode.String:
                        if (!string.IsNullOrEmpty(value))
                        {
                            return value.ToString();
                        }
                        break;
                }
                #endregion
            }

            return defaultValue;
        }

        public static void ReadXml(string strFileName,Condition condition)
        {
            if (!IsFileExist(strFileName))
            {
                MessageBox.Show(_fileNotExists);
                return;
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(strFileName);
                XmlNode Spectrum = doc.SelectSingleNode("Spectrum");
                SpecListEntity specList = new SpecListEntity();
                specList.Color = GetNodeInnerText<int>(Spectrum, "Color", Type.Int);
                //specList.Id = GetNodeInnerText<int>(Spectrum, "Id", Type.Int);
                //specList.Image = GetNodeInnerText<Byte[]>(Spectrum, "Image", Type.Imag);                
                specList.Name = GetNodeInnerText<string>(Spectrum, "Name", Type.String);
                specList.Operater = GetNodeInnerText<string>(Spectrum, "Operator", Type.String);
                specList.SampleName = GetNodeInnerText<string>(Spectrum, "SampleName", Type.String);    
                specList.Shape = GetNodeInnerText<string>(Spectrum, "Shape", Type.String);
                specList.SpecDate = GetNodeInnerText<DateTime>(Spectrum, "SpecDate", Type.DateTime);
                specList.SpecSummary = GetNodeInnerText<string>(Spectrum, "SpecSummary", Type.String);
                specList.SpecType = GetNodeInnerText<SpecType>(Spectrum, "SpecType", Type.SpecType);
                specList.Supplier = GetNodeInnerText<string>(Spectrum, "Supplier", Type.String);
                specList.VirtualColor = GetNodeInnerText<int>(Spectrum, "VirtualColor", Type.Int);
                specList.Weight = GetNodeInnerText<long>(Spectrum, "Weight", Type.Long);
                specList.WorkCurveName = GetNodeInnerText<string>(Spectrum, "WorkCurveName", Type.Long);
                //specList.Condition = condition;

                SpecEntity spec = new SpecEntity();
                spec.SpecData = GetNodeInnerText<string>(Spectrum, "Data", Type.String);
                spec.TubVoltage = GetNodeInnerText<int>(Spectrum, "TubCurrent", Type.Int);
                spec.TubVoltage = GetNodeInnerText<int>(Spectrum, "TubVoltage", Type.Int);

                specList.Specs = new SpecEntity[1];
                specList.Specs[0] = spec;
                specList.InitParam = condition.InitParam.ConvertToNewEntity();
                WorkCurveHelper.DataAccess.Save(specList);
            }
            catch(Exception e)
            {
                throw new Exception(_fileReadException + ":\r\n" + e.Message);
            }
        }

        public static void ReadXmls(string strDirectory)
        {
            if (!IsDirectoryExists(strDirectory))
            {
                MessageBox.Show(_directoryNotExists);
                return;
            }
            //DllExport.SaveToXMLFor600(strDirectory);
            Condition condition = Condition.New.Init("aa");

            DirectoryInfo dir = new DirectoryInfo(strDirectory);
            if (dir == null)
                return;
            FileInfo[] files = dir.GetFiles("*.xml");
            foreach( FileInfo file in files )
            {
                string strEx = file.Extension.ToUpper();
                string strFileName = file.FullName;
                ReadXml(strFileName,condition);
            }
        }
    }
}
