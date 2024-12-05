using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class LoyaltyCard : WalletPass
    {
        public string Program_Name       { get; set; }
        public int    Points_Balance     { get; set; }
        public string Reward_Details     { get; set; }
    }


}
