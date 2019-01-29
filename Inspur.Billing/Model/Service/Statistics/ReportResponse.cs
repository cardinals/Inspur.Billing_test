using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Model.Service.Statistics
{
    public class ReportResponse
    {
        public int ReportType { get; set; }
        public XReportModel X { get; set; }
        public ZReportModel Z { get; set; }
        public PeriodicModel Periodic { get; set; }
    }
}
