using Stripe;
using System.Net;
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
                var CustomerOptions = new CustomerCreateOptions
                {
                    Email       = checkoutModel.Email,
                    Name        = checkoutModel.Name,
                    Description = checkoutModel.CompanyName
                };
                var customer =  _customerService.Create(CustomerOptions);
                var option  = new SessionCreateOptions
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
                    SuccessUrl  = "https://pelicanhrm.com/success",
                    CancelUrl   = "https://pelicanhrm.com/",
                };
                option.Customer = customer.Id ;
                var service     = new SessionService();
                Session session = await service.CreateAsync(option);
                SuccessResponse successResponse= new SuccessResponse();
                successResponse.Message = session.Url;
                return successResponse;
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
                var customer =  _customerService.Create(CustomerOptions);
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



        public List<InvoiceSummary> GetInvoices(string CustomerEmail)
        {
            StripeConfiguration.ApiKey = stripeModel.SecreteKey;
            var customerService        = new CustomerService();
            var customers              = customerService.List
            (
              new CustomerListOptions
              {
                Email = CustomerEmail
              }
            );
            if (customers.Data.Count == 0)
            {
                return new List<InvoiceSummary>();
            }
            var customerId     = customers.Data.First().Id;
            var invoiceService = new Stripe.InvoiceService();
            var invoices       = invoiceService.List(new InvoiceListOptions
            {
                Customer = customerId
            });

            var invoiceSummaries = invoices.Data.Select
            (
              invoice => new InvoiceSummary
              {
                Id            = invoice.Id,
                InvoiceNumber = invoice.Number,
                CustomerEmail = invoice.CustomerEmail,
                Frequency     = invoice.BillingReason ?? "N/A",
                Created       = invoice.Created,
                Amount        = invoice.AmountDue / 100.0m 
              }
            )
            .ToList();

            return invoiceSummaries;
        }



        public string DownloadInvoice(string invoiceId)
        {
            StripeConfiguration.ApiKey = stripeModel.SecreteKey;
            try
            {
                var service              = new InvoiceService();
                var invoice              = service.Get(invoiceId);
                var invoicePdfUrl        = invoice.InvoicePdf;
                return invoicePdfUrl;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
