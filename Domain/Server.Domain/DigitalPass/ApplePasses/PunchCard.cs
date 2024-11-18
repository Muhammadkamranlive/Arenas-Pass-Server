using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class PunchCard : WalletPass
    {
        public string Punch_Title { get; set; }
        public int Total_Punches { get; set; }
        public int Current_Punches { get; set; }
        public string Reward_Details { get; set; }
        public DateTime? Expiration_Date { get; set; }
        public string Barcode_Type { get; set; }
        public string Barcode_Format { get; set; }
    }

}
