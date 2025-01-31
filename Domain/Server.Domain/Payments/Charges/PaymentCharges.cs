using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class PaymentCharges
    {
        public int Id                { get; set; }
        public string ChargeType     { get; set; }
        public decimal ChargePayment { get; set; }
        public int RedemptionId      { get; set; }
        public int CardId            { get; set; }
    }
}
