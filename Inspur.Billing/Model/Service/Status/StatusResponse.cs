using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.Model.Service.Status
{
    public class StatusResponse
    {
        ///// <summary>
        ///// If PIN is not entered, or if wrong PIN is entered in the previous attempt, this field shall be set to true; otherwise set to false 
        ///// </summary>
        //public bool IsPinRequired { get; set; }
        ///// <summary>
        ///// If Audit is required, this field shall be set to true. 
        ///// Audit is required if Total Amount of all invoices is 75% or more of Maximum Limit. Maximum Limit and Total Amount are obtained from the Secure element using Amount Status APDU command
        ///// </summary>
        //public bool AuditRequired { get; set; }
        ///// <summary>
        ///// Current Local Date and Time in ISO8601 format
        ///// </summary>
        //public string DT { get; set; }
        ///// <summary>
        ///// UID RequestedBy-UID Signed By-Ordinal Number 
        ///// (Example: Signedby SE ORG674J1-ORG674J1-98637,
        ///// Signed by EFD ORG674J1-G8O0PA43-887)
        ///// </summary>
        //public string LastInvoiceNumber { get; set; }
        ///// <summary>
        ///// Always return 1.0.0.0
        ///// </summary>
        //public string ProtocolVersion { get; set; }
        ///// <summary>
        ///// Version obtained from the Secure element using Get Status APDU command
        ///// </summary>
        //public string SecureElementVersion { get; set; }
        ///// <summary>
        ///// Manufacturer-specific hardware version, if applicable
        ///// </summary>
        //public string HardwareVersion { get; set; }
        ///// <summary>
        ///// Manufacturer-specific software version 
        ///// </summary>
        //public string SoftwareVersion { get; set; }
        ///// <summary>
        ///// Manufacturer specific serial number
        ///// </summary>
        //public string DeviceSerialNumber { get; set; }
        ///// <summary>
        ///// Manufacturer specific Make Name
        ///// </summary>
        //public string Make { get; set; }
        ///// <summary>
        ///// Manufacturer specific Model Name
        ///// </summary>
        //public string Model { get; set; }
        ///// <summary>
        ///// Manufacturer Specific Errors, Warnings and info messages 
        ///// </summary>
        //public string[] MSSC { get; set; }
        ///// <summary>
        ///// General Errors, Warnings and info messages defined in Status and Error Codes section
        ///// </summary>
        //public string[] GSC { get; set; }



        public string HardwareVersion { get; set; }
        public string SoftwareVersion { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacture { get; set; }
        public string Model { get; set; }
        public string TPIN { get; set; }
        public string TaxpayerName { get; set; }
        public string Addreess { get; set; }
        public string ESDTime { get; set; }
        public string TerminalID { get; set; }
        public bool isInitialized { get; set; }
        public bool isLocked { get; set; }
        public MonitoringInfo MonitoringInfo { get; set; }
        public List<TaxInfo> TaxInfo { get; set; }
    }
}
