using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Model.Service.Statistics
{
    public class ReportTaxItems
    {
        public string TaxLabel { get; set; }
        public string TaxName { get; set; }
        public double TaxRate { get; set; }
        public double TaxAmount { get; set; }
    }
}
