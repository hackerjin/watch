using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Lephone.Data.Definition;
using Lephone.Util;
using Skyray.EDXRFLibrary;


namespace Skyray.EDX.Common
{
   

    public class WorkCurveProcess : MessageInterface
    {
        /// <summary>
        /// 填充工作曲线容器对象
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="orientType"></param>
        /// <param name="isFixed"></param>
        /// <param name="tempobj"></param>
        /// <param name="dataGridView"></param>
        public override void RecordElementValusInfo(bool flag,bool orientType, bool isFixed, BaseMessage tempobj, Skyray.Controls.DataGridViewW dataGridView,ElementList list)
        {
            StaticCommonInfo(orientType, isFixed, tempobj, dataGridView);
        }

        /// <summary>
        /// 构造工作曲线容器对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isFixed"></param>
        /// <param name="orientType"></param>
        /// <param name="datagridview"></param>
        public override void ContructDataContainer(BaseMessage obj, bool isFixed, bool orientType, Skyray.Controls.DataGridViewW datagridview)
        {
            ContructDataCommonInfo(obj, orientType, datagridview, isFixed);
        }
    }
}
