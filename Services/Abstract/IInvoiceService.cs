using StateControlSystem.Models.Requests;
using StateControlSystem.Models.Responses;

namespace StateControlSystem.Services.Abstract
{
    public interface IInvoiceService
    {
        Task<ServiceResponse<InvoiceCheckResponseDto>> CheckAsync(InvoiceCheckRequestDto request);
    }
}