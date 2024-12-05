using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Apple_Passes_Voucher_Model:WalletPassModel
    {

        public decimal Amount              { get; set; }
        public string Currency_Code        { get; set; }
        public string Currency_Sign        { get; set; }
        public string Offer                { get; set; }
        public string Is_Redeemed          { get; set; }
    }

}
