using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Model
{
    public class MonitoringInfo
    {
        public int OfflineNum { get; set; }
        public double SingleAmount { get; set; }
        public int MonthlyInvoiceQuantity { get; set; }
        public double CreditNoteAmountMonthly { get; set; }
        public int InvoiceHoldingQuantity { get; set; }
        public int RemainThreshold { get; set; }
        public int MonthlyCreditNoteNum { get; set; }
    }
}
