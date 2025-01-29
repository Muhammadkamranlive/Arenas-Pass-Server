using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class MembershipCard : WalletPass
    {
        public string Member_Name          { get; set; }
        public string? Membership_Number { get; set; } = "N/A";
        public string? Additional_Benefits { get; set; } = "N/A";
        public string Status              { get; set; } 
    }

}
