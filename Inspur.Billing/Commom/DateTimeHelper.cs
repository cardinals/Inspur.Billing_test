using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Commom
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 时间格式转换
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="format">源格式</param>
        /// <param name="targetFormat">转换之后的格式</param>
        /// <returns></returns>
        public static string Converter(string source, string format, string targetFormat)
        {
            DateTime dt;
            if (DateTime.TryParseExact(source, format, new CultureInfo("zh-CN", true), DateTimeStyles.None, out dt))
            {
                return dt.ToString(targetFormat);
            }
            return source;
        }
    }
}
