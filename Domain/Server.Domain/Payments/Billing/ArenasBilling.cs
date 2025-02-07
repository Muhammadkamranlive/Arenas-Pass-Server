using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class ArenasBilling
    {
        public int       Id                   { get; set; }
        public int       TenantId             { get; set; }
        public string    CompanyName          { get; set; }
        public string    CompanyEmail         { get; set; }
        public string    StripePaymentPlanId  { get; set; }
        public DateTime  SubscriptionStarted  { get; set; }
        public int       BonusDays            { get; set; }
        public DateTime  NextPaymentDate      { get; set; }
        public DateTime Created_At            { get; set; }
        public DateTime? Update_At            { get; set; }
        public ArenasBilling()
        {
            Created_At = DateTime.UtcNow;
        }
    }
}
