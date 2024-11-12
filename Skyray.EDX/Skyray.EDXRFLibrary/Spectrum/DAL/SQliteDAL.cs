using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace Skyray.EDXRFLibrary.Spectrum
{
    public class SQliteDAL : ISpectrumDAL
    {

        private string _strCurDeviceName = null;
        public void SetCurDeviceName(string strDeviceName)
        {
            _strCurDeviceName=strDeviceName;
        }
        #region IDAL 成员
        public void Save(SpecListEntity model)
        {
            byte[] obj = SerializeHelper.SerializeObj(model);
            SpectrumData data = SpectrumData.New.Init(model.Name, model.Height,model.CalcAngleHeight, model.DeviceName, model.WorkCurveName, model.NameType, model.SpecType, model.SampleName, obj, model.SpecDate);
            data.Save();
        }

        //public void Save(SpecListEntity model, string deviceName)
        //{
        //    byte[] obj = SerializeHelper.SerializeObj(model);
        //    SpectrumData data = SpectrumData.New.Init(model.Name, deviceName, model.WorkCurveName, model.NameType, model.SpecType, model.SampleName, obj, model.SpecDate);
        //    data.Save();
        //}
        public void Save(SpecListEntity model, long specListId)
        {
            
            SpectrumData data = SpectrumData.FindOne(w => w.Name == model.Name && w.Id == specListId);
            if (data != null) model.DeviceName = data.DeviceName;
            byte[] obj = SerializeHelper.SerializeObj(model);
            if (data == null)
                data = SpectrumData.New.Init(model.Name, model.Height,model.CalcAngleHeight, model.DeviceName, model.WorkCurveName, model.NameType, model.SpecType, model.SampleName, obj, model.SpecDate);
            else
            {
                //data.DeviceName = model.DeviceName;
                data.WorkCurveName = model.WorkCurveName;
                data.NameType = model.NameType;
                data.SampleName = model.SampleName;
                data.SpecDate = model.SpecDate;
                data.Data = obj;
            }
            //    SpectrumData data = SpectrumData.New.Init(model.Name, deviceName, model.WorkCurveName, model.NameType, model.SpecType, model.SampleName, obj, model.SpecDate);
             data.Save();
        }

        public SpecListEntity Query(string name)
        {
            SpectrumData data = SpectrumData.FindOne(w => w.Name == name&&w.DeviceName==_strCurDeviceName);
            if (data == null) return null;
            SpecListEntity entity = SerializeHelper.DeSerializeObj(data.Data);
            return entity;
        }

        public List<SpecListEntity> Query(SqlParams[] sqlParams)
        {
            List<SpecListEntity> returnData = new List<SpecListEntity>();
            if (sqlParams == null || sqlParams.Length == 0)
                return returnData;
            string sql = @"select * from SpectrumData where 1=1 and DeviceName='"+_strCurDeviceName+"'";
            foreach (var tt in sqlParams)
            {
                if (tt.Islike&&!tt.IsValueType)
                    sql += " and " + (tt.Label + " like " + tt.PreSuffix +"'"+ tt.Value + tt.AfterSuffix+"'");
                else if (!tt.Islike && !tt.IsValueType)
                    sql += " and " + tt.Label + "='" + tt.Value+"'";
                else if (!tt.Islike && tt.IsValueType)
                    sql += " and " + tt.Label + "=" + tt.Value;
            }
            List<SpectrumData> findData = SpectrumData.FindBySql(sql);
            foreach(SpectrumData tt in findData)
            {
                SpecListEntity temp = SerializeHelper.DeSerializeObj(tt.Data);
                temp.DeviceName = _strCurDeviceName;
                returnData.Add(temp);
                //returnData.Add(SerializeHelper.DeSerializeObj(tt.Data));
            }
            return returnData;
        }

        public bool ExistsRecord(string name,out int specType)
        {
            specType = 0;
            SpectrumData data = SpectrumData.FindOne(w => w.Name == name && w.DeviceName==_strCurDeviceName);
            if (data == null)
                return false;
            specType = (int)data.SpecTypeValue;
            return true;
        }

        public void DeleteRecord(string name)
        {
            SpectrumData data = SpectrumData.FindOne(w => w.Name == name&&w.DeviceName==_strCurDeviceName);
            if(data!=null)
                data.Delete();
        }

        public int ReturnRecordCount()
        {
            return SpectrumData.FindAll(w=>w.DeviceName.Equals(_strCurDeviceName)).Count;
        }

        public List<SpecListEntity> GetSpecList(string strWhere)
        {
            List<SpecListEntity> returnData = new List<SpecListEntity>();
            string sql = @"select * from SpectrumData where DeviceName='"+_strCurDeviceName+"'and name in (" + strWhere+")";
            List<SpectrumData> findData = SpectrumData.FindBySql(sql);
            foreach (SpectrumData tt in findData)
            {
                //returnData.Add(SerializeHelper.DeSerializeObj(tt.Data));
                SpecListEntity temp = SerializeHelper.DeSerializeObj(tt.Data);
                temp.DeviceName = _strCurDeviceName;
                returnData.Add(temp);
            }
            return returnData;
        }

        public List<SpecListEntity> GetSpecListById(string strWhereId)
        {
            List<SpecListEntity> returnData = new List<SpecListEntity>();
            string sql = @"select * from SpectrumData where DeviceName='" + _strCurDeviceName + "'and Id in (" + strWhereId + ")";
            List<SpectrumData> findData = SpectrumData.FindBySql(sql);
            foreach (SpectrumData tt in findData)
            {
                SpecListEntity temp = SerializeHelper.DeSerializeObj(tt.Data);
                temp.DeviceName = _strCurDeviceName;
                returnData.Add(temp);
                //returnData.Add(SerializeHelper.DeSerializeObj(tt.Data));
            }
            return returnData;
        }
        public  List<SpecListEntity> ResearchByConditions(string strName, string strBeginT, string strEndT, string strOrName, string strOrTime)
        {
            List<SpecListEntity> returnData = new List<SpecListEntity>();
            if (_strCurDeviceName == null || _strCurDeviceName==string.Empty)
                return returnData;
            string sql = "select * from SpectrumData where 1=1 ";

            if (strName != null && strName.Trim() != string.Empty) sql += " And Name like '%" + strName.Trim() + "%'";
            if (strBeginT!=null&&strBeginT.Trim()!=string.Empty
                &&strEndT!=null&&strEndT.Trim()!=string.Empty)
                sql += "And datetime(SpecDate) <=datetime('" + strEndT + " 23:59:59')" + " and datetime(SpecDate)>=datetime('" + strBeginT + " 00:00:00')";
            else if (strEndT != null && strEndT.Trim() != string.Empty)
            {
                sql += "And datetime(SpecDate) < datetime('" + strEndT + " 00:00:00')";
            }
            if (_strCurDeviceName!=null&&_strCurDeviceName.Trim()!=string.Empty)
                sql += " and DeviceName='" + _strCurDeviceName.Trim() + "'";
            sql += " order by  SpecTypeValue asc,SpecDate " + (strOrTime != null && strOrTime.Trim() != string.Empty ? strOrTime.Trim() : "asc") + ",LOWER(Name) " + (strOrName != null && strOrName.Trim() != string.Empty ? strOrName.Trim() : "asc");
            List<SpectrumData> findData = SpectrumData.FindBySql(sql);
            foreach (SpectrumData tt in findData)
            {
                SpecListEntity temp = SerializeHelper.DeSerializeObj(tt.Data);
                temp.DeviceName = _strCurDeviceName;
                returnData.Add(temp);
                //returnData.Add(SerializeHelper.DeSerializeObj(tt.Data));
            }
            return returnData;
        }
        public List<SpecListEntity> GetAllSpectrum()
        {
            List<SpecListEntity> returnData = new List<SpecListEntity>();
           var tt = SpectrumData.FindAll();
            foreach (SpectrumData temp in tt)
            {
                if (temp.DeviceName.Equals(_strCurDeviceName))
                {
                    SpecListEntity temp1 = SerializeHelper.DeSerializeObj(temp.Data);
                    temp1.DeviceName = _strCurDeviceName;
                    returnData.Add(temp1);
                   // returnData.Add(SerializeHelper.DeSerializeObj(temp.Data));
                }
            }
            return returnData;
        }

        public Decimal HandleSpecListEntityByConditions(string strName, string strBeginT, string strEndT, string strOrName, string strOrTime, int? limit, Func<SpecListEntity, int> func)
        {
            if (_strCurDeviceName == null || _strCurDeviceName == string.Empty)
                return 2;
            string sql = (limit == null ? "select count(*) from SpectrumData where 1=1 " : "select * from SpectrumData where 1=1 ");

            if (strName != null && strName.Trim() != string.Empty) sql += " And Name like '%" + strName.Trim() + "%'";
            if (strBeginT != null && strBeginT.Trim() != string.Empty
                && strEndT != null && strEndT.Trim() != string.Empty)
                sql += "And datetime(SpecDate) <=datetime('" + strEndT + " 23:59:59')" + " and datetime(SpecDate)>=datetime('" + strBeginT + " 00:00:00')";
            else if (strEndT != null && strEndT.Trim() != string.Empty)
            {
                sql += "And datetime(SpecDate) < datetime('" + strEndT + " 00:00:00')";
            }
            if (_strCurDeviceName != null && _strCurDeviceName.Trim() != string.Empty)
                sql += " and DeviceName='" + _strCurDeviceName.Trim() + "'";
            sql += " order by  SpecTypeValue asc,SpecDate " + (strOrTime != null && strOrTime.Trim() != string.Empty ? strOrTime.Trim() : "asc") + ",LOWER(Name) " + (strOrName != null && strOrName.Trim() != string.Empty ? strOrName.Trim() : "asc");
            if(limit != null)
                sql += " limit " + limit;
            else
            {
                return this.GetCount(sql);
            }
            List<SpectrumData> queryList = null;
            int result = 1;
            do
            {
                queryList = SpectrumData.FindBySql(sql);
                if (queryList == null || queryList.Count <= 0)
                    break;
                foreach (SpectrumData sd in queryList)
                {
                    SpecListEntity temp = SerializeHelper.DeSerializeObj(sd.Data);
                    temp.DeviceName = _strCurDeviceName;
                    result = func(temp);
                    if (result == 0)
                        break;
                }
                if (result == 0)
                    break;
            } while (queryList != null && queryList.Count >= limit);

            return result;
        }

        public Decimal GetCount(string sql)
        {
            string connStr = Lephone.Data.DbEntry.Context.Driver.ConnectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    object obj = cmd.ExecuteScalar();
                    if (obj != null)
                    {
                        return Convert.ToDecimal(obj);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        #endregion
    }
}
