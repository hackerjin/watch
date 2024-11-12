using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common
{
    public interface IEDXRF
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        void SetAutoManualType(bool flag);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        void ShowResultType(bool flag);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        void SetConcluteMode(bool flag);
    }
}
