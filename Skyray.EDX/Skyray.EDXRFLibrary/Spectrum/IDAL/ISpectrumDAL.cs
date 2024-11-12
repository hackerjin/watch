using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Skyray.EDXRFLibrary.Spectrum
{
    public interface ISpectrumDAL
    {
        /// <summary>
        /// 保存谱数据
        /// </summary>
        /// <param name="specList"></param>
        void Save(SpecListEntity specList);

        /// <summary>
        /// 保存谱数据
        /// </summary>
        /// <param name="specList"></param>
        //void Save(SpecListEntity specList, string deviceName);
        void Save(SpecListEntity specList, long specListId);

        /// <summary>
        /// 根据名称查询记录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        SpecListEntity Query(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        List<SpecListEntity> Query(SqlParams[] sqlParams);

        /// <summary>
        /// 是否存在指定名字记录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ExistsRecord(string name,out int specType);

        /// <summary>
        /// 删除指定名字记录
        /// </summary>
        /// <param name="name"></param>
        void DeleteRecord(string name);

        /// <summary>
        /// 当前记录数目
        /// </summary>
        /// <returns></returns>
        int ReturnRecordCount();

        /// <summary>
        /// 有条件获得数据列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        List<SpecListEntity> GetSpecList(string strWhere);

        List<SpecListEntity> GetSpecListById(string strWhereId);

        /// <summary>
        /// 按条件寻找谱
        /// </summary>
        /// <param name="strName">谱名</param>
        /// <param name="strBeginT">开始时间</param>
        /// <param name="strEndT">结束时间</param>
        /// <param name="strOrType">排序种类</param>
        /// <param name="Oder">排序的升降</param>
        /// <returns></returns>
        List<SpecListEntity> ResearchByConditions(string strName, string strBeginT, string strEndT, string strOrName, string strOrTime);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        List<SpecListEntity> GetAllSpectrum();
        void SetCurDeviceName(string strDeviceName);

        /// <summary>
        /// 针对数据庞大的情况，限制每次处理的数量，循环执行特定操作
        /// </summary>
        /// <param name="strName">谱名</param>
        /// <param name="strBeginT">开始时间</param>
        /// <param name="strEndT">结束时间</param>
        /// <param name="strOrType">排序种类</param>
        /// <param name="Oder">排序的升降</param>
        /// <param name="limit">每次处理的数量</param>
        /// <param name="func">要进行的操作，由外部传入</param>
        /// <returns></returns>
        decimal HandleSpecListEntityByConditions(string strName, string strBeginT, string strEndT, string strOrName, string strOrTime, int? limit, Func<SpecListEntity, int> func);

    }
}
