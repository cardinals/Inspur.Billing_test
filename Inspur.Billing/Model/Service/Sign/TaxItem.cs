using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Model.Service.Sign
{
    public class TaxItem
    {
        public string TaxLabel{ get; set; }
        public string CategoryName{ get; set; }
        public double Rate { get; set; }
        public double TaxAmount{ get; set; }
    }
}
