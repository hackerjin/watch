using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Skyray.EDXRFLibrary.Spectrum
{
    public class FileDAL : ISpectrumDAL
    {

        public string Path;
        public FileDAL(string path)
        {
            this.Path = path;
        }
        #region ISpectrumDAL 成员

        public void Save(SpecListEntity specList)
        {
            SerializeHelper.ObjToFile(specList, Path + "\\" + specList.Name + ".Spec");
        }

       // public void Save(SpecListEntity specList, string deviceName)
        public void Save(SpecListEntity specList, long specListId)
        {
            SerializeHelper.ObjToFile(specList, Path + "\\" + specList.Name + ".Spec");
        }

        public SpecListEntity Query(string name)
        {
            SpecListEntity entity = null;
            string fileFullName = File.Exists(name) ? name : Path + "\\" + name + ".Spec";
            FileInfo info = new FileInfo(fileFullName);
            if (info.Exists)
            {
                entity = (SpecListEntity)SerializeHelper.FileToObj(fileFullName);
            }
            return entity;
        }

        public List<SpecListEntity> Query(SqlParams[] sqlParams)
        {
            List<SpecListEntity> tt = new List<SpecListEntity>();
            DirectoryInfo info = new DirectoryInfo(Path);
            FileInfo[] files = info.GetFiles("*.Spec");
            if (files != null && files.Length > 0)
            {
                foreach (var temp in files)
                {
                    SpecListEntity tempResult = Query(temp.Name.Replace(".Spec", "").Trim());
                    Type type = tempResult.GetType();
                    int count = 0;
                    foreach (var sqlPas in sqlParams)
                    {
                        PropertyInfo propertyInfo = type.GetProperty(sqlPas.Label);
                        if (propertyInfo != null)
                        {
                            object obj = propertyInfo.GetValue(tempResult, null);
                            if (sqlPas.Islike)
                            {
                                string matter = sqlPas.Value;
                                if (!string.IsNullOrEmpty(sqlPas.PreSuffix))
                                    matter = sqlPas.PreSuffix.Replace("%", "[a-z]?") + matter;
                                if (!string.IsNullOrEmpty(sqlPas.AfterSuffix))
                                    matter += sqlPas.AfterSuffix.Replace("%", "[a-z]?");
                                Regex regex = new Regex(matter, RegexOptions.IgnoreCase);
                                if (regex.IsMatch(obj.ToString()))
                                    count++;
                            }
                            else
                            {
                                if (obj.ToString().IndexOf(sqlPas.Value) >= 0)
                                {
                                    count++;
                                }
                            }
                        }
                    }
                    if (count == sqlParams.Length)
                        tt.Add(tempResult);
                }
            }
            return tt;
        }

        //private DataTable XmlToDataTable(string fileName)
        //{
        //    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
        //    doc.Load(fileName);
        //    DataTable dt = new DataTable();
        //    //以第一个元素song的子元素建立表结构
        //    XmlNode songNode = doc.SelectSingleNode("Spectrum");
        //    string colName;
        //    if (songNode != null)
        //    {
        //        for (int i = 0; i < songNode.ChildNodes.Count; i++)
        //        {
        //            colName = songNode.ChildNodes.Item(i).Name;
        //            dt.Columns.Add(colName);
        //        }
        //        System.Data.DataRow dr = dt.NewRow();
        //        for (int i = 0; i < songNode.ChildNodes.Count; i++)
        //        {
        //            dr[songNode.ChildNodes.Item(i).Name] = songNode.ChildNodes.Item(i).InnerText;
        //        }
        //        dt.Rows.Add(dr);
        //    }
        //    return dt;
        //}

        public bool ExistsRecord(string name,out int specType)
        {
            specType = 0;
            bool result = false;
            string fileFullName = Path + "\\" + name + ".Spec";
            FileInfo info = new FileInfo(fileFullName);
            result = info.Exists;
            if (result)
            {
                SpecListEntity entity = Query(name);
                if (entity != null)
                    specType = entity.SpecTypeValue;
            }
            return result;
        }

        public void DeleteRecord(string name)
        {
            string fileFullName = Path + "\\" + name + ".Spec";
            FileInfo info = new FileInfo(fileFullName);
            if (info.Exists)
                info.Delete();
        }

        public int ReturnRecordCount()
        {
            DirectoryInfo info = new DirectoryInfo(Path);
            return info.GetFiles("*.Spec").Count();
        }

        public List<SpecListEntity> GetSpecList(string strWhere)
        {
            List<SpecListEntity> tt = new List<SpecListEntity>();
            string[] str = strWhere.Split(',');
            if (str.Length > 0)
            {
                foreach (string temp in str)
                    tt.Add(Query(temp));
            }
            return tt;
        }
        public List<SpecListEntity> GetSpecListById(string strWhereId)
        {
            return null;
        }
        public List<SpecListEntity> GetAllSpectrum()
        {
            List<SpecListEntity> tt = new List<SpecListEntity>();
            DirectoryInfo info = new DirectoryInfo(Path);
            FileInfo[] files = info.GetFiles("*.Spec");
            if (files != null && files.Length > 0)
            {
                foreach (var temp in files)
                    tt.Add(Query(temp.Name.Replace(".Spec", "").Trim()));
            }
            return tt;
        }
        public List<SpecListEntity> ResearchByConditions(string strName, string strBeginT, string strEndT, string strOrName, string strOrTime)
        {
            List<SpecListEntity> tt = new List<SpecListEntity>();
            return tt;
        }
        private string _strCurDeviceName = null;
        public void SetCurDeviceName(string strDeviceName)
        {
            _strCurDeviceName = strDeviceName;
        }

        public decimal HandleSpecListEntityByConditions(string strName, string strBeginT, string strEndT, string strOrName, string strOrTime, int? limit, Func<SpecListEntity, int> func)
        {
            return 1;
        }
        #endregion
    }
}
