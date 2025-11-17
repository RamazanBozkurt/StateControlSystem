namespace StateControlSystem.Entities
{
    public class InvoiceStatusLog : Invoice
    {
        public Guid CorrelationId { get; set; }
        public DateTime RequestTime { get; set; }
    }
}