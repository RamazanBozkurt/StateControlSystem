using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using StateControlSystem.Entities;
using StateControlSystem.Enums;
using StateControlSystem.Models.Requests;
using StateControlSystem.Models.Responses;
using StateControlSystem.Repositories.Abstract;
using StateControlSystem.Services.Abstract;

namespace StateControlSystem.Services.Concrete
{
    public class InvoiceService : IInvoiceService
    {
        private readonly List<Invoice> _invoices = new List<Invoice>
        {
            new Invoice
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = "FAT20251411001",
                TaxNumber = "11111",
                ResponseCode = Enums.ServiceResponseCode.APPROVED,
                Message = "Fatura onaylandı."
            },
            new Invoice
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = "FAT20251411002",
                TaxNumber = "22222",
                ResponseCode = Enums.ServiceResponseCode.REJECTED,
                Message = "Hatalı imza."
            },
        };

        private const int ABSOLUTE_CACHE_EXPIRATION_IN_SECONDS = 60;    // 1 min
        private const int INVOICE_LOG_BLOCKED_ATTEMPT_COUNT = 2;

        private readonly IInvoiceRepository _repository;
        private IMemoryCache _memoryCache;
        public InvoiceService(IInvoiceRepository repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        public async Task<ServiceResponse<InvoiceCheckResponseDto>> CheckAsync(InvoiceCheckRequestDto request)
        {
            // Get cache by request (InvoiceNumber, TaxNumber)
            // if isExist => return response
            // if is not exist => continue process.

            // Cache key: { taxNumber}-{ invoiceNumber}
            string cacheKey = $"{request.TaxNumber}-{request.InvoiceNumber}".ToLower();
            var cachedRequest = _memoryCache.Get<string>(cacheKey);

            if (!string.IsNullOrEmpty(cachedRequest))
            {
                var responseModel = new InvoiceCheckResponseDto(ServiceResponseCode.BLOCKED);

                Console.WriteLine("Cache Response Success");
                return ServiceResponse<InvoiceCheckResponseDto>.Success(responseModel);
            }

            ServiceResponseCode responseCode;
            string message = string.Empty;
            bool isBlocked = await IsBlockedAsync(request);

            if (isBlocked)
            {
                responseCode = ServiceResponseCode.BLOCKED;
                message = ServiceResponseMessage.BlockedMessage;

                // Set cache
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(ABSOLUTE_CACHE_EXPIRATION_IN_SECONDS);
                _memoryCache.Set<string>(cacheKey, cacheKey, options);
                Console.WriteLine("Cache created with key : " + cacheKey);
            }
            else
            {
                var entity = _invoices.FirstOrDefault(invoice => invoice.InvoiceNumber == request.InvoiceNumber && invoice.TaxNumber == request.TaxNumber);

                if (entity == null)
                {
                    responseCode = ServiceResponseCode.REJECTED;
                    message = ServiceResponseMessage.RejectedMessage;
                }
                else
                {
                    responseCode = ServiceResponseCode.APPROVED;
                    message = ServiceResponseMessage.ApprovedMessage;
                }
            }

            var logModel = new InvoiceLogCreateRequestDto
            {
                InvoiceNumber = request.InvoiceNumber,
                TaxNumber = request.TaxNumber,
                ResponseCode = responseCode,
                Message = message,
                RequestTime = DateTime.UtcNow,
                CorrelationId = Guid.NewGuid()
            };

            bool createLogResponse = await CreateLogAsync(logModel);

            if (!createLogResponse) return ServiceResponse<InvoiceCheckResponseDto>.Fail("Log cannot be created successfully!");

            string jsonResponse = JsonConvert.SerializeObject(logModel);

            Console.WriteLine(jsonResponse);

            var response = new InvoiceCheckResponseDto(responseCode);

            return ServiceResponse<InvoiceCheckResponseDto>.Success(response);
        }

        private async Task<bool> CreateLogAsync(InvoiceLogCreateRequestDto request)
        {
            // AutoMapper can be used in here.
            var entity = new InvoiceStatusLog
            {
                InvoiceNumber = request.InvoiceNumber,
                TaxNumber = request.TaxNumber,
                ResponseCode = request.ResponseCode,
                Message = request.Message,
                RequestTime = request.RequestTime,
                CorrelationId = request.CorrelationId
            };

            return await _repository.CreateAsync(entity);
        }

        private async Task<bool> IsBlockedAsync(InvoiceCheckRequestDto request)
        {
            var logControlCount = await _repository.GetLogControlCountAsync(request.InvoiceNumber, request.TaxNumber);

            if (logControlCount >= INVOICE_LOG_BLOCKED_ATTEMPT_COUNT) return true;

            return false;
        }
    }
}