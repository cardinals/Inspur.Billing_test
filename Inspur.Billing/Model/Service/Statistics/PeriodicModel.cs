using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Model.Service.Statistics
{
    public class PeriodicModel : XReportModel
    {
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
    }
}
