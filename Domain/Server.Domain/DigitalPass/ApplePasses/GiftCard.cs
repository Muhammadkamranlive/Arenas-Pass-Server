using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class GiftCard : WalletPass
    {
        public string Currency_Code       { get; set; }
        public decimal Balance            { get; set; }
        public string Currency_Sign       { get; set; }
    }

}
