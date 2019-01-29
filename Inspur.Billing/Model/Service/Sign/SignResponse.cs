using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Model.Service.Sign
{
    public class SignResponse
    {
        ///// <summary>
        ///// UID of Secure Element digital certificate. In case of EFD it will be the same value as SignedBy field.
        ///// </summary>
        //public string RequestedBy { get; set; }
        ///// <summary>
        ///// UID of Secure Element digital certificate. In case of EFD it will be the same value as RequestedBy field.
        ///// </summary>
        //public string SignedBy { get; set; }
        ///// <summary>
        ///// Local date and time in ISO 8601 format
        ///// </summary>
        //public DateTime DT { get; set; }
        ///// <summary>
        ///// TransactionTypeCounter/TotalCounter InvoiceCounterExtension
        ///// </summary>
        //public string IC { get; set; }
        ///// <summary>
        ///// First letters of Transaction Type and Invoice Type of the invoice. NS for Normal Sale, CR – Copy Refund, TS – Training Sale etc
        ///// </summary>
        //public string InvoiceCounterExtension { get; set; }
        ///// <summary>
        ///// RequestedBy-SignedBy-TotalCounter
        ///// </summary>
        //public string IN { get; set; }
        ///// <summary>
        ///// VerificationURL generated in Create Verification URL process
        ///// </summary>
        //public string VerificationUrl { get; set; }
        ///// <summary>
        ///// Base64 encoded byte array of GIF image created in Create QR Code process
        ///// </summary>
        //public string VerificationQRCode { get; set; }
        ///// <summary>
        ///// Textual Representation of the invoice created in Create Textual Representation of a Receipt process
        ///// </summary>
        //public string Journal { get; set; }
        ///// <summary>
        ///// Custom human readable message printed or displayed by POS
        ///// </summary>
        //public string Message { get; set; }
        ////public Dictionary<string,string> ModelState { get; set; }
        //private Dictionary<string, string[]> _modelState=new Dictionary<string, string[]>();

        //public Dictionary<string, string[]> ModelState
        //{
        //    get { return _modelState; }
        //    set { _modelState = value; }
        //}

        ///// <summary>
        ///// Total number of invoices signed by Secure Element. Returned by Sign Invoice APDU command
        ///// </summary>
        //public double TotalCounter { get; set; }
        ///// <summary>
        ///// Total number of invoices of a requested type. Returned by Sign Invoice APDU command
        ///// </summary>
        //public double TransactionTypeCounter { get; set; }
        ///// <summary>
        ///// Sum of all Items – total payable by customer
        ///// </summary>
        //public double TotalAmount { get; set; }
        ///// <summary>
        ///// Encrypted Internal Data - Base64 encoded byte array returned by Sign Invoice APDU command
        ///// </summary>
        //public string ID { get; set; }
        ///// <summary>
        ///// Signature - Base64 encoded byte array returned by Sign Invoice APDU command 
        ///// </summary>
        //public string S { get; set; }
        ///// <summary>
        ///// Array of TaxItem entities
        ///// </summary>
        //public List<TaxItem> TaxItems { get; set; }
        ///// <summary>
        ///// Tax Label (A, F, G)
        ///// </summary>
        //public string[] Label { get; set; }
        ///// <summary>
        ///// Tax Category Name (i.e. VAT, Consumption)
        ///// </summary>
        //public string Name { get; set; }
        ///// <summary>
        ///// Tax rate percentage for Label (i.e. 12.50%) 
        ///// </summary>
        //public string Rate { get; set; }
        ///// <summary>
        ///// Tax amount calculated by EFD during invoice fiscalization 
        ///// </summary>
        //public double Amount { get; set; }
        ///// <summary>
        ///// Hash received from POS in request field Hash
        ///// </summary>
        //public string Hash { get; set; }
        ///// <summary>
        ///// Taxpayer Business Name obtained from digital certificate subject field
        ///// </summary>
        //public string BusinessName { get; set; }
        ///// <summary>
        ///// Location Name obtained from digital certificate subject field
        ///// </summary>
        //public string LocationName { get; set; }
        ///// <summary>
        ///// Street address obtained from digital certificate subject field
        ///// </summary>
        //public string Address { get; set; }
        ///// <summary>
        ///// Tax Identification Number obtained from digital certificate subject field
        ///// </summary>
        //public string TIN { get; set; }
        ///// <summary>
        ///// District obtained from digital certificate subject field
        ///// </summary>
        //public string District { get; set; }
        ///// <summary>
        ///// Manufacturer registration code (in format MakeCode-SW-Serial), mandatory for audit package, optional for POS response. 
        ///// MakeCode: unique 2 characters obtained on FRCA Accreditation 
        ///// SW: software version 
        ///// Serial: manufacturer serial number(max 32 characters)
        ///// </summary>
        //public string MRC { get; set; }

        public string TPIN { get; set; }
        public string TaxpayerName { get; set; }
        public string Addreess { get; set; }
        public string ESDTime { get; set; }
        public string TerminalID { get; set; }
        public string InvoiceCode { get; set; }
        public string InvoiceNumber { get; set; }
        public string FiscalCode { get; set; }
        public string TalkTime { get; set; }
        public string Operator { get; set; }
        public List<TaxItem> TaxItems { get; set; }
        public string VerificationUrl { get; set; }
        public string VerificationQRCode { get; set; }
    }
}
