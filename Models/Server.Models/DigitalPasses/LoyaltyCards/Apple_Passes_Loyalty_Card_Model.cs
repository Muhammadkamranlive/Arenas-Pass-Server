using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Apple_Passes_Loyalty_Card_Model:WalletPassModel
    {
        public string Program_Name       { get; set; }
        public int    Points_Balance     { get; set; }
        public string Reward_Details     { get; set; }
    }

}
