using System;
using Stripe;
using System.Linq;
using System.Text;
using Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public interface IPaymentService
    {
        Task<dynamic> Checkout(CheckoutModel checkoutModel);
        Task<dynamic> GetPaymentPlans();
        Task<dynamic> GetClientPaymentSession();
        List<InvoiceSummary> GetInvoices(string customerId);
        string DownloadInvoice(string invoiceId);
    }
}
