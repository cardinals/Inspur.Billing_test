using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Model.Service.Sign
{
    public class SignGoodItem
    {
        /// <summary>
        /// Global Trade Item Number (GTIN) is an identifier for trade items, incorporated the ISBN, ISSN, ISMN, IAN (which includes the European Article Number and Japanese Article Number) and some Universal Product Codes, into a universal number space.
        /// </summary>
        [JsonProperty(PropertyName = "ItemId")]
        public int GTIN { get; set; }
        public string BarCode { get; set; }
        /// <summary>
        /// Human readable name of the product or service. Required Max Length 2048 character
        /// </summary>
        [JsonProperty(PropertyName = "Description")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Quantity")]
        public double Quantity { get; set; }
        [JsonProperty(PropertyName = "UnitPrice")]
        public double UnitPrice { get; set; }
        //public double Discount { get; set; }
        /// <summary>
        /// Array of labels. Each Label represents one of the Tax Rates applied on invoice item. Tax Items are calculated based on TotalAmount and applied Labels as described in Calculate Taxes section. 
        /// Required, Array of strings.In case no taxes are applicable online item this field is optional
        /// </summary>
        [JsonProperty(PropertyName = "TaxLabels")]
        public string[] Labels { get; set; }
        /// <summary>
        /// Gross price for the line item, including discount. 
        /// Required, Decimal(14,2)
        /// </summary>
        [JsonProperty(PropertyName = "TotalAmount")]
        public double TotalAmount { get; set; }
        [JsonProperty(PropertyName = "isTaxInclusive")]
        public bool IsTaxInclusive { get; set; }
        [JsonProperty(PropertyName = "RRP")]
        public double RRP { get; set; }
    }
}
