using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common.IApplication
{
    public interface IWorkCurve
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        ElementList GetIntestedElementList(string filePath, string workCurveDir);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        List<Condition> ReadFileCondition(string filePath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Compounds ReadFileCompounds(string filePath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        void ReadElementsSample(string filePath, CurveElement elements);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        List<DemarcateEnergy> GetConditionDemarcate(string filePath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        QualeElement GetQualeElement(string filePath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="filePath"></param>
        void CreateCurve(WorkCurve curve, string filePath, List<Skyray.EDXRFLibrary.Condition> conditions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        //CalibrationParam GetCalibrationParam(string filePath);
    }
}
   