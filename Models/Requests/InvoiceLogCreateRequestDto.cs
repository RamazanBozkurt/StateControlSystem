using StateControlSystem.Enums;

namespace StateControlSystem.Models.Requests
{
    public class InvoiceLogCreateRequestDto
    {
        public string InvoiceNumber { get; set; }
        public string TaxNumber { get; set; }
        public ServiceResponseCode ResponseCode { get; set; }
        public string Message { get; set; }
        public DateTime RequestTime { get; set; }
        public Guid CorrelationId { get; set; }
    }
}