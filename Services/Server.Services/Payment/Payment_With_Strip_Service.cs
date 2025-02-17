
using Microsoft.AspNetCore.Identity;
using Stripe;
using Server.Models;
using Stripe.Checkout;
using Server.Models.Payments;
using Microsoft.Extensions.Options;
using Server.Configurations;
using Server.Domain;

namespace Server.Services
{
    public class Payment_With_Strip_Service : IPayment_With_Strip_Service
    {

        private readonly StripeModel         stripeModel;
        private readonly ProductService      _productService;
        private readonly ChargeService       _chargeService;
        private readonly CustomerService     _customerService;
        private readonly TokenService        _tokenService;
        private readonly SubscriptionService _subscriptionService;
        private readonly IGet_Tenant_Id_Service _get_Tenant_Id_Service;
        private readonly IAccount_Transaction_Service  _account_Transaction_Service;
        private readonly UserManager<ApplicationUser> _auth_Manager_Service;
        private readonly RoleManager<CustomRole>      _roleManager;
        private readonly IUser_Vault_Service    _vault_Service;
        public Payment_With_Strip_Service
        (
            IOptions<StripeModel> Model,
            ProductService        productService,
            ChargeService         chargeService,
            CustomerService       customerService,
            TokenService          tokenService,
            SubscriptionService   subscriptionService,
            IGet_Tenant_Id_Service get_Tenant_Id_Service,
            IAccount_Transaction_Service account_Transaction_Service,
            UserManager<ApplicationUser>  auth_Manager_Service,
            RoleManager<CustomRole>  roleManager,
            IUser_Vault_Service  vault_Service
        )
        {
            stripeModel          = Model.Value;
            _productService      = productService;
            _chargeService       = chargeService;
            _customerService     = customerService;
            _tokenService        = tokenService;
            _subscriptionService = subscriptionService;
            _get_Tenant_Id_Service = get_Tenant_Id_Service;
            _account_Transaction_Service = account_Transaction_Service;
            _auth_Manager_Service = auth_Manager_Service;
            _roleManager = roleManager;
            _vault_Service     = vault_Service;

        }
        
        public async Task<ResponseModel<string>> CreatePaymentIntent(PaymentRequest request)
        {
            ResponseModel<string> response = new ResponseModel<string>(){Status_Code = "200",Description = "OK",Response =""};
            try
            {
                StripeConfiguration.ApiKey = stripeModel.SecreteKey;
                var options            = new PaymentIntentCreateOptions
                {
                    Amount             = request.Amount* 100,
                    Currency           = "usd",
                    PaymentMethodTypes = new List<string> { "card" },
                    ReceiptEmail       = request.Email
                };
                var service          = new PaymentIntentService();
                var paymentIntent    = await service.CreateAsync(options);
                response.Response    = paymentIntent.ClientSecret;
                response.Description = paymentIntent.Id;
                return response;
            }
            catch (StripeException e)
            {
                response.Status_Code = "500";
                response.Description = e.Message+ "Inner Exception" + e.InnerException?.Message;
                return response; 
            }
        }
        
        public async Task<ResponseModel<string>> ConfirmPayment(ConfirmPaymentRequest request)
        {
            ResponseModel<string> response = new ResponseModel<string>(){Status_Code = "200",Description = "OK",Response =""};
            try
            {
                ApplicationUser user                 = await _auth_Manager_Service.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    response.Status_Code = "400";
                    response.Description = "User not  Found";
                    return response;
                }
                StripeConfiguration.ApiKey = stripeModel.SecreteKey;
                var service              = new PaymentIntentService();
                var paymentIntent        = await service.GetAsync(request.PaymentIntentId);
                
                if (paymentIntent.Status == "succeeded")
                {
                       var role = await _auth_Manager_Service.GetRolesAsync(user);
                       if (role == null || role.Count == 0 )
                       {
                           role.Add("Customer");
                       }
                       Vault  vault = new Vault()
                       {
                         UserId      = _get_Tenant_Id_Service.GetUserId(),
                         Email       = request.Email, 
                         VaultType   = role.FirstOrDefault(),
                         TenantId    = _get_Tenant_Id_Service.GetTenantId(),
                         Lastupdated = DateTime.UtcNow
                       };
                       //user vault
                       var resv=await _vault_Service.AddReturn(vault);
                       if (resv == null)
                       {
                           response.Status_Code = "400";
                           response.Description = "Vault not Added";
                           return response;
                       }

                       await _vault_Service.CompleteAync();
                       //saving account Transaction Detail
                       Account_Transaction account_Transaction = new Account_Transaction();
                       account_Transaction.Tenant_Id               = _get_Tenant_Id_Service.GetTenantId().ToString();
                       account_Transaction.Amount                  = paymentIntent.Amount / 100;
                       account_Transaction.Card_Id                 = 11;
                       account_Transaction.Card_Type               = paymentIntent.Id;
                       account_Transaction.Customer_First_Name     = user.FirstName + " " + user.LastName;
                       account_Transaction.Email                   = user.Email;
                       account_Transaction.DrCrFlag                = Account_Txn_Flag_GModel.Credit;
                       account_Transaction.Processor_Id            = _get_Tenant_Id_Service.GetUserId();
                       account_Transaction.Processor_Name          = _get_Tenant_Id_Service.GetUserName();
                       account_Transaction.Txn_Type                = Account_Transaction_Type_GModel.AddetoVault;
                       account_Transaction.RedemptionType          = Account_Transaction_Type_GModel.AddetoVault;
                       var res= await _account_Transaction_Service.AddReturn(account_Transaction);
                       if (res == null)
                       {
                           await _account_Transaction_Service.Rollback();
                       }
                       await _account_Transaction_Service.CompleteAync();
                       response.Status_Code = "200";
                       response.Description = "Payment Success";
                       return response;
                }
                else
                {
                    response.Status_Code = "500";
                    response.Description = paymentIntent.Status;
                    response.Response    = paymentIntent.CancellationReason;
                }
                return response;
            }
            catch (StripeException e)
            {
                response.Status_Code = "500";
                response.Description = e.Message+ "Inner Exception" + e.InnerException?.Message;
                return response; 
            }
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
