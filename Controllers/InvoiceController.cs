using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StateControlSystem.Models.Requests;
using StateControlSystem.Services.Abstract;

namespace StateControlSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("check")]
        public async Task<IActionResult> InvoiceCheckAsync(InvoiceCheckRequestDto request)
        {
            var response = await _invoiceService.CheckAsync(request);

            if (!response.IsSuccess) return BadRequest(response.ErrorMessage);

            return Ok(response.Data);
        }
    }
}