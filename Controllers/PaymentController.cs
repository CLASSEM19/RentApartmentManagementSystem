using Microsoft.AspNetCore.Mvc;
using ApartmentRentManagementSystem.Interfaces.Services;
using ApartmentRentManagementSystem.Dtos;
using Microsoft.AspNetCore.Hosting;
namespace ApartmentRentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("MakePayment")]
        public async Task<IActionResult> MakePayment(PaymentRequestModel model)
        {
           if (model == null)
           {
               return BadRequest();
           }
           var payment = await _paymentService.MakePayment(model);
           if (payment.status == false)
           {
               return BadRequest(payment);
           }
           return Ok(payment);
        }

        [HttpPut("VerifyPayment/{referrenceNumber}")]
        public async Task<IActionResult> VerifyPayment([FromRoute]string referrenceNumber)
        {
           if (referrenceNumber == null)
           {
               return BadRequest();
           }
           var payment = await _paymentService.VerifyPayment(referrenceNumber);
           if (payment.Status == false)
           {
               return BadRequest(payment);
           }
           return Ok(payment);
        }

        [HttpGet("GetAllPaymentsByCustomer/{customerId}")]
        public async Task<IActionResult> GetAllPaymentsByCustomer([FromRoute]int customerId)
        {
            var payment = await _paymentService.GetAllPaymentsByCustomer(customerId);
           if (payment.Status == false)
           {
               return BadRequest(payment);
           }
           return Ok(payment);
        }

        [HttpGet("GetAllApartmentPayments/{apartmentId}")]
        public async Task<IActionResult> GetAllApartmentPayments([FromRoute]int apartmentId)
        {
           var payment = await _paymentService.GetAllApartmentPayments(apartmentId);
           if (payment.Status == false)
           {
               return BadRequest(payment);
           }
           return Ok(payment);
        }
        [HttpGet("GetAllApartmentPaymentsByCustomer/{apartmentId}/{customerId}")]
        public async Task<IActionResult> GetAllApartmentPaymentsByCustomer([FromRoute]int apartmentId, [FromRoute]int customerId)
        {
           var payment = await _paymentService.GetAllApartmentPaymentsByCustomer(apartmentId, customerId);
           if (payment.Status == false)
           {
               return BadRequest(payment);
           }
           return Ok(payment);

        }
    }
}