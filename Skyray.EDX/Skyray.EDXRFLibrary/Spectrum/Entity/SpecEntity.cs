using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Skyray.EDXRFLibrary.Spectrum
{
    [Serializable]
    public class SpecEntity
    {
       
        public double SpecTime { get; set; }
        public double UsedTime { get; set; }
        public double TubVoltage { get; set; }
        public double TubCurrent { get; set; }
        public string RemarkInfo { get; set; }
        public bool IsSmooth { get; set; }
        public string Name { get; set; }
#if SqlServer
        [Length(ColLength.SpecDataByte)]
        public  byte[] SpecDataByte { get; set; }

        private string _SpecData = string.Empty;
        /// <summary>
        ///子谱数据
        /// </summary>
        public string SpecData
        {
            get
            {
                if (_SpecData == string.Empty)
                {
                    _SpecData = Helper.ToString(SpecDataByte);
                }
                return _SpecData;
            }
            set
            {
                _SpecData = value;
                SpecDataByte = Helper.ToBytes(_SpecData);
            }
        }
#else



        public string SpecData
        {

            get;
            set;



        }
#endif

        public int[] changeSpecData(int[] data, float coeff)
        {
            coeff = Math.Abs(coeff);
            if(coeff == 1)
                return data;

            int[] newData = (int[])data.Clone();
            
            //压缩
            if (coeff > 1)
            {
                int splitIndex = (int)(data.Length / coeff);
                for (int i = 0; i < data.Length; i++)
                {
                    if(i < splitIndex)
                    {
                        int lowIndex = (int)(i * coeff);
                        int highIndex = lowIndex + 1;
                        newData[i] = (int)(data[lowIndex] + (highIndex < data.Length ? (data[highIndex] - data[lowIndex]) * (i * coeff - lowIndex) : 0));              

                    }
                    else
                        newData[i] = 0;
                }
            }
            //拉伸
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    int lowIndex = (int)(i * coeff);
                    int highIndex = lowIndex + 1;
                    newData[i] = (int)(data[lowIndex] + ( highIndex < data.Length ? (data[highIndex] - data[lowIndex]) * (i * coeff - lowIndex) : 0 ));              
                }
            }

            return newData;
        }


        public int[] SpecDatas   //显示所用
        {
            get
            {
                if (SpecData == null)
                {
                    return Helper.ToInts("");
                }
                
                int[] intSpec = Helper.ToInts(SpecData);

                
               
                //加入特殊处理
                SpecProcess.SpectrumProcBySkraySpecial(SpecHelper.CURRENTWorkCurveTemp, ref intSpec, (float)this.UsedTime);
                if (IsSmooth)
                    intSpec = Helper.Smooth(intSpec, SpecHelper.SmoothTimes);
                
                if (this.DeviceParameter != null && DemarcateEnergyHelp.k1 > 0)//&& UsedTime >= this.DeviceParameter.PrecTime
                {
                    int[] returnInt = SpecProcess.SpectrumProc(SpecHelper.CURRENTWorkCurveTemp, intSpec,(float)this.UsedTime,1);
                    
                    return returnInt;
                }
                return intSpec;

            }
        }


        public int[] SpecDatac //计算所用
        {
            get
            {
                if (SpecData == null)
                {
                    return Helper.ToInts("");
                }
                int[] intSpec = Helper.ToInts(SpecData);
         
                
                        
                if (IsSmooth)
                    intSpec = Helper.Smooth(intSpec, SpecHelper.SmoothTimes);
                if (this.DeviceParameter != null && DemarcateEnergyHelp.k1 > 0)//&& UsedTime >= this.DeviceParameter.PrecTime
                {
                    int[] returnInt = SpecProcess.SpectrumProc(SpecHelper.CURRENTWorkCurveTemp, intSpec, (float)this.UsedTime, 1);
                    
                    return returnInt;
                }
                return intSpec;

            }
        }


        public DeviceParameterEntity DeviceParameter { set; get; }
      
        public SpecEntity() {
            DeviceParameter = new DeviceParameterEntity();
        }
        public SpecEntity(
            string Name,
            string SpecData,
            double SpecTime,
            double UsedTime,
            double TubVoltage,
            double TubCurrent, string RemarkInfo):this()
        {
            this.Name = Name;
            this.SpecData = SpecData;
            this.SpecTime = SpecTime;
            this.UsedTime = UsedTime;
            this.TubCurrent = TubCurrent;
            this.TubVoltage = TubVoltage;
            this.RemarkInfo = RemarkInfo;
        }
    }
}
