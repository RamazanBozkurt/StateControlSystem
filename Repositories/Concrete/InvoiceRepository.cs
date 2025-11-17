using Microsoft.EntityFrameworkCore;
using StateControlSystem.Entities;
using StateControlSystem.Enums;
using StateControlSystem.Repositories.Abstract;

namespace StateControlSystem.Repositories.Concrete
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DataContext _context;
        public InvoiceRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(InvoiceStatusLog invoiceStatusLog)
        {
            invoiceStatusLog.CreatedAt = DateTime.UtcNow;

            try
            {
                await _context.AddAsync(invoiceStatusLog);

                return Convert.ToBoolean(await _context.SaveChangesAsync());
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public async Task<int> GetLogControlCountAsync(string invoiceNumber, string taxNumber)
        {
            return await _context.InvoiceStatusLogs.CountAsync(x => x.InvoiceNumber == invoiceNumber && x.TaxNumber == taxNumber && x.ResponseCode == ServiceResponseCode.REJECTED);
        }
    }
}