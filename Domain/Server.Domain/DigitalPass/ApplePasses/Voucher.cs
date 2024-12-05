using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class Voucher : WalletPass
    {
        public decimal Amount              { get; set; }
        public string Currency_Code        { get; set; }
        public string Currency_Sign        { get; set; }
        public string Offer                { get; set; }
    }

}
