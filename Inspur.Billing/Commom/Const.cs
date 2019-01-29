using DataModels;
using Inspur.Billing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Inspur.Billing.Commom
{
    class Const
    {
        /// <summary>
        /// sqlite连接字符串
        /// </summary>
        public static string ConnectString = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "Billing.db";
        /// <summary>
        /// 数据库对象
        /// </summary>
        public static BillingDB dB = new BillingDB();
        private static ViewModelLocator _locator;
        /// <summary>
        /// 数据源字典
        /// </summary>
        public static ViewModelLocator Locator
        {
            get
            {
                if (_locator == null)
                {
                    _locator = (ViewModelLocator)Application.Current.Resources["Locator"];
                }
                return _locator;
            }
        }
        public static long CashierId;

        public static string BaseUri
        {
            get { return string.Format("http://{0}", Locator.ParameterSetting.SdcUrl); }
        }
        public static string GetStatusUri
        {
            get { return string.Format("{0}/api/Status/GetStatus", BaseUri); }
        }
        public static string VerifyPinUri
        {
            get { return string.Format("{0}/api/Status/VerifyPin", BaseUri); }
        }
        public static string AttentionUri
        {
            get { return string.Format("{0}/api/Status/Attention", BaseUri); }
        }
        public static string SignUri
        {
            get { return string.Format("{0}/api/Sign/SignInvoice", BaseUri); }
        }
        public static string GetLastSignedUri
        {
            get { return string.Format("{0}/api/Sign/GetSignedInvoice", BaseUri); }
        }

        public static List<SystemStatu> Statues { get; set; }

        public static string QrPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "qr.bmp";
            }
        }
        /// <summary>
        /// 是否获取过esdc状态
        /// </summary>
        public static bool IsHasGetStatus { get; set; }
        /// <summary>
        /// 是否需要提示信息
        /// </summary>
        public static bool IsNeedMessage { get; set; }

    }
}
