using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class Voucher : WalletPass
    {
        public decimal Discount_Percentage { get; set; }
    }

}
