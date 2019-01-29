using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Model.Service.Statistics
{
    public class XReportModel
    {
        public string CurrentTime { get; set; }
        public double TotalSales { get; set; }
        public double TotalTax { get; set; }
        public double InvoiceQuantity { get; set; }
        public List<ReportTaxItems> TaxItems { get; set; }
    }
}
