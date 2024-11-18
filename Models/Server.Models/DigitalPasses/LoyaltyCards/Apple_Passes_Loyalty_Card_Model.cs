using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Apple_Passes_Loyalty_Card_Model
    {
        public string Team_Identifier { get; set; } // Required
        public string Serial_Number { get; set; } // Required
        public string Organization_Name { get; set; } // Required
        public string Logo_Text { get; set; } // Optional
        public string Background_Color { get; set; } // Optional
        public string Foreground_Color { get; set; } // Optional
        public string Label_Color { get; set; } // Optional
        public string Points { get; set; } // Loyalty points, primary field
        public string Membership_Level { get; set; } // Optional, e.g., "Gold", "Silver"
        public DateTime? Expiry_Date { get; set; } // Optional, expiry date
        public string Barcode { get; set; } // Required for QR/Code128 representation
        public string Logo_Url { get; set; } // Optional, URL for logo image
    }

}
