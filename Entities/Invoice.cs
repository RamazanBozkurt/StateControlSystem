using StateControlSystem.Enums;

namespace StateControlSystem.Entities
{
    public class Invoice : EntityBase
    {
        public string InvoiceNumber { get; set; }
        public string TaxNumber { get; set; }
        public ServiceResponseCode ResponseCode { get; set; }
        public string Message { get; set; }
    }
}