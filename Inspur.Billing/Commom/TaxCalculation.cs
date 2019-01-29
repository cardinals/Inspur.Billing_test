using Inspur.TaxModel.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Commom
{
    public class TaxCalculation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode">计税方式 1 固定税额，0-税率</param>
        /// <param name="price">单价</param>
        /// <param name="count">数量</param>
        /// <param name="rate">税率</param>
        /// <param name="fixTaxAmount">固定税额</param>
        /// <returns></returns>
        public static double Calculation(string isTaxInclusive, string mode, double price, double count, double rate, double fixTaxAmount)
        {
            double result = 0;
            if (mode == ((int)CalculationMode.Fixed).ToString())
            {
                //固定税额
                result = count * fixTaxAmount;
            }
            else
            {
                //税率
                if (isTaxInclusive == ((int)ProductTaxInclusive.Not).ToString())
                {
                    //不含税
                    result = price * count * rate / 100;
                }
                else
                {
                    //含税
                    result = price * count / (1 + rate / 100) * rate / 100;
                }
            }

            return result;
        }
    }
}
