using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Model.Service.Statistics
{
    public class ZReportModel : XReportModel
    {
        public string ReportNumber { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
    }
}
