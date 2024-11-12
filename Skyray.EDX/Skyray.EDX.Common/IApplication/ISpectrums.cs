using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using System.IO;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDX.Common.IApplication
{
    public interface ISpectrums
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        void GetSpectrum(FileInfo filePath, SpecType type, SpecListEntity specList, List<Device> deviceList);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <returns></returns>
        //Condition GetSpecCondition(string filePath);
    }
}
