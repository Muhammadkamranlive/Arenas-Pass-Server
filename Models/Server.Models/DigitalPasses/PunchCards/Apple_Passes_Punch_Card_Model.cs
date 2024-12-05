using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Apple_Passes_Punch_Card_Model:WalletPassModel
    {
        public int Total_Punches         { get; set; }
        public int Current_Punches       { get; set; }
        public string Reward_Details     { get; set; }
    }

}
