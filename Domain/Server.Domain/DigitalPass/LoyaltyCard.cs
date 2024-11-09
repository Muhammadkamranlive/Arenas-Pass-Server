using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class LoyaltyCard : WalletPass
    {
        public int PointsBalance     { get; set; }
        public int PointsEarned      { get; set; }
        public string MembershipTier { get; set; }
        public string AccountNumber  { get; set; }
        public string RewardDetails  { get; set; }
    }

}
