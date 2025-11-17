using StateControlSystem.Entities;

namespace StateControlSystem.Repositories.Abstract
{
    public interface IInvoiceRepository
    {
        Task<bool> CreateAsync(InvoiceStatusLog invoiceStatusLog);
        Task<int> GetLogControlCountAsync(string invoiceNumber, string taxNumber);
    }
}