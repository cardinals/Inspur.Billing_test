using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.TaxModel.Enum
{
    /// <summary>
    /// 计税方式
    /// </summary>
    public enum CalculationMode
    {
        /// <summary>
        /// 税率
        /// </summary>
        Rate = 0,
        /// <summary>
        /// 固定税额
        /// </summary>
        Fixed = 1
    }
}
