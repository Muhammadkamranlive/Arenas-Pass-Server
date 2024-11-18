using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class LoyaltyCard : WalletPass
    {
        public string Program_Name { get; set; }
        public int Points_Balance { get; set; }
        public int Points_Required { get; set; }
        public DateTime? Expiration_Date { get; set; }
        public string Reward_Details { get; set; }
    }


}
