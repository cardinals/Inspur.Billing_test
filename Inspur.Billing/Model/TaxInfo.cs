using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Model
{
    /// <summary>
    /// 税种信息
    /// </summary>
    public class TaxInfo
    {
        public string TaxTpye { get; set; }
        public List<TaxCategory> Category { get; set; }
    }
}
