using System;
using System.Collections.Generic;

namespace Inspur.Billing.Model.Service.Sign
{
    public class SignRequest
    {
        public int OperationMode { get; set; }
        public string POSVendor { get; set; }
        public string PosSerialNumber { get; set; }
        public string IssueTime { get; set; }
        public int TransactionType { get; set; }
        public int PaymentMode { get; set; }
        public int SaleType { get; set; }
        public string LocalPurchaseOrder { get; set; }
        public string Cashier { get; set; }
        public string BuyerTPIN { get; set; }
        public string BuyerName { get; set; }
        public string BuyerTaxAccountName { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerTel { get; set; }
        public string OriginalInvoiceCode { get; set; }
        public string OriginalInvoiceNumber { get; set; }
        public List<SignGoodItem> Items { get; set; }
    }
}
