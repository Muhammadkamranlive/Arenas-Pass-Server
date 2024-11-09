using Stripe;
using Server.Models;
using Stripe.Checkout;
using Server.Models.Payments;
using Microsoft.Extensions.Options;

namespace Server.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly StripeModel         stripeModel;
        private readonly ProductService      _productService;
        private readonly ChargeService       _chargeService;
        private readonly CustomerService     _customerService;
        private readonly TokenService        _tokenService;
        private readonly SubscriptionService _subscriptionService;

        public PaymentService
        (
            IOptions<StripeModel> Model,
            ProductService        productService,
            ChargeService         chargeService,
            CustomerService       customerService,
            TokenService          tokenService,
            SubscriptionService   subscriptionService
        )
        {
            stripeModel          = Model.Value;
            _productService      = productService;
            _chargeService       = chargeService;
            _customerService     = customerService;
            _tokenService        = tokenService;
            _subscriptionService = subscriptionService;

        }


        //Checkout for Clients
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

        /// <summary>
        /// //Create Customer in Stripe
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Email"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// GetPayment Plans
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Client Payment Sessions
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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


        /// <summary>
        /// //Get Invocies Details by Customer Email
        /// </summary>
        /// <param name="CustomerEmail"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Downalod Invoice Pdf
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Charge Employee Payments
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<dynamic> EmployeePayment(EmployeePayment request)
        {
                StripeConfiguration.ApiKey = stripeModel.SecreteKey;
                // Check if customer already exists
                var existingCustomers     = _customerService.List(new CustomerListOptions
                {                         
                    Email                 = request.EmployeeEmail
                });

                Customer customer;

                if (existingCustomers.Any())
                {
                    customer = existingCustomers.First();
                }
                else
                {
                       var CustomerOptions        = new CustomerCreateOptions
                       {
                         Email    = request.EmployeeEmail,
                         Name     = request.EmployeeName,
                         Address  = new AddressOptions
                         {
                             Line1      = request.EmployeeAddress,
                             PostalCode = request.EmployeeZipcode,
                             City       = request.EmployeeCity,
                             Country    = request.EmployeeCountry,
                            
                         }
                       };
                       customer  =  _customerService.Create(CustomerOptions);
                }
                var subscriptionOptions = new SubscriptionCreateOptions
                {
                    Customer = customer.Id,
                    Items    = new List<SubscriptionItemOptions>
                    {
                      new SubscriptionItemOptions { Price = request.PriceId }
                    },
                    PaymentBehavior             = "default_incomplete",
                    Expand                      = new List<string> { "latest_invoice.payment_intent" },
                    
                };
                var subscriptionService         = new SubscriptionService();
                var subscription                = await subscriptionService.CreateAsync(subscriptionOptions);
                var clientSecret                = subscription.LatestInvoice.PaymentIntent.ClientSecret;
               
                return clientSecret;
        }



        /// <summary>
        /// Check Employee Subscription
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<string> SubscriptionCheck(string email)
        {
            string retValue            = "OK";
            StripeConfiguration.ApiKey = stripeModel.SecreteKey;
            var options = new CustomerListOptions
            {
                Email   = email,
                Limit   = 1
            };
            var Customers = await _customerService.ListAsync(options);
            var customer = Customers.FirstOrDefault();
            if (customer != null)
            {
                var customerId = customer.Id;
                var optionsSub = new SubscriptionListOptions
                {
                    Customer = customerId,
                    Status   = "active",
                    Limit    = 1
                };

                var subscriptions = await _subscriptionService.ListAsync(optionsSub);
                if (subscriptions.Any())
                {
                    retValue = "OK";
                    return retValue;
                }
            }

            retValue = "Customer Not have subcription";
            return retValue;
        }

        
    
    }
}
