using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class Voucher : WalletPass
    {
        public string Voucher_Code { get; set; }
        public string Issuer { get; set; }
        public decimal? Amount { get; set; }
        public string Currency_Code { get; set; }
        public DateTime? Expiration_Date { get; set; }
        public string Terms_And_Conditions { get; set; }
    }

}
