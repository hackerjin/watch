using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    public class BaseObject
    {
        /// <summary>
        /// Clone the object, and returning a reference to a cloned object.
        /// </summary>
        /// <returns>Reference to the new cloned
        /// object.</returns>

        public static object Clone(object coloneSource)
        {

            //First we create an instance of this specific type.



            object newObject = Activator.CreateInstance(coloneSource.GetType());



            //We get the array of fields for the new type instance.

            PropertyInfo[] fields = newObject.GetType().GetProperties();



            int i = 0;



            foreach (PropertyInfo fi in coloneSource.GetType().GetProperties())
            {

                //We query if the fiels support the ICloneable interface.



                Type ICloneType = fi.PropertyType.

                            GetInterface("ICloneable", true);



                if (ICloneType != null)
                {

                    //Getting the ICloneable interface from the object.



                    ICloneable IClone = (ICloneable)fi.GetValue(coloneSource, null);

                    //We use the clone method to set the new value to the field.



                    fields[i].SetValue(newObject, IClone.Clone(),null);

                }

                else
                {

                    // If the field doesn't support the ICloneable



                    // interface then just set it.



                    fields[i].SetValue(newObject, fi.GetValue(coloneSource,null),null);

                }



                //Now we check if the object support the



                //IEnumerable interface, so if it does



                //we need to enumerate all its items and check if



                //they support the ICloneable interface.



                Type IEnumerableType = fi.PropertyType.GetInterface

                                ("IEnumerable", true);

                if (IEnumerableType != null)
                {

                    //Get the IEnumerable interface from the field.



                    IEnumerable IEnum = (IEnumerable)fi.GetValue(coloneSource,null);



                    //This version support the IList and the



                    //IDictionary interfaces to iterate on collections.



                    Type IListType = fields[i].GetType().GetInterface

                                        ("IList", true);

                    //Type IDicType = fields[i].FieldType.GetInterface

                    //                    ("IDictionary", true);



                    int j = 0;

                    if (IListType != null)
                    {

                        //Getting the IList interface.



                        IList list = (IList)fields[i].GetValue(newObject,null);



                        foreach (object obj in IEnum)
                        {

                            //Checking to see if the current item



                            //support the ICloneable interface.



                            ICloneType = obj.GetType().

                                GetInterface("ICloneable", true);



                            if (ICloneType != null)
                            {

                                //If it does support the ICloneable interface,



                                //we use it to set the clone of



                                //the object in the list.



                                ICloneable clone = (ICloneable)obj;



                                list[j] = clone.Clone();

                            }



                            //NOTE: If the item in the list is not



                            //support the ICloneable interface then in the



                            //cloned list this item will be the same



                            //item as in the original list



                            //(as long as this type is a reference type).





                            j++;

                        }

                    }

                    //else if (IDicType != null)
                    //{

                    //    //Getting the dictionary interface.



                    //    IDictionary dic = (IDictionary)fields[i].

                    //                        GetValue(newObject);

                    //    j = 0;



                    //    foreach (DictionaryEntry de in IEnum)
                    //    {

                    //        //Checking to see if the item



                    //        //support the ICloneable interface.

                    //        ICloneType = de.Value.GetType().

                    //            GetInterface("ICloneable", true);



                    //        if (ICloneType != null)
                    //        {

                    //            ICloneable clone = (ICloneable)de.Value;



                    //            dic[de.Key] = clone.Clone();

                    //        }

                    //        j++;

                    //    }

                    //}

                }

                i++;

            }

            return newObject;
        }

        public static byte[] Serialize(object pObj)
        {
            if (pObj == null)
                return null;
            System.IO.MemoryStream _memory = new System.IO.MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(_memory, pObj);
            _memory.Position = 0;
            byte[] read = new byte[_memory.Length];
            _memory.Read(read, 0, read.Length);
            _memory.Close();
            return read;

            //JObject Jo = new JObject();
            //T t = (T)o;
            //ObjectInfo oi = ObjectInfo.GetInstance(typeof(T));
            //foreach (MemberHandler memberHandler in oi.SimpleFields)
            //{
            //    Jo.Add(new JProperty(memberHandler.Name, memberHandler.GetValue(t)));
            //}
            //foreach (MemberHandler memberHandler in oi.RelationFields)
            //{
            //    if (memberHandler.IsBelongsTo)
            //    {
            //        var os = (IBelongsTo)memberHandler.GetValue(t);
            //        Jo.Add(new JProperty(memberHandler.Name, os.ForeignKey));
            //    }
            //}
            //return Jo.ToString(Newtonsoft.Json.Formatting.None, new IsoDateTimeConverter());

        }
    }
}
