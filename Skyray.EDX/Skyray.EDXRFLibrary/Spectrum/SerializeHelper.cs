using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Skyray.EDXRFLibrary.Spectrum
{
    public class SerializeHelper
    {
        public static byte[] SerializeObj(SpecListEntity model)
        {
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.Stream stream = new System.IO.MemoryStream();
            formatter.Serialize(stream, model);
            stream.Flush();
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, Convert.ToInt32(stream.Length));
            stream.Close();
            return bytes;
        }

        public static SpecListEntity DeSerializeObj(byte[] data)
        {
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.Stream stream = new System.IO.MemoryStream(data);
            SpecListEntity obj = (SpecListEntity)formatter.Deserialize(stream);
            stream.Close();
            return obj;
        }

        public static void ObjToFile(object objToSerialization, string strFileName)
        {
            Stream serializationStream = File.Open(strFileName, FileMode.OpenOrCreate);
            new BinaryFormatter().Serialize(serializationStream, objToSerialization);
            serializationStream.Close();
        }

        public static object FileToObj(string strFileName)
        {
            try
            {
                Stream serializationStream = File.Open(strFileName, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                object obj = formatter.Deserialize(serializationStream);
                serializationStream.Close();
                return obj;
            }
            catch
            {
                return null;
            }
        }

       
    }

    public class SqlParams
    {
        public string Label { get; set; }

        public string Value { get; set; }

        public bool Islike { get; set; }

        public string PreSuffix { get; set; }

        public string AfterSuffix { get; set; }

        public bool IsValueType { get; set; }

        public SqlParams(string label, string value,bool isValueType)
        {
            this.Label = label;
            this.Value = value;
            this.IsValueType = isValueType;
        }

        public SqlParams(string label, string value,bool isLike,bool isValueType)
            :this(label,value,isValueType)
        {
            this.Islike = isLike;
        }

        public SqlParams(string label, string value, bool isLike, string preSuffix, string afterSuffix,bool isValueType)
            :this(label,value,isLike,isValueType)
        {
            this.PreSuffix = preSuffix;
            this.AfterSuffix = afterSuffix;
        }
    }
}
