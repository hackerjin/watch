using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDX.Common.IApplication
{
    public interface IFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryDev"></param>
        void LoadDeviceFactory(string directoryDev,string curvedir);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryDev"></param>
        void LoadCurveFactory(string directoryDev,List<Condition> conditions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryDev"></param>
        SpecListEntity LoadSpecFactory(string fileName);

        void RepeatCaculateSampleIntensity(string dataPath);
    }
}
