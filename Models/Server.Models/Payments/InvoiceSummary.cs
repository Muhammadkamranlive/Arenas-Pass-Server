using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class InvoiceSummary
    {
        public string Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerEmail { get; set; }
        public string Frequency { get; set; }
        public DateTime Created { get; set; }
        public decimal Amount { get; set; }
    }
}
