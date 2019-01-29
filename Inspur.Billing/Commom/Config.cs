using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Commom
{
    public class Config
    {
        static Config()
        {
            string qr = ConfigurationManager.AppSettings["QrMagnification"];
            double multiple;
            if (double.TryParse(qr, out multiple))
            {
                QrMagnification = multiple;
            }
            else
            {
                QrMagnification = 1;
            }
            PosSerialNumber = ConfigurationManager.AppSettings["PosSerialNumber"];
            PosVendor = ConfigurationManager.AppSettings["PosVendor"];
        }
        //二维码放大倍数
        public static double QrMagnification { get; set; }
        /// <summary>
        /// pos序列号
        /// </summary>
        public static string PosSerialNumber { get; set; }
        /// <summary>
        /// pos厂家信息
        /// </summary>
        public static string PosVendor { get; set; }
    }
}
