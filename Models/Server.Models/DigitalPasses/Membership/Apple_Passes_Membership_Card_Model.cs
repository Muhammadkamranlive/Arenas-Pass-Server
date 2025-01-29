using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Apple_Passes_Membership_Card_Model:WalletPassModel
    {
        public string Member_Name          { get; set; }
        public string? Membership_Number   { get; set; }
        public string? Additional_Benefits { get; set; }
        public string Status               { get; set; } 
    }

}
