using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Apple_Passes_Coupon_Model:WalletPassModel
    {
        public string Offer_Code           { get; set; }
        public decimal Discount_Percentage { get; set; }
        public string Is_Redeemed          { get; set; }
    }

}
