using Stripe;
using Server.Models;
using Stripe.Checkout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Server.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly StripeModel     stripeModel;
        private readonly ProductService  _productService;
        private readonly ChargeService   _chargeService;
        private readonly CustomerService _customerService;
        private readonly TokenService    _tokenService;


        public PaymentService
        (
            IOptions<StripeModel> Model,
            ProductService        productService,
            ChargeService         chargeService,
            CustomerService       customerService,
            TokenService          tokenService
        )
        {
            stripeModel        = Model.Value;
            _productService    = productService;
            _chargeService     = chargeService;
            _customerService   = customerService;
            _tokenService      = tokenService;

        }

        public async Task<dynamic> Checkout(CheckoutModel checkoutModel)
        {
            try
            {
                StripeConfiguration.ApiKey = stripeModel.SecreteKey;
                string Id  = await  CreatCustomer(checkoutModel.Name, checkoutModel.Email,checkoutModel.CompanyName);
                var option = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>
                   {
                     new SessionLineItemOptions
                     {
                         Price    = checkoutModel.PriceId,
                         Quantity = 1
                     }
                   },
                    Mode        = "subscription",
                    SuccessUrl  = "http://localhost:4200/success",
                    CancelUrl   = "http://localhost:4200/",
                };
                option.Customer = Id;
                var service     = new SessionService();
                Session session = await service.CreateAsync(option);

                return session.Url;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }


        private async Task<dynamic> CreatCustomer( string Name,string Email,string companyName)
        {
            try
            {
                StripeConfiguration.ApiKey = stripeModel.SecreteKey;
                var CustomerOptions = new CustomerCreateOptions
                {
                    Email       = Email,
                    Name        = Name,
                    Description = companyName
                };
                var customer = await _customerService.CreateAsync(CustomerOptions);
                return new { customer.Id};
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<dynamic> GetPaymentPlans()
        {
            try
            {
                StripeConfiguration.ApiKey = stripeModel.SecreteKey;
                var options  = new ProductListOptions { Expand = new List<string> { "data.default_price" } };
                var plans    = await _productService.ListAsync(options);
                return plans;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<dynamic> GetClientPaymentSession()
        {
            try
            {
                StripeConfiguration.ApiKey = stripeModel.SecreteKey;
                var paymentService  = new PaymentIntentService();
                var Customers       = await _customerService.ListAsync();
                var paymentIntent   = await paymentService.ListAsync();
                var PaymentsRecord  = paymentIntent.Join
                    (
                       Customers,
                       (a) => new { a.CustomerId },
                       (b) => new { CustomerId = b.Id },
                       (a, b) => new { a, b }
                    )
                    .Where(x=>x.a.Status== "succeeded")
                    .Select
                    (
                     intent => new PaymentSession
                       {
                         PaymentIntentId  =  intent.a.Id,
                         Amount           =  intent.a.Amount,
                         Currency         =  intent.a.Currency,
                         Created          =  intent.a.Created,
                         Status           =  intent.a.Status,
                         CustomerName     =  intent.b.Name,
                         CustomerEmail    =  intent.b.Email,
                         CompanyName      =  intent.b.Description 
                       }
                    )
                    .ToList();
                    return PaymentsRecord;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }
    }
}
