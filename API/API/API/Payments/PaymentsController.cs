using Stripe;
using Server.Models;
using Stripe.Checkout;
using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.API.Payments
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        private readonly StripeModel      stripeModel;
        private readonly ProductService   _productService;
        private readonly ChargeService    _chargeService;
        private readonly CustomerService  _customerService;
        private readonly TokenService     _tokenService;
        private readonly IPaymentService  _paymentService;
        public PaymentsController
        (
            IOptions<StripeModel>  Model,
            ProductService         productService,
            ChargeService          chargeService,
            CustomerService        customerService,
            TokenService           tokenService,
            IPaymentService        paymentService
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


        


    }
}
