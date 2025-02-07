using Stripe;
using Server.Models;
using Stripe.Checkout;
using Server.Services;
using Server.Models.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.API.Payments
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        private readonly StripeModel       stripeModel;
        private readonly ProductService   _productService;
        private readonly ChargeService    _chargeService;
        private readonly CustomerService  _customerService;
        private readonly TokenService     _tokenService;
        private readonly IPayment_With_Strip_Service  _paymentService;

        public PaymentsController
        (
            IOptions<StripeModel>  Model,
            ProductService         productService,
            ChargeService          chargeService,
            CustomerService        customerService,
            TokenService           tokenService,
            IPayment_With_Strip_Service        paymentService
        )
        {
            stripeModel      = Model.Value;
            _productService  = productService;
            _chargeService   = chargeService;
            _customerService = customerService;
            _tokenService    = tokenService;
            _paymentService  = paymentService;

        }


        [HttpPost()]
        [Route("EmployeePayment")]
        public async Task<IActionResult> EmployeePayment([FromBody] EmployeePayment checkoutModel)
        {
            try
            {
                var res = await _paymentService.EmployeePayment(checkoutModel);
                return Ok(res);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        [HttpPost()]
        [Route("Checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutModel checkoutModel)
        {
            try
            {
                var res = await _paymentService.Checkout(checkoutModel);
                return Ok(res);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        [HttpGet()]
        [Route("GetPaymentPlans")]
        public async Task<IActionResult> GetPaymentPlans()
        {
            try
            {
                var plans   = await  _paymentService.GetPaymentPlans(); 
                return Ok(plans);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }


        [HttpGet()]
        [Route("GetInvoices")]
        public IActionResult GetInvoices(string customerId)
        {
            try
            {
                var plans =  _paymentService.GetInvoices(customerId);
                return Ok(plans);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        [HttpGet()]
        [Route("DownloadInvoice")]
        public IActionResult DownloadInvoice(string Invoice)
        {
            try
            {
                 var res=_paymentService.DownloadInvoice(Invoice);
                SuccessResponse successResponse = new SuccessResponse();
                successResponse.Message = res; ;
                return Ok(successResponse);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
              }
        }
             
    }
}
