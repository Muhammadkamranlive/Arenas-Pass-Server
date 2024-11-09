using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class Coupon : WalletPass
    {
        public string OfferName               { get; set; }
        public string DiscountType            { get; set; }  
        public decimal DiscountValue          { get; set; }
        public string ValidFor                { get; set; }  
        public decimal? MinimumPurchaseAmount { get; set; }
        public string TermsAndConditionsURL   { get; set; }
    }

}
