using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Model
{
    /// <summary>
    /// 税目信息
    /// </summary>
    public class TaxCategory
    {
        public long CategoryId { get; set; }
        public string TaxLabel { get; set; }
        public string TaxName { get; set; }
        public double TaxRate { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpiredDate { get; set; }
    }
}
