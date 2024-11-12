using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;


namespace Skyray.EDX.Common.IApplication
{
    public interface IDevice
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
       void ReadFileCollimator(string filePath,Device device);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
       void ReadFileFilter(string filePath, Device device);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePaht"></param>
        /// <returns></returns>
       void ReadFileDetector(string filePaht, Device device);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
       void ReadFileChamber(string filePath, Device device);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
       void ReadFileRaytube(string filePath, Device device);
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
       void IsExistMoveAis(string filePath,Device device);
    }

}
