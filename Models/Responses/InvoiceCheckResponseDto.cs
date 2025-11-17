using StateControlSystem.Enums;

namespace StateControlSystem.Models.Responses
{
    public class InvoiceCheckResponseDto 
    {
        public InvoiceCheckResponseDto()
        {
        }

        public InvoiceCheckResponseDto(ServiceResponseCode status)
        {
            this.Status = status.ToString();

            if (status == ServiceResponseCode.APPROVED)
            {
                this.Message = ServiceResponseMessage.ApprovedMessage;
            }
            else if (status == ServiceResponseCode.REJECTED)
            {
                this.Message = ServiceResponseMessage.RejectedMessage;
            }
            else if (status == ServiceResponseCode.BLOCKED)
            {
                this.Message = ServiceResponseMessage.BlockedMessage;
            }
        }

        public string Status { get; set; }
        public string Message { get; set; }
    }
}